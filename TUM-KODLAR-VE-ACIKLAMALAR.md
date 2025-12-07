# 📚 TÜM KODLAR VE AÇIKLAMALAR

Bu dosya projedeki tüm kodların detaylı açıklamalarını içerir.

---

## 📄 1. Program.cs - Ana Başlangıç Dosyası

**Konum:** `SoruCevapPortali/Program.cs`

**İşlevi:** Uygulamanın başlangıç noktası, tüm servisleri yapılandırır ve middleware'leri ayarlar.

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SoruCevapPortali.Models.Context;
using SoruCevapPortali.Models.Entity;
using SoruCevapPortali.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
```
**Açıklama:** MVC pattern için gerekli servisleri ekler.

```csharp
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
```
**Açıklama:** 
- SQL Server bağlantısını yapılandırır
- Bağlantı koparsa 3 kez yeniden dener (her denemede 5 saniye bekler)

```csharp
// Repository Registration
builder.Services.AddScoped<IRepository<Kullanici>, Repository<Kullanici>>();
builder.Services.AddScoped<IRepository<Kategori>, Repository<Kategori>>();
builder.Services.AddScoped<IRepository<Soru>, Repository<Soru>>();
builder.Services.AddScoped<IRepository<Cevap>, Repository<Cevap>>();
```
**Açıklama:** Dependency Injection ile Repository pattern'i kaydeder. Her HTTP isteğinde yeni bir instance oluşturulur (Scoped).

```csharp
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
```
**Açıklama:**
- Cookie tabanlı kimlik doğrulama ayarlar
- 7 gün oturum süresi
- Sliding expiration: Her istekte oturum süresi yenilenir
- HttpOnly: JavaScript ile cookie'ye erişilemez (güvenlik)
- SecurePolicy: HTTPS zorunluluğu (production'da)

```csharp
// Veritabanını Migration ile oluştur
try
{
    await using (var scope = app.Services.CreateAsyncScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        context.Database.Migrate();
        logger.LogInformation("Veritabanı migration'ları başarıyla uygulandı!");
    }
}
```
**Açıklama:**
- Uygulama başlarken otomatik migration çalıştırır
- Veritabanı yoksa oluşturur, varsa günceller
- HasData seed data'sını ekler (admin kullanıcısı, kategoriler)

```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
```
**Açıklama:**
- Middleware sırası önemli!
- UseHttpsRedirection: HTTP'yi HTTPS'ye yönlendirir
- UseStaticFiles: wwwroot klasöründeki statik dosyaları (CSS, JS) servis eder
- UseRouting: Routing sistemini aktif eder
- UseAuthentication: Kimlik doğrulama kontrolü yapar
- UseAuthorization: Yetkilendirme kontrolü yapar (Authentication'dan sonra gelmeli)

```csharp
// Admin Area Route
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default Route - Giriş ekranına yönlendir
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
```
**Açıklama:**
- İlk route: Admin area için (örn: /Admin/Dashboard)
- İkinci route: Varsayılan route, Account/Login'e yönlendirir

---

## 📄 2. AppDbContext.cs - Veritabanı Bağlamı

**Konum:** `SoruCevapPortali/Models/Context/AppDbContext.cs`

**İşlevi:** Entity Framework ile veritabanı işlemlerini yönetir, tablo ilişkilerini tanımlar.

```csharp
public DbSet<Kullanici> Kullanicilar { get; set; }
public DbSet<Kategori> Kategoriler { get; set; }
public DbSet<Soru> Sorular { get; set; }
public DbSet<Cevap> Cevaplar { get; set; }
```
**Açıklama:** Her DbSet bir veritabanı tablosunu temsil eder.

```csharp
// Kullanici tablosu için unique email constraint
modelBuilder.Entity<Kullanici>()
    .HasIndex(k => k.Email)
    .IsUnique();
