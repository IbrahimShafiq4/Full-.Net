# 10 — Cheat Sheet ⚡ مرجع سريع

> **"الصفحة دي هي صاحبتك في الـ Interview وأثناء الكود!"**

---

## 🏗️ Program.cs Template

```csharp
var builder = WebApplication.CreateBuilder(args);

// ─── Services ───────────────────────────────────────
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSession();

// ─── Build ──────────────────────────────────────────
var app = builder.Build();

// ─── Middleware Pipeline (الترتيب مهم!) ─────────────
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();   // قبل Authorization!
app.UseAuthorization();

// ─── Routes ─────────────────────────────────────────
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
```

---

## 🎭 Controller Template

```csharp
[Route("products")]
public class ProductsController : Controller
{
    private readonly IProductService _svc;
    private readonly ILogger<ProductsController> _log;

    public ProductsController(IProductService svc, ILogger<ProductsController> log)
        => (_svc, _log) = (svc, log);

    [HttpGet("")]
    public async Task<IActionResult> Index() =>
        View(await _svc.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id) =>
        await _svc.GetByIdAsync(id) is { } p ? View(p) : NotFound();

    [HttpGet("create")]
    public IActionResult Create() => View();

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product p)
    {
        if (!ModelState.IsValid) return View(p);
        await _svc.CreateAsync(p);
        TempData["Success"] = "تم الحفظ!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id) =>
        await _svc.GetByIdAsync(id) is { } p ? View(p) : NotFound();

    [HttpPost("edit/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product p)
    {
        if (id != p.Id) return BadRequest();
        if (!ModelState.IsValid) return View(p);
        await _svc.UpdateAsync(p);
        TempData["Success"] = "تم التعديل!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id);
        TempData["Success"] = "تم الحذف!";
        return RedirectToAction(nameof(Index));
    }
}
```

---

## 📋 Model + Validation

```csharp
public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "مطلوب")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }

    [Range(0.01, 999999)]
    public decimal Price { get; set; }

    [EmailAddress]    public string? Email { get; set; }
    [Phone]           public string? Phone { get; set; }
    [Url]             public string? Website { get; set; }
    [MaxLength(500)]  public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

---

## ⚡ Razor Quick Reference

```html
@model List<Product>                   @* Model declaration *@
@{ var count = Model.Count; }          @* Code block *@
@count                                  @* Expression (HTML Encoded) *@
@Html.Raw(htmlString)                  @* Raw HTML (Unsafe!) *@

@if (condition) { }                    @* Condition *@
@foreach (var x in Model) { }         @* Loop *@
@for (int i = 0; i < 5; i++) { }      @* For loop *@

@* Comment *@                          @* Razor Comment (مش في HTML) *@

@* Tag Helpers *@
<a asp-controller="X" asp-action="Y" asp-route-id="@id">Link</a>
<form asp-action="Create" method="post">
<input asp-for="Name" class="form-control" />
<span asp-validation-for="Name" class="text-danger"></span>
<div asp-validation-summary="All"></div>
<select asp-for="CategoryId" asp-items="ViewBag.Categories"></select>

@* Sections *@
@section Scripts { <script>...</script> }
@await RenderSectionAsync("Scripts", required: false)
@RenderBody()

@* Partial Views *@
<partial name="_MyPartial" model="item" />
@await Html.PartialAsync("_MyPartial", model)
```

---

## 🔗 HTTP Quick Reference

```
Methods:
GET    → Read      | Idempotent ✓ | Safe ✓ | Body ✗
POST   → Create    | Idempotent ✗ | Safe ✗ | Body ✓
PUT    → Replace   | Idempotent ✓ | Safe ✗ | Body ✓
PATCH  → Update    | Idempotent ✗ | Safe ✗ | Body ✓
DELETE → Delete    | Idempotent ✓ | Safe ✗ | Body opt

