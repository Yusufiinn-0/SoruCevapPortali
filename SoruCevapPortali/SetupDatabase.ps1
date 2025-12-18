# Veritabanı Kurulum Scripti
# Bu script SQL Server LocalDB'yi başlatır ve migration'ları uygular

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Veritabanı Kurulum Scripti" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# SQL Server LocalDB'yi başlat
Write-Host "1. SQL Server LocalDB başlatılıyor..." -ForegroundColor Yellow
try {
    $localdb = sqllocaldb info MSSQLLocalDB 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "   LocalDB bulunamadı. Başlatılıyor..." -ForegroundColor Yellow
        sqllocaldb start MSSQLLocalDB
        Start-Sleep -Seconds 3
    } else {
        Write-Host "   LocalDB zaten çalışıyor." -ForegroundColor Green
    }
} catch {
    Write-Host "   UYARI: LocalDB komutu bulunamadı. SQL Server LocalDB kurulu olmayabilir." -ForegroundColor Red
    Write-Host "   Visual Studio Installer > Individual components > 'SQL Server Express LocalDB' yükleyin" -ForegroundColor Yellow
}

Write-Host ""

# Migration'ları uygula
Write-Host "2. Migration'lar uygulanıyor..." -ForegroundColor Yellow
try {
    dotnet ef database update
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   Migration'lar başarıyla uygulandı!" -ForegroundColor Green
    } else {
        Write-Host "   HATA: Migration uygulanırken hata oluştu." -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "   HATA: Migration komutu çalıştırılamadı." -ForegroundColor Red
    Write-Host "   Lütfen 'dotnet tool install --global dotnet-ef' komutunu çalıştırın." -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Veritabanı kurulumu tamamlandı!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Varsayılan Admin Kullanıcısı:" -ForegroundColor Yellow
Write-Host "  Email: admin@admin.com" -ForegroundColor White
Write-Host "  Password: Admin123!" -ForegroundColor White
Write-Host ""

