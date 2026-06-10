## أولاً: إيه اللي بنبنيه أصلاً؟

تخيل إنك بتبني موقع ويب بيتحكم في بيانات موظفين وأقسام. الموقع ده بيعمل 4 حاجات أساسية:
- **عرض** الموظفين والأقسام
- **إضافة** موظف جديد أو قسم جديد
- **تعديل** البيانات
- **حذف** أي موظف أو قسم

المشروع اتبنى باستخدام **ASP.NET MVC** — وده معناه إن الكود اتقسم لـ 3 أجزاء:
- **Model** → البيانات (الموظف عنده اسم، راتب، إيميل...)
- **View** → الصفحة اللي المستخدم بيشوفها
- **Controller** → الكود اللي بيربط البيانات بالصفحة

---

## ثانياً: هيكل المشروع (ملفات وفولدرات)

```
المشروع
├── DAL (Data Access Layer)    → طبقة البيانات - بتتكلم مع قاعدة البيانات
│   ├── Entities               → كلاسات بتمثل الجداول في الداتابيز
│   │   ├── Employee.cs        → كلاس الموظف
│   │   ├── Department.cs      → كلاس القسم
│   │   └── BaseEntity.cs      → كلاس أساسي فيه Id لكل حاجة
│   └── Context                → DbContext - اللي بيوصل بقاعدة البيانات
│
├── BLL (Business Logic Layer) → طبقة المنطق - بتتحكم في الشغل
│   ├── Interfaces             → عقود (interfaces) بتحدد الميثودز المطلوبة
│   │   ├── IGenericRepository → عقد الريبوزيتوري العام
│   │   ├── IEmployeeRepository→ عقد خاص بالموظفين
│   │   ├── IDepartmentRepository → عقد خاص بالأقسام
│   │   └── IUnitOfWork        → عقد Unit of Work
│   └── Repositories           → التنفيذ الفعلي للعقود دي
│       ├── GenericRepository  → ريبوزيتوري عام بيتوارث منه الباقي
│       ├── EmployeeRepository → ريبوزيتوري خاص بالموظفين
│       ├── DepartmentRepository → ريبوزيتوري خاص بالأقسام
│       └── UnitOfWork         → بيجمع كل الريبوزيتوري في حتة واحدة
│
└── PL (Presentation Layer)    → طبقة العرض - الكنترولرز والـ Views
    ├── Controllers
    │   ├── EmployeeController.cs
    │   ├── DepartmentController.cs
    │   └── HomeController.cs
    └── Views
        ├── Employee/
        └── Department/
```

---

## ثالثاً: الـ Entities (الكلاسات اللي بتمثل الداتا)

### كلاس `Employee` (الموظف)

ده الكلاس اللي بيمثل جدول الموظفين في قاعدة البيانات. كل `property` في الكلاس ده = عمود في الجدول.

```csharp
[Index(nameof(Name), nameof(Department.Name), nameof(Address), nameof(Email))]
public class Employee : BaseEntity
```

**السطر الأول ده:**
- `[Index(...)]` ده **Attribute** (تعليمة) بتقول لـ Entity Framework: "اعمل Index في قاعدة البيانات على الأعمدة دي"
- الـ Index ده زي فهرس الكتاب — بيخلي البحث أسرع بدل ما الداتابيز تدور في كل السجلات
- مثال: لو بتدور على موظف باسمه، الداتابيز هتلاقيه بسرعة أكبر بكتير

**الـ Properties (الخصائص):**

```csharp
[Required(ErrorMessage = "اسم الموظف مطلوب")]
[Display(Name = "اسم الموظف")]
[StringLength(50, MinimumLength = 2, ErrorMessage = "{0} يجب أن يكون بين {2} و {1} حرف")]
public string Name { get; set; } = string.Empty;
```

