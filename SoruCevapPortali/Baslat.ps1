# Soru Cevap Portalı - PowerShell Başlatma Scripti
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Soru Cevap Portalı - Başlatılıyor..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# SQL LocalDB kontrol et / başlat (Varsayılan: MSSQLLocalDB)
Write-Host "SQL LocalDB kontrol ediliyor..." -ForegroundColor Yellow
$sqlLocalDbCmd = Get-Command "sqllocaldb" -ErrorAction SilentlyContinue

if ($null -eq $sqlLocalDbCmd) {
    Write-Host "UYARI: sqllocaldb bulunamadı. SQL Server LocalDB kurulu değil olabilir." -ForegroundColor Red
    Write-Host "Çözüm:" -ForegroundColor Yellow
    Write-Host " - Visual Studio Installer > Individual components > 'SQL Server Express LocalDB' yükleyin" -ForegroundColor Yellow
    Write-Host " - veya SQL Server Express kurun (SQL-INDIR.bat)" -ForegroundColor Yellow
    Read-Host "Devam etmek için Enter'a basın"
    exit 1
}

try {
    # MSSQLLocalDB instance'ını başlat (varsa)
    sqllocaldb start MSSQLLocalDB | Out-Null
    Write-Host "SQL LocalDB (MSSQLLocalDB) hazır." -ForegroundColor Green
}
catch {
    Write-Host "HATA: SQL LocalDB başlatılamadı!" -ForegroundColor Red
    Write-Host "Çözüm: Visual Studio Installer'dan 'SQL Server Express LocalDB' yükleyin veya SQL Server Express kurun." -ForegroundColor Yellow
    Read-Host "Devam etmek için Enter'a basın"
    exit 1
}

Write-Host ""

# Proje dizinine git
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath
Write-Host "Proje dizini: $scriptPath" -ForegroundColor Cyan
Write-Host ""

# Çalışan dotnet process'lerini durdur
Write-Host "Eski process'ler temizleniyor..." -ForegroundColor Yellow
Get-Process -Name dotnet -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2
Write-Host ""

# Projeyi başlat
Write-Host "Proje başlatılıyor..." -ForegroundColor Yellow
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Proje çalışıyor!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Tarayıcı otomatik açılacak..." -ForegroundColor Cyan
Write-Host "Bu pencereyi KAPATMAYIN - Proje burada çalışıyor!" -ForegroundColor Yellow
Write-Host "Durdurmak için Ctrl+C tuşlarına basın veya pencereyi kapatın." -ForegroundColor Yellow
Write-Host ""

# 3 saniye bekle ve tarayıcıyı aç
Start-Sleep -Seconds 3
Start-Process "http://localhost:5000"

# Projeyi başlat
dotnet run --urls "http://localhost:5000"









