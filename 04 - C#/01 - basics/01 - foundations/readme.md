# 🎯 C# Fundamentals
## 🧠 (Big Picture)

قبل ما تكتب أي كود C#، لازم تفهم الرحلة اللي الكود بيمر بيها:

```text
[C# Source Code]
        ↓
[Compiler / Interpreter]
        ↓
[IL - Intermediate Language]
        ↓
[CLR - Common Language Runtime]
        ↓
[Machine Code]
        ↓
[CPU Execution]
```

* الكود مش بيتنفذ مباشرة
* بيتحوّل لأكتر من مرحلة
* كل مرحلة ليها أخطاءها وسلوكها

---

## 🆚 Compiler vs Interpreter (بالعقلية الصح)

### 🔹 Compiler

* بياخد **كل الكود مرة واحدة**
* يحوله بالكامل
* بعد كده يبدأ التنفيذ

📌 المميزات:

* أداء أسرع
* أخطاء واضحة من البداية

📌 العيوب:

* لازم تخلص Compilation الأول

---

### 🔹 Interpreter

* يقرأ **سطر سطر**
* ينفذ مباشرة
* لو سطر فيه Error → يوقف

📌 المميزات:

* مناسب للتجربة السريعة

📌 العيوب:

* أبطأ
* مش مناسب للبرامج الكبيرة

---

## 🚀 Top Level Statements (يعني إيه الكلام ده؟)

زمان أي برنامج C# لازم يحتوي على:

```csharp
namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
        }
    }
}
```

دلوقتي:

```csharp
Console.WriteLine("Hello");
```

💡 **اللي حصل؟**

* الكومبايلر كتب الباقي بالنيابة عنك
* الهدف تقليل الـ Boilerplate Code

⚠️ **مهم**:

* ده مش سحر
* كل حاجة لسه بتحصل زي الأول

---

## ❌ Types of Errors (أهم جزء تفهمه)

### 1️⃣ Syntax Error

```csharp
intint x = 10; // ❌
```

📌 ده خطأ لغة
📌 الكومبايلر مش فاهمك
📌 بيظهر وقت Compilation

---

### 📁 ملفات الـ Build (تفصيل ممل 😅)

داخل:

```text
bin/Debug/netX.X/
```

| الملف                 | بالتفصيل                          |
| --------------------- | --------------------------------- |
| `.exe`                | البرنامج التنفيذي النهائي         |
| `.dll`                | الكود المترجم IL                  |
| `.pdb`                | معلومات الـ Debug (سطر بسطر)      |
| `.deps.json`          | كل المكتبات اللي البرنامج محتاجها |
| `.runtimeconfig.json` | نسخة .NET وخصائص التشغيل          |

---

### 2️⃣ Runtime Error

```csharp
int x = 10 / 0;
```

📌 الكود Compile عادي
📌 المشكلة حصلت أثناء التشغيل
📌 البرنامج ينهار

---

### 3️⃣ Logical Error (أخطرهم)

```csharp
int avg = total / count; // count = 0
```

📌 لا Compiler اشتكى
📌 لا Runtime وقف
📌 الناتج غلط

🧠 الحل؟

* Debugging
* Breakpoints
* Step by Step Execution

---

### 4️⃣ Warnings

📌 مثال:

```csharp
int x = 10; // مش مستخدم
```

* مش Error
* البرنامج يشتغل

⚠️ ليه مهم؟

* ممكن يخبي Bug
* كود ملوش لازمة

---

## 🏁 Main Method (بوابة التنفيذ)

```text
Main() = أول باب البرنامج بيخش منه
```

* أي كود برة Main مش بيتنفذ
* البرنامج يبدأ وينتهي هنا

---

## 📦 Variables (يعني إيه متغير بجد؟)

### Declaration vs Initialization

```csharp
int x;      // Declaration
int y = 5;  // Initialization
```

❌ استخدام x:

```text
Use of unassigned local variable
```

📌 لأن x ملهاش قيمة

---

## 🧠 الذاكرة (افهمها صح)

### Stack (سريع – صغير)

```text
| x | 10 |
| y | 20 |
```

* Value Types
* سريع جدًا
* CPU يديره

---

### Heap (أكبر – أبطأ)

```text
| Object |
|  X=0  |
|  Y=0  |
```

* Reference Types
* CLR يديره
* Garbage Collector ينظفه

---

## 🌐 CTS vs CLS (بالعقلية الصح)

### CTS - Common Type System

* كل أنواع .NET
* لكل لغة Types خاصة

### CLS - Common Language Specification

* الأنواع المشتركة
* تضمن توافق اللغات

---

## 📊 Data Types (تفصيل)

### Value Types

* int (4 bytes)
* long (8 bytes)
* float (4 bytes)
* double (8 bytes)
* decimal (16 bytes)
* bool (1 byte)
* char (2 bytes)

---

### Enum

```csharp
enum Days { Sat, Sun, Mon }
```

* ثوابت
* أرقام متسلسلة

---

### Struct

* Value Type
* يخزن بيانات صغيرة
* أسرع من Class

---

### Reference Types

* class
* string
* array
* delegate

---

## 🔁 Value vs Reference (بعقلية الذاكرة)

### Value Type

```text
x = 10 → y = x
نسخة مستقلة
```

### Reference Type

```text
p1 ─┐
    ├─> Object (Heap)
p2 ─┘
```

* نفس العنوان
* أي تعديل يؤثر على الكل

---

## 🧩 Object vs Generics

### Object

```csharp
void Print(object x)
```

📌 يقبل أي نوع
📌 محتاج Casting
📌 أبطأ

---

### Generics (الحل المثالي)

```csharp
void Print<T>(T x)
```

✔️ Type Safe
✔️ أسرع
✔️ Cleaner

---

## 🔄 Type Casting (تفصيل)

### Implicit

```csharp
int x = 5;
long y = x;
```

* Safe
* بدون فقد بيانات

---

### Explicit

```csharp
long y = 999999;
int x = (int)y;
```

⚠️ ممكن Data Loss

---

### checked / unchecked

* checked → Exception
* unchecked → تجاهل الخطأ

---

## 🔢 Parse vs Convert

```csharp
int.Parse("10");       // Exception
int.TryParse("10", out int x); // Safe
Convert.ToInt32("10");
```

---

## ➕ Operators (تفصيل كامل)

### Arithmetic

`+  -  *  /  %`

### Assignment

`=  +=  -=  *=  /=`

### Comparison

`==  !=  >  <  >=  <=`

### Logical

`&&  ||  !`

### Unary

`++  --`

### Bitwise

`&  |  ^  <<  >>`

### Ternary

```csharp
x = (a > b) ? a : b;
```

---

## 🧠 Operator Priority

1. Unary
2. * /
3. * -
4. Comparison
5. Logical
6. Assignment

---

## ✅ الخلاصة

* افهم الذاكرة قبل الكود
* افهم الأخطاء قبل الحل
* Debug بعقلك مش بعينك

🎯 **الهدف مش كتابة كود… الهدف تفهم اللي بيحصل وراه**
