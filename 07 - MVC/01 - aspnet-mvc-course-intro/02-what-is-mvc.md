# 02 — ما هو MVC؟ 🎭

> **"MVC زي مطعم — في Menu (View)، في طباخ (Controller)، وفي المكونات (Model). كل واحد عنده شغله وما بيتدخلش في شغل الثاني"**

---

## 📋 Table of Contents

- [التعريف](#-التعريف)
- [المشكلة اللي بيحلها](#-المشكلة-اللي-بيحلها)
- [الـ 3 أجزاء بالتفصيل](#-الـ-3-أجزاء-بالتفصيل)
- [إزاي البيانات بتتحرك في MVC](#-إزاي-البيانات-بتتحرك-في-mvc)
- [MVC Flow Diagram](#-mvc-flow-diagram)
- [أمثلة عملية](#-أمثلة-عملية)
- [مقارنة مع الـ Patterns التانية](#-مقارنة-مع-الـ-patterns-التانية)
- [أخطاء شائعة](#-أخطاء-شائعة)
- [Best Practices](#-best-practices)
- [Interview Questions](#-interview-questions)
- [Mini Summary](#-mini-summary)

---

## 📖 التعريف

**MVC = Model + View + Controller**

ده **Architectural Pattern** — يعني طريقة لتنظيم الكود بتاعك.

الفكرة الأساسية: **فصل المسؤوليات (Separation of Concerns)**

```
Model     → بيتعامل مع البيانات (Data + Business Logic)
View      → بيتعامل مع العرض (UI + HTML)
Controller → بيتعامل مع التحكم (Request Handling + Orchestration)
```

---

## 🔧 المشكلة اللي بيحلها

### قبل MVC — الكود كان زباله 😅

تخيل كنا بنكتب ASP Classic أو PHP قديم:

```php
<!-- products.php — كل حاجة في ملف واحد! -->
<?php
// Database query
$conn = mysql_connect("localhost", "user", "pass");
$result = mysql_query("SELECT * FROM products");
?>
<html>
<body>
<?php while($row = mysql_fetch_array($result)) { ?>
    <div>
        <?php 
        // Business Logic جوا الـ HTML!
        $price = $row['price'] * 1.14; // Add VAT
        echo $row['name'] . " - " . $price;
        ?>
    </div>
<?php } ?>
</body>
</html>
```

**المشاكل:**
- ❌ الكود فوضى — كل حاجة مخلوطة مع بعض
- ❌ صعب تعدل في الـ Design من غير ما تكسر الـ Logic
- ❌ صعب تعمل Testing
- ❌ مش ممكن يشتغل عليه أكتر من Developer بسهولة

### مع MVC — الدنيا اتنظمت 🎉

```
المشكلة                    الحل في MVC
---                        ---
الكود مخلوط             →  كل حاجة في مكانها (M, V, C)
صعب التعديل             →  تعدل في View من غير ما تلمس Logic
صعب الـ Testing          →  تعمل Unit Tests على كل جزء لوحده
صعب التعاون             →  Developer على Model، تاني على View
```

---

## 🏗️ الـ 3 أجزاء بالتفصيل

### 1️⃣ MODEL — البيانات والـ Business Logic

الـ Model هو كل حاجة خاصة بالبيانات:

```
Model = Data + Rules + Database Operations
```

**أمثلة على Model:**

```csharp
// Models/Product.cs
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    
    // Business Logic جوا الـ Model
    public bool IsInStock => Stock > 0;
    
    public decimal GetPriceWithVAT()
    {
        return Price * 1.14m; // 14% VAT
    }
}

// Models/Order.cs
public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItem> Items { get; set; }
    public DateTime OrderDate { get; set; }
    
    // Computed Property
    public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
    
    // Business Rule
    public bool CanBeCancelled => 
        OrderDate > DateTime.Now.AddHours(-24) && Status == "Pending";
}
```

**أنواع الـ Models في ASP.NET:**

```
Models
├── Domain Models    → بيمثلوا الـ Database Tables
├── View Models      → بيتبعتوا للـ View (بس البيانات اللي الـ View محتاجها)
├── DTOs             → للتبادل مع الـ APIs
└── Input Models     → للـ Form Data من المستخدم
```

### 2️⃣ VIEW — الواجهة والـ HTML

الـ View مسؤول عن **عرض البيانات بس** — مش بيعمل أي Logic.

في ASP.NET بنستخدم **Razor** — وهو HTML عادي + C# Code:

```html
<!-- Views/Products/Index.cshtml -->
@model List<Product>

<h1>قائمة المنتجات</h1>

@if (Model.Count == 0)
{
    <p>مفيش منتجات متاحة حالياً</p>
}
else
{
    <table>
        <thead>
            <tr>
                <th>الاسم</th>
                <th>السعر</th>
                <th>الحالة</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var product in Model)
            {
                <tr>
                    <td>@product.Name</td>
                    <td>@product.Price.ToString("C")</td>
                    <td>@(product.IsInStock ? "متاح" : "غير متاح")</td>
                </tr>
            }
        </tbody>
    </table>
}
```

**قواعد الـ View:**
- ✅ بيعرض البيانات اللي الـ Controller بعتهالها
- ✅ ممكن يحتوي على Logic بسيطة (if, foreach)
- ❌ مش المفروض يتصل بالـ Database مباشرة
- ❌ مش المفروض يعمل Business Logic معقدة

### 3️⃣ CONTROLLER — المنظم والـ Traffic Director

الـ Controller هو اللي:
1. بيستقبل الـ Request
2. بيتصل بالـ Model عشان يجيب البيانات
3. بيبعت البيانات للـ View

```csharp
// Controllers/ProductsController.cs
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    
    // Dependency Injection
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    // GET /products
    public async Task<IActionResult> Index()
    {
        // 1. جيب البيانات من الـ Model/Service
        var products = await _productService.GetAllActiveProducts();
        
        // 2. ابعتها للـ View
        return View(products);
    }
    
    // GET /products/details/5
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetById(id);
        
        if (product == null)
            return NotFound(); // 404
            
        return View(product);
    }
    
    // POST /products/create
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
            return View(product); // رجّع الـ Form مع الأخطاء
            
        await _productService.Create(product);
        return RedirectToAction("Index");
    }
}
```

---

## 🔄 إزاي البيانات بتتحرك في MVC

```
Flow تفصيلي:

المتصفح
  |
  | GET /products/details/5
  ↓
Routing Engine
  | (بيعرف إن ProductsController + Details Action)
  ↓
ProductsController
  |
  ├─→ _productService.GetById(5)
  |         |
  |         ↓
  |    ProductService
  |         |
  |         ↓
  |    Database (EF Core)
  |         |
  |         ↓
  |    بيرجع Product Object
  |         |
  ←─────────┘
  |
  | return View(product)
  ↓
Razor View Engine
  | (بيأخد الـ .cshtml + Product Data)
  ↓
HTML يتولد
  |
  ↓
HTTP Response يترجع للمتصفح
  |
  ↓
المتصفح يعرض الصفحة ✓
```

---

## 📊 MVC Flow Diagram

```
┌─────────────────────────────────────────────────────┐
│                    MVC Pattern                       │
├───────────────┬─────────────────┬───────────────────┤
│    MODEL      │   CONTROLLER    │      VIEW          │
│               │                 │                    │
│  ┌─────────┐  │  ┌───────────┐  │  ┌─────────────┐  │
│  │Database │  │  │ Receives  │  │  │  HTML/Razor │  │
│  │  Data   │←─┤  │ Request   │  │  │   Template  │  │
│  └─────────┘  │  └─────┬─────┘  │  └──────┬──────┘  │
│               │        │        │         ↑          │
│  ┌─────────┐  │        ↓        │         │          │
│  │Business │  │  ┌───────────┐  │  ┌──────┴──────┐  │
│  │  Logic  │←─┤  │  Calls    │  │  │  Renders    │  │
│  └─────────┘  │  │  Model    │  │  │   Data      │  │
│               │  └─────┬─────┘  │  └─────────────┘  │
│               │        │        │         ↑          │
│               │        ↓        │         │          │
│               │  ┌───────────┐  │         │          │
│               │  │  Passes   ├──┼─────────┘          │
│               │  │ Data to   │  │                    │
│               │  │   View    │  │                    │
│               │  └───────────┘  │                    │
└───────────────┴─────────────────┴───────────────────┘
```

---

## 💻 أمثلة عملية

### مثال كامل: عرض قائمة طلاب

**Model:**
```csharp
// Models/Student.cs
public class Student
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "الاسم مطلوب")]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Range(16, 60)]
    public int Age { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    public string Grade { get; set; }
    
    // Business Logic
    public bool IsExcellent => Grade == "A";
}
```

**Controller:**
```csharp
// Controllers/StudentsController.cs
public class StudentsController : Controller
{
    private readonly SchoolDbContext _db;
    
    public StudentsController(SchoolDbContext db)
    {
        _db = db;
    }
    
    // قائمة كل الطلاب
    public async Task<IActionResult> Index()
    {
        var students = await _db.Students
            .OrderBy(s => s.Name)
            .ToListAsync();
        return View(students);
    }
    
    // تفاصيل طالب معين
    public async Task<IActionResult> Details(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student == null) return NotFound();
        return View(student);
    }
    
    // فورم إضافة طالب
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    // حفظ الطالب الجديد
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (ModelState.IsValid)
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
            TempData["Success"] = "تم إضافة الطالب بنجاح!";
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }
}
```

**View:**
```html
<!-- Views/Students/Index.cshtml -->
@model List<Student>

@{
    ViewData["Title"] = "قائمة الطلاب";
}

<div class="container">
    <h2>@ViewData["Title"]</h2>
    
    @if (TempData["Success"] != null)
    {
        <div class="alert alert-success">@TempData["Success"]</div>
    }
    
    <a asp-action="Create" class="btn btn-primary">+ إضافة طالب جديد</a>
    
    <table class="table mt-3">
        <thead>
            <tr>
                <th>الاسم</th>
                <th>السن</th>
                <th>الدرجة</th>
                <th>الإجراءات</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var student in Model)
            {
                <tr class="@(student.IsExcellent ? "table-success" : "")">
                    <td>@student.Name</td>
                    <td>@student.Age</td>
                    <td>@student.Grade</td>
                    <td>
                        <a asp-action="Details" asp-route-id="@student.Id">تفاصيل</a>
                        <a asp-action="Edit" asp-route-id="@student.Id">تعديل</a>
                    </td>
                </tr>
            }
        </tbody>
    </table>
</div>
```

---

## ⚖️ مقارنة مع الـ Patterns التانية

| Pattern | الوصف | متى تستخدمه |
|---------|-------|------------|
| **MVC** | Model-View-Controller | Web Apps تقليدية مع Server-Side Rendering |
| **MVVM** | Model-View-ViewModel | Blazor, WPF, Mobile Apps |
| **MVP** | Model-View-Presenter | WinForms, Android القديم |
| **API-Only** | بس Controllers بدون Views | REST APIs اللي Front-End منفصل |
| **Razor Pages** | Page-based Model | أبسط من MVC لصفحات بسيطة |

```
متى تختار إيه؟

Server-Side Web App كامل  →  MVC
REST API فقط             →  API Controllers (بدون Views)
صفحات بسيطة             →  Razor Pages
Frontend منفصل (React)  →  ASP.NET Core Web API
```

---

## ⚠️ أخطاء شائعة

### 1. Thin Model — Fat Controller ❌
```csharp
// ❌ غلط — Controller بيعمل Business Logic
public IActionResult ProcessOrder(Order order)
{
    // كل ده في الـ Controller — غلط!
    decimal total = 0;
    foreach (var item in order.Items)
    {
        total += item.Price * item.Quantity;
        if (item.Quantity > 100) total *= 0.9m; // Discount
    }
    order.Total = total;
    order.Status = "Processing";
    SendEmail(order.CustomerEmail, "Order received");
    _db.Orders.Add(order);
    _db.SaveChanges();
    return View();
}

// ✅ صح — Controller رفيع
public async Task<IActionResult> ProcessOrder(Order order)
{
    var result = await _orderService.ProcessAsync(order);
    return View(result);
}
```

### 2. Logic في الـ View ❌
```html
<!-- ❌ غلط — Business Logic في الـ View -->
@{
    decimal tax = 0;
    if (Model.Country == "Egypt") tax = 0.14m;
    else if (Model.Country == "UAE") tax = 0.05m;
    decimal total = Model.Price + (Model.Price * tax);
}

<!-- ✅ صح — الحساب في الـ Model أو Service -->
<span>@Model.TotalWithTax</span>
```

### 3. نسيان ViewModels ❌
```csharp
// ❌ غلط — بعت الـ Database Model للـ View مباشرة (Security Risk)
return View(userFromDatabase); // فيه Password Hash وبيانات حساسة!

// ✅ صح — استخدم ViewModel
var viewModel = new UserViewModel 
{
    Name = user.Name,
    Email = user.Email
    // Password مش موجود هنا
};
return View(viewModel);
```

---

## ✅ Best Practices

1. **Thin Controllers** — الـ Controller بس بيوجه، مش بيشتغل
2. **Fat Models/Services** — الـ Business Logic في الـ Services
3. **Use ViewModels** — متبعتش الـ Database Models للـ View
4. **Async Everything** — استخدم async/await في كل الـ Actions
5. **Validate Input** — استخدم Data Annotations
6. **Handle Errors** — اعمل Error Pages مناسبة

---

## 💼 Interview Questions

**Q1: إيه هو الفرق بين MVC و Razor Pages؟**
> MVC بيفصل بين Model/View/Controller في مجلدات منفصلة وبيناسب Apps كبيرة ومعقدة. Razor Pages بتدمج Logic مع View في ملف واحد (.cshtml + .cshtml.cs) وبتناسب صفحات بسيطة.

**Q2: إيه هو ViewModel وليه بنستخدمه؟**
> ViewModel هو Class بيحتوي على البيانات اللي الـ View بس محتاجها. بنستخدمه عشان: (1) نحمي البيانات الحساسة، (2) ندمج بيانات من أكتر من Model، (3) نضيف Validation خاص بالـ UI.

**Q3: ممكن الـ View يتصل بالـ Database مباشرة؟**
> ممكن تقنياً لكن ده Bad Practice جداً! بيكسر مبدأ الـ Separation of Concerns ويخلي الكود صعب الـ Testing والصيانة.

**Q4: إيه الفرق بين TempData و ViewBag و ViewData؟**
> ViewBag و ViewData: بيعملوا طريق للـ Controller إنه يبعت Data للـ View في نفس الـ Request. TempData: بيحتفظ بالبيانات لحد الـ Request الجاي (مفيد بعد Redirect). راجع الشرح التفصيلي في الملف 06.

**Q5: إيه هو الـ Action Filter؟**
> هو Code بيتنفذ قبل أو بعد الـ Action Method. بنستخدمه لـ: Logging، Authentication، Caching، Error Handling. بيعمل Cross-Cutting Concerns.

---

## 📝 Mini Summary

```
MVC = نمط لتنظيم الكود في 3 أجزاء منفصلة

Model      → البيانات وقواعد الـ Business
View       → عرض البيانات بس (HTML + Razor)
Controller → التنسيق بين M و V

الفايدة:
✓ كل جزء عنده مسؤولية واحدة
✓ سهل تعدل في جزء من غير ما تكسر التاني
✓ سهل التعاون في الفريق
✓ سهل الـ Testing

القاعدة الذهبية:
"Fat Model (Services), Thin Controller, Dumb View"
```

---

**التالي: [03 — HTTP بالتفصيل](./03-http-complete.md) →**