- `[Required]` → الحقل ده مش اختياري، لازم المستخدم يكتب فيه حاجة، غير كده هيطلع رسالة خطأ
- `[Display(Name = "...")]` → ده اللي بيظهر كـ Label في الفورم بدل اسم الـ Property
- `[StringLength(50, MinimumLength = 2)]` → الاسم لازم يكون من 2 لـ 50 حرف
- `{0}` في رسالة الخطأ → بتتحول لـ Display Name (اسم الموظف)
- `{1}` → الحد الأقصى (50)
- `{2}` → الحد الأدنى (2)
- `= string.Empty` → القيمة الابتدائية string فاضية بدل ما تكون `null` وتسبب مشاكل

```csharp
[Range(100, 50000, ErrorMessage = "{0} يجب أن يكون بين {1} و {2}")]
[DataType(DataType.Currency)]
[DisplayFormat(DataFormatString = "{0:C}")]
public decimal Salary { get; set; }
```

- `[Range(100, 50000)]` → الراتب لازم يكون بين 100 و 50000، مش أقل ومش أكتر
- `[DataType(DataType.Currency)]` → بيقول للـ Framework إن الرقم ده فلوس — بيفيد في الـ Scaffolding والـ UI
- `[DisplayFormat(DataFormatString = "{0:C}")]` → `{0:C}` يعني `Currency Format`، يعني 1500 هتبقى `$1,500.00`

```csharp
[DataType(DataType.Date)]
public DateTime HireDate { get; set; } = DateTime.Now;
```

- `[DataType(DataType.Date)]` → بيقول للـ HTML إنه يعمل `<input type="date">` مش input عادي
- `= DateTime.Now` → لما بتعمل موظف جديد، تاريخ التعيين هيكون النهارده تلقائياً

```csharp
public int DepartmentId { get; set; }
public Department? Department { get; set; }
```

- `DepartmentId` → ده الـ **Foreign Key** — بيحفظ رقم القسم اللي الموظف بينتمي ليه
- `Department?` → ده الـ **Navigation Property** — بيخلي Entity Framework يجيب بيانات القسم كامل لما تحتاجه
- مثال: لو قلتله `employee.Department.Name` هيرجع اسم القسم
- علامة `?` معناها إن القسم ممكن يكون `null` (لو مش متحمّل)

---

## رابعاً: نمط Repository Pattern

### إيه المشكلة اللي بيحلها؟

لو كتبت الكود كده مباشرة في الكنترولر:

```csharp
public class EmployeeController : Controller
{
    private readonly MVCAppDbContext _context;

    public IActionResult Index()
    {
        var employees = _context.Employees.ToList();  // مباشرة من الداتابيز!
        return View(employees);
    }
}
```

ده وحش ليه؟
- الكنترولر بقى مرتبط بـ Entity Framework بشكل مباشر
- لو غيرت الداتابيز من SQL لـ MongoDB مثلاً، هتغير كل كنترولر
- مش تقدر تعمل Unit Testing بسهولة

### الحل: Repository Pattern

بدل كده، بنعمل **طبقة وسط** بين الكنترولر والداتابيز.

```
Controller  →  Repository  →  Database
```

الكنترولر مش بيعرف إزاي الداتابيز شغالة. بس عارف إنه يطلب وهياخد.

---

## خامساً: الـ Interfaces

### إيه الـ Interface أصلاً؟

الـ Interface ده **عقد**. بيقول "أي كلاس يوقع على العقد ده لازم يعمل الميثودز دي". 

تخيله زي قائمة مهام: لو الكلاس قال "أنا بطبق الـ Interface ده"، يبقى لازم يعمل كل الحاجات اللي فيه.

### `IGenericRepository<T>`

```csharp
public interface IGenericRepository<T> where T : BaseEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    int Add(T entity);
    int Update(T entity);
    int Delete(T entity);
}
```

- الـ `T` ده **Generic Type** — يعني الـ Interface ده ينفع مع أي كلاس، مش بس Employees أو Departments
- `where T : BaseEntity` → قيد بيقول: "الـ T ده لازم يكون من النوع `BaseEntity` أو يورث منه" — عشان متقدرش تحطه مع أي كلاس عشوائي
- الميثودز الخمسة دي (Get, GetAll, Add, Update, Delete) هي اللي بتتكرر مع كل Entity — فبدل ما نكتبهم كل مرة، كتبناهم مرة واحدة هنا

