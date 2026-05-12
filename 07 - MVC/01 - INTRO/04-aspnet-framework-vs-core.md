# 04 — ASP.NET Framework vs ASP.NET Core ⚔️

> **"ده زي الفرق بين عربية 2010 وعربية 2024 — نفس الاسم، بس كل حاجة اتغيرت!"**

---

## 📋 Table of Contents

- [التاريخ باختصار](#-التاريخ-باختصار)
- [المقارنة الشاملة](#-المقارنة-الشاملة)
- [الأداء](#-الأداء)
- [Cross-Platform](#-cross-platform)
- [Middleware](#-middleware)
- [Dependency Injection](#-dependency-injection)
- [Hosting](#-hosting)
- [Security](#-security)
- [Deployment](#-deployment)
- [ليه عملت Microsoft الـ Core؟](#-ليه-عملت-microsoft-الـ-core)
- [إيه اللي تختار؟](#-إيه-اللي-تختار)
- [Interview Questions](#-interview-questions)
- [Mini Summary](#-mini-summary)

---

## 📅 التاريخ باختصار

```
Timeline:

2002 → ASP.NET 1.0 (أول نسخة)
2005 → ASP.NET 2.0 (تحسينات كبيرة)
2009 → ASP.NET MVC 1.0 (نمط MVC جديد)
2012 → ASP.NET MVC 4 + Web API
2013 → ASP.NET MVC 5 (ده اللي الناس عارفاه)
2016 → ASP.NET Core 1.0 (إعادة كتابة كاملة من الصفر!)
2017 → ASP.NET Core 2.0
2019 → ASP.NET Core 3.0/3.1 (LTS)
2020 → .NET 5 (اتدمج كل حاجة)
2021 → .NET 6 (LTS - الأشهر)
2022 → .NET 7
2023 → .NET 8 (LTS - الأحدث المستقر)
2024 → .NET 9
```

---

## 📊 المقارنة الشاملة

### جدول المقارنة الكبير

| Feature | ASP.NET Framework (4.x) | ASP.NET Core (6+) |
|---------|------------------------|-------------------|
| **نظام التشغيل** | Windows فقط | Windows + Linux + macOS |
| **الأداء** | جيد | ممتاز (5-10x أسرع) |
| **Open Source** | جزئياً | ✅ كامل |
| **Side-by-Side** | ❌ | ✅ (نسخ متعددة على نفس Machine) |
| **Hosting** | IIS فقط | IIS, Kestrel, Nginx, Docker |
| **Middleware** | HTTP Modules (قديم) | Middleware Pipeline (حديث) |
| **DI Built-in** | ❌ (محتاج NuGet) | ✅ مبني جوّاه |
| **Configuration** | web.config (XML) | appsettings.json + Environment |
| **Package Size** | كبير جداً | صغير (Modular) |
| **Global.asax** | ✅ موجود | ❌ استُبدل بـ Program.cs |
| **Web.config** | ✅ مطلوب | اختياري |
| **Razor Pages** | ❌ | ✅ |
| **Blazor** | ❌ | ✅ |
| **gRPC** | ❌ | ✅ |
| **SignalR** | نسخة قديمة | نسخة محسّنة |
| **Entity Framework** | EF 6 | EF Core |
| **Docker Support** | محدود | ✅ ممتاز |
| **Hot Reload** | ❌ | ✅ |
| **Minimal APIs** | ❌ | ✅ (.NET 6+) |

---

## ⚡ الأداء

### ASP.NET Core أسرع بكتير

**TechEmpower Benchmarks (مقارنة حقيقية):**

```
Requests per Second (تقريبي):

ASP.NET Core (Minimal API):  ~7,000,000 req/sec
ASP.NET Core (MVC):          ~2,000,000 req/sec
ASP.NET Framework (MVC):     ~  200,000 req/sec
Node.js (Express):           ~  500,000 req/sec
Django:                      ~   50,000 req/sec
```

### ليه Core أسرع؟

```
1. Kestrel Web Server
   - مكتوب بـ C# وOptimized للـ Performance
   - بيستخدم Async I/O من البداية

2. No Global.asax overhead
   - ASP.NET Framework كان بيعمل Initialization كبير
   - Core بيشغل بس اللي انت طالبه

3. Modular Pipeline
   - بتضيف بس الـ Middleware اللي محتاجه
   - مش كل حاجة بتشتغل زي الـ Framework

4. Improved Memory Management
   - Span<T>, Memory<T>, Pipelines API
   - أقل Allocations = أقل Garbage Collection
```

---

## 🌍 Cross-Platform

### ASP.NET Framework — Windows Only

```
❌ مش شغال على Linux
❌ مش شغال على macOS
✅ Windows فقط
```

**المشكلة العملية:**
```
Development: Windows 💻
Production:  Windows Server 💸 (غالي جداً!)

Linux Server أرخص بكتير من Windows Server
وASP.NET Framework مش شغال على Linux
= فرصة ضيعتها على نفسك!
```

### ASP.NET Core — شغال في أي حتة

```
✅ Windows
✅ Linux (Ubuntu, Debian, CentOS...)
✅ macOS
✅ Docker Containers
✅ Kubernetes
✅ Cloud (Azure, AWS, GCP)
```

**المميزات العملية:**
```
Development: Mac أو Linux عادي 💻
Production:  Linux Server 🐧 (أرخص وأسرع)
             Docker Container 🐳
             Kubernetes Cluster ☸️
```

---

## ⚙️ Middleware

### ASP.NET Framework — HTTP Modules و Handlers (قديم)

```csharp
// ASP.NET Framework — HTTP Module (web.config)
public class LoggingModule : IHttpModule
{
    public void Init(HttpApplication context)
    {
        context.BeginRequest += OnBeginRequest;
    }
    
    void OnBeginRequest(object sender, EventArgs e)
    {
        var app = (HttpApplication)sender;
        // Logging logic
    }
    
    public void Dispose() { }
}

// ولازم تسجله في web.config!
```

### ASP.NET Core — Middleware Pipeline (حديث)

```csharp
// ASP.NET Core — Middleware بسيط وواضح
app.Use(async (context, next) =>
{
    // قبل الـ Request
    Console.WriteLine($"Request: {context.Request.Path}");
    
    await next(); // روّح للـ Middleware الجاي
    
    // بعد الـ Response
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});

// أو Middleware Class كامل
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    
    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"→ {context.Request.Method} {context.Request.Path}");
        await _next(context);
        Console.WriteLine($"← {context.Response.StatusCode}");
    }
}

app.UseMiddleware<LoggingMiddleware>();
```

### Middleware Pipeline Order (مهم جداً!)

```csharp
// Program.cs — الترتيب مهم!
app.UseExceptionHandler("/Error");   // 1. Exception Handling (الأول دايماً)
app.UseHsts();                       // 2. HTTPS Security
app.UseHttpsRedirection();           // 3. Redirect HTTP → HTTPS
app.UseStaticFiles();                // 4. بيرجع CSS/JS/Images بدون ما يكمل
app.UseRouting();                    // 5. بيحدد هيروح فين
app.UseAuthentication();             // 6. Authentication (قبل Authorization!)
app.UseAuthorization();              // 7. Authorization
app.MapControllers();                // 8. Controllers
```

```
Pipeline Flow:

Request
   ↓
Exception Handler
   ↓
HTTPS Redirect
   ↓
Static Files ──────────────────→ Response (لو File موجود)
   ↓
Routing
   ↓
Authentication
   ↓
Authorization
   ↓
Controller Action
   ↓
Response
   ↑
(بيرجع من تحت لفوق)
```

---

## 💉 Dependency Injection

### ASP.NET Framework — محتاج Library خارجية

```csharp
// كنا محتاجين Ninject أو Autofac أو Unity
// في Global.asax:
NinjectDependencyResolver resolver = new NinjectDependencyResolver(CreateKernel());
DependencyResolver.SetResolver(resolver);

// أو بنبني بإيدنا
public class ProductsController : Controller
{
    private readonly IProductService _service;
    
    public ProductsController()
    {
        _service = new ProductService(new ProductRepository()); // يعدي عليها كده!
    }
}
```

### ASP.NET Core — DI مبني من الأول

```csharp
// Program.cs — تسجيل الـ Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddTransient<IRepository<Product>, ProductRepository>();

// Controller — بيجيب الـ Services تلقائياً
public class ProductsController : Controller
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;
    
    // Constructor Injection — ASP.NET Core بيوفّره تلقائي
    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }
}
```

### أنواع الـ Lifetimes في DI

```
Singleton  → Object واحد للـ Application كلها
             مناسب للـ: Configuration, Logging, Caching
             
Scoped     → Object جديد لكل HTTP Request
             مناسب للـ: Database Contexts, Services
             
Transient  → Object جديد في كل مرة تطلبه
             مناسب للـ: Lightweight, Stateless Services
```

```csharp
// أمثلة
builder.Services.AddSingleton<IConfig, AppConfig>();    // مرة واحدة للأبد
builder.Services.AddScoped<AppDbContext>();              // مرة لكل Request
builder.Services.AddTransient<IEmailSender, SmtpSender>(); // مرة لكل Injection
```

---

## 🏠 Hosting

### ASP.NET Framework

```
مش عندك خيارات كتير:
✅ IIS (Internet Information Services)
❌ Self-hosted (صعب جداً)
❌ Docker
❌ Linux
```

### ASP.NET Core

```
عندك كل الخيارات:
✅ Kestrel (الـ Built-in Web Server)
✅ IIS (فوق Kestrel كـ Reverse Proxy)
✅ Nginx (فوق Kestrel كـ Reverse Proxy)
✅ Apache
✅ Docker
✅ Self-contained Executable
✅ Azure App Service
✅ AWS Elastic Beanstalk
```

---

## 🔒 Security

### مقارنة الـ Security Features

| Feature | Framework | Core |
|---------|-----------|------|
| Anti-CSRF | ✅ | ✅ (محسّن) |
| CORS | ✅ | ✅ (أسهل) |
| Data Protection API | ❌ | ✅ |
| HTTPS Enforcement | يدوي | سهل |
| Security Headers | يدوي | Middleware جاهز |
| Secret Management | ❌ | ✅ User Secrets + Azure Key Vault |

---

## 🚀 Deployment

### ASP.NET Framework

```
Build → DLL Files
Deploy → نسخ الملفات على IIS Server

المشكلة:
- لازم IIS مثبت
- لازم .NET Framework مثبت على الـ Server
- Version Conflicts ممكن تحصل
```

### ASP.NET Core

```
خيارات كتير:

1. Self-Contained Deployment
   dotnet publish -r win-x64 --self-contained
   → Executable كامل مش محتاج .NET على الـ Server

2. Framework-Dependent Deployment
   dotnet publish
   → محتاج .NET Runtime على الـ Server (أصغر حجم)

3. Docker
   FROM mcr.microsoft.com/dotnet/aspnet:8.0
   COPY . .
   ENTRYPOINT ["dotnet", "MyApp.dll"]

4. CI/CD Pipeline
   GitHub Actions, Azure DevOps, Jenkins
```

---

## 🤔 ليه عملت Microsoft الـ Core؟

### المشاكل الكبيرة في ASP.NET Framework

```
1. Windows Only
   → Microsoft خسرت عملاء كتير بيستخدموا Linux

2. IIS Dependency
   → مش ممكن تشتغل بدون IIS على Windows Server

3. System.Web Monolith
   → كل حاجة في Assembly واحدة ضخمة
   → حتى لو محتاج حاجة بسيطة، بتحمّل كل حاجة

4. Performance
   → مش تنافسي مع Node.js وGo

5. مش Open Source
   → مجتمع المطورين مش قادر يساهم

6. Cloud Native مش ممكن
   → Docker مش ممكن، Microservices صعب
```

### الحل: ASP.NET Core — إعادة كتابة من الصفر

```
Microsoft قررت:
"مش هنعدل على الـ Framework القديم — هنكتب جديد!"

الأهداف:
✅ Cross-Platform (Windows + Linux + Mac)
✅ Open Source
✅ High Performance
✅ Cloud Ready (Docker, Kubernetes)
✅ Modular (بتضيف بس اللي محتاجه)
✅ DI Built-in
✅ Modern Pipeline
```

---

## 🎯 إيه اللي تختار؟

### اختار ASP.NET Core لو:

```
✅ بتبدأ مشروع جديد
✅ محتاج Performance عالي
✅ بتشتغل مع Cloud أو Docker
✅ فريقك على Linux/Mac
✅ محتاج Microservices
✅ عايز Modern Development Experience
```

### اختار ASP.NET Framework (4.x) لو:

```
✅ عندك مشروع قديم وعايز تكمله (مش تغيره)
✅ بتستخدم Libraries قديمة مش اتنقلت لـ Core
✅ الـ Server بتاعك Windows فقط وما تقدرش تغير
✅ الفريق عنده خبرة طويلة في الـ Framework ومش في وقت للتغيير
```

> 💡 **نصيحة:** لو بتتعلم دلوقتي، تعلم ASP.NET Core بس. الـ Framework القديم بيتراجع تدريجياً.

---

## 💼 Interview Questions

**Q1: إيه الفرق الرئيسي بين ASP.NET Framework وASP.NET Core؟**
> Core هو إعادة كتابة كاملة من الصفر. الفروق الرئيسية: Core Cross-Platform (مش Windows بس)، أسرع بكتير، Open Source، فيه DI Built-in، Pipeline أبسط، وبيدعم Docker وCloud بشكل أفضل.

**Q2: إيه هو الـ Middleware في ASP.NET Core؟**
> هو Component بيتنفذ في الـ Request Pipeline. كل Middleware بيعالج الـ Request ثم بيبعته للـ Middleware الجاي. الترتيب مهم جداً. أمثلة: Authentication, Authorization, Routing, Logging.

**Q3: إيه هو الفرق بين Singleton وScoped وTransient؟**
> Singleton: Object واحد للـ App كلها. Scoped: Object جديد لكل HTTP Request. Transient: Object جديد في كل مرة بتطلبه. اختيار الغلط ممكن يعمل Memory Leaks.

**Q4: ليه Microsoft عملت ASP.NET Core؟**
> عشان تحل مشاكل الـ Framework القديم: Windows-only، الأداء المنخفض، الـ Monolithic Nature، وعشان تنافس Node.js وGo في الـ Cloud وDocker era.

**Q5: إيه هو الـ Program.cs في ASP.NET Core 6+؟**
> ده نقطة دخول الـ Application. بعد .NET 6 اتغير الـ Style - اتدمج Startup.cs مع Program.cs في ملف واحد باستخدام Minimal Hosting Model. بتسجل فيه الـ Services والـ Middleware.

---

## 📝 Mini Summary

```
ASP.NET Framework (قديم):
- Windows Only
- IIS فقط
- أبطأ
- DI من Library خارجية
- Monolithic (كبير وثقيل)

ASP.NET Core (حديث):
- Cross-Platform (Win + Linux + Mac)
- Kestrel + IIS + Nginx + Docker
- أسرع بكتير
- DI Built-in
- Modular (بتضيف بس اللي محتاجه)
- Open Source
- Cloud Native

القرار: لو بتبدأ جديد → ASP.NET Core دائماً
```

---

**التالي: [05 — Kestrel & Reverse Proxy](./05-kestrel-and-reverse-proxy.md) →**
