# 📁 DOSYA VE KLASÖR AÇIKLAMALARI

Bu dokümantasyon projedeki tüm klasör ve dosyaların ne işe yaradığını açıklar.

---

## 📂 Kök Dizin (Root) Dosyaları

### 📄 SoruCevapPortali.sln
**İşlevi:** Visual Studio Solution dosyası
- Tüm projeleri bir arada yönetir
- Visual Studio'da açıldığında tüm projeyi gösterir
- Birden fazla proje varsa (ör: Test projesi) bunları da içerir

### 📄 README.md
**İşlevi:** Proje ana dokümantasyonu
- Proje hakkında genel bilgiler
- Kullanılan teknolojiler
- Kurulum talimatları
- Özellikler listesi

### 📄 CALISTIRMA-KILAVUZU.md
**İşlevi:** Detaylı çalıştırma kılavuzu
- Adım adım kurulum talimatları
- Veritabanı kurulumu
- Sorun giderme
- Tüm çalıştırma yöntemleri

### 📄 HIZLI-BASLANGIC.md
**İşlevi:** Hızlı başlangıç rehberi
- 5 dakikada çalıştırma
- Temel adımlar
- Yaygın sorunlar

### 📄 TUM-KODLAR-VE-ACIKLAMALAR.md
**İşlevi:** Kod açıklamaları dokümantasyonu
- Tüm kodların detaylı açıklamaları
- Her kod bloğunun ne işe yaradığı
- Örnekler ve kullanım senaryoları

### 📄 .gitignore
**İşlevi:** Git ignore kuralları
- Git'e eklenmemesi gereken dosyaları belirtir
- bin/, obj/, .vs/ gibi klasörler ignore edilir
- Gereksiz dosyaların repository'ye eklenmesini önler

---

## 📂 SoruCevapPortali/ - Ana Proje Klasörü

### 📄 SoruCevapPortali.csproj
**İşlevi:** .NET proje dosyası
- Proje yapılandırması
- NuGet paket bağımlılıkları
- Target framework (.NET 8.0)
- Build ayarları

### 📄 Program.cs
**İşlevi:** Uygulamanın ana başlangıç dosyası
- Tüm servislerin kaydedildiği yer
- Middleware'lerin yapılandırıldığı yer
- Veritabanı migration'larının çalıştırıldığı yer
- Routing yapılandırması

### 📄 appsettings.json
**İşlevi:** Uygulama yapılandırma dosyası
- Veritabanı bağlantı string'i
- Logging ayarları
- Diğer yapılandırma değerleri

### 📄 appsettings.Development.json
**İşlevi:** Geliştirme ortamı yapılandırması
- Development modunda geçerli ayarlar
- Debug için özel log seviyeleri

### 📄 Baslat.bat
**İşlevi:** Windows batch dosyası (projeyi çalıştırır)
- SQL Server'ı başlatır
- Projeyi otomatik çalıştırır
- Çift tıklayarak çalıştırılabilir

### 📄 Baslat.ps1
**İşlevi:** PowerShell script (projeyi çalıştırır)
- Baslat.bat'ın PowerShell versiyonu
- Daha gelişmiş özellikler için kullanılabilir

### 📄 SQL-INDIR.bat
**İşlevi:** SQL Server Express indirme sayfasını açar
- SQL Server Express'in indirilebileceği web sayfasını açar

### 📄 SQL-KURULUM-TALIMATI.md
**İşlevi:** SQL Server kurulum talimatları
- SQL Server Express kurulum adımları
- Veritabanı yapılandırması

### 📄 README-KURULUM.md
**İşlevi:** Kurulum kılavuzu
- Visual Studio ile kurulum
- Komut satırı ile kurulum
- LocalDB kurulumu

---

## 📂 Areas/ - Area Klasörü

**İşlevi:** MVC Area yapısı
- Büyük projelerde modüler yapı sağlar
- Admin paneli ayrı bir area olarak organize edilir

### 📂 Areas/Admin/ - Admin Panel Area

**İşlevi:** Yönetici paneli modülü

#### 📂 Areas/Admin/Controllers/ - Admin Controller'ları

