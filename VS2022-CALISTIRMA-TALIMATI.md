# Visual Studio 2022'de Projeyi Çalıştırma Talimatları

## Adım 1: Visual Studio 2022'yi Açın
- Visual Studio 2022'yi başlatın

## Adım 2: Solution Dosyasını Açın
1. Visual Studio 2022'de **File > Open > Project/Solution** menüsüne tıklayın
2. Şu dosyayı seçin:
   ```
   C:\Users\yusuf\OneDrive\Masaüstü\SoruCevapPortali\SoruCevapPortali.sln
   ```
   VEYA
   - Visual Studio'yu açtıktan sonra **"Open a project or solution"** seçeneğine tıklayın
   - `SoruCevapPortali.sln` dosyasını bulup açın

## Adım 3: SQL Server'ı Kontrol Edin
Projeyi çalıştırmadan önce SQL Server Express'in çalıştığından emin olun:

### PowerShell ile Kontrol:
```powershell
Get-Service -Name "MSSQL$SQLEXPRESS"
```

### Eğer Çalışmıyorsa Başlatın:
```powershell
Start-Service -Name "MSSQL$SQLEXPRESS"
```

### Alternatif (Servisler Penceresi):
1. `Win + R` tuşlarına basın
2. `services.msc` yazın ve Enter'a basın
3. **SQL Server (SQLEXPRESS)** servisini bulun
4. Sağ tıklayıp **Start** seçeneğine tıklayın

## Adım 4: NuGet Paketlerini Geri Yükleyin (Gerekirse)
1. Visual Studio'da **Tools > NuGet Package Manager > Package Manager Console** açın
2. Şu komutu çalıştırın:
   ```
   dotnet restore
   ```

## Adım 5: Projeyi Çalıştırın

### Yöntem 1: F5 ile Çalıştırma
1. Visual Studio'da **F5** tuşuna basın
   VEYA
2. Üst menüden **Debug > Start Debugging** seçeneğine tıklayın
   VEYA
3. Yeşil **▶** (Start) butonuna tıklayın

### Yöntem 2: Ctrl+F5 ile Çalıştırma (Debug Olmadan)
1. **Ctrl + F5** tuşlarına basın
   VEYA
2. Üst menüden **Debug > Start Without Debugging** seçeneğine tıklayın

## Adım 6: Tarayıcıda Açılması
- Proje otomatik olarak tarayıcıda açılacaktır
- Eğer açılmazsa, tarayıcınızda şu adresi açın:
  ```
  http://localhost:5000
  ```
  veya
  ```
  https://localhost:5001
  ```

## Giriş Bilgileri
- **Email:** admin@admin.com
- **Şifre:** Admin123!

## Sorun Giderme

### Hata: "Cannot open database"
- SQL Server Express'in çalıştığından emin olun
- `appsettings.json` dosyasındaki connection string'i kontrol edin

### Hata: "Port already in use"
- Başka bir uygulama aynı portu kullanıyor olabilir
- Çalışan tüm Visual Studio ve dotnet process'lerini kapatın
- `Properties/launchSettings.json` dosyasında port numarasını değiştirebilirsiniz

### Hata: "Project failed to load"
- Visual Studio 2022'nin güncel olduğundan emin olun
- .NET 8.0 SDK'nın yüklü olduğundan emin olun
- Solution dosyasını yeniden oluşturun

### NuGet Paketleri Yüklenmiyor
- **Tools > NuGet Package Manager > Package Manager Settings** açın
- **Package Sources** bölümünde **nuget.org**'un aktif olduğundan emin olun
- İnternet bağlantınızı kontrol edin

## Önemli Notlar
- İlk çalıştırmada veritabanı otomatik olarak oluşturulacaktır
- Admin kullanıcı ve varsayılan kategoriler otomatik olarak eklenecektir
- Proje .NET 8.0 kullanmaktadır, Visual Studio 2022'nin .NET 8.0 SDK'sını desteklediğinden emin olun