```
**Açıklama:** Email alanı unique (tekil) olmalı, aynı email ile iki kullanıcı olamaz.

```csharp
// Soru - Kullanici ilişkisi
modelBuilder.Entity<Soru>()
    .HasOne(s => s.Kullanici)
    .WithMany(k => k.Sorular)
    .HasForeignKey(s => s.KullaniciId)
    .OnDelete(DeleteBehavior.Restrict);
```
**Açıklama:**
- Bir soru bir kullanıcıya aittir (HasOne)
- Bir kullanıcının birden fazla sorusu olabilir (WithMany)
- Kullanıcı silinmeye çalışılırsa, soruları varsa silme işlemi engellenir (Restrict)

```csharp
// Cevap - Soru ilişkisi
modelBuilder.Entity<Cevap>()
    .HasOne(c => c.Soru)
    .WithMany(s => s.Cevaplar)
    .HasForeignKey(c => c.SoruId)
    .OnDelete(DeleteBehavior.Cascade);
```
**Açıklama:** Soru silinirse, cevapları da otomatik silinir (Cascade).

```csharp
// Varsayılan admin kullanıcısı
modelBuilder.Entity<Kullanici>().HasData(
    new Kullanici
    {
        KullaniciId = 1,
        Ad = "Admin",
        Soyad = "User",
        Email = "admin@admin.com",
        Sifre = "Admin123!",
        AdminMi = true,
        AktifMi = true,
        KayitTarihi = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
    }
);
```
**Açıklama:** Migration ile veritabanına eklenen varsayılan veri (seed data).

---

## 📄 3. Entity Sınıfları - Veritabanı Tabloları

### 3.1. Kullanici.cs

**Konum:** `SoruCevapPortali/Models/Entity/Kullanici.cs`

**İşlevi:** Kullanıcı bilgilerini tutar.

```csharp
[Key]
public int KullaniciId { get; set; }
```
**Açıklama:** Primary key, veritabanında otomatik artan ID.

```csharp
[Required(ErrorMessage = "Ad alanı zorunludur")]
[StringLength(50)]
[Display(Name = "Ad")]
public string Ad { get; set; } = string.Empty;
```
**Açıklama:**
- Required: Zorunlu alan
- StringLength: Maksimum 50 karakter
- Display: View'da görünen isim

```csharp
[EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
public string Email { get; set; } = string.Empty;
```
**Açıklama:** Email formatı kontrolü yapar.

```csharp
public bool AktifMi { get; set; } = true;
public bool AdminMi { get; set; } = false;
```
**Açıklama:**
- AktifMi: Kullanıcı aktif mi pasif mi?
- AdminMi: Admin yetkisi var mı?

```csharp
// Navigation Properties
public virtual ICollection<Soru>? Sorular { get; set; }
public virtual ICollection<Cevap>? Cevaplar { get; set; }
```
**Açıklama:** 
- Bir kullanıcının tüm sorularını ve cevaplarını getirmek için
- Lazy loading için virtual keyword'ü kullanılır
- ? (nullable) çünkü başlangıçta null olabilir

### 3.2. Kategori.cs

**Konum:** `SoruCevapPortali/Models/Entity/Kategori.cs`

**İşlevi:** Soru kategorilerini tutar.

```csharp
public string? Ikon { get; set; }
```
**Açıklama:** FontAwesome ikon adı (örn: "fa-globe", "fa-laptop").

```csharp
public int SiraNo { get; set; } = 0;
```
**Açıklama:** Kategorilerin listelenme sırası.

### 3.3. Soru.cs

**Konum:** `SoruCevapPortali/Models/Entity/Soru.cs`

**İşlevi:** Kullanıcıların sorduğu soruları tutar.

```csharp
public bool OnayliMi { get; set; } = false;
```
**Açıklama:** Admin onayı bekleyen sorular için.

```csharp
public int GoruntulenmeSayisi { get; set; } = 0;
```
**Açıklama:** Sorunun kaç kez görüntülendiğini tutar.

```csharp
[ForeignKey("KategoriId")]
public virtual Kategori? Kategori { get; set; }
```
**Açıklama:** Sorunun hangi kategoriye ait olduğunu belirtir.

### 3.4. Cevap.cs

**Konum:** `SoruCevapPortali/Models/Entity/Cevap.cs`

**İşlevi:** Sorulara verilen cevapları tutar.

```csharp
public bool DogruCevapMi { get; set; } = false;
```
**Açıklama:** En doğru cevap olarak işaretlenebilir.

```csharp
public int BegeniSayisi { get; set; } = 0;
```
**Açıklama:** Cevabın beğenilme sayısı.

---

## 📄 4. Repository Pattern

### 4.1. IRepository.cs - Interface

**Konum:** `SoruCevapPortali/Repository/IRepository.cs`

**İşlevi:** Veritabanı işlemleri için genel interface.

```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync();
}
```
**Açıklama:**
- Generic interface: Her entity için aynı interface kullanılır
- Async metotlar: Performans için asenkron işlemler
- GetByIdAsync: ID'ye göre tek kayıt getirir
- Update/Delete: Senkron (değişiklikleri takip eder)
- SaveAsync: Değişiklikleri veritabanına kaydeder

### 4.2. Repository.cs - Implementation

**Konum:** `SoruCevapPortali/Repository/Repository.cs`

**İşlevi:** IRepository interface'ini uygular.

```csharp
private readonly AppDbContext _context;
private readonly DbSet<T> _dbSet;

