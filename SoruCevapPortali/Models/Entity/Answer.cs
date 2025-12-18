using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models.Entity
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required(ErrorMessage = "Cevap içeriği zorunludur")]
        [Display(Name = "Cevap İçeriği")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Onaylı Mı?")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Doğru Cevap Mı?")]
        public bool IsCorrectAnswer { get; set; } = false;

        [Display(Name = "Beğeni Sayısı")]
        public int LikeCount { get; set; } = 0;

        // Foreign Keys
        [Display(Name = "Soru")]
        public int QuestionId { get; set; }

        [Display(Name = "Kullanıcı")]
        public string UserId { get; set; } = string.Empty;

        // Navigation Properties
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}