##### 📄 DashboardController.cs
**İşlevi:** Admin panel ana sayfa controller'ı
- Dashboard istatistiklerini toplar
- Toplam kullanıcı, soru, cevap, kategori sayıları
- Son eklenen sorular ve cevaplar
- Bekleyen onaylar

##### 📄 KategoriController.cs
**İşlevi:** Kategori yönetimi controller'ı
- Kategori listeleme (Index)
- Yeni kategori ekleme (Create)
- Kategori düzenleme (Edit)
- Kategori silme (Delete)
- Kategori durumu değiştirme (ToggleStatus - AJAX)

##### 📄 SoruController.cs
**İşlevi:** Soru yönetimi controller'ı
- Soru listeleme (Index)
- Soru detayları (Details)
- Soru düzenleme (Edit)
- Soru silme (Delete)
- Soru onaylama (Onayla - AJAX)
- Soru durumu değiştirme (ToggleStatus - AJAX)

##### 📄 CevapController.cs
**İşlevi:** Cevap yönetimi controller'ı
- Cevap listeleme (Index)
- Cevap detayları (Details)
- Cevap düzenleme (Edit)
- Cevap silme (Delete)
- Cevap onaylama (Onayla - AJAX)
- Doğru cevap işaretleme (DogruIsaretle - AJAX)
- Cevap durumu değiştirme (ToggleStatus - AJAX)

##### 📄 KullaniciController.cs
**İşlevi:** Kullanıcı yönetimi controller'ı
- Kullanıcı listeleme (Index)
- Yeni kullanıcı ekleme (Create)
- Kullanıcı düzenleme (Edit)
- Kullanıcı silme (Delete)
- Kullanıcı durumu değiştirme (ToggleStatus - AJAX)
- Admin yetkisi verme/alma (ToggleAdmin - AJAX)

#### 📂 Areas/Admin/Views/ - Admin View'ları

**İşlevi:** Admin paneli görsel arayüz dosyaları

##### 📂 Areas/Admin/Views/Dashboard/
- **Index.cshtml** - Admin panel ana sayfa görünümü

##### 📂 Areas/Admin/Views/Kategori/
- **Index.cshtml** - Kategori listesi görünümü
- **Create.cshtml** - Yeni kategori ekleme formu
- **Edit.cshtml** - Kategori düzenleme formu

##### 📂 Areas/Admin/Views/Soru/
- **Index.cshtml** - Soru listesi görünümü
- **Details.cshtml** - Soru detay görünümü
- **Edit.cshtml** - Soru düzenleme formu

##### 📂 Areas/Admin/Views/Cevap/
- **Index.cshtml** - Cevap listesi görünümü
- **Details.cshtml** - Cevap detay görünümü
- **Edit.cshtml** - Cevap düzenleme formu

##### 📂 Areas/Admin/Views/Kullanici/
- **Index.cshtml** - Kullanıcı listesi görünümü
- **Create.cshtml** - Yeni kullanıcı ekleme formu
- **Edit.cshtml** - Kullanıcı düzenleme formu

##### 📂 Areas/Admin/Views/Shared/
- **_AdminLayout.cshtml** - Admin panel layout dosyası
  - Admin panelinin ana şablonu
  - Sidebar menü
  - Header ve footer
  - Tüm admin sayfalarında kullanılır

##### 📂 Areas/Admin/Views/
- **_ViewImports.cshtml** - Global namespace'ler ve helper'lar
  - Tüm view'larda kullanılan namespace'ler
  - Tag helper'lar

- **_ViewStart.cshtml** - View başlangıç dosyası
  - Tüm view'lar için varsayılan layout
  - Admin area için _AdminLayout.cshtml kullanılır

---

## 📂 Controllers/ - Ana Controller'lar

### 📄 AccountController.cs
**İşlevi:** Giriş/Çıkış işlemleri
- **Login (GET)** - Giriş sayfası gösterir
- **Login (POST)** - Giriş işlemini gerçekleştirir
  - Email ve şifre kontrolü
  - Cookie oluşturma
  - Claims (kullanıcı bilgileri) ekleme
  - Admin rolü kontrolü
- **Logout (POST)** - Çıkış işlemi, cookie'yi siler
- **AccessDenied** - Erişim engellendi sayfası

