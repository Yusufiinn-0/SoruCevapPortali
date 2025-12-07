using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace SoruCevapPortali.Models.Migrations
{
    [DbContext(typeof(Context.AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Cevap", b =>
                {
                    b.Property<int>("CevapId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CevapId"));

                    b.Property<bool>("AktifMi")
                        .HasColumnType("bit");

                    b.Property<int>("BegeniSayisi")
                        .HasColumnType("int");

                    b.Property<bool>("DogruCevapMi")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("GuncellenmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<string>("Icerik")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OlusturmaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<bool>("OnayliMi")
                        .HasColumnType("bit");

                    b.Property<int>("KullaniciId")
                        .HasColumnType("int");

                    b.Property<int>("SoruId")
                        .HasColumnType("int");

                    b.HasKey("CevapId");

                    b.HasIndex("KullaniciId");

                    b.HasIndex("SoruId");

                    b.ToTable("Cevaplar");
                });

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Kategori", b =>
                {
                    b.Property<int>("KategoriId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KategoriId"));

                    b.Property<string>("Aciklama")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("AktifMi")
                        .HasColumnType("bit");

                    b.Property<string>("Ikon")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("KategoriAdi")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SiraNo")
                        .HasColumnType("int");

                    b.HasKey("KategoriId");

                    b.ToTable("Kategoriler");

                    b.HasData(
                        new
                        {
                            KategoriId = 1,
                            Aciklama = "Genel konular",
                            AktifMi = true,
                            Ikon = "fa-globe",
                            KategoriAdi = "Genel",
                            SiraNo = 1
                        },
                        new
                        {
                            KategoriId = 2,
                            Aciklama = "Teknoloji ile ilgili sorular",
                            AktifMi = true,
                            Ikon = "fa-laptop",
                            KategoriAdi = "Teknoloji",
                            SiraNo = 2
                        },
                        new
                        {
                            KategoriId = 3,
                            Aciklama = "Yazılım geliştirme soruları",
                            AktifMi = true,
                            Ikon = "fa-code",
                            KategoriAdi = "Yazılım",
                            SiraNo = 3
                        },
                        new
                        {
                            KategoriId = 4,
                            Aciklama = "Eğitim ile ilgili sorular",
                            AktifMi = true,
                            Ikon = "fa-graduation-cap",
                            KategoriAdi = "Eğitim",
                            SiraNo = 4
                        },
                        new
                        {
                            KategoriId = 5,
                            Aciklama = "Sağlık ile ilgili sorular",
                            AktifMi = true,
                            Ikon = "fa-heartbeat",
                            KategoriAdi = "Sağlık",
                            SiraNo = 5
                        });
                });

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Kullanici", b =>
                {
                    b.Property<int>("KullaniciId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KullaniciId"));

                    b.Property<bool>("AktifMi")
                        .HasColumnType("bit");

                    b.Property<bool>("AdminMi")
                        .HasColumnType("bit");

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("KayitTarihi")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProfilResmi")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Sifre")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("KullaniciId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Kullanicilar");

                    b.HasData(
                        new
                        {
                            KullaniciId = 1,
                            AktifMi = true,
                            AdminMi = true,
                            Ad = "Admin",
                            Email = "admin@admin.com",
                            KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProfilResmi = (string)null,
                            Sifre = "Admin123!",
                            Soyad = "User"
                        });
                });

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Soru", b =>
                {
                    b.Property<int>("SoruId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SoruId"));

                    b.Property<bool>("AktifMi")
                        .HasColumnType("bit");

                    b.Property<string>("Baslik")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("GoruntulenmeSayisi")
                        .HasColumnType("int");

                    b.Property<DateTime?>("GuncellenmeTarihi")
                        .HasColumnType("datetime2");

                    b.Property<string>("Icerik")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KategoriId")
                        .HasColumnType("int");

                    b.Property<int>("KullaniciId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OlusturmaTarihi")
                        .HasColumnType("datetime2");

                    b.Property<bool>("OnayliMi")
                        .HasColumnType("bit");

                    b.HasKey("SoruId");

                    b.HasIndex("KategoriId");

                    b.HasIndex("KullaniciId");

                    b.ToTable("Sorular");
                });

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Cevap", b =>
                {
                    b.HasOne("SoruCevapPortali.Models.Entity.Kullanici", "Kullanici")
                        .WithMany("Cevaplar")
                        .HasForeignKey("KullaniciId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SoruCevapPortali.Models.Entity.Soru", "Soru")
                        .WithMany("Cevaplar")
                        .HasForeignKey("SoruId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kullanici");

                    b.Navigation("Soru");
                });

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Soru", b =>
                {
                    b.HasOne("SoruCevapPortali.Models.Entity.Kategori", "Kategori")
                        .WithMany("Sorular")
                        .HasForeignKey("KategoriId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SoruCevapPortali.Models.Entity.Kullanici", "Kullanici")
                        .WithMany("Sorular")
                        .HasForeignKey("KullaniciId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Kategori");

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Kategori", b =>
                {
                    b.Navigation("Sorular");
                });

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Kullanici", b =>
                {
                    b.Navigation("Cevaplar");

                    b.Navigation("Sorular");
                });

            modelBuilder.Entity("SoruCevapPortali.Models.Entity.Soru", b =>
                {
                    b.Navigation("Cevaplar");
                });
#pragma warning restore 612, 618
        }
    }
}

