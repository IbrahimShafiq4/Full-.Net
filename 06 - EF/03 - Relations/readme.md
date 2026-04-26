# 📚 شرح Entity Framework Core — بالعامية المصرية

---

## 📑 فهرس

- [📚 شرح Entity Framework Core — بالعامية المصرية](#-شرح-entity-framework-core--بالعامية-المصرية)
  - [📑 فهرس](#-فهرس)
- [1. إيه هو الـ EF Core ده أصلاً؟](#1-إيه-هو-الـ-ef-core-ده-أصلاً)
- [2. الـ Entity — الجدول بتاعك في الـ DB](#2-الـ-entity--الجدول-بتاعك-في-الـ-db)
  - [🧩 الـ Employee Class](#-الـ-employee-class)
  - [🏬 الـ Department Class](#-الـ-department-class)
  - [📮 الـ EmployeeAddress Class](#-الـ-employeeaddress-class)
- [3. الـ DbContext — البوابة للـ DB](#3-الـ-dbcontext--البوابة-للـ-db)
- [4. Data Annotations — بتتحكم في الـ Columns](#4-data-annotations--بتتحكم-في-الـ-columns)
- [5. العلاقات — Relationships](#5-العلاقات--relationships)
  - [One-to-One بين Department والـ Manager](#one-to-one-بين-department-والـ-manager)
  - [One-to-One بين Employee والـ Department](#one-to-one-بين-employee-والـ-department)
  - [One-to-Many بين Employee و EmployeeAddress](#one-to-many-بين-employee-و-employeeaddress)
- [6. Fluent API — طريقة تانية أقوى](#6-fluent-api--طريقة-تانية-أقوى)
- [7. الوراثة في الـ DB — TPCC و TPH](#7-الوراثة-في-الـ-db--tpcc-و-tph)
  - [الموقف](#الموقف)
  - [الـ TPCC — Table Per Concrete Class](#الـ-tpcc--table-per-concrete-class)
  - [الـ TPH — Table Per Hierarchy](#الـ-tph--table-per-hierarchy)
  - [المقارنة](#المقارنة)
- [8. الـ Program.cs — كيف بنستخدم كل ده](#8-الـ-programcs--كيف-بنستخدم-كل-ده)
- [9. أوامر الـ Migrations](#9-أوامر-الـ-migrations)

---

---

# 1. إيه هو الـ EF Core ده أصلاً؟

تخيل إنك بتشتغل على مشروع وعندك DB فيها جداول وبيانات.

عادةً لو هتتكلم مع الـ DB ده بتكتب SQL:

```sql
SELECT * FROM Employees WHERE Id = 1
INSERT INTO Employees (Name, Salary) VALUES ('Ali', 5000)
```

**الـ EF Core بيلغي الحاجة دي كلها.**

بدل ما تكتب SQL، بتكتب C# عادي:

```csharp
var emp = context.Employees.Find(1);
context.Employees.Add(new Employee { Name = "Ali", Salary = 5000 });
context.SaveChanges();
```

وهو بيترجم الكود بتاعك لـ SQL ويبعته للـ DB لوحده.

ده اللي بيتسمى **ORM — Object Relational Mapper** يعني بيحول الـ Objects بتاعتك لـ Tables والـ Tables لـ Objects.

```
  كودك C#          EF Core           SQL Server
┌──────────┐     ┌─────────┐      ┌────────────────┐
│  Classes │ ──► │ Translat│ ──►  │  SELECT/INSERT │
│  Objects │ ◄── │   or    │ ◄──  │  UPDATE/DELETE │
└──────────┘     └─────────┘      └────────────────┘
```

---

---

# 2. الـ Entity — الجدول بتاعك في الـ DB

الـ **Entity** ببساطة هي الـ Class اللي بتمثل جدول في الـ DB.
كل Property في الـ Class بتتحول لـ Column في الجدول.

---

## 🧩 الـ Employee Class

```csharp
public enum Gender
{
    Male,
    Female
}
```

الـ `enum` ده بيعمل نوع بيانات جديد اسمه `Gender` بس فيه قيمتين.
**ليه نستخدمه؟** بدل ما نكتب String وننسى نكتب "Male" صح أو غلط، بنستخدم قيمة محددة من الـ enum ومستحيل نغلط.
في الـ DB هيتخزن كـ رقم — `Male = 0` و `Female = 1`.

---

```csharp
public class Employee
{
    public int Id { get; set; }
```

الـ `Id` ده هو الـ **Primary Key** بتاع الجدول.
EF بيعرفه تلقائياً لأن اسمه `Id` — ده اتفاق معروف اسمه Convention.
هيتخزن في الـ DB كـ `INT IDENTITY` يعني بيزيد لوحده كل ما تضيف Row جديدة.

---

```csharp
    [Required]
    [Column(TypeName = "varchar")]
    [StringLength(50), MinLength(5)]
    public string Name { get; set; }
```

الـ `[Required]` معناها الـ Column ده **NOT NULL** في الـ DB — يعني مش ممكن تحفظ Employee من غير Name.

الـ `[Column(TypeName = "varchar")]` بيغير نوع الـ Column في الـ DB.
الـ Default لو ماقلتش حاجة بيبقى `nvarchar` وده بيدعم Arabic وكل اللغات بس بياخد ضعف الحجم.
لو قلت `varchar` بياخد نص الحجم بس بيدعم الإنجليزي بس.

الـ `[StringLength(50)]` بيقول أقصى عدد حروف 50.
الـ `[MinLength(5)]` بيقول أقل عدد حروف 5.
يعني الاسم لازم يكون **من 5 لـ 50 حرف**.

---

```csharp
    [Column(TypeName = "money")]
    public decimal Salary { get; set; }
```

الـ `decimal` في C# بيتحول افتراضياً لـ `decimal(18,2)` في الـ DB.
هنا بنقول خزنه كـ `money` وده type خاص في SQL Server للتعامل مع العملات والفلوس.

---

```csharp
    [Range(18, 60, ErrorMessage = "Invalid Input You Must Enter Age in between [10, 60]")]
    public int? Age { get; set; }
```

الـ `int?` — الـ `?` دي مهمة جداً.
معناها الـ Property دي **Nullable** يعني مقدرش تخزن فيها `null`.
في الـ DB هيبقى `INT NULL` بدل `INT NOT NULL`.

الـ `[Range(18, 60)]` بيعمل Validation إن القيمة لازم تكون بين 18 و 60.
لو حد بعت 17 أو 61 هيطلع الـ Error Message المكتوب.
**لاحظ:** في الـ Error Message مكتوب `[10, 60]` بس الـ Range الفعلي `[18, 60]` — ده خطأ في الكود الأصلي.

---

```csharp
    public string Address { get; set; }
```

String عادية من غير أي Annotations يعني:
في الـ DB هيبقى `nvarchar(MAX)` وممكن يبقى `null` — مش مطلوب.

---

```csharp
    [EmailAddress]
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
```

الـ `[Required]` — مش ممكن يبقى فاضي.

الـ `[EmailAddress]` بيعمل Validation إن الـ String دي فيها `@` وصيغتها صح كـ Email.

الـ `[DataType(DataType.EmailAddress)]` بيقول نوع الـ Field ده Email — ده بيُستخدم أكتر في ASP.NET لما بتعمل Forms عشان يعرف يعرض الـ Input بشكل مناسب.

الاتنين `[EmailAddress]` و `[DataType]` بيعملوا حاجة قريبة من بعض — وجود الاتنين مع بعض مش ضروري بس مش غلط.

---

```csharp
    [Required]
    public Gender Gender { get; set; }
```

الـ Gender مش ممكن يبقى فاضي.
هيتخزن كـ `int` في الـ DB — `0` لـ Male و `1` لـ Female.

---

```csharp
    [ForeignKey("Department")]
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
```

ده أهم جزء في الـ Employee — بيربطه بالـ Department.

الـ `DepartmentId` ده الـ **Foreign Key** الفعلي اللي بيتخزن في الـ DB كـ Column.

الـ `[ForeignKey("Department")]` بيقول لـ EF إن الـ `DepartmentId` ده هو الـ FK الخاص بالـ `Department` Navigation Property اللي تحته — ده بس بيوضح العلاقة لـ EF.

الـ `Department Department` ده الـ **Navigation Property** — مش Column في الـ DB.
ده بيخليك توصل لكل بيانات الـ Department من غير ما تكتب JOIN.

```
  جدول Employees في الـ DB
  ┌──────┬────────┬────────────┬──────────────────┐
  │  Id  │  Name  │   Salary   │  DepartmentId    │ ← بيتخزن هنا في الـ DB
  ├──────┼────────┼────────────┼──────────────────┤
  │   1  │  Ali   │    5000    │        1         │
  │   2  │  Omar  │    4000    │        1         │
  └──────┴────────┴────────────┴──────────────────┘
                                        ↓
  جدول Departments في الـ DB
  ┌──────┬─────────┬──────────────────┐
  │  Id  │  Name   │  DateOfCreation  │
  ├──────┼─────────┼──────────────────┤
  │   1  │   IT    │   2024-01-01     │
  └──────┴─────────┴──────────────────┘
```

---

```csharp
    public List<EmployeeAddress> EmployeeAddresses { get; set; }
}
```

الـ `List<EmployeeAddress>` دي Navigation Property كمان بس للعلاقة **One-to-Many**.
معناها الموظف الواحد ممكن يكون عنده عناوين كتير.
`List` دايماً معناها Many في العلاقة.

---

## 🏬 الـ Department Class

```csharp
public class Department
{
    public Department()
    {
        DateOfCreation = DateTime.Now;
    }
```

الـ Constructor بيتشغل تلقائياً كل ما تعمل `new Department()`.
هنا بيحط في `DateOfCreation` التاريخ والوقت الحالي تلقائياً من غير ما تكتبه أنت.

```csharp
    public int      Id             { get; set; }
    public string   Name           { get; set; }
    public DateTime DateOfCreation { get; set; }
```

عادي — Primary Key، اسم القسم، وتاريخ الإنشاء.
الـ `DateTime` بيتخزن كـ `datetime2` في SQL Server.

---

```csharp
    public int      ManagerId { get; set; }
    public Employee Manager   { get; set; }
}
```

زي بالظبط اللي شوفناه في الـ Employee.
الـ `ManagerId` هو الـ FK، والـ `Manager` هو الـ Navigation Property.
معناها كل Department عنده Manager وهو Employee معين.

```
  جدول Departments في الـ DB
  ┌──────┬────────┬──────────────┬────────────────┐
  │  Id  │  Name  │DateOfCreation│   ManagerId    │ ← FK بيشاور على Employee
  ├──────┼────────┼──────────────┼────────────────┤
  │   1  │   IT   │  2024-01-01  │       3        │ ← Employee رقم 3 هو الـ Manager
  │   2  │   HR   │  2024-01-01  │       1        │ ← Employee رقم 1 هو الـ Manager
  └──────┴────────┴──────────────┴────────────────┘
```

---

## 📮 الـ EmployeeAddress Class

```csharp
public class EmployeeAddress
{
    public int    Id     { get; set; }
    public string Streat { get; set; }
    public string City   { get; set; }
    public string State  { get; set; }
}
```

ده جدول بسيط للعناوين.
**لاحظ:** مكتوب `Streat` بدل `Street` — ده خطأ إملائي في الكود الأصلي وهيتنقل للـ DB Column بنفس الاسم الغلط.

الـ Class ده محتاج FK للـ Employee عشان يتربط بيه:

```csharp
public int      EmployeeId { get; set; }  // لازم تضيفها
public Employee Employee   { get; set; }  // لازم تضيفها
```

من غيرهم EF مش هيعرف يعمل العلاقة مع الـ Employee.

---

---

# 3. الـ DbContext — البوابة للـ DB

الـ `DbContext` ده زي **مدير** بين كودك والـ DB.
كل حاجة بتعملها في الـ DB بتعملها من خلاله.

```csharp
internal class CompanyDbContext : DbContext
{
```

الـ `internal` معناها الـ Class ده مش ينفع يتشاف من بره الـ Project.
الـ `: DbContext` معناها بيرث من الـ `DbContext` اللي في EF Core وبياخد منه كل الـ Methods والـ Features.

---

```csharp
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "server=IBRAHIMSHAFIQ\\SQLEXPRESS; database=CombanyDb; Trusted_Connection=true; TrustServerCertificate=true;"
        );
    }
```

الـ `OnConfiguring` ده بتقول فيه لـ EF: **هتتكلم مع أنهي DB وفين.**

الـ `optionsBuilder.UseSqlServer(...)` بيقول هنستخدم SQL Server.
لو كنت بتستخدم SQLite كانت هتكتب `UseSqlite(...)` وهكذا.

الـ Connection String دي فيها:

```
server=IBRAHIMSHAFIQ\\SQLEXPRESS  ← اسم الـ PC بتاعك \ اسم الـ SQL Server Instance
database=CombanyDb                ← اسم الـ DB اللي هتتعمل أو هتتكلم معاها
Trusted_Connection=true           ← هنستخدم Windows Login مش User/Password
TrustServerCertificate=true       ← بنتجاهل الـ SSL Certificate في الـ Development
```

---

```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>()
                    .HasOne(dept => dept.Manager)
                    .WithOne()
                    .HasForeignKey<Department>(dept => dept.ManagerId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
                    .HasOne(emp => emp.Department)
                    .WithOne()
                    .HasForeignKey<Employee>(emp => emp.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);
    }
```

الـ `OnModelCreating` ده بتعرف فيه العلاقات بـ **Fluent API**.
هنشرحه تفصيلياً في section رقم 6.

---

```csharp
    public DbSet<Employee>        Employees        { get; set; }
    public DbSet<Department>      Departments      { get; set; }
    public DbSet<EmployeeAddress> EmployeeAddresses{ get; set; }
}
```

الـ `DbSet<T>` ده بيمثل الجدول في الـ DB.
من غير ما تضيف DbSet للـ Class، الجدول مش هيتعمل في الـ DB حتى لو الـ Class موجود.

```
  DbSet<Employee> Employees    →  جدول اسمه "Employees" في الـ DB
  DbSet<Department> Departments →  جدول اسمه "Departments" في الـ DB
```

---

---

# 4. Data Annotations — بتتحكم في الـ Columns

الـ Data Annotations هي Attributes بتتكتب فوق الـ Property وبتقول لـ EF:
إزاي يعمل الـ Column ده في الـ DB وإيه الـ Validation اللي هيتعمل عليه.

```
  ┌───────────────────────┬──────────────────────────────────────────────────┐
  │    Annotation         │   بيعمل إيه بالظبط؟                             │
  ├───────────────────────┼──────────────────────────────────────────────────┤
  │ [Required]            │ Column NOT NULL في الـ DB                        │
  │                       │ لو بعت قيمة فاضية هيطلع Error                   │
  ├───────────────────────┼──────────────────────────────────────────────────┤
  │ [Column(TypeName="..")] │ بيغير نوع الـ Column في الـ DB                │
  │                       │ مثال: "varchar", "money", "nvarchar(100)"        │
  ├───────────────────────┼──────────────────────────────────────────────────┤
  │ [StringLength(n)]     │ أقصى عدد حروف = n                               │
  ├───────────────────────┼──────────────────────────────────────────────────┤
  │ [MinLength(n)]        │ أقل عدد حروف = n                                │
  ├───────────────────────┼──────────────────────────────────────────────────┤
  │ [Range(min, max)]     │ القيمة لازم تكون بين min و max                   │
  ├───────────────────────┼──────────────────────────────────────────────────┤
  │ [EmailAddress]        │ لازم الـ String تكون صيغة Email صح               │
  ├───────────────────────┼──────────────────────────────────────────────────┤
  │ [ForeignKey("Name")]  │ بيقول إن الـ Property دي FK لـ Navigation معين  │
  ├───────────────────────┼──────────────────────────────────────────────────┤
  │ [DataType(DataType.x)]│ بيحدد نوع البيانات للـ UI والـ Validation      │
  └───────────────────────┴──────────────────────────────────────────────────┘
```

---

---

# 5. العلاقات — Relationships

في قواعد البيانات في 3 أنواع علاقات أساسية:

```
  One-to-One    →  موظف واحد عنده رخصة قيادة واحدة بس
  One-to-Many   →  قسم واحد عنده موظفين كتير
  Many-to-Many  →  طالب ممكن يكون في مواد كتير، ومادة فيها طلاب كتير
```

في الكود بتاعنا عندنا One-to-One و One-to-Many.

---

## One-to-One بين Department والـ Manager

الفكرة: كل Department عنده Manager واحد بس — وكل Employee ممكن يكون Manager لـ Department واحدة بس.

```
  Department                        Employee
  ┌─────────────────────┐           ┌────────────────────┐
  │ Id                  │           │ Id                 │
  │ Name                │  1 ─── 1  │ Name               │
  │ DateOfCreation      │           │ Salary             │
  │ ManagerId  ─────────┼──────────►│ ...                │
  └─────────────────────┘           └────────────────────┘
```

عشان تعمل العلاقة دي محتاج حاجتين في الـ Department Class:

```csharp
public int      ManagerId { get; set; }   // الـ FK اللي بيتخزن في الـ DB
public Employee Manager   { get; set; }   // Navigation Property للوصول للـ Employee Object
```

---

## One-to-One بين Employee والـ Department

الفكرة: كل Employee بيشتغل في Department واحد بس.

```csharp
[ForeignKey("Department")]
public int        DepartmentId { get; set; }   // الـ FK
public Department Department   { get; set; }   // Navigation Property
```

---

## One-to-Many بين Employee و EmployeeAddress

الفكرة: الموظف الواحد ممكن يكون عنده أكتر من عنوان.

```
  Employee                         EmployeeAddress
  ┌─────────────┐                  ┌──────────────────┐
  │ Id          │                  │ Id               │
  │ Name        │  1 ──────── *    │ Street           │
  │ ...         │                  │ City             │
  │             │                  │ State            │
  │             │◄─────────────────│ EmployeeId (FK)  │
  └─────────────┘                  └──────────────────┘
```

في الـ Employee بيبقى عندك:
```csharp
public List<EmployeeAddress> EmployeeAddresses { get; set; }
```

وفي الـ EmployeeAddress لازم يبقى عندك:
```csharp
public int      EmployeeId { get; set; }
public Employee Employee   { get; set; }
```

الـ `List` دايماً معناها Many.

---

---

# 6. Fluent API — طريقة تانية أقوى

الـ Fluent API بتكتبها جوه `OnModelCreating` في الـ Context.
هي بديل لـ Data Annotations وبتديك تحكم أكتر في العلاقات المعقدة.

```
  ┌──────────────────────────┬──────────────────────────────────┐
  │    Data Annotations      │         Fluent API               │
  ├──────────────────────────┼──────────────────────────────────┤
  │ بتكتبها فوق الـ Property │ بتكتبها في OnModelCreating     │
  │ أبسط وأوضح قراية         │   أقوى وبتدي تحكم أكتر        │
  │ مناسبة للحاجات البسيطة   │ مناسبة للعلاقات المعقدة       │
  └──────────────────────────┴──────────────────────────────────┘
```

---

```csharp
modelBuilder.Entity<Department>()
            .HasOne(dept => dept.Manager)
            .WithOne()
            .HasForeignKey<Department>(dept => dept.ManagerId)
            .OnDelete(DeleteBehavior.Cascade);
```

خليني أشرح كل جزء لوحده:

**`.Entity<Department>()`**
بتقول لـ EF: "أنا دلوقتي هتكلم عن جدول الـ Department."

**`.HasOne(dept => dept.Manager)`**
بتقول: "الـ Department عنده علاقة بـ واحد من نوع Employee وهو الـ Manager."
الـ Lambda `dept => dept.Manager` بتحدد الـ Navigation Property اللي في الـ Department Class.

**`.WithOne()`**
بتقول: "الطرف التاني كمان عنده علاقة بـ واحد — يعني One-to-One."
الأقواس فاضية لأن الـ Employee مش عنده Navigation Property ترجعله من ناحية الـ Manager.

**`.HasForeignKey<Department>(dept => dept.ManagerId)`**
بتقول: "الـ FK موجود في جدول `Department` والـ Column اسمه `ManagerId`."
الـ `<Department>` بتحدد إن الـ FK في الـ Department مش في الـ Employee.

**`.OnDelete(DeleteBehavior.Cascade)`**
بتقول: لو حذفت الـ Department — احذف الـ Employee المرتبط بيه تلقائياً.

```
  ┌──────────────────┬────────────────────────────────────────────────────┐
  │  DeleteBehavior  │  بيحصل إيه لو حذفت الـ Parent؟                   │
  ├──────────────────┼────────────────────────────────────────────────────┤
  │ Cascade          │ بيحذف الـ Children معاه تلقائياً                   │
  ├──────────────────┼────────────────────────────────────────────────────┤
  │ Restrict         │ بيمنعك من الحذف لو في Children مرتبطين           │
  ├──────────────────┼────────────────────────────────────────────────────┤
  │ SetNull          │ بيحذف الـ Parent وبيحط NULL في الـ FK الـ Children  │
  ├──────────────────┼────────────────────────────────────────────────────┤
  │ NoAction         │ مش بيعمل حاجة — الـ DB بتتعامل معاه              │
  └──────────────────┴────────────────────────────────────────────────────┘
```

---

```csharp
modelBuilder.Entity<Employee>()
            .HasOne(emp => emp.Department)
            .WithOne()
            .HasForeignKey<Employee>(emp => emp.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
```

نفس الفكرة بالظبط، بس من ناحية الـ Employee.
بتقول: الـ Employee عنده Department واحد، والـ FK هو `DepartmentId` الموجود في جدول الـ Employee.

---

---

# 7. الوراثة في الـ DB — TPCC و TPH

## الموقف

عندنا Abstract Class اسمها `Employee` فيها البيانات المشتركة.
وعندنا نوعين من الموظفين بيرثوا منها.

```csharp
public abstract class Employee
{
    public int    Id      { get; set; }
    public string Name    { get; set; }
    public string Address { get; set; }
    public int    Age     { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Age: {Age}, Address: {Address}";
    }
}
```

الـ `abstract` معناها مستحيل تعمل Object منها مباشرة:
```csharp
var e = new Employee();         // ❌ Error
var e = new FullTimeEmployee(); // ✅ يشتغل
```

ليه؟ لأن مفيش حاجة اسمها "موظف عادي" — كل موظف إما Full Time أو Part Time.

الـ `ToString()` ده بيتعمل Override عشان لما تكتب `Console.WriteLine(emp)` يطبع البيانات بشكل مفيد بدل اسم الـ Class.

---

```csharp
public class FullTimeEmployee : Employee
{
    public DateTime StartDate { get; set; }
    public double   Salary    { get; set; }
}

public class PartTimeEmployee : Employee
{
    public double HourRate     { get; set; }
    public int    CountOfHours { get; set; }
}
```

الـ `FullTimeEmployee` عنده راتب ثابت وتاريخ بداية.
الـ `PartTimeEmployee` بيتحسب أجره على حسب ساعات الشغل.

---

## الـ TPCC — Table Per Concrete Class

الفكرة: نعمل جدول منفصل لكل Type.

```csharp
// في الـ Context بتعمل DbSet للـ Derived Classes بس
public DbSet<FullTimeEmployee> FullTimeEmployees { get; set; }
public DbSet<PartTimeEmployee> PartTimeEmployees { get; set; }
```

**الناتج في الـ DB:**

```
  ┌────────────────────────────────────────────────────────┐
  │               FullTimeEmployees                        │
  ├──────┬───────┬─────┬──────────┬───────────┬───────────┤
  │  Id  │ Name  │ Age │ Address  │ StartDate │  Salary   │
  ├──────┼───────┼─────┼──────────┼───────────┼───────────┤
  │  1   │  Ali  │ 27  │  Cairo   │2024-01-01 │   5000    │
  └──────┴───────┴─────┴──────────┴───────────┴───────────┘

  ┌────────────────────────────────────────────────────────────┐
  │               PartTimeEmployees                            │
  ├──────┬────────┬─────┬──────────┬──────────┬────────────────┤
  │  Id  │  Name  │ Age │ Address  │ HourRate │ CountOfHours   │
  ├──────┼────────┼─────┼──────────┼──────────┼────────────────┤
  │  1   │  Omar  │ 25  │  Cairo   │   100    │      80        │
  └──────┴────────┴─────┴──────────┴──────────┴────────────────┘
```

لاحظ: كل جدول فيه Columns الـ Base Class (Id, Name, Age, Address) + الـ Columns الخاصة بيه.
مفيش جدول اسمه `Employees`.

---

## الـ TPH — Table Per Hierarchy

الفكرة: ندمج كل حاجة في **جدول واحد**.

```csharp
// في الـ Context بتعمل DbSet للـ Abstract Class بس
public DbSet<Employee> Employees { get; set; }
```

وفي الـ `OnModelCreating` بتقول للـ EF إن فيه Derived Classes:

```csharp
modelBuilder.Entity<PartTimeEmployee>().HasBaseType<Employee>();
modelBuilder.Entity<FullTimeEmployee>().HasBaseType<Employee>();
```

**الناتج في الـ DB:**

```
  ┌────────────────────────────────────────────────────────────────────────────────────────┐
  │                                     Employees                                          │
  ├──────┬───────────┬─────┬─────────┬───────────┬────────┬──────────┬─────────────────────┤
  │  Id  │   Name    │ Age │ Address │ StartDate │ Salary │ HourRate │ CountOfHours│ Disc. │
  ├──────┼───────────┼─────┼─────────┼───────────┼────────┼──────────┼─────────────────────┤
  │  1   │ Ibrahim   │ 27  │  Cairo  │2024-01-01 │  5000  │  NULL    │    NULL  │FullTime  │
  │  2   │ Ibrahim2  │ 27  │  Cairo  │   NULL    │  NULL  │   100    │    100   │PartTime  │
  └──────┴───────────┴─────┴─────────┴───────────┴────────┴──────────┴─────────────────────┘
                                        ↑ NULL                ↑ NULL
                               (مش PartTime)          (مش FullTime)
```

**الـ Discriminator Column** — EF بيضيفها تلقائياً عشان يعرف كل Row من أي Type.
الـ Columns الخاصة بكل Type بتبقى `Nullable` لأن مش كل موظف عنده كل الـ Columns.

---

## المقارنة

```
  ┌──────────────────┬─────────────────────────────┬──────────────────────────────┐
  │                  │          TPCC               │            TPH               │
  ├──────────────────┼─────────────────────────────┼──────────────────────────────┤
  │ عدد الجداول     │ جدول لكل Derived Class      │ جدول واحد بس               │
  ├──────────────────┼─────────────────────────────┼──────────────────────────────┤
  │ الـ NULL Values  │ مفيش Nulls خالص             │ فيه Nullable Columns        │
  ├──────────────────┼─────────────────────────────┼──────────────────────────────┤
  │ الـ Query        │ بتكتب على الجدول المحدد      │ بتكتب على جدول Employees │
  ├──────────────────┼─────────────────────────────┼──────────────────────────────┤
  │ تنظيم الـ DB     │ أنظف — كل جدول واضح         │ جدول واحد ممكن يبقى كبير │
  ├──────────────────┼─────────────────────────────┼──────────────────────────────┤
  │ متى تستخدمه؟     │ لما الـ Types مختلفة جداً    │ لما الـ Types متشابهة     │
  └──────────────────┴─────────────────────────────┴──────────────────────────────┘
```

---

---

# 8. الـ Program.cs — كيف بنستخدم كل ده

```csharp
using (EmployeeDbContext context = new EmployeeDbContext())
{
```

الـ `using` هنا بتضمن إن الـ Connection بتاعت الـ DB هتتقفل تلقائياً لما الـ Block ينتهي حتى لو حصل Error.
ده الطريقة الصح دايماً مع الـ DbContext.

---

```csharp
    FullTimeEmployee fullTimeEmployee = new FullTimeEmployee()
    {
        Name      = "Ibrahim Shafiq",
        StartDate = DateTime.Now,
        Salary    = 5000,
        Age       = 27,
        Address   = "Cairo",
    };

    PartTimeEmployee partTimeEmployee = new PartTimeEmployee()
    {
        Name         = "Ibrahim Shafiq Two",
        Age          = 27,
        Address      = "Cairo",
        HourRate     = 100,
        CountOfHours = 100,
    };
```

بنعمل Objects جديدة من كل Type ونملاها بالبيانات.
الـ `Id` مش بنحطه لأن الـ DB بتحطه تلقائياً — Auto Increment.

---

```csharp
    context.Employees.Add(fullTimeEmployee);
    context.Employees.Add(partTimeEmployee);
```

بنضيف الاتنين من خلال `Employees` DbSet الواحد.
EF بيعرف نوع كل Object ويحط في الـ Discriminator Column القيمة الصح تلقائياً.

---

```csharp
    var firstEmployee = context.Employees.FirstOrDefault();
```

`.FirstOrDefault()` بترجع أول Record في الجدول.
لو الجدول فاضي بترجع `null` — مش بتعمل Error.
لو كانت `.First()` من غير الـ OrDefault كانت هترمي Exception لو الجدول فاضي.

---

```csharp
    context.Set<FullTimeEmployee>().Add(fullTimeEmployee);
    context.Set<PartTimeEmployee>().Add(partTimeEmployee);
```

دي طريقة تانية للإضافة من غير ما تحتاج DbSet محدد.
`context.Set<T>()` بترجعلك الـ DbSet بتاع أي Type حتى لو مش معرّف صراحةً في الـ Context.

```
  3 طرق مختلفة للإضافة وكلهم بيعملوا نفس الحاجة:

  맡 context.FullTimeEmployees.Add(emp)   ← لو عندك DbSet محدد للـ Type
  맡 context.Employees.Add(emp)           ← عن طريق الـ Base DbSet
  맡 context.Set<FullTimeEmployee>().Add(emp) ← Generic بدون DbSet
```

---

```csharp
    // context.SaveChanges();
```

ده معلّق — لو فككت الـ Comment كل التغييرات اللي عملتها هتتحفظ في الـ DB.
من غيره، كل حاجة بتبقى في الـ Memory بس وبتتمسح.

---

```csharp
    foreach (var item in context.Employees.OfType<FullTimeEmployee>())
    {
        Console.WriteLine(item);
    }
```

`.OfType<T>()` بيرجع بس العناصر اللي نوعها `FullTimeEmployee` من الجدول.
EF بيترجمها لـ SQL بيضيف فيها `WHERE Discriminator = 'FullTimeEmployee'` تلقائياً.

الـ `Console.WriteLine(item)` بيستدعي الـ `ToString()` اللي Override-نها في الـ Base Class وبيطبع:
```
Id: 1, Name: Ibrahim Shafiq, Age: 27, Address: Cairo
```

---

---

# 9. أوامر الـ Migrations

الـ Migration هي اللي بتنقل التغييرات اللي عملتها في الـ Entities للـ DB الفعلية.

```
  عملت تغيير في الـ Class        Migration File           الـ DB
  ┌────────────────────┐       ┌──────────────────┐    ┌────────────────┐
  │  أضفت Column جديد │──────►│ Up()   SQL Code  │───►│ Column اتضاف  │
  │  أو جدول جديد      │      │ Down() SQL Code  │    │ في الجدول     │
  └────────────────────┘       └──────────────────┘    └────────────────┘
                                 الـ Up() بيطبق
                                 الـ Down() بيرجع للخلف
```

```
  ┌───────────────────────────────┬──────────────────────────────────────────────┐
  │           الأمر               │             بيعمل إيه؟                      │
  ├───────────────────────────────┼──────────────────────────────────────────────┤
  │ add-migration <اسم>           │ بيعمل Migration File جديد فيه SQL            │
  │                               │ خد عادة تسميه بالتغيير اللي عملته            │
  │                               │ مثلاً: AddManagerToDepartment                │
  ├───────────────────────────────┼──────────────────────────────────────────────┤
  │ update-database               │ بيطبق آخر Migration على الـ DB               │
  ├───────────────────────────────┼──────────────────────────────────────────────┤
  │ update-database <اسم>         │ بيرجع الـ DB لـ Migration معين بالاسم        │
  ├───────────────────────────────┼──────────────────────────────────────────────┤
  │ update-database 0             │ بيمسح كل التغييرات ويرجع الـ DB للـ Zero     │
  ├───────────────────────────────┼──────────────────────────────────────────────┤
  │ remove-migration              │ بيحذف آخر Migration File من الـ Code         │
  │                               │ يشتغل بس لو لسه مش عملت update-database     │
  └───────────────────────────────┴──────────────────────────────────────────────┘
```

**الخطوات الصح في أي تغيير:**

```
  1. عدّل الـ Entity أو الـ Context
          ↓
  2. add-migration اسم_واضح_للتغيير
          ↓
  3. افتح الـ Migration File وراجع الـ Up() و Down()
          ↓
  4. update-database
          ↓
  5. افتح الـ DB Server Explorer وتأكد إن التغيير اتطبق ✅
```

---

> 🎯 **الخلاصة بجملة واحدة:**
> EF Core بيخليك تتعامل مع الـ DB كأنك بتشتغل على Objects عادية في C#، وهو بيتولى كل الـ SQL من ورا الكواليس.