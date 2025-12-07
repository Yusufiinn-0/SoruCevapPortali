using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SoruCevapPortali.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    KategoriId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ikon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    SiraNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.KategoriId);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProfilResmi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    AdminMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "Sorular",
                columns: table => new
                {
                    SoruId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GoruntulenmeSayisi = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OnayliMi = table.Column<bool>(type: "bit", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    KullaniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorular", x => x.SoruId);
                    table.ForeignKey(
                        name: "FK_Sorular_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "KategoriId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sorular_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cevaplar",
                columns: table => new
                {
                    CevapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OnayliMi = table.Column<bool>(type: "bit", nullable: false),
                    DogruCevapMi = table.Column<bool>(type: "bit", nullable: false),
                    BegeniSayisi = table.Column<int>(type: "int", nullable: false),
                    SoruId = table.Column<int>(type: "int", nullable: false),
                    KullaniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cevaplar", x => x.CevapId);
                    table.ForeignKey(
                        name: "FK_Cevaplar_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cevaplar_Sorular_SoruId",
                        column: x => x.SoruId,
                        principalTable: "Sorular",
                        principalColumn: "SoruId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kategoriler",
                columns: new[] { "KategoriId", "Aciklama", "AktifMi", "Ikon", "KategoriAdi", "SiraNo" },
                values: new object[,]
                {
                    { 1, "Genel konular", true, "fa-globe", "Genel", 1 },
                    { 2, "Teknoloji ile ilgili sorular", true, "fa-laptop", "Teknoloji", 2 },
                    { 3, "Yazılım geliştirme soruları", true, "fa-code", "Yazılım", 3 },
                    { 4, "Eğitim ile ilgili sorular", true, "fa-graduation-cap", "Eğitim", 4 },
                    { 5, "Sağlık ile ilgili sorular", true, "fa-heartbeat", "Sağlık", 5 }
                });

            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "KullaniciId", "Ad", "AdminMi", "AktifMi", "Email", "KayitTarihi", "ProfilResmi", "Sifre", "Soyad" },
                values: new object[] { 1, "Admin", true, true, "admin@sorucevap.com", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin123", "User" });

            migrationBuilder.CreateIndex(
                name: "IX_Cevaplar_KullaniciId",
                table: "Cevaplar",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Cevaplar_SoruId",
                table: "Cevaplar",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_Email",
                table: "Kullanicilar",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_KategoriId",
                table: "Sorular",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_KullaniciId",
                table: "Sorular",
                column: "KullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cevaplar");

            migrationBuilder.DropTable(
                name: "Sorular");

            migrationBuilder.DropTable(
                name: "Kategoriler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");
        }
    }
}
