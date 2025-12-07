using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoruCevapPortali.Models.Entity
{
    public class Cevap
    {
        [Key]
        public int CevapId { get; set; }

        [Required(ErrorMessage = "Cevap içeriği zorunludur")]
        [Display(Name = "Cevap İçeriği")]
        public string Icerik { get; set; } = string.Empty;

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? GuncellenmeTarihi { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Onaylı Mı?")]
        public bool OnayliMi { get; set; } = false;

        [Display(Name = "Doğru Cevap Mı?")]
        public bool DogruCevapMi { get; set; } = false;

        [Display(Name = "Beğeni Sayısı")]
        public int BegeniSayisi { get; set; } = 0;

        // Foreign Keys
        [Display(Name = "Soru")]
        public int SoruId { get; set; }

        [Display(Name = "Kullanıcı")]
        public int KullaniciId { get; set; }

        // Navigation Properties
        [ForeignKey("SoruId")]
        public virtual Soru? Soru { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual Kullanici? Kullanici { get; set; }
    }
}