### 📄 HomeController.cs
**İşlevi:** Ana sayfa controller'ı
- **Index** - Ana sayfa görünümü
- **Privacy** - Gizlilik sayfası
- **Error** - Hata sayfası

---

## 📂 Models/ - Model Klasörü

**İşlevi:** Veri modelleri ve veritabanı yapısı

### 📂 Models/Context/

#### 📄 AppDbContext.cs
**İşlevi:** Entity Framework DbContext sınıfı
- Veritabanı bağlantısını yönetir
- DbSet'ler (tablolar) tanımlar
- Tablo ilişkilerini (relationships) yapılandırır
- Foreign key davranışlarını belirler
- Seed data (varsayılan veriler) ekler
- Unique constraint'ler tanımlar

### 📂 Models/Entity/ - Veritabanı Tabloları

**İşlevi:** Her entity bir veritabanı tablosunu temsil eder

#### 📄 Kullanici.cs
**İşlevi:** Kullanıcı bilgileri
- Kullanıcı tablosu için entity
- Ad, Soyad, Email, Şifre
- AdminMi, AktifMi gibi özellikler
- Navigation properties (Sorular, Cevaplar)

#### 📄 Kategori.cs
**İşlevi:** Soru kategorileri
- Kategori tablosu için entity
- KategoriAdi, Aciklama, Ikon
- SiraNo (listeleme sırası)
- Navigation property (Sorular)

#### 📄 Soru.cs
**İşlevi:** Kullanıcıların sorduğu sorular
- Soru tablosu için entity
- Baslik, Icerik
- OnayliMi, AktifMi
- Foreign keys (KategoriId, KullaniciId)
- Navigation properties (Kategori, Kullanici, Cevaplar)

#### 📄 Cevap.cs
**İşlevi:** Sorulara verilen cevaplar
- Cevap tablosu için entity
- Icerik
- DogruCevapMi, OnayliMi
- BegeniSayisi
- Foreign keys (SoruId, KullaniciId)
- Navigation properties (Soru, Kullanici)

### 📂 Models/ViewModel/ - View İçin Modeller

**İşlevi:** View'lara gönderilecek özel modeller
- Entity'lerden farklı olarak sadece view için gerekli alanları içerir
- Güvenlik için hassas bilgileri (şifre gibi) gizler

#### 📄 LoginViewModel.cs
**İşlevi:** Giriş formu için model
- Email, Sifre
- BeniHatirla checkbox'ı

#### 📄 DashboardViewModel.cs
**İşlevi:** Dashboard sayfası için model
- ToplamKullanici, ToplamSoru, ToplamCevap, ToplamKategori
- BekleyenSorular, BekleyenCevaplar
- SonSorular, SonCevaplar listeleri

#### 📄 KategoriViewModel.cs
**İşlevi:** Kategori işlemleri için model
- KategoriId, KategoriAdi, Aciklama, Ikon
- AktifMi, SiraNo
- SoruSayisi (kategoriye ait soru sayısı)

#### 📄 SoruViewModel.cs
**İşlevi:** Soru işlemleri için model
- SoruId, Baslik, Icerik
- KategoriAdi, KullaniciAdi (ilişkili tablolardan)
- OnayliMi, AktifMi
- CevapSayisi, GoruntulenmeSayisi

#### 📄 CevapViewModel.cs
**İşlevi:** Cevap işlemleri için model
- CevapId, Icerik
- SoruBaslik (ilişkili tablodan)
- KullaniciAdi
- DogruCevapMi, OnayliMi
- BegeniSayisi

#### 📄 KullaniciViewModel.cs
**İşlevi:** Kullanıcı işlemleri için model
- KullaniciId, Ad, Soyad, Email
- AktifMi, AdminMi
- SoruSayisi, CevapSayisi (kullanıcının toplam soru/cevap sayısı)

#### 📄 ErrorViewModel.cs
**İşlevi:** Hata sayfası için model
- RequestId (hata takibi için)

### 📂 Models/Migrations/ - Veritabanı Migration'ları

**İşlevi:** Veritabanı şeması değişikliklerini yönetir

