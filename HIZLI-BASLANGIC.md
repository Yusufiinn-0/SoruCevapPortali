# ⚡ Hızlı Başlangıç Kılavuzu

## 🚀 5 Dakikada Çalıştırma

### 1️⃣ Gereksinimleri Kontrol Edin

```bash
# .NET SDK kontrolü
dotnet --version
# Çıktı: 8.0.x olmalı
```

### 2️⃣ SQL Server'ı Başlatın

**SQL Server Express için:**
```powershell
Start-Service "MSSQL$SQLEXPRESS"
```

**Veya LocalDB için:**
```powershell
sqllocaldb start MSSQLLocalDB
```

### 3️⃣ Projeyi Çalıştırın

**Seçenek A: Batch Dosyası (En Kolay)**
```
Baslat.bat dosyasına çift tıklayın
```

**Seçenek B: Visual Studio**
```
1. SoruCevapPortali.sln dosyasını açın
2. F5 tuşuna basın
```

**Seçenek C: Komut Satırı**
```bash
cd SoruCevapPortali/SoruCevapPortali
dotnet run
```

### 4️⃣ Giriş Yapın

**Tarayıcıda açın:** `http://localhost:5000`

**Giriş Bilgileri:**
- **Email:** `admin@admin.com`
- **Şifre:** `Admin123!`

### 5️⃣ Admin Paneline Erişin

Giriş yaptıktan sonra otomatik olarak yönlendirileceksiniz:
- **URL:** `http://localhost:5000/Admin/Dashboard`

---

## ⚙️ Yapılandırma

### Veritabanı Ayarları

`appsettings.json` dosyasında:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SoruCevapDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**SQL Server Express kullanıyorsanız:** Yukarıdaki gibi bırakın  
**LocalDB kullanıyorsanız:** Şunu kullanın:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SoruCevapDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

---

## 🐛 Yaygın Sorunlar

### "Cannot open database" Hatası
```powershell
Start-Service "MSSQL$SQLEXPRESS"
```

### Port Zaten Kullanılıyor
```bash
dotnet run --urls "http://localhost:5002"
```

### NuGet Paketleri Eksik
```bash
dotnet restore
dotnet build
```

---

## 📚 Daha Fazla Bilgi

Detaylı kılavuz için: `CALISTIRMA-KILAVUZU.md` dosyasına bakın.

---

**İyi çalışmalar! 🎉**

