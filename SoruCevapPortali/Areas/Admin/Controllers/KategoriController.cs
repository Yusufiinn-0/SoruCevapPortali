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
    public class KategoriController : Controller
    {
        private readonly IRepository<Kategori> _kategoriRepository;
        private readonly IRepository<Soru> _soruRepository;

        public KategoriController(IRepository<Kategori> kategoriRepository, IRepository<Soru> soruRepository)
        {
            _kategoriRepository = kategoriRepository;
            _soruRepository = soruRepository;
        }

        public async Task<IActionResult> Index()
        {
            var kategoriler = await _kategoriRepository.GetQueryable()
                .OrderBy(k => k.SiraNo)
                .Select(k => new KategoriViewModel
                {
                    KategoriId = k.KategoriId,
                    KategoriAdi = k.KategoriAdi,
                    Aciklama = k.Aciklama,
                    Ikon = k.Ikon,
                    AktifMi = k.AktifMi,
                    SiraNo = k.SiraNo,
                    SoruSayisi = k.Sorular != null ? k.Sorular.Count : 0
                })
                .ToListAsync();

            return View(kategoriler);
        }

        public IActionResult Create()
        {
            return View(new KategoriViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KategoriViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kategori = new Kategori
                {
                    KategoriAdi = model.KategoriAdi,
                    Aciklama = model.Aciklama,
                    Ikon = model.Ikon,
                    AktifMi = model.AktifMi,
                    SiraNo = model.SiraNo
                };

                await _kategoriRepository.AddAsync(kategori);
                await _kategoriRepository.SaveAsync();

                TempData["Success"] = "Kategori başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var kategori = await _kategoriRepository.GetByIdAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }

            var model = new KategoriViewModel
            {
                KategoriId = kategori.KategoriId,
                KategoriAdi = kategori.KategoriAdi,
                Aciklama = kategori.Aciklama,
                Ikon = kategori.Ikon,
                AktifMi = kategori.AktifMi,
                SiraNo = kategori.SiraNo
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KategoriViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kategori = await _kategoriRepository.GetByIdAsync(model.KategoriId);
                if (kategori == null)
                {
                    return NotFound();
                }

                kategori.KategoriAdi = model.KategoriAdi;
                kategori.Aciklama = model.Aciklama;
                kategori.Ikon = model.Ikon;
                kategori.AktifMi = model.AktifMi;
                kategori.SiraNo = model.SiraNo;

                _kategoriRepository.Update(kategori);
                await _kategoriRepository.SaveAsync();

                TempData["Success"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var kategori = await _kategoriRepository.GetByIdAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }

            // Kategoriye bağlı soru var mı kontrol et
            var soruSayisi = await _soruRepository.CountAsync(s => s.KategoriId == id);
            if (soruSayisi > 0)
            {
                TempData["Error"] = "Bu kategoriye bağlı sorular bulunduğu için silinemez.";
                return RedirectToAction(nameof(Index));
            }

            _kategoriRepository.Delete(kategori);
            await _kategoriRepository.SaveAsync();

            TempData["Success"] = "Kategori başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Kategori Durum Değiştirme
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var kategori = await _kategoriRepository.GetByIdAsync(id);
            if (kategori == null)
            {
                return Json(new { success = false, message = "Kategori bulunamadı." });
            }

            kategori.AktifMi = !kategori.AktifMi;
            _kategoriRepository.Update(kategori);
            await _kategoriRepository.SaveAsync();

            return Json(new { success = true, aktifMi = kategori.AktifMi, message = kategori.AktifMi ? "Kategori aktif edildi." : "Kategori pasif edildi." });
        }
    }
}