public Repository(AppDbContext context)
{
    _context = context;
    _dbSet = context.Set<T>();
}
```
**Açıklama:** 
- Constructor injection ile AppDbContext alınır
- DbSet<T> generic olarak ayarlanır

```csharp
public async Task<IEnumerable<T>> GetAllAsync()
{
    return await _dbSet.ToListAsync();
}
```
**Açıklama:** Tüm kayıtları asenkron olarak getirir.

```csharp
public async Task<T?> GetByIdAsync(int id)
{
    return await _dbSet.FindAsync(id);
}
```
**Açıklama:** Primary key'e göre kayıt bulur (FindAsync en hızlı yöntem).

```csharp
public void Update(T entity)
{
    _dbSet.Update(entity);
}
```
**Açıklama:** 
- Senkron çünkü sadece Entity Framework'e "bu değişti" der
- Asıl kayıt SaveAsync'te yapılır

```csharp
public async Task SaveAsync()
{
    await _context.SaveChangesAsync();
}
```
**Açıklama:** Tüm değişiklikleri tek seferde veritabanına yazar.

---

## 📄 5. Controllers

### 5.1. AccountController.cs - Giriş/Çıkış İşlemleri

**Konum:** `SoruCevapPortali/Controllers/AccountController.cs`

```csharp
[HttpGet]
public IActionResult Login(string? returnUrl = null)
{
    if (User.Identity?.IsAuthenticated == true)
    {
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
    return View();
}
```
**Açıklama:**
- Zaten giriş yapmışsa yönlendirir
- Admin ise admin paneline, değilse home'a gider

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
{
    var kullanici = await _kullaniciRepository.GetFirstOrDefaultAsync(
        k => k.Email == model.Email && k.Sifre == model.Sifre && k.AktifMi);

    if (kullanici != null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciId.ToString()),
            new Claim(ClaimTypes.Name, $"{kullanici.Ad} {kullanici.Soyad}"),
            new Claim(ClaimTypes.Email, kullanici.Email),
            new Claim("AdminMi", kullanici.AdminMi.ToString())
        };

        if (kullanici.AdminMi)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }
```
**Açıklama:**
- Email ve şifre kontrolü
- Aktif kullanıcı kontrolü
- Claims (kullanıcı bilgileri) oluşturulur
- Admin ise "Admin" rolü eklenir

```csharp
await HttpContext.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme,
    new ClaimsPrincipal(claimsIdentity),
    authProperties);
```
**Açıklama:** Cookie'ye kullanıcı bilgileri kaydedilir, tarayıcı cookie'yi saklar.

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Logout()
{
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Login", "Account");
}
```
**Açıklama:** Cookie'yi siler ve giriş sayfasına yönlendirir.

### 5.2. DashboardController.cs - Admin Panel Ana Sayfası

**Konum:** `SoruCevapPortali/Areas/Admin/Controllers/DashboardController.cs`

```csharp
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
```
**Açıklama:**
- Area attribute: Admin area'sında olduğunu belirtir
- Authorize: Sadece Admin rolündeki kullanıcılar erişebilir

```csharp
var model = new DashboardViewModel
{
    ToplamKullanici = await _kullaniciRepository.CountAsync(),
    ToplamSoru = await _soruRepository.CountAsync(),
    ToplamCevap = await _cevapRepository.CountAsync(),
    ToplamKategori = await _kategoriRepository.CountAsync(),
    BekleyenSorular = await _soruRepository.CountAsync(s => !s.OnayliMi),
    BekleyenCevaplar = await _cevapRepository.CountAsync(c => !c.OnayliMi)
};
```
**Açıklama:** Dashboard istatistiklerini toplar.

---

## 📄 6. ViewModels - View İçin Model'ler

### 6.1. LoginViewModel.cs

**Konum:** `SoruCevapPortali/Models/ViewModel/LoginViewModel.cs`

```csharp
[Required(ErrorMessage = "E-posta alanı zorunludur")]
[EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
public string Email { get; set; } = string.Empty;

[Required(ErrorMessage = "Şifre alanı zorunludur")]
[DataType(DataType.Password)]
public string Sifre { get; set; } = string.Empty;

[Display(Name = "Beni Hatırla")]
public bool BeniHatirla { get; set; }
```
**Açıklama:**
- View'dan gelen veriler için model
- Validation attribute'ları form validasyonu için
- DataType.Password: Şifre alanını gizler (****)

---

## 📄 7. AJAX İşlemleri

### 7.1. KategoriController - ToggleStatus

**Konum:** `SoruCevapPortali/Areas/Admin/Controllers/KategoriController.cs`

```csharp
[HttpPost]
public async Task<IActionResult> ToggleStatus(int id)
{
    var kategori = await _kategoriRepository.GetByIdAsync(id);
    if (kategori == null)
    {
        return Json(new { success = false, message = "Kategori bulunamadı." });
    }

    kategori.AktifMi = !kategori.AktifMi;
    _kategoriRepository.Update(kategori);
    await _kategoriRepository.SaveAsync();

    return Json(new { success = true, aktifMi = kategori.AktifMi, message = "Durum değiştirildi." });
}
```
**Açıklama:**
- AJAX için JSON döner
- Sayfa yenilenmeden durum değişir
- Aktif/Pasif durumunu tersine çevirir

### 7.2. View Tarafında AJAX Kullanımı

```javascript
function toggleStatus(id) {
    $.post('@Url.Action("ToggleStatus")', { id: id }, function(response) {
        if (response.success) {
            var statusBadge = $('#status-' + id);
            if (response.aktifMi) {
                statusBadge.removeClass('badge-inactive').addClass('badge-active').text('Aktif');
            } else {
                statusBadge.removeClass('badge-active').addClass('badge-inactive').text('Pasif');
            }
            alert(response.message);
        }
    });
}
```
**Açıklama:**
- jQuery $.post ile AJAX isteği gönderilir
- Response'a göre DOM güncellenir
- Sayfa yenilenmeden değişiklik görülür

---

## 🔑 Önemli Kavramlar

### 1. Dependency Injection (DI)
```csharp
// Program.cs'de kayıt
builder.Services.AddScoped<IRepository<Kullanici>, Repository<Kullanici>>();

// Controller'da kullanım
public AccountController(IRepository<Kullanici> kullaniciRepository)
{
    _kullaniciRepository = kullaniciRepository;
}
```
**Açıklama:** Framework otomatik olarak bağımlılıkları enjekte eder.

### 2. Async/Await
```csharp
public async Task<IActionResult> Index()
{
    var kategoriler = await _kategoriRepository.GetAllAsync();
    return View(kategoriler);
}
```
**Açıklama:** Asenkron işlemler sayesinde thread bloklanmaz, performans artar.

### 3. LINQ
```csharp
var aktifKategoriler = await _kategoriRepository.GetAllAsync(k => k.AktifMi == true);
```
**Açıklama:** Veritabanı sorgularını C# kodu ile yazar.

### 4. Navigation Properties
```csharp
var soru = await _soruRepository.GetQueryable()
    .Include(s => s.Kullanici)
    .Include(s => s.Kategori)
    .FirstOrDefaultAsync(s => s.SoruId == id);
```
**Açıklama:** Include ile ilişkili tabloları tek sorguda getirir.

---

## 🎯 Kullanım Örnekleri

### Örnek 1: Yeni Kategori Ekleme
```csharp
[HttpPost]
public async Task<IActionResult> Create(KategoriViewModel model)
{
    if (ModelState.IsValid)
    {
        var kategori = new Kategori
        {
            KategoriAdi = model.KategoriAdi,
            Aciklama = model.Aciklama,
            Ikon = model.Ikon,
            AktifMi = model.AktifMi,
            SiraNo = model.SiraNo
        };
        
        await _kategoriRepository.AddAsync(kategori);
        await _kategoriRepository.SaveAsync();
        
        TempData["Success"] = "Kategori başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }
    return View(model);
}
```

### Örnek 2: Kullanıcı Listeleme
```csharp
public async Task<IActionResult> Index()
{
    var kullanicilar = await _kullaniciRepository.GetAllAsync();
    var model = kullanicilar.Select(k => new KullaniciViewModel
    {
        KullaniciId = k.KullaniciId,
        Ad = k.Ad,
        Soyad = k.Soyad,
        Email = k.Email,
        AktifMi = k.AktifMi,
        AdminMi = k.AdminMi
    }).ToList();
    
    return View(model);
}
```

---

## 📝 Özet

1. **Program.cs**: Uygulama yapılandırması, servis kayıtları, middleware
2. **AppDbContext**: Veritabanı bağlamı, ilişkiler, seed data
3. **Entity'ler**: Veritabanı tablolarını temsil eden sınıflar
4. **Repository**: Veritabanı işlemlerini soyutlar
5. **Controllers**: HTTP isteklerini işler, View'lara veri gönderir
6. **ViewModels**: View için özel modeller
7. **AJAX**: Sayfa yenilenmeden veri güncelleme

Her dosya belirli bir sorumluluğa sahiptir (Single Responsibility Principle).

---

## 📄 8. Admin Controller'lar - Detaylı Açıklamalar

### 8.1. KategoriController.cs

**Konum:** `SoruCevapPortali/Areas/Admin/Controllers/KategoriController.cs`

**İşlevi:** Kategori CRUD işlemleri ve AJAX işlemleri.

#### Index Action - Liste Görüntüleme
```csharp
public async Task<IActionResult> Index()
{
    var kategoriler = await _kategoriRepository.GetQueryable()
        .OrderBy(k => k.SiraNo)
        .Select(k => new KategoriViewModel
        {
            KategoriId = k.KategoriId,
            KategoriAdi = k.KategoriAdi,
            // ...
        })
        .ToListAsync();
    return View(kategoriler);
}
```
**Açıklama:**
- GetQueryable(): IQueryable döner, daha esnek sorgu yazılabilir
- OrderBy: Sıra numarasına göre sıralar
- Select: Sadece gerekli alanları ViewModel'e map eder
- ToListAsync(): Asenkron olarak listeye çevirir

#### Create Action - Yeni Kategori Ekleme
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(KategoriViewModel model)
{
    if (ModelState.IsValid)
    {
        // Email benzersizlik kontrolü
        var existing = await _kategoriRepository.AnyAsync(k => k.KategoriAdi == model.KategoriAdi);
        if (existing)
        {
            ModelState.AddModelError("KategoriAdi", "Bu kategori adı zaten kullanılıyor.");
            return View(model);
        }
        
        var kategori = new Kategori { /* ... */ };
        await _kategoriRepository.AddAsync(kategori);
        await _kategoriRepository.SaveAsync();
        
        TempData["Success"] = "Kategori başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }
    return View(model);
}
```
**Açıklama:**
- ModelState.IsValid: Validation kontrollerini yapar
- TempData: Bir sonraki sayfaya mesaj taşır (Redirect sonrası)
- nameof(Index): Refactoring-safe, Index metodu adı değişirse hata verir

#### ToggleStatus Action - AJAX ile Durum Değiştirme
```csharp
[HttpPost]
public async Task<IActionResult> ToggleStatus(int id)
{
    var kategori = await _kategoriRepository.GetByIdAsync(id);
    kategori.AktifMi = !kategori.AktifMi;
    _kategoriRepository.Update(kategori);
    await _kategoriRepository.SaveAsync();
    
    return Json(new { success = true, aktifMi = kategori.AktifMi });
}
```
**Açıklama:**
- Json döner: AJAX için
- ! operatörü: Tersine çevirir (true → false, false → true)
- Update + SaveAsync: Değişikliği kaydeder

### 8.2. KullaniciController.cs

**Konum:** `SoruCevapPortali/Areas/Admin/Controllers/KullaniciController.cs`

#### ToggleAdmin Action - Admin Yetkisi Verme/Alma
```csharp
[HttpPost]
public async Task<IActionResult> ToggleAdmin(int id)
{
    var kullanici = await _kullaniciRepository.GetByIdAsync(id);
    
    // Ana admin'in yetkisi değiştirilemez
    if (kullanici.KullaniciId == 1)
    {
        return Json(new { success = false, message = "Ana admin yetkisi değiştirilemez." });
    }
    
    kullanici.AdminMi = !kullanici.AdminMi;
    _kullaniciRepository.Update(kullanici);
    await _kullaniciRepository.SaveAsync();
    
    return Json(new { success = true, adminMi = kullanici.AdminMi });
}
```
**Açıklama:**
- Güvenlik kontrolü: ID=1 (ana admin) korunur
- AJAX ile sayfa yenilenmeden yetki değişir

---

## 📄 9. View'lar - Razor Pages

### 9.1. View Yapısı

**Örnek:** `Areas/Admin/Views/Kategori/Index.cshtml`

```razor
@model List<KategoriViewModel>
@{
    ViewData["Title"] = "Kategoriler";
}
```
**Açıklama:**
- @model: Controller'dan gelen model tipi
- ViewData: Sayfa başlığı gibi veriler

```razor
@if (Model.Any())
{
    @foreach (var kategori in Model)
    {
        <tr>
            <td>@kategori.KategoriAdi</td>
        </tr>
    }
}
```
**Açıklama:**
- @: Razor syntax
- @foreach: Döngü
- @kategori.KategoriAdi: Model özelliğini yazdırır

### 9.2. AJAX JavaScript

```javascript
function toggleStatus(id) {
    $.post('@Url.Action("ToggleStatus")', { id: id }, function(response) {
        if (response.success) {
            // DOM manipülasyonu
            $('#status-' + id).text(response.aktifMi ? 'Aktif' : 'Pasif');
        }
    });
}
```
**Açıklama:**
- @Url.Action: Razor ile action URL'ini oluşturur
- $.post: jQuery AJAX POST isteği
- DOM manipülasyonu: jQuery ile HTML güncelleme

---

## 📄 10. Migration Dosyaları

### 10.1. InitialCreate.cs

**Konum:** `SoruCevapPortali/Models/Migrations/20241207210400_InitialCreate.cs`

**İşlevi:** İlk veritabanı şemasını oluşturur.

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "Kullanicilar",
        columns: table => new
        {
            KullaniciId = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
            // ...
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
        });
}
```
**Açıklama:**
- Up(): Migration uygulanırken çalışır (veritabanı oluşturma)
- Down(): Migration geri alınırken çalışır (veritabanı silme)
- SqlServer:Identity: Otomatik artan ID

```csharp
// Seed Data - Admin Kullanıcısı
migrationBuilder.InsertData(
    table: "Kullanicilar",
    columns: new[] { "KullaniciId", "Ad", "Email", ... },
    values: new object[] { 1, "Admin", "admin@admin.com", ... });
```
**Açıklama:** Migration ile varsayılan veriler eklenir.

---

## 🔍 Kod Akışı Örnekleri

### Örnek 1: Kullanıcı Girişi Akışı

1. **Kullanıcı:** Tarayıcıda `http://localhost:5000` açar
2. **Routing:** Program.cs'deki default route → `Account/Login`
3. **Controller:** `AccountController.Login()` (GET) çalışır
4. **View:** `Views/Account/Login.cshtml` render edilir
5. **Kullanıcı:** Email/şifre girer, form submit eder
6. **Controller:** `AccountController.Login()` (POST) çalışır
7. **Repository:** `_kullaniciRepository.GetFirstOrDefaultAsync()` kullanıcıyı arar
8. **Claims:** Kullanıcı bilgileri Claims'e dönüştürülür
9. **Cookie:** `SignInAsync()` ile cookie oluşturulur
10. **Redirect:** Admin paneline yönlendirilir

### Örnek 2: AJAX İşlemi Akışı

1. **Kullanıcı:** Kategori listesinde "Durum Değiştir" butonuna tıklar
2. **JavaScript:** `toggleStatus(5)` fonksiyonu çalışır
3. **AJAX:** jQuery `$.post()` ile POST isteği gönderilir
4. **Controller:** `KategoriController.ToggleStatus(5)` çalışır
5. **Repository:** Kategori getirilir, durum değiştirilir, kaydedilir
6. **Response:** JSON döner `{ success: true, aktifMi: false }`
7. **JavaScript:** DOM güncellenir (badge rengi değişir)
8. **Kullanıcı:** Sayfa yenilenmeden değişikliği görür

---

## 🎓 Öğrenilen Kavramlar

### 1. MVC Pattern
- **Model:** Entity, ViewModel sınıfları
- **View:** Razor (.cshtml) dosyaları
- **Controller:** Controller sınıfları

### 2. Dependency Injection
- Servisler Program.cs'de kaydedilir
- Constructor'da otomatik enjekte edilir
- Test edilebilirlik artar

### 3. Repository Pattern
- Veritabanı işlemlerini soyutlar
- Kod tekrarını azaltır
- Değişikliklere karşı esnek

### 4. Entity Framework Core
- Code-First yaklaşımı
- Migration ile veritabanı yönetimi
- LINQ ile veri sorgulama

### 5. Authentication & Authorization
- Cookie tabanlı kimlik doğrulama
- Role-based yetkilendirme
- Claims ile kullanıcı bilgileri

### 6. AJAX
- Sayfa yenilenmeden veri güncelleme
- jQuery ile kolay implementasyon
- JSON ile veri alışverişi

---

## ✅ Sonuç

Bu proje şu kavramları içerir:
- ✅ MVC Pattern
- ✅ Entity Framework Core
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ Authentication & Authorization
- ✅ AJAX Operations
- ✅ Code-First Migrations
- ✅ LINQ Queries
- ✅ Razor Views
- ✅ jQuery

Her kod parçası belirli bir görevi yerine getirir ve birbiriyle uyumlu çalışır.

