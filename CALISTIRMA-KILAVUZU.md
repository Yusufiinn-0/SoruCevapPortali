# 🚀 Soru Cevap Portalı - Çalıştırma Kılavuzu

## 📋 İçindekiler
1. [Gereksinimler](#gereksinimler)
2. [Kurulum Adımları](#kurulum-adımları)
3. [Veritabanı Kurulumu](#veritabanı-kurulumu)
4. [Projeyi Çalıştırma](#projeyi-çalıştırma)
5. [Giriş Bilgileri](#giriş-bilgileri)
6. [Proje Yapısı](#proje-yapısı)
7. [Sorun Giderme](#sorun-giderme)

---

## 📦 Gereksinimler

### Zorunlu Yazılımlar

1. **.NET 8.0 SDK**
   - İndirme: https://dotnet.microsoft.com/download/dotnet/8.0
   - Kurulum sonrası kontrol: `dotnet --version`

2. **SQL Server Express** (veya LocalDB)
   - İndirme: Projede `SQL-INDIR.bat` dosyasını çalıştırın
   - Alternatif: Visual Studio Installer'dan "SQL Server Express LocalDB" kurun

3. **Visual Studio 2022** (Önerilen)
   - Community Edition ücretsiz
   - İndirme: https://visualstudio.microsoft.com/

### Alternatif: Visual Studio Code
- VS Code + C# Extension
- İndirme: https://code.visualstudio.com/

---

## 🔧 Kurulum Adımları

### Adım 1: Projeyi İndirin

**GitHub'dan:**
```bash
git clone https://github.com/Yusufiinn-0/SoruCevapPortali.git
cd SoruCevapPortali
```

**Veya ZIP olarak:**
1. GitHub'dan ZIP dosyasını indirin
2. Klasöre çıkarın

### Adım 2: Proje Klasörüne Gidin

```bash
cd SoruCevapPortali/SoruCevapPortali
```

### Adım 3: Bağımlılıkları Kontrol Edin

```bash
dotnet restore
```

---

## 🗄️ Veritabanı Kurulumu

### Yöntem 1: SQL Server Express (Önerilen)

1. **SQL Server Express Kurulumu:**
   ```bash
   # Proje klasöründeki SQL-INDIR.bat dosyasını çalıştırın
   # Veya manuel: https://www.microsoft.com/sql-server/sql-server-downloads
   ```

2. **SQL Server Servisini Başlatın:**
   - Windows Services (services.msc) açın
   - "SQL Server (SQLEXPRESS)" servisini başlatın
   - Veya PowerShell'de:
     ```powershell
     Start-Service "MSSQL$SQLEXPRESS"
     ```

3. **Bağlantı Stringini Kontrol Edin:**
   - `appsettings.json` dosyasında:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SoruCevapDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
     }
   }
   ```

### Yöntem 2: SQL Server LocalDB

1. **LocalDB Başlatın:**
   ```powershell
   sqllocaldb start MSSQLLocalDB
   ```

2. **appsettings.json'u Güncelleyin:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SoruCevapDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
     }
   }
   ```

---

## ▶️ Projeyi Çalıştırma

### Yöntem 1: Visual Studio 2022 ile (En Kolay)

1. **Projeyi Açın:**
   - Visual Studio 2022'yi açın
   - `File > Open > Project/Solution`
   - `SoruCevapPortali.sln` dosyasını seçin

2. **SQL Server'ı Başlatın:**
   - SQL Server Express servisinin çalıştığından emin olun

3. **Projeyi Çalıştırın:**
   - **F5** tuşuna basın (Debug modunda)
   - Veya yeşil ▶ butonuna tıklayın
   - Veya **Ctrl+F5** (Debug olmadan)

4. **Tarayıcı Açılacak:**
   - Otomatik olarak: `https://localhost:5001` veya `http://localhost:5000`
   - Giriş ekranı görünecek

### Yöntem 2: Komut Satırı ile (PowerShell/CMD)

1. **Proje Klasörüne Gidin:**
   ```powershell
   cd SoruCevapPortali/SoruCevapPortali
   ```

2. **SQL Server'ı Başlatın:**
   ```powershell
   # SQL Server Express için:
   Start-Service "MSSQL$SQLEXPRESS"
   
   # Veya LocalDB için:
   sqllocaldb start MSSQLLocalDB
   ```

3. **Projeyi Çalıştırın:**
   ```bash
   dotnet run
   ```

4. **Tarayıcıda Açın:**
   - Çıktıda görünen URL'yi kopyalayın (genelde `http://localhost:5000`)
   - Tarayıcıda açın

### Yöntem 3: Batch Dosyası ile (Otomatik)

1. **Baslat.bat Dosyasını Çalıştırın:**
   ```bash
   # Çift tıklayın veya:
   .\Baslat.bat
   ```

2. **Otomatik olarak:**
   - SQL Server başlatılır (eğer durmuşsa)
   - Proje çalıştırılır
   - Tarayıcı açılır

### Yöntem 4: PowerShell Script ile

```powershell
.\Baslat.ps1
```

---

## 🔐 Giriş Bilgileri

### Admin Kullanıcısı (Otomatik Oluşturulur)

- **Email:** `admin@admin.com`
- **Şifre:** `Admin123!`

### İlk Çalıştırma

1. Uygulama ilk çalıştığında:
   - Veritabanı otomatik oluşturulur
   - Migration'lar uygulanır
   - Admin kullanıcısı ve kategoriler otomatik eklenir

2. Giriş ekranında:
   - Email: `admin@admin.com`
   - Şifre: `Admin123!`
   - "Beni Hatırla" seçeneğini işaretleyebilirsiniz

3. Giriş yaptıktan sonra:
   - Otomatik olarak Admin Paneline yönlendirilirsiniz
   - URL: `https://localhost:5001/Admin/Dashboard`

---

## 📁 Proje Yapısı

```
SoruCevapPortali/
│
├── SoruCevapPortali/              # Ana proje klasörü
│   ├── Areas/
│   │   └── Admin/                 # Admin paneli
│   │       ├── Controllers/       # Admin controller'ları
│   │       │   ├── DashboardController.cs
│   │       │   ├── KategoriController.cs
│   │       │   ├── SoruController.cs
│   │       │   ├── CevapController.cs
│   │       │   └── KullaniciController.cs
│   │       └── Views/             # Admin view'ları
│   │
│   ├── Controllers/               # Ana controller'lar
│   │   ├── AccountController.cs   # Giriş/Çıkış
│   │   └── HomeController.cs      # Ana sayfa
│   │
│   ├── Models/
│   │   ├── Context/
│   │   │   └── AppDbContext.cs    # Veritabanı context
│   │   ├── Entity/                # Veritabanı entity'leri
│   │   │   ├── Kullanici.cs
│   │   │   ├── Kategori.cs
│   │   │   ├── Soru.cs
│   │   │   └── Cevap.cs
│   │   ├── ViewModel/             # View model'leri
│   │   │   ├── LoginViewModel.cs
│   │   │   ├── DashboardViewModel.cs
│   │   │   └── ...
│   │   └── Migrations/            # Veritabanı migration'ları
│   │       ├── 20241207210400_InitialCreate.cs
│   │       └── AppDbContextModelSnapshot.cs
│   │
│   ├── Repository/                # Repository pattern
│   │   ├── IRepository.cs
│   │   └── Repository.cs
│   │
│   ├── Views/                     # Ana view'lar
│   │   ├── Account/
│   │   │   └── Login.cshtml
│   │   ├── Home/
│   │   └── Shared/
│   │       └── _Layout.cshtml
│   │
│   ├── wwwroot/                   # Statik dosyalar
│   │   ├── css/
│   │   ├── js/
│   │   └── lib/                   # Bootstrap, jQuery, FontAwesome
│   │
│   ├── Program.cs                 # Ana başlangıç dosyası
│   ├── appsettings.json           # Yapılandırma dosyası
│   └── SoruCevapPortali.csproj    # Proje dosyası
│
├── SoruCevapPortali.sln           # Solution dosyası
├── README.md                      # Proje dokümantasyonu
└── CALISTIRMA-KILAVUZU.md        # Bu dosya
```

---

## 🎯 Özellikler

### Admin Paneli Özellikleri

1. **Dashboard**
   - Toplam kullanıcı, soru, cevap, kategori sayıları
   - Bekleyen sorular ve cevaplar
   - Son eklenen sorular ve cevaplar

2. **Kategori Yönetimi**
   - Kategori ekleme, düzenleme, silme
   - Aktif/Pasif durumu değiştirme (AJAX)
   - Sıra numarası ayarlama

3. **Soru Yönetimi**
   - Soruları listeleme, görüntüleme, düzenleme
   - Soru onaylama (AJAX)
   - Aktif/Pasif durumu değiştirme (AJAX)
   - Soru silme

4. **Cevap Yönetimi**
   - Cevapları listeleme, görüntüleme, düzenleme
   - Cevap onaylama (AJAX)
   - Doğru cevap işaretleme (AJAX)
   - Cevap silme

5. **Kullanıcı Yönetimi**
   - Kullanıcı ekleme, düzenleme, silme
   - Aktif/Pasif durumu değiştirme (AJAX)
   - Admin yetkisi verme/alma (AJAX)

### AJAX Özellikleri

- Kategori durum değiştirme
- Soru onaylama ve durum değiştirme
- Cevap onaylama ve doğru cevap işaretleme
- Kullanıcı durum ve admin yetkisi değiştirme

---

## 🛠️ Sorun Giderme

### Sorun 1: "Cannot open database" Hatası

**Çözüm:**
```powershell
# SQL Server Express servisini başlatın
Start-Service "MSSQL$SQLEXPRESS"

# Veya LocalDB için:
sqllocaldb start MSSQLLocalDB
```

### Sorun 2: "The database already exists" Hatası

**Çözüm:**
```bash
# Veritabanını silin ve yeniden oluşturun
# SQL Server Management Studio'da:
DROP DATABASE SoruCevapDB;
# Veya appsettings.json'da farklı bir database adı kullanın
```

### Sorun 3: Migration Hatası

**Çözüm:**
```bash
# Migration'ı yeniden oluşturun
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Sorun 4: Port Kullanımda

**Çözüm:**
- `launchSettings.json` dosyasında farklı bir port belirleyin
- Veya çalışan uygulamayı kapatın

### Sorun 5: NuGet Paketleri Hatası

**Çözüm:**
```bash
dotnet restore
dotnet clean
dotnet build
```

### Sorun 6: Visual Studio'da Dosyalar Görünmüyor

**Çözüm:**
1. Solution Explorer'da "Show All Files" butonuna basın
2. Projeyi "Unload Project" sonra "Reload Project" yapın
3. `VS2022-DOSYALARI-GORME-COZUM.md` dosyasına bakın

---

## 📝 Veritabanı Şeması

### Tablolar

1. **Kullanicilar**
   - KullaniciId (PK)
   - Ad, Soyad, Email (Unique), Sifre
   - ProfilResmi, KayitTarihi
   - AktifMi, AdminMi

2. **Kategoriler**
   - KategoriId (PK)
   - KategoriAdi, Aciklama, Ikon
   - AktifMi, SiraNo

3. **Sorular**
   - SoruId (PK)
   - Baslik, Icerik
   - OlusturmaTarihi, GuncellenmeTarihi
   - GoruntulenmeSayisi, AktifMi, OnayliMi
   - KategoriId (FK), KullaniciId (FK)

4. **Cevaplar**
   - CevapId (PK)
   - Icerik
   - OlusturmaTarihi, GuncellenmeTarihi
   - AktifMi, OnayliMi, DogruCevapMi
   - BegeniSayisi
   - SoruId (FK), KullaniciId (FK)

### İlişkiler

- Soru → Kullanici (Many-to-One, Restrict Delete)
- Soru → Kategori (Many-to-One, Restrict Delete)
- Cevap → Soru (Many-to-One, Cascade Delete)
- Cevap → Kullanici (Many-to-One, Restrict Delete)

---

## 🔒 Güvenlik Özellikleri

1. **Cookie Authentication**
   - Oturum yönetimi
   - "Beni Hatırla" özelliği
   - 7 gün oturum süresi

2. **Role-Based Authorization**
   - Admin rolü kontrolü
   - `[Authorize(Roles = "Admin")]` attribute'u

3. **CSRF Protection**
   - AntiForgeryToken kullanımı

4. **Input Validation**
   - Model validation
   - Required, EmailAddress, StringLength attributes

---

## 📞 İletişim ve Destek

- **GitHub Repository:** https://github.com/Yusufiinn-0/SoruCevapPortali
- **Dokümantasyon:** README.md dosyasına bakın

---

## ✅ Kontrol Listesi

Projeyi çalıştırmadan önce:

- [ ] .NET 8.0 SDK kurulu mu? (`dotnet --version`)
- [ ] SQL Server Express veya LocalDB kurulu mu?
- [ ] SQL Server servisi çalışıyor mu?
- [ ] `appsettings.json` bağlantı string'i doğru mu?
- [ ] Proje klasöründe `dotnet restore` çalıştırıldı mı?
- [ ] Port 5000/5001 boş mu?

---

## 🎉 Başarılı Kurulum Sonrası

1. Tarayıcıda giriş ekranı açılmalı
2. `admin@admin.com` / `Admin123!` ile giriş yapın
3. Admin paneline yönlendirilmelisiniz
4. Dashboard'da istatistikler görünmeli
5. Kategori, Soru, Cevap, Kullanıcı yönetimi çalışmalı

**İyi çalışmalar! 🚀**

