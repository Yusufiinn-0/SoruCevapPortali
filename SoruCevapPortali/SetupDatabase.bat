@echo off
echo ========================================
echo Veritabanı Kurulum Scripti
echo ========================================
echo.

echo 1. SQL Server LocalDB başlatılıyor...
sqllocaldb start MSSQLLocalDB
if %errorlevel% neq 0 (
    echo UYARI: LocalDB başlatılamadı. SQL Server LocalDB kurulu olmayabilir.
    echo Visual Studio Installer ^> Individual components ^> 'SQL Server Express LocalDB' yükleyin
    pause
    exit /b 1
)

echo.
echo 2. Migration'lar uygulanıyor...
dotnet ef database update
if %errorlevel% neq 0 (
    echo HATA: Migration uygulanırken hata oluştu.
    echo Lütfen 'dotnet tool install --global dotnet-ef' komutunu çalıştırın.
    pause
    exit /b 1
)

echo.
echo ========================================
echo Veritabanı kurulumu tamamlandı!
echo ========================================
echo.
echo Varsayılan Admin Kullanıcısı:
echo   Email: admin@admin.com
echo   Password: Admin123!
echo.
pause

