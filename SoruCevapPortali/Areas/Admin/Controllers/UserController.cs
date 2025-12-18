using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(k => k.Questions)
                .Include(k => k.Answers)
                .OrderByDescending(k => k.RegistrationDate)
                .Select(k => new UserViewModel
                {
                    UserId = k.Id,
                    FirstName = k.FirstName,
                    LastName = k.LastName,
                    Email = k.Email!,
                    IsActive = k.IsActive,
                    IsAdmin = _context.UserRoles.Any(ur => ur.UserId == k.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Admin")),
                    RegistrationDate = k.RegistrationDate,
                    QuestionCount = k.Questions != null ? k.Questions.Count : 0,
                    AnswerCount = k.Answers != null ? k.Answers.Count : 0
                })
                .ToListAsync();

            return View(users);
        }

        public IActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = model.IsActive,
                    RegistrationDate = DateTime.Now,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password ?? "123456");
                if (result.Succeeded)
                {
                    if (model.IsAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    TempData["Success"] = "User added successfully.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var model = new UserViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                IsActive = user.IsActive,
                IsAdmin = isAdmin
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            ModelState.Remove("Password");
            ModelState.Remove("PasswordConfirm");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                // Email uniqueness check
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id != model.UserId)
                {
                    ModelState.AddModelError("Email", "This email address is already in use.");
                    return View(model);
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.IsActive = model.IsActive;

                // Role management
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (model.IsAdmin && !isAdmin)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else if (!model.IsAdmin && isAdmin)
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                }

                // Password change
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, model.Password);
                }

                await _userManager.UpdateAsync(user);

                TempData["Success"] = "User updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Admin cannot delete themselves
            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser?.Id)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.DeleteAsync(user);

            TempData["Success"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Toggle User Status
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            // Admin cannot deactivate themselves
            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser?.Id)
            {
                return Json(new { success = false, message = "You cannot change your own account status." });
            }

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);

            return Json(new { success = true, isActive = user.IsActive, message = user.IsActive ? "User activated." : "User deactivated." });
        }

        // AJAX - Toggle Admin Role
        [HttpPost]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            // Admin cannot remove their own admin role
            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser?.Id)
            {
                return Json(new { success = false, message = "You cannot remove your own admin privileges." });
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return Json(new { success = true, isAdmin = !isAdmin, message = !isAdmin ? "Admin privileges granted." : "Admin privileges removed." });
        }
    }
}


