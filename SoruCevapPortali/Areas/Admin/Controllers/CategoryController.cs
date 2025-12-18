using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Question> _questionRepository;

        public CategoryController(IRepository<Category> categoryRepository, IRepository<Question> questionRepository)
        {
            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetQueryable()
                .OrderBy(k => k.OrderNumber)
                .Select(k => new CategoryViewModel
                {
                    CategoryId = k.CategoryId,
                    CategoryName = k.CategoryName,
                    Description = k.Description,
                    Icon = k.Icon,
                    IsActive = k.IsActive,
                    OrderNumber = k.OrderNumber,
                    QuestionCount = k.Questions != null ? k.Questions.Count : 0
                })
                .ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    CategoryName = model.CategoryName,
                    Description = model.Description,
                    Icon = model.Icon,
                    IsActive = model.IsActive,
                    OrderNumber = model.OrderNumber
                };

                await _categoryRepository.AddAsync(category);
                await _categoryRepository.SaveAsync();

                TempData["Success"] = "Category added successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Icon = category.Icon,
                IsActive = category.IsActive,
                OrderNumber = category.OrderNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryRepository.GetByIdAsync(model.CategoryId);
                if (category == null)
                {
                    return NotFound();
                }

                category.CategoryName = model.CategoryName;
                category.Description = model.Description;
                category.Icon = model.Icon;
                category.IsActive = model.IsActive;
                category.OrderNumber = model.OrderNumber;

                _categoryRepository.Update(category);
                await _categoryRepository.SaveAsync();

                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Check if category has questions
            var questionCount = await _questionRepository.CountAsync(s => s.CategoryId == id);
            if (questionCount > 0)
            {
                TempData["Error"] = "Cannot delete category with associated questions.";
                return RedirectToAction(nameof(Index));
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveAsync();

            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Toggle Category Status
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found." });
            }

            category.IsActive = !category.IsActive;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();

            return Json(new { success = true, isActive = category.IsActive, message = category.IsActive ? "Category activated." : "Category deactivated." });
        }
    }
}


