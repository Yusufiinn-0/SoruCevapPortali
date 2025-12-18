using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Repository;
using SoruCevapPortali.Hubs;

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

// Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Cookie Authentication Configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Repository Registration
builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IRepository<Question>, Repository<Question>>();
builder.Services.AddScoped<IRepository<Answer>, Repository<Answer>>();

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Veritabanını oluştur ve seed data ekle (Code-First Migration)
try
{
    await using (var scope = app.Services.CreateAsyncScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Veritabanı bağlantısı kontrol ediliyor...");
        
        // Code-First Migration yaklaşımı - Yönergede istenen şekilde
        try
        {
            // Pending migration'ları uygula
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation("Pending migration'lar uygulanıyor...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Migration'lar başarıyla uygulandı.");
            }
            else
            {
                logger.LogInformation("Tüm migration'lar güncel.");
            }
        }
        catch (Exception migrationEx)
        {
            logger.LogWarning(migrationEx, "Migration uygulanırken hata oluştu. Veritabanı bağlantısı kontrol ediliyor...");
            
            // Veritabanı bağlantısını test et
            if (!await context.Database.CanConnectAsync())
            {
                throw new Exception("Veritabanına bağlanılamıyor. Lütfen SQL Server'ın çalıştığından emin olun.");
            }
            
            // Veritabanı var ama migration uygulanamıyorsa, tabloları kontrol et
            try
            {
                _ = await context.Categories.AnyAsync();
                logger.LogInformation("Veritabanı şeması mevcut.");
            }
            catch
            {
                logger.LogWarning("Veritabanı şeması eksik. Migration'ları manuel olarak uygulayın: dotnet ef database update");
                throw;
            }
        }

        // Seed Data - Kategoriler (Migration'da HasData ile zaten var ama yine de kontrol edelim)
        if (!await context.Categories.AnyAsync())
        {
            logger.LogInformation("Varsayılan kategoriler ekleniyor...");
            context.Categories.AddRange(
                new Category { CategoryName = "Genel", Description = "Genel konular", Icon = "fa-globe", OrderNumber = 1, IsActive = true },
                new Category { CategoryName = "Teknoloji", Description = "Teknoloji ile ilgili sorular", Icon = "fa-laptop", OrderNumber = 2, IsActive = true },
                new Category { CategoryName = "Yazılım", Description = "Yazılım geliştirme soruları", Icon = "fa-code", OrderNumber = 3, IsActive = true },
                new Category { CategoryName = "Eğitim", Description = "Eğitim ile ilgili sorular", Icon = "fa-graduation-cap", OrderNumber = 4, IsActive = true },
                new Category { CategoryName = "Sağlık", Description = "Sağlık ile ilgili sorular", Icon = "fa-heartbeat", OrderNumber = 5, IsActive = true }
            );
            await context.SaveChangesAsync();
            logger.LogInformation("Varsayılan kategoriler eklendi.");
        }
        
        // Rol oluştur
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            logger.LogInformation("Admin rolü oluşturuldu.");
        }
        
        // Admin kullanıcısı oluştur
        var adminEmail = "admin@admin.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                IsActive = true,
                RegistrationDate = DateTime.Now,
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                logger.LogInformation("Admin kullanıcısı oluşturuldu. Email: {Email}, Password: Admin123!", adminEmail);
            }
            else
            {
                logger.LogError("Admin kullanıcısı oluşturulamadı: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        
        logger.LogInformation("Veritabanı hazır!");
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "VERİTABANI OLUŞTURULAMADI!");
    logger.LogError("Hata Detayı: {Message}", ex.Message);
    
    // Daha detaylı hata mesajı
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n==========================================");
    Console.WriteLine("HATA: Veritabanı bağlantısı kurulamadı!");
    Console.WriteLine("==========================================");
    Console.WriteLine($"Hata: {ex.Message}");
    Console.WriteLine("\nÇözüm Adımları:");
    Console.WriteLine("1. SQL Server LocalDB veya Express'in kurulu olduğundan emin olun");
    Console.WriteLine("2. LocalDB başlatmak için: sqllocaldb start MSSQLLocalDB");
    Console.WriteLine("3. Migration'ları manuel uygulamak için:");
    Console.WriteLine("   cd SoruCevapPortali");
    Console.WriteLine("   dotnet ef database update");
    Console.WriteLine("4. Alternatif: Visual Studio Installer > Individual components > 'SQL Server Express LocalDB'");
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

// SignalR Hub Mapping
app.MapHub<NotificationHub>("/notificationHub");

// Admin Area Route
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default Route - Giriş ekranına yönlendir
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
