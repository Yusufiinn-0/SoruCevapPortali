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
    public class KullaniciController : Controller
    {
        private readonly IRepository<Kullanici> _kullaniciRepository;

        public KullaniciController(IRepository<Kullanici> kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        public async Task<IActionResult> Index()
        {
            var kullanicilar = await _kullaniciRepository.GetQueryable()
                .Include(k => k.Sorular)
                .Include(k => k.Cevaplar)
                .OrderByDescending(k => k.KayitTarihi)
                .Select(k => new KullaniciViewModel
                {
                    KullaniciId = k.KullaniciId,
                    Ad = k.Ad,
                    Soyad = k.Soyad,
                    Email = k.Email,
                    AktifMi = k.AktifMi,
                    AdminMi = k.AdminMi,
                    KayitTarihi = k.KayitTarihi,
                    SoruSayisi = k.Sorular != null ? k.Sorular.Count : 0,
                    CevapSayisi = k.Cevaplar != null ? k.Cevaplar.Count : 0
                })
                .ToListAsync();

            return View(kullanicilar);
        }

        public IActionResult Create()
        {
            return View(new KullaniciViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KullaniciViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Email benzersizlik kontrolü
                var existingUser = await _kullaniciRepository.AnyAsync(k => k.Email == model.Email);
                if (existingUser)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                    return View(model);
                }

                var kullanici = new Kullanici
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Email = model.Email,
                    Sifre = model.Sifre ?? "123456", // Varsayılan şifre
                    AktifMi = model.AktifMi,
                    AdminMi = model.AdminMi,
                    KayitTarihi = DateTime.Now
                };

                await _kullaniciRepository.AddAsync(kullanici);
                await _kullaniciRepository.SaveAsync();

                TempData["Success"] = "Kullanıcı başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var kullanici = await _kullaniciRepository.GetByIdAsync(id);
            if (kullanici == null)
            {
                return NotFound();
            }

            var model = new KullaniciViewModel
            {
                KullaniciId = kullanici.KullaniciId,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                Email = kullanici.Email,
                AktifMi = kullanici.AktifMi,
                AdminMi = kullanici.AdminMi
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KullaniciViewModel model)
        {
            // Şifre alanlarını validation'dan çıkar (opsiyonel)
            ModelState.Remove("Sifre");
            ModelState.Remove("SifreTekrar");

            if (ModelState.IsValid)
            {
                var kullanici = await _kullaniciRepository.GetByIdAsync(model.KullaniciId);
                if (kullanici == null)
                {
                    return NotFound();
                }

                // Email benzersizlik kontrolü (kendi emaili hariç)
                var existingUser = await _kullaniciRepository.AnyAsync(k => k.Email == model.Email && k.KullaniciId != model.KullaniciId);
                if (existingUser)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                    return View(model);
                }

                kullanici.Ad = model.Ad;
                kullanici.Soyad = model.Soyad;
                kullanici.Email = model.Email;
                kullanici.AktifMi = model.AktifMi;
                kullanici.AdminMi = model.AdminMi;

                // Şifre değişikliği
                if (!string.IsNullOrEmpty(model.Sifre))
                {
                    kullanici.Sifre = model.Sifre;
                }

                _kullaniciRepository.Update(kullanici);
                await _kullaniciRepository.SaveAsync();

                TempData["Success"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var kullanici = await _kullaniciRepository.GetByIdAsync(id);
            if (kullanici == null)
            {
                return NotFound();
            }

            // Admin kendini silemesin
            if (kullanici.KullaniciId == 1)
            {
                TempData["Error"] = "Ana admin kullanıcısı silinemez.";
                return RedirectToAction(nameof(Index));
            }

            _kullaniciRepository.Delete(kullanici);
            await _kullaniciRepository.SaveAsync();

            TempData["Success"] = "Kullanıcı başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Kullanıcı Durumunu Değiştir
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var kullanici = await _kullaniciRepository.GetByIdAsync(id);
            if (kullanici == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            // Admin kendini pasif yapmasın
            if (kullanici.KullaniciId == 1)
            {
                return Json(new { success = false, message = "Ana admin kullanıcısının durumu değiştirilemez." });
            }

            kullanici.AktifMi = !kullanici.AktifMi;
            _kullaniciRepository.Update(kullanici);
            await _kullaniciRepository.SaveAsync();

            return Json(new { success = true, aktifMi = kullanici.AktifMi, message = kullanici.AktifMi ? "Kullanıcı aktif edildi." : "Kullanıcı pasif edildi." });
        }

        // AJAX - Admin Yetkisi Değiştir
        [HttpPost]
        public async Task<IActionResult> ToggleAdmin(int id)
        {
            var kullanici = await _kullaniciRepository.GetByIdAsync(id);
            if (kullanici == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            // Ana admin'in yetkisi değiştirilemez
            if (kullanici.KullaniciId == 1)
            {
                return Json(new { success = false, message = "Ana admin kullanıcısının yetkisi değiştirilemez." });
            }

            kullanici.AdminMi = !kullanici.AdminMi;
            _kullaniciRepository.Update(kullanici);
            await _kullaniciRepository.SaveAsync();

            return Json(new { success = true, adminMi = kullanici.AdminMi, message = kullanici.AdminMi ? "Admin yetkisi verildi." : "Admin yetkisi kaldırıldı." });
        }
    }
}


