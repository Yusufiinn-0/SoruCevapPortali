using Microsoft.EntityFrameworkCore;
using SoruCevapPortali.Models.Entity;

namespace SoruCevapPortali.Models.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Soru> Sorular { get; set; }
        public DbSet<Cevap> Cevaplar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kullanici tablosu için unique email constraint
            modelBuilder.Entity<Kullanici>()
                .HasIndex(k => k.Email)
                .IsUnique();

            // Soru - Kullanici ilişkisi
            modelBuilder.Entity<Soru>()
                .HasOne(s => s.Kullanici)
                .WithMany(k => k.Sorular)
                .HasForeignKey(s => s.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // Soru - Kategori ilişkisi
            modelBuilder.Entity<Soru>()
                .HasOne(s => s.Kategori)
                .WithMany(k => k.Sorular)
                .HasForeignKey(s => s.KategoriId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cevap - Soru ilişkisi
            modelBuilder.Entity<Cevap>()
                .HasOne(c => c.Soru)
                .WithMany(s => s.Cevaplar)
                .HasForeignKey(c => c.SoruId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cevap - Kullanici ilişkisi
            modelBuilder.Entity<Cevap>()
                .HasOne(c => c.Kullanici)
                .WithMany(k => k.Cevaplar)
                .HasForeignKey(c => c.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // Varsayılan admin kullanıcısı
            modelBuilder.Entity<Kullanici>().HasData(
                new Kullanici
                {
                    KullaniciId = 1,
                    Ad = "Admin",
                    Soyad = "User",
                    Email = "admin@sorucevap.com",
                    Sifre = "admin123", // Gerçek uygulamada hash'lenmiş olmalı
                    AdminMi = true,
                    AktifMi = true,
                    KayitTarihi = new DateTime(2024, 1, 1)
                }
            );

            // Varsayılan kategoriler
            modelBuilder.Entity<Kategori>().HasData(
                new Kategori { KategoriId = 1, KategoriAdi = "Genel", Aciklama = "Genel konular", Ikon = "fa-globe", SiraNo = 1, AktifMi = true },
                new Kategori { KategoriId = 2, KategoriAdi = "Teknoloji", Aciklama = "Teknoloji ile ilgili sorular", Ikon = "fa-laptop", SiraNo = 2, AktifMi = true },
                new Kategori { KategoriId = 3, KategoriAdi = "Yazılım", Aciklama = "Yazılım geliştirme soruları", Ikon = "fa-code", SiraNo = 3, AktifMi = true },
                new Kategori { KategoriId = 4, KategoriAdi = "Eğitim", Aciklama = "Eğitim ile ilgili sorular", Ikon = "fa-graduation-cap", SiraNo = 4, AktifMi = true },
                new Kategori { KategoriId = 5, KategoriAdi = "Sağlık", Aciklama = "Sağlık ile ilgili sorular", Ikon = "fa-heartbeat", SiraNo = 5, AktifMi = true }
            );
        }
    }
}
