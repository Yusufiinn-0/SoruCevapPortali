using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Hubs;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AnswerController(
            IRepository<Answer> answerRepository,
            IRepository<Question> questionRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnswerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = await _questionRepository.GetByIdAsync(model.QuestionId);
                if (question == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var answer = new Answer
                {
                    Content = model.Content,
                    QuestionId = model.QuestionId,
                    UserId = user.Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsApproved = false // Waiting for admin approval
                };

                await _answerRepository.AddAsync(answer);
                await _answerRepository.SaveAsync();

                // Send SignalR notification
                await _hubContext.Clients.Group("Admins").SendAsync("ReceiveNotification", 
                    $"New answer pending: {question.Title}", "info");

                TempData["Success"] = "Your answer has been submitted. It will be published after admin approval.";
                return RedirectToAction("Details", "Question", new { id = model.QuestionId });
            }

            return RedirectToAction("Details", "Question", new { id = model.QuestionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCorrectAnswer(int id, int questionId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "You need to login." });
            }

            var question = await _questionRepository.GetQueryable()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.QuestionId == questionId);

            if (question == null)
            {
                return Json(new { success = false, message = "Question not found." });
            }

            // Only question owner can mark correct answer
            if (question.UserId != user.Id)
            {
                return Json(new { success = false, message = "Only the question owner can mark the correct answer." });
            }

            // Remove previous correct answers
            var previousCorrectAnswers = await _answerRepository.GetQueryable()
                .Where(c => c.QuestionId == questionId && c.IsCorrectAnswer)
                .ToListAsync();

            foreach (var answer in previousCorrectAnswers)
            {
                answer.IsCorrectAnswer = false;
                _answerRepository.Update(answer);
            }

            // Mark new correct answer
            var answerToUpdate = await _answerRepository.GetByIdAsync(id);
            if (answerToUpdate == null)
            {
                return Json(new { success = false, message = "Answer not found." });
            }

            answerToUpdate.IsCorrectAnswer = true;
            _answerRepository.Update(answerToUpdate);
            await _answerRepository.SaveAsync();

            return Json(new { success = true, message = "Marked as correct answer." });
        }
    }
}


