using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.ViewModel
{
    public class UserViewModel
    {
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [StringLength(50)]
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [StringLength(50)]
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100)]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [StringLength(255, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        public string? PasswordConfirm { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Admin Mi?")]
        public bool IsAdmin { get; set; } = false;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime? RegistrationDate { get; set; }

        [Display(Name = "Soru Sayısı")]
        public int QuestionCount { get; set; }

        [Display(Name = "Cevap Sayısı")]
        public int AnswerCount { get; set; }
    }
}


