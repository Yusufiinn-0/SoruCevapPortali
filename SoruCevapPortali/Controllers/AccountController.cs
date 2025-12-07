using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;
using SoruCevapPortali.Repository;
using System.Security.Claims;

namespace SoruCevapPortali.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<Kullanici> _kullaniciRepository;

        public AccountController(IRepository<Kullanici> kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var kullanici = await _kullaniciRepository.GetFirstOrDefaultAsync(
                    k => k.Email == model.Email && k.Sifre == model.Sifre && k.AktifMi);

                if (kullanici != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciId.ToString()),
                        new Claim(ClaimTypes.Name, $"{kullanici.Ad} {kullanici.Soyad}"),
                        new Claim(ClaimTypes.Email, kullanici.Email),
                        new Claim("AdminMi", kullanici.AdminMi.ToString())
                    };

                    if (kullanici.AdminMi)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.BeniHatirla,
                        ExpiresUtc = model.BeniHatirla ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(2)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    if (kullanici.AdminMi)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}


