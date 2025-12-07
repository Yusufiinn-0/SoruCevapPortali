# SQL Server Express Kurulum Talimatları

## 🚀 Hızlı Başlangıç

**`SQL-INDIR.bat`** dosyasına çift tıklayarak indirme sayfasını açabilirsiniz!

---

## 📥 Adım 1: İndirme

1. **SQL-INDIR.bat** dosyasına çift tıklayın (veya tarayıcıda şu linki açın):
   - https://www.microsoft.com/en-us/sql-server/sql-server-downloads

2. Sayfada **"Download now"** butonuna tıklayın
   - **SQL Server 2022 Express** seçeneğini seçin (ÜCRETSİZ)

3. İndirme başlayacak (yaklaşık 5-10 dakika)

---

## 🔧 Adım 2: Kurulum

1. İndirilen dosyayı çalıştırın (genellikle `SQL2022-SSEI-Expr.exe` gibi bir isimle)

2. Kurulum sihirbazı açılacak:
   - **"Basic"** seçeneğini seçin (en kolay yol)
   - **"Accept"** butonuna tıklayın
   - Kurulumun tamamlanmasını bekleyin (10-15 dakika)

3. Kurulum tamamlandığında **"Close"** butonuna tıklayın

---

## ✅ Adım 3: Projeyi Çalıştırma

SQL Server kurulduktan sonra:

1. **PowerShell** açın (normal kullanıcı olarak yeterli)

2. Şu komutları çalıştırın:
```powershell
cd "C:\Users\yusuf\OneDrive\Masaüstü\SoruCevapPortali\SoruCevapPortali"
dotnet run
```

3. Tarayıcıda açın: **http://localhost:5000/Account/Login**

4. Giriş yapın:
   - **E-posta:** admin@sorucevap.com
   - **Şifre:** admin123

---

## 🔍 Kurulum Kontrolü

SQL Server'ın kurulup kurulmadığını kontrol etmek için:

1. **Windows Arama** → "Services" yazın
2. Açılan listede **"SQL Server (SQLEXPRESS)"** servisini arayın
3. Eğer görüyorsanız kurulum başarılı!

---

## ⚠️ Sorun Giderme

**"Cannot open database" hatası alıyorsanız:**
- SQL Server servisinin çalıştığından emin olun
- Services'te "SQL Server (SQLEXPRESS)" servisini başlatın

**"Login failed" hatası alıyorsanız:**
- Connection string'i kontrol edin
- `appsettings.json` dosyasında connection string doğru olmalı

---

## 📝 Notlar

- SQL Server Express **tamamen ücretsizdir**
- Kurulum yaklaşık 1-2 GB disk alanı kullanır
- Kurulum sırasında internet bağlantısı gereklidir
- Visual Studio 2022 kuruluysa, SQL Server LocalDB zaten yüklü olabilir

