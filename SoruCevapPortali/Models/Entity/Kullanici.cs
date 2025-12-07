using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.Entity
{
    public class Kullanici
    {
        [Key]
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

        [Required(ErrorMessage = "Şifre alanı zorunludur")]
        [StringLength(255)]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Profil Resmi")]
        public string? ProfilResmi { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Admin Mi?")]
        public bool AdminMi { get; set; } = false;

        // Navigation Properties
        public virtual ICollection<Soru>? Sorular { get; set; }
        public virtual ICollection<Cevap>? Cevaplar { get; set; }
    }
}


