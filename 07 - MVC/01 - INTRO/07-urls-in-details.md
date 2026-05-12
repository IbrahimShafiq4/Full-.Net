# 07 — URLs 🔗

> **"URL زي عنوان بيتك — لو كتبته صح، أي حد يوصلك. لو كتبته غلط، حد هيتوه!"**

---

## 📋 Table of Contents

- [07 — URLs 🔗](#07--urls-)
  - [📋 Table of Contents](#-table-of-contents)
  - [📖 ما هو URL؟](#-ما-هو-url)
  - [🔍 تشريح الـ URL الكامل](#-تشريح-الـ-url-الكامل)
  - [🌐 Protocol/Scheme](#-protocolscheme)
  - [🏢 Domain و Subdomain](#-domain-و-subdomain)
    - [Domain Structure](#domain-structure)
    - [أنواع الـ TLDs](#أنواع-الـ-tlds)
    - [Subdomains الشائعة](#subdomains-الشائعة)
  - [🔌 Port](#-port)
    - [Default Ports](#default-ports)
  - [📂 Path](#-path)
    - [Path في ASP.NET Core MVC](#path-في-aspnet-core-mvc)
    - [Path Conventions](#path-conventions)
  - [❓ Query String](#-query-string)
    - [Query String في ASP.NET Core](#query-string-في-aspnet-core)
    - [Query String vs Route Parameters](#query-string-vs-route-parameters)
  - [#️⃣ Fragment](#️⃣-fragment)
    - [خصائص مهمة](#خصائص-مهمة)
  - [🔢 Route Parameters](#-route-parameters)
    - [في ASP.NET Core](#في-aspnet-core)
  - [🔤 URL Encoding](#-url-encoding)
    - [المحارف الخاصة](#المحارف-الخاصة)
    - [مثال عملي](#مثال-عملي)
    - [في ASP.NET Core](#في-aspnet-core-1)
  - [🔍 SEO Friendly URLs](#-seo-friendly-urls)
    - [قواعد SEO Friendly URLs](#قواعد-seo-friendly-urls)
    - [Custom Slugs في ASP.NET Core](#custom-slugs-في-aspnet-core)
  - [📍 Absolute vs Relative URLs](#-absolute-vs-relative-urls)
    - [Absolute URL](#absolute-url)
    - [Relative URL](#relative-url)
    - [في ASP.NET Core](#في-aspnet-core-2)
  - [🛠️ URLs في ASP.NET Core](#️-urls-في-aspnet-core)
    - [بناء URLs](#بناء-urls)
    - [Canonical URLs للـ SEO](#canonical-urls-للـ-seo)
  - [💼 Interview Questions](#-interview-questions)
  - [📝 Mini Summary](#-mini-summary)

---

## 📖 ما هو URL؟

**URL = Uniform Resource Locator**

هو **عنوان فريد** لأي Resource على الـ Internet.

```
URL زي العنوان بالظبط:
"مصر، القاهرة، المعادي، شارع 9، عمارة 15، شقة 3"

URL على الإنترنت:
"https://shop.example.com:443/products/details?id=5&color=red#reviews"
   ↑         ↑           ↑      ↑           ↑      ↑            ↑
Protocol  Subdomain    Port   Path       Action  Query        Fragment
```

---

## 🔍 تشريح الـ URL الكامل

```
https://shop.example.com:5001/products/details?id=10&color=blue#reviews
  │       │       │       │      │        │     │              │
  │       │       │       │      │        │     └──────────────┘
  │       │       │       │      │        │       Query String
  │       │       │       │      │        │
  │       │       │       │      └────────┘
  │       │       │       │         Path
  │       │       │       │
  │       │       │       └── Port
  │       │       │
  │       └───────┘
  │        Domain (example.com) + Subdomain (shop)
  │
  └── Protocol (https)
  
كمان:
https://shop.example.com:5001/products/details?id=10#reviews
                                                    ↑
                                               Fragment (#reviews)
```

---

## 🌐 Protocol/Scheme

الـ Protocol هو **طريقة الاتصال** — زي اللغة اللي بيتكلموا بيها.

```
http://    → HTTP (غير مشفر) ← مش آمن!
https://   → HTTPS (مشفر)   ← آمن ✅
ftp://     → File Transfer Protocol
mailto:    → لفتح برنامج الـ Email
tel:       → لتشغيل المكالمة
file://    → ملف محلي على جهازك

أمثلة:
https://google.com           ← موقع عادي
http://old-site.com          ← قديم وغير آمن
ftp://files.server.com       ← رفع/تحميل ملفات
mailto:info@example.com      ← فتح Email Client
tel:+201001234567            ← اتصال هاتفي
```

---

## 🏢 Domain و Subdomain

### Domain Structure

```
shop.example.com.eg
  │      │    │   │
  │      │    │   └── Country Code TLD (ccTLD)
  │      │    └────── TLD (Top Level Domain)
  │      └─────────── SLD (Second Level Domain)
  └────────────────── Subdomain
```

### أنواع الـ TLDs

```
Generic TLDs:
.com → Commercial (الأشهر)
.org → Organizations
.net → Networks
.edu → Education
.gov → Government

Country TLDs:
.eg  → Egypt
.uk  → United Kingdom
.de  → Germany
.jp  → Japan

New TLDs:
.app, .dev, .io, .ai, .shop, .store, .tech
```

### Subdomains الشائعة

```
www.example.com       ← الموقع الرئيسي
api.example.com       ← الـ API
admin.example.com     ← لوحة الإدارة
shop.example.com      ← المتجر
blog.example.com      ← المدونة
mail.example.com      ← الـ Email
cdn.example.com       ← Content Delivery Network
dev.example.com       ← Development
staging.example.com   ← Staging
```

---

## 🔌 Port

الـ Port هو **"باب" معين** للخدمة.

```
IP Address = عنوان المبنى
Port       = رقم الشقة في المبنى

مثال:
Server IP: 192.168.1.10
Port 80   = HTTP Service
Port 443  = HTTPS Service
Port 3306 = MySQL Database
Port 5432 = PostgreSQL Database
Port 22   = SSH
```

### Default Ports

```
http://example.com     ← Port 80 افتراضي (مش لازم تكتبه)
https://example.com    ← Port 443 افتراضي (مش لازم تكتبه)
https://example.com:443 ← نفس الحاجة

غير الافتراضي:
http://localhost:5000    ← ASP.NET Core Development
https://localhost:5001   ← ASP.NET Core Development HTTPS
http://localhost:3000    ← Node.js شائع
http://localhost:8080    ← شائع في Development
```

---

## 📂 Path

الـ Path بيحدد **أي Resource على الـ Server** إنت عايزه.

```
https://example.com/products/electronics/laptops/details/5
                     ↑        ↑           ↑       ↑      ↑
                   Segment1 Segment2   Segment3 Segment4 Segment5
```

### Path في ASP.NET Core MVC

```
/                              → Default Route (HomeController.Index)
/home                          → HomeController.Index
/home/about                    → HomeController.About
/products                      → ProductsController.Index
/products/details/5            → ProductsController.Details(id=5)
/admin/users/edit/10           → Admin/UsersController.Edit(id=10)
/api/v1/products               → API Route
```

### Path Conventions

```
✅ Good Practices:
/products                    ← Plural nouns
/products/5                  ← Resource by ID
/products/5/reviews          ← Nested Resource
/products/category/laptops   ← Filtered by category
/account/settings            ← User-related pages

❌ Bad Practices:
/getProducts                 ← Verbs في URL (REST violation)
/ProductsList.aspx           ← Extension في URL (قديم)
/p/5                         ← Abbreviation غير واضح
/product_details             ← Underscore (يفضل Hyphen)
```

---

## ❓ Query String

الـ Query String هو **بيانات إضافية** بتتبعت مع الـ URL.

```
URL مع Query String:
https://example.com/products?category=laptops&price_max=5000&page=2&sort=price_asc

يبدأ بـ ?
كل Parameter: name=value
بينهم &

تحليل:
? ← بداية الـ Query String
category=laptops    ← Parameter 1
&                   ← Separator
price_max=5000      ← Parameter 2
&                   ← Separator
page=2              ← Parameter 3
&                   ← Separator
sort=price_asc      ← Parameter 4
```

### Query String في ASP.NET Core

```csharp
// الـ URL: /products?category=laptops&page=2&sort=price
public IActionResult Index(string category, int page = 1, string sort = "name")
{
    // ASP.NET Core بيعمل Model Binding تلقائياً من الـ Query String
    var products = _service.GetFiltered(category, page, sort);
    return View(products);
}

// قراءة يدوية
var category = Request.Query["category"].ToString();
var page = int.Parse(Request.Query["page"].FirstOrDefault() ?? "1");

// بناء Query String في الـ View
<a asp-action="Index" 
   asp-route-category="laptops"
   asp-route-page="2"
   asp-route-sort="price">
    تصفح اللابتوبات
</a>
// ← ينتج: /products?category=laptops&page=2&sort=price
```

### Query String vs Route Parameters

```
متى تستخدم Route Parameters؟
/products/5          ← ID (Resource Identifier) - Route
/users/ahmed/profile ← Hierarchical - Route

متى تستخدم Query String؟
/products?page=2           ← Pagination
/products?sort=price       ← Sorting
/products?search=laptop    ← Search
/products?min=100&max=500  ← Filtering
```

---

## #️⃣ Fragment

الـ Fragment (أو Anchor أو Hash) بيوجهك لـ **جزء معين في الصفحة**.

```
https://docs.example.com/guide#installation

# → يبدأ الـ Fragment
installation → ID بتاع العنصر في الصفحة

HTML:
<h2 id="installation">التثبيت</h2>  ← بيتنقل لهنا
```

### خصائص مهمة

```
⚠️ الـ Fragment مش بيتبعت للـ Server!
الـ Browser بس هو اللي بيفسره وبيعمل Scroll

استخدامات:
- التنقل داخل صفحة طويلة (Documentation)
- جدول المحتويات (Table of Contents)
- Single Page Applications (SPA) بيستخدموا # للـ Client-side Routing
```

---

## 🔢 Route Parameters

الـ Route Parameters هي **جزء من الـ Path** بيتغير.

```
/products/{id}
/users/{username}/posts/{postId}
/blog/{year}/{month}/{slug}
```

### في ASP.NET Core

```csharp
// Convention-based Route
app.MapControllerRoute(
    name: "blog",
    pattern: "blog/{year}/{month}/{slug}",
    defaults: new { controller = "Blog", action = "Post" }
);

// Attribute Routing
[Route("blog/{year:int:min(2000)}/{month:int:range(1,12)}/{slug}")]
public IActionResult Post(int year, int month, string slug)
{
    // يشتغل على: /blog/2026/05/my-first-post
}

// Optional Parameter
[Route("products/{category?}")]
public IActionResult Index(string? category)
{
    // يشتغل على: /products أو /products/electronics
}

// Catch-all Parameter
[Route("files/{*filepath}")]
public IActionResult File(string filepath)
{
    // يشتغل على: /files/docs/2026/report.pdf
    // filepath = "docs/2026/report.pdf"
}
```

---

## 🔤 URL Encoding

بعض الحروف مش مسموح بيها في الـ URL — لازم تتحول لـ Encoded Format.

### المحارف الخاصة

```
Space    → %20  أو +
#        → %23
&        → %26
+        → %2B
=        → %3D
/        → %2F
?        → %3F
@        → %40
:        → %3A
العربي و → %D9%88  (UTF-8 Encoded)
أ        → %D8%A3
```

### مثال عملي

```
بدون Encoding:
/search?q=لابتوب جيمنج 2026

مع Encoding:
/search?q=%D9%84%D8%A7%D8%A8%D8%AA%D9%88%D8%A8+%D8%AC%D9%8A%D9%85%D9%86%D8%AC+2026
```

### في ASP.NET Core

```csharp
// Encoding
var encoded = Uri.EscapeDataString("مرحبا بالعالم");
// → "%D9%85%D8%B1%D8%AD%D8%A8%D8%A7+%D8%A8%D8%A7%D9%84%D8%B9%D8%A7%D9%84%D9%85"

// Decoding
var decoded = Uri.UnescapeDataString("%D9%85%D8%B1%D8%AD%D8%A8%D8%A7");
// → "مرحبا"

// في JavaScript
encodeURIComponent("مرحبا بالعالم")
decodeURIComponent("...")
```

---

## 🔍 SEO Friendly URLs

### قواعد SEO Friendly URLs

```
✅ DO:
/products/laptop-gaming-2026          ← Hyphens للفصل
/blog/how-to-learn-aspnet            ← Descriptive
/ar/products/laptops                  ← Language prefix
/products?category=electronics&page=1 ← Query String للـ Filtering

❌ DON'T:
/p?id=5&cat=3                        ← مش Descriptive
/ProductDetails.aspx?productId=123   ← File extension ومش واضح
/products/product_details_page       ← Underscores
/PRODUCTS/LAPTOP                     ← Capital Letters
```

### Custom Slugs في ASP.NET Core

```csharp
// Model
public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; } // "my-first-post"
    public string Content { get; set; }
}

// توليد Slug من الـ Title
public static string GenerateSlug(string title)
{
    return title
        .ToLowerInvariant()
        .Replace(" ", "-")
        .Replace("--", "-")
        .Trim('-');
    // "مشروع لابتوب جيمنج" → لازم تستخدم Library زي Slugify
}

// Route
[Route("blog/{slug}")]
public async Task<IActionResult> Post(string slug)
{
    var post = await _db.BlogPosts
        .FirstOrDefaultAsync(p => p.Slug == slug);
    
    if (post == null) return NotFound();
    return View(post);
}
```

---

## 📍 Absolute vs Relative URLs

### Absolute URL

```
URL كامل من البداية:
https://example.com/products/details/5

استخدامه:
- في الـ Emails
- في الـ APIs
- لما بتشير لـ Domain تاني
```

### Relative URL

```
URL جزئي — بيبنى على الـ Base URL الحالي:

/products         ← Root-relative (دايماً من الـ Domain)
products/details  ← Document-relative (نسبة للصفحة الحالية)
../images/pic.jpg ← Up one level
./style.css       ← نفس المجلد
```

### في ASP.NET Core

```csharp
// Absolute URL
var absoluteUrl = Url.Action("Details", "Products", 
    new { id = 5 }, Request.Scheme, Request.Host.ToString());
// → https://example.com/products/details/5

// Relative URL
var relativeUrl = Url.Action("Details", "Products", new { id = 5 });
// → /products/details/5

// في الـ View
@Url.Action("Index", "Home")              ← /home/index
@Url.RouteUrl("default", new { id = 5 }) ← /home/index/5
```

---

## 🛠️ URLs في ASP.NET Core

### بناء URLs

```csharp
// في Controller
var url = Url.Action("Details", "Products", new { id = 5 });

// في View
<a asp-controller="Products" asp-action="Details" asp-route-id="5">
    رابط المنتج
</a>

// في Service (مع IUrlHelper)
public class EmailService
{
    private readonly IUrlHelper _url;
    
    public string GetProductUrl(int productId)
    {
        return _url.Action("Details", "Products", 
            new { id = productId }, "https");
    }
}
```

### Canonical URLs للـ SEO

```csharp
// منع Duplicate Content - استخدام Canonical
[Route("products")]
[Route("product-list")]  // Alias
public IActionResult Index()
{
    // الـ Canonical هو /products
    return View();
}
```

```html
<!-- في الـ Layout أو View -->
<link rel="canonical" href="https://example.com/products" />
```

---

## 💼 Interview Questions

**Q1: إيه الفرق بين Route Parameters وQuery String؟**
> Route Parameters جزء من الـ Path (/products/5) - بتستخدمه للـ Resource Identifiers والـ Hierarchical Relationships. Query String بيجي بعد ? (/products?page=2) - بتستخدمه للـ Filtering والـ Sorting والـ Pagination.

**Q2: إيه هو URL Encoding وامتى بنحتاجه؟**
> هو تحويل الحروف الخاصة (مسافات، عربي، رموز) لـ %XX Format عشان تبقى URL-safe. بنحتاجه لما بيكون في Query String فيه Spaces أو حروف خاصة أو Unicode. ASP.NET بيعملها تلقائياً في معظم الحالات.

**Q3: إيه هو الفرق بين Absolute وRelative URL؟**
> Absolute URL كامل يشمل Protocol + Domain + Path. Relative URL جزئي ومتعمل بناءاً على الـ Base URL. Absolute مهم في Emails والـ External References. Relative أسهل في الـ Development ومرن مع تغيير الـ Domain.

**Q4: إزاي تعمل SEO Friendly URLs في ASP.NET Core؟**
> باستخدام Attribute Routing مع Slugs (حروف صغيرة + hyphens)، وضبط Custom Routes بدل الـ Default Convention. أيضاً استخدام Canonical URLs لمنع Duplicate Content.

**Q5: إيه هو الـ Fragment وبيتبعت للـ Server؟**
> الـ Fragment (الجزء بعد #) مش بيتبعت للـ Server - ده Behavior خاص بالـ Browser بس. بيتستخدم للـ In-page Navigation وفي الـ SPAs للـ Client-side Routing.

---

## 📝 Mini Summary

```
URL Structure:
https://shop.example.com:5001/products/details?id=10&color=blue#reviews
   │         │          │      │            │    │                 │
Protocol  Domain+Sub   Port   Path        Action Query String   Fragment

أهم Rules:
✓ HTTPS دايماً في Production
✓ Lowercase للـ URLs
✓ Hyphens (-) مش Underscores (_)
✓ Descriptive Paths
✓ Query String للـ Filtering والـ Pagination
✓ Route Parameters للـ Resource IDs
✓ URL Encoding للـ Special Characters
```

---

**التالي: [08 — المشاريع العملية](./08-projects.md) →**
