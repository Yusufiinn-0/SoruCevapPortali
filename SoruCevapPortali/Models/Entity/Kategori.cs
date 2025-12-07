using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.Entity
{
    public class Kategori
    {
        [Key]
        public int KategoriId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100)]
        [Display(Name = "Kategori Adı")]
        public string KategoriAdi { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; }

        [StringLength(100)]
        [Display(Name = "İkon")]
        public string? Ikon { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Sıra No")]
        public int SiraNo { get; set; } = 0;

        // Navigation Properties
        public virtual ICollection<Soru>? Sorular { get; set; }
    }
}


