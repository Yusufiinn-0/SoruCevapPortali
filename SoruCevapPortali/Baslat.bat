@echo off
chcp 65001 >nul
echo ========================================
echo Soru Cevap Portalı - Başlatılıyor...
echo ========================================
echo.

REM SQL Server Express servisini kontrol et
echo SQL Server Express kontrol ediliyor...
sc query MSSQL$SQLEXPRESS | find "RUNNING" >nul
if %errorlevel% neq 0 (
    echo SQL Server Express servisi çalışmıyor! Başlatılıyor...
    net start MSSQL$SQLEXPRESS
    if %errorlevel% neq 0 (
        echo HATA: SQL Server Express başlatılamadı!
        echo Lütfen SQL Server Express'in kurulu olduğundan emin olun.
        pause
        exit /b 1
    )
    echo SQL Server Express başlatıldı.
) else (
    echo SQL Server Express çalışıyor.
)
echo.

REM Proje dizinine git
cd /d "%~dp0"
echo Proje dizini: %CD%
echo.

REM Projeyi başlat
echo Proje başlatılıyor...
echo.
echo ========================================
echo Proje çalışıyor!
echo ========================================
echo.
echo Tarayıcı otomatik açılacak...
echo Bu pencereyi KAPATMAYIN - Proje burada çalışıyor!
echo Durdurmak için bu pencereyi kapatın veya Ctrl+C tuşlarına basın.
echo.

REM 3 saniye bekle ve tarayıcıyı aç
timeout /t 3 /nobreak >nul
start http://localhost:5000

REM Projeyi başlat
dotnet run --urls "http://localhost:5000"


