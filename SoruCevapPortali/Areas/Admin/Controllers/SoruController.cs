using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Models.ViewModel;
using SoruCevapPortali.Repository;

namespace SoruCevapPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SoruController : Controller
    {
        private readonly IRepository<Soru> _soruRepository;
        private readonly IRepository<Kategori> _kategoriRepository;
        private readonly IRepository<Cevap> _cevapRepository;

        public SoruController(
            IRepository<Soru> soruRepository,
            IRepository<Kategori> kategoriRepository,
            IRepository<Cevap> cevapRepository)
        {
            _soruRepository = soruRepository;
            _kategoriRepository = kategoriRepository;
            _cevapRepository = cevapRepository;
        }

        public async Task<IActionResult> Index()
        {
            var sorular = await _soruRepository.GetQueryable()
                .Include(s => s.Kullanici)
                .Include(s => s.Kategori)
                .Include(s => s.Cevaplar)
                .OrderByDescending(s => s.OlusturmaTarihi)
                .Select(s => new SoruViewModel
                {
                    SoruId = s.SoruId,
                    Baslik = s.Baslik,
                    KategoriAdi = s.Kategori!.KategoriAdi,
                    KullaniciAdi = s.Kullanici!.Ad + " " + s.Kullanici.Soyad,
                    OlusturmaTarihi = s.OlusturmaTarihi,
                    GoruntulenmeSayisi = s.GoruntulenmeSayisi,
                    AktifMi = s.AktifMi,
                    OnayliMi = s.OnayliMi,
                    CevapSayisi = s.Cevaplar != null ? s.Cevaplar.Count : 0
                })
                .ToListAsync();

            return View(sorular);
        }

        public async Task<IActionResult> Details(int id)
        {
            var soru = await _soruRepository.GetQueryable()
                .Include(s => s.Kullanici)
                .Include(s => s.Kategori)
                .Include(s => s.Cevaplar)!
                    .ThenInclude(c => c.Kullanici)
                .FirstOrDefaultAsync(s => s.SoruId == id);

            if (soru == null)
            {
                return NotFound();
            }

            return View(soru);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var soru = await _soruRepository.GetByIdAsync(id);
            if (soru == null)
            {
                return NotFound();
            }

            var model = new SoruViewModel
            {
                SoruId = soru.SoruId,
                Baslik = soru.Baslik,
                Icerik = soru.Icerik,
                KategoriId = soru.KategoriId,
                AktifMi = soru.AktifMi,
                OnayliMi = soru.OnayliMi
            };

            await LoadKategoriler(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SoruViewModel model)
        {
            if (ModelState.IsValid)
            {
                var soru = await _soruRepository.GetByIdAsync(model.SoruId);
                if (soru == null)
                {
                    return NotFound();
                }

                soru.Baslik = model.Baslik;
                soru.Icerik = model.Icerik;
                soru.KategoriId = model.KategoriId;
                soru.AktifMi = model.AktifMi;
                soru.OnayliMi = model.OnayliMi;
                soru.GuncellenmeTarihi = DateTime.Now;

                _soruRepository.Update(soru);
                await _soruRepository.SaveAsync();

                TempData["Success"] = "Soru başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            await LoadKategoriler(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var soru = await _soruRepository.GetByIdAsync(id);
            if (soru == null)
            {
                return NotFound();
            }

            // Soruya ait cevapları sil
            var cevaplar = await _cevapRepository.GetAllAsync(c => c.SoruId == id);
            foreach (var cevap in cevaplar)
            {
                _cevapRepository.Delete(cevap);
            }

            _soruRepository.Delete(soru);
            await _soruRepository.SaveAsync();

            TempData["Success"] = "Soru ve ilgili cevaplar başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Soru Onayla
        [HttpPost]
        public async Task<IActionResult> Onayla(int id)
        {
            var soru = await _soruRepository.GetByIdAsync(id);
            if (soru == null)
            {
                return Json(new { success = false, message = "Soru bulunamadı." });
            }

            soru.OnayliMi = true;
            _soruRepository.Update(soru);
            await _soruRepository.SaveAsync();

            return Json(new { success = true, message = "Soru onaylandı." });
        }

        // AJAX - Soru Durumunu Değiştir
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var soru = await _soruRepository.GetByIdAsync(id);
            if (soru == null)
            {
                return Json(new { success = false, message = "Soru bulunamadı." });
            }

            soru.AktifMi = !soru.AktifMi;
            _soruRepository.Update(soru);
            await _soruRepository.SaveAsync();

            return Json(new { success = true, aktifMi = soru.AktifMi, message = soru.AktifMi ? "Soru aktif edildi." : "Soru pasif edildi." });
        }

        private async Task LoadKategoriler(SoruViewModel model)
        {
            var kategoriler = await _kategoriRepository.GetAllAsync(k => k.AktifMi);
            model.Kategoriler = new SelectList(kategoriler, "KategoriId", "KategoriAdi", model.KategoriId);
        }
    }
}


