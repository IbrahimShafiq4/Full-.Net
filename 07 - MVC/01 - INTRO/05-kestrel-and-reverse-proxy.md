# 05 — Kestrel, Reverse Proxy, وWeb Servers 🔧

> **"Kestrel ده زي الطباخ في المطبخ — شغال وبيطبخ. Nginx أو IIS دول زي الـ Waiter — بيستقبل العيل ويوصل الطلب للطباخ"**

---

## 📋 Table of Contents

- [Kestrel](#-kestrel)
- [IIS](#-iis-internet-information-services)
- [Nginx](#-nginx)
- [Apache](#-apache)
- [Reverse Proxy](#-reverse-proxy)
- [Forward Proxy](#-forward-proxy)
- [Load Balancing](#-load-balancing)
- [HTTPS Termination](#-https-termination)
- [مقارنة Web Servers](#-مقارنة-web-servers)
- [Interview Questions](#-interview-questions)
- [Mini Summary](#-mini-summary)

---

## 🦅 Kestrel

### ما هو Kestrel؟

**Kestrel** هو الـ **Default Web Server** المبني جوا ASP.NET Core.

```
تخيل إنك عامل App على .NET وحاطط:
app.Run();

في السطر ده، Kestrel هو اللي بيبدأ يستمع على Port معين
(افتراضياً 5000 للـ HTTP، 5001 للـ HTTPS)
```

### مميزات Kestrel

```
1. Cross-Platform
   ✅ Windows
   ✅ Linux
   ✅ macOS

2. Async من الأساس
   بيستخدم libuv/Sockets Async
   = Performance ممتاز

3. Open Source
   كود المصدر على GitHub

4. HTTP/1.1 + HTTP/2 + HTTP/3
   بيدعم أحدث بروتوكولات

5. سريع جداً
   من أسرع الـ Web Servers في العالم
```

### إزاي Kestrel بيشتغل؟

```
ASP.NET Core App Starts
         |
         ↓
Kestrel يبدأ يستمع على Port 5000
         |
         ↓
يجي HTTP Request
         |
         ↓
Kestrel يستقبله
         |
         ↓
يبعته للـ ASP.NET Core Middleware Pipeline
         |
         ↓
بيرجع Response من الـ Pipeline
         |
         ↓
Kestrel يبعت الـ Response للـ Client
```

### تكوين Kestrel

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// تكوين Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    // استمع على Port 5000 HTTP
    options.Listen(IPAddress.Any, 5000);
    
    // استمع على Port 5001 HTTPS
    options.Listen(IPAddress.Any, 5001, listenOptions =>
    {
        listenOptions.UseHttps("certificate.pfx", "password");
    });
    
    // Limits
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10MB
    options.Limits.MaxConcurrentConnections = 100;
});
```

### هل Kestrel كافي لوحده في Production؟

```
Development:  ✅ Kestrel وحده كافي
Production:   ⚠️ ممكن تحتاج Reverse Proxy

ليه؟
- Kestrel مش مصمم للـ Internet Direct Exposure
- محتاج SSL/TLS Management
- محتاج Load Balancing
- محتاج Static File Serving الأفضل
- محتاج Security Headers المتقدمة
```

---

## 🪟 IIS (Internet Information Services)

### ما هو IIS؟

هو الـ **Web Server** بتاع Microsoft للـ Windows.

```
قبل ASP.NET Core:
App → IIS (هو اللي بيشتغل كل حاجة)

بعد ASP.NET Core:
Internet → IIS → Kestrel → App
            ↑
       (Reverse Proxy)
```

### IIS كـ Reverse Proxy لـ ASP.NET Core

```
╔═══════════════════════════════════════════╗
║              Windows Server               ║
║                                           ║
║  ┌─────────┐     ┌─────────────────────┐  ║
║  │   IIS   │────→│  ASP.NET Core App   │  ║
║  │ Port 80 │     │  (Kestrel Port 5000)│  ║
║  └─────────┘     └─────────────────────┘  ║
║       ↑                                   ║
╚═══════════════════════════════════════════╝
         ↑
    Internet Requests
```

### تثبيت ASP.NET Core على IIS

```xml
<!-- web.config لازم موجود للـ IIS -->
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <add name="aspNetCore" 
           path="*" verb="*" 
           modules="AspNetCoreModuleV2" 
           resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath="dotnet"
                arguments=".\MyApp.dll"
                stdoutLogEnabled="false"
                stdoutLogFile=".\logs\stdout"
                hostingModel="inprocess" />
  </system.webServer>
</configuration>
```

---

## 🟢 Nginx

### ما هو Nginx؟

Nginx (بتنطق "Engine-X") هو **Web Server + Reverse Proxy** مفتوح المصدر وخفيف جداً.

**الأشهر على Linux Servers.**

### استخدام Nginx مع ASP.NET Core

```
Internet
    |
    ↓ Port 80/443
 ┌──────┐
 │ Nginx │  ← بيستقبل كل الـ Requests
 └──┬───┘
    │ Pass to Kestrel
    ↓ Port 5000
 ┌──────────────┐
 │ ASP.NET Core │  ← بيعالج الـ Requests
 │   (Kestrel)  │
 └──────────────┘
```

### ملف تكوين Nginx

```nginx
# /etc/nginx/sites-available/myapp

server {
    listen 80;
    server_name example.com www.example.com;
    
    # Redirect HTTP → HTTPS
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl http2;
    server_name example.com www.example.com;
    
    # SSL Certificate
    ssl_certificate /etc/ssl/certs/example.com.crt;
    ssl_certificate_key /etc/ssl/private/example.com.key;
    
    # Security Headers
    add_header X-Frame-Options "SAMEORIGIN" always;
    add_header X-Content-Type-Options "nosniff" always;
    add_header X-XSS-Protection "1; mode=block" always;
    
    location / {
        proxy_pass http://localhost:5000;    # Kestrel
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
    
    # Static Files مباشرة من Nginx (أسرع)
    location /static/ {
        root /var/www/myapp;
        expires 1y;
        add_header Cache-Control "public, no-transform";
    }
}
```

---

## 🪶 Apache

Apache هو ثاني أشهر Web Server على Linux.

```nginx
# /etc/apache2/sites-available/myapp.conf

<VirtualHost *:80>
    ServerName example.com
    Redirect permanent / https://example.com/
</VirtualHost>

<VirtualHost *:443>
    ServerName example.com
    
    SSLEngine on
    SSLCertificateFile /etc/ssl/certs/example.com.crt
    SSLCertificateKeyFile /etc/ssl/private/example.com.key
    
    ProxyPreserveHost On
    ProxyPass / http://127.0.0.1:5000/
    ProxyPassReverse / http://127.0.0.1:5000/
    
    RequestHeader set X-Forwarded-Proto "https"
</VirtualHost>
```

---

## 🔄 Reverse Proxy

### ما هو Reverse Proxy؟

**Reverse Proxy** هو Server وسيط بيستقبل الـ Requests من المستخدمين ويحولها للـ Backend Servers.

```
بدون Reverse Proxy:
User ←→ App Server مباشرة

مع Reverse Proxy:
User ←→ Reverse Proxy ←→ App Server(s)
```

### فايدة الـ Reverse Proxy

```
1. Security
   - الـ App Server مش مكشوف للـ Internet مباشرة
   - ممكن تعمل WAF (Web Application Firewall)

2. SSL Termination
   - الـ SSL Encryption بيحصل في الـ Nginx/IIS
   - الاتصال بين Nginx و App (داخلي) ممكن يكون HTTP عادي

3. Load Balancing
   - توزيع الـ Requests على أكتر من App Instance

4. Caching
   - ممكن يحفظ بعض الـ Responses ويردها بسرعة

5. Compression
   - ممكن يعمل Gzip Compression

6. Logging & Monitoring
   - نقطة واحدة لتسجيل كل الـ Traffic
```

### Reverse Proxy Flow

```
┌────────────────────────────────────────────────────┐
│                    Internet                         │
└───────────────────────┬────────────────────────────┘
                        │ HTTPS Request
                        ↓
┌───────────────────────────────────────────────────┐
│              Reverse Proxy (Nginx/IIS)             │
│  - SSL Termination                                 │
│  - Security Headers                                │
│  - Rate Limiting                                   │
│  - Logging                                         │
└──────────┬────────────────────┬───────────────────┘
           │ HTTP               │ HTTP
           ↓                    ↓
┌──────────────────┐  ┌──────────────────┐
│  App Server 1    │  │  App Server 2    │
│  (Kestrel:5000)  │  │  (Kestrel:5001)  │
└──────────────────┘  └──────────────────┘
```

---

## 🕵️ Forward Proxy

### الفرق بين Forward وReverse Proxy

```
Forward Proxy:
User → Forward Proxy → Internet
(الـ Proxy بيخفي هوية المستخدم عن الـ Internet)

مثال: VPN، Tor

Reverse Proxy:
Internet → Reverse Proxy → App Server(s)
(الـ Proxy بيخفي هوية الـ App Server عن الـ Internet)

مثال: Nginx, IIS, AWS CloudFront
```

```
                    Forward Proxy
                    ┌──────────────┐
Users ─────────────→│ Proxy Server │────────→ Internet
(هويتهم مخفية)     └──────────────┘

                    Reverse Proxy
                    ┌──────────────┐
Internet ──────────→│ Proxy Server │────────→ App Servers
(الـ Servers مخفية) └──────────────┘
```

---

## ⚖️ Load Balancing

**Load Balancing** = توزيع الـ Traffic على أكتر من Server.

### ليه محتاجينه؟

```
مشكلة:
App Server واحد → يستقبل 1000 Request في الثانية
              → يهنج ويوقع!

الحل:
3 App Servers + Load Balancer
→ كل Server يأخذ 333 Request في الثانية
→ نظام أسرع وأكثر استقراراً
```

### استراتيجيات الـ Load Balancing

```
1. Round Robin (الأشهر)
   Request 1 → Server 1
   Request 2 → Server 2
   Request 3 → Server 3
   Request 4 → Server 1 (من أول وجديد)

2. Least Connections
   الـ Request بيروح للـ Server الأقل ازدحاماً

3. IP Hash
   نفس الـ IP دايماً يروح لنفس الـ Server
   (مهم للـ Session-based Apps)

4. Weighted
   بعض الـ Servers بياخدوا Load أكتر بناءاً على قوتهم
```

### Nginx Load Balancing Config

```nginx
upstream aspnet_servers {
    # Round Robin (default)
    server localhost:5000;
    server localhost:5001;
    server localhost:5002;
    
    # أو Weighted
    # server localhost:5000 weight=3;
    # server localhost:5001 weight=1;
    
    # أو Least Connections
    # least_conn;
}

server {
    location / {
        proxy_pass http://aspnet_servers;
    }
}
```

---

## 🔐 HTTPS Termination

### ما هو HTTPS Termination؟

```
HTTPS Termination = الـ Reverse Proxy بيفك التشفير
                    والباقي بيمشي HTTP عادي (داخلياً)

Internet
   |
   | HTTPS (Encrypted)
   ↓
Nginx/IIS ← هنا بيحصل "SSL Termination"
   |
   | HTTP (Unencrypted) ← داخلي في نفس السيرفر
   ↓
ASP.NET Core (Kestrel)
```

### ليه ده مفيد؟

```
1. تبسيط الـ SSL Management
   - Certificates في مكان واحد (الـ Reverse Proxy)
   - مش محتاج تعمله Configure في كل App

2. Performance
   - SSL Encryption/Decryption مكلف
   - بيحصل مرة واحدة بدل ما يتكرر

3. الـ App مش محتاج يعرف عن SSL
   - بيشتغل بـ HTTP بسيط
```

### في ASP.NET Core مع Reverse Proxy

```csharp
// لازم تضيف ده عشان الـ App يعرف إنه ورا Reverse Proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | 
                               ForwardedHeaders.XForwardedProto;
});

app.UseForwardedHeaders(); // لازم أول حاجة في الـ Pipeline
```

---

## 📊 مقارنة Web Servers

| Feature | IIS | Nginx | Apache | Kestrel |
|---------|-----|-------|--------|---------|
| **OS** | Windows | Linux/Win/Mac | Linux/Win/Mac | كل حاجة |
| **Performance** | جيد | ممتاز | جيد | ممتاز |
| **Memory Usage** | عالي | منخفض | متوسط | منخفض |
| **Configuration** | GUI | Text File | Text File | Code (C#) |
| **Static Files** | ممتاز | ممتاز | جيد | ليس مثالياً |
| **SSL/TLS** | ✅ | ✅ | ✅ | ✅ |
| **Load Balancing** | محدود | ✅ ممتاز | محدود | ❌ |
| **Hot Reload Config** | ❌ | ✅ | ✅ | ❌ |
| **مع ASP.NET Core** | ✅ Reverse Proxy | ✅ Reverse Proxy | ✅ Reverse Proxy | ✅ مباشرة |
| **الاستخدام المناسب** | Windows Production | Linux Production | Linux (قديم) | Development |

---

## 💼 Interview Questions

**Q1: إيه هو Kestrel وإيه دوره؟**
> Kestrel هو الـ Built-in Web Server في ASP.NET Core. هو اللي بيستمع على الـ Port ويستقبل الـ HTTP Requests ويبعتها للـ ASP.NET Core Pipeline. سريع جداً ومبني على Async I/O.

**Q2: إيه الفرق بين Reverse Proxy وForward Proxy؟**
> Forward Proxy يخفي هوية المستخدم عن الـ Internet (مثل VPN). Reverse Proxy يخفي هوية الـ Servers عن الـ Internet ويوزع الـ Traffic (مثل Nginx, IIS).

**Q3: ليه بنستخدم Nginx مع Kestrel بدل ما نستخدم Kestrel مباشرة؟**
> Nginx بيوفر: SSL Management, Load Balancing, Static File Serving بكفاءة أعلى, Security Headers, Rate Limiting, وCaching. Kestrel مش مصمم للـ Direct Internet Exposure.

**Q4: إيه هو SSL Termination؟**
> هو إن الـ Reverse Proxy (Nginx/IIS) بيفك تشفير الـ HTTPS ويبعت HTTP عادي للـ App Server. بيبسّط الـ SSL Management ويحسن الـ Performance.

**Q5: إزاي تعمل Load Balancing في Nginx؟**
> بتعرف upstream group فيه قائمة الـ Servers، وبتحط proxy_pass في الـ Location Block. Nginx بيوزع الـ Requests بـ Round Robin افتراضياً.

---

## 📝 Mini Summary

```
Web Server Landscape:

Kestrel → الـ Built-in Server في ASP.NET Core
IIS     → Reverse Proxy على Windows
Nginx   → Reverse Proxy على Linux (الأشهر)
Apache  → Reverse Proxy قديم على Linux

Production Setup الأشهر:

Internet → Nginx (443/80) → Kestrel (5000)
                ↓
         SSL Termination
         Load Balancing
         Static Files
         Security Headers

الـ Flow:
HTTPS Request → Nginx → HTTP → Kestrel → ASP.NET Pipeline → Response
```

---

**التالي: [06 — MVC بالتفصيل](./06-mvc-in-details.md) →**
