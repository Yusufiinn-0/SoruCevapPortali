using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SoruCevapPortali.Models.ViewModel
{
    public class SoruViewModel
    {
        public int SoruId { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur")]
        [StringLength(300)]
        [Display(Name = "Başlık")]
        public string Baslik { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik alanı zorunludur")]
        [Display(Name = "İçerik")]
        public string Icerik { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        [Display(Name = "Kategori")]
        public int KategoriId { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Onaylı Mı?")]
        public bool OnayliMi { get; set; } = false;

        // Display Properties
        [Display(Name = "Kategori Adı")]
        public string? KategoriAdi { get; set; }

        [Display(Name = "Kullanıcı")]
        public string? KullaniciAdi { get; set; }

        public int KullaniciId { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; }

        [Display(Name = "Görüntülenme")]
        public int GoruntulenmeSayisi { get; set; }

        [Display(Name = "Cevap Sayısı")]
        public int CevapSayisi { get; set; }

        // Dropdown için
        public SelectList? Kategoriler { get; set; }
    }
}


