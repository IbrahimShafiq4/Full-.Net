# 06 — MVC بالتفصيل الكامل 🏗️

> **"دي ملكة الملفات — هنشرح كل حاجة في MVC من أول وجديد بالتفصيل الزيادة"**

---

## 📋 Table of Contents

- [Folder Structure](#-folder-structure)
- [Routing بالتفصيل](#-routing-بالتفصيل)
- [Action Methods](#-action-methods)
- [Dependency Injection](#-dependency-injection-في-mvc)
- [ViewBag, ViewData, TempData](#-viewbag-viewdata-tempdata)
- [Razor Views](#-razor-views)
- [Layouts و Partial Views](#-layouts-و-partial-views)
- [Model Validation](#-model-validation)
- [Filters](#-filters)
- [Data Flow في MVC](#-data-flow-في-mvc)
- [Interview Questions](#-interview-questions)
- [Mini Summary](#-mini-summary)

---

## 📁 Folder Structure

```
MyMvcApp/
│
├── Controllers/
│   ├── HomeController.cs
│   ├── ProductsController.cs
│   └── AccountController.cs
│
├── Models/
│   ├── Product.cs              (Domain Model)
│   ├── Order.cs
│   └── ViewModels/
│       ├── ProductViewModel.cs (View Model)
│       └── LoginViewModel.cs
│
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── About.cshtml
│   ├── Products/
│   │   ├── Index.cshtml
│   │   ├── Details.cshtml
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   ├── Account/
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   └── Shared/
│       ├── _Layout.cshtml      (الـ Master Layout)
│       ├── _Navbar.cshtml      (Partial View)
│       ├── Error.cshtml
│       └── _ValidationScripts.cshtml
│
├── wwwroot/                    (Static Files)
│   ├── css/
│   │   └── site.css
│   ├── js/
│   │   └── site.js
│   └── images/
│
├── Services/                   (Business Logic)
│   ├── IProductService.cs
│   └── ProductService.cs
│
├── Data/                       (Database)
│   ├── AppDbContext.cs
│   └── Migrations/
│
├── appsettings.json
├── appsettings.Development.json
└── Program.cs
```

### ليه الـ Structure ده؟

```
Controllers/ → فصل منطق التحكم
Models/      → فصل البيانات والـ Business Rules
Views/       → فصل الـ UI
             → كل Controller عنده مجلد Views بنفس اسمه
wwwroot/     → الملفات الـ Static (CSS, JS, Images)
             → المتصفح بيوصلها مباشرة
```

---

## 🗺️ Routing بالتفصيل

### ما هو Routing؟

الـ Routing هو عملية **تحديد أي Controller وAction تتنفذ** بناءاً على الـ URL.

```
URL: /Products/Details/5
     ↓
Routing Engine يحلل الـ URL
     ↓
ProductsController → Details Action → id = 5
```

### أنواع الـ Routing

#### 1. Convention-based Routing (الـ Pattern التقليدي)

```csharp
// Program.cs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

```
Pattern:  {controller}/{action}/{id?}

URL Examples:
/                    → HomeController.Index()
/Home                → HomeController.Index()
/Home/About          → HomeController.About()
/Products            → ProductsController.Index()
/Products/Details/5  → ProductsController.Details(id: 5)
/Products/Edit/10    → ProductsController.Edit(id: 10)
```

#### 2. Attribute Routing (الأحدث والأفضل)

```csharp
[Route("products")]
public class ProductsController : Controller
{
    [Route("")]              // GET /products
    [Route("list")]          // GET /products/list
    public IActionResult Index() { ... }
    
    [Route("{id:int}")]      // GET /products/5
    public IActionResult Details(int id) { ... }
    
    [Route("category/{name}")]  // GET /products/category/electronics
    public IActionResult ByCategory(string name) { ... }
    
    [HttpGet("search")]      // GET /products/search
    public IActionResult Search(string q) { ... }
}
```

#### 3. Route Constraints (قيود على الـ Parameters)

```csharp
// id لازم يكون int
[Route("{id:int}")]

// name لازم يكون string فقط حروف
[Route("{name:alpha}")]

// id بين 1 و 999
[Route("{id:int:range(1,999)}")]

// guid
[Route("{id:guid}")]

// datetime
[Route("{date:datetime}")]

// minlength
[Route("{name:minlength(3)}")]
```

### Route Parameters

```csharp
// Optional Parameter
[Route("products/{id?}")]
public IActionResult Details(int? id)
{
    if (id == null) return Index();
    // ...
}

// Default Value
[Route("products/{page=1}")]
public IActionResult Index(int page)
{
    // page = 1 لو مش موجود في الـ URL
}

// Multiple Parameters
[Route("shop/{category}/{page:int=1}")]
public IActionResult Shop(string category, int page) { ... }
```

---

## ⚡ Action Methods

### أنواع الـ Action Results

```csharp
public class ProductsController : Controller
{
    // 200 + View
    public IActionResult Index()
    {
        return View(); // Views/Products/Index.cshtml
    }
    
    // 200 + View + Data
    public IActionResult Details(int id)
    {
        var product = GetProduct(id);
        return View(product); // بيبعت الـ Model للـ View
    }
    
    // 200 + JSON
    public IActionResult GetJson()
    {
        return Json(new { name = "Test", price = 100 });
    }
    
    // 302 Redirect
    public IActionResult Create()
    {
        // بعد الحفظ
        return RedirectToAction("Index");
        // أو
        return RedirectToAction("Details", new { id = 5 });
        // أو
        return Redirect("/some/url");
    }
    
    // 404
    public IActionResult FindProduct(int id)
    {
        var p = GetProduct(id);
        if (p == null) return NotFound();
        return View(p);
    }
    
    // File Download
    public IActionResult Download()
    {
        var bytes = GetFileBytes();
        return File(bytes, "application/pdf", "report.pdf");
    }
    
    // 204 No Content
    public IActionResult Delete(int id)
    {
        DeleteProduct(id);
        return NoContent();
    }
    
    // View بمسار مختلف
    public IActionResult CustomView()
    {
        return View("~/Views/Shared/MySpecialView.cshtml");
    }
    
    // Content
    public IActionResult Text()
    {
        return Content("Hello World", "text/plain");
    }
}
```

### HTTP Method Attributes

```csharp
public class ProductsController : Controller
{
    // GET فقط
    [HttpGet]
    public IActionResult Create() => View();
    
    // POST فقط
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product) { ... }
    
    // PUT
    [HttpPut("{id}")]
    public IActionResult Update(int id, Product product) { ... }
    
    // DELETE
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) { ... }
    
    // أكتر من Method
    [AcceptVerbs("GET", "POST")]
    public IActionResult MultiMethod() { ... }
}
```

### Async Actions (الأفضل للـ Performance)

```csharp
public class ProductsController : Controller
{
    private readonly AppDbContext _db;
    
    // Async Action
    public async Task<IActionResult> Index()
    {
        var products = await _db.Products
            .Where(p => p.IsActive)
            .ToListAsync();
        return View(products);
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (product == null) return NotFound();
        return View(product);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);
        
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        
        TempData["Success"] = "تم إضافة المنتج!";
        return RedirectToAction(nameof(Index));
    }
}
```

---

## 💉 Dependency Injection في MVC

### تسجيل الـ Services

```csharp
// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddTransient<IFileService, FileService>();
```

### Constructor Injection في Controller

```csharp
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;
    
    // Constructor Injection — ASP.NET Core بيوفر الـ Services تلقائياً
    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger,
        IMapper mapper)
    {
        _productService = productService;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Getting all products");
        var products = await _productService.GetAllAsync();
        return View(products);
    }
}
```

### Interface و Implementation

```csharp
// Services/IProductService.cs
public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

// Services/ProductService.cs
public class ProductService : IProductService
{
    private readonly AppDbContext _db;
    
    public ProductService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
    
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _db.Products.FindAsync(id);
    }
    
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
    
    public async Task DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            product.IsActive = false; // Soft Delete
            await _db.SaveChangesAsync();
        }
    }
}
```

---

## 📦 ViewBag, ViewData, TempData

### مقارنة الثلاثة

| Feature | ViewBag | ViewData | TempData |
|---------|---------|----------|----------|
| **النوع** | dynamic | Dictionary | Dictionary |
| **المدة** | Request واحد | Request واحد | حتى الـ Request الجاي |
| **بعد Redirect** | ❌ يضيع | ❌ يضيع | ✅ يبقى |
| **Type Safety** | ❌ | ❌ | ❌ |
| **Null Check** | لازم | لازم | لازم |

### ViewBag

```csharp
// في الـ Controller
ViewBag.Title = "قائمة المنتجات";
ViewBag.Count = products.Count;
ViewBag.User = currentUser;

// في الـ View
<h1>@ViewBag.Title</h1>
<p>عدد المنتجات: @ViewBag.Count</p>
```

### ViewData

```csharp
// في الـ Controller
ViewData["Title"] = "قائمة المنتجات";
ViewData["Products"] = products;

// في الـ View
<h1>@ViewData["Title"]</h1>
<p>@((int)ViewData["Count"]) منتج</p>
```

### TempData (الأهم — بيتبقى بعد Redirect)

```csharp
// Controller → بعد Save
TempData["SuccessMessage"] = "تم حفظ البيانات بنجاح!";
TempData["ErrorMessage"] = "حصل خطأ!";
return RedirectToAction("Index"); // Redirect!

// في View/Index.cshtml
@if (TempData["SuccessMessage"] != null)
{
    <div class="alert alert-success">
        @TempData["SuccessMessage"]
    </div>
}
@if (TempData["ErrorMessage"] != null)
{
    <div class="alert alert-danger">
        @TempData["ErrorMessage"]
    </div>
}
```

---

## ⚡ Razor Views

### أساسيات Razor

```html
<!-- ملف .cshtml = HTML + Razor Syntax -->

<!-- 1. تعريف Model -->
@model List<Product>

<!-- 2. Code Block -->
@{
    ViewData["Title"] = "Products";
    var isAdmin = User.IsInRole("Admin");
    int totalProducts = Model.Count;
}

<!-- 3. Expression (بيتحول لـ HTML) -->
<h1>@ViewData["Title"]</h1>
<p>عدد المنتجات: @totalProducts</p>

<!-- 4. Condition -->
@if (Model.Count == 0)
{
    <p>لا يوجد منتجات</p>
}
else
{
    <p>يوجد @Model.Count منتج</p>
}

<!-- 5. Loop -->
@foreach (var product in Model)
{
    <div class="product-card">
        <h3>@product.Name</h3>
        <p>@product.Price.ToString("C2")</p>
    </div>
}

<!-- 6. HTML Encoding (تلقائي — حماية من XSS) -->
@product.Name <!-- Safe: بيعمل HTML Encoding -->
@Html.Raw(product.Description) <!-- Unsafe: مش بيعمل Encoding -->

<!-- 7. Tag Helpers -->
<a asp-controller="Products" asp-action="Details" asp-route-id="@product.Id">
    تفاصيل
</a>

<form asp-action="Create" method="post">
    <input asp-for="Name" class="form-control" />
    <span asp-validation-for="Name" class="text-danger"></span>
    <button type="submit">حفظ</button>
</form>
```

### Tag Helpers الشائعة

```html
<!-- Links -->
<a asp-controller="Home" asp-action="Index">الرئيسية</a>
<a asp-action="Edit" asp-route-id="@product.Id">تعديل</a>

<!-- Forms -->
<form asp-action="Create" asp-controller="Products" method="post">
    @Html.AntiForgeryToken()
    
    <div class="form-group">
        <label asp-for="Name"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    
    <button type="submit">حفظ</button>
</form>

<!-- Select List -->
<select asp-for="CategoryId" asp-items="ViewBag.Categories" class="form-control">
    <option value="">-- اختر قسم --</option>
</select>

<!-- Checkbox -->
<input asp-for="IsActive" type="checkbox" />

<!-- Image -->
<img asp-append-version="true" src="~/images/logo.png" />

<!-- Environment -->
<environment include="Development">
    <script src="~/js/site.js"></script>
</environment>
<environment exclude="Development">
    <script src="~/js/site.min.js"></script>
</environment>
```

---

## 🎨 Layouts و Partial Views

### _Layout.cshtml (الـ Master Template)

```html
<!-- Views/Shared/_Layout.cshtml -->
<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - My App</title>
    <link rel="stylesheet" href="~/css/site.css" />
    
    @* أي CSS خاص بالصفحة *@
    @await RenderSectionAsync("Styles", required: false)
</head>
<body>
    @* Navigation Bar *@
    <partial name="_Navbar" />
    
    @* الـ Alert Messages *@
    <partial name="_Alerts" />
    
    @* محتوى كل صفحة هنا *@
    <main class="container">
        @RenderBody()
    </main>
    
    <footer>
        <p>© 2026 My App</p>
    </footer>
    
    <script src="~/js/site.js"></script>
    
    @* أي Scripts خاصة بالصفحة *@
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### استخدام الـ Layout في الصفحة

```html
<!-- Views/Products/Index.cshtml -->
@model List<Product>

@{
    ViewData["Title"] = "المنتجات";
    Layout = "_Layout"; // ممكن تغير الـ Layout لكل صفحة
}

<h1>@ViewData["Title"]</h1>

@* محتوى الصفحة *@
<div class="products-grid">
    @foreach (var product in Model)
    {
        <partial name="_ProductCard" model="product" />
    }
</div>

@* Section خاص بالصفحة دي *@
@section Scripts {
    <script src="~/js/products.js"></script>
}
```

### Partial Views

```html
<!-- Views/Shared/_ProductCard.cshtml -->
@model Product

<div class="card">
    <div class="card-body">
        <h5 class="card-title">@Model.Name</h5>
        <p class="card-text">@Model.Price.ToString("C")</p>
        @if (Model.IsInStock)
        {
            <span class="badge bg-success">متاح</span>
        }
        else
        {
            <span class="badge bg-danger">نفذت الكمية</span>
        }
        <a asp-action="Details" asp-route-id="@Model.Id" 
           class="btn btn-primary">تفاصيل</a>
    </div>
</div>
```

### _ViewStart.cshtml و _ViewImports.cshtml

```csharp
// Views/_ViewStart.cshtml — بيتنفذ قبل كل View
@{
    Layout = "_Layout"; // Default Layout لكل الـ Views
}

// Views/_ViewImports.cshtml — Imports مشتركة
@using MyApp.Models
@using MyApp.Models.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

---

## ✅ Model Validation

```csharp
// Models/Product.cs
public class Product
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "اسم المنتج مطلوب")]
    [StringLength(100, MinimumLength = 3, 
        ErrorMessage = "الاسم بين 3 و 100 حرف")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "السعر مطلوب")]
    [Range(0.01, 99999.99, ErrorMessage = "السعر بين 0.01 و 99999.99")]
    public decimal Price { get; set; }
    
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string? ContactEmail { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
    
    [Url]
    public string? Website { get; set; }
    
    [RegularExpression(@"^[A-Z]{2,4}$", ErrorMessage = "كود غير صحيح")]
    public string Code { get; set; }
    
    [Compare("Password", ErrorMessage = "كلمات المرور غير متطابقة")]
    public string ConfirmPassword { get; set; }
}
```

### Validation في Controller

```csharp
[HttpPost]
public async Task<IActionResult> Create(Product product)
{
    // تحقق من الـ Validation
    if (!ModelState.IsValid)
    {
        // لو في Errors، ارجع الـ Form مع الأخطاء
        return View(product);
    }
    
    await _service.CreateAsync(product);
    return RedirectToAction(nameof(Index));
}
```

### Validation في View

```html
@model Product

<form asp-action="Create" method="post">
    <div class="form-group">
        <label asp-for="Name">اسم المنتج</label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    
    <div class="form-group">
        <label asp-for="Price">السعر</label>
        <input asp-for="Price" class="form-control" type="number" step="0.01" />
        <span asp-validation-for="Price" class="text-danger"></span>
    </div>
    
    @* عرض Summary الأخطاء *@
    <div asp-validation-summary="All" class="text-danger"></div>
    
    <button type="submit" class="btn btn-primary">حفظ</button>
</form>

@section Scripts {
    @* Client-side Validation *@
    <partial name="_ValidationScripts" />
}
```

---

## 🎛️ Filters

الـ Filters بتتنفذ قبل أو بعد الـ Action.

```csharp
// 1. Authorization Filter
[Authorize]
[Authorize(Roles = "Admin,Manager")]

// 2. Action Filter — بيتنفذ قبل وبعد الـ Action
public class LogActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        Console.WriteLine($"Before: {actionName}");
    }
    
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine($"After Action");
    }
}