### `IEmployeeRepository`

```csharp
public interface IEmployeeRepository : IGenericRepository<Employee>
{
    IEnumerable<Employee> GetEmployeesByDepartmentName(string name);
    IEnumerable<Employee> GetAllEmpsWithDepartment();
    IEnumerable<Employee> Search(string searchItem);
}
```

- الـ Interface ده **بيورث** من `IGenericRepository<Employee>` — يعني بيجيب معاه الـ 5 ميثودز الأساسية + بيضيف ميثودز خاصة بالموظفين
- `GetEmployeesByDepartmentName` → جيب الموظفين اللي في قسم معين
- `GetAllEmpsWithDepartment` → جيب الموظفين مع بيانات القسم بتاعهم كمان
- `Search` → ابحث في الاسم والإيميل والعنوان والقسم

### `IUnitOfWork`

```csharp
public interface IUnitOfWork
{
    public IEmployeeRepository EmployeeRepository { get; set; }
    public IDepartmentRepository DepartmentRepository { get; set; }
}
```

ده **Unit of Work** — مفهومه إنك بدل ما تحقن كل Repository لوحده في الكنترولر، تحقن حاجة واحدة بس فيها كل الـ Repositories.

بدل ما تعمل كده:
```csharp
// ❌ كتير وممل
public DepartmentController(IEmployeeRepository emp, IDepartmentRepository dept, ISomethingRepository s...) { }
```

تعمل كده:
```csharp
// ✅ بسيط ومرتب
public DepartmentController(IUnitOfWork unitOfWork) { }
```

---

## سادساً: الـ Repositories (التنفيذ)

### `GenericRepository<T>`

```csharp
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly MVCAppDbContext _context;

    public GenericRepository(MVCAppDbContext context) {
        _context = context;
    }

    public int Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return _context.SaveChanges();
    }
    // ...
}
```

- `_context.Set<T>()` → ده ماجيك Entity Framework — بيرجع الـ DbSet اللي بيطابق النوع T
  - لو T = Employee → هيرجع `_context.Employees`
  - لو T = Department → هيرجع `_context.Departments`
- `SaveChanges()` → بيحفظ التغييرات في قاعدة البيانات وبيرجع عدد الصفوف اللي اتأثرت

### `EmployeeRepository`

```csharp
public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public IEnumerable<Employee> GetAllEmpsWithDepartment()
    {
        return _context.Employees.Include(e => e.Department).ToList();
    }
```

- `.Include(e => e.Department)` → ده **Eager Loading** — بيقول لـ Entity Framework: "لما تجيب الموظفين، جيب بيانات القسم بتاع كل واحد كمان"
- من غيره، `employee.Department` هتكون `null`

```csharp
    public IEnumerable<Employee> Search(string searchItem)
    {
        return _context.Employees
                       .Include(e => e.Department)
                       .Where(e =>
                           e.Name.Trim().ToLower().Contains(searchItem.Trim().ToLower()) ||
                           e.Email.Trim().ToLower().Contains(searchItem.Trim().ToLower()) ||
                           e.Address.Trim().ToLower().Contains(searchItem.Trim().ToLower()) ||
                           e.Department!.Name.Trim().ToLower().Contains(searchItem.Trim().ToLower()))
                       .ToList();
    }
```

- `.Trim()` → بيشيل المسافات من الأول والآخر (عشان لو حد كتب "أحمد " بمسافة)
- `.ToLower()` → بيحوله لـ lowercase عشان البحث يبقى Case-Insensitive (مش فرق بين كبير وصغير)
- `.Contains()` → بيشوف لو الـ searchItem موجود في النص
- `||` → OR — يعني لو الكلمة موجودة في أي حقل من الحقول يرجعه

---

## سابعاً: Dependency Injection

### إيه ده أصلاً؟

