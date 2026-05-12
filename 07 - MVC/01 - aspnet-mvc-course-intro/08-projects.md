# 08 — المشاريع العملية 🛠️

> **"الكلام النظري كويس، بس اللي بيثبّت المعلومة هو إنك تعمل بإيدك!"**

---

## 📋 Table of Contents

- [المشروع الأول: CRUD App (Product Manager)](#-مشروع-1-crud-app--product-manager)
- [المشروع الثاني: Blog System](#-مشروع-2-blog-system)

---

# 🗂️ مشروع 1: CRUD App — Product Manager

## الفكرة

نظام إدارة منتجات بسيط — Create, Read, Update, Delete.

```
Features:
✓ عرض قائمة المنتجات (Read)
✓ إضافة منتج جديد (Create)
✓ تعديل منتج موجود (Update)
✓ حذف منتج (Delete)
✓ بحث وفلترة
✓ Validation
✓ Flash Messages (TempData)
```

---

## 📁 Project Structure

```
ProductManager/
│
├── Controllers/
│   └── ProductsController.cs
│
├── Models/
│   ├── Product.cs
│   └── ViewModels/
│       └── ProductIndexViewModel.cs
│
├── Services/
│   ├── IProductService.cs
│   └── ProductService.cs
│
├── Data/
│   ├── AppDbContext.cs
│   └── Migrations/
│
├── Views/
│   ├── Products/
│   │   ├── Index.cshtml
│   │   ├── Details.cshtml
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       └── _Alerts.cshtml
│
├── wwwroot/
│   ├── css/site.css
│   └── js/site.js
│
├── appsettings.json
└── Program.cs
```

---

## 🏗️ الكود الكامل

### 1. Model

```csharp
// Models/Product.cs
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم المنتج مطلوب")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "الاسم بين 2 و 100 حرف")]
    [Display(Name = "اسم المنتج")]
    public string Name { get; set; }

    [Required(ErrorMessage = "الوصف مطلوب")]
    [StringLength(500)]
    [Display(Name = "الوصف")]
    public string Description { get; set; }

    [Required(ErrorMessage = "السعر مطلوب")]
    [Range(0.01, 999999, ErrorMessage = "السعر يجب أن يكون أكبر من 0")]
    [Display(Name = "السعر")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "الكمية مطلوبة")]
    [Range(0, 10000)]
    [Display(Name = "الكمية المتاحة")]
    public int Stock { get; set; }

    [Display(Name = "القسم")]
    public string Category { get; set; }

    [Display(Name = "نشط؟")]
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Computed Properties
    public bool IsInStock => Stock > 0;
    public string StockStatus => Stock > 10 ? "وفير" : Stock > 0 ? "محدود" : "نفذ";
}
```

### 2. ViewModel

```csharp
// Models/ViewModels/ProductIndexViewModel.cs
public class ProductIndexViewModel
{
    public List<Product> Products { get; set; } = new();
    public string? SearchTerm { get; set; }
    public string? Category { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public List<string> Categories { get; set; } = new();
}
```

### 3. DbContext

```csharp
// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Data
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "لابتوب Dell", Price = 15000, Stock = 10, Category = "إلكترونيات", Description = "لابتوب قوي للعمل" },
            new Product { Id = 2, Name = "موبايل Samsung", Price = 8000, Stock = 25, Category = "موبايلات", Description = "موبايل ممتاز" },
            new Product { Id = 3, Name = "سماعات Sony", Price = 1200, Stock = 0, Category = "إلكترونيات", Description = "سماعات لاسلكية" }
        );
    }
}
```

### 4. Service Interface

```csharp
// Services/IProductService.cs
public interface IProductService
{
    Task<ProductIndexViewModel> GetFilteredAsync(string? search, string? category, int page, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
    Task<List<string>> GetCategoriesAsync();
}
```

### 5. Service Implementation

```csharp
// Services/ProductService.cs
public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProductIndexViewModel> GetFilteredAsync(
        string? search, string? category, int page, int pageSize)
    {
        var query = _db.Products.Where(p => p.IsActive);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search) ||
                                     p.Description.Contains(search));

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        var total = await query.CountAsync();
        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ProductIndexViewModel
        {
            Products = products,
            SearchTerm = search,
            Category = category,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            Categories = await GetCategoriesAsync()
        };
    }

    public async Task<Product?> GetByIdAsync(int id)
        => await _db.Products.FindAsync(id);

    public async Task<Product> CreateAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return false;

        // Soft Delete
        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<string>> GetCategoriesAsync()
        => await _db.Products
            .Where(p => p.IsActive)
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
}
```

### 6. Controller

```csharp
// Controllers/ProductsController.cs
public class ProductsController : Controller
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET /Products
    public async Task<IActionResult> Index(
        string? search, string? category, int page = 1)
    {
        var viewModel = await _service.GetFilteredAsync(search, category, page, 10);
        return View(viewModel);
    }

    // GET /Products/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
        {
            TempData["Error"] = "المنتج غير موجود!";
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET /Products/Create
    public IActionResult Create() => View();

    // POST /Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
            return View(product);

        try
        {
            await _service.CreateAsync(product);
            TempData["Success"] = $"تم إضافة '{product.Name}' بنجاح! ✓";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            ModelState.AddModelError("", "حصل خطأ غير متوقع، حاول تاني");
            return View(product);
        }
    }

    // GET /Products/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    // POST /Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id) return BadRequest();

        if (!ModelState.IsValid)
            return View(product);

        try
        {
            await _service.UpdateAsync(product);
            TempData["Success"] = $"تم تعديل '{product.Name}' بنجاح! ✓";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {Id}", id);
            ModelState.AddModelError("", "حصل خطأ، حاول تاني");
            return View(product);
        }
    }

    // POST /Products/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        TempData[success ? "Success" : "Error"] =
            success ? "تم حذف المنتج بنجاح!" : "المنتج غير موجود!";
        return RedirectToAction(nameof(Index));
    }
}
```

### 7. Views

```html
<!-- Views/Products/Index.cshtml -->
@model ProductIndexViewModel

@{
    ViewData["Title"] = "إدارة المنتجات";
}

<div class="container-fluid py-4">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>🗂️ @ViewData["Title"]</h2>
        <a asp-action="Create" class="btn btn-success">
            ➕ إضافة منتج جديد
        </a>
    </div>

    @* Alerts *@
    <partial name="_Alerts" />

    @* Search & Filter Form *@
    <div class="card mb-4">
        <div class="card-body">
            <form asp-action="Index" method="get" class="row g-3">
                <div class="col-md-5">
                    <input type="text" name="search" value="@Model.SearchTerm"
                           class="form-control" placeholder="🔍 ابحث عن منتج..." />
                </div>
                <div class="col-md-4">
                    <select name="category" class="form-select">
                        <option value="">-- كل الأقسام --</option>
                        @foreach (var cat in Model.Categories)
                        {
                            <option value="@cat"
                                    selected="@(Model.Category == cat)">
                                @cat
                            </option>
                        }
                    </select>
                </div>
                <div class="col-md-3">
                    <button type="submit" class="btn btn-primary w-100">بحث</button>
                </div>
            </form>
        </div>
    </div>

    @* Products Table *@
    @if (!Model.Products.Any())
    {
        <div class="alert alert-info text-center">
            <h5>😕 لا توجد منتجات</h5>
            <a asp-action="Create" class="btn btn-success mt-2">أضف أول منتج</a>
        </div>
    }
    else
    {
        <div class="card">
            <div class="card-body p-0">
                <table class="table table-hover mb-0">
                    <thead class="table-dark">
                        <tr>
                            <th>#</th>
                            <th>اسم المنتج</th>
                            <th>القسم</th>
                            <th>السعر</th>
                            <th>المخزون</th>
                            <th>الحالة</th>
                            <th>الإجراءات</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach (var product in Model.Products)
                        {
                            <tr>
                                <td>@product.Id</td>
                                <td>
                                    <strong>@product.Name</strong>
                                    <br />
                                    <small class="text-muted">@product.Description.Substring(0, Math.Min(50, product.Description.Length))...</small>
                                </td>
                                <td>
                                    <span class="badge bg-secondary">@product.Category</span>
                                </td>
                                <td>
                                    <strong class="text-success">@product.Price.ToString("N2") ج.م</strong>
                                </td>
                                <td>
                                    <span class="badge @(product.Stock > 10 ? "bg-success" : product.Stock > 0 ? "bg-warning" : "bg-danger")">
                                        @product.StockStatus (@product.Stock)
                                    </span>
                                </td>
                                <td>
                                    @if (product.IsActive)
                                    {
                                        <span class="badge bg-success">✓ نشط</span>
                                    }
                                    else
                                    {
                                        <span class="badge bg-danger">✗ معطل</span>
                                    }
                                </td>
                                <td>
                                    <a asp-action="Details" asp-route-id="@product.Id"
                                       class="btn btn-sm btn-info">👁️</a>
                                    <a asp-action="Edit" asp-route-id="@product.Id"
                                       class="btn btn-sm btn-warning">✏️</a>
                                    <form asp-action="Delete" method="post"
                                          style="display:inline"
                                          onsubmit="return confirm('هل أنت متأكد من حذف @product.Name؟')">
                                        @Html.AntiForgeryToken()
                                        <input type="hidden" name="id" value="@product.Id" />
                                        <button type="submit" class="btn btn-sm btn-danger">🗑️</button>
                                    </form>
                                </td>
                            </tr>
                        }
                    </tbody>
                </table>
            </div>
        </div>

        @* Pagination *@
        @if (Model.TotalPages > 1)
        {
            <nav class="mt-3">
                <ul class="pagination justify-content-center">
                    @for (int i = 1; i <= Model.TotalPages; i++)
                    {
                        <li class="page-item @(i == Model.Page ? "active" : "")">
                            <a class="page-link"
                               asp-action="Index"
                               asp-route-page="@i"
                               asp-route-search="@Model.SearchTerm"
                               asp-route-category="@Model.Category">
                                @i
                            </a>
                        </li>
                    }
                </ul>
            </nav>
        }

        <p class="text-muted text-center">
            عرض @Model.Products.Count من @Model.TotalCount منتج
        </p>
    }
</div>
```

```html
<!-- Views/Products/Create.cshtml -->
@model Product

@{
    ViewData["Title"] = "إضافة منتج جديد";
}

<div class="container py-4">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <div class="card shadow">
                <div class="card-header bg-success text-white">
                    <h4 class="mb-0">➕ @ViewData["Title"]</h4>
                </div>
                <div class="card-body">
                    <form asp-action="Create" method="post">
                        @Html.AntiForgeryToken()

                        <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>

                        <div class="row g-3">
                            <div class="col-md-8">
                                <label asp-for="Name" class="form-label fw-bold"></label>
                                <input asp-for="Name" class="form-control" placeholder="اسم المنتج" />
                                <span asp-validation-for="Name" class="text-danger small"></span>
                            </div>

                            <div class="col-md-4">
                                <label asp-for="Category" class="form-label fw-bold"></label>
                                <input asp-for="Category" class="form-control" placeholder="مثال: إلكترونيات" />
                                <span asp-validation-for="Category" class="text-danger small"></span>
                            </div>

                            <div class="col-12">
                                <label asp-for="Description" class="form-label fw-bold"></label>
                                <textarea asp-for="Description" class="form-control"
                                          rows="3" placeholder="وصف المنتج..."></textarea>
                                <span asp-validation-for="Description" class="text-danger small"></span>
                            </div>

                            <div class="col-md-6">
                                <label asp-for="Price" class="form-label fw-bold"></label>
                                <div class="input-group">
                                    <input asp-for="Price" class="form-control"
                                           type="number" step="0.01" placeholder="0.00" />
                                    <span class="input-group-text">ج.م</span>
                                </div>
                                <span asp-validation-for="Price" class="text-danger small"></span>
                            </div>

                            <div class="col-md-6">
                                <label asp-for="Stock" class="form-label fw-bold"></label>
                                <input asp-for="Stock" class="form-control"
                                       type="number" placeholder="0" />
                                <span asp-validation-for="Stock" class="text-danger small"></span>
                            </div>

                            <div class="col-12">
                                <div class="form-check form-switch">
                                    <input asp-for="IsActive" class="form-check-input"
                                           role="switch" />
                                    <label asp-for="IsActive" class="form-check-label">
                                        المنتج نشط ومتاح للعرض
                                    </label>
                                </div>
                            </div>
                        </div>

                        <hr />
                        <div class="d-flex gap-2">
                            <button type="submit" class="btn btn-success">
                                💾 حفظ المنتج
                            </button>
                            <a asp-action="Index" class="btn btn-outline-secondary">
                                ❌ إلغاء
                            </a>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScripts" />
}
```

### 8. Program.cs

```csharp
// Program.cs
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();

// Session (اختياري)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
```

### 9. appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductManagerDb;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

# 📝 مشروع 2: Blog System

## الفكرة

نظام مدونة كامل مع Authentication، Comments، Categories، وTags.

```
Features:
✓ إدارة المقالات (CRUD)
✓ Categories
✓ Tags
✓ Comments نظام
✓ User Authentication (Login/Register)
✓ Role-based Authorization (Admin/Author/Reader)
✓ Rich Text Editor (Markdown)
✓ Image Upload
✓ Pagination
✓ SEO Friendly URLs (Slugs)
✓ Search
```

---

## 📁 Project Structure

```
BlogSystem/
│
├── Controllers/
│   ├── HomeController.cs         (الصفحة الرئيسية)
│   ├── PostsController.cs        (المقالات للقراء)
│   ├── AccountController.cs      (Login/Register)
│   ├── Admin/
│   │   ├── PostsController.cs    (إدارة المقالات)
│   │   ├── CategoriesController.cs
│   │   └── CommentsController.cs
│   └── CommentsController.cs     (API للتعليقات)
│
├── Models/
│   ├── Post.cs
│   ├── Category.cs
│   ├── Tag.cs
│   ├── Comment.cs
│   ├── ApplicationUser.cs
│   └── ViewModels/
│       ├── PostViewModel.cs
│       ├── PostDetailsViewModel.cs
│       ├── LoginViewModel.cs
│       └── RegisterViewModel.cs
│
├── Services/
│   ├── IPostService.cs
│   ├── PostService.cs
│   ├── ICommentService.cs
│   ├── CommentService.cs
│   └── IFileService.cs
│       FileService.cs
│
├── Data/
│   └── BlogDbContext.cs
│
├── Views/
│   ├── Home/Index.cshtml
│   ├── Posts/
│   │   ├── Index.cshtml          (قائمة المقالات)
│   │   └── Details.cshtml        (تفاصيل مقال)
│   ├── Account/
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   ├── Admin/
│   │   └── Posts/
│   │       ├── Index.cshtml
│   │       ├── Create.cshtml
│   │       └── Edit.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       ├── _PostCard.cshtml
│       └── _CommentSection.cshtml
│
└── Program.cs
```

---

## 🏗️ الكود الكامل

### 1. Models

```csharp
// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
```

```csharp
// Models/Category.cs
public class Category
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    public string Slug { get; set; }

    public string? Description { get; set; }

    // Navigation
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
```

```csharp
// Models/Tag.cs
public class Tag
{
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string Name { get; set; }

    public string Slug { get; set; }

    // Navigation
    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
}
```

```csharp
// Models/Post.cs
public class Post
{
    public int Id { get; set; }

    [Required(ErrorMessage = "العنوان مطلوب")]
    [MaxLength(200)]
    [Display(Name = "العنوان")]
    public string Title { get; set; }

    // SEO Friendly URL
    public string Slug { get; set; }

    [Required(ErrorMessage = "المحتوى مطلوب")]
    [Display(Name = "المحتوى")]
    public string Content { get; set; }

    // مختصر للـ Preview
    [MaxLength(300)]
    [Display(Name = "المقتطف")]
    public string? Excerpt { get; set; }

    public string? FeaturedImageUrl { get; set; }

    [Display(Name = "الحالة")]
    public PostStatus Status { get; set; } = PostStatus.Draft;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }

    // SEO
    [MaxLength(160)]
    public string? MetaDescription { get; set; }

    public int ViewCount { get; set; } = 0;

    // Foreign Keys
    public string AuthorId { get; set; }
    public int CategoryId { get; set; }

    // Navigation Properties
    public ApplicationUser Author { get; set; }
    public Category Category { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

    // Computed
    public int CommentsCount => Comments.Count(c => c.IsApproved);
    public string ReadingTime => $"{Math.Ceiling(Content.Split(' ').Length / 200.0)} دقيقة للقراءة";
}

public enum PostStatus
{
    Draft = 0,
    Published = 1,
    Archived = 2
}

// Models/PostTag.cs (Many-to-Many Join Table)
public class PostTag
{
    public int PostId { get; set; }
    public int TagId { get; set; }
    public Post Post { get; set; }
    public Tag Tag { get; set; }
}
```

```csharp
// Models/Comment.cs
public class Comment
{
    public int Id { get; set; }

    [Required(ErrorMessage = "التعليق لا يمكن أن يكون فارغاً")]
    [MaxLength(1000)]
    [Display(Name = "التعليق")]
    public string Content { get; set; }

    public bool IsApproved { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int PostId { get; set; }
    public string? AuthorId { get; set; }

    // للزوار (مش مسجلين)
    public string? GuestName { get; set; }
    public string? GuestEmail { get; set; }

    // Navigation
    public Post Post { get; set; }
    public ApplicationUser? Author { get; set; }

    public string DisplayName => Author?.DisplayName ?? GuestName ?? "زائر";
}
```

### 2. DbContext

```csharp
// Data/BlogDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class BlogDbContext : IdentityDbContext<ApplicationUser>
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Many-to-Many: Post ↔ Tag
        builder.Entity<PostTag>()
            .HasKey(pt => new { pt.PostId, pt.TagId });

        builder.Entity<PostTag>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId);

        builder.Entity<PostTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PostTags)
            .HasForeignKey(pt => pt.TagId);

        // Index for Slug (unique)
        builder.Entity<Post>()
            .HasIndex(p => p.Slug).IsUnique();

        builder.Entity<Category>()
            .HasIndex(c => c.Slug).IsUnique();

        // Seed Categories
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "تقنية", Slug = "technology" },
            new Category { Id = 2, Name = "برمجة", Slug = "programming" },
            new Category { Id = 3, Name = "ASP.NET", Slug = "aspnet" }
        );
    }
}
```

### 3. ViewModels

```csharp
// Models/ViewModels/PostDetailsViewModel.cs
public class PostDetailsViewModel
{
    public Post Post { get; set; }
    public List<Post> RelatedPosts { get; set; } = new();
    public CommentFormViewModel NewComment { get; set; } = new();
}

public class CommentFormViewModel
{
    public int PostId { get; set; }

    [Required(ErrorMessage = "من فضلك اكتب تعليقك")]
    [MaxLength(1000)]
    [Display(Name = "تعليقك")]
    public string Content { get; set; }

    // للزوار
    [MaxLength(50)]
    [Display(Name = "الاسم")]
    public string? GuestName { get; set; }

    [EmailAddress]
    [Display(Name = "البريد الإلكتروني")]
    public string? GuestEmail { get; set; }
}

// Models/ViewModels/LoginViewModel.cs
public class LoginViewModel
{
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "بريد إلكتروني غير صحيح")]
    [Display(Name = "البريد الإلكتروني")]
    public string Email { get; set; }

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [DataType(DataType.Password)]
    [Display(Name = "كلمة المرور")]
    public string Password { get; set; }

    [Display(Name = "تذكرني")]
    public bool RememberMe { get; set; }
}
```

### 4. PostService

```csharp
// Services/PostService.cs
public class PostService : IPostService
{
    private readonly BlogDbContext _db;

    public PostService(BlogDbContext db) => _db = db;

    public async Task<(List<Post> posts, int total)> GetPublishedAsync(
        string? search, string? categorySlug, string? tag, int page, int size = 6)
    {
        var query = _db.Posts
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            .Where(p => p.Status == PostStatus.Published);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Title.Contains(search) ||
                                     p.Content.Contains(search));

        if (!string.IsNullOrEmpty(categorySlug))
            query = query.Where(p => p.Category.Slug == categorySlug);

        if (!string.IsNullOrEmpty(tag))
            query = query.Where(p => p.PostTags.Any(pt => pt.Tag.Slug == tag));

        var total = await query.CountAsync();
        var posts = await query
            .OrderByDescending(p => p.PublishedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (posts, total);
    }

    public async Task<Post?> GetBySlugAsync(string slug)
    {
        var post = await _db.Posts
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.Comments.Where(c => c.IsApproved))
                .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(p => p.Slug == slug && p.Status == PostStatus.Published);

        if (post != null)
        {
            post.ViewCount++;
            await _db.SaveChangesAsync();
        }

        return post;
    }

    public async Task<Post> CreateAsync(Post post, string[] tagNames)
    {
        // توليد Slug
        post.Slug = await GenerateUniqueSlugAsync(post.Title);
        post.CreatedAt = DateTime.UtcNow;

        if (post.Status == PostStatus.Published)
            post.PublishedAt = DateTime.UtcNow;

        // Auto-generate excerpt
        if (string.IsNullOrEmpty(post.Excerpt))
            post.Excerpt = post.Content.Length > 200
                ? post.Content.Substring(0, 200) + "..."
                : post.Content;

        // Tags
        foreach (var tagName in tagNames.Where(t => !string.IsNullOrEmpty(t)))
        {
            var tag = await _db.Tags.FirstOrDefaultAsync(t => t.Name == tagName)
                      ?? new Tag { Name = tagName, Slug = GenerateSlug(tagName) };

            post.PostTags.Add(new PostTag { Tag = tag });
        }

        _db.Posts.Add(post);
        await _db.SaveChangesAsync();
        return post;
    }

    private async Task<string> GenerateUniqueSlugAsync(string title)
    {
        var baseSlug = GenerateSlug(title);
        var slug = baseSlug;
        var counter = 1;

        while (await _db.Posts.AnyAsync(p => p.Slug == slug))
            slug = $"{baseSlug}-{counter++}";

        return slug;
    }

    private static string GenerateSlug(string text)
    {
        // تحويل بسيط للـ Slug
        return text.ToLower()
            .Replace(" ", "-")
            .Replace("ا", "a").Replace("ب", "b") // يفضل Library متخصصة
            + "-" + DateTime.Now.Ticks.ToString().Substring(10);
    }
}
```

### 5. Controllers

```csharp
// Controllers/PostsController.cs (للقراء)
public class PostsController : Controller
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public PostsController(IPostService postService, ICommentService commentService)
    {
        _postService = postService;
        _commentService = commentService;
    }

    // GET /posts
    public async Task<IActionResult> Index(
        string? search, string? category, string? tag, int page = 1)
    {
        var (posts, total) = await _postService.GetPublishedAsync(
            search, category, tag, page);

        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.Tag = tag;
        ViewBag.Page = page;
        ViewBag.TotalPages = (int)Math.Ceiling(total / 6.0);

        return View(posts);
    }

    // GET /posts/{slug}
    [Route("posts/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var post = await _postService.GetBySlugAsync(slug);
        if (post == null) return NotFound();

        var viewModel = new PostDetailsViewModel
        {
            Post = post,
            RelatedPosts = await _postService.GetRelatedAsync(post.CategoryId, post.Id, 3),
            NewComment = new CommentFormViewModel { PostId = post.Id }
        };

        return View(viewModel);
    }

    // POST /posts/{postId}/comments
    [HttpPost]
    [Route("posts/{postId}/comments")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int postId, CommentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // رجّع لصفحة التفاصيل مع الأخطاء
            var post = await _postService.GetByIdAsync(postId);
            var viewModel = new PostDetailsViewModel { Post = post!, NewComment = model };
            return View("Details", viewModel);
        }

        var comment = new Comment
        {
            PostId = postId,
            Content = model.Content,
            AuthorId = User.Identity?.IsAuthenticated == true
                ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null,
            GuestName = model.GuestName,
            GuestEmail = model.GuestEmail,
            IsApproved = User.IsInRole("Admin") // الـ Admins تعليقاتهم معتمدة تلقائياً
        };

        await _commentService.CreateAsync(comment);

        TempData["Success"] = comment.IsApproved
            ? "تم إضافة تعليقك! ✓"
            : "تم إرسال تعليقك وسيظهر بعد المراجعة.";

        return RedirectToAction(nameof(Details), new { slug = (await _postService.GetByIdAsync(postId))?.Slug });
    }
}
```

```csharp
// Controllers/AccountController.cs
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            TempData["Success"] = "أهلاً بعودتك! 👋";
            return LocalRedirect(returnUrl ?? "/");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "الحساب محظور مؤقتاً بسبب محاولات كثيرة. حاول بعد قليل.");
            return View(model);
        }

        ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        TempData["Success"] = "تم تسجيل الخروج بنجاح";
        return RedirectToAction("Index", "Home");
    }
}
```

### 6. Admin Controller

```csharp
// Controllers/Admin/PostsController.cs
[Area("Admin")]
[Authorize(Roles = "Admin,Author")]
public class PostsController : Controller
{
    private readonly IPostService _service;
    private readonly UserManager<ApplicationUser> _userManager;

    public PostsController(IPostService service, UserManager<ApplicationUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var isAdmin = User.IsInRole("Admin");

        var posts = isAdmin
            ? await _service.GetAllAsync()
            : await _service.GetByAuthorAsync(userId!);

        return View(posts);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _service.GetCategoriesSelectListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Post post, string[] tags,
        IFormFile? featuredImage)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _service.GetCategoriesSelectListAsync();
            return View(post);
        }

        post.AuthorId = _userManager.GetUserId(User)!;

        // Image Upload
        if (featuredImage != null)
            post.FeaturedImageUrl = await SaveImageAsync(featuredImage);

        await _service.CreateAsync(post, tags);
        TempData["Success"] = "تم نشر المقال! 🎉";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveImageAsync(IFormFile file)
    {
        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        Directory.CreateDirectory(uploads);
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploads, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/{fileName}";
    }
}
```

### 7. Program.cs للـ Blog

```csharp
// Program.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogConnection")));

// Identity (Authentication System)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<BlogDbContext>()
.AddDefaultTokenProviders();

// Services
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Cookie Settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  // لازم قبل UseAuthorization
app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Posts}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "blog",
    pattern: "posts/{slug}",
    defaults: new { controller = "Posts", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed Roles
await SeedRolesAsync(app);

app.Run();

async Task SeedRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // أنشئ الـ Roles
    string[] roles = { "Admin", "Author", "Reader" };
    foreach (var role in roles)
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

    // أنشئ Admin User
    var adminEmail = "admin@blog.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            DisplayName = "Admin",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(admin, "Admin@123456");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, "Admin");
    }
}
```

---

## 📊 Database Migrations

```bash
# إنشاء الـ Migration الأولى
dotnet ef migrations add InitialCreate

# تطبيق الـ Migrations
dotnet ef database update

# التراجع عن Migration
dotnet ef migrations remove

# عرض الـ Migrations
dotnet ef migrations list
```

---

## 🚀 تشغيل المشاريع

```bash
# Install Dependencies
dotnet restore

# Run in Development
dotnet run

# Run with Hot Reload
dotnet watch run

# Build for Production
dotnet publish -c Release -o ./publish

# Run Tests
dotnet test
```

---

**التالي: [09 — أسئلة الـ Interview](./09-interview-questions.md) →**
