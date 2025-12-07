using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models.Entity
{
    public class Soru
    {
        [Key]
        public int SoruId { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur")]
        [StringLength(300)]
        [Display(Name = "Başlık")]
        public string Baslik { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik alanı zorunludur")]
        [Display(Name = "İçerik")]
        public string Icerik { get; set; } = string.Empty;

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? GuncellenmeTarihi { get; set; }

        [Display(Name = "Görüntülenme Sayısı")]
        public int GoruntulenmeSayisi { get; set; } = 0;

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Onaylı Mı?")]
        public bool OnayliMi { get; set; } = false;

        // Foreign Keys
        [Display(Name = "Kategori")]
        public int KategoriId { get; set; }

        [Display(Name = "Kullanıcı")]
        public int KullaniciId { get; set; }

        // Navigation Properties
        [ForeignKey("KategoriId")]
        public virtual Kategori? Kategori { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual Kullanici? Kullanici { get; set; }

        public virtual ICollection<Cevap>? Cevaplar { get; set; }
    }
}


