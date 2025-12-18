using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Models.Entity.Category> _categoryRepository;
    private readonly IRepository<Models.Entity.Question> _questionRepository;
    private readonly AppDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        IRepository<Models.Entity.Category> categoryRepository,
        IRepository<Models.Entity.Question> questionRepository,
        AppDbContext context)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
        _questionRepository = questionRepository;
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        // Get categories and question counts
        try
        {
            var categories = await _categoryRepository.GetQueryable()
                .Where(k => k.IsActive)
                .OrderBy(k => k.OrderNumber)
                .Select(k => new
                {
                    k.CategoryId,
                    k.CategoryName,
                    k.Icon,
                    QuestionCount = _context.Questions.Count(s => s.CategoryId == k.CategoryId && s.IsActive && s.IsApproved)
                })
                .ToListAsync();

            ViewBag.Categories = categories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading categories");
            ViewBag.Categories = new List<object>();
        }
        
        if (ViewBag.Categories == null)
        {
            ViewBag.Categories = new List<object>();
        }
        
        // Total statistics
        ViewBag.TotalUser = _context.Users.Count();
        ViewBag.TotalQuestion = await _questionRepository.CountAsync(s => s.IsActive && s.IsApproved);
        ViewBag.TotalAnswer = await _context.Answers.CountAsync(c => c.IsActive && c.IsApproved);
        
        // Ana sayfa göster (tüm kullanıcılar için)
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
