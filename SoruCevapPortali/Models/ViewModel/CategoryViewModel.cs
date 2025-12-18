using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.ViewModel
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100)]
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [StringLength(100)]
        [Display(Name = "İkon (FontAwesome)")]
        public string? Icon { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Sıra No")]
        public int OrderNumber { get; set; }

        [Display(Name = "Soru Sayısı")]
        public int QuestionCount { get; set; }
    }
}


