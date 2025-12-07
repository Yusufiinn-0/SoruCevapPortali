# Soru Cevap Portalı - Kurulum Kılavuzu

## ⚠️ ÖNEMLİ: LocalDB Başlatma

Eğer "Cannot open database" hatası alıyorsanız, LocalDB'yi başlatmanız gerekiyor!

### Yöntem 1: PowerShell ile (Önerilen)

1. **PowerShell'i Yönetici olarak açın**
2. Şu komutu çalıştırın:
```powershell
sqllocaldb start MSSQLLocalDB
```

3. Sonra projeyi çalıştırın:
```powershell
cd "C:\Users\yusuf\OneDrive\Masaüstü\SoruCevapPortali\SoruCevapPortali"
dotnet run
```

### Yöntem 2: Batch Dosyası ile

1. Proje klasöründeki **`Baslat.bat`** dosyasına çift tıklayın
2. Otomatik olarak LocalDB başlatılacak ve proje çalışacak

### Yöntem 3: Visual Studio ile (Detaylı Adımlar)

#### Adım 1: Projeyi Açma

1. **Visual Studio 2022**'yi açın
2. **File → Open → Project/Solution** (veya `Ctrl+Shift+O`)
3. Şu dosyayı seçin:
   ```
   C:\Users\yusuf\OneDrive\Masaüstü\SoruCevapPortali\SoruCevapPortali\SoruCevapPortali.csproj
   ```
4. Proje yüklenecek ve Solution Explorer'da görünecek

#### Adım 2: LocalDB'yi Başlatma

1. **Tools → NuGet Package Manager → Package Manager Console** menüsünü açın
   - Veya kısayol: `Alt+T, N, O`
2. Package Manager Console penceresinin altında şu komutu çalıştırın:
   ```powershell
   sqllocaldb start MSSQLLocalDB
   ```
3. "LocalDB instance 'MSSQLLocalDB' started." mesajını görmelisiniz

#### Adım 3: Veritabanını Oluşturma

**Seçenek A: Otomatik (Önerilen)**
- Proje zaten `Program.cs` içinde otomatik veritabanı oluşturma koduna sahip
- İlk çalıştırmada veritabanı otomatik oluşturulacak

**Seçenek B: Manuel Migration**
- Package Manager Console'da şu komutu çalıştırın:
  ```powershell
  Update-Database
  ```

#### Adım 4: Projeyi Çalıştırma

1. **F5** tuşuna basın (veya **Debug → Start Debugging**)
   - Veya **Ctrl+F5** (Debug olmadan çalıştırma)
2. Visual Studio otomatik olarak:
   - Projeyi derleyecek (Build)
   - Tarayıcıyı açacak
   - Uygulamayı başlatacak

#### Adım 5: İlk Giriş

1. Tarayıcıda otomatik açılan sayfada (veya manuel olarak `http://localhost:5000`) giriş ekranı görünecek
2. Giriş bilgileri:
   - **E-posta:** `admin@sorucevap.com`
   - **Şifre:** `admin123`
3. Giriş yaptıktan sonra Admin Paneline yönlendirileceksiniz

#### ⚠️ Önemli Notlar

- **Port Değiştirme:** Eğer 5000 portu kullanılıyorsa:
  - `Properties/launchSettings.json` dosyasında port numarasını değiştirebilirsiniz
  - Veya Visual Studio'da proje özelliklerinden port ayarını yapabilirsiniz

- **Hata Durumunda:**
  - **Build → Rebuild Solution** yapın
  - **View → Output** penceresinden hata mesajlarını kontrol edin
  - LocalDB'nin çalıştığından emin olun: `sqllocaldb info MSSQLLocalDB`

- **Debug Modu:**
  - **F5:** Debug modunda çalıştırır (breakpoint'ler çalışır)
  - **Ctrl+F5:** Normal modda çalıştırır (daha hızlı)

---

## 🔧 Alternatif: SQL Server Express Kullanma

Eğer LocalDB çalışmıyorsa, `appsettings.json` dosyasındaki connection string'i değiştirin:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=SoruCevapDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

---

## ✅ Başarılı Kurulum Sonrası

1. Tarayıcıda açın: **http://localhost:5000/Account/Login**
2. Giriş yapın:
   - **E-posta:** admin@sorucevap.com
   - **Şifre:** admin123
3. Admin paneline yönlendirileceksiniz!

---

## 🆘 Sorun Giderme

**"Login failed for user" hatası:**
- LocalDB'yi başlatın: `sqllocaldb start MSSQLLocalDB`
- Veya SQL Server Express kullanın

**"Invalid object name" hatası:**
- Projeyi yeniden başlatın (CTRL+C sonra tekrar `dotnet run`)
- Veritabanı otomatik oluşturulacak

**Port zaten kullanılıyor:**
- Görev Yöneticisi'nde "dotnet" işlemlerini sonlandırın
- Veya farklı port kullanın: `dotnet run --urls "http://localhost:5002"`


