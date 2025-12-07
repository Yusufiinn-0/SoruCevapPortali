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

### Yöntem 3: Visual Studio ile

1. Visual Studio 2022'de projeyi açın
2. **Tools → NuGet Package Manager → Package Manager Console** açın
3. Şu komutları sırayla çalıştırın:
```
sqllocaldb start MSSQLLocalDB
Update-Database
```
4. F5 ile projeyi çalıştırın

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


