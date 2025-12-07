using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.ViewModel
{
    public class KategoriViewModel
    {
        public int KategoriId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100)]
        [Display(Name = "Kategori Adı")]
        public string KategoriAdi { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; }

        [StringLength(100)]
        [Display(Name = "İkon (FontAwesome)")]
        public string? Ikon { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Sıra No")]
        public int SiraNo { get; set; }

        [Display(Name = "Soru Sayısı")]
        public int SoruSayisi { get; set; }
    }
}