#### 📄 20241207210400_InitialCreate.cs
**İşlevi:** İlk migration dosyası
- Veritabanı tablolarını oluşturur
- İlişkileri (foreign keys) tanımlar
- Index'leri oluşturur
- Seed data'yı (varsayılan veriler) ekler
- **Up()** metodu: Migration uygulanırken çalışır
- **Down()** metodu: Migration geri alınırken çalışır

#### 📄 AppDbContextModelSnapshot.cs
**İşlevi:** Model snapshot dosyası
- Mevcut veritabanı modelinin snapshot'ı
- Entity Framework tarafından otomatik oluşturulur
- Yeni migration oluştururken değişiklikleri tespit etmek için kullanılır

---

## 📂 Repository/ - Repository Pattern

**İşlevi:** Veritabanı işlemlerini soyutlar

### 📄 IRepository.cs
**İşlevi:** Repository interface'i
- Generic interface (her entity için kullanılabilir)
- GetAllAsync, GetByIdAsync, AddAsync, Update, Delete
- CountAsync, AnyAsync gibi yardımcı metotlar
- GetQueryable (LINQ sorguları için)

### 📄 Repository.cs
**İşlevi:** IRepository implementasyonu
- Entity Framework ile veritabanı işlemlerini yapar
- Generic yapı sayesinde tüm entity'ler için çalışır
- Async metotlar kullanır (performans için)

---

## 📂 Views/ - Ana View'lar

**İşlevi:** Kullanıcı arayüzü dosyaları (Razor Pages)

### 📂 Views/Account/

#### 📄 Login.cshtml
**İşlevi:** Giriş sayfası görünümü
- Email ve şifre input alanları
- "Beni Hatırla" checkbox'ı
- Form validation
- Hata mesajları

#### 📄 AccessDenied.cshtml
**İşlevi:** Erişim engellendi sayfası
- Kullanıcıya yetki hatası mesajı gösterir
- Giriş yapma veya ana sayfaya dönme seçenekleri

### 📂 Views/Home/

#### 📄 Index.cshtml
**İşlevi:** Ana sayfa görünümü
- Site ana sayfası
- Kullanıcıların göreceği ilk sayfa

#### 📄 Privacy.cshtml
**İşlevi:** Gizlilik politikası sayfası

### 📂 Views/Shared/

**İşlevi:** Tüm sayfalarda kullanılan ortak dosyalar

#### 📄 _Layout.cshtml
**İşlevi:** Ana site layout dosyası
- Ana site şablonu
- Navbar (üst menü)
- Footer (alt bilgi)
- Tüm CSS ve JavaScript dosyalarının dahil edildiği yer
- Tüm public sayfalarda kullanılır

#### 📄 _Layout.cshtml.css
**İşlevi:** Layout için özel CSS stilleri

#### 📄 _ValidationScriptsPartial.cshtml
**İşlevi:** Form validation için JavaScript dosyaları
- jQuery Validation
- Unobtrusive Validation
- Form kontrolleri için gerekli script'ler

#### 📄 Error.cshtml
**İşlevi:** Genel hata sayfası
- Beklenmeyen hatalarda gösterilir

### 📂 Views/
- **_ViewImports.cshtml** - Global namespace'ler ve tag helper'lar
- **_ViewStart.cshtml** - Varsayılan layout ayarı

---

## 📂 wwwroot/ - Statik Dosyalar

**İşlevi:** Tarayıcıya direkt servis edilen dosyalar
- CSS, JavaScript, resimler
- Kütüphaneler (Bootstrap, jQuery, FontAwesome)

### 📂 wwwroot/css/

#### 📄 site.css
**İşlevi:** Özel CSS stilleri
- Projeye özel stil tanımlamaları
- Layout ve component stilleri

### 📂 wwwroot/js/

#### 📄 site.js
**İşlevi:** Özel JavaScript kodları
- Projeye özel JavaScript fonksiyonları
- Custom script'ler

### 📂 wwwroot/lib/ - Üçüncü Parti Kütüphaneler

#### 📂 wwwroot/lib/bootstrap/
**İşlevi:** Bootstrap CSS framework
- Grid system
- Component'ler (buton, form, card, vb.)
- Responsive tasarım

#### 📂 wwwroot/lib/jquery/
**İşlevi:** jQuery JavaScript kütüphanesi
- DOM manipülasyonu
- AJAX işlemleri
- Event handling

