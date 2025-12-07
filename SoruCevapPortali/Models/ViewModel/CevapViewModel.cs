using System.ComponentModel.DataAnnotations;

namespace SoruCevapPortali.Models.ViewModel
{
    public class CevapViewModel
    {
        public int CevapId { get; set; }

        [Required(ErrorMessage = "Cevap içeriği zorunludur")]
        [Display(Name = "Cevap İçeriği")]
        public string Icerik { get; set; } = string.Empty;

        public int SoruId { get; set; }

        public int KullaniciId { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool AktifMi { get; set; } = true;

        [Display(Name = "Onaylı Mı?")]
        public bool OnayliMi { get; set; } = false;

        [Display(Name = "Doğru Cevap Mı?")]
        public bool DogruCevapMi { get; set; } = false;

        [Display(Name = "Beğeni Sayısı")]
        public int BegeniSayisi { get; set; }

        // Display Properties
        [Display(Name = "Kullanıcı")]
        public string? KullaniciAdi { get; set; }

        [Display(Name = "Soru Başlığı")]
        public string? SoruBaslik { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; }
    }
}


