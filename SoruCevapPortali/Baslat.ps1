# Soru Cevap Portalı - PowerShell Başlatma Scripti
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Soru Cevap Portalı - Başlatılıyor..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# SQL Server Express servisini kontrol et
Write-Host "SQL Server Express kontrol ediliyor..." -ForegroundColor Yellow
$sqlService = Get-Service -Name "MSSQL*SQLEXPRESS" -ErrorAction SilentlyContinue

if ($null -eq $sqlService -or $sqlService.Status -ne "Running") {
    Write-Host "SQL Server Express servisi çalışmıyor! Başlatılıyor..." -ForegroundColor Yellow
    try {
        Start-Service -Name "MSSQL`$SQLEXPRESS" -ErrorAction Stop
        Write-Host "SQL Server Express başlatıldı." -ForegroundColor Green
    }
    catch {
        Write-Host "HATA: SQL Server Express başlatılamadı!" -ForegroundColor Red
        Write-Host "Lütfen SQL Server Express'in kurulu olduğundan emin olun." -ForegroundColor Red
        Read-Host "Devam etmek için Enter'a basın"
        exit 1
    }
}
else {
    Write-Host "SQL Server Express çalışıyor." -ForegroundColor Green
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


