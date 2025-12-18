using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Controllers
{
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly AppDbContext _context;

        public CategoryController(
            IRepository<Category> categoryRepository,
            AppDbContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get categories
                var categories = await _categoryRepository.GetQueryable()
                    .Where(k => k.IsActive)
                    .OrderBy(k => k.OrderNumber)
                    .ToListAsync();

                // Get category IDs
                var categoryIds = categories.Select(k => k.CategoryId).ToList();

                // Calculate question count for each category
                var questionCounts = await _context.Questions
                    .Where(s => categoryIds.Contains(s.CategoryId) && s.IsActive && s.IsApproved)
                    .GroupBy(s => s.CategoryId)
                    .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.CategoryId, x => x.Count);

                // Convert to ViewModel
                var categoryList = categories.Select(k => new CategoryListViewModel
                {
                    CategoryId = k.CategoryId,
                    CategoryName = k.CategoryName,
                    Description = k.Description,
                    Icon = k.Icon,
                    QuestionCount = questionCounts.ContainsKey(k.CategoryId) ? questionCounts[k.CategoryId] : 0
                }).ToList();

                ViewBag.Categories = categoryList;
                ViewBag.TotalCategory = categoryList.Count;
                ViewBag.TotalQuestion = categoryList.Sum(k => k.QuestionCount);
            }
            catch (Exception)
            {
                ViewBag.Categories = new List<CategoryListViewModel>();
                ViewBag.TotalCategory = 0;
                ViewBag.TotalQuestion = 0;
            }

            return View();
        }
    }
}


