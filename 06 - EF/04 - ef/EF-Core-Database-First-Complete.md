# 📦 Entity Framework Core — Database First Approach
### شرح شامل ومفصل بالعامية المصرية

---

## 📋 فهرس المحتويات

| # | الموضوع |
|---|---------|
| 01 | [ما هو Database First؟](#01--ما-هو-database-first) |
| 02 | [EF Core Power Tools — التثبيت والإعداد](#02--ef-core-power-tools--التثبيت-والإعداد) |
| 03 | [طرق الـ Authentication في SQL Server](#03--طرق-الـ-authentication-في-sql-server) |
| 04 | [Advanced Connection Settings](#04--advanced-connection-settings) |
| 05 | [Code Generation Templates](#05--code-generation-templates) |
| 06 | [Stored Procedures](#06--stored-procedures) |
| 07 | [Threading و Async/Await](#07--threading-و-asyncawait) |
| 08 | [FromSqlRaw vs FromSqlInterpolated](#08--fromsqlraw-vs-fromsqlinterpolated) |
| 09 | [IQueryable vs IEnumerable](#09--iqueryable-vs-ienumerable) |
| 10 | [Partial Classes](#10--partial-classes) |
| 11 | [Lazy vs Eager vs Explicit Loading](#11--lazy-vs-eager-vs-explicit-loading) |
| 12 | [N+1 Query Problem](#12--n1-query-problem) |
| 13 | [Reference vs Collection](#13--reference-vs-collection) |
| 14 | [Remote vs Local](#14--remote-vs-local) |
| 15 | [JOIN — Fluent vs Query Syntax](#15--join--fluent-vs-query-syntax) |
| 16 | [Tracking vs No Tracking](#16--tracking-vs-no-tracking) |
| 17 | [ChangeTracker بالتفصيل](#17--changetracker-بالتفصيل) |
| 18 | [using keyword وإدارة الـ Resources](#18--using-keyword-وإدارة-الـ-resources) |

---

## 01 — ما هو Database First؟

### التعريف

في Entity Framework Core في **ثلاث طرق** تشتغل بيها:

```
┌─────────────────────────────────────────────────────────────┐
│                    EF Core Approaches                        │
│                                                             │
│  1️⃣  Code First                                             │
│      انت بتكتب الـ C# Classes الأول                         │
│      وبعدين EF بيعمل الـ Database                            │
│                                                             │
│  2️⃣  Database First  ← ده اللي احنا شغالين بيه             │
│      عندك Database موجودة خلاص                              │
│      وعايز EF يعملك الـ C# Classes منها                     │
│                                                             │
│  3️⃣  Model First (قديم - مش بيتستخدم دلوقتي)               │
│      بترسم الـ Model وبيولد DB وCode                         │
└─────────────────────────────────────────────────────────────┘
```

### الـ Database First بالتفصيل

**الفكرة:** عندك Database جاهزة (ممكن تكون قديمة أو ورثتها من حد تاني)، وعايز تتعامل معها بـ C# بدون ما تكتب SQL كتير.

```
Database موجودة (SQL Server)
         │
         │  Reverse Engineering
         ▼
EF Core Power Tools بيقرأ الـ Database
         │
         ▼
بيولد ليك C# Classes تلقائياً:
 ├── Models (Product, Category, ...)
 ├── DbContext (NorthwindContext)
 ├── Stored Procedures Interfaces
 └── Views كـ Keyless Entities
```

### امتى تستخدم Database First؟

| الموقف | الاختيار |
|--------|---------|
| عندك DB قديمة وعايز تتعامل معها | ✅ Database First |
| بتشتغل مع DBA وهو اللي بيعمل الـ DB | ✅ Database First |
| بتشتغل على نظام Legacy | ✅ Database First |
| بتبدأ مشروع من الصفر | ❌ استخدم Code First |
| عايز تتحكم في الـ Schema بالكود | ❌ استخدم Code First |

---

## 02 — EF Core Power Tools — التثبيت والإعداد

### خطوات التثبيت خطوة بخطوة

```
الخطوة 1: تثبيت الـ Extension
─────────────────────────────────────────────
Extensions
    └── Manage Extensions
            └── ابحث عن "EF Core Power Tools"
                    └── Install
                            └── أغلق Visual Studio وافتحه تاني
```

> [!WARNING]
> لازم تتأكد إن الـ Extension متوافق مع إصدار الـ Visual Studio بتاعك. في إصدارات لـ VS 2019، VS 2022، إلخ.

### خطوات الـ Reverse Engineering

```
الخطوة 2: عمل Reverse Engineer
─────────────────────────────────────────────
Right Click على المشروع
    └── EF Core Power Tools
            └── Reverse Engineer
```

**شاشة الاتصال بالـ Database:**

```
┌─────────────────────────────────────────┐
│     Add Database Connection              │
├─────────────────────────────────────────┤
│  Server Name:  [.] أو [DESKTOP\SQLEXPR] │
│  Authentication: [Windows Auth ▼]        │
│  ☑ Trust Server Certificate              │
│  Database: [Northwind ▼]                 │
└─────────────────────────────────────────┘
```

**الـ Dropdown اللي بيظهر:**

```
1. Add database connection    ← هنختار ده
2. Add .dacpac file           ← لو عندك نسخة .dacpac من الـ DB
3. Add custom connection      ← لو عندك Connection String خاص
```

---

### اختيار الـ Tables/Views/Stored Procedures

```
الخطوة 3: تحديد اللي عايزه
─────────────────────────────────────────────

📁 Tables (dbo)
   ☑ Categories
   ☑ Customers
   ☑ Products
   ☑ Suppliers

📁 Views (dbo)
   ☑ Products Above Average Price
   ☑ Products By Category

📁 Stored Procedures (dbo)
   ☑ CustOrdersDetails
   ☑ CustOrdersOrders
   ☑ Employee Sales by Country
   ☑ Sales by Year
   ☑ SalesByCategory
```

---

### إعدادات توليد الكود

```
الخطوة 4: إعدادات الـ Code Generation
─────────────────────────────────────────────

Context Name:       NorthwindContext    ← اسم الـ DbContext
Namespace:          EF04                ← نفس namespace المشروع
EntityTypes Path:   Models              ← مجلد الـ Models

What to generate:
   ◉ EntityTypes & DbContext    ← ده الأشيع
   ○ DbContext only
   ○ EntityTypes only

Naming:
   ☐ Pluralize or singularize names
   ☐ Use table names directly from DB
```

---

### الـ Output اللي بيتولد

```
📁 المشروع
├── 📁 Context
│   ├── NorthwindContext.cs              ← الـ DbContext الرئيسي
│   ├── INorthwindContextProcedures.cs   ← Interface للـ Stored Procedures
│   └── NorthwindContextProcedures.cs   ← Implementation للـ Stored Procedures
│
└── 📁 Models
    ├── Category.cs
    ├── Customer.cs
    ├── Product.cs
    ├── Supplier.cs
    ├── ProductsAboveAveragePrice.cs     ← View (Keyless)
    ├── ProductsByCategory.cs            ← View (Keyless)
    ├── SalesByCategoryResult.cs         ← Result of Stored Procedure
    ├── CustOrdersDetailsResult.cs
    └── ...
```

### ما معنى `[Keyless]`؟

```csharp
// لما تلاقي فوق الـ Class ده الـ Attribute ده:
[Keyless]
public partial class ProductsAboveAveragePrice
{
    public string? ProductName { get; set; }
    public decimal? UnitPrice { get; set; }
}
```

> **الـ `[Keyless]` معناه:** الـ Class ده بيمثل **View** في الـ Database مش Table — والـ View مالهاش Primary Key، فـ EF لازم يعرف ده عشان ما يحاولش يعمل Insert/Update/Delete عليها.

---

### لو اتغيرت الـ Database

```
Right Click على المشروع
    └── EF Core Power Tools
            └── Update Model from Database
```

> [!WARNING]
> **تحذير مهم جداً:** لو عملت أي تعديلات يدوية على الـ Models، هتتمسح بعد الـ Update! الحل هو عمل **Partial Classes** زي ما هنشرح في القسم 10.

---

## 03 — طرق الـ Authentication في SQL Server

دي الطرق المختلفة اللي تقدر تتصل بيها بالـ SQL Server:

### 🔵 1. Windows Authentication

```
┌─────────────────────────────────────────────────────────────┐
│  Windows Authentication                                      │
│                                                             │
│  بيستخدم هويتك في Windows (Username + Password بتاع Windows)│
│  مش محتاج Username وPassword في SQL Server                  │
│                                                             │
│  Connection String:                                         │
│  Server=.;Database=Northwind;Integrated Security=True;      │
└─────────────────────────────────────────────────────────────┘
```

**امتى تستخدمه؟**
- في بيئات الشركات (Corporate Environments)
- لما السيرفر والتطبيق على نفس الـ Domain
- أكتر أماناً لأنه مش بيخزن Passwords في الكود

---

### 🟡 2. SQL Server Authentication

```
┌─────────────────────────────────────────────────────────────┐
│  SQL Server Authentication                                   │
│                                                             │
│  Username + Password بتاع SQL Server نفسه                   │
│  (مش بتاع Windows)                                          │
│                                                             │
│  Connection String:                                         │
│  Server=.;Database=Northwind;User Id=sa;Password=myPass;    │
└─────────────────────────────────────────────────────────────┘
```

**امتى تستخدمه؟**
- لما التطبيق مش على نفس الـ Domain
- للـ Development والـ Testing المحلي
- للاتصال من أنظمة تشغيل مختلفة (Linux, Mac)

> [!WARNING]
> متحطش الـ Password في الكود! استخدم Environment Variables أو Secret Manager.

---

### 🟢 3. Microsoft Entra Password

```
┌─────────────────────────────────────────────────────────────┐
│  Microsoft Entra Password (كان اسمه Azure AD Password)      │
│                                                             │
│  بيستخدم Username + Password بتاع Microsoft Entra (Azure AD)│
│  يعني حساب الشغل بتاعك على Office 365 / Azure              │
└─────────────────────────────────────────────────────────────┘
```

**امتى تستخدمه؟**
- لما الـ Database على Azure SQL
- لما الشركة بتستخدم Microsoft 365

---

### 🟢 4. Microsoft Entra Integrated

```
┌─────────────────────────────────────────────────────────────┐
│  Microsoft Entra Integrated                                  │
│                                                             │
│  نفس Windows Authentication بس على Azure                    │
│  بيستخدم الـ Token بتاع Azure AD اللي انت متسجل بيه        │
│  مش محتاج تدخل Username/Password                            │
└─────────────────────────────────────────────────────────────┘
```

---

### 🔵 5. Microsoft Entra MFA

```
┌─────────────────────────────────────────────────────────────┐
│  Microsoft Entra MFA (Multi-Factor Authentication)          │
│                                                             │
│  Username + Password + كود تاني (SMS أو Authenticator App)  │
│  أعلى مستوى أمان                                            │
│  بيفتحلك Browser عشان تعمل Login                            │
└─────────────────────────────────────────────────────────────┘
```

---

### 🔵 6. Microsoft Entra Service Principal

```
┌─────────────────────────────────────────────────────────────┐
│  Microsoft Entra Service Principal                          │
│                                                             │
│  مش بتاع إنسان — بتاع Application أو Service               │
│  بيستخدم Client ID + Client Secret (أو Certificate)         │
│  للـ Automated Systems والـ CI/CD Pipelines                 │
└─────────────────────────────────────────────────────────────┘
```

**مثال:** لما الـ CI/CD Pipeline بتاعك (GitHub Actions / Azure DevOps) بيتصل بالـ Database، مش بيستخدم حساب إنسان.

---

### 🟣 7. Microsoft Entra Managed Identity

```
┌─────────────────────────────────────────────────────────────┐
│  Microsoft Entra Managed Identity                           │
│                                                             │
│  للـ Azure Resources (VMs, App Services, Functions)         │
│  مش محتاج تدير Passwords خالص                               │
│  Azure بيدير الهوية تلقائياً                                 │
│                                                             │
│  أنواعه:                                                    │
│  - System-assigned: بيتمسح مع الـ Resource                  │
│  - User-assigned:   مستقل عن الـ Resource                   │
└─────────────────────────────────────────────────────────────┘
```

---

### 🟣 8. Microsoft Entra Default

```
┌─────────────────────────────────────────────────────────────┐
│  Microsoft Entra Default                                    │
│                                                             │
│  بيجرب طرق كتير بالترتيب ده:                                 │
│  1. Environment Variables                                    │
│  2. Workload Identity                                       │
│  3. Managed Identity                                        │
│  4. Visual Studio                                           │
│  5. Azure CLI                                               │
│  6. Azure PowerShell                                        │
│  7. Interactive Browser                                     │
│                                                             │
│  مناسب للـ Development لأنه بيجرب كل الطرق                  │
└─────────────────────────────────────────────────────────────┘
```

---

### جدول مقارنة كل الطرق

| الطريقة | بيئة الاستخدام | محتاج Password؟ | الأمان |
|---------|--------------|----------------|--------|
| Windows Auth | On-Premise / Corporate | ❌ | ⭐⭐⭐⭐ |
| SQL Server Auth | Development / Cross-Platform | ✅ | ⭐⭐ |
| Entra Password | Azure SQL | ✅ | ⭐⭐⭐ |
| Entra Integrated | Azure + Domain-joined | ❌ | ⭐⭐⭐⭐ |
| Entra MFA | Azure (حساسة) | ✅ + Code | ⭐⭐⭐⭐⭐ |
| Service Principal | CI/CD / Automation | Client Secret | ⭐⭐⭐⭐ |
| Managed Identity | Azure Resources | ❌ | ⭐⭐⭐⭐⭐ |
| Default | Development | حسب البيئة | ⭐⭐⭐ |

---

## 04 — Advanced Connection Settings

لما بتضغط على **Advanced** في شاشة الاتصال بتلاقي إعدادات كتير:

### Security Settings

| الإعداد | النوع | الشرح |
|---------|-------|-------|
| `Encrypt` | Boolean | هل البيانات بين التطبيق والـ DB مشفرة؟ (افتراضي: True في الإصدارات الحديثة) |
| `Trust Server Certificate` | Boolean | هل تثق في شهادة الـ SSL بتاعة السيرفر حتى لو مش معتمدة؟ (في الـ Dev: True، في الـ Production: False) |
| `Host Name In Certificate` | String | اسم الـ Host المتوقع في شهادة السيرفر لو مختلف عن اسم السيرفر |
| `Integrated Security` | Boolean | Windows Authentication (True = استخدمها) |
| `Persist Security Info` | Boolean | هل بيحتفظ بالـ Password في الـ Connection String بعد الاتصال؟ (خطير — خليه False) |
| `Server Certificate` | Boolean | متعلق بالـ SSL Certificate |

### Network Settings

| الإعداد | النوع | الشرح |
|---------|-------|-------|
| `IP Address Preference` | Enum | تفضيل IPv4 أو IPv6 |
| `Multi Subnet Failover` | Boolean | للـ High Availability (Always On Groups) |
| `Transparent Network IP Resolution` | Boolean | بيحاول IP Addresses كتير لو الاتصال فشل |

### Source Settings (المصدر)

| الإعداد | الشرح |
|---------|-------|
| `DataSource` | اسم السيرفر (زي `.` أو `DESKTOP\SQLEXPRESS`) |
| `Initial Catalog` | اسم الـ Database (زي `Northwind`) |
| `AttachDbFilename` | لو عايز تعمل Attach لـ .mdf File مباشرة |
| `User Instance` | للـ SQL Server Express بس (قديم) |
| `Failover Partner` | سيرفر احتياطي لو الأول وقع |

### Connection String مثال كامل

```
data source=.;
initial catalog=Northwind;
integrated security=True;
encrypt=True;
trust server certificate=True;
```

> [!TIP]
> في الـ Development: `Trust Server Certificate=True` وعادي.
> في الـ Production: لازم تكون عندك شهادة SSL حقيقية وتحط `Trust Server Certificate=False`.

---

## 05 — Code Generation Templates

لما بتولد الكود، بتقدر تختار Template مختلفة:

### 🔵 C# - T4

```
T4 = Text Template Transformation Toolkit
```

- **الشرح:** T4 هو نظام Templates قديم من Microsoft. بيستخدم ملفات `.tt` عشان يولد كود.
- **المميزات:** Standard، مدعوم من Visual Studio مباشرة
- **العيوب:** التعديل عليه صعب شوية

```
مثال على T4 Template:
<#@ template debug="false" hostspecific="false" language="C#" #>
<#@ output extension=".cs" #>
// Generated Code
public class <#= ClassName #> { ... }
```

---

### 🔵 C# - T4 (Split DbContext)

- **زي T4 بالظبط** بس بيفصل الـ DbContext على ملفات منفصلة
- **الفايدة:** لو الـ DbContext كبير جداً، بيخليه أسهل في الـ Management

---

### 🟢 C# - Handlebars

- **الشرح:** بيستخدم Handlebars.js-style templates
- **المميزات:** أسهل في التعديل من T4، مرن أكتر
- **امتى تستخدمه؟** لو عايز تعدل في شكل الكود الناتج كتير

```
مثال Handlebars Template:
public partial class {{class}} {
  {{#each properties}}
  public {{type}} {{name}} { get; set; }
  {{/each}}
}
```

---

### 🟢 TypeScript - Handlebars

- بيولد TypeScript Classes بدل C#
- **امتى تستخدمه؟** لو عندك Frontend بـ TypeScript وعايز نفس الـ Models

---

### 🟡 C# - T4 (POCO)

```
POCO = Plain Old CLR Object
```

- بيولد Classes بسيطة جداً بدون أي Attributes أو Annotations
- مناسب لو بتستخدم Fluent API كلياً بدل الـ Attributes

---

### جدول المقارنة

| Template | سهولة التعديل | المخرج | امتى تستخدمه؟ |
|---------|--------------|--------|--------------|
| C# T4 | متوسط | C# مع Attributes | الاستخدام العادي |
| C# T4 Split | متوسط | C# مفصول | DbContext كبير |
| C# Handlebars | سهل | C# مخصص | لو عايز تعدل الشكل |
| TypeScript Handlebars | سهل | TypeScript | Frontend TS |
| C# T4 POCO | متوسط | C# نظيف | مع Fluent API |

---

### Data Annotations vs Fluent API

```csharp
// ───── Data Annotations ─────
// بيكتبها EF Power Tools فوق الـ Properties
[Required]
[MaxLength(100)]
[Column("product_name")]
public string ProductName { get; set; }

// ───── Fluent API ─────
// بتكتبها في الـ DbContext داخل OnModelCreating
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>()
        .Property(p => p.ProductName)
        .IsRequired()
        .HasMaxLength(100)
        .HasColumnName("product_name");
}
```

| المقارنة | Data Annotations | Fluent API |
|---------|-----------------|------------|
| المكان | فوق الـ Properties | في DbContext |
| الوضوح | كل حاجة في مكانها | الـ Config منفصل |
| القوة | محدود | أقوى وأشمل |
| Database First | بيولدها EF تلقائياً | لازم تكتبها |

---

## 06 — Stored Procedures

### الملفات المولّدة

```
📁 Context
├── INorthwindContextProcedures.cs   ← Interface
└── NorthwindContextProcedures.cs   ← Implementation

📁 Models
├── SalesByCategoryResult.cs         ← نتيجة الـ SP
├── CustOrdersDetailsResult.cs
└── ...
```

### الـ Interface

```csharp
// INorthwindContextProcedures.cs
public partial interface INorthwindContextProcedures
{
    // لكل Stored Procedure Method بنفس الاسم + Async
    Task<List<SalesByCategoryResult>> SalesByCategoryAsync(
        string? CategoryName,
        string? OrdYear,
        OutputParameter<int> returnValue = null,
        CancellationToken cancellationToken = default
    );
}
```

### الـ Implementation

```csharp
// NorthwindContextProcedures.cs
public partial class NorthwindContextProcedures : INorthwindContextProcedures
{
    private readonly NorthwindContext _context;

    public NorthwindContextProcedures(NorthwindContext context)
    {
        _context = context;
    }

    public async Task<List<SalesByCategoryResult>> SalesByCategoryAsync(
        string? CategoryName, string? OrdYear, ...)
    {
        // بيستخدم SqlParameter عشان يبعت الـ Parameters بأمان
        var categoryNameParam = new SqlParameter("@CategoryName", CategoryName ?? (object)DBNull.Value);
        var ordYearParam      = new SqlParameter("@OrdYear", OrdYear ?? (object)DBNull.Value);

        // بيستخدم FromSqlRaw عشان ينفذ الـ Stored Procedure
        var result = await _context.Set<SalesByCategoryResult>()
            .FromSqlRaw("EXEC [dbo].[SalesByCategory] @CategoryName, @OrdYear",
                categoryNameParam, ordYearParam)
            .ToListAsync();

        return result;
    }
}
```

---

### ازاي تستخدم الـ Stored Procedure

```csharp
using (NorthwindContext _northwindContext = new NorthwindContext())
{
    // Step 1: عمل Instance من الـ Procedures Class
    var procedures = new NorthwindContextProcedures(_northwindContext);

    // Step 2: استدعاء الـ Stored Procedure
    // ❌ ما تستخدمش .Result في Production
    var results = procedures.SalesByCategoryAsync("Beverages", "1998").Result;

    // ✅ الأحسن: GetAwaiter().GetResult() (في Console App مش فيه async main)
    var results = procedures.SalesByCategoryAsync("Beverages", "1998")
                            .GetAwaiter()
                            .GetResult();

    // Step 3: استخدام النتيجة
    foreach (var item in results)
    {
        Console.WriteLine($"Product: {item.ProductName} - Sales: {item.TotalPurchase}");
    }
}
```

---

## 07 — Threading و Async/Await

### ما هو الـ Thread؟

```
تخيل الـ CPU زي مطعم والـ Threads زي الـ Chefs:
───────────────────────────────────────────────
Thread واحد (Synchronous):
  Chef 1: يطبخ الطبق → ينتهي → يطبخ التاني → ينتهي...
  المشكلة: الزبون الثاني لازم يستنى الأول ينتهي!

Multiple Threads (Asynchronous):
  Chef 1: يبدأ طبق 1 → يحطه على النار (ينتظر) → يبدأ طبق 2
  Chef 2: يشتغل على طبق 3 في نفس الوقت
  النتيجة: أسرع بكتير! ✅
───────────────────────────────────────────────
```

### الفرق بين Sync و Async

```csharp
// ─── Synchronous (المزعج) ───
public IActionResult GetProducts()
{
    // هنا الـ Thread بيستنى لحد ما الـ DB ترجع البيانات
    // في الوقت ده مش بيعمل أي حاجة! (Blocking)
    var products = dbContext.Products.ToList();
    return Ok(products);
}

// ─── Asynchronous (الصح) ───
public async Task<IActionResult> GetProducts()
{
    // هنا الـ Thread بيروح يخدم Requests تانية
    // لحد ما الـ DB ترجع، وبعدين بيكمل هنا
    var products = await dbContext.Products.ToListAsync();
    return Ok(products);
}
```

---

### الـ 30 Second Timeout

```
الموضوع ببساطة:
─────────────────────────────────────────────────────
HTTP Request جه → السيرفر اشتغل → لو اشتغل أكتر من 30 ثانية
                                         ↓
                              المتصفح بيقول: "Timeout!"
                              والـ Exception بتحصل

الحل لو العملية طويلة جداً: استخدم JOBS
─────────────────────────────────────────────────────
```

### JOBS للعمليات الطويلة

```csharp
// بدل ما تستنى العملية الطويلة
// بتعمل Job في الـ Background

// مثال باستخدام Hangfire
[HttpPost("generate-report")]
public IActionResult GenerateReport()
{
    // بتبدأ الـ Job وبترجع فوراً
    BackgroundJob.Enqueue(() => ReportService.Generate());
    return Ok("الريبورت بيتولد وهنبعتهولك على الإيميل");
    // المستخدم مش محتاج يستنى!
}
```

---

### .Result vs GetAwaiter().GetResult() vs await

```csharp
// ─── 1. .Result ─── (الأسوأ في الغالب)
var result = myTask.Result;
// بيـ Block الـ Thread
// ممكن يعمل DEADLOCK في بعض الحالات!
// الـ Exception بتيجي wrapped في AggregateException

// ─── 2. .GetAwaiter().GetResult() ─── (أحسن من .Result)
var result = myTask.GetAwaiter().GetResult();
// بيـ Block الـ Thread
// بس الـ Exception بتيجي مباشرة (مش wrapped)
// استخدمه في Console Apps أو لما ما تقدرش تعمل async

// ─── 3. await ─── (الأفضل دايماً)
var result = await myTask;
// ما بيـ Block الـ Thread
// الـ Exception بتيجي مباشرة
// استخدمه دايماً في ASP.NET Core
```

```
المقارنة:
────────────────────────────────────────────────
             .Result  │ .GetAwaiter().GetResult() │  await
────────────────────────────────────────────────
يبلوك التريد؟   ✅    │          ✅               │   ❌
Deadlock ممكن؟  ✅    │          ✅               │   ❌
Exception نظيفة؟ ❌   │          ✅               │   ✅
الأفضل في؟   الاضطرار│       Console App         │ ASP.NET Core
────────────────────────────────────────────────
```

---

## 08 — FromSqlRaw vs FromSqlInterpolated

### FromSqlRaw — للـ Queries العادية

```csharp
// ─── SELECT كل الـ Categories ───
var result = _northwindContext.Categories
    .FromSqlRaw("SELECT * FROM Categories")
    .ToList();

// ─── لو عايز تمرر Parameter ───
// ❌ ده SQL Injection!
var name = "Beverages";
var result = _northwindContext.Categories
    .FromSqlRaw($"SELECT * FROM Categories WHERE CategoryName = '{name}'")
    .ToList();

// ✅ الصح: تمرير Parameters بشكل آمن
var name = "Beverages";
var result = _northwindContext.Categories
    .FromSqlRaw("SELECT * FROM Categories WHERE CategoryName = {0}", name)
    .ToList();
```

---

### FromSqlInterpolated — للـ Queries مع Parameters

```csharp
// ✅ أحسن وأأمن طريقة لتمرير Parameters
var categoryName = "Beverages";

var result = _northwindContext.Categories
    .FromSqlInterpolated($"SELECT * FROM Categories WHERE CategoryName = {categoryName}")
    .FirstOrDefault();

Console.WriteLine($"{result.CategoryName} :: {result.Description}");
```

> **ليه FromSqlInterpolated أأمن؟** لأنه بيتعامل مع الـ Parameters كـ SqlParameter مش كـ String عادي — يعني بيحميك من SQL Injection تلقائياً.

---

### ExecuteSqlRaw و ExecuteSqlInterpolated

```csharp
// ─── ExecuteSqlInterpolated ─── (للـ DML مع Parameters)
int productId = 1;
string newName = "BURGER";

_northwindContext.Database
    .ExecuteSqlInterpolated($"UPDATE Products SET ProductName = {newName} WHERE ProductId = {productId}");

// ─── ExecuteSqlRaw ─── (للـ DML العادي)
_northwindContext.Database
    .ExecuteSqlRaw($"UPDATE Products SET ProductName = 'Chai' WHERE ProductId = {1}");
// ⚠️ بيقبل $ بس يعني String Interpolation عادية — مش آمنة من SQL Injection
```

---

### الفرق الرئيسي

```
FromSqlRaw / ExecuteSqlRaw:
  - Parameters بـ {0}, {1} أو SqlParameter Objects
  - بيقبل $ (String Interpolation) بس ده UNSAFE!
  - لو حطيت المتغير جوه الـ String مباشرة = SQL Injection Risk

FromSqlInterpolated / ExecuteSqlInterpolated:
  - بيستخدم C# String Interpolation ($"...")
  - EF بيحول المتغيرات تلقائياً لـ SqlParameters
  - آمن 100% من SQL Injection ✅
```

---

### ⚠️ الخطأ الشائع الخطير

```csharp
// ❌❌❌ SQL INJECTION! لا تعمل ده أبداً ❌❌❌
var id = Request.Query["id"]; // ممكن يكون "1; DROP TABLE Products--"
_northwindContext.Database.ExecuteSqlRaw(
    $"DELETE FROM Products WHERE ProductId = {id}"
    // هنا الـ $ بيعمل String Interpolation عادية!
    // ممكن حد يحذف الجدول كله!
);

// ✅ الصح
_northwindContext.Database.ExecuteSqlInterpolated(
    $"DELETE FROM Products WHERE ProductId = {id}"
    // هنا الـ $ + ExecuteSqlInterpolated = آمن ✅
);
```

> [!WARNING]
> **الـ SQLEXCEPTION الغريبة:** لو لقيت `Incorrect syntax near '='` أو أي SQL Error غريبة، اتحقق من:
> 1. اسم الـ Column والـ Table صح؟
> 2. الـ Parameter بيتمرر صح؟
> 3. مش فيه Space أو Quote زيادة في الـ Query؟
> الـ Error مش بيقولك السطر بالظبط — لازم تفحص الـ SQL اللي بيتولد.

---

### جدول المقارنة

| الأمر | الاستخدام | آمن من SQL Injection؟ |
|-------|----------|----------------------|
| `FromSqlRaw("...", params)` | SELECT + Parameters بـ {0} | ✅ |
| `FromSqlRaw($"...")` | ⚠️ بدون params | ❌ |
| `FromSqlInterpolated($"...")` | SELECT + String Interpolation | ✅ |
| `ExecuteSqlRaw("...", params)` | DML + Parameters | ✅ |
| `ExecuteSqlRaw($"...")` | ⚠️ بدون params | ❌ |
| `ExecuteSqlInterpolated($"...")` | DML + String Interpolation | ✅ |

---

## 09 — IQueryable vs IEnumerable

ده من أهم الفروق في EF Core!

### IEnumerable

```csharp
// ─── IEnumerable ───
// بيجيب كل البيانات من الـ DB في الـ Memory الأول
// وبعدين بيفلتر على جهازك

IEnumerable<Product> products = _northwindContext.Products;
// ⚠️ هنا: بعت SQL: SELECT * FROM Products  (كل الصفوف!)

var filtered = products.Where(p => p.Price > 100);
// الـ Filtering هنا بيحصل في الـ Memory (C#)

// يعني:
// 1. جاب 1000 منتج من الـ DB
// 2. فلتر منهم 50 في الـ Memory
// بضيع وقت وذاكرة! 😱
```

---

### IQueryable

```csharp
// ─── IQueryable ───
// بيبني الـ SQL Query في الـ Memory
// وبعدين بيبعتها للـ DB مرة واحدة

IQueryable<Product> products = _northwindContext.Products;
// هنا: لسه ما اتبعت حاجة للـ DB!

var filtered = products.Where(p => p.Price > 100);
// هنا: لسه ما اتبعت حاجة! بس أضاف الـ WHERE للـ Query

var result = filtered.ToList();
// هنا بس بعت: SELECT * FROM Products WHERE Price > 100
// يعني جاب 50 منتج بس! ✅
```

---

### الفرق البصري

```
IEnumerable:
───────────────────────────────────────────────
Database → [1000 Record] → Memory → Filter → [50 Record]
               ↑ كل حاجة اتنزلت!

IQueryable:
───────────────────────────────────────────────
Database → (Query Building) → SQL With WHERE → [50 Record]
                                    ↑ بس اللي محتاجه اتنزل!
```

---

### متى تستخدم كل واحد؟

```csharp
// ✅ استخدم IQueryable مع EF Core دايماً
// لأن الـ LINQ operations بتتحول لـ SQL تلقائياً

var expensiveProducts = _northwindContext.Products
    .Where(p => p.UnitPrice > 100)    // WHERE UnitPrice > 100
    .OrderBy(p => p.ProductName)      // ORDER BY ProductName
    .Take(10)                         // TOP 10
    .ToList();
// SQL الناتج: SELECT TOP 10 * FROM Products WHERE UnitPrice > 100 ORDER BY ProductName

// ✅ استخدم IEnumerable لما البيانات بعيدة عن الـ DB
// زي لو عندك List<> في الـ Memory
List<Product> localProducts = GetFromCache();
var filtered = localProducts.Where(p => p.Price > 100); // IEnumerable - OK
```

---

### جدول المقارنة

| المقارنة | IQueryable | IEnumerable |
|---------|-----------|-------------|
| التنفيذ | على الـ DB (SQL) | في الـ Memory (C#) |
| الأداء | أسرع بكثير | أبطأ للبيانات الكبيرة |
| الاستخدام مع EF | ✅ مثالي | ⚠️ تجنبه مع EF |
| يدعم LINQ→SQL؟ | ✅ | ❌ |
| Namespace | `System.Linq` | `System.Collections.Generic` |

---

## 10 — Partial Classes

### ليه محتاجين Partial Classes مع Database First؟

```
المشكلة:
──────────────────────────────────────────────
EF Power Tools ولّد Category.cs تلقائياً
أنت عدّلت فيه (أضفت ToString مثلاً)
حدث تغيير في الـ DB وعملت Update Model
                    ↓
EF أعاد توليد Category.cs وكل تعديلاتك اتمسحت! 😱
──────────────────────────────────────────────

الحل: Partial Class
```

### ازاي تعمل Partial Class؟

```csharp
// ─── الملف الأصلي (EF ولّده - ما تلمسوش) ───
// Models/Category.cs
public partial class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Product> Products { get; set; }
}

// ─── ملفك الإضافي (انت اللي بتعمله) ───
// Models/Category.Partial.cs
public partial class Category
{
    // Override ToString
    public override string ToString()
    {
        return $"[{CategoryId}] {CategoryName} - {Description}";
    }

    // ممكن تضيف Methods زيادة
    public string GetDisplayName() => $"Category: {CategoryName}";

    // ممكن تضيف Validation
    public bool IsValid() => !string.IsNullOrEmpty(CategoryName);
}
```

---

### قواعد الـ Partial Class

```
القواعد اللازم تاخد بالها:
──────────────────────────────────────────────────────────
1. نفس الـ Class Name                 partial class Category
2. نفس الـ Namespace                  namespace EF04.Models
3. نفس الـ Project
4. الكلمة partial لازم تكون موجودة في الاتنين
5. مش بنعمل Inherit — بنعمل partial من نفس الـ Class
──────────────────────────────────────────────────────────
```

### ليه مش بنعمل Inheritance؟

```csharp
// ❌ غلط - Inheritance
public class MyCategoryDerived : Category  // هتبقى Class تانية!
{
    public override string ToString() => CategoryName; // هيشتغل
}
// المشكلة: لما تبعت List<Category> هترجع Category مش MyCategoryDerived

// ✅ صح - Partial
// في ملف 1:
public partial class Category { public int CategoryId { get; set; } }
// في ملف 2:
public partial class Category { public override string ToString() => CategoryName; }
// وقت الـ Compilation: بيتدمجوا في Class واحدة بكل محتوياتهم
```

---

## 11 — Lazy vs Eager vs Explicit Loading

### المشكلة الأساسية

```
عندنا:
Product → Category (Many-to-One)
Product → Supplier (Many-to-One)
Category → Products (One-to-Many)

السؤال: لما أجيب Product، يجيب معاه الـ Category والـ Supplier تلقائياً؟
الجواب: يعتمد على نوع الـ Loading اللي بتستخدمه!
```

---

### 1. Eager Loading (التحميل المتحمس 😄)

```csharp
// بيجيب البيانات المرتبطة في نفس الـ Query (JOIN)
var product = _northwindContext.Products
    .Include(p => p.Category)           // JOIN Categories
    .Include(p => p.Supplier)           // JOIN Suppliers
    .FirstOrDefault();

Console.WriteLine($"{product.ProductName} - {product.Category.CategoryName}");
// يشتغل ✅ لأن Category اتجابت مع Product في نفس الـ Query
```

**الـ SQL اللي بيتولد:**

```sql
SELECT p.*, c.*, s.*
FROM Products p
LEFT JOIN Categories c ON p.CategoryId = c.CategoryId
LEFT JOIN Suppliers s ON p.SupplierId = s.SupplierId
```

---

### ThenInclude — للـ Nested Relations

```csharp
// لو عايز تجيب: Products → Category → Products of that Category
var product = _northwindContext.Products
    .Include(p => p.Category)
        .ThenInclude(c => c.Products)  // products inside the category!
    .FirstOrDefault();

// ⚠️ تحذير: ده ممكن يعمل Infinite Loop في الـ Data!
// Product → Category → Products → Category → Products → ...
```

---

### 2. Explicit Loading (التحميل الصريح)

```csharp
// تجيب الـ Product الأول بدون أي Relations
var product = _northwindContext.Products.FirstOrDefault();
// product.Category == null هنا!

// بعدين بتقول بصريح العبارة: "حمّل الـ Category دلوقتي"
_northwindContext.Entry(product).Reference(p => p.Category).Load();
// دلوقتي product.Category != null ✅

_northwindContext.Entry(product).Reference(p => p.Supplier).Load();
// دلوقتي product.Supplier != null ✅

Console.WriteLine($"{product.ProductName} :: {product.Category.CategoryName}");
```

---

### Reference vs Collection في Explicit Loading

```csharp
// ─── Reference ─── (للـ Single Object — Many-to-One)
// Product → Category (كل Product عنده Category واحدة)
_northwindContext.Entry(product).Reference(p => p.Category).Load();
//                                ↑ Reference لأن Category واحدة

// ─── Collection ─── (للـ List — One-to-Many)
// Category → Products (كل Category عندها Products كتير)
var category = _northwindContext.Categories.FirstOrDefault();
_northwindContext.Entry(category).Collection(c => c.Products).Load();
//                                ↑ Collection لأن Products كتير

// ❌ لو استخدمت Reference مع Collection هيجي Error:
// System.InvalidOperationException: Category.Products is a collection navigation!
_northwindContext.Entry(category).Reference(c => c.Products).Load(); // ❌ ERROR
```

---

### 3. Lazy Loading (التحميل الكسول 😴)

```csharp
// Step 1: ثبّت الـ Package
// Tools → NuGet → Microsoft.EntityFrameworkCore.Proxies

// Step 2: فعّله في الـ DbContext
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseLazyLoadingProxies()    // ← هنا
        .UseSqlServer(connectionString);
}

// Step 3: خلي الـ Properties virtual
public partial class Product
{
    public virtual Category Category { get; set; }  // ← virtual مهمة!
    public virtual Supplier Supplier { get; set; }  // ← virtual مهمة!
}

// Step 4: استخدمها بدون Include
var product = _northwindContext.Products.FirstOrDefault();
// product.Category == null

Console.WriteLine(product.Category.CategoryName);
// هنا بس بيعمل Query للـ DB ويجيب الـ Category (Lazy = كسول = بيجيب لما يحتاج)
```

**ازاي Lazy Loading بيشتغل؟**

```
EF بيعمل Proxy Class بدل الـ Class الأصلية
الـ Proxy بيـ Override كل الـ virtual Properties
لما تاخد Product، في الحقيقة بتاخد ProductProxy
لما بتوصل لـ product.Category، الـ Proxy بيعمل:
  - SQL Query للـ Categories
  - ويرجعلك الـ Category
```

---

### مقارنة الثلاث طرق

```
┌─────────────────┬──────────────┬──────────────┬──────────────┐
│                 │ Eager        │ Explicit     │ Lazy         │
├─────────────────┼──────────────┼──────────────┼──────────────┤
│ عدد الـ Queries │ 1 (JOIN)     │ 1 + N        │ 1 + N        │
│ الكود           │ بسيط         │ طويل شوية    │ أبسط         │
│ Package زيادة؟ │ ❌           │ ❌           │ ✅ Proxies   │
│ virtual لازمة؟ │ ❌           │ ❌           │ ✅            │
│ تحكم           │ واضح         │ واضح         │ مخفي         │
│ N+1 Problem     │ ❌           │ ممكن         │ ⚠️ خطر كبير  │
├─────────────────┼──────────────┼──────────────┼──────────────┤
│ أمتى تستخدم؟   │ لما عارف     │ لما محتاجها  │ Prototyping  │
│                 │ محتاج إيه    │ أحياناً بس   │ فقط!         │
└─────────────────┴──────────────┴──────────────┴──────────────┘
```

> [!TIP]
> في Production: **Eager Loading** أو **Explicit Loading** هو الاختيار الأفضل. Lazy Loading ممكن يعمل مشاكل كبيرة في الأداء.

---

## 12 — N+1 Query Problem

ده من أشهر مشاكل الأداء في الـ ORMs!

### المشكلة

```csharp
// ❌ كود بيعمل N+1 Problem
var categories = _northwindContext.Categories.ToList();
// Query 1: SELECT * FROM Categories → رجع 8 Categories

foreach (var category in categories)
{
    // Query 2: SELECT * FROM Products WHERE CategoryId = 1
    // Query 3: SELECT * FROM Products WHERE CategoryId = 2
    // Query 4: SELECT * FROM Products WHERE CategoryId = 3
    // ... وهكذا لكل Category (8 Categories = 8 Queries زيادة!)
    Console.WriteLine($"{category.CategoryName}: {category.Products.Count} products");
}

// المجموع: 1 + 8 = 9 Queries! 😱
// لو عندك 1000 Category: 1001 Queries!
```

---

### البيان البصري

```
N+1 Problem:

Query 1:   SELECT * FROM Categories
             ↓ يرجع: [Bev, Cond, Conf, Dairy, ...]

Query 2:   SELECT * FROM Products WHERE CategoryId = 1  (Beverages)
Query 3:   SELECT * FROM Products WHERE CategoryId = 2  (Condiments)
Query 4:   SELECT * FROM Products WHERE CategoryId = 3  (Confections)
Query 5:   SELECT * FROM Products WHERE CategoryId = 4  (Dairy)
Query 6:   SELECT * FROM Products WHERE CategoryId = 5  (Grains)
Query 7:   SELECT * FROM Products WHERE CategoryId = 6  (Meat)
Query 8:   SELECT * FROM Products WHERE CategoryId = 7  (Produce)
Query 9:   SELECT * FROM Products WHERE CategoryId = 8  (Seafood)

Total: 9 Queries للـ Database! (N=8 categories, +1 الأساسية)
```

---

### الحل: Eager Loading

```csharp
// ✅ Include بيعمل JOIN ويجيب كل حاجة في Query واحدة
var categories = _northwindContext.Categories
    .Include(c => c.Products)
    .ToList();
// SQL: SELECT c.*, p.* FROM Categories c
//      LEFT JOIN Products p ON c.CategoryId = p.CategoryId
// Query واحدة بس! ✅

foreach (var category in categories)
{
    // البيانات موجودة في الـ Memory خلاص — مفيش Query جديدة
    Console.WriteLine($"{category.CategoryName}: {category.Products.Count} products");
}
```

---

### لو شيلنا ThenInclude الخطير؟

```
الـ Infinite Loop Data Problem:
──────────────────────────────────────────────
Product ─── Include(Category) ──→ Category
                                      │
                                 ThenInclude(Products)
                                      │
                                      ↓
                                  [Product1, Product2, ...]
                                  كل Product ده فيه Category
                                  وكل Category فيها Products
                                  وهكذا للأبد!!

EF بيكتشف ده ويوقف عند نقطة معينة
بس الـ Data بتبقى ضخمة جداً في الـ Memory!
──────────────────────────────────────────────
```

---

## 13 — Reference vs Collection

الموضوع ده اتشرح فوق بس خلينا نوضحه أكتر:

```
في الـ Database Relations:
──────────────────────────────────────────────
Many-to-One:    Product → Category
                كل Product عندها Category واحدة
                ده Reference (Single Object)

One-to-Many:    Category → Products
                كل Category عندها Products كتير
                ده Collection (ICollection<T>)
──────────────────────────────────────────────

في الكود:
──────────────────────────────────────────────
// في Product Class:
public virtual Category Category { get; set; }    ← Reference
public virtual Supplier Supplier { get; set; }    ← Reference

// في Category Class:
public virtual ICollection<Product> Products { get; set; }  ← Collection
──────────────────────────────────────────────
```

```csharp
// Explicit Loading:

// Reference → للـ Single Navigation Property
_northwindContext.Entry(product).Reference(p => p.Category).Load();
_northwindContext.Entry(product).Reference(p => p.Supplier).Load();

// Collection → للـ Collection Navigation Property
_northwindContext.Entry(category).Collection(c => c.Products).Load();

// لو استخدمت الغلط هيجي:
// InvalidOperationException: 'Products' is defined as a collection navigation.
// Use Collection() instead of Reference().
```

---

## 14 — Remote vs Local

### Remote (من الـ Database)

```csharp
// كل ده Remote — بيبعت Request للـ SQL Server
var product = _northwindContext.Products.FirstOrDefault();
var count = _northwindContext.Products.Count();
var expensive = _northwindContext.Products.Where(p => p.UnitPrice > 100).ToList();
```

### Local (من الـ Memory)

```csharp
// Step 1: حمّل كل البيانات في الـ Memory
_northwindContext.Products.Load();
// SQL: SELECT * FROM Products (مرة واحدة)

// Step 2: الآن اشتغل Local بدون أي Database Queries
if (_northwindContext.Products.Local.Any(p => p.UnitsInStock == 0))
{
    Console.WriteLine("في منتجات نفدت من المخزن (من الـ Memory)");

    var outOfStock = _northwindContext.Products.Local
                     .Where(p => p.UnitsInStock == 0)
                     .ToList();
    // ده Filtering في الـ Memory — مفيش SQL!
}
```

---

### متى تستخدم Local؟

```
Remote (Default):
  ✅ عندما تريد أحدث بيانات
  ✅ عندما البيانات كثيرة
  ✅ للعمليات العادية

Local:
  ✅ عندما تحتاج نفس البيانات مرات كثيرة (Caching بسيط)
  ✅ عندما تريد تجنب Queries متكررة
  ✅ للـ Batch Operations (تغيير كثير في الذاكرة ثم Save مرة واحدة)
  ❌ لا تستخدمه للبيانات الكبيرة جداً
  ❌ البيانات ممكن تكون Stale (قديمة)
```

---

### Caching بشكل عام

```
Caching = تخزين البيانات مؤقتاً عشان ما ترجعش للـ DB كل مرة

أنواع الـ Caching في ASP.NET Core:
─────────────────────────────────────────────
1. In-Memory Cache      ← في الـ RAM (على نفس السيرفر)
2. Distributed Cache    ← Redis أو SQL Server (بين سيرفرات)
3. Response Cache       ← Cache الـ HTTP Response كاملة
4. EF Local Cache       ← اللي شرحناه (DbSet.Local)
─────────────────────────────────────────────
```

---

## 15 — JOIN — Fluent vs Query Syntax

### Fluent Syntax

```csharp
var result = _northwindContext.Categories   // Outer Sequence
    .Join(
        _northwindContext.Products,          // Inner Sequence
        category => category.CategoryId,    // Outer Key (PK)
        product  => product.CategoryId,     // Inner Key (FK)
        (category, product) => new           // Result Selector
        {
            ProductName  = product.ProductName,
            CategoryName = category.CategoryName
        }
    );

foreach (var item in result)
    Console.WriteLine($"Product: {item.ProductName} | Category: {item.CategoryName}");
```

---

### Query Syntax (أوضح للقراءة)

```csharp
var result = from category in _northwindContext.Categories
             join product in _northwindContext.Products
             on category.CategoryId equals product.CategoryId
             select new
             {
                 ProductName  = product.ProductName,
                 CategoryName = category.CategoryName
             };

foreach (var item in result)
    Console.WriteLine($"Product: {item.ProductName} | Category: {item.CategoryName}");
```

---

### الـ SQL اللي بيتولد

```sql
SELECT p.ProductName, c.CategoryName
FROM Categories c
INNER JOIN Products p ON c.CategoryId = p.CategoryId
```

---

### مقارنة الاتنين

```csharp
// ─── Fluent Syntax ───
// زي LINQ Methods — chain of method calls
var result = source.Join(other, outerKey, innerKey, (a, b) => new { ... });

// ─── Query Syntax ───
// زي SQL بالظبط — أوضح للـ SQL developers
var result = from a in source
             join b in other on a.Key equals b.Key
             select new { ... };
```

| المقارنة | Fluent Syntax | Query Syntax |
|---------|--------------|--------------|
| الوضوح | أقل لـ JOINs | أوضح لـ JOINs المعقدة |
| Grouping | أصعب | أسهل |
| Subqueries | أسهل | أصعب |
| المفضل عند .NET Devs | ✅ | عند SQL Devs |

---

## 16 — Tracking vs No Tracking

### الـ Tracking الافتراضي

```csharp
// EF بيـ Track كل Object بتجيبه من الـ DB
var product = _northwindContext.Products
              .Where(p => p.ProductId == 1)
              .FirstOrDefault();

// EF دلوقتي بيتابع (Track) هذا الـ Object
// لو غيرت فيه، EF هيعرف!
product.UnitPrice = 50;

_northwindContext.SaveChanges();
// SQL: UPDATE Products SET UnitPrice = 50 WHERE ProductId = 1
// اشتغل لأن EF لاحظ التغيير! ✅
```

---

### AsNoTracking

```csharp
// EF مش هيتابع الـ Object ده
var product = _northwindContext.Products
              .Where(p => p.ProductId == 1)
              .AsNoTracking()       // ← مفيش Tracking
              .FirstOrDefault();

product.UnitPrice = 18;

_northwindContext.SaveChanges();
// ⚠️ مفيش SQL UPDATE! EF ما يعرفش في التغيير
// لأن AsNoTracking
```

---

### AsNoTrackingWithIdentityResolution

```csharp
// ─── AsNoTracking مشكلة ─── (Object تكرار)
var products = _northwindContext.Products
    .Include(p => p.Category)
    .AsNoTracking()
    .ToList();

// هنا ممكن تلاقي:
// products[0].Category هو Category مختلف في الـ Memory
// عن products[1].Category (حتى لو نفس الـ CategoryId!)
// لأن AsNoTracking مش بيتذكر الـ Objects

// ─── AsNoTrackingWithIdentityResolution ─── (الحل)
var products = _northwindContext.Products
    .Include(p => p.Category)
    .AsNoTrackingWithIdentityResolution()  // ← الحل
    .ToList();

// دلوقتي:
// products[0].Category و products[1].Category
// لو نفس الـ CategoryId → نفس الـ Object في الـ Memory ✅
```

---

### مقارنة الثلاث طرق

| الطريقة | بيتتبع التغييرات؟ | الأداء | Identity Resolution؟ |
|---------|-----------------|--------|----------------------|
| Default (Tracking) | ✅ | أبطأ | ✅ |
| AsNoTracking | ❌ | أسرع | ❌ |
| AsNoTrackingWithIdentityResolution | ❌ | وسط | ✅ |

```
متى تستخدم كل واحد؟
───────────────────────────────────────────────────────────
Default Tracking:
  ✅ لما هتعدل أو تحذف البيانات
  ✅ لما محتاج SaveChanges يشتغل

AsNoTracking:
  ✅ Read-Only Operations (عرض بيانات بس)
  ✅ Reports & Dashboards
  ✅ تحسين الأداء (بياخد ذاكرة أقل)
  ❌ لو محتاج تعدل البيانات

AsNoTrackingWithIdentityResolution:
  ✅ Read-Only مع Include (Relations)
  ✅ لما مهم الـ Objects تكون نفسها في الـ Memory
  ✅ أحسن من AsNoTracking مع Include
───────────────────────────────────────────────────────────
```

---

### إلغاء الـ Tracking للكل

```csharp
// طريقة 1: على مستوى Query واحدة
_northwindContext.Products.AsNoTracking().ToList();

// طريقة 2: على مستوى الـ Context كله (لجلسة واحدة)
_northwindContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

// طريقة 3: في الـ OnConfiguring (لكل الـ Instances)
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(connectionString)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // ← هنا
}
```

---

## 17 — ChangeTracker بالتفصيل

### ما هو الـ ChangeTracker؟

```
الـ ChangeTracker هو "دفتر الملاحظات" بتاع EF
بيراقب كل Object بتجيبه من الـ DB
ويعرف لو اتغير أو لا

States للـ Entity:
─────────────────────────────────────────────
Detached    ← مش معروف لـ EF (Object جديد)
Added       ← هيتضاف (INSERT)
Unchanged   ← ما اتغيرش
Modified    ← اتغير (UPDATE)
Deleted     ← هيتحذف (DELETE)
─────────────────────────────────────────────
```

---

### ازاي تستخدم الـ ChangeTracker؟

```csharp
using (var context = new NorthwindContext())
{
    var product = context.Products.FirstOrDefault(p => p.ProductId == 1);
    // State: Unchanged

    product.ProductName = "New Name";
    // State: Modified (EF لاحظ التغيير!)

    // شوف الـ State
    var entry = context.Entry(product);
    Console.WriteLine(entry.State); // Modified

    // شوف اللي اتغير
    foreach (var prop in entry.Properties)
    {
        if (prop.IsModified)
        {
            Console.WriteLine($"{prop.Metadata.Name}: {prop.OriginalValue} → {prop.CurrentValue}");
        }
    }
    // Output: ProductName: Chai → New Name

    context.SaveChanges();
    // SQL: UPDATE Products SET ProductName = 'New Name' WHERE ProductId = 1
}
```

---

### عمليات الـ ChangeTracker

```csharp
// 1. شوف كل الـ Entities اللي اتغيرت
var changedEntries = context.ChangeTracker.Entries()
    .Where(e => e.State == EntityState.Modified)
    .ToList();

// 2. شوف كل الـ Entities بكل States
foreach (var entry in context.ChangeTracker.Entries())
{
    Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
}

// 3. إلغاء تتبع كل الـ Entities
context.ChangeTracker.Clear();

// 4. تتبع behavior إعداده
context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
// أو
context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll; // الـ Default
```

---

### مثال عملي: Audit Log باستخدام ChangeTracker

```csharp
// تسجيل كل التغييرات قبل SaveChanges
public override int SaveChanges()
{
    var entries = ChangeTracker.Entries()
        .Where(e => e.State == EntityState.Added
                 || e.State == EntityState.Modified
                 || e.State == EntityState.Deleted);

    foreach (var entry in entries)
    {
        Console.WriteLine($"[AUDIT] {entry.Entity.GetType().Name} - {entry.State}");
    }

    return base.SaveChanges();
}
```

---

## 18 — using keyword وإدارة الـ Resources

### ما هو الـ IDisposable؟

```
في C#، في نوعان من الـ Resources:
──────────────────────────────────────────────────────────
Managed Resources:     الـ .NET Runtime بيديرها
                       (زي Objects العادية)

Unmanaged Resources:   لازم تتحرر يدوياً!
                       (زي: Database Connections, File Handles,
                        Network Connections, Memory غير الـ .NET)
──────────────────────────────────────────────────────────
```

### ليه لازم نستخدم using مع DbContext؟

```
لو ما عملتش using:
──────────────────────────────────────────────────────────
NorthwindContext ctx = new NorthwindContext();
// بتعمل Database Connection

ctx.Products.ToList();
// شغلت الـ Query

// لو نسيت ctx.Dispose() !!!
// الـ Database Connection فضلت مفتوحة
// بعد كذا connection مفتوحة: SQL Server بيقول "رفضت!"
// ده Resource Leak
──────────────────────────────────────────────────────────
```

---

### طرق الاستخدام

```csharp
// ─── الطريقة 1: using statement (الكلاسيكية) ───
using (NorthwindContext ctx = new NorthwindContext())
{
    var products = ctx.Products.ToList();
    // في الـ } بتاع using، بيتنادى Dispose() تلقائياً
} // ctx.Dispose() هنا تلقائياً ✅

// ─── الطريقة 2: using declaration (C# 8+، أحدث وأبسط) ───
using NorthwindContext ctx = new NorthwindContext();
var products = ctx.Products.ToList();
// ctx.Dispose() بيتنادى في آخر الـ Scope تلقائياً ✅

// ─── الطريقة 3: في ASP.NET Core (الأفضل) ───
// مش محتاج using! لأن DI بيتحكم في الـ Lifetime
public class ProductsController : Controller
{
    private readonly NorthwindContext _ctx;

    public ProductsController(NorthwindContext ctx) // DI بيحقنه
    {
        _ctx = ctx;
        // بيتعمل Dispose تلقائياً في آخر الـ HTTP Request
    }
}
```

---

### ازاي using بيشتغل من الداخل؟

```csharp
// ده:
using (var ctx = new NorthwindContext())
{
    // كود
}

// بيتحول لـ:
var ctx = new NorthwindContext();
try
{
    // كود
}
finally
{
    // حتى لو حصل Exception، هيتنادى Dispose
    if (ctx != null) ctx.Dispose();
}
```

---

## 🎯 ملخص عام وسريع

```
Database First:   عندك DB خلاص → EF Power Tools → يولد Classes تلقائياً

Authentication:   Windows (Domain) | SQL Server (Username/Password) |
                  Entra (Azure) | Managed Identity (Azure Resources)

Code Templates:   T4 (Standard) | Handlebars (Flexible) | POCO (No Annotations)

Stored Procs:     Interface + Implementation + Result Classes
                  استخدم .GetAwaiter().GetResult() في Console

Async:            await (أحسن) > .GetAwaiter().GetResult() > .Result (الأسوأ)

SQL Methods:      FromSqlInterpolated + ExecuteSqlInterpolated = آمن ✅
                  Raw + $ مباشرة = SQL Injection Risk ❌

IQueryable:       Filter في الـ DB (SQL) → أسرع بكثير ✅
IEnumerable:      Filter في الـ Memory → أبطأ مع بيانات كثيرة ⚠️

Partial Class:    للتعديل على Generated Classes بدون ما تتمسح

Loading:
  Eager:     Include() → JOIN → Query واحدة ✅
  Explicit:  Reference/Collection.Load() → Queries منفصلة
  Lazy:      UseLazyLoadingProxies() → تلقائي عند الوصول (خطر N+1!)

N+1 Problem:      N+1 Queries بدل Query واحدة → الحل: Include()

Tracking:
  Default:            يتابع التغييرات → للـ Update/Delete
  AsNoTracking:       لا يتابع → للـ Read-Only (أسرع)
  + IdentityRes:      لا يتابع + نفس Object للـ ID نفسه → مع Include

ChangeTracker:    بيراقب States: Detached/Added/Unchanged/Modified/Deleted

using:            بيتأكد من Dispose للـ Unmanaged Resources تلقائياً
```

---

*تم الانتهاء من الشرح الكامل 🎉*