**Dependency Injection** هو إن الكلاس مش بيعمل الـ Objects اللي محتاجها بنفسه، ده بيطلبها من برا.

**بدون DI (وحش):**
```csharp
public class EmployeeController : Controller
{
    private EmployeeRepository _repo;

    public EmployeeController()
    {
        // الكنترولر بيعمل الـ Repository بنفسه — مرتبط بيه للأبد
        _repo = new EmployeeRepository(new MVCAppDbContext(...));
    }
}
```

**مع DI (كويس):**
```csharp
public class EmployeeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    // ASP.NET بيبعتلك الـ UnitOfWork جاهز تلقائياً
    public EmployeeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
```

### الأنواع الثلاثة للـ DI في ASP.NET Core

دي بتتسجل في `Program.cs`:

| النوع | المعنى | أمتى تستخدمه |
|-------|--------|-------------|
| **Singleton** | Object واحد للتطبيق كله طول عمره | اللوجينج، الـ Configuration |
| **Scoped** | Object جديد لكل Request | الـ DbContext، الـ Repositories |
| **Transient** | Object جديد كل ما تطلبه | Services خفيفة بدون State |

```csharp
// في Program.cs
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
```

---

## ثامناً: الـ Controllers

### `EmployeeController`

```csharp
public class EmployeeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public EmployeeController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
```

الكنترولر بيعمل حاجة واحدة: **بيستقبل Request من المتصفح ويرجع Response (صفحة أو Redirect)**.

**Index Action:**
```csharp
public IActionResult Index(string? searchItem)
{
    IEnumerable<Employee> employees;

    if (string.IsNullOrWhiteSpace(searchItem))
        employees = _unitOfWork.EmployeeRepository.GetAllEmpsWithDepartment();
    else
        employees = _unitOfWork.EmployeeRepository.Search(searchItem);

    ViewBag.SearchItem = searchItem;
    return View(employees);
}
```

- `string? searchItem` → البراميتر ده بييجي من الـ URL، مثلاً: `/Employee/Index?searchItem=أحمد`
- `string.IsNullOrWhiteSpace()` → بيشوف لو الـ string فاضية أو فيها مسافات بس
- `ViewBag.SearchItem = searchItem` → بيبعت قيمة البحث للـ View عشان يعرضها في الـ Input Field

**Create Action (GET):**
```csharp
[HttpGet]
public IActionResult Create()
{
    var departments = _unitOfWork.DepartmentRepository.GetAll();
    ViewBag.Departments = departments;
    return View();
}
```

- `[HttpGet]` → الـ Action ده بيتشتغل لما المتصفح يعمل GET Request (يعني لما تفتح الصفحة)
- بيجيب الأقسام من الداتابيز وبيبعتها للـ View عشان تتعبي في الـ Dropdown

**Create Action (POST):**
```csharp
[HttpPost]
public IActionResult Create(Employee employee)
{
    if (ModelState.IsValid)
    {
        _unitOfWork.EmployeeRepository.Add(employee);
        return RedirectToAction(nameof(Index));
    }

    ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
    return View(employee);
}
```

- `[HttpPost]` → بيتشتغل لما المستخدم يبعت الفورم (يضغط إضافة)
- `ModelState.IsValid` → بيتحقق إن البيانات صح (كل الـ Attributes زي `[Required]` و `[StringLength]` اتتحققت)
- لو صح: يضيف الموظف ويرجع للقائمة
- لو غلط: يرجع نفس الصفحة مع رسائل الخطأ (لازم تبعت الأقسام تاني عشان الـ ViewBag بيتمسح)

---

## تاسعاً: ViewBag vs ViewData vs TempData

دي 3 طرق لنقل البيانات من الـ Controller للـ View — وكل واحدة ليها حالة استخدام مختلفة.

---

### ViewBag

```csharp
// في الكنترولر
ViewBag.Departments = departments;
ViewBag.SearchItem = searchItem;
ViewBag.WelcomeMessage = "أهلاً بيك!";
```

