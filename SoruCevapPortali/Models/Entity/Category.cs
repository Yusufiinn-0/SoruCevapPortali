using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.Entity
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100)]
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [StringLength(100)]
        [Display(Name = "İkon")]
        public string? Icon { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Sıra No")]
        public int OrderNumber { get; set; } = 0;

        // Navigation Properties
        public virtual ICollection<Question>? Questions { get; set; }
    }
}


