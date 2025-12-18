using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Hubs;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public QuestionController(
            IRepository<Question> questionRepository,
            IRepository<Category> categoryRepository,
            IRepository<Answer> answerRepository,
            UserManager<ApplicationUser> userManager,
            AppDbContext context,
            IHubContext<NotificationHub> hubContext)
        {
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _answerRepository = answerRepository;
            _userManager = userManager;
            _context = context;
            _hubContext = hubContext;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? categoryId, string? search)
        {
            var query = _questionRepository.GetQueryable()
                .Include(s => s.Category)
                .Include(s => s.User)
                .Include(s => s.Answers)
                .Where(s => s.IsActive && s.IsApproved);

            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Title.Contains(search) || s.Content.Contains(search));
            }

            var questions = await query
                .OrderByDescending(s => s.CreatedDate)
                .Select(s => new QuestionViewModel
                {
                    QuestionId = s.QuestionId,
                    Title = s.Title,
                    Content = s.Content.Length > 200 ? s.Content.Substring(0, 200) + "..." : s.Content,
                    CategoryName = s.Category!.CategoryName,
                    UserName = s.User!.FirstName + " " + s.User.LastName,
                    CreatedDate = s.CreatedDate,
                    ViewCount = s.ViewCount,
                    AnswerCount = s.Answers != null ? s.Answers.Count(c => c.IsActive && c.IsApproved) : 0
                })
                .ToListAsync();

            ViewBag.Categories = await _categoryRepository.GetQueryable()
                .Where(k => k.IsActive)
                .OrderBy(k => k.OrderNumber)
                .ToListAsync();

            ViewBag.CategoryId = categoryId;
            ViewBag.Search = search;

            return View(questions);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionRepository.GetQueryable()
                .Include(s => s.Category)
                .Include(s => s.User)
                .Include(s => s.Answers!)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.QuestionId == id && s.IsActive && s.IsApproved);

            if (question == null)
            {
                return NotFound();
            }

            // Increment view count
            question.ViewCount++;
            _questionRepository.Update(question);
            await _questionRepository.SaveAsync();

            var model = new QuestionViewModel
            {
                QuestionId = question.QuestionId,
                Title = question.Title,
                Content = question.Content,
                CategoryName = question.Category!.CategoryName,
                UserName = question.User!.FirstName + " " + question.User.LastName,
                CreatedDate = question.CreatedDate,
                ViewCount = question.ViewCount,
                Answers = question.Answers!
                    .Where(c => c.IsActive && c.IsApproved)
                    .OrderByDescending(c => c.IsCorrectAnswer)
                    .ThenByDescending(c => c.LikeCount)
                    .ThenByDescending(c => c.CreatedDate)
                    .Select(c => new AnswerViewModel
                    {
                        AnswerId = c.AnswerId,
                        Content = c.Content,
                        UserName = c.User!.FirstName + " " + c.User.LastName,
                        CreatedDate = c.CreatedDate,
                        LikeCount = c.LikeCount,
                        IsCorrectAnswer = c.IsCorrectAnswer
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryRepository.GetQueryable()
                .Where(k => k.IsActive)
                .OrderBy(k => k.OrderNumber)
                .ToListAsync();

            return View(new QuestionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var question = new Question
                {
                    Title = model.Title,
                    Content = model.Content,
                    CategoryId = model.CategoryId,
                    UserId = user.Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsApproved = false // Waiting for admin approval
                };

                await _questionRepository.AddAsync(question);
                await _questionRepository.SaveAsync();

                // Send SignalR notification
                await _hubContext.Clients.Group("Admins").SendAsync("ReceiveNotification", 
                    $"New question pending: {question.Title}", "info");

                TempData["Success"] = "Question submitted successfully. It will be published after admin approval.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryRepository.GetQueryable()
                .Where(k => k.IsActive)
                .OrderBy(k => k.OrderNumber)
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int id)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return Json(new { success = false, message = "Answer not found." });
            }

            answer.LikeCount++;
            _answerRepository.Update(answer);
            await _answerRepository.SaveAsync();

            return Json(new { success = true, likeCount = answer.LikeCount });
        }
    }
}


