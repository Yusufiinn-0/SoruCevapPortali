using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class AnswerController : Controller
    {
        private readonly IRepository<Answer> _answerRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AnswerController(
            IRepository<Answer> answerRepository,
            IHubContext<NotificationHub> hubContext)
        {
            _answerRepository = answerRepository;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var answers = await _answerRepository.GetQueryable()
                .Include(c => c.User)
                .Include(c => c.Question)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new AnswerViewModel
                {
                    AnswerId = c.AnswerId,
                    Content = c.Content.Length > 150 ? c.Content.Substring(0, 150) + "..." : c.Content,
                    QuestionId = c.QuestionId,
                    QuestionTitle = c.Question!.Title,
                    UserName = c.User!.FirstName + " " + c.User.LastName,
                    CreatedDate = c.CreatedDate,
                    IsActive = c.IsActive,
                    IsApproved = c.IsApproved,
                    IsCorrectAnswer = c.IsCorrectAnswer,
                    LikeCount = c.LikeCount
                })
                .ToListAsync();

            return View(answers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var answer = await _answerRepository.GetQueryable()
                .Include(c => c.User)
                .Include(c => c.Question)
                .FirstOrDefaultAsync(c => c.AnswerId == id);

            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var answer = await _answerRepository.GetQueryable()
                .Include(c => c.Question)
                .FirstOrDefaultAsync(c => c.AnswerId == id);

            if (answer == null)
            {
                return NotFound();
            }

            var model = new AnswerViewModel
            {
                AnswerId = answer.AnswerId,
                Content = answer.Content,
                QuestionId = answer.QuestionId,
                QuestionTitle = answer.Question?.Title,
                IsActive = answer.IsActive,
                IsApproved = answer.IsApproved,
                IsCorrectAnswer = answer.IsCorrectAnswer
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnswerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var answer = await _answerRepository.GetByIdAsync(model.AnswerId);
                if (answer == null)
                {
                    return NotFound();
                }

                answer.Content = model.Content;
                answer.IsActive = model.IsActive;
                answer.IsApproved = model.IsApproved;
                answer.IsCorrectAnswer = model.IsCorrectAnswer;
                answer.UpdatedDate = DateTime.Now;

                _answerRepository.Update(answer);
                await _answerRepository.SaveAsync();

                TempData["Success"] = "Answer updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _answerRepository.Delete(answer);
            await _answerRepository.SaveAsync();

            TempData["Success"] = "Answer deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Approve Answer
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var answer = await _answerRepository.GetQueryable()
                .Include(c => c.Question)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.AnswerId == id);
            
            if (answer == null)
            {
                return Json(new { success = false, message = "Answer not found." });
            }

            answer.IsApproved = true;
            _answerRepository.Update(answer);
            await _answerRepository.SaveAsync();

            // Send SignalR notification
            await _hubContext.Clients.Group("Admins").SendAsync("ReceiveNotification", 
                $"New answer approved: {answer.Question?.Title}", "success");

            return Json(new { success = true, message = "Answer approved." });
        }

        // AJAX - Mark Correct Answer
        [HttpPost]
        public async Task<IActionResult> MarkCorrect(int id)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return Json(new { success = false, message = "Answer not found." });
            }

            answer.IsCorrectAnswer = !answer.IsCorrectAnswer;
            _answerRepository.Update(answer);
            await _answerRepository.SaveAsync();

            return Json(new { success = true, isCorrect = answer.IsCorrectAnswer, message = answer.IsCorrectAnswer ? "Marked as correct answer." : "Correct answer mark removed." });
        }

        // AJAX - Toggle Answer Status
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return Json(new { success = false, message = "Answer not found." });
            }

            answer.IsActive = !answer.IsActive;
            _answerRepository.Update(answer);
            await _answerRepository.SaveAsync();

            return Json(new { success = true, isActive = answer.IsActive, message = answer.IsActive ? "Answer activated." : "Answer deactivated." });
        }
    }
}


