using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly AppDbContext _context;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository,
            AppDbContext context)
        {
            _userManager = userManager;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var questions = await _questionRepository.GetQueryable()
                    .Include(s => s.Category)
                    .Include(s => s.Answers)
                    .Where(s => s.UserId == user.Id)
                    .OrderByDescending(s => s.CreatedDate)
                    .Select(s => new
                    {
                        s.QuestionId,
                        s.Title,
                        s.CreatedDate,
                        s.IsApproved,
                        s.IsActive,
                        CategoryName = s.Category != null ? s.Category.CategoryName : "Kategori Yok",
                        AnswerCount = s.Answers != null ? s.Answers.Count(c => c.IsActive && c.IsApproved) : 0
                    })
                    .ToListAsync();

                var answers = await _answerRepository.GetQueryable()
                    .Include(c => c.Question)
                    .Where(c => c.UserId == user.Id)
                    .OrderByDescending(c => c.CreatedDate)
                    .Select(c => new
                    {
                        c.AnswerId,
                        Content = c.Content ?? "",
                        c.CreatedDate,
                        c.IsApproved,
                        c.IsActive,
                        c.IsCorrectAnswer,
                        QuestionTitle = c.Question != null ? c.Question.Title : "Soru Bulunamadı",
                        QuestionId = c.QuestionId
                    })
                    .ToListAsync();

                ViewBag.Questions = questions;
                ViewBag.Answers = answers;
                ViewBag.TotalQuestion = questions?.Count ?? 0;
                ViewBag.TotalAnswer = answers?.Count ?? 0;
                ViewBag.ApprovedQuestion = questions?.Count(s => s.IsApproved) ?? 0;
                ViewBag.ApprovedAnswer = answers?.Count(c => c.IsApproved) ?? 0;
            }
            catch (Exception)
            {
                // Hata durumunda boş listeler gönder
                ViewBag.Questions = new List<object>();
                ViewBag.Answers = new List<object>();
                ViewBag.TotalQuestion = 0;
                ViewBag.TotalAnswer = 0;
                ViewBag.ApprovedQuestion = 0;
                ViewBag.ApprovedAnswer = 0;
                
                TempData["Error"] = "Profil bilgileri yüklenirken bir hata oluştu.";
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.ProfilePicture = model.ProfilePicture;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(user);
        }
    }
}


