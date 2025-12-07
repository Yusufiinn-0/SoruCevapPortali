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
    public class DashboardController : Controller
    {
        private readonly IRepository<Kullanici> _kullaniciRepository;
        private readonly IRepository<Soru> _soruRepository;
        private readonly IRepository<Cevap> _cevapRepository;
        private readonly IRepository<Kategori> _kategoriRepository;

        public DashboardController(
            IRepository<Kullanici> kullaniciRepository,
            IRepository<Soru> soruRepository,
            IRepository<Cevap> cevapRepository,
            IRepository<Kategori> kategoriRepository)
        {
            _kullaniciRepository = kullaniciRepository;
            _soruRepository = soruRepository;
            _cevapRepository = cevapRepository;
            _kategoriRepository = kategoriRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                ToplamKullanici = await _kullaniciRepository.CountAsync(),
                ToplamSoru = await _soruRepository.CountAsync(),
                ToplamCevap = await _cevapRepository.CountAsync(),
                ToplamKategori = await _kategoriRepository.CountAsync(),
                BekleyenSorular = await _soruRepository.CountAsync(s => !s.OnayliMi),
                BekleyenCevaplar = await _cevapRepository.CountAsync(c => !c.OnayliMi)
            };

            // Son 5 soru
            var sonSorular = await _soruRepository.GetQueryable()
                .Include(s => s.Kullanici)
                .Include(s => s.Kategori)
                .OrderByDescending(s => s.OlusturmaTarihi)
                .Take(5)
                .Select(s => new SoruViewModel
                {
                    SoruId = s.SoruId,
                    Baslik = s.Baslik,
                    KategoriAdi = s.Kategori!.KategoriAdi,
                    KullaniciAdi = s.Kullanici!.Ad + " " + s.Kullanici.Soyad,
                    OlusturmaTarihi = s.OlusturmaTarihi,
                    OnayliMi = s.OnayliMi
                })
                .ToListAsync();

            model.SonSorular = sonSorular;

            // Son 5 cevap
            var sonCevaplar = await _cevapRepository.GetQueryable()
                .Include(c => c.Kullanici)
                .Include(c => c.Soru)
                .OrderByDescending(c => c.OlusturmaTarihi)
                .Take(5)
                .Select(c => new CevapViewModel
                {
                    CevapId = c.CevapId,
                    Icerik = c.Icerik.Length > 100 ? c.Icerik.Substring(0, 100) + "..." : c.Icerik,
                    SoruBaslik = c.Soru!.Baslik,
                    KullaniciAdi = c.Kullanici!.Ad + " " + c.Kullanici.Soyad,
                    OlusturmaTarihi = c.OlusturmaTarihi,
                    OnayliMi = c.OnayliMi
                })
                .ToListAsync();

            model.SonCevaplar = sonCevaplar;

            return View(model);
        }
    }
}


