# Soru Cevap Portalı

İnternet Programcılığı I Dersi Proje Ödevi - Soru Cevap Portalı

## 📋 Proje Hakkında

Bu proje, ASP.NET Core 8.0 MVC ile geliştirilmiş bir Soru-Cevap Portalı uygulamasıdır. Kullanıcılar sorular sorabilir, cevap verebilir ve kategorilere göre soruları filtreleyebilir.

## 🛠 Kullanılan Teknolojiler

- **Framework:** ASP.NET Core 8.0 MVC
- **Veritabanı:** SQL Server Express
- **ORM:** Entity Framework Core 8.0 (Code-First)
- **Tasarım:** Bootstrap 5.3, FontAwesome 6.5
- **Pattern:** Repository Pattern
- **Kimlik Doğrulama:** Cookie Authentication

## 📁 Proje Yapısı

```
SoruCevapPortali/
├── Areas/
│   └── Admin/
│       ├── Controllers/
│       │   ├── DashboardController.cs
│       │   ├── KategoriController.cs
│       │   ├── SoruController.cs
│       │   ├── CevapController.cs
│       │   └── KullaniciController.cs
│       └── Views/
│           ├── Dashboard/
│           ├── Kategori/
│           ├── Soru/
│           ├── Cevap/
│           ├── Kullanici/
│           └── Shared/
├── Controllers/
│   ├── AccountController.cs
│   └── HomeController.cs
├── Models/
│   ├── Context/
│   │   └── AppDbContext.cs
│   ├── Entity/
│   │   ├── Kullanici.cs
│   │   ├── Kategori.cs
│   │   ├── Soru.cs
│   │   └── Cevap.cs
│   └── ViewModel/
│       ├── LoginViewModel.cs
│       ├── KullaniciViewModel.cs
│       ├── KategoriViewModel.cs
│       ├── SoruViewModel.cs
│       ├── CevapViewModel.cs
│       └── DashboardViewModel.cs
├── Repository/
│   ├── IRepository.cs
│   └── Repository.cs
├── Views/
│   ├── Account/
│   │   ├── Login.cshtml
│   │   └── AccessDenied.cshtml
│   ├── Home/
│   └── Shared/
└── wwwroot/
```

## ✅ Tamamlanan İş Paketleri (Ara Sınav)

| İş Paketi | Durum |
|-----------|-------|
| Veri tabanının tasarımı | ✅ |
| Model ve ViewModellerin oluşturulması | ✅ |
| Veri tabanı bağlantısı ve Migration işlemleri | ✅ |
| Repository yapısının oluşturulması | ✅ |
| Yönetici (Admin) Panelin tasarımı | ✅ |
| Cookie bazlı oturum açma ve yetkilendirme sistemi | ✅ |
| Yönetici sayfalarının kodlanması | ✅ |
| En az bir bölümde AJAX metodunun kullanılması | ✅ |

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8.0 SDK
- SQL Server (LocalDB veya SQL Server Express)
- Visual Studio 2022 veya VS Code

### Adımlar

1. Projeyi klonlayın:
```bash
git clone https://github.com/[kullanici-adi]/SoruCevapPortali.git
cd SoruCevapPortali/SoruCevapPortali
```

2. SQL Server Express'i kurun (eğer kurulu değilse):
   - `SoruCevapPortali/SQL-INDIR.bat` dosyasını çalıştırarak SQL Server Express indirme sayfasını açın
   - `SoruCevapPortali/SQL-KURULUM-TALIMATI.md` dosyasındaki adımları takip edin
   - SQL Server Express servisinin çalıştığından emin olun

3. Projeyi çalıştırın:

   **Visual Studio 2022 ile:**
   - `SoruCevapPortali.sln` dosyasını Visual Studio 2022 ile açın
   - SQL Server Express servisinin çalıştığından emin olun
   - **F5** tuşuna basın veya yeşil ▶ butonuna tıklayın
   - Detaylı talimatlar için `VS2022-CALISTIRMA-TALIMATI.md` dosyasına bakın

   **Komut Satırı ile:**
   - `SoruCevapPortali/Baslat.bat` dosyasını çift tıklayın (SQL Server'ı başlatır ve projeyi çalıştırır)
   
   **Manuel:**
   ```bash
   cd SoruCevapPortali
   dotnet run --urls "http://localhost:5000"
   ```

4. Tarayıcıda açın:
   - Giriş Ekranı: http://localhost:5000
   - Admin Panel: http://localhost:5000/Admin/Dashboard

## 👤 Varsayılan Admin Kullanıcısı

- **E-posta:** admin@admin.com
- **Şifre:** Admin123!

## 📊 Veritabanı Şeması

### Tablolar

**Kullanicilar**
- KullaniciId (PK)
- Ad, Soyad, Email, Sifre
- ProfilResmi, KayitTarihi
- AktifMi, AdminMi

**Kategoriler**
- KategoriId (PK)
- KategoriAdi, Aciklama, Ikon
- AktifMi, SiraNo

**Sorular**
- SoruId (PK)
- Baslik, Icerik
- OlusturmaTarihi, GuncellenmeTarihi
- GoruntulenmeSayisi, AktifMi, OnayliMi
- KategoriId (FK), KullaniciId (FK)

**Cevaplar**
- CevapId (PK)
- Icerik
- OlusturmaTarihi, GuncellenmeTarihi
- AktifMi, OnayliMi, DogruCevapMi
- BegeniSayisi
- SoruId (FK), KullaniciId (FK)

## 🔧 AJAX Kullanımı

Admin panelinde aşağıdaki işlemler AJAX ile gerçekleştirilmektedir:

- Kategori aktif/pasif durumu değiştirme
- Soru onaylama ve durum değiştirme
- Cevap onaylama ve doğru cevap işaretleme
- Kullanıcı aktif/pasif ve admin yetkisi değiştirme

## 📝 Özellikler

### Admin Panel
- Dashboard (Özet istatistikler)
- Kategori yönetimi (CRUD)
- Soru yönetimi (Listeleme, Düzenleme, Silme, Onaylama)
- Cevap yönetimi (Listeleme, Düzenleme, Silme, Onaylama)
- Kullanıcı yönetimi (CRUD, Yetkilendirme)

### Güvenlik
- Cookie tabanlı kimlik doğrulama
- Role-based yetkilendirme (Admin)
- CSRF koruması (AntiForgeryToken)

## 📌 Notlar

- Proje Visual Studio 2022 ile geliştirilmiştir
- .NET 8.0 Core MVC kullanılmıştır
- Code-First yaklaşımı ile veritabanı oluşturulmuştur
- Repository Pattern uygulanmıştır
- Bootstrap 5 ile responsive tasarım yapılmıştır

## 👨‍💻 Geliştirici

İnternet Programcılığı I - Soru Cevap Portalı Projesi


