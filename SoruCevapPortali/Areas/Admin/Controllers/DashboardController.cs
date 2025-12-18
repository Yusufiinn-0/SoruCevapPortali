using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly AppDbContext _context;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository,
            IRepository<Category> categoryRepository,
            AppDbContext context)
        {
            _userManager = userManager;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalUser = _context.Users.Count(),
                TotalQuestion = await _questionRepository.CountAsync(),
                TotalAnswer = await _answerRepository.CountAsync(),
                TotalCategory = await _categoryRepository.CountAsync(),
                PendingQuestions = await _questionRepository.CountAsync(s => !s.IsApproved),
                PendingAnswers = await _answerRepository.CountAsync(c => !c.IsApproved)
            };

            // Recent 5 questions
            var recentQuestions = await _questionRepository.GetQueryable()
                .Include(s => s.User)
                .Include(s => s.Category)
                .OrderByDescending(s => s.CreatedDate)
                .Take(5)
                .Select(s => new QuestionViewModel
                {
                    QuestionId = s.QuestionId,
                    Title = s.Title,
                    CategoryName = s.Category!.CategoryName,
                    UserName = s.User!.FirstName + " " + s.User.LastName,
                    CreatedDate = s.CreatedDate,
                    IsApproved = s.IsApproved
                })
                .ToListAsync();

            model.RecentQuestions = recentQuestions;

            // Recent 5 answers
            var recentAnswers = await _answerRepository.GetQueryable()
                .Include(c => c.User)
                .Include(c => c.Question)
                .OrderByDescending(c => c.CreatedDate)
                .Take(5)
                .Select(c => new AnswerViewModel
                {
                    AnswerId = c.AnswerId,
                    Content = c.Content.Length > 100 ? c.Content.Substring(0, 100) + "..." : c.Content,
                    QuestionTitle = c.Question!.Title,
                    UserName = c.User!.FirstName + " " + c.User.LastName,
                    CreatedDate = c.CreatedDate,
                    IsApproved = c.IsApproved
                })
                .ToListAsync();

            model.RecentAnswers = recentAnswers;

            return View(model);
        }
    }
}