```razor
// في الـ View
<p>@ViewBag.WelcomeMessage</p>
<select asp-items="@(new SelectList(ViewBag.Departments, "Id", "Name"))"></select>
```

**الخصائص:**
- **Dynamic** → مش محتاج تحدد النوع، تقدر تحط أي حاجة فيه
- **بيتعمل ومبيرجعش** — بييجي مع Request واحد بس، الـ Redirect بيمسحه
- مفيش IntelliSense — يعني لو غلطت في الاسم مش هيعرفلك في الكتابة
- ممكن يكون `null` لو الـ Property مش موجودة

**أمتى تستخدمه:** لما تبعت بيانات بسيطة من الكنترولر للـ View، مثل قوائم الـ Dropdowns أو رسائل بسيطة.

---

### ViewData

```csharp
// في الكنترولر
ViewData["Title"] = "صفحة الموظفين";
ViewData["Count"] = employees.Count();
```

```razor
// في الـ View
<h1>@ViewData["Title"]</h1>
<p>عدد الموظفين: @ViewData["Count"]</p>
```

**الخصائص:**
- بيشتغل بنفس طريقة الـ ViewBag، الفرق إنك بتحدد المفتاح بـ string
- لو المفتاح مش موجود بيرجع `null` من غير ما يعمل Error
- غالباً مستخدم في `_Layout.cshtml` عشان تتحكم في عنوان الصفحة

**الفرق بين ViewBag و ViewData:**

في الحقيقة، **هما نفس الحاجة** من جوا!
```
ViewBag.Title      ← نفسها ←  ViewData["Title"]
```
الـ ViewBag بيستخدم `dynamic` ويشيل منك علامة `[]` والـ string.

---

### TempData

```csharp
// في الكنترولر - قبل الـ Redirect
TempData["SuccessMessage"] = "تم إضافة الموظف بنجاح!";
return RedirectToAction("Index");
```

```razor
// في الـ View التانية - بعد الـ Redirect
@if (TempData["SuccessMessage"] != null)
{
    <div class="alert alert-success">
        @TempData["SuccessMessage"]
    </div>
}
```

**الخصائص:**
- **بيفضل موجود بعد Redirect** — ده الفرق الأساسي
- بيتخزن في الـ Session أو Cookie
- بيتمسح أوتوماتيك بعد ما يتقرأ مرة واحدة
- لو محتاجه يفضل موجود أكتر من Request، استخدم `TempData.Keep("SuccessMessage")`

**ليه محتاجه؟** لو عملت `Add` وبعدين عملت `Redirect`، الـ ViewBag هيتمسح. TempData هو الوحيد اللي بيعيش بعد الـ Redirect.

---

### مقارنة سريعة

| الميزة | ViewBag | ViewData | TempData |
|--------|---------|----------|----------|
| بيفضل بعد Redirect | ❌ | ❌ | ✅ |
| Type Safety | ❌ Dynamic | ❌ object | ❌ object |
| IntelliSense | ❌ | ❌ | ❌ |
| بيتخزن في | HTTP Request | HTTP Request | Session/Cookie |
| أمتى تستخدمه | بيانات للـ View | عنوان الصفحة | رسائل بعد Redirect |

---

## عاشراً: الـ Views (Razor Pages)

### Tag Helpers

**Tag Helpers** هي Keywords خاصة بتبدأ بـ `asp-` وبتخلي HTML أذكى.

```razor
<form asp-action="Create" asp-controller="Employee">
```
- `asp-action="Create"` → بيولد رابط الفورم تلقائياً زي `/Employee/Create`
- بدلها كنت هتكتب يدوياً: `action="/Employee/Create"` — والطريقة دي مش كويسة لو الـ URL اتغير

```razor
<a asp-controller="Employee" asp-action="Details" asp-route-id="@item.Id">
```
- `asp-route-id="@item.Id"` → بيضيف الـ id في الـ URL، النتيجة: `/Employee/Details/5`

