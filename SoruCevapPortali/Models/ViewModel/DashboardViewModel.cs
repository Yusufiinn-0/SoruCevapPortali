namespace SoruCevapPortali.Models.ViewModel
{
    public class DashboardViewModel
    {
        public int ToplamKullanici { get; set; }
        public int ToplamSoru { get; set; }
        public int ToplamCevap { get; set; }
        public int ToplamKategori { get; set; }
        public int BekleyenSorular { get; set; }
        public int BekleyenCevaplar { get; set; }
        public List<SoruViewModel> SonSorular { get; set; } = new();
        public List<CevapViewModel> SonCevaplar { get; set; } = new();
    }
}


