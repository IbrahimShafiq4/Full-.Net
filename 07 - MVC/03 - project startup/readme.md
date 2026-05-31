# 🏗️ شرح مشروع ASP.NET MVC — تفصيلي من الصفر    
---

## 📌 فهرس المحتوى

1. [إيه هو الـ MVC أصلاً؟](#1-إيه-هو-الـ-mvc-أصلاً)
2. [إيه هو الـ N-Tier Architecture؟](#2-إيه-هو-الـ-n-tier-architecture)
3. [هيكل المشروع الكامل](#3-هيكل-المشروع-الكامل)
4. [الـ DAL — Data Access Layer](#4-الـ-dal--data-access-layer)
   - [4.1 — الـ MVCAppDbContext بالتفصيل الكامل](#41--الـ-mvcappdbcontext-بالتفصيل-الكامل)
   - [4.2 — الـ Department Entity](#42--الـ-department-entity)
5. [الـ BLL — Business Logic Layer](#5-الـ-bll--business-logic-layer)
6. [الـ Interface بالتفصيل](#6-الـ-interface-بالتفصيل)
7. [الـ DepartmentRepository بالتفصيل](#7-الـ-departmentrepository-بالتفصيل)
8. [الـ Dependency Injection بالتفصيل](#8-الـ-dependency-injection-بالتفصيل)
9. [الـ Controller بالتفصيل — سطر بسطر](#9-الـ-controller-بالتفصيل--سطر-بسطر)
10. [الـ Views بالتفصيل — سطر بسطر](#10-الـ-views-بالتفصيل--سطر-بسطر)
11. [الـ Tag Helpers بالتفصيل الكامل](#11-الـ-tag-helpers-بالتفصيل-الكامل)
12. [الـ Razor Syntax بالتفصيل](#12-الـ-razor-syntax-بالتفصيل)
13. [الـ ModelState والـ Validation](#13-الـ-modelstate-والـ-validation)
14. [الـ Async و Await ليه بنستخدمهم؟](#14-الـ-async-و-await-ليه-بنستخدمهم)
15. [تدفق البيانات الكامل في المشروع](#15-تدفق-البيانات-الكامل-في-المشروع)
16. [مشاكل في الكود الحالي وإزاي تحلها](#16-مشاكل-في-الكود-الحالي-وإزاي-تحلها)
17. [إضافات متقدمة للمشروع](#17-إضافات-متقدمة-للمشروع)

---

## 1. إيه هو الـ MVC أصلاً؟

الـ **MVC** اختصار لـ **Model — View — Controller**
ده نمط تصميم (Design Pattern) بيقسم التطبيق لـ 3 أجزاء مستقلة.

### الجزء الأول — Model
الـ **Model** هو البيانات نفسها. زي مثلاً الـ `Department` Class:

```csharp
public class Department
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public DateTime DateOfCreation { get; set; }
}
```

ده بيمثل "قسم" في قاعدة البيانات — مش بيعمل أي حاجة، بس بيحتفظ بالبيانات.

### الجزء الثاني — View
الـ **View** هو الصفحة اللي المستخدم بيشوفها — الـ HTML.
في ASP.NET بنكتبها بـ **Razor** (HTML + C#):

```html
@model Department
<h1>@Model.Name</h1>
```

### الجزء الثالث — Controller
الـ **Controller** هو "المدير" — بيستقبل الـ Request، بيطلب البيانات، وبيبعت الـ Response:

```csharp
public class DepartmentController : Controller
{
    public IActionResult Index()
    {
        var departments = /* جيب البيانات */;
        return View(departments); // ابعت للـ View
    }
}
```

### إزاي الـ 3 بيتكلموا مع بعض؟

```
المستخدم يكتب URL في المتصفح
         ↓
الـ Routing بيحدد أي Controller وأي Action
         ↓
الـ Controller بيجيب البيانات (Model)
         ↓
الـ Controller بيبعت البيانات للـ View
         ↓
الـ View بيعمل HTML ويبعته للمتصفح
         ↓
المستخدم بيشوف الصفحة
```

---

## 2. إيه هو الـ N-Tier Architecture؟

### المشكلة اللي بيحلها

تخيل إنك كتبت الكود ده في الـ Controller:

```csharp
public class DepartmentController : Controller
{
    private readonly MVCAppDbContext _context; // ← Controller بيكلم DB مباشرة!

    public IActionResult Index()
    {
        var departments = _context.Departments.ToList();
        return View(departments);
    }
}
```

ده شغال، بس فيه مشاكل كتير:

| المشكلة | التفسير |
|---|---|
| **Tight Coupling** | الـ Controller مرتبط بالـ Database مباشرة |
| **صعب التعديل** | لو غيرت الـ Database هتعدل في كل Controller |
| **صعب الـ Testing** | مش تقدر تـ Test الـ Controller من غير Database |
| **الكود بيتكرر** | نفس الـ Query بتكتبها في أكتر من Controller |
| **الـ Business Logic متبعثرة** | منتشرة في كل Controllers |

### الحل — N-Tier Architecture

بنقسم المشروع لـ **طبقات (Tiers/Layers)**، كل طبقة مسؤولة عن حاجة معينة فقط:

```
┌─────────────────────────────────┐
│   PL — Presentation Layer       │  ← الـ UI (Controllers + Views)
│   (MVC03.PL)                    │
└────────────────┬────────────────┘
                 │ بتكلم
┌────────────────▼────────────────┐
│   BLL — Business Logic Layer    │  ← الـ Rules والـ Logic
│   (mvc.BLL)                     │
└────────────────┬────────────────┘
                 │ بتكلم
┌────────────────▼────────────────┐
│   DAL — Data Access Layer       │  ← الـ Database
│   (mvc.DAL)                     │
└─────────────────────────────────┘
```

> 🔑 **القاعدة الأساسية:** كل طبقة بتكلم اللي تحتها بس — مش بتتخطى طبقة!
> يعني الـ PL ما بتكلمش الـ DAL مباشرة، لازم تعدي من الـ BLL.

---

## 3. هيكل المشروع الكامل

```
📁 Solution
│
├── 📁 mvc.DAL                        ← Data Access Layer
│   ├── 📁 Context
│   │   └── MVCAppDbContext.cs         ← بوابة الـ Database
│   └── 📁 Entities
│       └── Department.cs             ← الـ Model/Entity
│
├── 📁 mvc.BLL                        ← Business Logic Layer
│   ├── 📁 Interfaces
│   │   └── IDepartmentRepository.cs  ← العقد (Contract)
│   └── 📁 Repositories
│       └── DepartmentRepository.cs   ← التنفيذ الفعلي
│
└── 📁 MVC03.PL                       ← Presentation Layer
    ├── 📁 Controllers
    │   └── DepartmentController.cs
    └── 📁 Views
        └── 📁 Department
            ├── Index.cshtml
            ├── Create.cshtml
            ├── Details.cshtml
            ├── Update.cshtml
            └── Delete.cshtml
```

---

## 4. الـ DAL — Data Access Layer

### 4.1 — الـ MVCAppDbContext بالتفصيل الكامل

#### إيه هو الـ DbContext أصلاً؟

الـ `DbContext` هو الـ Class الأساسي اللي جاي من مكتبة **Entity Framework Core**.
هو اللي بيعمل كل الـ Magic — بيحول الـ C# Objects لـ SQL Queries والعكس.

```
C# Code          Entity Framework Core        SQL Server
────────         ─────────────────────        ──────────
departments      →  SELECT * FROM             → Database
.ToList()           Departments
```

ده اللي بنسميه **ORM — Object Relational Mapper**:
يعني بيعمل Mapping بين الـ Objects في الـ C# والـ Tables في الـ Database.

---

#### الـ Inheritance — مين بييجي من مين؟

```
System.Object                    (أساس كل حاجة في C#)
    └── DbContext                (من Microsoft.EntityFrameworkCore)
            └── MVCAppDbContext  (الـ Class اللي عملناه إحنا)
```

```csharp
// DbContext جاي من الـ Namespace ده
using Microsoft.EntityFrameworkCore;

// MVCAppDbContext بيرث من DbContext
// يعني بياخد كل قدراته:
//   - التعامل مع الـ Database
//   - الـ Change Tracking
//   - الـ Query Building
//   - الـ Transaction Management
public class MVCAppDbContext : DbContext
```

---

#### الـ Constructor بالتفصيل

```csharp
// DbContextOptions<MVCAppDbContext> → الـ Options بتاعة الـ Context ده بالتحديد
// مش DbContextOptions بدون Generic عشان ممكن يكون في أكتر من DbContext في المشروع
// فلازم يحدد هو بيكلم مين
public MVCAppDbContext(DbContextOptions<MVCAppDbContext> options)
    : base(options)
    // : base(options) → بيبعت الـ Options للـ Parent Class (DbContext)
    // عشان DbContext هو اللي بيستخدم الـ Options دي فعلاً
    // زي ما بتقوله: "يا أبويا، خد الإعدادات دي وشتغل بيها"
{
    // الـ Constructor فاضي عمداً
    // كل الشغل بيتعمل في الـ base(options)
}
```

**إيه اللي جوا الـ DbContextOptions؟**

```
DbContextOptions بتحتوي على:
├── Provider   → SQL Server? SQLite? PostgreSQL?
├── Connection String → أدرس الـ Server + اسم الـ Database + الـ Auth
├── Command Timeout → قد إيه بنستنى الـ Query
└── Logging Settings → هنسجل الـ Queries ولا لأ؟
```

---

#### الـ OnConfiguring — وليه متعطل؟

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // ده Method بييجي من DbContext الأصلي
    // بتـ Override عليه لو عايز تحدد الـ Connection String هنا

    // ⚠️ الكود ده معلق عمداً:
    // optionsBuilder.UseSqlServer("Server=IBRAHIMSHAFIQ\\SQLEXPRESS;...");

    // ليه معلق؟
    // لأن الـ Connection String هنا معناها:
    // 1. اتحولت لـ DLL وقت الـ Build → مش تقدر تعدل عليها
    // 2. لو غيرت السيرفر → لازم تعمل Rebuild للمشروع كله
    // 3. مش آمن — Connection String في الكود ممكن يتشاف من أي حد
}
```

---

#### الـ Connection String — فين المكان الصح؟

**المشكلة:**

```
لما بتعمل Build للمشروع:
  MVCAppDbContext.cs  →  mvc.DAL.dll  ← ملف مضغوط، مش تقدر تعدله!

لو حطيت الـ Connection String في الـ .cs:
  بعد الـ Build → مش تقدر تغير السيرفر أو الـ Database
  المشروع على سيرفر Production → نفس الـ Development Connection!
```

**الحل — appsettings.json:**

```json
// appsettings.json → في الـ PL Project (MVC03.PL)
// ده ملف JSON عادي — مش بيتحول لـ DLL
// تقدر تعدل عليه حتى بعد الـ Deploy!
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=IBRAHIMSHAFIQ\\SQLEXPRESS;Database=MVCAppDB;Integrated Security=True;Trusted_Connection=True"
  }
}
```

**شرح الـ Connection String:**

```
Server=IBRAHIMSHAFIQ\\SQLEXPRESS
  └── اسم السيرفر\اسم الـ Instance
      (عندك \\  عشان في JSON الـ \ بتحتاج Escape)

Database=MVCAppDB
  └── اسم الـ Database اللي هتشتغل عليها
      (EF هيعملها لو مش موجودة بعد الـ Migration)

Integrated Security=True
  └── هيستخدم Windows Authentication
      يعني نفس اليوزر اللي شغال على الويندوز

Trusted_Connection=True
  └── نفس Integrated Security — بس للـ Compatibility
```

---

#### الـ Program.cs — إزاي بنربط كل حاجة

```csharp
// Program.cs في الـ PL Project

var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────────────────────
// تسجيل الـ DbContext في الـ DI Container
// ───────────────────────────────────────────────────────
builder.Services.AddDbContext<MVCAppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// شرح كل جزء:

// builder.Services
//   └── الـ DI Container اللي بنسجل فيه كل الـ Services

// .AddDbContext<MVCAppDbContext>(...)
//   └── بيقوله: "سجل MVCAppDbContext كـ Scoped Service"
//   └── يعني: Object جديد لكل HTTP Request

// options => options.UseSqlServer(...)
//   └── Lambda بتقوله: "استخدم SQL Server كـ Provider"
//   └── UseSqlServer() جاي من: Microsoft.EntityFrameworkCore.SqlServer

// builder.Configuration.GetConnectionString("DefaultConnection")
//   └── بيقرأ من appsettings.json
//   └── بيدور على "ConnectionStrings" → "DefaultConnection"
//   └── بيرجع: "Server=IBRAHIMSHAFIQ\\SQLEXPRESS;..."
```

**الـ Full Inheritance Chain للـ Program.cs:**

```
WebApplication.CreateBuilder(args)
    └── بيعمل WebApplicationBuilder
            └── builder.Services → IServiceCollection
                    └── .AddDbContext<T>() → Extension Method من EF Core
                            └── بيسجل T كـ Scoped في الـ DI Container
```

---

#### الـ Migrations — إزاي بتعمل الـ Tables؟

الـ **Migration** هو طريقة EF بيحفظ بيها التغييرات في الـ Database Schema.

**الخطوات بالترتيب:**

**الخطوة 1 — تثبيت الـ Package المطلوب:**

```
في الـ PL Project → NuGet Package Manager:
Install: Microsoft.EntityFrameworkCore.Design
```

ليه في الـ PL وليس الـ DAL؟ لأن الـ Migration Tools بتشتغل من الـ Startup Project.

```
المشروع اللي فيه Program.cs = الـ Startup Project = MVC03.PL
الـ Design Package لازم يكون هنا عشان الـ Tools تلاقيه
```

**الخطوة 2 — Package Manager Console:**

```
Tools → NuGet Package Manager → Package Manager Console

تأكد من:
├── Default project: mvc.DAL  ← اللي فيه الـ DbContext
└── Startup project: MVC03.PL ← اللي فيه الـ Program.cs
```

**الخطوة 3 — أول Migration:**

```powershell
# بتعمل Snapshot من الـ Entities الحالية
Add-Migration InitialCreate

# النتيجة: بيعمل Folder اسمه Migrations في الـ DAL Project فيه:
# └── 20240101000000_InitialCreate.cs  ← الـ Migration File
# └── MVCAppDbContextModelSnapshot.cs  ← Snapshot من الـ Schema الحالي
```

**الخطوة 4 — تطبيق الـ Migration على الـ Database:**

```powershell
Update-Database

# بيعمل:
# 1. بيشوف الـ Database موجودة ولا لأ → لو لأ بيعملها
# 2. بيعمل جدول __EFMigrationsHistory لتتبع الـ Migrations
# 3. بينفذ الـ SQL اللي في الـ Migration File
# 4. CREATE TABLE Departments (Id INT IDENTITY PRIMARY KEY, ...)
```

**إيه اللي جوا الـ Migration File؟**

```csharp
// 20240101000000_InitialCreate.cs
public partial class InitialCreate : Migration
{
    // Up() → بيتنفذ لما بتعمل Update-Database
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Departments",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"), // AUTO INCREMENT
                Code = table.Column<string>(maxLength: 10, nullable: false),
                Name = table.Column<string>(maxLength: 100, nullable: false),
                DateOfCreation = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Departments", x => x.Id);
            });
    }

    // Down() → بيتنفذ لما بتعمل Rollback (Remove-Migration أو Update-Database -Migration)
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Departments");
    }
}
```

**أوامر الـ Migrations المهمة:**

```powershell
# إضافة Migration جديدة
Add-Migration اسم_التغيير
# مثال: Add-Migration AddEmployeeTable

# تطبيق آخر Migration
Update-Database

# الرجوع لـ Migration معينة
Update-Database -Migration InitialCreate

# حذف آخر Migration (لو لسه مش اتطبقت)
Remove-Migration

# شوف كل الـ Migrations والحالة
Get-Migration
```

---

#### الـ DbSet بالتفصيل

```csharp
// كل DbSet بيمثل جدول في الـ Database
public DbSet<Department> Departments { get; set; }

// DbSet<Department>:
//   ← Generic Type Parameter → بيحدد الـ Entity
//   ← بيدي قدرات:
//     - Departments.ToList()        → SELECT * FROM Departments
//     - Departments.Find(id)        → SELECT * WHERE Id = @id
//     - Departments.Add(dept)       → INSERT INTO Departments
//     - Departments.Update(dept)    → UPDATE Departments SET ...
//     - Departments.Remove(dept)    → DELETE FROM Departments WHERE ...
//     - Departments.Where(...)      → SELECT * WHERE ...
//     - Departments.OrderBy(...)    → ORDER BY ...
//     - Departments.Include(...)    → JOIN مع جداول تانية

// Departments (بالجمع):
//   Convention في EF → بيستخدم اسم الـ DbSet كاسم الـ Table
//   Department (Entity) → Departments (Table Name)
```

---

#### ليه بنعمل OnConfiguring فاضي؟

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // فاضي عمداً!
}

// ليه بنكتبه من الأصل لو فاضي؟
// 1. Documentation → بيوضح إننا عارفين بالـ Method دي ومش ناسيينها
// 2. Extension Point → لو في المستقبل احتجنا نضيف إعدادات إضافية
//    زي: optionsBuilder.EnableSensitiveDataLogging(); للـ Development
//    أو: optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

// لو مش هتستخدمه خالص → تمسحه عادي، مش هيأثر
```

---

#### الصورة الكاملة — كل حاجة مع بعض

```
appsettings.json
  "DefaultConnection": "Server=...;Database=MVCAppDB;..."
            │
            │ builder.Configuration.GetConnectionString(...)
            ▼
Program.cs
  builder.Services.AddDbContext<MVCAppDbContext>(options =>
      options.UseSqlServer(connectionString))
            │
            │ DI Container بيسجل MVCAppDbContext كـ Scoped
            ▼
MVCAppDbContext
  Constructor(DbContextOptions<MVCAppDbContext> options)
      : base(options)   ← بيبعت الـ Options لـ DbContext
  DbSet<Department> Departments   ← بيمثل جدول Departments
            │
            │ EF Core بيستخدم الـ Options
            ▼
SQL Server
  Database: MVCAppDB
  Table: Departments
    Id | Code | Name | DateOfCreation
```

---

### 4.2 — الـ Department Entity

```csharp
public class Department
{
    // Primary Key — EF بيعرفه أوتوماتيك لو اسمه Id أو DepartmentId
    public int Id { get; set; }

    // [Required] → NOT NULL في الـ Database
    // ErrorMessage → الرسالة اللي بتظهر للمستخدم لو الـ Field فاضي
    [Required(ErrorMessage = "كود القسم مطلوب")]

    // [StringLength] → VARCHAR(10) في الـ Database
    // MinimumLength → بيتأكد إن الطول مش أقل من 2
    [StringLength(10, MinimumLength = 2, ErrorMessage = "الكود بين 2 و 10 أحرف")]

    // [Display] → الاسم اللي بيتعرض في الـ Label في الـ View
    [Display(Name = "كود القسم")]
    public string Code { get; set; }

    [Required(ErrorMessage = "اسم القسم مطلوب")]
    [StringLength(100, ErrorMessage = "الاسم لا يتجاوز 100 حرف")]
    [Display(Name = "اسم القسم")]
    public string Name { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "تاريخ الإنشاء")]
    // = DateTime.Now → Default Value لو مفيش تاريخ اتبعت
    public DateTime DateOfCreation { get; set; } = DateTime.Now;
}
```

### إزاي الـ EF بيحول الـ Entity لـ SQL Table؟

```
C# Class             →    SQL Table
─────────────────────────────────────
Department           →    Departments
int Id               →    Id INT PRIMARY KEY IDENTITY
string Code          →    Code NVARCHAR(10) NOT NULL
string Name          →    Name NVARCHAR(100) NOT NULL
DateTime DateOf...   →    DateOfCreation DATETIME2
```

---

## 5. الـ BLL — Business Logic Layer

### إيه اللي بيحصل جوا الـ BLLClass.cs؟

```csharp
namespace mvc.BLL
{
    public class BLLClass
    {
        // الملف ده مجرد ملف تعريفي للـ Project
        // مش بيحتوي على Main Method
        // لأن ده مش Application — ده Library بتستخدمه تطبيقات تانية

        // الكود المعلق ده بيشرح المشكلة اللي كانت موجودة:

        /*
        public class DepartmentController : Controller
        {
            private readonly MVCAppDbContext _context;
            // ← هنا الـ Controller بيعرف عن الـ DbContext مباشرة
            // ده مش كويس عشان:
            // 1. الـ Controller مش مفروض يعرف إزاي البيانات بتتحفظ
            // 2. الكود صعب يتاختبر
            // 3. الكود صعب يتغير
        }
        */

        // الحل: Repository Pattern + Dependency Injection
        // بنعمل:
        // 1. Interface → بيحدد "إيه" اللي هيتعمل
        // 2. Repository → بيحدد "إزاي" هيتعمل
        // 3. DI → بيربط بينهم
    }
}
```

---

## 6. الـ Interface بالتفصيل

### الكود الكامل

```csharp
namespace mvc.BLL.Interfaces
{
    public interface IDepartmentRepository
    {
        Department? GetDepartmentById(int id);
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        int Add(Department department);
        int Update(Department department);
        int Delete(Department department);
    }
}
```

### شرح كل ميثود

#### `Department? GetDepartmentById(int id)`

```
Department?   → النوع اللي بترجعه، علامة الـ ? معناها ممكن ترجع null
               (لو مفيش Department بالـ Id ده)
GetDepartment → اسم الميثود
ById          → جزء من الاسم بيوضح إننا بندور بالـ Id
(int id)      → الـ Parameter — بناخد Id من نوع int
```

#### `Task<IEnumerable<Department>> GetAllDepartmentsAsync()`

```
Task<...>              → الميثود Async (هتشرح تفصيلي في القسم 14)
IEnumerable<Department> → مجموعة من الـ Departments (زي List بس أعم)
GetAllDepartments       → اسم الميثود
Async                   → Convention بيقول إن الميثود Async
()                      → مش محتاجة Parameters — بتجيب الكل
```

#### `int Add(Department department)`

```
int        → بترجع عدد الـ Rows اللي اتأثرت في الـ Database
             لو رجعت 1 → نجح الـ Insert
             لو رجعت 0 → فشل
Add        → اسم الميثود
(Department department) → بناخد الـ Object كامل عشان نحفظه
```

### ليه Interface وليس Class مباشرة؟

```csharp
// ❌ من غير Interface — الـ Controller مرتبط بـ Class معين
public class DepartmentController : Controller
{
    private readonly DepartmentRepository _repo;
    // لو عايز تغير لـ MongoDB → لازم تعدل هنا وفي كل مكان
}

// ✅ مع Interface — الـ Controller مش عارف مين بينفذ
public class DepartmentController : Controller
{
    private readonly IDepartmentRepository _repo;
    // ممكن تغير التنفيذ من غير ما تلمس الـ Controller خالص
}
```

**مثال عملي على الفايدة:**

```csharp
// الـ SQL Server Repository
public class SqlDepartmentRepository : IDepartmentRepository
{
    // بيحفظ في SQL Server
}

// الـ MongoDB Repository
public class MongoDepartmentRepository : IDepartmentRepository
{
    // بيحفظ في MongoDB
}

// في Program.cs — بس هنا بتغير سطر واحد!
// builder.Services.AddScoped<IDepartmentRepository, SqlDepartmentRepository>();
builder.Services.AddScoped<IDepartmentRepository, MongoDepartmentRepository>();

// الـ Controller مش اتغير خالص ✅
```

---

## 7. الـ DepartmentRepository بالتفصيل

### الكود الكامل مع الشرح سطر بسطر

```csharp
// بنقوله إن الـ Class ده بينفذ الـ Interface ده
// يعني لازم يعمل كل الميثودز اللي في الـ Interface
public class DepartmentRepository : IDepartmentRepository
{
    // readonly → مش هيتغير بعد ما يتحدد في الـ Constructor
    // private → بس الـ Class ده اللي يشوفه
    private readonly MVCAppDbContext _context;

    // Constructor — بيتنادى لما الـ DI Container يعمل Object من الـ Class ده
    public DepartmentRepository(MVCAppDbContext context)
    {
        _context = context;
        // الـ DI Container بيبعتلنا الـ MVCAppDbContext جاهز
        // إحنا مش بنعمل new MVCAppDbContext() بإيدينا
    }
```

#### ميثود الـ Add

```csharp
    public int Add(Department department)
    {
        // Add() → بتضيف الـ Object للـ DbSet في الـ Memory
        // لسه مش اتحفظ في الـ Database!
        _context.Departments.Add(department);

        // SaveChanges() → دي اللي بتكتب في الـ Database فعلاً
        // بتعمل INSERT INTO Departments VALUES (...)
        // بترجع عدد الـ Rows اللي اتأثرت
        return _context.SaveChanges();

        // لو رجعت 1 → تمام، اتحفظ
        // لو رجعت 0 → حاجة غلط
    }
```

#### ميثود الـ Delete

```csharp
    public int Delete(Department department)
    {
        // Remove() → بتعلم الـ Object إنه محتاج يتحذف
        // بس لسه في الـ Memory
        _context.Departments.Remove(department);

        // SaveChanges() → بتعمل DELETE FROM Departments WHERE Id = ...
        return _context.SaveChanges();
    }
```

#### ميثود الـ GetAllDepartmentsAsync

```csharp
    // async → بتقول إن الميثود دي Asynchronous
    // Task<> → زي الـ Promise في JavaScript، بيقول "هترجع نتيجة في المستقبل"
    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        // ToListAsync() → SELECT * FROM Departments
        // await → بنستنى النتيجة من غير ما نوقف الـ Thread
        return await _context.Departments.ToListAsync();
    }
```

#### ميثود الـ GetDepartmentById

```csharp
    public Department? GetDepartmentById(int id)
        // Find() → SELECT * FROM Departments WHERE Id = @id
        // الميزة: بيبص في الـ Memory Cache الأول
        // يعني لو الـ Object ده اتجاب قبل كده، مش هيروح للـ Database تاني
        => _context.Departments.Find(id);
        // لو ملقاش، بترجع null (عشان الـ ? في النوع)
```

#### ميثود الـ Update

```csharp
    public int Update(Department department)
    {
        // Update() → بتعلم الـ Object إنه اتعدل
        // EF بيعمل Tracking للـ Object ويعرف إيه اللي اتغير
        _context.Departments.Update(department);

        // SaveChanges() → UPDATE Departments SET ... WHERE Id = @id
        return _context.SaveChanges();
    }
}
```

### الفرق بين `Add()` و `AddAsync()`

```csharp
// Add() → Synchronous — بيوقف الـ Thread لحد ما يخلص
_context.Departments.Add(department);
_context.SaveChanges();

// AddAsync() → Asynchronous — مش بيوقف الـ Thread
await _context.Departments.AddAsync(department);
await _context.SaveChangesAsync();

// في الـ Add() → التأثير على الـ Memory بس (سريع جداً)
// فمش محتاج Async هنا — لكن الـ SaveChanges() لازم يكون Async
```

---

## 8. الـ Dependency Injection بالتفصيل

### المشكلة اللي بيحلها

```csharp
// ❌ من غير DI — بنعمل Objects بإيدينا
public class DepartmentController : Controller
{
    private readonly DepartmentRepository _repo;

    public DepartmentController()
    {
        // مشاكل:
        // 1. لو DepartmentRepository محتاج DbContext → لازم تعمله بإيدك
        // 2. لو DbContext محتاج Connection String → فين تجيبها؟
        // 3. لو عايز تـ Test → مش هتقدر تـ Mock الـ Repository
        var context = new MVCAppDbContext(???);
        _repo = new DepartmentRepository(context);
    }
}
```

```csharp
// ✅ مع DI — الـ Framework بيعمل الـ Objects
public class DepartmentController : Controller
{
    private readonly IDepartmentRepository _repo;

    // الـ Framework بيشوف إن الـ Constructor محتاج IDepartmentRepository
    // بيبص في الـ DI Container ويلاقي إن IDepartmentRepository → DepartmentRepository
    // بيعمل Object من DepartmentRepository (وبيحل كل الـ Dependencies بتاعته)
    // وبيبعته هنا أوتوماتيك
    public DepartmentController(IDepartmentRepository repo)
    {
        _repo = repo;
    }
}
```

### الأنواع الثلاثة بالتفصيل

#### Singleton

```csharp
builder.Services.AddSingleton<ILoggingService, LoggingService>();
```

```
Request 1 ──→ LoggingService (Object #1)
Request 2 ──→ LoggingService (Object #1) ← نفس الـ Object!
Request 3 ──→ LoggingService (Object #1) ← نفس الـ Object!
```

- بيعمل Object واحد بس طول عمر التطبيق
- كل الـ Requests بتستخدم نفس الـ Object
- **مناسب لـ:** Logging, Configuration, Caching
- **خطير لو:** الـ Service بتحتفظ بـ State خاص بكل Request

#### Scoped

```csharp
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
```

```
Request 1 ──→ DepartmentRepository (Object #1)
              [نفس الـ Object لو اتطلب أكتر من مرة في نفس الـ Request]
              [اتحذف بعد انتهاء الـ Request]

Request 2 ──→ DepartmentRepository (Object #2) ← Object جديد!
```

- Object جديد لكل Request
- لو اتطلب أكتر من مرة في نفس الـ Request → نفس الـ Object
- **مناسب لـ:** DbContext, Repositories ← الأنسب للمشروع ده

#### Transient

```csharp
builder.Services.AddTransient<IEmailService, EmailService>();
```

```
Request 1:
  ├── Controller طلبه → EmailService (Object #1)
  └── Service تاني طلبه → EmailService (Object #2) ← Object مختلف!

Request 2:
  └── Controller طلبه → EmailService (Object #3) ← Object جديد تاني!
```

- Object جديد كل مرة يتطلب فيها
- **مناسب لـ:** Services خفيفة ومش بتحتفظ بـ State

### الـ Program.cs — أين بنسجل الـ Services

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// إضافة الـ DbContext
builder.Services.AddDbContext<MVCAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// لما حد يطلب MVCAppDbContext → الـ Framework يعمله Object
// ويعمله Scoped أوتوماتيك (الـ Default للـ DbContext)

// تسجيل الـ Repository
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
// لما حد يطلب IDepartmentRepository → ابعتله DepartmentRepository

var app = builder.Build();
```

---

## 9. الـ Controller بالتفصيل — سطر بسطر

### الكود الكامل مع الشرح

```csharp
// بيقوله إن الـ Class ده Controller
// Controller هو Base Class بييدينا:
// - View() لإرجاع الـ View
// - RedirectToAction() للـ Redirect
// - NotFound() لإرجاع 404
// - ModelState للـ Validation
public class DepartmentController : Controller
{
    // readonly → مش هيتغير بعد الـ Constructor
    // private → بس الـ Class ده يشوفه
    // IDepartmentRepository → بنشتغل مع الـ Interface مش الـ Class
    private readonly IDepartmentRepository _deparmentRepository;

    // Constructor — الـ DI بيبعتلنا الـ Repository جاهز
    public DepartmentController(IDepartmentRepository departmentRepository)
    {
        _deparmentRepository = departmentRepository;
    }
```

#### Action الـ Index

```csharp
    // async Task<IActionResult> → Action Async بترجع IActionResult
    // IActionResult → ممكن ترجع View أو Redirect أو NotFound أو غيره
    public async Task<IActionResult> Index()
    {
        // بنطلب كل الأقسام من الـ Repository
        // await → مش بنوقف الـ Thread لحد ما تيجي البيانات
        var departments = await _deparmentRepository.GetAllDepartmentsAsync();

        // View("Index", departments):
        // "Index" → افتح Views/Department/Index.cshtml
        // departments → ابعت البيانات دي كـ Model
        return View("Index", deparmtnets);
    }
```

#### Action الـ Create GET

```csharp
    // [HttpGet] → الـ Action دي بتشتغل بس لو الـ Request نوعه GET
    // GET = لما المستخدم يفتح الصفحة أو يضغط على لينك
    [HttpGet]
    public IActionResult Create()
    {
        // بنبعت Object فاضي للـ View
        // ليه مش بنبعت null؟
        // عشان الـ Tag Helpers زي asp-for محتاجة Object موجود
        // عشان تقدر تعمل الـ id و name الصح للـ Inputs
        return View(new Department() { });
    }
```

#### Action الـ Create POST

```csharp
    // [HttpPost] → الـ Action دي بتشتغل بس لو الـ Request نوعه POST
    // POST = لما المستخدم يبعت Form
    [HttpPost]
    public IActionResult Create(Department department)
    // department → الـ Model Binder بياخد البيانات من الـ Form
    // ويحطها في Object من نوع Department أوتوماتيك
    {
        // ModelState.IsValid → بيتأكد إن البيانات صح
        // بيشوف الـ Data Annotations على الـ Model
        // [Required], [StringLength], إلخ
        if (ModelState.IsValid)
        {
            _deparmentRepository.Add(department);

            // RedirectToAction → بدل ما نرجع View
            // بنعمل Redirect للـ Action التاني
            // nameof(Index) → أحسن من "Index" عشان لو الاسم اتغير الـ Compiler يقولك
            return RedirectToAction(nameof(Index));
        }

        // لو في Validation Errors → رجع نفس الـ View
        // مع نفس البيانات اللي المستخدم كتبها عشان متتمسحش
        return View(department);
    }
```

#### Action الـ Delete

```csharp
    // ⚠️ مشكلة هنا! مش فيه [HttpPost]
    // ده معناه إن الـ Action بتشتغل مع GET و POST
    // خطر لأن أي حد يعمل لينك زي:
    // <img src="/Department/Delete/5"> → هيحذف القسم 5 من غير ما حد يعرف!
    public IActionResult Delete(int id)
    {
        var department = _deparmentRepository.GetDepartmentById(id);

        // is null → أحسن من == null في C# الحديثة
        if (department is null) { return NotFound(); }
        // NotFound() → بترجع HTTP 404 Status Code

        _deparmentRepository.Delete(department);
        return RedirectToAction(nameof(Index));
    }
```

#### Action الـ Details

```csharp
    public IActionResult Details(int id)
    {
        // الـ id ممكن ييجي من الـ URL زي /Department/Details/5
        // لو جاء 0 → مش Valid
        if (id == 0) { return NotFound(); }

        var department = _deparmentRepository.GetDepartmentById(id);

        // ;بنتأكد تاني إن الـ Object موجود
        if (department is null) { return NotFound(); }

        return View("Details", department);
    }
```

#### Action الـ Update GET و POST

```csharp
    [HttpGet]
    public IActionResult Update(int id)
    {
        var department = _deparmentRepository.GetDepartmentById(id);

        // لو مفيش قسم بالـ ID ده
        if (department is null)
            return NotFound();

        // بنبعت البيانات الموجودة للـ View
        // عشان الـ Form يتعبى بالبيانات القديمة
        return View(department);
    }

    [HttpPost]
    public IActionResult Update(Department department)
    // الـ Model Binder بياخد البيانات من الـ Form
    // بما فيها الـ Id اللي كان Hidden في الـ Form
    {
        if (ModelState.IsValid)
        {
            _deparmentRepository.Update(department);
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }
}
```

### إيه الـ Model Binding؟

```
Form Data اللي بيتبعت:
  Code = "HR"
  Name = "Human Resources"
  DateOfCreation = "2024-01-01"
         ↓
الـ Model Binder بياخد الـ Data دي
ويحطها في Object من نوع Department:
  department.Code = "HR"
  department.Name = "Human Resources"
  department.DateOfCreation = DateTime(2024, 1, 1)
         ↓
بيبعت الـ Object للـ Action Method
```

---

## 10. الـ Views بالتفصيل — سطر بسطر

### Index.cshtml

```html
@* السطر ده بيقول: النوع اللي جاي من الـ Controller هو IEnumerable<Department> *@
@* يعني List من الـ Departments *@
@model IEnumerable<Department>;

@* ViewData["Title"] → بيتعرض في الـ <title> تاع الصفحة في الـ _Layout *@
@{
    ViewData["Title"] = "Departments";
}

@* الكارت اللي فيه العنوان وزر الإضافة *@
<div class="page-card mb-4">
    <div class="d-flex flex-row-reverse flex-wrap justify-content-between align-items-center">
        <div>
            <h1 class="main-header-text">🏢 جميع الأقسام</h1>
            <p class="text-muted mt-3">إدارة وعرض جميع أقسام المؤسسة</p>
        </div>

        @* asp-action="Create" → هيعمل href="/Department/Create" *@
        <a asp-action="Create" class="c-btn" style="--bg-clr:#5b8eda;">
            <i class="fa-solid fa-plus"></i>
            إضافة قسم جديد
        </a>
    </div>
</div>

@* بنتأكد إن Model موجود ومش فاضي قبل ما نعرض الجدول *@
@if (Model?.Count() > 0)
{
    <div class="department-table-wrapper">
        <table class="department-table" dir="rtl">
            <thead>
                <tr style="background-color: #5b8eda">
                    <th>
                        @* DisplayNameFor → بيجيب الاسم من [Display(Name = "...")] في الـ Model *@
                        @* لو مفيش [Display] → بياخد اسم الـ Property نفسه *@
                        <span>@Html.DisplayNameFor(Dept => Dept.Code)</span>
                    </th>
                    <th><span>@Html.DisplayNameFor(Dept => Dept.Name)</span></th>
                    <th>تاريخ الإنشاء</th>
                    <th>تفاصيل</th>
                    <th>تعديل</th>
                    <th>حذف</th>
                </tr>
            </thead>
            <tbody>
                @* foreach → بندور على كل Department في الـ Model *@
                @foreach (var item in Model)
                {
                    <tr class="d-s-table-row">
                        <td>@item.Code</td>
                        <td>@item.Name</td>

                        @* ToShortDateString() → بيعرض التاريخ بشكل قصير: "1/1/2024" *@
                        <td>@item.DateOfCreation.ToShortDateString()</td>

                        @* asp-route-id="@item.Id" → بيضيف الـ Id في الـ URL *@
                        @* النتيجة: href="/Department/Details/5" *@
                        <td>
                            <a class="c-btn p-0 p-1"
                               style="--bg-clr: #198754;"
                               asp-controller="Department"
                               asp-action="Details"
                               asp-route-id="@item.Id">
                                <i class="fas fa-eye"></i> تفاصيل
                            </a>
                        </td>
                        <td>
                            <a class="c-btn p-0 p-1"
                               style="--bg-clr: #ffc107;"
                               asp-controller="Department"
                               asp-action="Update"
                               asp-route-id="@item.Id">
                                <i class="fas fa-edit"></i> تعديل
                            </a>
                        </td>
                        <td>
                            <a class="c-btn p-0 p-1"
                               style="--bg-clr: #dc3545;"
                               asp-controller="Department"
                               asp-action="Delete"
                               asp-route-id="@item.Id">
                                <i class="fas fa-trash"></i> حذف
                            </a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
}
else
{
    @* لو مفيش أقسام → بنعرض رسالة *@
    <div class="alert alert-warning mx-auto mb-0 d-flex align-items-center justify-content-center"
         style="width: fit-content;">
        <h3 class="m-0">لا توجد أقسام 🥲</h3>
    </div>
}
```

---

### Create.cshtml

```html
@* النوع هنا Department مش IEnumerable<Department> *@
@* عشان بنتعامل مع قسم واحد بس (اللي عايزين نضيفه) *@
@model Department;

@{
    ViewData["Title"] = "Create Department";
}

<div dir="rtl">
    <h1 class="main-header-text">إضافة قسم جديد</h1>
</div>

<div class="row d-flex align-items-center justify-content-center flex-row-reverse">
    <div class="col-6">
        @* asp-action="Create" → الـ Form هيتبعت POST لـ /Department/Create *@
        @* asp-controller="Department" → يحدد الـ Controller *@
        <form asp-action="Create" asp-controller="Department">

            <div class="form-group" dir="rtl">
                @* asp-for="@Model.Code":
                   على الـ Label → بيعمل: <label for="Code">كود القسم</label>
                   الاسم من [Display(Name="كود القسم")] في الـ Model *@
                <label asp-for="@Model.Code" class="col-form-label"></label>

                @* asp-for="@Model.Code" على الـ Input:
                   بيعمل: <input type="text" id="Code" name="Code" value="...">
                   id و name مهمين عشان الـ Model Binding يشتغل *@
                <input asp-for="@Model.Code" class="form-control" />

                @* asp-validation-for="@Model.Code":
                   لو الـ Validation فشل → بيعرض رسالة الـ Error هنا
                   مثلاً: "كود القسم مطلوب" *@
                <span asp-validation-for="@Model.Code" class="text-danger"></span>
            </div>

            <div class="form-group" dir="rtl">
                <label asp-for="@Model.Name" class="col-form-label"></label>
                <input asp-for="@Model.Name" class="form-control" />
                <span asp-validation-for="@Model.Name" class="text-danger"></span>
            </div>

            <div class="d-flex align-items-center flex-row-reverse justify-content-between mt-3">
                @* type="submit" → بيبعت الـ Form لما المستخدم يضغطه *@
                <input type="submit" value="إضافة" class="c-btn" style="--bg-clr: #258cfb" />

                @* asp-action="Index" → لينك يرجع لصفحة القائمة *@
                <a asp-action="Index" class="c-btn" style="--bg-clr: #7a7a7a">رجوع الى القائمة</a>
            </div>
        </form>
    </div>
</div>
```

---

### Update.cshtml

```html
@model Department

@{
    ViewData["Title"] = "Update Department";
}

<div class="update-page">
    <div class="update-card">

        <div class="update-header">
            <div class="update-icon">
                <i class="fa-solid fa-pen-to-square"></i>
            </div>
            <h2>تعديل بيانات القسم</h2>
            <p>قم بتحديث معلومات القسم وحفظ التغييرات</p>
        </div>

        @* asp-action="Update" method="post" → بيبعت POST لـ /Department/Update *@
        <form asp-action="Update" method="post" class="update-form">

            @* ⭐ مهم جداً! *@
            @* الـ Id لازم يتبعت مع الـ Form عشان الـ Controller يعرف أي قسم هيعدل *@
            @* hidden → مش بيظهر للمستخدم بس بيتبعت مع الـ Form *@
            <input asp-for="Id" hidden />

            <div class="form-group-fantasy">
                @* asp-for="Code" → مش محتاج @Model. هنا، Razor بيعرف من الـ @model *@
                <label asp-for="Code"></label>
                @* الـ Input هيتعبى بالقيمة الموجودة في الـ Model *@
                <input asp-for="Code" class="form-control fantasy-input" />
                <span asp-validation-for="Code" class="text-danger"></span>
            </div>

            <div class="form-group-fantasy">
                <label asp-for="Name"></label>
                <input asp-for="Name" class="form-control fantasy-input" />
                <span asp-validation-for="Name" class="text-danger"></span>
            </div>

            <div class="form-group-fantasy">
                <label asp-for="DateOfCreation">تاريخ الإنشاء</label>
                @* type="date" → بيعرض Date Picker في المتصفح *@
                <input asp-for="DateOfCreation" type="date" class="form-control fantasy-input" />
            </div>

            <div class="update-actions">
                <a asp-action="Index" class="c-btn" style="--bg-clr:#6c757d">
                    <i class="fa-solid fa-arrow-right"></i> رجوع
                </a>

                @* button type="submit" → بيبعت الـ Form *@
                <button type="submit" class="c-btn" style="--bg-clr:#5b8eda">
                    <i class="fa-solid fa-floppy-disk"></i>
                    حفظ التعديلات
                </button>
            </div>

        </form>
    </div>
</div>

@* ده Section للـ Validation Scripts *@
@* محتاج jQuery Validation عشان Client-Side Validation تشتغل *@
@* @section ValidationScripts
{
    <partial name="_ValidationScriptsPartial" />
} *@
```

---

### Details.cshtml

```html
@model Department

@{
    ViewData["Title"] = "Department Details";
}

<div class="department-details-container">
    <div class="details-card">

        <div class="details-header">
            <div class="department-icon">
                <i class="fa-solid fa-building"></i>
            </div>

            @* @Model.Name → بيعرض الـ Name من الـ Object اللي جاء من الـ Controller *@
            <h1>@Model.Name</h1>
            <p>Department Information</p>
        </div>

        <div class="details-body">
            <div class="info-box">
                <span>الكود</span>
                <h4>@Model.Code</h4>
            </div>

            <div class="info-box">
                <span>اسم القسم</span>
                <h4>@Model.Name</h4>
            </div>

            <div class="info-box">
                <span>تاريخ الإنشاء</span>
                @* ToString("dd MMM yyyy") → مثلاً: "01 Jan 2024" *@
                <h4>@Model.DateOfCreation.ToString("dd MMM yyyy")</h4>
            </div>
        </div>

        <div class="details-actions">
            <a asp-action="Index" class="c-btn" style="--bg-clr:#5b8eda">
                <i class="fa-solid fa-arrow-right"></i> رجوع
            </a>

            @* asp-route-id="@Model.Id" → بيضيف Id=5 في الـ URL *@
            <a asp-action="Update" asp-route-id="@Model.Id"
               class="c-btn" style="--bg-clr:#ffc107;color:black">
                <i class="fa-solid fa-pen"></i> تعديل
            </a>

            <a asp-action="Delete"
               asp-route-id="@Model.Id"
               asp-controller="Department"
               class="c-btn" style="--bg-clr: #dc3545; color: black;">
                حذف
            </a>
        </div>
    </div>
</div>
```

---

## 11. الـ Tag Helpers بالتفصيل الكامل

Tag Helpers هي Attributes بتبدأ بـ `asp-` بتكتبها في الـ HTML وبيتحولوا لـ HTML عادي وقت الـ Render.

### `asp-controller` و `asp-action`

```html
@* Input *@
<a asp-controller="Department" asp-action="Index">القائمة</a>

@* Output (الـ HTML اللي بيتولد) *@
<a href="/Department/Index">القائمة</a>
```

```html
@* لو الـ Route غير Standard *@
<a asp-controller="Department" asp-action="Details" asp-route-id="5">تفاصيل</a>
@* Output *@
<a href="/Department/Details/5">تفاصيل</a>
```

**ليه أحسن من href مباشرة؟**

```html
@* ❌ Hard-coded — لو الـ Route اتغير هتكسر *@
<a href="/Department/Index">القائمة</a>

@* ✅ Tag Helper — بيتحدث أوتوماتيك مع الـ Route *@
<a asp-controller="Department" asp-action="Index">القائمة</a>
```

---

### `asp-for` على الـ Label

```html
@* Input *@
<label asp-for="Code"></label>

@* Output (لو الـ Property عنده [Display(Name = "كود القسم")]) *@
<label for="Code">كود القسم</label>
@* for="Code" → بيربط الـ Label بالـ Input اللي id="Code" *@
@* "كود القسم" → من [Display(Name = "كود القسم")] في الـ Model *@
@* لو مفيش [Display] → هيعرض "Code" (اسم الـ Property) *@
```

---

### `asp-for` على الـ Input

```html
@* Input *@
<input asp-for="Code" class="form-control" />

@* Output (لو القيمة الحالية هي "HR") *@
<input type="text"
       id="Code"
       name="Code"
       value="HR"
       class="form-control"
       data-val="true"
       data-val-required="كود القسم مطلوب"
       data-val-length="الكود بين 2 و 10 أحرف"
       data-val-length-max="10"
       data-val-length-min="2" />
@* لاحظ إنه بيضيف الـ Validation Attributes أوتوماتيك! *@
```

---

### `asp-for` على الـ Span (Validation Messages)

```html
@* Input *@
<span asp-validation-for="Code" class="text-danger"></span>

@* Output لو في Error *@
<span class="text-danger field-validation-error"
      data-valmsg-for="Code"
      data-valmsg-replace="true">
    كود القسم مطلوب
</span>

@* Output لو مفيش Error *@
<span class="text-danger field-validation-valid"
      data-valmsg-for="Code"
      data-valmsg-replace="true">
</span>
@* بيكون فاضي *@
```

---

### `asp-validation-summary`

```html
@* بيعرض كل الـ Errors في مكان واحد *@
<div asp-validation-summary="All" class="text-danger"></div>

@* الخيارات: *@
@* All → بيعرض كل الـ Errors (Model-level + Field-level) *@
@* ModelOnly → بيعرض الـ Errors اللي مش مرتبطة بـ Field معين *@
@* None → مش بيعرض حاجة *@
```

---

### `asp-route-{parameterName}`

```html
@* بتبعت Parameters في الـ URL *@
<a asp-action="Details" asp-route-id="5">تفاصيل</a>
@* Output: /Department/Details/5 *@

<a asp-action="Search" asp-route-keyword="HR" asp-route-page="2">بحث</a>
@* Output: /Department/Search?keyword=HR&page=2 *@
```

---

### `@Html.DisplayNameFor()` vs `@Html.DisplayFor()`

```html
@* DisplayNameFor → بيجيب اسم الـ Property (مش القيمة) *@
@Html.DisplayNameFor(d => d.Code)
@* Output: "كود القسم" (من [Display(Name = "...")]) *@


@* DisplayFor → بيعرض القيمة بالـ Format الصح *@
@Html.DisplayFor(d => d.Code)
@* Output: "HR" (القيمة الفعلية) *@

@Html.DisplayFor(d => d.DateOfCreation)
@* Output: "1/1/2024" (مش DateTime الكامل) *@
```

---

## 12. الـ Razor Syntax بالتفصيل

### الـ `@model`

```html
@* بيقول للـ View نوع الـ Object اللي جاي من الـ Controller *@
@model Department
@* → Model عنده خصائص: Id, Code, Name, DateOfCreation *@

@model IEnumerable<Department>
@* → Model ده List من الـ Departments *@

@model List<Department>
@* → نفس بس List أكتر تخصصاً *@
```

### الـ `@{ }` — C# Code Block

```html
@{
    // هنا بتكتب C# عادي
    ViewData["Title"] = "الأقسام";

    var count = Model?.Count() ?? 0;
    var pageTitle = count > 0 ? $"الأقسام ({count})" : "لا توجد أقسام";
}

<h1>@pageTitle</h1>
```

### الـ `@( )` — C# Expression

```html
@* عشان تكتب Expression متعددة *@
<p>عدد الأقسام: @(Model?.Count() ?? 0)</p>

<p>@(DateTime.Now.Year)</p>

@* لما يكون في التباس *@
<p>@item.Name@item.Code</p>  @* غلط — هيبقى ملتبس *@
<p>@(item.Name)@(item.Code)</p>  @* صح *@
```

### الـ `@if` و `@else`

```html
@if (Model?.Count() > 0)
{
    <p>في أقسام</p>
}
else if (Model == null)
{
    <p>الـ Model فاضي</p>
}
else
{
    <p>مفيش أقسام</p>
}
```

### الـ `@foreach`

```html
@foreach (var item in Model)
{
    <tr>
        @* هنا بتوصل لـ Properties الـ item *@
        <td>@item.Code</td>
        <td>@item.Name</td>
    </tr>
}
```

### الـ `@for`

```html
@for (int i = 0; i < Model.Count(); i++)
{
    <tr class="@(i % 2 == 0 ? "even-row" : "odd-row")">
        <td>@(i + 1)</td>
        <td>@Model.ElementAt(i).Name</td>
    </tr>
}
```

### الـ Comments في Razor

```html
@* ده Comment في Razor — مش بيظهر في الـ HTML خالص *@

<!-- ده Comment HTML — بيظهر في الـ Source Code بس مش على الشاشة -->

@{
    // ده Comment C# — داخل الـ Code Block
    /* ده Comment C# متعدد الأسطر */
}
```

### الـ `@ViewData` و `@ViewBag`

```html
@* في الـ Controller: *@
@* ViewData["Title"] = "الأقسام"; *@
@* ViewBag.Message = "مرحباً"; *@

@* في الـ View: *@
<title>@ViewData["Title"]</title>
<p>@ViewBag.Message</p>

@* الفرق: *@
@* ViewData → Dictionary بيحتاج Casting لو نوع معين *@
@* ViewBag → Dynamic Property — أسهل بس أبطأ شوية *@
```

---

## 13. الـ ModelState والـ Validation

### إيه الـ ModelState؟

الـ **ModelState** هو Dictionary بيحتفظ بـ:
1. القيم اللي جت من الـ Form
2. الـ Errors لو في Validation فشل

```csharp
if (ModelState.IsValid)
// IsValid → true لو مفيش أي Errors
// false لو في Error واحدة على الأقل
```

### Data Annotations — الـ Attributes على الـ Model

```csharp
public class Department
{
    public int Id { get; set; }

    // [Required] → الـ Field ده مش ممكن يكون null أو فاضي
    [Required(ErrorMessage = "كود القسم مطلوب")]

    // [StringLength] → أقصى طول مسموح
    // MinimumLength → أقل طول مسموح
    [StringLength(10, MinimumLength = 2, ErrorMessage = "الكود بين 2 و 10 أحرف")]

    // [Display] → الاسم اللي بيتعرض في الـ Label
    [Display(Name = "كود القسم")]

    // [RegularExpression] → لازم يتطابق مع الـ Pattern ده
    [RegularExpression(@"^[A-Z]{2,5}$", ErrorMessage = "الكود أحرف كبيرة إنجليزية بس")]
    public string Code { get; set; }

    [Required(ErrorMessage = "اسم القسم مطلوب")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "الاسم بين 3 و 100 حرف")]
    [Display(Name = "اسم القسم")]
    public string Name { get; set; }

    // [Range] → لو كانت أرقام، تحديد النطاق المسموح
    // [Range(1, 100, ErrorMessage = "الرقم بين 1 و 100")]

    // [DataType] → بيحدد نوع الـ Input في الـ HTML
    [DataType(DataType.Date)]
    [Display(Name = "تاريخ الإنشاء")]
    public DateTime DateOfCreation { get; set; } = DateTime.Now;
}
```

### Server-Side vs Client-Side Validation

```
Server-Side Validation:
  المستخدم يبعت الـ Form
  ↓
  الـ Server بيستقبل البيانات
  ↓
  الـ Model Binding بيحول البيانات
  ↓
  الـ Framework بيتأكد من الـ Annotations
  ↓
  لو في Errors → بيضيفهم في ModelState
  ↓
  if (!ModelState.IsValid) return View(model)
  ↓
  المستخدم بيشوف الـ Errors

Client-Side Validation:
  بيشتغل في المتصفح قبل ما البيانات تتبعت
  بيستخدم jQuery Validation (محتاج تضيف الـ Scripts)
  أسرع للمستخدم بس مش كافي لوحده
```

### إزاي تضيف Client-Side Validation؟

```html
@* في آخر الـ View *@
@section ValidationScripts {
    @* الملف ده بيحتوي على jQuery و jQuery Validation *@
    <partial name="_ValidationScriptsPartial" />
}
```

---

## 14. الـ Async و Await ليه بنستخدمهم؟

### المشكلة بدون Async

```
Thread 1: يستقبل Request → يطلب البيانات من DB → ينتظر → ينتظر → ينتظر → يرد
                                                   ╔══════════════╗
                                                   ║ Thread محجوز ║
                                                   ║ مش شغال حاجة║
                                                   ╚══════════════╝
Thread 2: يستقبل Request → ينتظر Thread 1 يخلص → يبدأ شغله

الـ Server عنده عدد محدود من الـ Threads
لو كل Thread واقف مستني الـ Database → الـ Server هيوقع تحت الضغط
```

### الحل مع Async

```
Thread 1: يستقبل Request → يطلب البيانات من DB → يترك الـ Request → يخدم Requests تانية
                                                   ╔══════════════╗
                                                   ║ Thread شغال  ║
                                                   ║ على حاجة تاني║
                                                   ╚══════════════╝
[لما البيانات ترجع من DB]
Thread 1 (أو تاني): بياخد نتيجة الـ DB → يكمل → يرد على المستخدم
```

### في الكود

```csharp
// ❌ Synchronous — بيوقف الـ Thread
public IActionResult Index()
{
    var departments = _context.Departments.ToList(); // Thread واقف هنا
    return View(departments);
}

// ✅ Asynchronous — Thread حر
public async Task<IActionResult> Index()
{
    // await → "يا Thread، روح اشتغل حاجة تانية لحد ما ترجع البيانات"
    var departments = await _context.Departments.ToListAsync();
    return View(departments);
}
```

### قاعدة مهمة

```
لو بستخدم await → لازم الميثود تكون async
لو الميثود async → لازم ترجع Task أو Task<T>
async void → خطر! بس مسموح في Event Handlers
```

```csharp
// ✅ صح
public async Task<IActionResult> Index() { ... }
public async Task DoSomethingAsync() { ... }

// ❌ غلط — async بدون await بيدي Warning
public async Task<IActionResult> Index()
{
    var departments = _context.Departments.ToList(); // مش await!
    return View(departments);
}
```

---

## 15. تدفق البيانات الكامل في المشروع

### مثال: المستخدم يفتح صفحة القائمة

```
1. المستخدم يكتب: https://mysite.com/Department/Index
                          ↓
2. الـ Routing بيشوف: Controller = Department, Action = Index
                          ↓
3. الـ DI Container بيعمل:
   - MVCAppDbContext (Scoped)
   - DepartmentRepository (Scoped) مع inject الـ DbContext فيه
   - DepartmentController مع inject الـ Repository فيه
                          ↓
4. DepartmentController.Index() بيتنادى
                          ↓
5. await _deparmentRepository.GetAllDepartmentsAsync()
                          ↓
6. DepartmentRepository.GetAllDepartmentsAsync()
   return await _context.Departments.ToListAsync()
                          ↓
7. Entity Framework بيعمل:
   SELECT * FROM Departments
                          ↓
8. SQL Server بيرجع النتيجة
                          ↓
9. EF بيحول الـ Rows لـ List<Department>
                          ↓
10. الـ Repository بيرجع القائمة للـ Controller
                          ↓
11. return View("Index", departments)
                          ↓
12. Razor Engine بيأخد Index.cshtml ويحط فيه البيانات
                          ↓
13. بيعمل HTML كامل
                          ↓
14. HTML بيتبعت للمتصفح
                          ↓
15. المستخدم بيشوف الصفحة ✅
```

---

### مثال: المستخدم يضيف قسم جديد

```
1. المستخدم يفتح /Department/Create  [GET]
                    ↓
2. DepartmentController.Create() [HttpGet] بتنادى
                    ↓
3. return View(new Department() { })
   بيبعت Object فاضي للـ View
                    ↓
4. المستخدم بيملي الـ Form ويضغط "إضافة"
                    ↓
5. المتصفح بيبعت POST Request لـ /Department/Create
   مع البيانات:
   Code=HR&Name=Human+Resources
                    ↓
6. الـ Model Binder بيحول البيانات لـ Department Object:
   { Code = "HR", Name = "Human Resources", DateOfCreation = DateTime.Now }
                    ↓
7. DepartmentController.Create(department) [HttpPost] بتنادى
                    ↓
8. ModelState.IsValid بيتأكد من الـ Annotations
   - Code مش فاضي ✅
   - Code طوله بين 2 و 10 ✅
   - Name مش فاضي ✅
                    ↓
9. _deparmentRepository.Add(department)
                    ↓
10. _context.Departments.Add(department)
    _context.SaveChanges()
                    ↓
11. EF بيعمل:
    INSERT INTO Departments (Code, Name, DateOfCreation)
    VALUES ('HR', 'Human Resources', '2024-01-01')
                    ↓
12. return RedirectToAction(nameof(Index))
                    ↓
13. المتصفح بيروح لـ /Department/Index
                    ↓
14. المستخدم بيشوف القائمة بالقسم الجديد ✅
```

---

## 16. مشاكل في الكود الحالي وإزاي تحلها

### المشكلة الأولى — الـ Delete مش محمي

```csharp
// ❌ في الكود الحالي
public IActionResult Delete(int id)
// مفيش [HttpPost] → بيشتغل مع GET
// أي حد يعمل: <img src="/Department/Delete/5">
// أو يزور اللينك مباشرة → هيحذف البيانات!
```

**الحل الصح:**

```csharp
// ✅ الحل
[HttpGet]
public IActionResult Delete(int id)
{
    // بس هنعرض صفحة التأكيد
    var department = _deparmentRepository.GetDepartmentById(id);
    if (department is null) return NotFound();
    return View(department); // صفحة: "هل متأكد من الحذف؟"
}

[HttpPost, ActionName("Delete")] // ActionName عشان اسمهم بيتعارض
[ValidateAntiForgeryToken]        // حماية من CSRF Attack
public IActionResult DeleteConfirmed(int id)
{
    var department = _deparmentRepository.GetDepartmentById(id);
    if (department is null) return NotFound();
    _deparmentRepository.Delete(department);
    return RedirectToAction(nameof(Index));
}
```

```html
@* Views/Department/Delete.cshtml *@
@model Department
<h3>هل أنت متأكد من حذف القسم: <strong>@Model.Name</strong>؟</h3>

<form asp-action="Delete" method="post">
    @Html.AntiForgeryToken() @* ← مهم للأمان! *@
    <input asp-for="Id" hidden />
    <button type="submit" class="btn btn-danger">نعم، احذف</button>
    <a asp-action="Index" class="btn btn-secondary">لا، رجوع</a>
</form>
```

### المشكلة الثانية — الـ Validation Scripts معطلة

```html
@* في Update.cshtml، الـ Section ده معلق *@
@* @section ValidationScripts
{
    <partial name="_ValidationScriptsPartial" />
} *@

@* ده معناه إن الـ Client-Side Validation مش شغالة *@
@* المستخدم لازم يبعت الـ Form عشان يشوف الـ Errors *@
```

**الحل:** إزالة الـ Comment من الـ Section في كل الـ Views.

### المشكلة الثالثة — الـ GetAllDepartments مش Async في الـ BLL Version القديمة

```csharp
// ❌ في بعض أماكن الكود
//public IEnumerable<Department> GetAllDepartments()
//     => _context.Departments.ToList();
// ده Synchronous → بيوقف الـ Thread
```

**الحل:** استخدام الـ Async Version اللي موجودة:

```csharp
// ✅ الصح
public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    => await _context.Departments.ToListAsync();
```

---

## 17. إضافات متقدمة للمشروع

### 17.1 — الـ Generic Repository

```csharp
// ✅ بدل ما تكرر نفس الكود في كل Repository

// في mvc.BLL/Interfaces/IGenericRepository.cs
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<int> AddAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
}

// في mvc.BLL/Repositories/GenericRepository.cs
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly MVCAppDbContext _context;

    public GenericRepository(MVCAppDbContext context)
        => _context = context;

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();
        // Set<T>() → بياخد الـ DbSet اللي بيمثل الـ Table بتاع T

    public async Task<T?> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

    public async Task<int> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return await _context.SaveChangesAsync();
    }
}

// الـ DepartmentRepository بقى أبسط بكتير
public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(MVCAppDbContext context) : base(context) { }

    // بس بتضيف الميثودز الخاصة بـ Department لو في
    public async Task<IEnumerable<Department>> GetByDateAsync(DateTime date)
        => await _context.Departments
            .Where(d => d.DateOfCreation.Date == date.Date)
            .ToListAsync();
}
```

---

### 17.2 — الـ TempData للـ Notifications

```csharp
// في الـ Controller
[HttpPost]
public IActionResult Create(Department department)
{
    if (ModelState.IsValid)
    {
        _deparmentRepository.Add(department);
        TempData["SuccessMessage"] = $"تم إضافة القسم '{department.Name}' بنجاح ✅";
        return RedirectToAction(nameof(Index));
    }
    TempData["ErrorMessage"] = "يرجى تصحيح الأخطاء ❌";
    return View(department);
}
```

```html
@* في _Layout.cshtml — يظهر في كل الصفحات *@
@if (TempData["SuccessMessage"] != null)
{
    <div class="alert alert-success alert-dismissible fade show mx-3" role="alert">
        <i class="fa-solid fa-circle-check"></i>
        @TempData["SuccessMessage"]
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}

@if (TempData["ErrorMessage"] != null)
{
    <div class="alert alert-danger alert-dismissible fade show mx-3" role="alert">
        <i class="fa-solid fa-circle-xmark"></i>
        @TempData["ErrorMessage"]
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}
```

---

### 17.3 — الـ Search و Filter

```csharp
// في الـ Interface
Task<IEnumerable<Department>> SearchAsync(string keyword);

// في الـ Repository
public async Task<IEnumerable<Department>> SearchAsync(string keyword)
    => await _context.Departments
        .Where(d => d.Name.Contains(keyword) || d.Code.Contains(keyword))
        .OrderBy(d => d.Name)
        .ToListAsync();

// في الـ Controller
public async Task<IActionResult> Index(string? search)
{
    IEnumerable<Department> departments;

    if (!string.IsNullOrWhiteSpace(search))
        departments = await _deparmentRepository.SearchAsync(search);
    else
        departments = await _deparmentRepository.GetAllDepartmentsAsync();

    ViewBag.SearchTerm = search; // عشان يفضل مكتوب في الـ Search Box
    return View(departments);
}
```

```html
@* في Index.cshtml *@
<form asp-action="Index" method="get" class="d-flex gap-2 mb-3">
    <input type="text"
           name="search"
           value="@ViewBag.SearchTerm"
           class="form-control"
           placeholder="ابحث بالاسم أو الكود..." />
    <button type="submit" class="c-btn" style="--bg-clr: #5b8eda">
        <i class="fa-solid fa-search"></i> بحث
    </button>
    @if (ViewBag.SearchTerm != null)
    {
        <a asp-action="Index" class="c-btn" style="--bg-clr: #6c757d">
            <i class="fa-solid fa-xmark"></i> إلغاء
        </a>
    }
</form>
```

---

### 17.4 — الـ Pagination (ترقيم الصفحات)

```csharp
// ViewModel للـ Pagination
public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}

// في الـ Repository
public async Task<PaginatedResult<Department>> GetPagedAsync(int page = 1, int pageSize = 10)
{
    var totalCount = await _context.Departments.CountAsync();
    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

    var items = await _context.Departments
        .OrderBy(d => d.Name)
        .Skip((page - 1) * pageSize)  // تخطي الصفحات السابقة
        .Take(pageSize)                // خد بس الـ pageSize
        .ToListAsync();

    return new PaginatedResult<Department>
    {
        Items = items,
        CurrentPage = page,
        TotalPages = totalPages
    };
}

// في الـ Controller
public async Task<IActionResult> Index(int page = 1)
{
    var result = await _deparmentRepository.GetPagedAsync(page, pageSize: 10);
    return View(result);
}
```

```html
@* في الـ View *@
@model PaginatedResult<Department>

@* عرض البيانات *@
@foreach (var item in Model.Items) { ... }

@* Pagination Controls *@
<nav class="d-flex justify-content-center mt-4">
    <ul class="pagination">
        @if (Model.HasPrevious)
        {
            <li class="page-item">
                <a class="page-link" asp-action="Index" asp-route-page="@(Model.CurrentPage - 1)">
                    السابق
                </a>
            </li>
        }

        @for (int i = 1; i <= Model.TotalPages; i++)
        {
            <li class="page-item @(i == Model.CurrentPage ? "active" : "")">
                <a class="page-link" asp-action="Index" asp-route-page="@i">@i</a>
            </li>
        }

        @if (Model.HasNext)
        {
            <li class="page-item">
                <a class="page-link" asp-action="Index" asp-route-page="@(Model.CurrentPage + 1)">
                    التالي
                </a>
            </li>
        }
    </ul>
</nav>
```

---

### 17.5 — الـ Unit of Work Pattern

```csharp
// بيضمن إن كل العمليات تتعمل مع بعض أو متتعملش

// في mvc.BLL/Interfaces/IUnitOfWork.cs
public interface IUnitOfWork : IDisposable
{
    IDepartmentRepository Departments { get; }
    // لما تضيف Repositories جديدة زي Employee
    // IEmployeeRepository Employees { get; }
    Task<int> CompleteAsync();
}

// في mvc.BLL/UnitOfWork.cs
public class UnitOfWork : IUnitOfWork
{
    private readonly MVCAppDbContext _context;

    public IDepartmentRepository Departments { get; private set; }

    public UnitOfWork(MVCAppDbContext context)
    {
        _context = context;
        Departments = new DepartmentRepository(_context);
    }

    // بدل ما كل Repository تعمل SaveChanges
    // بنعملها هنا مرة واحدة
    public async Task<int> CompleteAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}

// في Program.cs
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// في الـ Controller
public class DepartmentController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentController(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    [HttpPost]
    public async Task<IActionResult> Create(Department department)
    {
        if (ModelState.IsValid)
        {
            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.CompleteAsync(); // ← SaveChanges هنا بس
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }
}
```

---

## 📝 ملخص سريع لأهم المفاهيم

| المفهوم | تعريف بسيط |
|---|---|
| **MVC** | بيقسم التطبيق لـ Model (البيانات) + View (الشاشة) + Controller (المدير) |
| **N-Tier** | بيقسم المشروع لـ طبقات: DAL → BLL → PL |
| **DAL** | بتتكلم مع الـ Database فقط (DbContext + Entities) |
| **BLL** | بتنظم العمليات والـ Business Rules (Interfaces + Repositories) |
| **PL** | الـ UI (Controllers + Views) |
| **Interface** | عقد بيحدد "إيه" هيتعمل من غير "إزاي" |
| **Repository** | بيعزل عمليات الـ Database |
| **DI** | الـ Framework بيعمل الـ Objects وبيبعتها تلقائياً |
| **Singleton** | Object واحد طول عمر التطبيق |
| **Scoped** | Object واحد لكل Request |
| **Transient** | Object جديد كل مرة يتطلب |
| **Tag Helpers** | `asp-for`, `asp-action`, `asp-controller`, `asp-route-id` |
| **Razor** | HTML + C# عشا