// 3. Exception Filter
public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Handle exception
        context.Result = new ViewResult { ViewName = "Error" };
        context.ExceptionHandled = true;
    }
}

// 4. Result Filter — بيتنفذ قبل وبعد الـ View
```

---

## 🔄 Data Flow في MVC

```
Complete Request/Response Flow:

1. Browser يبعت:
   GET /products/details/5

2. Kestrel يستقبل الـ Request

3. Middleware Pipeline:
   → Exception Handler
   → Authentication (إيه اليوزر؟)
   → Authorization (عنده صلاحية؟)
   → Routing (هيروح فين؟)

4. Routing Engine:
   /products/details/5
   → ProductsController + Details Action + id=5

5. Model Binding:
   id=5 يتحول من string لـ int تلقائياً

6. Action Filters:
   OnActionExecuting بيتنفذ

7. Action Method:
   public async Task<IActionResult> Details(int id)
   {
       var product = await _service.GetByIdAsync(id);
       return View(product);
   }

8. Service Call:
   _service.GetByIdAsync(5)
   → Database Query
   → Return Product Object

9. View Engine:
   اختار Views/Products/Details.cshtml
   + Model (Product Object)
   → اولد HTML

10. Response Pipeline:
    Result Filters تتنفذ
    HTML يترجع للـ Browser

