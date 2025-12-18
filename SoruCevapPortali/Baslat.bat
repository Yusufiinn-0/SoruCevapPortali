@echo off
chcp 65001 >nul
echo ========================================
echo Soru Cevap Portalı - Başlatılıyor...
echo ========================================
echo.

REM SQL LocalDB kontrol et / başlat (Varsayılan: MSSQLLocalDB)
echo SQL LocalDB kontrol ediliyor...
where sqllocaldb >nul 2>nul
if %errorlevel% neq 0 (
    echo HATA: sqllocaldb bulunamadi. SQL Server LocalDB kurulu degil olabilir.
    echo Cozum:
    echo  - Visual Studio Installer ^> Individual components ^> 'SQL Server Express LocalDB' yukleyin
    echo  - veya SQL Server Express kurun (SQL-INDIR.bat)
    pause
    exit /b 1
)

sqllocaldb start MSSQLLocalDB >nul 2>nul
if %errorlevel% neq 0 (
    echo HATA: SQL LocalDB (MSSQLLocalDB) baslatilamadi!
    echo Visual Studio Installer'dan 'SQL Server Express LocalDB' yukleyin veya SQL Server Express kurun.
    pause
    exit /b 1
)

echo SQL LocalDB (MSSQLLocalDB) hazir.
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


