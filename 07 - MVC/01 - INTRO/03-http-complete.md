# 03 — HTTP 🌐

> **"HTTP هو اللغة اللي بيتكلم بيها المتصفح والـ Server — زي إنك بتتكلم مع حد، في طلب وفي رد"**

---

## 📋 Table of Contents

- [ما هو HTTP؟](#-ما-هو-http)
- [HTTP Request vs Response](#-http-request-vs-response)
- [HTTP Methods](#-http-methods)
- [HTTP Headers](#-http-headers)
- [HTTP Status Codes](#-http-status-codes)
- [Cookies](#-cookies)
- [Sessions](#-sessions)
- [Authentication vs Authorization](#-authentication-vs-authorization)
- [HTTPS](#-https)
- [Stateless Nature](#-stateless-nature)
- [REST APIs](#-rest-apis)
- [أمثلة Raw HTTP](#-أمثلة-raw-http)
- [أخطاء شائعة](#-أخطاء-شائعة)
- [Best Practices](#-best-practices)
- [Interview Questions](#-interview-questions)
- [Mini Summary](#-mini-summary)

---

## 📖 ما هو HTTP؟

**HTTP = HyperText Transfer Protocol**

هو **بروتوكول اتصال** (يعني مجموعة قواعد متفق عليها) بيحدد إزاي المتصفح يتكلم مع الـ Web Server.

**تشبيه من الحياة:**  
لما بتطلب أكل في مطعم، في قواعد:
- إنت بتقول "عايز كده وكده"
- الـ Waiter بيجيبلك
- لو مش موجود بيقولك "معندناش"

نفس الكلام مع HTTP:
```
المتصفح     → "عايز صفحة /products"   (HTTP Request)
الـ Server  → "اتفضل، ده الـ HTML"    (HTTP Response)
```

**نبذة تاريخية:**
- **HTTP/1.0** (1996): كل Request بيفتح Connection جديد
- **HTTP/1.1** (1997): Keep-Alive Connections، أسرع
- **HTTP/2** (2015): Multiplexing، Binary، أسرع بكتير
- **HTTP/3** (2022): فوق QUIC بدل TCP، الأسرع

---

## 📨 HTTP Request vs Response

```
┌────────────────────────────────────────────────────┐
│                  HTTP Request                       │
├────────────────────────────────────────────────────┤
│  Request Line:  GET /products HTTP/1.1              │
│  Headers:       Host: example.com                   │
│                 Accept: text/html                   │
│                 Cookie: session=abc123              │
│                 (... More Headers ...)              │
│  [Empty Line]                                       │
│  Body:          (فاضي في GET، في بيانات في POST)   │
└────────────────────────────────────────────────────┘

┌────────────────────────────────────────────────────┐
│                  HTTP Response                      │
├────────────────────────────────────────────────────┤
│  Status Line:   HTTP/1.1 200 OK                     │
│  Headers:       Content-Type: text/html             │
│                 Content-Length: 1234                │
│                 Set-Cookie: session=xyz             │
│                 (... More Headers ...)              │
│  [Empty Line]                                       │
│  Body:          <html><body>...</body></html>        │
└────────────────────────────────────────────────────┘
```

---

## 🔨 HTTP Methods

### GET — اطلب بيانات

```http
GET /api/products HTTP/1.1
Host: api.example.com
Accept: application/json
```

**خصائص:**
- ✅ Safe (مش بيغير حاجة في الـ Server)
- ✅ Idempotent (ممكن تبعته أكتر من مرة بنفس النتيجة)
- ❌ مش بيبعت Body
- ❌ البيانات في الـ URL (مش مناسب للـ Sensitive Data)

**في ASP.NET:**
```csharp
[HttpGet]
public IActionResult GetProducts()
{
    return Ok(_products.GetAll());
}

// أو بدون Attribute (GET هو الـ Default)
public IActionResult Index()
{
    return View();
}
```

---

### POST — ابعت بيانات جديدة

```http
POST /api/products HTTP/1.1
Host: api.example.com
Content-Type: application/json
Content-Length: 85

{
    "name": "Laptop",
    "price": 15000,
    "category": "Electronics"
}
```

**خصائص:**
- ❌ مش Safe (بيعمل حاجة في الـ Server)
- ❌ مش Idempotent (لو بعتته مرتين، هيعمل Record تاني)
- ✅ البيانات في الـ Body (أمان أكتر)

**في ASP.NET:**
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] ProductDto product)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);
    
    var created = await _service.CreateAsync(product);
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
}
```

---

### PUT — استبدل بيانات كاملة

```http
PUT /api/products/5 HTTP/1.1
Host: api.example.com
Content-Type: application/json

{
    "id": 5,
    "name": "Gaming Laptop",
    "price": 20000,
    "category": "Electronics",
    "stock": 10
}
```

**خصائص:**
- ❌ مش Safe
- ✅ Idempotent (لو بعتته مرتين، نفس النتيجة)
- بيستبدل الـ Resource كامل

---

### PATCH — عدّل جزء من البيانات

```http
PATCH /api/products/5 HTTP/1.1
Host: api.example.com
Content-Type: application/json

{
    "price": 18000
}
```

**الفرق بين PUT وPATCH:**
```
PUT  → بتبعت الـ Object كامل (حتى اللي ما اتغيرش)
PATCH → بتبعت بس اللي اتغير

مثال:
Product: {id:5, name:"Laptop", price:15000, stock:10}

PUT  → لازم تبعت: {id:5, name:"Laptop", price:18000, stock:10} (كامل)
PATCH → تبعت بس: {price:18000}
```

---

### DELETE — احذف

```http
DELETE /api/products/5 HTTP/1.1
Host: api.example.com
Authorization: Bearer eyJhbGc...
```

**في ASP.NET:**
```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    var product = await _service.GetByIdAsync(id);
    if (product == null) return NotFound();
    
    await _service.DeleteAsync(id);
    return NoContent(); // 204
}
```

---

### مقارنة شاملة للـ Methods

| Method | الغرض | Body | Idempotent | Safe |
|--------|--------|------|-----------|------|
| GET | جيب بيانات | ❌ | ✅ | ✅ |
| POST | أضف جديد | ✅ | ❌ | ❌ |
| PUT | استبدل كامل | ✅ | ✅ | ❌ |
| PATCH | عدّل جزئي | ✅ | ❌* | ❌ |
| DELETE | احذف | اختياري | ✅ | ❌ |
| HEAD | زي GET بدون Body | ❌ | ✅ | ✅ |
| OPTIONS | معرفة الـ Methods المتاحة | ❌ | ✅ | ✅ |

---

## 📋 HTTP Headers

الـ Headers هي **معلومات إضافية** بتتبعت مع الـ Request أو Response.

### Request Headers الشائعة

```http
GET /api/products HTTP/1.1
Host: api.example.com              ← اسم الـ Server (مطلوب دايماً)
Accept: application/json           ← أنا عايز الرد في أي صيغة؟
Accept-Language: ar, en            ← أنا بتكلم أنهي لغة؟
Accept-Encoding: gzip, deflate     ← أقبل Compression؟
Authorization: Bearer eyJhbGc...   ← التوكن بتاعي
Content-Type: application/json     ← صيغة الـ Body اللي بابعته
Content-Length: 256                ← حجم الـ Body
User-Agent: Mozilla/5.0...         ← أنا مين؟ (المتصفح)
Cookie: sessionId=abc; theme=dark  ← الـ Cookies بتاعتي
Referer: https://google.com        ← جيت منين؟
Cache-Control: no-cache            ← تعليمات الـ Caching
```

### Response Headers الشائعة

```http
HTTP/1.1 200 OK
Content-Type: text/html; charset=utf-8  ← نوع الـ Body
Content-Length: 1523                     ← حجم الـ Body
Set-Cookie: session=xyz; HttpOnly       ← احفظ الـ Cookie دي
Cache-Control: max-age=3600             ← كاش الـ Response لساعة
Location: /products/5                   ← عنوان الـ Redirect (في 301/302)
WWW-Authenticate: Bearer               ← مطلوب توثيق
Access-Control-Allow-Origin: *          ← CORS Policy
X-Frame-Options: DENY                   ← منع الـ iFrame
Strict-Transport-Security: max-age=... ← HTTPS Enforcement
```

### Custom Headers في ASP.NET

```csharp
// إضافة Header في الـ Response
Response.Headers.Add("X-Custom-Header", "MyValue");

// قراءة Header من الـ Request
var userAgent = Request.Headers["User-Agent"].ToString();
var authHeader = Request.Headers["Authorization"].ToString();
```

---

## 🔢 HTTP Status Codes

```
1xx → Informational (نادر الاستخدام)
2xx → Success ✅
3xx → Redirection ↩️
4xx → Client Error (الغلطة عندك أنت) ❌
5xx → Server Error (الغلطة عند الـ Server) 💥
```

### الـ Status Codes الأكثر شيوعاً

**2xx — النجاح**
```
200 OK              → تمام، اتفضل الـ Response
201 Created         → تم إنشاء Record جديد
204 No Content      → تمام، بس مفيش حاجة أبعتهالك (بعد Delete مثلاً)
206 Partial Content → بعتلك جزء بس (في الـ File Downloads)
```

**3xx — الـ Redirects**
```
301 Moved Permanently  → الصفحة اتنقلت بشكل دائم (SEO مهم)
302 Found              → Redirect مؤقت
304 Not Modified       → الـ Cache بتاعك لسه صالح، استخدمه
307 Temporary Redirect → Redirect مؤقت (زي 302 بس بيحافظ على Method)
```

**4xx — غلطة المستخدم**
```
400 Bad Request        → الـ Request فيه مشكلة (Invalid Data)
401 Unauthorized       → مش معاك Token / مش Logged In
403 Forbidden          → معاك Token بس مش عندك صلاحية
404 Not Found          → الصفحة مش موجودة
405 Method Not Allowed → بتستخدم Method غلط (مثلاً POST على URL بيقبل GET بس)
409 Conflict           → في تعارض (مثلاً Email موجود قبل كده)
422 Unprocessable      → البيانات شكلها صح بس المعنى غلط
429 Too Many Requests  → عملت Requests كتير (Rate Limiting)
```

**5xx — غلطة الـ Server**
```
500 Internal Server Error → حصل Error غير متوقع في الـ Server
502 Bad Gateway           → الـ Reverse Proxy فشل في الاتصال بالـ Backend
503 Service Unavailable   → الـ Server مش شغال أو مزنوق
504 Gateway Timeout       → الـ Backend أخد وقت طويل جداً
```

**في ASP.NET:**
```csharp
// Return Status Codes مختلفة
return Ok(data);              // 200
return Created(url, data);    // 201
return NoContent();           // 204
return BadRequest("Error");   // 400
return Unauthorized();        // 401
return Forbid();              // 403
return NotFound();            // 404
return Conflict("Duplicate"); // 409
return StatusCode(500, "Error"); // أي كود تاني
```

---

## 🍪 Cookies

الـ Cookie هو **بيانات صغيرة** بتتخزن في المتصفح وبتتبعت مع كل Request.

### إزاي بيشتغل الـ Cookie؟

```
أول مرة:
Browser → GET /login (مفيش Cookie)
Server  → 200 OK + Set-Cookie: session=abc123; HttpOnly

تاني مرة:
Browser → GET /dashboard + Cookie: session=abc123
Server  → 200 OK (عرف إنك أنت)
```

### أنواع الـ Cookies

```
Session Cookie   → بتاتت لما المتصفح يقفل (مفيش Expiry)
Persistent Cookie → عندها تاريخ انتهاء محدد
```

### Cookie Attributes المهمة

```
HttpOnly  → المتصفح بيقراها بس، JavaScript مش تقدر تلمسها (حماية من XSS)
Secure    → بتتبعت بس على HTTPS
SameSite  → Strict/Lax/None (حماية من CSRF)
Expires   → امتى بتنتهي
Path      → على أنهي URLs بتتبعت
Domain    → على أنهي Domain بتتبعت
```

### في ASP.NET:

```csharp
// كتابة Cookie
Response.Cookies.Append("UserPreference", "dark-mode", new CookieOptions
{
    Expires = DateTimeOffset.UtcNow.AddDays(30),
    HttpOnly = true,
    Secure = true,
    SameSite = SameSiteMode.Strict
});

// قراءة Cookie
var preference = Request.Cookies["UserPreference"];

// حذف Cookie
Response.Cookies.Delete("UserPreference");
```

---

## 🔐 Sessions

الـ Session هي طريقة لتخزين بيانات المستخدم **على الـ Server** بدل المتصفح.

### الفرق بين Cookie و Session

```
Cookie:
- البيانات عند المتصفح
- المتصفح بيبعتها مع كل Request
- ممكن يعدلها المستخدم (خطر)
- مناسبة للبيانات غير الحساسة

Session:
- الـ Session ID فقط عند المتصفح (في Cookie)
- البيانات الحقيقية عند الـ Server
- المستخدم ما يقدرش يعدل البيانات
- مناسبة للبيانات الحساسة
```

```
Session Flow:
1. User Logs In
2. Server ينشئ Session ID فريد
3. Server يحفظ البيانات مع الـ Session ID (في Memory أو Database)
4. Server يبعت الـ Session ID للمتصفح في Cookie
5. في كل Request، المتصفح يبعت الـ Cookie
6. Server يبحث عن الـ Session بالـ ID ويجيب البيانات
```

### في ASP.NET:

```csharp
// Setup في Program.cs
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

app.UseSession(); // لازم قبل UseRouting

// استخدام في Controller
// كتابة
HttpContext.Session.SetString("UserName", "Ahmed");
HttpContext.Session.SetInt32("UserId", 123);

// قراءة
var userName = HttpContext.Session.GetString("UserName");
var userId = HttpContext.Session.GetInt32("UserId");

// حذف
HttpContext.Session.Remove("UserName");
HttpContext.Session.Clear(); // كل الـ Session
```

---

## 🔑 Authentication vs Authorization

### Authentication — التحقق من الهوية

> "مين أنت؟"

**طرق الـ Authentication:**

```
1. Username/Password (الأشهر)
2. JWT Tokens (للـ APIs)
3. OAuth2 / OpenID Connect (Login with Google/Facebook)
4. API Keys
5. Certificates
```

### Authorization — التحقق من الصلاحية

> "هل تقدر تعمل ده؟"

```
Authentication  →  أنا عارف إنك Ahmed
Authorization   →  Ahmed مش عنده صلاحية تحذف Users
```

### في ASP.NET:

```csharp
// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => 
        policy.RequireRole("Admin"));
    
    options.AddPolicy("PremiumUser", policy =>
        policy.RequireClaim("Subscription", "Premium"));
});

// استخدام في Controller
[Authorize] // لازم يكون Logged In
public class DashboardController : Controller { }

[Authorize(Roles = "Admin")]  // لازم يكون Admin
public class AdminController : Controller { }

[Authorize(Policy = "PremiumUser")]
public IActionResult PremiumContent() { return View(); }

[AllowAnonymous] // ولا حاجة مطلوبة
public IActionResult PublicPage() { return View(); }
```

---

## 🔒 HTTPS

**HTTPS = HTTP + TLS/SSL (Encryption)**

```
HTTP   → البيانات بتتبعت عادية (Plaintext)
HTTPS → البيانات بتتبعت مشفرة (Encrypted)

مثال:
HTTP: "Password: ahmed123"  ← أي حد يتنصت يشوفها!
HTTPS: "x9kL#mP2...@qR5"   ← مش مفهوم للمتنصت
```

### إزاي بيشتغل HTTPS؟

```
TLS Handshake:

Client                          Server
  |                               |
  |──── Client Hello ────────────→|
  |    (SSL Version, Cipher List) |
  |                               |
  |←─── Server Hello ────────────|
  |    (Chosen Cipher, Certificate)|
  |                               |
  | Client يتحقق من الـ Certificate |
  |                               |
  |──── Key Exchange ────────────→|
  |                               |
  |←─── بدء التشفير ─────────────|
  |                               |
  |════ Encrypted Data ══════════|
```

### في ASP.NET Core:

```csharp
// Force HTTPS في Program.cs
app.UseHsts();           // Header: Strict-Transport-Security
app.UseHttpsRedirection(); // Redirect من HTTP لـ HTTPS
```

---

## 🔄 Stateless Nature

HTTP هو **Stateless Protocol** — يعني الـ Server مش بيتذكر الـ Request السابق.

```
Request 1: GET /page1 → Server ماشي، يتفضل
Request 2: GET /page2 → Server ماشي، يتفضل
                         (مش عارف إن Request 1 و 2 نفس الشخص!)
```

**الحل:** نستخدم Cookies أو Sessions أو JWT Tokens عشان نحتفظ بالـ State.

---

## 🔌 REST APIs

**REST = Representational State Transfer**

مجموعة قواعد لتصميم APIs:

```
قواعد REST:

1. Stateless       → كل Request مكتمل بنفسه
2. Uniform Interface → URLs موحدة ومنطقية
3. Resource-Based  → بتشتغل على Resources مش Actions
4. HTTP Methods    → GET/POST/PUT/DELETE بمعناها الصح
```

### تصميم REST API صح

```
✅ RESTful:
GET    /api/products         → جيب كل المنتجات
GET    /api/products/5       → جيب المنتج رقم 5
POST   /api/products         → أضف منتج جديد
PUT    /api/products/5       → عدّل المنتج رقم 5 كامل
PATCH  /api/products/5       → عدّل جزء من المنتج رقم 5
DELETE /api/products/5       → احذف المنتج رقم 5

❌ مش RESTful:
GET /getProducts             → مش صح
GET /deleteProduct?id=5      → غلط جداً! DELETE مش GET
POST /api/products/create    → "create" في الـ URL زيادة
```

---

## 📝 أمثلة Raw HTTP

### Raw HTTP Request (GET)

```http
GET /api/products?category=electronics&page=1 HTTP/1.1
Host: api.example.com
Accept: application/json
Accept-Language: ar-EG, en-US
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64)
```

### Raw HTTP Request (POST)

```http
POST /api/orders HTTP/1.1
Host: api.example.com
Content-Type: application/json
Content-Length: 156
Authorization: Bearer eyJhbGc...
Cookie: sessionId=abc123

{
    "customerId": 42,
    "items": [
        {"productId": 5, "quantity": 2},
        {"productId": 8, "quantity": 1}
    ],
    "shippingAddress": "Cairo, Egypt"
}
```

### Raw HTTP Response (Success)

```http
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Content-Length: 98
Location: https://api.example.com/api/orders/789
Cache-Control: no-cache
Date: Mon, 12 May 2026 10:30:00 GMT

{
    "id": 789,
    "customerId": 42,
    "status": "pending",
    "totalAmount": 350.00,
    "createdAt": "2026-05-12T10:30:00Z"
}
```

### Raw HTTP Response (Error)

```http
HTTP/1.1 422 Unprocessable Entity
Content-Type: application/problem+json
Content-Length: 185

{
    "type": "https://tools.ietf.org/html/rfc4918#section-11.2",
    "title": "One or more validation errors occurred.",
    "status": 422,
    "errors": {
        "quantity": ["Quantity must be greater than 0"],
        "productId": ["Product not found"]
    }
}
```

---

## ⚠️ أخطاء شائعة

### 1. استخدام GET للـ Actions اللي بتغير Data

```
❌ غلط: GET /deleteUser?id=5
✅ صح:  DELETE /api/users/5
```

### 2. نسيان الـ Status Codes الصح

```csharp
// ❌ غلط — بترجع 200 حتى لو في Error
return Ok(new { error = "User not found" });

// ✅ صح
return NotFound(new { message = "User not found" });
```

### 3. تخزين بيانات حساسة في Cookies بدون HttpOnly

```csharp
// ❌ خطر — JavaScript ممكن تسرق الـ Cookie
Response.Cookies.Append("token", jwtToken);

// ✅ أمان
Response.Cookies.Append("token", jwtToken, new CookieOptions
{
    HttpOnly = true,
    Secure = true,
    SameSite = SameSiteMode.Strict
});
```

### 4. الخلط بين 401 و 403

```
401 Unauthorized → مش معاك Token (مش logged in)
403 Forbidden    → معاك Token بس مش عندك صلاحية
```

---

## ✅ Best Practices

1. **استخدم الـ Status Codes الصح** — مش كل حاجة 200
2. **HTTPS دايماً** — خصوصاً في الـ Production
3. **HttpOnly + Secure للـ Cookies** — حماية من XSS و MITM
4. **API Versioning** — `/api/v1/products` مش `/api/products`
5. **Rate Limiting** — امنع الـ Spam
6. **Input Validation** — تحقق من كل البيانات الجاية
7. **لا تبعت معلومات حساسة في الـ URL** — استخدم Body أو Headers

---

## 💼 Interview Questions

**Q1: إيه الفرق بين GET و POST؟**
> GET لجيب بيانات - البيانات في الـ URL، Safe وIdempotent. POST لإرسال بيانات - البيانات في الـ Body، مش Safe وليس Idempotent.

**Q2: إيه الفرق بين 401 و 403؟**
> 401: مش عارف مين أنت (غير مصادق). 403: عارف مين أنت بس مش عندك صلاحية (غير مصرح).

**Q3: إيه هو الـ Stateless في HTTP وإزاي بنتعامل معاه؟**
> HTTP مش بيحفظ State بين الـ Requests. بنتعامل معاه باستخدام: Cookies (لتخزين الـ Session ID)، Sessions (لتخزين البيانات على الـ Server)، أو JWT Tokens (Self-contained Tokens).

**Q4: إيه الفرق بين Authentication و Authorization؟**
> Authentication: التحقق من هوية المستخدم (مين أنت؟). Authorization: التحقق من الصلاحيات (هل تقدر تعمل ده؟).

**Q5: إيه هو CORS وليه مهم؟**
> CORS = Cross-Origin Resource Sharing. ده بروتوكول أمان بيمنع صفحة على domain معين من إنها تطلب Resources من domain تاني من غير إذن. محتاج تعمله Configure في الـ API عشان تسمح للـ Frontend بالوصول.

---

## 📝 Mini Summary

```
HTTP = البروتوكول اللي بيحكم التواصل بين Client وServer

Methods:
GET    → اقرأ    | POST   → أنشئ
PUT    → استبدل  | PATCH  → عدّل جزئي
DELETE → احذف

Status Codes:
2xx → نجاح | 3xx → Redirect | 4xx → غلطتك | 5xx → غلطة Server

Security:
HTTPS     → تشفير الاتصال
HttpOnly  → حماية الـ Cookies من JavaScript
JWT/Token → للـ Authentication في الـ APIs

State Management:
Cookies  → بيانات عند المتصفح
Sessions → بيانات عند الـ Server
```

---

**التالي: [04 — ASP.NET Framework vs ASP.NET Core](./04-aspnet-framework-vs-core.md) →**
