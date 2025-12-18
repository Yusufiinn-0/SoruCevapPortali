using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.ViewModel
{
    public class AnswerViewModel
    {
        public int AnswerId { get; set; }

        [Required(ErrorMessage = "Cevap içeriği zorunludur")]
        [Display(Name = "Cevap İçeriği")]
        public string Content { get; set; } = string.Empty;

        public int QuestionId { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Onaylı Mı?")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Doğru Cevap Mı?")]
        public bool IsCorrectAnswer { get; set; } = false;

        [Display(Name = "Beğeni Sayısı")]
        public int LikeCount { get; set; }

        // Display Properties
        [Display(Name = "Kullanıcı")]
        public string? UserName { get; set; }

        [Display(Name = "Soru Başlığı")]
        public string? QuestionTitle { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime CreatedDate { get; set; }
    }
}


