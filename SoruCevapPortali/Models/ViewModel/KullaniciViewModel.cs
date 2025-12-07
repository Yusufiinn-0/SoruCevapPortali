using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.ViewModel
{
    public class KullaniciViewModel
    {
        public int KullaniciId { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [StringLength(50)]
        [Display(Name = "Ad")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [StringLength(50)]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100)]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [StringLength(255, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Sifre { get; set; }

        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        public string? SifreTekrar { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Admin Mi?")]
        public bool AdminMi { get; set; } = false;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime? KayitTarihi { get; set; }

        [Display(Name = "Soru Sayısı")]
        public int SoruSayisi { get; set; }

        [Display(Name = "Cevap Sayısı")]
        public int CevapSayisi { get; set; }
    }
}