```razor
<label asp-for="@Model.Name"></label>
<input asp-for="@Model.Name" class="form-control" />
<span asp-validation-for="@Model.Name" class="text-danger"></span>
```
- `asp-for` → بيربط الـ HTML Element بالـ Property في الـ Model
  - الـ `label` هياخد اسمه من `[Display(Name = "...")]`
  - الـ `input` هيحط قيمة الـ Property الحالية فيه
  - الـ `span` هيعرض رسالة الخطأ لو الـ Validation فشلت

```razor
<select asp-for="@Model.DepartmentId"
        asp-items="@(new SelectList(ViewBag.Departments, "Id", "Name"))">
    <option value="">-- اختر القسم --</option>
</select>
```
- `asp-items` → بيملي الـ Dropdown تلقائياً
- `SelectList(data, valueField, textField)`:
  - البيانات هي `ViewBag.Departments`
  - القيمة اللي بتتبعت مع الفورم هي `Id`
  - اللي المستخدم بيشوفه هو `Name`

---

### `@Html` Helpers

```razor
@Html.DisplayNameFor(Emp => Emp.Name)
```
- بيجيب الـ Display Name من الـ `[Display(Name = "...")]` Attribute
- مفيد في headers الجدول

```razor
@Html.DisplayNameFor(Emp => Emp.Department.Name)
```
- شايل إنه ينفذ `Include` — بس بيجيب اسم الحقل من الـ Model hierarchy

---

### `ModelState.IsValid`

لما المستخدم يبعت الفورم، الـ ASP.NET بيشيك على كل الـ Attributes في الـ Model:
- `[Required]` → هل الحقل موجود؟
- `[StringLength]` → هل الطول في الحدود؟
- `[Range]` → هل الرقم في النطاق؟
- `[EmailAddress]` → هل الإيميل صحيح؟

لو كلهم عدوا، `ModelState.IsValid` = `true`. لو فيه واحد فشل = `false`.

---

## حادي عشر: Unit of Work

### إيه المشكلة اللي بيحلها؟

تخيل إنك بتضيف موظف وبتحدث رصيده في نفس الوقت — عمليتين مختلفتين. لو الأولى نجحت والتانية فشلت، هيبقى عندك بيانات ناقصة.

**Unit of Work** بيضمن إن العمليات دي كلها تتعمل مع بعض، أو متتعملش خالص.

### الكود

```csharp
public class UnitOfWork : IUnitOfWork
{
    public IEmployeeRepository EmployeeRepository { get; set; }
    public IDepartmentRepository DepartmentRepository { get; set; }

    public UnitOfWork(IEmployeeRepository emp, IDepartmentRepository dept)
    {
        EmployeeRepository = emp;
        DepartmentRepository = dept;
    }
}
```

في الكنترولر:
```csharp
// بدل ما تعمل injection لكل repository
_unitOfWork.EmployeeRepository.GetAll();
_unitOfWork.DepartmentRepository.GetAll();
```

---

## ثاني عشر: تسجيل الـ Services في `Program.cs`

```csharp
// الـ DbContext
builder.Services.AddDbContext<MVCAppDbContext>(options =>
    options.UseSqlServer(connectionString));

// الـ Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// الـ Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```

**إيه معنى Scoped هنا؟**

كل Request (طلب من المتصفح) = نسخة جديدة من الـ Repository والـ DbContext. ده مهم عشان الـ DbContext مش Thread-Safe، يعني مش اتبنى يتشارك بين Requests مختلفة.

---

## خلاصة الفلوفة

```
المتصفح يبعت Request
    ↓
Controller يستقبله
    ↓
Controller يكلم UnitOfWork
    ↓
UnitOfWork يكلم Repository المناسب
    ↓
Repository يكلم DbContext
    ↓
DbContext يكلم قاعدة البيانات
    ↓
البيانات ترجع لـ Controller
    ↓
Controller يبعتها للـ View
    ↓
View تعرضها للمستخدم
```

كل طبقة مسؤولة عن حاجة واحدة بس — وده اللي بيخلي الكود سهل في التعديل والتطوير.