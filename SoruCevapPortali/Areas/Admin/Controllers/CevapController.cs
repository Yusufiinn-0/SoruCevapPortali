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
    public class CevapController : Controller
    {
        private readonly IRepository<Cevap> _cevapRepository;

        public CevapController(IRepository<Cevap> cevapRepository)
        {
            _cevapRepository = cevapRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cevaplar = await _cevapRepository.GetQueryable()
                .Include(c => c.Kullanici)
                .Include(c => c.Soru)
                .OrderByDescending(c => c.OlusturmaTarihi)
                .Select(c => new CevapViewModel
                {
                    CevapId = c.CevapId,
                    Icerik = c.Icerik.Length > 150 ? c.Icerik.Substring(0, 150) + "..." : c.Icerik,
                    SoruId = c.SoruId,
                    SoruBaslik = c.Soru!.Baslik,
                    KullaniciAdi = c.Kullanici!.Ad + " " + c.Kullanici.Soyad,
                    OlusturmaTarihi = c.OlusturmaTarihi,
                    AktifMi = c.AktifMi,
                    OnayliMi = c.OnayliMi,
                    DogruCevapMi = c.DogruCevapMi,
                    BegeniSayisi = c.BegeniSayisi
                })
                .ToListAsync();

            return View(cevaplar);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cevap = await _cevapRepository.GetQueryable()
                .Include(c => c.Kullanici)
                .Include(c => c.Soru)
                .FirstOrDefaultAsync(c => c.CevapId == id);

            if (cevap == null)
            {
                return NotFound();
            }

            return View(cevap);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cevap = await _cevapRepository.GetQueryable()
                .Include(c => c.Soru)
                .FirstOrDefaultAsync(c => c.CevapId == id);

            if (cevap == null)
            {
                return NotFound();
            }

            var model = new CevapViewModel
            {
                CevapId = cevap.CevapId,
                Icerik = cevap.Icerik,
                SoruId = cevap.SoruId,
                SoruBaslik = cevap.Soru?.Baslik,
                AktifMi = cevap.AktifMi,
                OnayliMi = cevap.OnayliMi,
                DogruCevapMi = cevap.DogruCevapMi
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CevapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cevap = await _cevapRepository.GetByIdAsync(model.CevapId);
                if (cevap == null)
                {
                    return NotFound();
                }

                cevap.Icerik = model.Icerik;
                cevap.AktifMi = model.AktifMi;
                cevap.OnayliMi = model.OnayliMi;
                cevap.DogruCevapMi = model.DogruCevapMi;
                cevap.GuncellenmeTarihi = DateTime.Now;

                _cevapRepository.Update(cevap);
                await _cevapRepository.SaveAsync();

                TempData["Success"] = "Cevap başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cevap = await _cevapRepository.GetByIdAsync(id);
            if (cevap == null)
            {
                return NotFound();
            }

            _cevapRepository.Delete(cevap);
            await _cevapRepository.SaveAsync();

            TempData["Success"] = "Cevap başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX - Cevap Onayla
        [HttpPost]
        public async Task<IActionResult> Onayla(int id)
        {
            var cevap = await _cevapRepository.GetByIdAsync(id);
            if (cevap == null)
            {
                return Json(new { success = false, message = "Cevap bulunamadı." });
            }

            cevap.OnayliMi = true;
            _cevapRepository.Update(cevap);
            await _cevapRepository.SaveAsync();

            return Json(new { success = true, message = "Cevap onaylandı." });
        }

        // AJAX - Doğru Cevap İşaretle
        [HttpPost]
        public async Task<IActionResult> DogruIsaretle(int id)
        {
            var cevap = await _cevapRepository.GetByIdAsync(id);
            if (cevap == null)
            {
                return Json(new { success = false, message = "Cevap bulunamadı." });
            }

            cevap.DogruCevapMi = !cevap.DogruCevapMi;
            _cevapRepository.Update(cevap);
            await _cevapRepository.SaveAsync();

            return Json(new { success = true, dogruMu = cevap.DogruCevapMi, message = cevap.DogruCevapMi ? "Doğru cevap olarak işaretlendi." : "Doğru cevap işareti kaldırıldı." });
        }

        // AJAX - Cevap Durumunu Değiştir
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var cevap = await _cevapRepository.GetByIdAsync(id);
            if (cevap == null)
            {
                return Json(new { success = false, message = "Cevap bulunamadı." });
            }

            cevap.AktifMi = !cevap.AktifMi;
            _cevapRepository.Update(cevap);
            await _cevapRepository.SaveAsync();

            return Json(new { success = true, aktifMi = cevap.AktifMi, message = cevap.AktifMi ? "Cevap aktif edildi." : "Cevap pasif edildi." });
        }
    }
}


