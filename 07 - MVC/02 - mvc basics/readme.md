# 🚀 ASP.NET Core MVC - Study Guide

> **ملاحظة:** الملف ده هيبقى مرجعك الأساسي لكل حاجة اتعلمتها في الـ MVC. اقرأه بتركيز وهترجع له كتير! 💪

---

## 📋 فهرس المحتويات

1. [هيكل المشروع - القديم والجديد](#هيكل-المشروع)
2. [Program.cs - قلب التطبيق](#programcs)
3. [الـ Middleware - الوسيط](#middleware)
4. [الـ Routing - التوجيه](#routing)
5. [الـ Controllers و Actions](#controllers-و-actions)
6. [الـ Action Results - أنواع الاستجابات](#action-results)
7. [الـ Views - الواجهات](#views)
8. [الـ Layout - القالب العام](#layout)
9. [الـ Tag Helpers](#tag-helpers)
10. [الـ ViewImports و ViewStart](#viewimports-و-viewstart)
11. [الـ Static Files و Bootstrap](#static-files-و-bootstrap)
12. [تثبيت Tailwind CSS](#تثبيت-tailwind-css)
13. [الـ Running Profiles - طرق التشغيل](#running-profiles)
14. [نصايح ومفاهيم مهمة](#نصايح-ومفاهيم-مهمة)

---

## هيكل المشروع

### الهيكل القديم (قبل .NET 6)

في الأول كان في **ملفين** بيشتغلوا مع بعض:

#### 📄 ملف `Startup.cs`
```csharp
// الملف ده كان بيتقسم لجزئين:

// الجزء الأول: بيسجّل الخدمات (Services)
public void ConfigureServices(IServiceCollection services)
{
    // هنا بتقول للتطبيق "هتحتاج إيه" زي: Controllers, Database, Auth...
}

// الجزء التاني: بيرتب الـ Middleware Pipeline
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
    app.UseRouting();
    app.UseEndpoints(endpoints => {
        endpoints.MapGet("/", async context => {
            await context.Response.WriteAsync("Hello World!");
        });
    });
}
```

#### 📄 ملف `Program.cs` القديم
```csharp
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>(); // بيشير لملف Startup
            });
}
```

**الخلاصة:** كانوا ملفين منفصلين، كل واحد بيعمل دوره.

---

### الهيكل الجديد (من .NET 6 فصاحاً)

مايكروسوفت **دمجت** الملفين في ملف واحد بس هو `Program.cs`:

```csharp
// بدل ما يبقى فيه ملف Startup منفصل
// كل حاجة دلوقتي في Program.cs
var builder = WebApplication.CreateBuilder(args);  // بديل CreateHostBuilder
builder.Services.AddControllersWithViews();        // بديل ConfigureServices
var app = builder.Build();                          // بديل .Build()

// الـ Middleware بيتحط هنا مباشرة
app.UseStaticFiles();
app.MapControllerRoute(...);
app.Run();
```

> **💡 نقطة مهمة:** لو حبيت تعمل ملف `Startup.cs` بإيدك في الهيكل الجديد، ممكن! بس مش ضروري. الهيكل الجديد أسهل وأوضح.

---

## Program.cs

### `WebApplication.CreateBuilder(args)` - بيعمل إيه بالظبط؟

```csharp
var builder = WebApplication.CreateBuilder(args);
```

الـ `CreateBuilder` ده بيعمل **حاجات كتير في الخلف**:

| الحاجة | الشرح |
|--------|-------|
| يقرأ `appsettings.json` | بيجيب الإعدادات من ملف الـ Settings |
| يقرأ Environment Variables | متغيرات البيئة زي (Development, Production) |
| يحضّر الـ Logging | نظام الـ Logs التلقائي |
| يحضّر الـ DI Container | نظام الـ Dependency Injection |
| يشغّل الـ Kestrel | السيرفر الداخلي للتطبيق |

**ببساطة:** كأنك بتقول للتطبيق "استعد وجهّز نفسك" وهو بيعمل كل حاجة تلقائياً.

---

### `builder.Services.AddControllersWithViews()`

```csharp
builder.Services.AddControllersWithViews();
```

السطر ده بيقول للتطبيق: **"أنا هستخدم MVC"** — يعني Controllers وViews سوا.

ده بيعمل إيه بالظبط؟
- بيسجّل كل الـ Controllers في التطبيق تلقائياً
- بيفعّل نظام الـ Views (Razor)
- بيفعّل الـ Tag Helpers
- بيفعّل الـ Model Binding والـ Validation

> **مقارنة سريعة:**
> - `AddControllers()` ← لو بتعمل API بس (بدون Views)
> - `AddRazorPages()` ← لو بتستخدم Razor Pages (مختلفة عن MVC)
> - `AddControllersWithViews()` ← لو بتعمل MVC كامل ✅

---

### `var app = builder.Build()`

```csharp
var app = builder.Build();
```

بعد ما سجّلنا كل الخدمات، السطر ده بيـ**بني** (Build) التطبيق فعلاً ويرجّعه في متغير اسمه `app`.

---

### `app.UseStaticFiles()`

```csharp
app.UseStaticFiles();
```

ده بيقول للتطبيق: **"اسمح بتقديم الملفات الساكنة"** من مجلد `wwwroot`.

الملفات الساكنة هي: CSS, JavaScript, Images, Fonts...

لو مش كتبت السطر ده، لو حاولت تفتح bootstrap.min.css هيجيلك **404 Not Found** رغم إن الملف موجود!

---

### `app.Run()`

```csharp
app.Run();
```

آخر سطر في التطبيق. ده بيقول **"ابدأ التشغيل وفضل شغّال"**.

> ⚠️ **مهم جداً:** أي كود بتكتبه **بعد** `app.Run()` مش هيتنفذ أبداً! لأن `Run` بيبلك (Block) وبيفضل شغال.

---

## Middleware

### إيه هو الـ Middleware؟

تخيل إن عندك **سلسلة من البوابات** قبل ما الـ Request يوصل للتطبيق:

```
Request جاي من المتصفح
         ↓
   [ Middleware 1 ]  ← مثلاً: Static Files
         ↓
   [ Middleware 2 ]  ← مثلاً: Authentication
         ↓
   [ Middleware 3 ]  ← مثلاً: Routing
         ↓
   [ Middleware 4 ]  ← مثلاً: Authorization
         ↓
      Controller/Endpoint
         ↓
   Response راجع للمتصفح
```

كل Middleware بياخد الـ Request، يعمل حاجة، وبعدين يبعته للي بعده أو يرجع Response.

### أنواع الـ Middleware الجاهزة

| Middleware | الوظيفة |
|-----------|---------|
| `UseStaticFiles()` | يقدّم الملفات من wwwroot |
| `UseRouting()` | يحدد إيه الـ Endpoint المناسب |
| `UseAuthentication()` | يتحقق "مين الـ User ده؟" |
| `UseAuthorization()` | يتحقق "مسموحله يدخل؟" |
| `UseDeveloperExceptionPage()` | يوري صفحة الخطأ التفصيلية في Development |
| `UseHttpsRedirection()` | يحوّل HTTP لـ HTTPS تلقائياً |

### ترتيب الـ Middleware مهم جداً! ⚠️

```csharp
// الترتيب الصح
app.UseStaticFiles();      // الأول: الملفات الساكنة
app.UseRouting();          // التاني: تحديد الـ Route
app.UseAuthentication();   // التالت: مين الـ User؟
app.UseAuthorization();    // الرابع: مسموحله؟
app.MapControllerRoute(...)// الأخير: وصّله للـ Controller
app.Run();
```

لو غيّرت الترتيب ممكن حاجات تتكسر! مثلاً لو حطيت `UseAuthorization` قبل `UseAuthentication`، التطبيق مش هيعرف يعمل Authorization لأنه لسه ماعرفش مين الـ User.

### عمل Middleware بإيدك (Custom Middleware)

```csharp
// الطريقة الأولى: app.Use
app.Use(async (context, next) =>
{
    // كود بيتنفذ قبل باقي الـ Middleware
    Console.WriteLine("Request جاي: " + context.Request.Path);
    
    await next(context); // بعته للـ Middleware اللي بعده
    
    // كود بيتنفذ بعد ما الـ Response اتعمل
    Console.WriteLine("Response راجع");
});

// الطريقة التانية: app.Run (بيوقف السلسلة - Terminal Middleware)
app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("الصفحة مش موجودة...");
});
// أي Middleware بعد Run مش هيتنفذ!
```

### الـ Endpoint في الـ Middleware

```csharp
// لازم تكون بعد UseRouting عشان تشتغل صح
app.UseRouting();

app.Use(async (context, next) =>
{
    Endpoint? endpoint = context.GetEndpoint(); // جيب الـ Endpoint الحالي
    if (endpoint != null)
        Console.WriteLine("الـ Endpoint: " + endpoint.DisplayName);
    
    await next(context);
});
```

> **💡 تجربة مهمة:** لو حطيت `GetEndpoint()` **قبل** `UseRouting()` هترجعلك `null` دايماً! لأن الـ Routing لسه ماحددش الـ Endpoint.

---

## Routing

### إيه هو الـ Routing؟

الـ Routing = **URL** + **HTTP Method** (GET, POST, PUT, DELETE, PATCH)

```
GET  https://localhost:7146/Home        ← جيب صفحة Home
POST https://localhost:7146/products    ← أنشئ Product جديد
GET  https://localhost:7146/products/1  ← جيب Product رقم 1
PUT  https://localhost:7146/products/1  ← عدّل Product رقم 1
DELETE https://localhost:7146/products/1 ← امسح Product رقم 1
```

### طرق الـ Routing

#### الطريقة الأولى: `app.MapGet`, `app.MapPost`, إلخ

```csharp
// GET Request على /Home
app.MapGet("/Home", async (context) =>
{
    await context.Response.WriteAsync("أهلاً بيك في الهوم!");
});

// POST Request على /Employee
app.MapPost("/Employee", async (context) =>
{
    await context.Response.WriteAsync("بيتعمل إضافة موظف");
});

// بطريقة أحدث بدون context
app.MapGet("/", () => "Hello World");
```

#### الطريقة التانية: `app.UseEndpoints` (الطريقة القديمة شوية)

```csharp
app.UseEndpoints(endpoint =>
{
    endpoint.Map("/Home", async (context) =>    // Map = بيشتغل مع أي HTTP Method
    {
        await context.Response.WriteAsync("الهوم بيج");
    });

    endpoint.MapGet("/Employee", async (context) =>   // MapGet = GET فقط
    {
        await context.Response.WriteAsync("صفحة الموظفين");
    });

    endpoint.MapPost("/Employee", async (context) =>  // MapPost = POST فقط
    {
        await context.Response.WriteAsync("إضافة موظف");
    });
});
```

**الفرق بين Map و MapGet:**
- `Map` → بيشتغل مع **أي** HTTP Method (GET, POST, DELETE...)
- `MapGet` → بيشتغل مع **GET فقط** (لو بعتله POST هيرجع 405 Method Not Allowed)
- `MapPost` → بيشتغل مع **POST فقط**

---

### الـ Route Parameters - متغيرات في الـ URL

#### الأنواع:

```
/Employee/123    ← هنا 123 هو الـ Parameter
```

| النوع | الشكل | المعنى |
|-------|-------|--------|
| Static | `/Home` | ثابت |
| Variable | `/Employee/{id}` | متغير إجباري |
| Optional | `/Products/{id?}` | متغير اختياري |
| Default | `/Product/{id=10}` | متغير بقيمة افتراضية |

```csharp
// متغير إجباري
endpoint.MapGet("/Employee/{id}", async (context) =>
{
    var id = context.Request.RouteValues["id"]; // جيب الـ id من الـ URL
    await context.Response.WriteAsync($"الموظف رقم {id}");
});

// متغيرات متعددة
endpoint.MapGet("/Product/{name}/{id}", async (context) =>
{
    var name = context.Request.RouteValues["name"];
    var id = context.Request.RouteValues["id"];
    await context.Response.WriteAsync($"المنتج: {name}, رقم: {id}");
});

// متغير اختياري
endpoint.MapGet("/Products/{id?}", async (context) =>
{
    var id = context.Request.RouteValues["id"]?.ToString();
    if (id != null)
        await context.Response.WriteAsync($"المنتج رقم {id}");
    else
        await context.Response.WriteAsync("كل المنتجات");
});

// قيمة افتراضية
endpoint.MapGet("/Product/{id=10}", async (context) =>
{
    var id = context.Request.RouteValues["id"]; // لو مفيش id في الـ URL هيبقى 10
    await context.Response.WriteAsync($"المنتج رقم {id}");
});
```

---

### الـ Route Constraints - قيود على المتغيرات

بدل ما تفضل بتتحقق من الـ id يدوياً، ممكن تحط شروط مباشرة في الـ URL Pattern:

```csharp
// {id:int} يعني الـ id لازم يكون رقم صحيح
endpoint.MapGet("/Employee/{id:int}", async (context) =>
{
    var id = context.Request.RouteValues["id"];
    await context.Response.WriteAsync($"الموظف رقم {id}");
});
// لو دخلت /Employee/abc هيرجع 404 مش هيشتغل أصلاً

// {name:alpha} يعني حروف بس (A-Z, a-z)
endpoint.MapGet("/Category/{name:alpha}", async (context) =>
{
    var name = context.Request.RouteValues["name"];
    await context.Response.WriteAsync($"القسم: {name}");
});
```

**أهم الـ Constraints المتاحة:**

| الـ Constraint | المعنى | مثال |
|---------------|--------|------|
| `int` | رقم صحيح | `{id:int}` |
| `long` | رقم كبير | `{id:long}` |
| `double` | رقم عشري | `{price:double}` |
| `bool` | true/false | `{active:bool}` |
| `alpha` | حروف فقط | `{name:alpha}` |
| `guid` | GUID format | `{id:guid}` |
| `minlength(n)` | أقل طول | `{name:minlength(3)}` |
| `maxlength(n)` | أقصى طول | `{name:maxlength(20)}` |
| `min(n)` | أقل قيمة | `{age:min(18)}` |
| `max(n)` | أقصى قيمة | `{age:max(100)}` |
| `range(n,m)` | نطاق | `{age:range(18,60)}` |
| `regex(expr)` | Regex | `{code:regex(^[A-Z]{{3}}$)}` |

**تركيب Constraints مع بعض:**

```csharp
// الـ id لازم int وأكبر من 0 وأصغر من 1000
endpoint.MapGet("/Product/{id:int:min(1):max(999)}", ...);
```

---

### الـ MapControllerRoute - طريقة الـ MVC

دي أهم طريقة في المشروع بتاعنا:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id:int?}",
    defaults: new { controller = "Home", action = "Index" },
    constraints: new { id = new IntRouteConstraint() }
);
```

**شرح كل Parameter:**

| الـ Parameter | الشرح |
|--------------|-------|
| `name: "default"` | اسم الـ Route (بيتستخدم لما بتعمل `RedirectToAction`) |
| `pattern` | شكل الـ URL المتوقع |
| `{controller=Home}` | اسم الـ Controller الافتراضي لو مش موجود في الـ URL |
| `{action=Index}` | اسم الـ Action الافتراضي |
| `{id:int?}` | متغير id اختياري من نوع int |
| `defaults` | قيم افتراضية تانية |
| `constraints` | قيود إضافية على المتغيرات |

**أمثلة على كيف بيشتغل الـ Pattern:**

```
/                       → HomeController.Index()
/Products               → ProductsController.Index()
/Products/Details       → ProductsController.Details()
/Products/Details/5     → ProductsController.Details(id: 5)
```

---

## Controllers و Actions

### إيه هو الـ Controller؟

الـ Controller هو **مجرد class عادي** بس لازم:
1. يورث من `Controller`
2. اسمه ينتهي بكلمة "Controller"

```csharp
public class HomeController : Controller  // ✅
{
    // هنا بنحط الـ Actions
}

public class Home : Controller  // ❌ اسمه مش بينتهي بـ Controller
{
}
```

### إيه هو الـ Action؟

الـ Action هو **method عام (public)** جوه الـ Controller بيستقبل الـ Request ويرجع Response.

```
URL: /Home/Index
       ↓     ↓
  Controller Action
```

### هيكل مجلدات المشروع

```
MVC Project
├── Controllers/
│   └── HomeController.cs
├── Models/
│   └── (البيانات والـ Logic)
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   ├── AboutUs.cshtml
│   │   ├── Privacy.cshtml
│   │   └── ContactUs.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       └── _ViewImports.cshtml
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
│       └── bootstrap/
└── Program.cs
```

### تغيير اسم الـ Action بالـ ActionName Attribute

```csharp
[ActionName("HomePage")]
public IActionResult Index()
{
    return View();
}
// دلوقتي الـ URL هيبقى /Home/HomePage مش /Home/Index
```

### تحديد HTTP Method للـ Action

```csharp
[HttpGet]    // ← ده الافتراضي، مش محتاج تكتبه
public IActionResult Index() { return View(); }

[HttpPost]   // ← بيستجيب لـ POST فقط
public IActionResult CreateEmployee() { return View(); }

[HttpPut]    // ← بيستجيب لـ PUT فقط
public IActionResult UpdateEmployee() { return View(); }

[HttpDelete] // ← بيستجيب لـ DELETE فقط
public IActionResult DeleteEmployee() { return View(); }
```

---

## Action Results

### ليه بنستخدم Action Results؟

الـ Action مش بترجع string عادي، بترجع نوع خاص من النتائج (Action Result) عشان الـ Framework يعرف يبني الـ HTTP Response الصح.

### ١. ContentResult - نص مباشر

```csharp
public ContentResult ContentResultAction()
{
    return Content("مرحبا إبراهيم");
}
```

زي الـ `console.log` في JavaScript، بتستخدمه للـ Testing والـ Debugging.

---

### ٢. ViewResult - ترجع View

```csharp
// بيدور على: Views/Home/ViewResultAction.cshtml
public ViewResult ViewResultAction()
{
    return View(); // الاسم التلقائي = اسم الـ Action
}

// بيدور على: Views/Home/TestView.cshtml
public ViewResult SpecificViewAction()
{
    return View("TestView"); // اسم View تاني
}
```

**كيف بيدور على الـ View؟**

ASP.NET بيقسّم اسم الـ Controller لجزئين:
- `HomeController` → `Home` + `Controller`
- بيدور في: `Views/Home/اسم_الـ_Action.cshtml`

---

### ٣. JsonResult - بيرجع JSON

```csharp
public JsonResult JsonResultAction()
{
    return Json(new
    {
        Id = 1,
        Name = "Ibrahim",
        Age = 22
    });
}
```

**الاستجابة:**
```json
{
    "id": 1,
    "name": "Ibrahim",
    "age": 22
}
```

بتستخدمه في:
- APIs
- AJAX Requests
- React / Angular apps

---

### ٤. RedirectResult - تحويل لـ URL

```csharp
public RedirectResult RedirectResultAction()
{
    return Redirect("https://google.com");
    // المتصفح هياخد HTTP 302 ويروح جوجل
}
```

---

### ٥. RedirectToActionResult - تحويل لـ Action تاني

```csharp
// تحويل لـ Action في نفس الـ Controller
public RedirectToActionResult GoHome()
{
    return RedirectToAction("Index");
}

// تحويل لـ Action في Controller تاني
public RedirectToActionResult GoToLogin()
{
    return RedirectToAction(
        actionName: "Login",
        controllerName: "Account"
    );
}
// URL النهائي: /Account/Login
```

---

### ٦. RedirectToRouteResult

```csharp
public RedirectToRouteResult RedirectToRouteResultAction()
{
    return RedirectToRoute(new
    {
        controller = "Home",
        action = "Index"
    });
}
```

أقل استخدام من `RedirectToAction` بس بتعمل نفس الحاجة.

---

### ٧. EmptyResult - مفيش رد

```csharp
public EmptyResult EmptyResultAction()
{
    return new EmptyResult();
    // الـ HTTP Response body هيكون فاضي
}
```

بيتستخدم لما بتعمل حاجة في الخلف ومحتاجش ترجع data.

---

### ٨. FileResult - ترجع ملف

```csharp
// فتح الملف في المتصفح
public FileResult OpenFile()
{
    return File(
        "/files/report.pdf",
        "application/pdf"
    );
}

// تحميل الملف (Download)
public FileResult DownloadFile()
{
    return File(
        "/files/report.pdf",
        "application/pdf",
        "MyReport.pdf" // اسم الملف لما ينزل
    );
}
```

**MIME Types شائعة:**

| النوع | MIME Type |
|-------|-----------|
| PDF | `application/pdf` |
| PNG | `image/png` |
| JPEG | `image/jpeg` |
| Excel | `application/vnd.ms-excel` |
| Word | `application/msword` |
| ZIP | `application/zip` |

---

### ٩. StatusCodeResult - HTTP Status Codes

```csharp
public NotFoundResult NotFoundAction()    => NotFound();   // 404
public BadRequestResult BadRequest()      => BadRequest(); // 400
public UnauthorizedResult Unauthorized()  => Unauthorized(); // 401
public OkResult OkAction()               => Ok();          // 200
```

**أهم الـ Status Codes:**

| الكود | المعنى |
|-------|--------|
| 200 | OK - تمام |
| 201 | Created - اتعمل |
| 301 | Moved Permanently - انتقل بشكل دائم |
| 302 | Found - موجود (Redirect مؤقت) |
| 400 | Bad Request - الـ Request غلط |
| 401 | Unauthorized - مش مسجّل دخول |
| 403 | Forbidden - مسجّل بس مش مسموحله |
| 404 | Not Found - مش موجود |
| 500 | Internal Server Error - خطأ في السيرفر |

---

### ١٠. PartialViewResult - View جزئي

```csharp
public PartialViewResult ShowUserCard()
{
    return PartialView("_UserCard");
    // بيدور على: Views/Home/_UserCard.cshtml
}
```

**إيه هو الـ Partial View؟**

Partial View هو **Component صغير قابل لإعادة الاستخدام** زي:
- Navbar
- Footer  
- Sidebar
- Product Card
- Comment Box

بيتعمل ملفات بيبدأ اسمها بـ Underscore `_` تقليدياً.

---

### ١١. ObjectResult

```csharp
public ObjectResult ObjectResultAction()
{
    return new ObjectResult(new { Id = 1, Name = "Ibrahim" });
    // ASP.NET بيحوّله JSON تلقائياً
}
```

---

### ١٢. IActionResult - الأهم والأشمل ⭐

```csharp
// بدل ما تحدد نوع معين، خلّي الـ Action يرجع IActionResult
// وبكده ممكن ترجع أي نوع
public IActionResult SmartAction(bool isLoggedIn)
{
    if (isLoggedIn)
    {
        return View();                // ممكن ترجع View
    }
    
    if (SomethingWentWrong)
    {
        return BadRequest();          // أو StatusCode
    }
    
    return RedirectToAction("Login"); // أو Redirect
}
```

**ليه IActionResult هو الأفضل؟** لأن Action واحد ممكن يحتاج يرجع حاجات مختلفة حسب الحالة. `IActionResult` بيديك الفلكسيبيلتي دي.

---

### ١٣. ActionResult<T> - للـ APIs

```csharp
public ActionResult<string> GetName()
{
    return "Ibrahim"; // ترجع data مباشرة
}

public ActionResult<List<string>> GetNames()
{
    var names = new List<string> { "Ahmed", "Ibrahim", "Sara" };
    
    if (names == null)
        return NotFound(); // أو ترجع Status Code
    
    return names; // أو ترجع البيانات
}
```

---

### شجرة الـ Action Results

```
ActionResult (الأساسي)
├── ViewResult         → View()
├── PartialViewResult  → PartialView()
├── JsonResult         → Json()
├── ContentResult      → Content()
├── RedirectResult     → Redirect()
├── RedirectToActionResult → RedirectToAction()
├── RedirectToRouteResult  → RedirectToRoute()
├── FileResult         → File()
├── EmptyResult        → new EmptyResult()
├── ObjectResult       → new ObjectResult()
└── StatusCodeResult
    ├── OkResult       → Ok()
    ├── NotFoundResult → NotFound()
    ├── BadRequestResult → BadRequest()
    └── UnauthorizedResult → Unauthorized()
```

---

## Views

### إيه هي الـ Views؟

الـ Views هي **صفحات HTML** بس بتقدر تحط فيها كود C# باستخدام **Razor Syntax**.

### ملفات `.cshtml`

- `cs` = C#
- `html` = HTML
- الاتنين مع بعض في ملف واحد!

### Razor Syntax الأساسي

```cshtml
@* ده كومنت في Razor (مش بيتعرضش في الـ HTML) *@

@{
    // هنا بتكتب كود C# جوه block
    var name = "Ibrahim";
    var age = 22;
}

@* بتعرض متغير بـ @ *@
<h1>مرحبا @name</h1>
<p>عمرك @age سنة</p>

@* if condition *@
@if (age >= 18)
{
    <p>كبير</p>
}
else
{
    <p>صغير</p>
}

@* for loop *@
@for (int i = 0; i < 5; i++)
{
    <p>السطر رقم @i</p>
}

@* foreach loop *@
@foreach (var item in list)
{
    <p>@item</p>
}
```

---

### أنواع الـ Comments في cshtml

```cshtml
@* Razor Comment - مش بيطلع في HTML Source *@

<!-- HTML Comment - بيطلع في HTML Source -->

@{
    // C# Comment جوه Code Block
    /* C# Multi-line Comment */
}
```

---

### تحديد الـ Layout للـ View

```cshtml
@{
    Layout = "_Layout"; // استخدم الـ Layout ده
}

<h1>محتوى الصفحة</h1>
```

---

### الـ ViewBag - تبعت Data للـ View

```csharp
// في الـ Controller
public IActionResult Index()
{
    ViewBag.Title = "الصفحة الرئيسية";
    ViewBag.UserName = "Ibrahim";
    return View();
}
```

```cshtml
@* في الـ View *@
<title>@ViewBag.Title</title>
<h1>مرحبا @ViewBag.UserName</h1>
```

**خصائص ViewBag:**
- Dynamic (مفيش Type Checking)
- بيتبعت من Controller للـ View
- مش بيتبعتش من View للـ Controller

---

## Layout

### إيه هو الـ Layout؟

اللايوت هو **القالب العام** للموقع. بدل ما تكرر الـ Header والـ Footer في كل صفحة، بتحطهم في ملف اللايوت مرة واحدة.

### إزاي تعمل Layout

1. اعمل مجلد `Shared` جوه مجلد `Views`
2. اعمل ملف `_Layout.cshtml` (أو أي اسم تاني)
3. حط فيه الـ HTML الأساسي

```cshtml
@* _Layout.cshtml *@
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>@ViewBag.Title</title>
    
    @* CSS يتحمل هنا *@
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css" />
    
    <style>
        h1 {
            font-size: 32px;
            width: fit-content;
            padding: 20px;
            margin: 20px auto;
            background-color: #dfd0d0;
            border-radius: 10px;
        }
    </style>
</head>
<body>
    @* الـ Header بيكون هنا وبيتكرر في كل الصفحات *@
    <header class="d-flex align-items-center flex-wrap justify-content-center gap-3">
        <a asp-controller="Home" asp-action="Index"     class="btn btn-primary"> Home      </a>
        <a asp-controller="Home" asp-action="AboutUs"   class="btn btn-warning"> AboutUs   </a>
        <a asp-controller="Home" asp-action="Privacy"   class="btn btn-danger "> Privacy   </a>
        <a asp-controller="Home" asp-action="ContactUs" class="btn btn-dark   "> ContactUs </a>
    </header>

    @* هنا بيتعرض محتوى الصفحة الفعلي *@
    <div>
        @RenderBody()
    </div>

    @* الـ Footer بيكون هنا *@
    <footer class="p-2 bg-black fixed-bottom">
        <p class="text-center text-white fw-bold">
            Copyrights &copy; Ibrahim Shafiq @DateTime.Now.Year
        </p>
    </footer>

    @* JavaScript يتحمل هنا (في الآخر دايما) *@
    <script src="~/lib/bootstrap/js/bootstrap.bundle.js"></script>
    <script src="~/lib/bootstrap/js/bootstrap.min.js"></script>
</body>
</html>
```

### `@RenderBody()` - قلب اللايوت

```cshtml
<div>
    @RenderBody()
</div>
```

ده المكان اللي بيتحط فيه محتوى كل صفحة. لو عندك Layout وفيه `@RenderBody()`، وعندك View بيقول `Layout = "_Layout"`:

- كل حاجة في اللايوت بتتعرض ثابتة
- المحتوى الخاص بالـ View بيتحط مكان `@RenderBody()`

---

### الـ View الـ Index ومشكلة التكرار

**قبل استخدام Layout (المشكلة):**

```cshtml
@* كل صفحة كان فيها الـ Header والـ Bootstrap كامل *@
<html>
<head>
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css">
</head>
<body>
    <header>
        <a href="/Home/Index" class="btn btn-primary">Home</a>
        <a href="/Home/AboutUs" class="btn btn-success">About</a>
        ...
    </header>
    <h1>Index Page</h1>
    <script src="~/lib/bootstrap/js/bootstrap.bundle.js"></script>
</body>
</html>
@* لو عندك 10 صفحات = 10 مرات نفس الـ Header! *@
```

**بعد استخدام Layout (الحل):**

```cshtml
@* Index.cshtml - بقت بسيطة جداً *@
@{
    Layout = "_Layout";
}

<h1>Home Index Page</h1>
```

---

## Tag Helpers

### ٣ طرق لعمل Links

#### الطريقة الأولى: HTML عادي (قديمة ومش كويسة)

```cshtml
<a href="/Home/Index" class="btn btn-primary">Home</a>
```

**المشكلة:** لو غيّرت اسم الـ Controller أو الـ Action، هتضطر تغير كل الـ Links يدوياً!

---

#### الطريقة التانية: `Html.ActionLink` (أحسن بس verbose)

```cshtml
@* @Html.ActionLink(نص_الرابط, اسم_الـ_Action, اسم_الـ_Controller, route_params, html_attributes) *@
@Html.ActionLink("Home",     "Index",   "Home", null, new { @class = "btn btn-primary" })
@Html.ActionLink("AboutUs",  "AboutUs", "Home", null, new { @class = "btn btn-warning" })
```

ملاحظة: `@class` بدل `class` عشان `class` كلمة محجوزة في C#.

---

#### الطريقة التالتة: Tag Helpers (الأحدث والأفضل) ⭐

```cshtml
<a asp-controller="Home" asp-action="Index"     class="btn btn-primary"> Home      </a>
<a asp-controller="Home" asp-action="AboutUs"   class="btn btn-warning"> About Us  </a>
<a asp-controller="Home" asp-action="Privacy"   class="btn btn-danger "> Privacy   </a>
<a asp-controller="Home" asp-action="ContactUs" class="btn btn-dark   "> Contact   </a>
```

**مزايا Tag Helpers:**
- أقرب لـ HTML الطبيعي
- IntelliSense في Visual Studio بيساعدك
- لو غيّرت الـ Route، الروابط بتتغير تلقائياً
- أسهل في القراءة

**Tag Helpers تانية مفيدة:**

```cshtml
@* Form Submit *@
<form asp-controller="Account" asp-action="Login" method="post">
    <input asp-for="Email" class="form-control" />
    <span asp-validation-for="Email" class="text-danger"></span>
    <button type="submit">Login</button>
</form>

@* Image *@
<img asp-append-version="true" src="~/images/logo.png" />
@* بيضيف version query string تلقائياً عشان يتجنب الـ Cache *@

@* Script Minified *@
<environment include="Production">
    <script src="~/js/app.min.js"></script>
</environment>
<environment exclude="Production">
    <script src="~/js/app.js"></script>
</environment>
```

---

### تفعيل Tag Helpers

عشان Tag Helpers تشتغل لازم تكتب في أول الملف:

```cshtml
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

بس بدل ما تكتبها في كل ملف، في طريقة أذكى!

---

## ViewImports و ViewStart

### `_ViewImports.cshtml`

بتحطه في مجلد `Views` مباشرة. كل حاجة فيه **بتتطبق تلقائياً على كل الـ Views** بدون ما تكتبها في كل ملف.

```cshtml
@* Views/_ViewImports.cshtml *@

@* بدل ما تكتب ده في كل صفحة *@
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

@* ممكن تضيف Using Statements كمان *@
@using MVC02.Models
@using MVC02.ViewModels
```

**قبل `_ViewImports`:**
```
Index.cshtml       → فيه @addTagHelper
AboutUs.cshtml     → فيه @addTagHelper
Privacy.cshtml     → فيه @addTagHelper
ContactUs.cshtml   → فيه @addTagHelper
```

**بعد `_ViewImports`:**
```
_ViewImports.cshtml → فيه @addTagHelper (مرة واحدة بس!)
Index.cshtml        → نظيف
AboutUs.cshtml      → نظيف
Privacy.cshtml      → نظيف
ContactUs.cshtml    → نظيف
```

---

### `_ViewStart.cshtml`

بتحطه في مجلد `Views`. بيحدد الـ **Layout الافتراضي** لكل الـ Views.

```cshtml
@* Views/_ViewStart.cshtml *@
@{
    Layout = "_Layout";
}
```

**قبل `_ViewStart`:**
```cshtml
@* كل View كانت بتحدد الـ Layout يدوياً *@
@{
    Layout = "_Layout";
}
<h1>Index</h1>
```

**بعد `_ViewStart`:**
```cshtml
@* الـ View بقت بسيطة *@
<h1>Index</h1>
@* الـ Layout بيتحدد تلقائياً من _ViewStart *@
```

**لو صفحة معينة مش عايزاش تستخدم الـ Layout:**

```cshtml
@{
    Layout = null; // بيلغي الـ Layout لهذه الصفحة بس
}
```

---

## Static Files و Bootstrap

### إيه هي الـ Static Files؟

الملفات الساكنة (Static Files) هي الملفات اللي **مش بتتغير** في الـ Server. بتتبعت كما هي للـ Browser:
- CSS files
- JavaScript files
- Images
- Fonts
- HTML files

### مجلد `wwwroot`

ده المجلد الرسمي للـ Static Files في ASP.NET Core. كل حاجة فيه **متاحة للـ Browser** مباشرة.

```
wwwroot/
├── css/
│   └── site.css
├── js/
│   └── site.js
├── lib/
│   └── bootstrap/
│       ├── css/
│       │   └── bootstrap.min.css
│       └── js/
│           ├── bootstrap.bundle.js
│           └── bootstrap.min.js
└── images/
    └── logo.png
```

لو عندك ملف في `wwwroot/images/logo.png`، الـ URL بتاعه هيكون:
```
https://localhost:7146/images/logo.png
```

### تفعيل الـ Static Files

```csharp
// في Program.cs
app.UseStaticFiles(); // مش هتشتغل Static Files من غير السطر ده!
```

### الـ Tilde (~) في الـ Paths

```cshtml
<link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css" />
```

علامة `~` بتشير لمجلد `wwwroot` تلقائياً. بدلها ممكن تكتب:
```cshtml
<link rel="stylesheet" href="/lib/bootstrap/css/bootstrap.min.css" />
```

---

### تثبيت Bootstrap عبر Client-Side Library

1. **كليك يمين على `wwwroot`**
2. اختر **"Add"**
3. اختر **"Client-Side Library"**
4. في الـ Provider اختر **`cdnjs`**
5. في الـ Search Box اكتب **`bootstrap`**
6. اختر الإصدار اللي عايزه
7. اختر الملفات اللي تحتاجها:
   - `bootstrap.min.css` → حطه في `wwwroot/lib/bootstrap/css/`
   - `bootstrap.bundle.min.js` → حطه في `wwwroot/lib/bootstrap/js/`
   - `bootstrap.min.js` → حطه في `wwwroot/lib/bootstrap/js/`

**ملاحظة:** `bootstrap.bundle.js` بتضمن Popper.js اللي بتحتاجه الـ Dropdowns والـ Tooltips. لو هتستخدم واحد بس خد الـ Bundle.

---

## تثبيت Tailwind CSS

### الطريقة الأولى: عبر CDN (للتجربة السريعة فقط)

في الـ Layout أو الـ View حط في الـ `<head>`:

```cshtml
<script src="https://cdn.tailwindcss.com"></script>
```

> ⚠️ **تحذير:** CDN مش للـ Production! بس للتجربة والتعلم.

---

### الطريقة التانية: تثبيت Tailwind بشكل صحيح (NPM)

#### الخطوات:

**الخطوة 1:** تأكد إن عندك Node.js مثبّت:
```bash
node --version
npm --version
```

**الخطوة 2:** في مجلد المشروع، افتح Terminal وشغّل:
```bash
npm init -y
npm install -D tailwindcss
npx tailwindcss init
```

**الخطوة 3:** سيتعمل ملف `tailwind.config.js`، عدّله كده:
```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",     // كل ملفات Views
    "./Pages/**/*.cshtml",     // لو عندك Razor Pages
    "./wwwroot/**/*.html",     // أي HTML في wwwroot
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

**الخطوة 4:** اعمل ملف CSS جديد في `wwwroot/css/input.css`:
```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

**الخطوة 5:** في `package.json` أضف هذا الـ Script:
```json
{
  "scripts": {
    "tailwind": "npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/tailwind.css --watch"
  }
}
```

**الخطوة 6:** شغّل الأمر ده في Terminal:
```bash
npm run tailwind
```

ده هيعمل ملف `tailwind.css` في `wwwroot/css/` وهيبقى بيتحدث تلقائياً لما تعدّل.

**الخطوة 7:** في الـ Layout أضف:
```cshtml
<link rel="stylesheet" href="~/css/tailwind.css" />
```

---

### الفرق بين Bootstrap و Tailwind

| | Bootstrap | Tailwind |
|--|-----------|---------|
| الأسلوب | Component-based | Utility-first |
| السهولة | أسهل للمبتدئين | بيحتاج ممارسة |
| الحجم | أكبر | أصغر (بعد الـ Build) |
| التخصيص | محدود | لا نهائي |
| مثال | `class="btn btn-primary"` | `class="bg-blue-500 text-white px-4 py-2 rounded"` |

---

## Running Profiles

### طرق تشغيل المشروع

في Visual Studio في أعلى الشريط هتلاقي زر التشغيل وجنبه سهم للأسفل. لو ضغطته هتلاقي:
- **https** (أو http) ← ده Kestrel (السيرفر الداخلي)
- **IIS Express** ← ده IIS بنسخته المصغرة

### ملف `launchSettings.json`

هتلاقيه في مجلد `Properties` في المشروع:

```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "dotnetRunMessages": true,
      "applicationUrl": "http://localhost:5094"
    },
    "https": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "dotnetRunMessages": true,
      "applicationUrl": "https://localhost:7146;http://localhost:5094"
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  },
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:50591/",
      "sslPort": 44381
    }
  }
}
```

### الـ Environments

```
ASPNETCORE_ENVIRONMENT = Development  ← بيوري صفحات الأخطاء التفصيلية
ASPNETCORE_ENVIRONMENT = Staging      ← مرحلة ما قبل الـ Production
ASPNETCORE_ENVIRONMENT = Production   ← بيخفي تفاصيل الأخطاء
```

في `Program.cs`:
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // بيوري تفاصيل الخطأ
}
else
{
    app.UseExceptionHandler("/Home/Error"); // بيوجه لصفحة خطأ عامة
}
```

---

### Kestrel vs IIS Express

| | Kestrel | IIS Express |
|--|---------|-------------|
| النوع | Cross-platform | Windows فقط |
| السرعة | أسرع | أبطأ قليلاً |
| الاستخدام | Development & Production | Development بس |
| الـ Port | تحدده انت | بيتحدد تلقائياً |

---

## نصايح ومفاهيم مهمة

### ✅ Best Practices

```csharp
// 1. استخدم IActionResult دايماً (أمرن وأشمل)
public IActionResult MyAction()
{
    return View(); // أو Json() أو Redirect() إلخ
}

// 2. خلّي أسماء الـ Views تطابق أسماء الـ Actions
public IActionResult AboutUs() // الـ View: Views/Home/AboutUs.cshtml
{
    return View();
}

// 3. استخدم Tag Helpers بدل HTML عادي في الـ Links
<a asp-controller="Home" asp-action="Index">Home</a> // ✅
<a href="/Home/Index">Home</a>                        // ❌

// 4. حط JavaScript في آخر الـ Body مش في الـ Head
<body>
    <!-- محتوى الصفحة -->
    <script src="~/js/script.js"></script> <!-- في الآخر ✅ -->
</body>
```

### ⚠️ أخطاء شائعة

```
❌ نسيت UseStaticFiles() → Bootstrap مش بيشتغل
❌ الـ View اسمه مختلف عن الـ Action → 500 Internal Server Error
❌ نسيت AddControllersWithViews() → Controllers مش بتشتغل
❌ الـ Controller مش بيورث من Controller → مش هتشتغل
❌ اسم الـ Controller مش بينتهي بـ "Controller" → مش هيتعرف
```

### 🔑 نقاط مهمة للذاكرة

```
✅ URL Pattern:  {controller}/{action}/{id?}
✅ Views Path:   Views/ControllerName/ActionName.cshtml
✅ Layout Path:  Views/Shared/_Layout.cshtml
✅ Imports Path: Views/_ViewImports.cshtml
✅ ViewStart:    Views/_ViewStart.cshtml
✅ wwwroot:      الملفات الساكنة (CSS, JS, Images)
```

### 🚀 تدفق الـ Request في MVC

```
Browser يبعت Request
        ↓
   Middleware Pipeline
   (Static Files, Routing, Auth...)
        ↓
   Router بيحدد الـ Controller والـ Action
        ↓
   Controller.Action() بيتنفذ
        ↓
   Action بيرجع ActionResult
        ↓
   View Engine بيبني الـ HTML
        ↓
   Response يرجع للـ Browser
```

---

### الفرق بين Results.Text و context.Response.WriteAsync

```csharp
// الطريقة القديمة
endpoint.MapGet("/test", async (context) =>
{
    await context.Response.WriteAsync("Hello World");
    // بتكتب على الـ Response Stream مباشرة
    // أقل تنظيماً
});

// الطريقة الحديثة (Minimal APIs)
app.MapGet("/test", async (int id) =>
{
    return Results.Text($"Hello {id}");
    // Results.Text بيعمل IResult object
    // الـ Framework هو اللي بيكتب على الـ Response
    // أكثر تنظيماً وأسهل للـ Testing
});
```

---

### Task و async/await - فاهمهم صح

```csharp
// Task = عملية هتتنفذ في المستقبل (Asynchronous)
// async = بيقول للـ Method إنها Asynchronous
// await = "استنّي الـ Task ده خلص وكمّل"

// مثال: قراءة من Database (عملية بطيئة)
public async Task<IActionResult> GetProducts()
{
    var products = await _db.Products.ToListAsync(); // استنّي البيانات
    return View(products);
    // ← الـ Thread مش بيتعطل أثناء الانتظار
    // بيتحرر يخدم Requests تانية
}

// مقارنة مع Thread.Sleep (الطريقة الغلط)
Thread.Sleep(2000); // بيعطّل الـ Thread كله! ❌

// الطريقة الصح
await Task.Delay(2000); // بيستنى من غير ما يعطّل ✅
```

---

## سؤال وجواب سريع

**سؤال:** إيه الفرق بين `Redirect` و `RedirectToAction`؟
> `Redirect` بيروح لأي URL (حتى Google). `RedirectToAction` بيروح لـ Action في نفس التطبيق بشكل أذكى (بيبني الـ URL تلقائياً من اسم الـ Controller والـ Action).

**سؤال:** لما بكتب `return View()` بيدور على الـ View فين بالظبط؟
> بيدور في: `Views/{ControllerName}/{ActionName}.cshtml`، ولو مش لقاه بيدور في `Views/Shared/{ActionName}.cshtml`

**سؤال:** إيه الفرق بين `_ViewImports.cshtml` و `_ViewStart.cshtml`؟
> `_ViewImports` للـ Namespaces والـ Tag Helpers المشتركة. `_ViewStart` لتحديد الـ Layout الافتراضي.

**سؤال:** ليه أسماء الـ Partial Views بتبدأ بـ Underscore `_`؟
> ده Convention بس (اتفاق)، مش إجباري. بس ASP.NET بيستخدمه عشان يميّز الـ Partial Views عن الـ Full Views.

**سؤال:** إيه الفرق بين `Map` و `MapGet`؟
> `Map` بيقبل **أي** HTTP Method. `MapGet` بيقبل **GET فقط**، لو جه Request تاني هيرجع 405 Method Not Allowed.

---

> 💪 **خلّص هنا؟** كده انت فاهم MVC كويس جداً! الخطوة الجاية: ابدأ تجرب وتبني Views وControllers حقيقية، واستخدم الـ Model Binding وتبعت Data من Controller للـ View. 🚀