11. Browser يعرض الصفحة ✓
```

---

## 💼 Interview Questions

**Q1: إيه الفرق بين ViewBag وViewData وTempData؟**
> ViewBag وViewData بيعيشوا في نفس الـ Request — بيضيعوا بعد Redirect. TempData بيبقى حتى الـ Request الجاي، مفيد جداً بعد RedirectToAction. ViewBag هو Wrapper فوق ViewData ومش Type-Safe.

**Q2: إيه هو Model Binding في ASP.NET MVC؟**
> هو عملية تلقائية بيقوم بيها ASP.NET بتحويل بيانات الـ HTTP Request (Form Data, Query String, Route Values) لـ C# Objects. بيعمل Casting تلقائي.

**Q3: إيه هو الفرق بين Partial View وView Component؟**
> Partial View هو قطعة HTML بسيطة مع Model بسيط. View Component هو أقوى - عنده Logic منفصل (زي Mini-Controller) ومناسب للحاجات المعقدة زي Navbar ديناميكية أو Shopping Cart.

**Q4: ما هو [ValidateAntiForgeryToken] وليه مهم؟**
> بيحمي من CSRF (Cross-Site Request Forgery). بيتأكد إن الـ POST Request جاي من صفحتك الفعلية مش من موقع تاني. لازم تحطه على كل POST Actions.

**Q5: إزاي بتنفذ Custom Validation في ASP.NET MVC؟**
> ممكن تعمل Custom ValidationAttribute عن طريق Inherit من ValidationAttribute وOverride IsValid method. أو ممكن تستخدم IValidatableObject في الـ Model نفسه.

---

## 📝 Mini Summary

```
MVC In Practice:

Controllers/    → بيستقبل Requests وبيوجه
Models/         → Domain Models + ViewModels + Validation
Views/          → Razor .cshtml Files + Layouts + Partials
Services/       → Business Logic منفصلة
Data/           → Database Context + Migrations
wwwroot/        → Static Files (CSS, JS, Images)

Key Concepts:
✓ Routing      → URL → Controller + Action
✓ Model Binding → HTTP Data → C# Objects
✓ Validation   → Data Annotations على الـ Models
✓ DI           → Services مسجلة في Program.cs
✓ TempData     → للـ Messages بعد Redirect
✓ Tag Helpers  → HTML مع C# بدون HTML Helpers القديمة
```

---

**التالي: [07 — URLs بالتفصيل](./07-urls-in-details.md) →**