Status Codes:
200 OK | 201 Created | 204 No Content
301 Moved | 302 Found | 304 Not Modified
400 Bad Request | 401 Unauthorized | 403 Forbidden
404 Not Found | 409 Conflict | 422 Unprocessable
500 Server Error | 503 Unavailable
```

---

## 💉 Dependency Injection Lifetimes

```
Singleton  → 1 Object للـ App كلها     → Config, Logging
Scoped     → 1 Object لكل Request      → DbContext, Services
Transient  → Object جديد كل injection → Lightweight Services

builder.Services.AddSingleton<I, C>();
builder.Services.AddScoped<I, C>();
builder.Services.AddTransient<I, C>();
```

---

## 🗺️ Routing Patterns

```csharp
// Convention
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Attribute
[Route("api/[controller]")]          // api/products
[HttpGet("{id:int}")]                // GET api/products/5
[HttpGet("{id:int:range(1,999)}")]   // با Constraint
[HttpGet("{slug:alpha}")]            // string only
[HttpGet("{*catchAll}")]             // كل حاجة
```

---

## 🔐 Auth Quick Reference

```csharp
// Controllers Attributes
[Authorize]                          // أي User Logged In
[Authorize(Roles = "Admin")]         // Role محدد
[Authorize(Policy = "PremiumOnly")]  // Policy محددة
[AllowAnonymous]                     // كل الناس

// في View
@if (User.Identity?.IsAuthenticated ?? false) { }
@if (User.IsInRole("Admin")) { }
@User.Identity?.Name

// Cookie Auth Setup
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o => { o.LoginPath = "/login"; });
```

---

## 🗄️ EF Core Quick Reference

```csharp
// DbContext
public class AppDb : DbContext {
    public AppDb(DbContextOptions<AppDb> o) : base(o) { }
    public DbSet<Product> Products { get; set; }
}

// CRUD
var all = await _db.Products.ToListAsync();
var one = await _db.Products.FindAsync(id);
var filtered = await _db.Products.Where(p => p.Price > 100).ToListAsync();
var withNav = await _db.Products.Include(p => p.Category).ToListAsync();

_db.Products.Add(newProduct);
_db.Products.Update(product);
_db.Products.Remove(product);
await _db.SaveChangesAsync();

// Migrations CLI
dotnet ef migrations add MigrationName
dotnet ef database update
dotnet ef migrations remove
```

---

## 🍪 Cookies & Session

```csharp
// Cookies
Response.Cookies.Append("key", "value", new CookieOptions {
    Expires = DateTimeOffset.UtcNow.AddDays(30),
    HttpOnly = true, Secure = true,
    SameSite = SameSiteMode.Strict
});
var val = Request.Cookies["key"];
Response.Cookies.Delete("key");

// Session
HttpContext.Session.SetString("key", "value");
HttpContext.Session.SetInt32("count", 5);
var s = HttpContext.Session.GetString("key");
var n = HttpContext.Session.GetInt32("count");
```

---

## 📐 URL Structure

```
https://shop.example.com:5001/products/details?id=10&color=blue#reviews
  │           │           │       │          │    │               │
Protocol    Domain       Port   Path      Action  Query String  Fragment

Route: /products/{id:int}
Query: /products?search=laptop&page=2
Absolute: https://example.com/path
Relative: /path  OR  ./path  OR  ../path
```

---

## ⚔️ Framework vs Core

```
                 Framework    Core
Windows Only       ✓            ✗
Linux/Mac          ✗            ✓
Performance        Good      Excellent
Open Source     Partial        Full
DI Built-in        ✗            ✓
Docker             ✗            ✓
Hosting          IIS Only    Anything
Middleware      HTTP Modules  Pipeline
```

---

## 🔧 Common Packages (NuGet)

```
Microsoft.EntityFrameworkCore.SqlServer  → SQL Server
Microsoft.EntityFrameworkCore.Tools      → Migrations CLI
Microsoft.AspNetCore.Identity.EF         → Identity System
AutoMapper.Extensions.Microsoft.DI       → Object Mapping
FluentValidation.AspNetCore             → Validation
Serilog.AspNetCore                       → Logging
StackExchange.Redis                      → Redis Cache
Swashbuckle.AspNetCore                   → Swagger/OpenAPI
```

---

**التالي: [11 — Roadmap](./11-roadmap.md) →**
