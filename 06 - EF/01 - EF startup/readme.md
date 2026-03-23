# 🗄️ Entity Framework

<div align="center">

![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6+-blueviolet?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red?style=for-the-badge&logo=microsoftsqlserver)
![Status](https://img.shields.io/badge/Study%20Guide-Complete-success?style=for-the-badge)

**دليل دراسي متكامل لـ Entity Framework بالعربي المصري**

*مناسب للمذاكرة، الـ Portfolio، وتحضير الـ Interviews*

</div>

---

## 📋 فهرس المحتويات

| القسم | الموضوع |
|-------|---------|
| [القسم الأول](#-section-1--what-is-entity-framework) | ما هو Entity Framework؟ |
| [القسم الثاني](#-section-2--ef-architecture) | معمارية EF |
| [القسم الثالث](#-section-3--ef-approaches) | طرق العمل مع EF |
| [القسم الرابع](#-section-4--migrations) | الـ Migrations |
| [القسم الخامس](#-section-5--ef-vs-adonet) | EF مقابل ADO.NET |
| [القسم السادس](#-section-6--inheritance-mapping-strategies) | استراتيجيات الـ Inheritance Mapping |
| [القسم السابع](#-section-7--ef-providers) | الـ Providers |
| [القسم الثامن](#-section-8--crud-operations) | عمليات CRUD |
| [القسم التاسع](#-section-9--tracking-vs-no-tracking) | Tracking vs No Tracking |
| [القسم العاشر](#-section-10--full-working-examples) | أمثلة كاملة |
| [القسم الحادي عشر](#-section-11--loading-strategies) | استراتيجيات التحميل |
| [القسم الثاني عشر](#-section-12--model-configuration) | تهيئة الـ Model |

---

## 🧠 Section 1 – What is Entity Framework

### ما هو الـ ORM؟

الـ **ORM (Object-Relational Mapping)** هو تقنية بتحول البيانات الموجودة في جداول قاعدة البيانات لـ Objects في الكود والعكس بالعكس.

> 💡 **بالبساطة:** بدل ما تكتب SQL يدوياً عشان تجيب بيانات، الـ ORM بيعمل ده أوتوماتيك وبيديك الكلاسات جاهزة.

### ليه الـ ORM موجود؟

قديم الناس كانت بتكتب SQL يدوي زي كده:

```sql
SELECT * FROM Employees WHERE Id = 1
```

وبعدين تاخد النتيجة وتحطها في كلاس يدوي. ده كان بطيء ومتعب وفيه غلط كتير. الـ ORM جه يحل المشكلة دي.

### كيف EF بيعمل الـ Mapping؟

```
┌─────────────────────────────────────────────────────────────────┐
│                    DATABASE LAYER                               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────┐   │
│  │  Employees   │  │   Orders     │  │      Products        │   │
│  │──────────────│  │──────────────│  │──────────────────────│   │
│  │ Id (PK)      │  │ Id (PK)      │  │ Id (PK)              │   │
│  │ Name         │  │ CustomerId   │  │ Name                 │   │
│  │ Age          │  │ Total        │  │ Price                │   │
│  │ Address      │  │ Date         │  │ Stock                │   │
│  └──────────────┘  └──────────────┘  └──────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                  ┌───────────────────────┐
                  │    ENTITY FRAMEWORK   │
                  │  (الطبقة الوسيطة)    │
                  └───────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      C# CLASSES LAYER                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────┐   │
│  │  Employee    │  │    Order     │  │       Product        │   │
│  │──────────────│  │──────────────│  │──────────────────────│   │
│  │ int Id       │  │ int Id       │  │ int Id               │   │
│  │ string Name  │  │ int CustomId │  │ string Name          │   │
│  │ int Age      │  │ decimal Tot  │  │ decimal Price        │   │
│  │ string Addr  │  │ DateTime Dt  │  │ int Stock            │   │
│  └──────────────┘  └──────────────┘  └──────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

### القواعد الأساسية للـ Mapping:

| قاعدة البيانات | C# |
|---|---|
| جدول (Table) | كلاس (Class) |
| عمود (Column) | خاصية (Property) |
| صف (Row) | Object |
| علاقة (Relationship) | Navigation Property |
| Primary Key | `[Key]` أو `Id` |

---

## 🧩 Section 2 – EF Architecture

### مكونات EF الأساسية

```
┌─────────────────────────────────────────┐
│            YOUR APPLICATION             │
│  ┌───────────────────────────────────┐  │
│  │     Business Logic / Controllers  │  │
│  └───────────────────────────────────┘  │
└────────────────────┬────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────┐
│              DbContext                  │
│  ┌─────────────────────────────────┐    │
│  │  DbSet<Employee> Employees      │    │
│  │  DbSet<Order> Orders            │    │
│  │  DbSet<Product> Products        │    │
│  └─────────────────────────────────┘    │
│  ┌─────────────────────────────────┐    │
│  │      Change Tracker             │    │
│  │  (بيتابع كل تغيير في الـ Data) │   │
│  └─────────────────────────────────┘    │
└────────────────────┬────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────┐
│           DATABASE PROVIDER             │
│  (SQL Server / MySQL / PostgreSQL)      │
└────────────────────┬────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────┐
│              DATABASE                   │
└─────────────────────────────────────────┘
```

### DbContext

الـ **DbContext** ده هو قلب Entity Framework. هو اللي بيتعامل مع قاعدة البيانات ومسؤول عن:
- فتح وقفل الـ Connection
- تتبع التغييرات (Change Tracking)
- تنفيذ الـ Queries
- حفظ البيانات

```csharp
public class AppDbContext : DbContext
{
    // كل DbSet بيمثل جدول في قاعدة البيانات
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("your_connection_string");
    }
}
```

### DbSet

الـ **DbSet** بيمثل جدول كامل في قاعدة البيانات. من خلاله بتعمل كل عمليات الـ CRUD.

```csharp
// DbSet<Employee> = جدول الـ Employees
context.Employees.Add(newEmployee);        // Insert
context.Employees.Find(1);                // Select
context.Employees.Remove(employee);       // Delete
```

### Change Tracking

الـ **Change Tracker** هو نظام EF اللي بيتابع كل تغيير بيحصل على الـ Entities.

```
┌────────────────────────────────────────────────────────────────┐
│                    ENTITY STATES                               │
│                                                                │
│  ┌──────────┐   Add()    ┌──────────┐                          │
│  │ Detached │──────────▶ │  Added   │                         │
│  └──────────┘            └──────────┘                          │
│       │                       │                                │
│   Attach()               SaveChanges()                         │
│       │                       │                                │
│       ▼                       ▼                                │
│  ┌──────────┐            ┌──────────┐                          │
│  │Unchanged │◀───────────│ Database │                         │
│  └──────────┘            └──────────┘                          │
│       │                                                        │
│   Modify                                                       │
│   Property                                                     │
│       │                                                        │
│       ▼                                                        │
│  ┌──────────┐  SaveChanges()  ┌──────────┐                     │
│  │ Modified │───────────────▶ │Unchanged │                    │
│  └──────────┘                 └──────────┘                     │
│                                                                │
│  ┌──────────┐  SaveChanges()                                   │
│  │ Deleted  │───────────────▶  Removed from DB                │
│  └──────────┘                                                  │
└────────────────────────────────────────────────────────────────┘
```

### Unit of Work Pattern

الـ **Unit of Work** هو Design Pattern بيضمن إن كل التغييرات تتحفظ مرة واحدة أو ماتتحفظش خالص (All or Nothing). EF بيطبق الـ Pattern ده بشكل تلقائي من خلال `SaveChanges()`.

```csharp
// Unit of Work في EF - كل التغييرات دي هتتنفذ مرة واحدة
using (var context = new AppDbContext())
{
    // ➕ إضافة موظف جديد
    context.Employees.Add(new Employee { Name = "Ahmed", Age = 25 });
    
    // ✏️ تعديل موظف موجود
    var emp = context.Employees.Find(1);
    emp.Name = "Mohamed";
    
    // 🗑️ حذف موظف
    var old = context.Employees.Find(5);
    context.Employees.Remove(old);
    
    // ✅ تنفيذ كل التغييرات مرة واحدة - Unit of Work
    context.SaveChanges();
}
```

---

## 🧪 Section 3 – EF Approaches

EF عنده 3 طرق للتعامل مع قاعدة البيانات:

```
┌───────────────────────────────────────────────────────────────┐
│                  3 APPROACHES IN EF                           │
│                                                               │
│  ┌─────────────┐    ┌─────────────────┐    ┌──────────────┐   │
│  │ Code First  │    │ Database First  │    │ Model First  │   │
│  │             │    │                 │    │   (نادر)     │   │
│  │ Code ───▶  │    │   DB ────────▶  │    │  EDMX ─────▶ │  │
│  │    DB       │    │   Classes       │    │   Code + DB  │   │
│  └─────────────┘    └─────────────────┘    └──────────────┘   │
└───────────────────────────────────────────────────────────────┘
```

### 🔵 Code First

في الـ **Code First** بتبدأ بكتابة الـ Classes في الكود، وبعدين EF بيولد قاعدة البيانات منها أوتوماتيك.

**الخطوات:**

```
1. تكتب الكلاسات في C#
         │
         ▼
2. تعمل Migration
   (add-migration InitialCreate)
         │
         ▼
3. EF بيولد ملف Migration فيه SQL
         │
         ▼
4. تطبق الـ Migration على DB
   (update-database)
         │
         ▼
5. قاعدة البيانات اتعملت! 🎉
```

**مثال:**

```csharp
// 1. بتكتب الكلاس
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}

// 2. بتعمل migration
// PM> add-migration CreateEmployeeTable

// 3. بتطبق على DB
// PM> update-database
```

**متى تستخدمه؟**
- لما بتبدأ مشروع جديد من الصفر
- لما الـ Developers هم اللي بيتحكموا في هيكل قاعدة البيانات
- لما عايز تستخدم الـ Migrations لإدارة التغييرات

### 🟢 Database First

في الـ **Database First** بتبدأ من قاعدة البيانات الموجودة وبتولد الـ Classes أوتوماتيك.

**الخطوات:**

```
1. عندك DB موجودة
         │
         ▼
2. بتشغل أمر Scaffold
   (Scaffold-DbContext)
         │
         ▼
3. EF بيقرأ هيكل الـ DB
         │
         ▼
4. بيولد الـ Classes
         │
         ▼
5. Classes جاهزة للاستخدام! 🎉
```

**مثال الأمر:**

```bash
# Scaffold-DbContext أمر الـ Database First
Scaffold-DbContext "Server=.;Database=MyDb;Trusted_Connection=True;" 
  Microsoft.EntityFrameworkCore.SqlServer
  -OutputDir Models
```

**متى تستخدمه؟**
- لما عندك DB موجودة من قبل (Legacy Database)
- لما فريق الـ DBAs هم اللي بيتحكموا في قاعدة البيانات
- لما الـ DB كبيرة ومش منطقي تكتب الكلاسات يدوي

### 🟡 Model First (نادر الاستخدام)

الـ **Model First** هو النهج الثالث وده نادر جداً في الاستخدام الحديث. بيعتمد على الـ **EDMX Designer** في Visual Studio.

**كيف يشتغل؟**

```
┌─────────────────────────────────────────────────────────────┐
│                    Visual Studio                            │
│  ┌─────────────────────────────────────────────────────┐    │
│  │              EDMX Designer Surface                  │    │
│  │                                                     │    │
│  │   ┌──────────┐      ┌──────────┐                    │    │
│  │   │ Employee │──────│  Order   │                    │    │
│  │   │          │      │          │                    │    │
│  │   └──────────┘      └──────────┘                    │    │
│  │                                                     │    │
│  │        بتصمم الـ Model بالـ Drag & Drop            │    │
│  └─────────────────────────────────────────────────────┘    │
└──────────────────────────┬──────────────────────────────────┘
                           │
                 ┌─────────┴─────────┐
                 │                   │
                 ▼                   ▼
         ┌──────────────┐   ┌──────────────────┐
         │   C# Code    │   │   Database DDL   │
         │  (Generated) │   │   (Generated)    │
         └──────────────┘   └──────────────────┘
```

**ملفات الـ EDMX:**
- **`.edmx`** - ملف XML بيحتوي على الـ Model كاملاً
- **`.tt` files** - T4 Templates بتولد الكود
- **`.cs` files** - الكلاسات المولدة أوتوماتيك

**ليه ده بيتستخدم نادر؟**
- الـ EDMX ملفات XML ضخمة وصعبة الـ Merge في الـ Git
- EF Core مش بيدعمه (بس EF 6 بيدعمه)
- Code First أسهل وأكثر مرونة
- الـ Tooling قديم ومش بيتطور

### جدول المقارنة

| الجانب | Code First | Database First | Model First |
|--------|-----------|----------------|-------------|
| **نقطة البداية** | C# Code | قاعدة بيانات موجودة | EDMX Designer |
| **الاتجاه** | Code → DB | DB → Code | Model → Code + DB |
| **الاستخدام** | شائع جداً | شائع | نادر |
| **Migrations** | ✅ متاح | ❌ مش متاح | محدود |
| **EF Core دعم** | ✅ | ✅ | ❌ |
| **مناسب لـ** | مشاريع جديدة | قواعد بيانات قديمة | مشاريع قديمة |

---

## 🧱 Section 4 – Migrations

### ما هي الـ Migrations؟

الـ **Migrations** هي نظام EF لإدارة التغييرات على هيكل قاعدة البيانات بطريقة منظمة وقابلة للـ Version Control.

```
┌─────────────────────────────────────────────────────────────┐
│                   MIGRATION FLOW                            │
│                                                             │
│  ┌──────────────┐                                           │
│  │  عدّلت كلاس │  (زدت عمود جديد مثلاً)                       │
│  └──────┬───────┘                                           │
│         │                                                   │
│         ▼                                                   │
│  ┌──────────────────────────────────┐                       │
│  │  add-migration AddEmailToEmployee│                       │
│  └──────┬───────────────────────────┘                       │
│         │                                                   │
│         ▼                                                   │
│  ┌──────────────────────────────────────────────────────┐   │
│  │        Migration File Created                        │   │
│  │  20240101_AddEmailToEmployee.cs                      │   │
│  │  ┌────────────────┐  ┌──────────────────────────┐    │   │
│  │  │   Up() Method  │  │     Down() Method        │    │   │
│  │  │   (Apply)      │  │     (Rollback)           │    │   │
│  │  │                │  │                          │    │   │
│  │  │ ADD COLUMN     │  │ DROP COLUMN              │    │   │
│  │  │ Email nvarchar │  │ Email                    │    │   │
│  │  └────────────────┘  └──────────────────────────┘    │   │
│  └──────────────────────────────────────────────────────┘   │
│         │                                                   │
│         ▼                                                   │
│  ┌──────────────────────────────────┐                       │
│  │       update-database            │                       │
│  └──────┬───────────────────────────┘                       │
│         │                                                   │
│         ▼                                                   │
│  ✅ Database Updated!                                       │
└─────────────────────────────────────────────────────────────┘
```

### ملف الـ Migration من جوا

```csharp
// ملف Migration تلقائي من EF
public partial class AddEmailToEmployee : Migration
{
    // Up() = تطبيق التغيير على قاعدة البيانات
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "Employees",
            type: "nvarchar(max)",
            nullable: true);
    }

    // Down() = التراجع عن التغيير (Rollback)
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Email",
            table: "Employees");
    }
}
```

### جدول الـ Migration History

EF بيعمل جدول اسمه `__EFMigrationsHistory` في قاعدة البيانات بيسجل فيه كل الـ Migrations اللي اتطبقت:

```
┌─────────────────────────────────────────────────────────────┐
│                  __EFMigrationsHistory                      │
├──────────────────────────────────────────┬──────────────────┤
│ MigrationId                              │ ProductVersion   │
├──────────────────────────────────────────┼──────────────────┤
│ 20240101_InitialCreate                   │ 8.0.0            │
│ 20240115_AddEmailToEmployee              │ 8.0.0            │
│ 20240201_AddOrdersTable                  │ 8.0.0            │
│ 20240210_AddIndexOnName                  │ 8.0.0            │
└──────────────────────────────────────────┴──────────────────┘
```

### أوامر الـ Migrations المهمة

```bash
# إنشاء Migration جديدة
add-migration MigrationName

# تطبيق كل الـ Migrations على DB
update-database

# الرجوع لـ Migration معينة
update-database TargetMigration

# عرض كل الـ Migrations
get-migrations

# حذف آخر Migration (لو لسه ما اتطبقتش)
remove-migration
```

### Seeding البيانات في الـ Migration

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    // إضافة بيانات ابتدائية (Seed Data)
    migrationBuilder.InsertData(
        table: "Roles",
        columns: new[] { "Id", "Name" },
        values: new object[]
        {
            new object[] { 1, "Admin" },
            new object[] { 2, "User" },
            new object[] { 3, "Manager" }
        });
}
```

---

## 🧾 Section 5 – EF vs ADO.NET

### ما هو الـ ADO.NET؟

الـ **ADO.NET (ActiveX Data Objects)** كان هو الطريقة القديمة للتعامل مع قواعد البيانات في .NET قبل ما EF ييجي. كان بيعتمد على **Stored Procedures** والكتابة اليدوية للـ SQL.

### كيف كان الكود بالـ ADO.NET؟

```csharp
// ❌ الطريقة القديمة - ADO.NET
public Employee GetEmployee(int id)
{
    Employee emp = null;
    
    string connectionString = "Server=.;Database=MyDb;...";
    string query = "SELECT * FROM Employees WHERE Id = @Id";
    
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Id", id);
            
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    emp = new Employee
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Age = (int)reader["Age"]
                    };
                }
            }
        }
    }
    return emp;
}
```

### نفس الحاجة بالـ EF

```csharp
// ✅ الطريقة الجديدة - Entity Framework
public Employee GetEmployee(int id)
{
    return context.Employees.Find(id);
}
```

### مقارنة EF مع ADO.NET

| الجانب | ADO.NET | Entity Framework |
|--------|---------|-----------------|
| **كمية الكود** | كتير جداً | بسيط وقليل |
| **SQL يدوي** | ✅ مطلوب | ❌ مش مطلوب |
| **Type Safety** | ❌ ضعيف | ✅ قوي |
| **Migrations** | ❌ مفيش | ✅ متاحة |
| **Change Tracking** | ❌ مفيش | ✅ أوتوماتيك |
| **LINQ Support** | ❌ | ✅ |
| **الـ Performance** | ⚡ أسرع (Raw SQL) | 🐢 أبطأ شوية |
| **Stored Procedures** | ✅ الأساسي | ✅ ممكن لو محتاج |
| **الصيانة** | صعبة | سهلة |
| **قابلية قراءة الكود** | منخفضة | عالية |

---

## 🧬 Section 6 – Inheritance Mapping Strategies

لما بيكون عندك Inheritance في الكلاسات، EF عنده 3 طرق يمثلها في قاعدة البيانات.

### الكلاسات الأساسية

```csharp
// الكلاس الأساسي
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
}

// كلاس الموظف الدائم
public class FullTimeEmployee : Employee
{
    public DateTime StartDate { get; set; }
    public decimal Salary { get; set; }
}

// كلاس الموظف بالساعة
public class PartTimeEmployee : Employee
{
    public decimal HourRate { get; set; }
    public int CountOfHours { get; set; }
}
```

---

### 🔵 TPT – Table Per Type

#### ما هو TPT؟

في الـ **TPT (Table Per Type)**، كل كلاس له جدول منفصل في قاعدة البيانات. الجدول الأساسي بيحتوي على الـ Properties المشتركة، والجداول الفرعية بتحتوي على الـ Properties الخاصة بيها مع Foreign Key للجدول الأساسي.

#### قبل (C# Classes):

```
┌─────────────────────────────────┐
│          Employee               │
│  - Id                           │
│  - Name                         │
│  - Age                          │
│  - Address                      │
│  - Gender                       │
└─────────────────────────────────┘
         ↑              ↑
         │              │
┌─────────────────┐ ┌────────────────────┐
│ FullTimeEmployee│ │ PartTimeEmployee   │
│ - StartDate     │ │ - HourRate         │
│ - Salary        │ │ - CountOfHours     │
└─────────────────┘ └────────────────────┘
```

#### بعد (Database Tables):

```
┌─────────────────────────────────────────────────┐
│                  Employees                      │
├────┬──────────┬─────┬───────────┬───────────────┤
│ Id │  Name    │ Age │  Address  │    Gender     │
├────┼──────────┼─────┼───────────┼───────────────┤
│  1 │  Ahmed   │ 30  │  Cairo    │    Male       │
│  2 │  Sara    │ 25  │  Alex     │    Female     │
│  3 │  Mohamed │ 28  │  Giza     │    Male       │
└────┴──────────┴─────┴───────────┴───────────────┘
        ▲ PK               ▲ FK                ▲ FK
        │                  │                   │
┌───────────────────────┐  │   ┌───────────────────────────┐
│   FullTimeEmployees   │  │   │    PartTimeEmployees      │
├───────┬───────────────┤  │   ├───────┬────────┬──────────┤
│ EmpId │  StartDate    │  │   │ EmpId │HourRate│CountOfHrs│
├───────┼───────────────┤  │   ├───────┼────────┼──────────┤
│   1   │  2020-01-01   │  │   │   2   │  50.00 │    8     │
│   3   │  2019-06-15   │  │   └───────┴────────┴──────────┘
└───────┴───────────────┘
     └──────────────────────┘
          Foreign Key → Employees.Id
```

#### كيف تستخدمه في EF:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // TPT Configuration
    modelBuilder.Entity<Employee>().ToTable("Employees");
    modelBuilder.Entity<FullTimeEmployee>().ToTable("FullTimeEmployees");
    modelBuilder.Entity<PartTimeEmployee>().ToTable("PartTimeEmployees");
}
```

#### المميزات والعيوب:

| المميزات ✅ | العيوب ❌ |
|------------|----------|
| بيانات منظمة بدون Nulls | بطيء بسبب الـ JOINs |
| Normalization كاملة | كل عملية CRUD بتمس جدولين |
| سهل الفهم ويعكس الـ OOP | الـ Queries معقدة |

---

### 🟢 TPH – Table Per Hierarchy

#### ما هو TPH؟

في الـ **TPH (Table Per Hierarchy)**، كل الكلاسات (الأساسية والفرعية) بتتحط في **جدول واحد بس**. EF بيضيف عمود اسمه **Discriminator** عشان يعرف كل صف بيخص أنهي كلاس.

#### بعد (Database Table):

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                                  Employees                                      │
├────┬──────────┬─────┬──────────┬────────┬───────────┬────────┬────────┬─────────┤
│ Id │  Name    │ Age │ Address  │Gender  │ StartDate │ Salary │HourRate│CountHrs │ Discriminator│
├────┼──────────┼─────┼──────────┼────────┼───────────┼────────┼────────┼─────────┤───────────────
│  1 │ Ahmed    │ 30  │ Cairo    │ Male   │2020-01-01 │5000.00 │  NULL  │  NULL   │ FullTime     │
│  2 │ Sara     │ 25  │ Alex     │ Female │   NULL    │  NULL  │ 50.00  │   8     │ PartTime     │
│  3 │ Mohamed  │ 28  │ Giza     │ Male   │2019-06-15 │4500.00 │  NULL  │  NULL   │ FullTime     │
└────┴──────────┴─────┴──────────┴────────┴───────────┴────────┴────────┴─────────┘
                                                                           ↑
                                                              EF بيستخدمه عشان يعرف نوع الموظف
```

#### مشكلة الـ NULLs:

```
الموظف Full-Time:
  ✅ StartDate = 2020-01-01
  ✅ Salary = 5000
  ❌ HourRate = NULL   ← مش محتاجها
  ❌ CountOfHours = NULL ← مش محتاجها

الموظف Part-Time:
  ❌ StartDate = NULL   ← مش محتاجها
  ❌ Salary = NULL      ← مش محتاجها
  ✅ HourRate = 50
  ✅ CountOfHours = 8
```

#### كيف تستخدمه في EF (ده الـ Default في EF Core):

```csharp
// TPH هو الـ Default في EF Core
// مش محتاج تعمل configuration خاص
public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    // EF هيعمل جدول واحد تلقائياً
}

// لو عايز تخصص اسم الـ Discriminator:
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Employee>()
        .HasDiscriminator<string>("EmployeeType")
        .HasValue<FullTimeEmployee>("FullTime")
        .HasValue<PartTimeEmployee>("PartTime");
}
```

#### المميزات والعيوب:

| المميزات ✅ | العيوب ❌ |
|------------|----------|
| Queries سريعة (جدول واحد) | كتير من الـ NULLs |
| لا يحتاج JOINs | بيانات غير Normalized |
| بسيط في الهيكل | الجدول بيكبر كتير |
| EF Core Default | صعب فرض الـ NOT NULL constraints |

---

### 🟠 TPC – Table Per Concrete Class

#### ما هو TPC؟

في الـ **TPC (Table Per Concrete Class)**، كل كلاس **Concrete** (مش Abstract) بيكون له جدوله الخاص اللي فيه كل الأعمدة، حتى الأعمدة المورثة من الكلاس الأساسي.

#### قبل (C# Classes):

```
┌─────────────────────────────────┐
│     Employee (Base Class)       │
│  - Id                           │
│  - Name                         │
│  - Age                          │
│  - Address                      │
│  - Gender                       │
└─────────────────────────────────┘
         ↑              ↑
         │              │
┌─────────────────┐ ┌────────────────────┐
│ FullTimeEmployee│ │ PartTimeEmployee   │
│ (Concrete)      │ │ (Concrete)         │
│ - StartDate     │ │ - HourRate         │
│ - Salary        │ │ - CountOfHours     │
└─────────────────┘ └────────────────────┘
```

#### بعد (Database Tables):

```
┌─────────────────────────────────────────────────────────────┐
│              FullTimeEmployeesTable                         │
├────┬──────────┬─────┬───────────┬────────┬──────────────────┤
│ Id │  Name    │ Age │  Address  │ Gender │ StartDate│ Salary│
├────┼──────────┼─────┼───────────┼────────┼──────────┼───────┤
│  1 │  Ahmed   │ 30  │  Cairo    │  Male  │2020-01-01│5000.00│
│  2 │  Mohamed │ 28  │  Giza     │  Male  │2019-06-15│4500.00│
└────┴──────────┴─────┴───────────┴────────┴──────────┴───────┘
              ↑ الأعمدة من Employee مكررة هنا

┌───────────────────────────────────────────────────────────────┐
│                 PartTimeEmployeesTable                        │
├────┬──────────┬─────┬───────────┬────────┬──────────┬─────────┤
│ Id │  Name    │ Age │  Address  │ Gender │ HourRate │CountHrs │
├────┼──────────┼─────┼───────────┼────────┼──────────┼─────────┤
│  3 │  Sara    │ 25  │  Alex     │Female  │  50.00   │   8     │
└────┴──────────┴─────┴───────────┴────────┴──────────┴─────────┘
              ↑ الأعمدة من Employee مكررة هنا أيضاً
```

> ⚠️ **مهم:** التكرار ده في **أسماء الأعمدة** مش في **البيانات**! البيانات نفسها مش متكررة.

#### كيف تستخدمه في EF Core:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // TPC Configuration في EF Core 7+
    modelBuilder.Entity<Employee>().UseTpcMappingStrategy();
    
    modelBuilder.Entity<FullTimeEmployee>().ToTable("FullTimeEmployees");
    modelBuilder.Entity<PartTimeEmployee>().ToTable("PartTimeEmployees");
}
```

#### المميزات والعيوب:

| المميزات ✅ | العيوب ❌ |
|------------|----------|
| لا JOINs مطلوبة | تكرار في أسماء الأعمدة |
| Queries سريعة | صعوبة في polymorphic queries |
| No NULLs | زيادة في حجم الـ Schema |
| كل جدول مكتفي بنفسه | - |

### ملخص المقارنة بين الثلاث استراتيجيات

```
┌───────────────────────────────────────────────────────────────────────┐
│                    STRATEGY COMPARISON                                │
│                                                                       │
│  TPT (Table Per Type)                                                 │
│  ┌──────────────┐     ┌─────────────────┐     ┌─────────────────┐     │
│  │  Employees   │◄────│  FullTimeEmps   │  ┌──│  PartTimeEmps   │     │
│  │  (Base)      │  FK │  (+ StartDate   │  │  │  (+ HourRate    │     │
│  └──────────────┘     │  + Salary)      │  │  │  + CountOfHrs)  │     │
│                       └─────────────────┘  │  └─────────────────┘     │
│  JOIN required for every query!            │                          │
│                                            │                          │
│  TPH (Table Per Hierarchy)                 │                          │
│  ┌─────────────────────────────────────────────────────────────────┐  │
│  │  Employees (ALL columns + Discriminator)                        │  │
│  │  Id│Name│Age│Address│Gender│StartDate│Salary│HourRate│CountHrs  │  │
│  └─────────────────────────────────────────────────────────────────┘  │
│  One table, many NULLs!                                               │
│                                                                       │
│  TPC (Table Per Concrete Class)                                       │
│  ┌──────────────────────────────────┐                                 │
│  │  FullTimeEmployees               │                                 │
│  │  Id│Name│Age│Address│Gender│...  │                                 │
│  └──────────────────────────────────┘                                 │
│  ┌──────────────────────────────────┐                                 │
│  │  PartTimeEmployees               │                                 │
│  │  Id│Name│Age│Address│Gender│...  │                                 │
│  └──────────────────────────────────┘                                 │
│  Separate tables, no JOINs, no NULLs!                                 │
└───────────────────────────────────────────────────────────────────────┘
```

| الجانب | TPT | TPH | TPC |
|--------|-----|-----|-----|
| **عدد الجداول** | قاعدة + فرعية | جدول واحد | جدول لكل Concrete |
| **الـ JOINs** | ✅ مطلوبة | ❌ مش مطلوبة | ❌ مش مطلوبة |
| **الـ NULLs** | ❌ مفيش | ✅ كتير | ❌ مفيش |
| **الـ Performance** | بطيء | سريع | سريع |
| **Normalization** | عالية | منخفضة | متوسطة |
| **الاستخدام** | شائع | الـ Default | أقل شيوعاً |

---

## 🧠 Section 7 – EF Providers

EF Core بيشتغل مع أنواع مختلفة من قواعد البيانات من خلال الـ **Providers**. الكود بتاعك بيفضل نفسه وبس الـ Provider بيتغير.

```
┌─────────────────────────────────────────────────────────────────┐
│                    YOUR EF CORE APP                             │
│               (نفس الكود LINQ واحد)                            | 
└──────────────────────────┬──────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
│                      EF CORE                                    │
│                  (Query Translation)                            │
└──────┬─────────────┬──────────────┬──────────────┬──────────────┘
       │             │              │              │
       ▼             ▼              ▼              ▼
┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐
│SQL Server│  │  MySQL   │  │PostgreSQL│  │  Oracle  │
│ Provider │  │ Provider │  │ Provider │  │ Provider │
└──────────┘  └──────────┘  └──────────┘  └──────────┘
       │             │              │              │
       ▼             ▼              ▼              ▼
┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐
│SQL Server│  │  MySQL   │  │PostgreSQL│  │  Oracle  │
│    DB    │  │    DB    │  │    DB    │  │    DB    │
└──────────┘  └──────────┘  └──────────┘  └──────────┘
```

### تثبيت الـ Providers

```bash
# SQL Server
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# MySQL
dotnet add package Pomelo.EntityFrameworkCore.MySql

# PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# SQLite (للـ Development)
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

# In-Memory (للـ Testing)
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

### Configuration لكل Provider

```csharp
// SQL Server
options.UseSqlServer("Server=.;Database=MyDb;Trusted_Connection=True;");

// MySQL
options.UseMySql("Server=localhost;Database=MyDb;User=root;Password=123;",
    ServerVersion.AutoDetect(connectionString));

// PostgreSQL
options.UseNpgsql("Host=localhost;Database=MyDb;Username=postgres;Password=123;");

// SQLite
options.UseSqlite("Data Source=mydb.db");
```

---

## 🧪 Section 8 – CRUD Operations

### Create (إضافة بيانات)

```csharp
using (var context = new AppDbContext())
{
    // طريقة 1: Add()
    var employee = new Employee
    {
        Name = "Ahmed Ali",
        Age = 28,
        Address = "Cairo",
        Gender = "Male"
    };
    context.Employees.Add(employee);
    context.SaveChanges();
    
    // طريقة 2: AddRange() - إضافة كتير مرة واحدة
    var employees = new List<Employee>
    {
        new Employee { Name = "Sara", Age = 25 },
        new Employee { Name = "Mohamed", Age = 30 }
    };
    context.Employees.AddRange(employees);
    context.SaveChanges();
}
```

### Read (قراءة بيانات)

```csharp
using (var context = new AppDbContext())
{
    // جيب كل الموظفين
    var allEmployees = context.Employees.ToList();
    
    // جيب موظف بالـ Id
    var emp = context.Employees.Find(1);
    
    // جيب موظفين بشرط (LINQ)
    var cairoEmployees = context.Employees
        .Where(e => e.Address == "Cairo")
        .ToList();
    
    // جيب أول موظف
    var first = context.Employees.FirstOrDefault();
    
    // Select معين
    var names = context.Employees
        .Select(e => new { e.Name, e.Age })
        .ToList();
        
    // Order وـ Skip وـ Take (Pagination)
    var page = context.Employees
        .OrderBy(e => e.Name)
        .Skip(10)
        .Take(5)
        .ToList();
}
```

### Update (تعديل بيانات)

```csharp
using (var context = new AppDbContext())
{
    // طريقة 1: Tracked Update
    var emp = context.Employees.Find(1);
    emp.Name = "Ahmed Mohamed"; // EF بيتتبع التغيير تلقائي
    context.SaveChanges(); // EF بيعمل UPDATE فقط للعمود اللي اتغير
    
    // طريقة 2: Update كامل
    var updatedEmp = new Employee
    {
        Id = 1,
        Name = "Ahmed New",
        Age = 29,
        Address = "Cairo"
    };
    context.Employees.Update(updatedEmp); // بيعمل UPDATE لكل الأعمدة
    context.SaveChanges();
}
```

### Delete (حذف بيانات)

```csharp
using (var context = new AppDbContext())
{
    // طريقة 1: Remove()
    var emp = context.Employees.Find(1);
    context.Employees.Remove(emp);
    context.SaveChanges();
    
    // طريقة 2: RemoveRange()
    var oldEmps = context.Employees
        .Where(e => e.Age > 60)
        .ToList();
    context.Employees.RemoveRange(oldEmps);
    context.SaveChanges();
    
    // طريقة 3: Bulk Delete (EF Core 7+)
    context.Employees
        .Where(e => e.Age > 60)
        .ExecuteDelete(); // بدون تحميل البيانات في الـ Memory
}
```

---

## 🧾 Section 9 – Tracking vs No Tracking

### Tracking (الـ Default)

لما بتجيب بيانات من EF بالطريقة العادية، EF بيحطها في الـ **Change Tracker** ويتابعها. ده مفيد لو هتعمل Update عليها.

```csharp
// Tracked Query
var employees = context.Employees.ToList();
// EF بيحتفظ بـ Snapshot من البيانات في الـ Memory
// لو عملت تعديل وبعدين SaveChanges() هيشوف الفرق ويعمل UPDATE
```

### AsNoTracking (بدون تتبع)

لو بتجيب بيانات للعرض بس ومش هتعدل عليها، استخدم `AsNoTracking()`. ده أسرع وبياخد ذاكرة أقل.

```csharp
// No Tracking Query - أسرع للـ Read-Only
var employees = context.Employees
    .AsNoTracking()
    .ToList();
// EF مبيحتفظش بأي Snapshot في الـ Memory
```

### متى تستخدم كل واحدة؟

```
استخدم Tracking لما:           استخدم AsNoTracking لما:
┌─────────────────────────┐     ┌───────────────────────────────┐
│ ✅ هتعمل Update        │     │ ✅ بتعرض بيانات بس          │
│ ✅ هتعمل Delete        │     │ ✅ Reports وـ Dashboards     │
│ ✅ محتاج Change Track  │     │ ✅ APIs اللي بترجع بيانات   │
│ ✅ Unit of Work         │     │ ✅ عايز أداء أحسن           │
└─────────────────────────┘     └───────────────────────────────┘
```

---

## 🧱 Section 10 – Full Working Examples

### Sample Entities

```csharp
// Models/Employee.cs
public class Employee
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    public int Age { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation Properties
    public virtual ICollection<Order> Orders { get; set; }
}

// Models/Order.cs
public class Order
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public decimal Total { get; set; }
    public DateTime OrderDate { get; set; }
    
    // Navigation Property
    public virtual Employee Employee { get; set; }
}
```

### Sample DbContext

```csharp
// Data/AppDbContext.cs
public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
            "Server=.;Database=EFStudyDb;Trusted_Connection=True;TrustServerCertificate=True;"
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API Configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name);
            
            // Relationship
            entity.HasMany(e => e.Orders)
                  .WithOne(o => o.Employee)
                  .HasForeignKey(o => o.EmployeeId);
        });
        
        // Seed Data
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Name = "Ahmed", Age = 30, Address = "Cairo" },
            new Employee { Id = 2, Name = "Sara", Age = 25, Address = "Alex" }
        );
    }
}
```

### Migration Example

```csharp
// Migrations/20240101_InitialCreate.cs (مثال مولد تلقائياً)
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Employees",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(maxLength: 100, nullable: false),
                Age = table.Column<int>(nullable: false),
                Address = table.Column<string>(nullable: true),
                Gender = table.Column<string>(nullable: true),
                CreatedAt = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employees", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Employees");
    }
}
```

---

## 🧾 Section 11 – Loading Strategies

### 🔴 Lazy Loading (التحميل الكسول)

في الـ **Lazy Loading** البيانات المرتبطة مش بتتحمل إلا لما تطلبها صراحةً. EF بيعمل Query جديدة كل مرة تطلب فيها Navigation Property.

```csharp
// تحتاج:
// 1. virtual على الـ Navigation Properties
// 2. تثبت: dotnet add package Microsoft.EntityFrameworkCore.Proxies
// 3. options.UseLazyLoadingProxies()

public class Employee
{
    public virtual ICollection<Order> Orders { get; set; } // virtual مهمة!
}

// الاستخدام
var emp = context.Employees.Find(1);
// لسه مجبتش الـ Orders

var orders = emp.Orders; // هنا EF بيعمل Query جديدة للـ Orders
// SELECT * FROM Orders WHERE EmployeeId = 1
```

**المشكلة: N+1 Problem**

```
لو عندك 100 موظف:
1 Query لجيب الموظفين
+
100 Query لجيب أوردرات كل موظف
= 101 Query!! 🔴 مشكلة كبيرة
```

### 🟢 Eager Loading (التحميل المتحمس)

في الـ **Eager Loading** بتقول لـ EF صراحةً "جيب البيانات المرتبطة معايا في نفس الـ Query" باستخدام `Include()`.

```csharp
// تحميل الموظفين مع أوردراتهم في query واحدة
var employees = context.Employees
    .Include(e => e.Orders)  // JOIN مع جدول Orders
    .ToList();

// تحميل متعدد المستويات
var employees = context.Employees
    .Include(e => e.Orders)
        .ThenInclude(o => o.Products) // Include nested
    .Include(e => e.Department)       // Include آخر
    .ToList();
```

### 🔵 Explicit Loading (التحميل الصريح)

في الـ **Explicit Loading** بتحمل البيانات المرتبطة يدوياً لما تحتاجها.

```csharp
var emp = context.Employees.Find(1);

// تحميل صريح لما تحتاجه
context.Entry(emp).Collection(e => e.Orders).Load();

// تحميل Reference (علاقة 1-1)
context.Entry(emp).Reference(e => e.Department).Load();
```

### المقارنة

| الجانب | Lazy Loading | Eager Loading | Explicit Loading |
|--------|-------------|---------------|-----------------|
| **وقت التحميل** | عند الطلب | مع Query الأساسية | يدوي |
| **عدد الـ Queries** | N+1 ⚠️ | 1 ✅ | حسب الحاجة |
| **الـ Performance** | بطيء مع كتير | سريع | مرن |
| **الاستخدام** | بسيط | الأفضل | عند الحاجة |

---

## 🧾 Section 12 – Model Configuration

### طريقة 1: Data Annotations

```csharp
public class Employee
{
    [Key]                          // Primary Key
    public int Id { get; set; }
    
    [Required]                     // NOT NULL
    [MaxLength(100)]               // varchar(100)
    [MinLength(2)]
    public string Name { get; set; }
    
    [Range(18, 65)]               // CHECK constraint
    public int Age { get; set; }
    
    [EmailAddress]                 // Validation
    public string Email { get; set; }
    
    [Column("emp_address")]       // اسم العمود مختلف
    public string Address { get; set; }
    
    [Table("tbl_Employees")]      // اسم الجدول
    [NotMapped]                    // مش بيتحفظ في DB
    public string FullInfo => $"{Name} - {Age}";
    
    [ForeignKey("DepartmentId")]  // Foreign Key
    public Department Department { get; set; }
    public int DepartmentId { get; set; }
}
```

### طريقة 2: Fluent API

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Employee>(entity =>
    {
        // الجدول والـ Schema
        entity.ToTable("tbl_Employees", schema: "hr");
        
        // الـ Primary Key
        entity.HasKey(e => e.Id);
        
        // الخصائص
        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("emp_name");
            
        entity.Property(e => e.Age)
            .HasDefaultValue(18);
            
        // الـ Index
        entity.HasIndex(e => e.Email)
            .IsUnique();
            
        // الـ Relationships
        entity.HasMany(e => e.Orders)
              .WithOne(o => o.Employee)
              .HasForeignKey(o => o.EmployeeId)
              .OnDelete(DeleteBehavior.Cascade);
              
        // Composite Key
        entity.HasKey(e => new { e.Id, e.DepartmentId });
    });
}
```

### Fluent API مقابل Data Annotations

| الجانب | Data Annotations | Fluent API |
|--------|-----------------|-----------|
| **المكان** | فوق الـ Properties | في `OnModelCreating` |
| **السهولة** | أسهل وأوضح | أكثر تعقيداً |
| **الـ Separation** | Entities مكتظة | منفصلة عن الـ Domain |
| **الإمكانيات** | محدودة | أكثر إمكانيات |
| **الـ Priority** | أقل | أعلى (بيلغي الـ Annotations) |

---

## 🚀 ملخص سريع

```
ENTITY FRAMEWORK = ORM من Microsoft
     ↓
يحول قاعدة البيانات ←→ C# Classes تلقائياً
     ↓
3 طرق للعمل:
    1️⃣ Code First  → اكتب كود → ولّد DB
    2️⃣ Database First → DB موجودة → ولّد Classes
    3️⃣ Model First  → EDMX Designer (نادر)
     ↓
Migrations = إدارة تغييرات DB بنظام منظم
     ↓
3 استراتيجيات Inheritance:
    🔵 TPT = جدول لكل كلاس (JOINs)
    🟢 TPH = جدول واحد (NULLs) - Default
    🟠 TPC = جدول لكل Concrete Class
     ↓
Change Tracker → SaveChanges() → Unit of Work
     ↓
Providers يدعم: SQL Server, MySQL, PostgreSQL, Oracle, ...
```
