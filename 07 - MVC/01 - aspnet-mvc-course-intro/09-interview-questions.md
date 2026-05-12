# 09 — أسئلة الـ Interview 💼

> **"الـ Interview مش امتحان حفظ — هو امتحان فهم. افهم المبادئ وهتعدي أي سؤال!"**

---

## 📋 المستويات

- [🟢 Junior Level](#-junior-level-سهل)
- [🟡 Mid Level](#-mid-level-متوسط)
- [🔴 Senior Level](#-senior-level-صعب)
- [🎯 Scenario-based Questions](#-scenario-based-questions)
- [💡 نصائح للـ Interview](#-نصائح-للـ-interview)

---

## 🟢 Junior Level (سهل)

### ASP.NET & MVC

**Q1: إيه هو ASP.NET Core؟**
> ASP.NET Core هو Framework مفتوح المصدر من Microsoft لبناء Web Applications وAPIs باستخدام C#. هو Cross-Platform (Windows/Linux/Mac)، سريع جداً، وبيدعم Dependency Injection من الأساس. ده الجيل الجديد من ASP.NET Framework اللي كان Windows-only.

**Q2: إيه هو MVC؟**
> MVC هو Architectural Pattern بيقسم الـ Application لـ 3 أجزاء:
> - **Model**: البيانات والـ Business Logic
> - **View**: الواجهة والـ HTML
> - **Controller**: بيستقبل الـ Requests، يتصل بالـ Model، ويبعت البيانات للـ View
>
> الهدف هو Separation of Concerns — كل جزء عنده مسؤولية واحدة.

**Q3: إيه هو الـ Routing في ASP.NET Core؟**
> Routing هو عملية تحديد أي Controller وأي Action تتنفذ بناءاً على الـ URL. في نوعين: Convention-based (Pattern محدد مسبقاً) وAttribute Routing (بتحدد الـ Route على كل Action).

**Q4: إيه الفرق بين ViewBag وViewData؟**
> كلاهم بيبعتوا Data من Controller للـ View في نفس الـ Request:
> - **ViewData**: Dictionary (`ViewData["key"] = value`) - محتاج Casting
> - **ViewBag**: dynamic wrapper فوق ViewData (`ViewBag.key = value`) - أسهل بس مش Type-Safe
> - كلاهم بيضيع بعد Redirect

**Q5: إيه هو TempData ومتى بتستخدمه؟**
> TempData بيحتفظ بالبيانات لحد الـ Request الجاي حتى بعد Redirect. بنستخدمه كتير لعرض رسائل النجاح أو الخطأ بعد عملية ما. بيتخزن في Session افتراضياً.

**Q6: إيه هو الـ Razor؟**
> Razor هو Template Engine بيخليك تدمج C# Code داخل HTML في ملفات `.cshtml`. الـ `@` هو علامة الـ Razor Code. بيعمل HTML Encoding تلقائياً للحماية من XSS.

**Q7: إيه هو الـ wwwroot folder؟**
> هو المجلد اللي فيه الـ Static Files (CSS, JavaScript, Images, Fonts). المتصفح بيوصله مباشرة بدون ما يمر على الـ Controller. ده Root الـ Web Server.

**Q8: إيه هو HTTP Request وHTTP Response؟**
> HTTP Request هو الرسالة اللي المتصفح بيبعتها للـ Server (بتحتوي Method, URL, Headers, Body). HTTP Response هو الرد بتاع الـ Server (بيحتوي Status Code, Headers, Body).

**Q9: إيه أهم HTTP Status Codes اللي لازم تعرفها؟**
> - **200 OK**: نجاح
> - **201 Created**: تم الإنشاء
> - **301/302**: Redirect
> - **400 Bad Request**: الطلب غلط
> - **401 Unauthorized**: مش Logged In
> - **403 Forbidden**: Logged In بس مش عندك صلاحية
> - **404 Not Found**: مش موجود
> - **500 Internal Server Error**: خطأ في الـ Server

**Q10: إيه الفرق بين GET وPOST؟**
> GET لجيب بيانات — البيانات في الـ URL، Safe وIdempotent. POST لإرسال بيانات — البيانات في الـ Body، مش Safe. GET ما فيهوش Body. POST الأنسب للـ Forms والبيانات الحساسة.

---

## 🟡 Mid Level (متوسط)

### Dependency Injection

**Q11: إيه هو Dependency Injection وليه مهم؟**
> DI هو Pattern بيديك فيه الـ Dependencies (الـ Services) من الخارج بدل ما تعملها بإيدك. بدل `new ProductService()` في الـ Controller، بتطلبها في الـ Constructor وASP.NET بيوفرها تلقائياً.
>
> الفوائد: Loose Coupling، سهل Testing، سهل تغيير الـ Implementation بدون تعديل الـ Consumer.

**Q12: إيه الفرق بين Singleton وScoped وTransient؟**
> - **Singleton**: Object واحد للـ App كلها من البداية للنهاية — مناسب للـ Configuration والـ Logging.
> - **Scoped**: Object جديد لكل HTTP Request — مناسب للـ DbContext والـ Services.
> - **Transient**: Object جديد في كل مرة تطلبه — مناسب للـ Stateless Services الخفيفة.
>
> ⚠️ خطر: لو حقنت Scoped في Singleton = Captive Dependency Problem!

**Q13: إيه هو الـ Middleware Pipeline؟**
> هو سلسلة من الـ Components اللي كل Request بيمر عليها بالترتيب. كل Middleware ممكن يعالج الـ Request وإما يبعته للـ Middleware الجاي أو يوقفه. الترتيب مهم جداً — مثلاً Authentication لازم قبل Authorization.

**Q14: إزاي بتعمل Custom Middleware؟**
```csharp
// طريقتين:

// 1. Inline
app.Use(async (context, next) => {
    // قبل
    await next();
    // بعد
});

// 2. Class
public class MyMiddleware {
    private readonly RequestDelegate _next;
    public MyMiddleware(RequestDelegate next) => _next = next;
    
    public async Task InvokeAsync(HttpContext context) {
        // logic
        await _next(context);
    }
}
app.UseMiddleware<MyMiddleware>();
```

**Q15: إيه هو Entity Framework Core؟**
> EF Core هو ORM (Object-Relational Mapper) بيخليك تتعامل مع الـ Database باستخدام C# Objects بدل SQL مباشرة. بيحول الـ LINQ Queries لـ SQL تلقائياً. فيه Code-First (بتكتب Models وبيولد Database) وDatabase-First.

**Q16: إيه هو CSRF وإزاي ASP.NET بيحمي منه؟**
> CSRF = Cross-Site Request Forgery. هجوم بيخلي المتصفح يبعت Request مزيف من موقع تاني. ASP.NET بيحمي منه عن طريق Anti-Forgery Token — بيولد Token سري في الـ Form وبيتحقق منه في كل POST Request. بتضيف `[ValidateAntiForgeryToken]` على الـ POST Actions.

**Q17: إيه هو الـ Action Filter؟**
> Action Filter هو Code بيتنفذ قبل أو بعد الـ Action Method. بنستخدمه لـ: Logging، Caching، Validation، Error Handling. بيطبق DRY Principle — كود مشترك بين أكتر من Action.

```csharp
public class LogTimeFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["StartTime"] = DateTime.Now;
    }
    
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var start = (DateTime)context.HttpContext.Items["StartTime"];
        var elapsed = DateTime.Now - start;
        Console.WriteLine($"Action took: {elapsed.TotalMilliseconds}ms");
    }
}
```

**Q18: إيه الفرق بين Authentication وAuthorization؟**
> Authentication: التحقق من هوية المستخدم ("مين أنت؟") — Login بـ Username/Password أو JWT.
> Authorization: التحقق من الصلاحيات ("هل تقدر تعمل ده؟") — Roles, Policies, Claims.
> Authentication لازم يحصل قبل Authorization.

**Q19: إيه هو الـ ViewModel وليه بنستخدمه؟**
> ViewModel هو Class مصمم خصيصاً للـ View — بيحتوي بس على البيانات اللي الـ View محتاجها. الفوائد:
> 1. أمان — مش بنكشف Database Models مباشرة
> 2. ممكن ندمج بيانات من أكتر من Model
> 3. بنضيف Validation خاص بالـ UI
> 4. Over-posting Protection

**Q20: إيه هو Over-posting وإزاي بتحمي منه؟**
> Over-posting هو لما المستخدم بيبعت Fields في الـ Form مش المفروض يعدلها (مثلاً IsAdmin = true). الحل: استخدام ViewModels ومش Bind الـ Database Models مباشرة. أو استخدام `[Bind]` Attribute لتحديد الـ Fields المسموح بيها.

---

## 🔴 Senior Level (صعب)

**Q21: إيه هو CORS وإزاي بتعمله Configure؟**
> CORS = Cross-Origin Resource Sharing. Browser Security Policy بتمنع JavaScript من طلب Resources من Domain مختلف. الـ Server لازم يرد بـ CORS Headers للسماح.
>
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("https://myapp.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});
app.UseCors("AllowFrontend");
```

**Q22: إيه هو JWT وإزاي بيشتغل؟**
> JWT = JSON Web Token. هو Token مشفر بيحتوي على Claims (بيانات المستخدم). Structure: `Header.Payload.Signature`
>
> Flow: Login → Server يولد JWT → Client يحتفظ بيه → Client يبعته في كل Request في Header: `Authorization: Bearer {token}` → Server يتحقق من الـ Signature.
>
> مميز للـ APIs والـ Microservices — Stateless (مش محتاج Sessions).

**Q23: إيه هو Clean Architecture وليه مهم؟**
> Clean Architecture هو تنظيم الكود في Layers منفصلة:
> - **Domain Layer**: Entities, Business Rules (مش بيعتمد على حاجة)
> - **Application Layer**: Use Cases, Interfaces
> - **Infrastructure Layer**: Database, External APIs
> - **Presentation Layer**: Controllers, Views
>
> الـ Dependency Rule: الـ Inner Layers مش بتعرف حاجة عن الـ Outer Layers.

**Q24: إيه الفرق بين IQueryable وIEnumerable مع EF Core؟**
> - **IEnumerable**: بيجيب كل البيانات من الـ Database وبعدين بيعمل Filtering في الـ Memory — بطيء وأكل Memory.
> - **IQueryable**: بيبني الـ Query ومش بيتنفذ إلا لما تطلبه (Deferred Execution) — الـ Filtering بيحصل في الـ Database — أسرع بكتير.
>
```csharp
// IQueryable - SQL: SELECT * FROM Products WHERE Price > 100
var products = _db.Products.Where(p => p.Price > 100).ToList();

// IEnumerable - SELECT كل حاجة ثم Filter في Memory
var products = _db.Products.AsEnumerable().Where(p => p.Price > 100).ToList();
```

**Q25: إزاي بتتعامل مع Concurrency في EF Core؟**
> Optimistic Concurrency: بتضيف `[ConcurrencyToken]` أو `RowVersion` على الـ Model. لما اتنين users يعدلوا نفس الـ Record في نفس الوقت، EF Core بيعمل Exception وانت بتقرر تعمل إيه.
>
```csharp
public class Product
{
    public int Id { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } // SQL Server Timestamp
}

// في Controller
try { await _db.SaveChangesAsync(); }
catch (DbUpdateConcurrencyException ex)
{
    // Handle conflict
    var entry = ex.Entries.Single();
    // Resolve conflict...
}
```

**Q26: إيه هو Response Caching وOutput Caching؟**
> Response Caching: بيضيف Cache Headers للـ Response عشان الـ Browser أو الـ Intermediate Proxies يحفظوا الـ Response.
> Output Caching: الـ Server نفسه بيحفظ الـ Response ويرده لـ Requests مشابهة بدون تنفيذ الـ Action.
>
```csharp
// Response Caching
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
public IActionResult Index() { ... }

// Output Caching (.NET 7+)
app.UseOutputCache();
[OutputCache(Duration = 60)]
public IActionResult Index() { ... }
```

**Q27: إيه هو N+1 Problem وإزاي بتحله؟**
> N+1 Problem: لما بتجيب List من الـ Database وبعدين لكل Item بتعمل Query منفصل لجيب Related Data = N Queries زيادة.
>
```csharp
// ❌ N+1 Problem
var posts = await _db.Posts.ToListAsync();
foreach (var post in posts)
{
    // Query لكل Post! = N Queries إضافية
    var author = await _db.Users.FindAsync(post.AuthorId);
}

// ✅ الحل: Eager Loading
var posts = await _db.Posts
    .Include(p => p.Author)  // واحد Query بدل N+1
    .ToListAsync();
```

---

## 🎯 Scenario-based Questions

**Q28: عندك API بيستقبل Requests كتير جداً وبيهنج. إيه اللي هتعمله؟**
> 1. **Profiling**: أعرف أين Bottleneck (Database Queries؟ Memory؟ CPU؟)
> 2. **Async/Await**: تأكد كل الـ I/O Operations async
> 3. **Database**: Indexes مناسبة، Avoid N+1، استخدم Pagination
> 4. **Caching**: Cache الـ Responses المتكررة (Redis/Memory Cache)
> 5. **Connection Pooling**: تأكد EF Core بيستخدم Connection Pooling
> 6. **Load Balancing**: وزع الـ Load على أكتر من Instance
> 7. **Rate Limiting**: امنع الـ Abuse

**Q29: كيف تأمّن الـ ASP.NET Core API؟**
> 1. **HTTPS فقط** — UseHsts + UseHttpsRedirection
> 2. **JWT Authentication** — Validate Token في كل Request
> 3. **Authorization Policies** — Role-based أو Claim-based
> 4. **Input Validation** — FluentValidation أو Data Annotations
> 5. **SQL Injection Prevention** — استخدم Parameterized Queries (EF Core بيعملها تلقائياً)
> 6. **CORS** — سمح بس للـ Origins المعروفة
> 7. **Rate Limiting** — امنع Brute Force
> 8. **Security Headers** — X-Frame-Options, CSP, etc.
> 9. **Secrets Management** — Azure Key Vault أو User Secrets
> 10. **Audit Logging** — سجل كل العمليات الحساسة

**Q30: إزاي بتعمل Unit Testing لـ Controller في ASP.NET Core؟**
```csharp
// باستخدام xUnit + Moq

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockService = new Mock<IProductService>();
        _controller = new ProductsController(_mockService.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewWithProducts()
    {
        // Arrange
        var products = new List<Product> { new() { Id = 1, Name = "Test" } };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<Product>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Details_WithInvalidId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Product?)null);

        var result = await _controller.Details(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
```

---

## 💡 نصائح للـ Interview

### قبل الـ Interview

```
✅ راجع الـ Basics كويس (HTTP, MVC, C#)
✅ اعمل مشروع عملي صغير وكن جاهز تشرحه
✅ اتمرن على الأسئلة الشائعة
✅ راجع الـ Design Patterns الأساسية (Repository, Service Layer)
✅ افهم Entity Framework Core كويس
```

### أثناء الـ Interview

```
✅ لو مش عارف الإجابة — قول "مش متأكد بس أعتقد..."
✅ اشرح تفكيرك — الـ Process أهم من الإجابة الصح
✅ اسأل لو السؤال مش واضح
✅ اربط الإجابة بتجربة عملية لو عندك
✅ خلي الإجابة منظمة: التعريف → الفائدة → مثال
```

### جمل مفيدة

```
"الفرق الأساسي هو..."
"بنستخدمه لما..."
"مثلاً في مشروع عملت فيه..."
"أفضل Practice هنا هو..."
"المشكلة اللي بيحلها هي..."
```

---

**التالي: [10 — Cheat Sheet](./10-cheatsheet.md) →**
