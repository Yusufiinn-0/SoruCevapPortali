using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SoruCevapPortali.Models.ViewModel
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur")]
        [StringLength(300)]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik alanı zorunludur")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Onaylı Mı?")]
        public bool IsApproved { get; set; } = false;

        // Display Properties
        [Display(Name = "Kategori Adı")]
        public string? CategoryName { get; set; }

        [Display(Name = "Kullanıcı")]
        public string? UserName { get; set; }

        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Görüntülenme")]
        public int ViewCount { get; set; }

        [Display(Name = "Cevap Sayısı")]
        public int AnswerCount { get; set; }

        // Answers list
        public List<AnswerViewModel>? Answers { get; set; }

        // Dropdown for
        public SelectList? Categories { get; set; }
    }
}


