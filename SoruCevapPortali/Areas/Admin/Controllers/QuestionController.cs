using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Hubs;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuestionController : Controller
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public QuestionController(
            IRepository<Question> questionRepository,
            IRepository<Category> categoryRepository,
            IRepository<Answer> answerRepository,
            IHubContext<NotificationHub> hubContext)
        {
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _answerRepository = answerRepository;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _questionRepository.GetQueryable()
                .Include(s => s.User)
                .Include(s => s.Category)
                .Include(s => s.Answers)
                .OrderByDescending(s => s.CreatedDate)
                .Select(s => new QuestionViewModel
                {
                    QuestionId = s.QuestionId,
                    Title = s.Title,
                    CategoryName = s.Category!.CategoryName,
                    UserName = s.User!.FirstName + " " + s.User.LastName,
                    CreatedDate = s.CreatedDate,
                    ViewCount = s.ViewCount,
                    IsActive = s.IsActive,
                    IsApproved = s.IsApproved,
                    AnswerCount = s.Answers != null ? s.Answers.Count : 0
                })
                .ToListAsync();

            return View(questions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionRepository.GetQueryable()
                .Include(s => s.User)
                .Include(s => s.Category)
                .Include(s => s.Answers)!
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.QuestionId == id);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            var model = new QuestionViewModel
            {
                QuestionId = question.QuestionId,
                Title = question.Title,
                Content = question.Content,
                CategoryId = question.CategoryId,
                IsActive = question.IsActive,
                IsApproved = question.IsApproved
            };

            await LoadCategories(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = await _questionRepository.GetByIdAsync(model.QuestionId);
                if (question == null)
                {
                    return NotFound();
                }

                question.Title = model.Title;
                question.Content = model.Content;
                question.CategoryId = model.CategoryId;
                question.IsActive = model.IsActive;
                question.IsApproved = model.IsApproved;
                question.UpdatedDate = DateTime.Now;

                _questionRepository.Update(question);
                await _questionRepository.SaveAsync();

                TempData["Success"] = "Question updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            await LoadCategories(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            // Delete related answers
            var answers = await _answerRepository.GetAllAsync(c => c.QuestionId == id);
            foreach (var answer in answers)
            {
                _answerRepository.Delete(answer);
            }

            _questionRepository.Delete(question);
            await _questionRepository.SaveAsync();

            TempData["Success"] = "Question and related answers deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Approve Question
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var question = await _questionRepository.GetQueryable()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.QuestionId == id);
            
            if (question == null)
            {
                return Json(new { success = false, message = "Question not found." });
            }

            question.IsApproved = true;
            _questionRepository.Update(question);
            await _questionRepository.SaveAsync();

            // Send SignalR notification
            await _hubContext.Clients.Group("Admins").SendAsync("ReceiveNotification", 
                $"New question approved: {question.Title}", "success");

            return Json(new { success = true, message = "Question approved." });
        }

        // AJAX - Toggle Question Status
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return Json(new { success = false, message = "Question not found." });
            }

            question.IsActive = !question.IsActive;
            _questionRepository.Update(question);
            await _questionRepository.SaveAsync();

            return Json(new { success = true, isActive = question.IsActive, message = question.IsActive ? "Question activated." : "Question deactivated." });
        }

        private async Task LoadCategories(QuestionViewModel model)
        {
            var categories = await _categoryRepository.GetAllAsync(k => k.IsActive);
            model.Categories = new SelectList(categories, "CategoryId", "CategoryName", model.CategoryId);
        }
    }
}


