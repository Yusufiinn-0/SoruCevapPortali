@echo off
echo LocalDB baslatiliyor...
sqllocaldb start MSSQLLocalDB
echo.
echo Proje baslatiliyor...
dotnet run --urls "http://localhost:5000"
pause