#### 📂 wwwroot/lib/jquery-validation/
**İşlevi:** jQuery Validation plugin
- Form validation
- Client-side kontroller

#### 📂 wwwroot/lib/jquery-validation-unobtrusive/
**İşlevi:** Unobtrusive Validation
- ASP.NET Core ile entegre validation
- Server-side validation attribute'larını client-side'a çevirir

### 📄 wwwroot/favicon.ico
**İşlevi:** Site ikonu
- Tarayıcı sekmesinde görünen ikon

---

## 📂 Properties/ - Proje Özellikleri

### 📄 launchSettings.json
**İşlevi:** Uygulama başlatma ayarları
- Çalıştırma profilleri
- Port ayarları (5000, 5001)
- Environment değişkenleri
- IIS Express ayarları

---

## 📂 bin/ - Build Çıktıları

**İşlevi:** Derlenmiş dosyalar
- .dll dosyaları (compiled code)
- .exe dosyası
- .NET runtime dosyaları
- **Not:** Git'e eklenmez (.gitignore'da)

---

## 📂 obj/ - Geçici Build Dosyaları

**İşlevi:** Geçici derleme dosyaları
- Intermediate build files
- Cache dosyaları
- **Not:** Git'e eklenmez (.gitignore'da)

---

## 🎯 Dosya Akışı Özeti

### 1. Kullanıcı İsteği Akışı
```
Tarayıcı → Routing (Program.cs) 
        → Controller (AccountController.cs)
        → Repository (Repository.cs)
        → Database (AppDbContext.cs)
        → Entity (Kullanici.cs)
        → ViewModel (LoginViewModel.cs)
        → View (Login.cshtml)
        → HTML Response → Tarayıcı
```

### 2. Veri Akışı
```
Database (SQL Server)
    ↓
AppDbContext.cs (Entity Framework)
    ↓
Repository.cs (Repository Pattern)
    ↓
Controller.cs (Business Logic)
    ↓
ViewModel (Data Transfer)
    ↓
View.cshtml (UI)
    ↓
Kullanıcı (Browser)
```

---

## 📋 Özet Tablo

| Klasör/Dosya | İşlevi | Önem Derecesi |
|--------------|--------|---------------|
| **Program.cs** | Uygulama başlangıcı, servis kayıtları | ⭐⭐⭐ Kritik |
| **AppDbContext.cs** | Veritabanı bağlamı | ⭐⭐⭐ Kritik |
| **Controllers/** | HTTP isteklerini işler | ⭐⭐⭐ Kritik |
| **Models/Entity/** | Veritabanı tabloları | ⭐⭐⭐ Kritik |
| **Models/ViewModel/** | View için modeller | ⭐⭐ Önemli |
| **Repository/** | Veritabanı işlemleri | ⭐⭐⭐ Kritik |
| **Views/** | Kullanıcı arayüzü | ⭐⭐⭐ Kritik |
| **wwwroot/** | Statik dosyalar | ⭐⭐ Önemli |
| **Areas/Admin/** | Admin paneli | ⭐⭐⭐ Kritik |
| **Migrations/** | Veritabanı şeması | ⭐⭐ Önemli |

---

## 🔍 Hızlı Referans

### En Çok Düzenlenen Dosyalar

1. **Controllers/** - İş mantığı burada
2. **Views/** - Arayüz değişiklikleri
3. **Models/Entity/** - Veritabanı şeması değişiklikleri
4. **Program.cs** - Yeni servis ekleme
5. **AppDbContext.cs** - Veritabanı ilişkileri

### Yeni Özellik Eklerken

1. **Entity** ekle → `Models/Entity/`
2. **ViewModel** ekle → `Models/ViewModel/`
3. **Repository** kaydet → `Program.cs`
4. **Controller** ekle → `Controllers/` veya `Areas/Admin/Controllers/`
5. **View** ekle → `Views/` veya `Areas/Admin/Views/`
6. **Migration** oluştur → `dotnet ef migrations add`

---

**Bu dokümantasyon projedeki her dosya ve klasörün işlevini açıklar. Sorularınız için TUM-KODLAR-VE-ACIKLAMALAR.md dosyasına bakabilirsiniz.**

