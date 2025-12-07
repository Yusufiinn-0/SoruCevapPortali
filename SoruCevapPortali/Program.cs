using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    }));

// Repository Registration
builder.Services.AddScoped<IRepository<Kullanici>, Repository<Kullanici>>();
builder.Services.AddScoped<IRepository<Kategori>, Repository<Kategori>>();
builder.Services.AddScoped<IRepository<Soru>, Repository<Soru>>();
builder.Services.AddScoped<IRepository<Cevap>, Repository<Cevap>>();

// Cookie Authentication Configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Veritabanını otomatik oluştur
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Veritabanı bağlantısı kontrol ediliyor...");
        
        // Veritabanını oluştur
        context.Database.EnsureCreated();
        
        logger.LogInformation("Veritabanı başarıyla oluşturuldu!");
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "VERİTABANI OLUŞTURULAMADI!");
    logger.LogError("Lütfen SQL Server'ın kurulu ve çalışır durumda olduğundan emin olun.");
    logger.LogError("SQL Server Express indirmek için: SQL-INDIR.bat dosyasını çalıştırın");
    
    // Uygulamayı durdurma, sadece uyarı ver
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n==========================================");
    Console.WriteLine("HATA: SQL Server bağlantısı kurulamadı!");
    Console.WriteLine("==========================================");
    Console.WriteLine("Lütfen SQL Server Express'i kurun:");
    Console.WriteLine("1. SQL-INDIR.bat dosyasını çalıştırın");
    Console.WriteLine("2. SQL Server Express'i kurun");
    Console.WriteLine("3. Projeyi tekrar başlatın");
    Console.WriteLine("==========================================\n");
    Console.ResetColor();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Admin Area Route
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default Route - Giriş ekranına yönlendir
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
