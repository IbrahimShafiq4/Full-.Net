# 🚀 LINQ Advanced Concepts — Visual Learning Guide

> **دليل تعليمي بصري لمفاهيم LINQ المتقدمة في C#**  
> مناسب للمراجعة قبل الـ Interviews وللنشر على GitHub كـ Portfolio

---

## 📋 Table of Contents

1. [What is LINQ?](#1-what-is-linq)
2. [ListGenerator & Data Model](#2-listgenerator--data-model)
3. [Zip Operator](#3-zip-operator)
4. [GroupBy Operator](#4-groupby-operator)
5. [Partitioning Operators](#5-partitioning-operators)
6. [Indexed LINQ Operations](#6-indexed-linq-operations)
7. [let and into in Query Syntax](#7-let-and-into-in-query-syntax)
8. [Regex with LINQ](#8-regex-with-linq)
9. [Projection & Anonymous Types](#9-projection--anonymous-types)
10. [Additional LINQ Methods](#10-additional-linq-methods)
11. [Edge Cases & Exceptions](#11-edge-cases--exceptions)

---

## 1. What is LINQ?

### تعريف LINQ

**LINQ** = **L**anguage **IN**tegrated **Q**uery

الـ LINQ هو نظام استعلام مدمج في C# بيخليك تعمل استعلامات على أي مصدر بيانات (Collections, Databases, XML, etc.) باستخدام نفس الـ Syntax.

**ليه LINQ موجود؟**  
قبل LINQ كنت لازم تتعلم SQL للـ Database، و XPath للـ XML، و loops للـ Collections. LINQ جاء عشان توحد كل ده في syntax واحد مدمج في الـ Language.

---

### 🔄 LINQ Pipeline — Data Flow

```
┌─────────────────────────────────────────────────────────────┐
│                     LINQ Pipeline                           │
│                                                             │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐  │
│  │  Data Source │───▶│    Query     │───▶│    Result    │  │
│  │              │    │              │    │              │  │
│  │ List<Product>│    │ .Where()     │    │ IEnumerable  │  │
│  │ int[]        │    │ .GroupBy()   │    │ <T>          │  │
│  │ XML / DB     │    │ .Select()    │    │              │  │
│  └──────────────┘    └──────────────┘    └──────────────┘  │
│                                                             │
│  الـ Query مش بيتنفذ غير لما تعمل foreach أو .ToList()!    │
│  (Deferred Execution)                                       │
└─────────────────────────────────────────────────────────────┘
```

---

### Fluent Syntax vs Query Syntax

| الخاصية | Fluent (Method) Syntax | Query Syntax |
|---------|----------------------|--------------|
| الشكل | Method chaining | شبه SQL |
| الاستخدام | أكثر شيوعاً | أقرب للـ SQL |
| المرونة | أكثر مرونة | أوضح في بعض الحالات |

**Fluent Syntax:**
```csharp
var result = ProductsList
    .Where(p => p.UnitsInStock > 0)
    .OrderBy(p => p.Category)
    .Select(p => p.ProductName);
```

**Query Syntax:**
```csharp
var result = from p in ProductsList
             where p.UnitsInStock > 0
             orderby p.Category
             select p.ProductName;
```

> 💡 الاتنين بيعملوا نفس الشغل وبيتحولوا لنفس الـ IL Code في الـ Compiler.

---

## 2. ListGenerator & Data Model

### ملف `ListGenerator.cs` — ما هو ودوره؟

الـ `ListGenerator` هو **Static Class** بيوفر بيانات جاهزة للـ LINQ Examples. بيتعمر مرة واحدة في الـ Static Constructor ويشتغل طول الـ Application.

---

### 📦 The Product Class

```csharp
class Product : IComparable<Product>
{
    public long ProductID { get; set; }       // رقم مميز للمنتج
    public string ProductName { get; set; }    // اسم المنتج
    public string Category { get; set; }       // الفئة (Beverages, Seafood, etc.)
    public decimal UnitPrice { get; set; }     // السعر بالـ decimal لأن فيه فلوس
    public int UnitsInStock { get; set; }      // الكمية المتاحة في المخزن

    // بيخلي المقارنة تبنى على السعر
    public int CompareTo(Product? other)
        => this.UnitPrice.CompareTo(other?.UnitPrice);

    // طريقة عرض المنتج كـ string
    public override string ToString()
        => $"ProductID:{ProductID}, ProductName:{ProductName}, ...";
}
```

**ليه بيعمل `IComparable<Product>`?**  
عشان لو استخدمت `.Sort()` أو `.OrderBy()` من غير ما تحدد Column، يعرف يقارن منتجين ببعض بناءً على السعر تلقائياً.

---

### 🏗️ Static Constructor Pattern

```
┌──────────────────────────────────────────────┐
│           Static Constructor Flow             │
│                                              │
│  App Starts                                  │
│       │                                      │
│       ▼                                      │
│  ListGenerator() runs ONCE                   │
│       │                                      │
│       ├──▶ ProductsList = new List<Product>  │
│       │         77 products added            │
│       │                                      │
│       └──▶ CustomersList = from XML file     │
│                (LINQ to XML parsing)         │
└──────────────────────────────────────────────┘
```

الـ `CustomersList` بيتحمل من **XML File** باستخدام **LINQ to XML** — وده بيوريك إن LINQ مش بس للـ Collections، ممكن تستخدمه على أي data source!

---

### 📊 Product Categories Distribution

```
Categories في الـ ProductsList:
─────────────────────────────────────
Beverages        ████████████  12 منتج
Confections      ███████████   11 منتج
Condiments       ████████      8 منتج
Seafood          █████████     9 منتج
Dairy Products   ████████      8 منتج
Grains/Cereals   ██████        6 منتج
Meat/Poultry     █████         5 منتج
Produce          ████          4 منتج
─────────────────────────────────────
Total:           77 منتج
```

---

## 3. Zip Operator

### ما هو الـ Zip؟

الـ `Zip` بيجمع عنصرين من **ليستين مختلفتين** مع بعض في عنصر واحد — زي ما بتعمل سوستة (Zipper) بتجمع الجانبين!

---

### 🔗 Zip Visual Diagram

```
List A: [ "ibrahim" | "Shafiq" | "Abd-Elshafy" | "muhammad" ]
                                                     ↑ stops here (A has 4 elements)
List B: [    0    |    1    |      2      |    3    |   4   |  ... | 9 ]
             ↑                                                     (B has 10 elements)

Zip Result:
┌──────────────┬───────────────────────────────────┐
│  Iteration   │         Output                    │
├──────────────┼───────────────────────────────────┤
│      1       │  ("ibrahim", 0)                   │
│      2       │  ("Shafiq", 1)                    │
│      3       │  ("Abd-Elshafy", 2)               │
│      4       │  ("muhammad", 3)                  │
│    STOP ⛔   │  List A is exhausted!             │
└──────────────┴───────────────────────────────────┘
```

> ⚠️ الـ Zip بيوقف عند أقصر ليست — العناصر الزيادة في الليست الأكبر بتتجاهل!

---

### 📝 Code Walkthrough

```csharp
List<string> names = new List<string>() { "ibrahim", "Shafiq", "Abd-Elshafy", "muhammad" };
List<int> numbers = Enumerable.Range(0, 10).ToList();
// numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
```

الـ `Enumerable.Range(0, 10)` بيعمل sequence من 0 لـ 9 (10 أرقام).

---

### Overload 1: Returns Tuples (Default)

```csharp
// بيرجع IEnumerable<(string, int)>
var result = names.Zip(numbers);

foreach (var item in result)
    Console.WriteLine(item);  // (ibrahim, 0) ... (muhammad, 3)
```

**Output:**
```
(ibrahim, 0)
(Shafiq, 1)
(Abd-Elshafy, 2)
(muhammad, 3)
```

---

### Overload 2: Result Selector (Custom Output)

```csharp
// بتحدد إيه اللي بيتعمل بالعناصر المتجمعة
var result = names.Zip(numbers, (name, index) => new { Index = index, Name = name });

foreach (var res in result)
    Console.WriteLine(res);
```

**Output:**
```
{ Index = 0, Name = ibrahim }
{ Index = 1, Name = Shafiq }
{ Index = 2, Name = Abd-Elshafy }
{ Index = 3, Name = muhammad }
```

---

### Overload 3: Three-Way Zip (.NET 6+)

```csharp
// ممكن تعمل Zip على 3 ليستات مع بعض
List<string> firstNames = new() { "Ibrahim", "Ahmed", "Sara" };
List<string> lastNames  = new() { "Hassan",  "Ali",   "Mohamed" };
List<int>    ages       = new() { 25,        30,      28 };

var result = firstNames.Zip(lastNames, ages)
                       .Select(t => $"{t.First} {t.Second}, Age: {t.Third}");

foreach (var r in result)
    Console.WriteLine(r);
// Ibrahim Hassan, Age: 25
// Ahmed Ali, Age: 30
// Sara Mohamed, Age: 28
```

---

### 🧪 Additional Zip Example: Combine Questions with Answers

```csharp
var questions = new[] { "ما اسمك؟", "كم عمرك؟", "من أين أنت؟" };
var answers   = new[] { "Ibrahim",   "25",         "Egypt" };

var qa = questions.Zip(answers, (q, a) => $"Q: {q}\nA: {a}");
foreach (var item in qa)
    Console.WriteLine(item + "\n---");
```

---

### ⚠️ Edge Cases

```
┌─────────────────────────────────────────────────────┐
│                   Zip Edge Cases                    │
├─────────────────────────────────────────────────────┤
│ ✅ Normal: A=4, B=10  → Result = 4 elements         │
│ ✅ Equal:  A=5, B=5   → Result = 5 elements         │
│ ✅ Empty:  A=0, B=5   → Result = 0 elements (empty) │
│ ✅ Empty:  A=5, B=0   → Result = 0 elements (empty) │
│ ❌ Null:   A=null     → NullReferenceException!     │
└─────────────────────────────────────────────────────┘
```

---

## 4. GroupBy Operator

### ما هو الـ GroupBy؟

الـ `GroupBy` بياخد الـ Collection ويقسمها لـ **مجموعات** بناءً على قيمة معينة (الـ Key). نتيجته بتبقى `IEnumerable<IGrouping<TKey, TElement>>`.

---

### 🗂️ GroupBy Visual Diagram

```
ProductsList (77 منتج مختلط):
┌────────────────────────────────────────────────────────┐
│ Chai(Bev) │ Salmon(Sea) │ Camembert(Dairy) │ Chai... │
└────────────────────────────────────────────────────────┘
                    ↓ .GroupBy(p => p.Category)
┌──────────────────────────────────────────────────────────────────┐
│  Key: "Beverages"   │  Key: "Seafood"     │  Key: "Dairy"        │
│  ┌───────────────┐  │  ┌───────────────┐  │  ┌───────────────┐   │
│  │ Chai          │  │  │ Salmon        │  │  │ Camembert     │   │
│  │ Chang         │  │  │ Ikura         │  │  │ Queso         │   │
│  │ Guaraná       │  │  │ Konbu         │  │  │ Geitost       │   │
│  │ ...           │  │  │ ...           │  │  │ ...           │   │
│  └───────────────┘  │  └───────────────┘  │  └───────────────┘   │
└──────────────────────────────────────────────────────────────────┘

IGrouping هو زي Dictionary<string, List<Product>>
   Key    =  Category name
   Values =  List of products in that category
```

---

### 📝 Fluent Syntax — Step by Step

```csharp
var result = ProductsList
    .Where(product => product.UnitsInStock > 0)        // 1️⃣ فلتر المنتجات المتاحة
    .GroupBy(product => product.Category)              // 2️⃣ جمّعهم حسب الفئة
    .Where(product => product.Count() > 10)            // 3️⃣ فلتر المجموعات الكبيرة
    .OrderByDescending(product => product.Count())     // 4️⃣ رتبهم تنازلياً
    .Select(product => new                             // 5️⃣ اختار البيانات المطلوبة
    {
        Category = product.Key,
        Count    = product.Count()
    });
```

**شرح كل سطر:**

| السطر | الشرح |
|-------|-------|
| `.Where(UnitsInStock > 0)` | بناخد بس المنتجات اللي متاحة في المخزن |
| `.GroupBy(p.Category)` | بنقسم المنتجات لمجموعات حسب الـ Category |
| `.Where(Count() > 10)` | بناخد بس المجموعات اللي فيها أكتر من 10 منتجات |
| `.OrderByDescending(Count())` | بنرتبها من الأكبر للأصغر |
| `.Select(new { ... })` | بنعمل Projection لاختيار الـ Key والـ Count بس |

---

### Iterating Grouped Results

```csharp
// لو عايز تشوف كل منتج داخل كل مجموعة:
foreach (var group in result)
{
    Console.WriteLine($"Category: {group.Key}");  // اسم المجموعة
    foreach (var item in group)                    // المنتجات داخل المجموعة
    {
        Console.WriteLine($"  - {item.ProductName}");
    }
}
```

---

### 📝 Query Syntax

```csharp
var result = from product in ProductsList
             where product.UnitsInStock > 0
             group product by product.Category   // هنا بيعمل الـ GroupBy
             into productGroup                   // into بتحفظ المجموعة في variable جديد
             where productGroup.Count() > 10     // فلتر على المجموعة نفسها
             orderby productGroup.Count() descending
             select new
             {
                 category = productGroup.Key,
                 Count    = productGroup.Count()
             };
```

**ليه بنحتاج `into` هنا؟**

```
┌──────────────────────────────────────────────────────────────┐
│                    into Keyword Flow                         │
│                                                              │
│  from product in ProductsList                                │
│       │                                                      │
│       ▼                                                      │
│  where product.UnitsInStock > 0                              │
│       │                                                      │
│       ▼                                                      │
│  group product by product.Category                           │
│       │                                                      │
│       │  ← هنا اتغير السياق! مش عندنا "product" تاني        │
│       ▼                                                      │
│  into productGroup  ← اسم جديد للمجموعة عشان نفلتر عليها   │
│       │                                                      │
│       ▼                                                      │
│  where productGroup.Count() > 10                             │
│       │                                                      │
│       ▼                                                      │
│  select productGroup.Key                                     │
└──────────────────────────────────────────────────────────────┘
```

---

### 🔗 GroupBy Returns IGrouping

```csharp
// IGrouping<TKey, TElement> يشبه Dictionary لكن read-only
IEnumerable<IGrouping<string, Product>> groups = ProductsList.GroupBy(p => p.Category);

foreach (IGrouping<string, Product> group in groups)
{
    string key = group.Key;            // اسم الـ Category
    int count  = group.Count();        // عدد المنتجات
    Product first = group.First();     // أول منتج في المجموعة
}
```

---

### 🧪 Additional GroupBy Examples

```csharp
// GroupBy مع Projection (ToLookup)
var lookup = ProductsList.ToLookup(p => p.Category);
// ToLookup بيحسب مرة واحدة في الـ Memory (vs GroupBy اللي بيؤجل التنفيذ)

// GroupBy on multiple keys (Composite Key)
var result = ProductsList.GroupBy(p => new { p.Category, InStock = p.UnitsInStock > 0 });
```

---

## 5. Partitioning Operators

### ما هو الـ Partitioning؟

الـ Partitioning Operators بتقسم الـ Sequence لجزأين: جزء تاخده وجزء تتجاهله.

---

### 🗺️ Partitioning Operators Overview

```
Original Sequence:
[ 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 ]

Take(3):
[ 1 | 2 | 3 ] ← خد أول 3 فقط

TakeLast(3):
                        [ 8 | 9 | 10 ] ← خد آخر 3 فقط

Skip(3):
            [ 4 | 5 | 6 | 7 | 8 | 9 | 10 ] ← تجاهل أول 3

SkipLast(3):
[ 1 | 2 | 3 | 4 | 5 | 6 | 7 ] ← تجاهل آخر 3

TakeWhile(x => x < 5):
[ 1 | 2 | 3 | 4 ] ← خد طالما الشرط صح، وقف عند 5

SkipWhile(x => x < 5):
                    [ 5 | 6 | 7 | 8 | 9 | 10 ] ← تجاوز طالما الشرط صح
```

---

### Take()

```csharp
// بياخد أول عدد محدد من العناصر
var takeResult = ProductsList
    .Where(product => product.UnitsInStock == 0)  // المنتجات المنتهية
    .Take(3);                                      // خد أول 3 بس

foreach (var item in takeResult)
    Console.WriteLine(item);
```

**Output (أول 3 منتجات بـ UnitsInStock = 0):**
```
Chef Anton's Gumbo Mix    (ID: 5)
Alice Mutton              (ID: 17)
Thüringer Rostbratwurst   (ID: 29)
```

---

### TakeLast()

```csharp
// بياخد آخر عدد محدد من العناصر
var takeLastResult = ProductsList
    .Where(product => product.UnitsInStock == 0)
    .TakeLast(3);  // خد آخر 3
```

```
┌─────────────────────────────────────────────────────────┐
│  Out-of-Stock Products:                                 │
│  [5] Chef Anton ← TakeLast(3) تتجاهل ده                │
│  [17] Alice Mutton ← وده                               │
│  [29] Thüringer ← وده                                  │
│  [31] Gorgonzola ← TakeLast(3) تاخد ده ✅              │
│  [53] Perth Pasties ← وده ✅                            │
│  [74] Longlife Tofu ← وده ✅                            │
└─────────────────────────────────────────────────────────┘
```

---

### Skip()

```csharp
// بيتخطى أول عدد محدد من العناصر ويرجع الباقي
var skipResult = ProductsList
    .Where(product => product.UnitsInStock == 0)
    .Skip(2);  // تجاهل أول 2 وخد الباقي
```

---

### SkipLast()

```csharp
// بيتخطى آخر عدد محدد من العناصر ويرجع الباقي
var skipLastResult = ProductsList
    .Where(product => product.UnitsInStock == 0)
    .SkipLast(2);  // تجاهل آخر 2 وخد الباقي
```

---

### 📄 Pagination Pattern

الـ Pagination مهم جداً في الـ Web Apps عشان متحملش كل البيانات مرة واحدة.

```
┌─────────────────────────────────────────────────────────────────┐
│                    Pagination Formula                           │
│                                                                 │
│  PageIndex = 1  (0-based) أو 2 (1-based)                        │
│  PageSize  = 10                                                 │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  0-Based Pagination:                                    │   │
│  │  Skip(PageIndex * PageSize).Take(PageSize)              │   │
│  │                                                         │   │
│  │  Page 0: Skip(0).Take(10)  → items 1-10                 │   │
│  │  Page 1: Skip(10).Take(10) → items 11-20                │   │
│  │  Page 2: Skip(20).Take(10) → items 21-30                │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  1-Based Pagination (ما بيجي من الـ UI):                │   │
│  │  var skipAmount = PageSize * (PageIndex - 1);            │   │
│  │  Skip(skipAmount).Take(PageSize)                        │   │
│  │                                                         │   │
│  │  Page 1: Skip(0).Take(10)  → items 1-10                 │   │
│  │  Page 2: Skip(10).Take(10) → items 11-20                │   │
│  └─────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

```csharp
// Example: Pagination Implementation
int pageIndex = 2;   // الصفحة التانية (1-based)
int pageSize  = 10;  // 10 منتجات في الصفحة

var pagedProducts = ProductsList
    .Skip(pageSize * (pageIndex - 1))  // Skip(10)
    .Take(pageSize);                    // Take(10)

// Page 2 → items 11 to 20
```

---

### TakeWhile()

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 7, 2, 0 };

// بياخد العناصر طالما الشرط صح
// لما الشرط يفشل أول مرة يوقف (حتى لو العناصر الجاية تحقق الشرط!)
var takeWhileResult = numbers.TakeWhile(num => num > 3);

foreach (int num in takeWhileResult)
    Console.WriteLine(num);
```

```
Sequence:   [ 5 | 4 | 1 | 3 | 9 | 8 | 7 | 2 | 0 ]

Step 1: 5 > 3? ✅ → خد
Step 2: 4 > 3? ✅ → خد
Step 3: 1 > 3? ❌ → وقف! (رغم إن 9 و8 و7 بعدها كبيرة)

Output: 5, 4
```

> 🔑 الفرق بين **TakeWhile** و **Where**: الـ Where بيفحص كل عنصر لوحده، لكن TakeWhile بيوقف أول ما يلاقي عنصر مش حاقق الشرط!

---

### TakeWhile with Index (Second Overload)

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 7, 2, 0 };

// Overload التاني: بياخد العنصر والـ Index
var takeWhileIndexed = numbers.TakeWhile((num, index) => num > index);

foreach (int num in takeWhileIndexed)
    Console.WriteLine(num);
```

```
Index:  [ 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 ]
Value:  [ 5 | 4 | 1 | 3 | 9 | 8 | 7 | 2 | 0 ]

Step 0: 5 > 0? ✅ → خد 5
Step 1: 4 > 1? ✅ → خد 4
Step 2: 1 > 2? ❌ → وقف!

Output: 5, 4
```

---

### SkipWhile()

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 7, 2, 0 };

// بيتخطى العناصر طالما الشرط صح
// لما الشرط يفشل أول مرة، بياخد باقي العناصر كلها!
var skipWhileResult = numbers.SkipWhile(num => num % 3 != 0);

foreach (int num in skipWhileResult)
    Console.WriteLine(num);
```

```
Sequence:    [ 5 | 4 | 1 | 3 | 9 | 8 | 7 | 2 | 0 ]

Step 1: 5 % 3 != 0? ✅ → تجاهل
Step 2: 4 % 3 != 0? ✅ → تجاهل
Step 3: 1 % 3 != 0? ✅ → تجاهل
Step 4: 3 % 3 != 0? ❌ → ابدأ تاخد من هنا!
Step 5: خد 9 (حتى لو 9 % 3 == 0، لأن الشرط اتكسر قبل كده)

Output: 3, 9, 8, 7, 2, 0
```

---

### 📊 Comparison Table

```
┌───────────────┬──────────────────────────────────────────────────┐
│   Operator    │              Behavior                            │
├───────────────┼──────────────────────────────────────────────────┤
│ Take(n)       │ خد أول n عنصر                                   │
│ TakeLast(n)   │ خد آخر n عنصر                                   │
│ Skip(n)       │ تجاهل أول n عنصر                                │
│ SkipLast(n)   │ تجاهل آخر n عنصر                               │
│ TakeWhile(f)  │ خد طالما الشرط صح (وقف عند أول فشل)            │
│ SkipWhile(f)  │ تجاهل طالما الشرط صح (خد من أول فشل للآخر)     │
└───────────────┴──────────────────────────────────────────────────┘
```

---

## 6. Indexed LINQ Operations

### Select with Index

ممكن تستخدم `Select` مع **Index** عشان تعرف مكان كل عنصر في الـ Sequence.

```csharp
int[] numbers = { 5, 4, 1, 3, 9, 8, 7, 2, 0 };

// (value, index) ← الـ Lambda بتاخد عنصرين
var result = numbers.Select((num, index) =>
    num > index
        ? $"✅ number={num} > index={index}"
        : $"❌ number={num} ≤ index={index}"
);

foreach (string line in result)
    Console.WriteLine(line);
```

**Visual:**

```
┌─────────────────────────────────────────────────────────────────┐
│  Index →  Element →  Predicate (num > index?) →  Output         │
├───────┬──────────┬────────────────────────────┬────────────────┤
│   0   │    5     │      5 > 0 ? YES            │  ✅ num=5      │
│   1   │    4     │      4 > 1 ? YES            │  ✅ num=4      │
│   2   │    1     │      1 > 2 ? NO             │  ❌ index=2    │
│   3   │    3     │      3 > 3 ? NO             │  ❌ index=3    │
│   4   │    9     │      9 > 4 ? YES            │  ✅ num=9      │
│   5   │    8     │      8 > 5 ? YES            │  ✅ num=8      │
│   6   │    7     │      7 > 6 ? YES            │  ✅ num=7      │
│   7   │    2     │      2 > 7 ? NO             │  ❌ index=7    │
│   8   │    0     │      0 > 8 ? NO             │  ❌ index=8    │
└───────┴──────────┴────────────────────────────┴────────────────┘
```

---

### Where with Index

```csharp
// ممكن كمان تعمل Where مع الـ Index
var evenIndexItems = numbers.Where((num, index) => index % 2 == 0);
// بياخد العناصر اللي في الـ Index الزوجي (0, 2, 4, 6, 8)
// Output: 5, 1, 9, 7, 0
```

---

## 7. let and into in Query Syntax

### المشكلة اللي بتحلها `into` و`let`

في Query Syntax، بعد ما تعمل `group by` أو `select`، بتحتاج تحفظ النتيجة في **variable جديد** عشان تكمل الـ Query.

---

### `into` Keyword

```csharp
List<string> names = new() { "Ibrahim", "Shafiq", "Abd-Elshafy", "Muhammad", "Hello", "World" };

var deletedVowels = from name in names
                    select Regex.Replace(name, "[aeoiuAEOIU]", String.Empty)
                    into newName                  // هنا بنحفظ النتيجة في newName
                    where newName.Length >= 3     // بنفلتر على الاسم الجديد
                    select newName;

foreach (var name in deletedVowels)
    Console.WriteLine(name);
```

**Flow Diagram:**

```
┌─────────────────────────────────────────────────────────────────┐
│                     into Flow                                   │
│                                                                 │
│  names: ["Ibrahim", "Shafiq", "Abd-Elshafy", "Muhammad", ...]   │
│       │                                                         │
│       ▼ select Regex.Replace(...)                               │
│  ["brhm", "Shfq", "bd-lshfy", "Mhmmd", "Hll", "Wrld"]         │
│       │                                                         │
│       ▼ into newName                                            │
│  newName = ["brhm", "Shfq", "bd-lshfy", "Mhmmd", "Hll", "Wrld"]│
│       │                                                         │
│       ▼ where newName.Length >= 3                               │
│  ["brhm", "Shfq", "bd-lshfy", "Mhmmd", "Hll", "Wrld"]         │
│  (كلهم ≥ 3 حروف)                                               │
│       │                                                         │
│       ▼ select newName                                          │
│  Final Output: ["brhm", "Shfq", "bd-lshfy", "Mhmmd", "Hll"...] │
└─────────────────────────────────────────────────────────────────┘
```

---

### `let` Keyword (الأسهل والأنضف)

```csharp
var letResult = from name in names
                let newName = Regex.Replace(name, "[aeiouAEIOU]", String.Empty)
                // let بيعمل variable مؤقت في نفس الـ Scope
                where newName.Length >= 3
                select newName;
                // ممكن كمان نرجع name الأصلي مع newName!

foreach (string name in letResult)
    Console.WriteLine(name);
```

**الفرق بين `let` و`into`:**

```
┌─────────────────────────────────────────────────────────────────┐
│  let vs into                                                    │
├─────────────────────┬───────────────────────────────────────────┤
│      let            │              into                         │
├─────────────────────┼───────────────────────────────────────────┤
│ بيعمل variable جنب  │ بيقطع الـ Query ويبدأ جديد                │
│ المتغير الأصلي       │                                          │
├─────────────────────┼───────────────────────────────────────────┤
│ ممكن تستخدم name    │ مش ممكن تستخدم name بعد into              │
│ و newName معاً       │                                          │
├─────────────────────┼───────────────────────────────────────────┤
│ let newName = ...   │ select ...  into newName                  │
├─────────────────────┼───────────────────────────────────────────┤
│ ✅ الأفضل للاستخدام  │ ✅ مطلوب بعد group by                    │
└─────────────────────┴───────────────────────────────────────────┘
```

**مثال يوضح قوة `let`:**

```csharp
// مع let ممكن ترجع الاسم الأصلي والمعدل
var letResult = from name in names
                let newName = Regex.Replace(name, "[aeiouAEIOU]", String.Empty)
                where newName.Length >= 3
                select new { Original = name, WithoutVowels = newName };
// ✅ ممكن تستخدم name هنا لأن let ما قطعش الـ Scope!
```

---

## 8. Regex with LINQ

### ما هو Regex؟

**Regex** = **Regular Expression** = نمط (Pattern) بتستخدمه عشان تبحث عن أو تعدل نصوص بطريقة متقدمة.

---

### `Regex.Replace` — الاستخدام في الكود

```csharp
// Pattern: [aeiouAEIOU]
// ده معناه: أي حرف من دول (a أو e أو i أو o أو u سواء صغير أو كبير)
// String.Empty = "" ← بدّل الحرف بـ إيه؟ بـ لا إيه (احذفه)
Regex.Replace(name, "[aeiouAEIOU]", String.Empty)
```

---

### 🔤 Regex Visual: Removing Vowels

```
Input: "Ibrahim"

I  b  r  a  h  i  m
↓  ↓  ↓  ↓  ↓  ↓  ↓
❌ ✅ ✅ ❌ ✅ ❌ ✅
(I) (b) (r) (a) (h) (i) (m)

حروف العلة (Vowels): I, a, i → اتحذفوا
باقي الحروف: b, r, h, m → اتبقوا

Output: "brhm"
```

```
"Shafiq"   → "Shfq"
"Abd-Elshafy" → "bd-lshfy"
"Muhammad" → "Mhmmd"
"Hello"    → "Hll"
"World"    → "Wrld"
```

---

### Regex Pattern Syntax

```
┌──────────────────────────────────────────────────────────────┐
│                   Common Regex Patterns                      │
├───────────────────┬──────────────────────────────────────────┤
│    Pattern        │           Meaning                        │
├───────────────────┼──────────────────────────────────────────┤
│ [aeiou]           │ أي حرف علة صغير                         │
│ [AEIOU]           │ أي حرف علة كبير                         │
│ [aeiouAEIOU]      │ أي حرف علة (كبير أو صغير)               │
│ [a-z]             │ أي حرف إنجليزي صغير                     │
│ [A-Z]             │ أي حرف إنجليزي كبير                     │
│ [0-9]             │ أي رقم                                   │
│ \d                │ أي رقم (مثل [0-9])                       │
│ \w                │ أي حرف أو رقم أو underscore              │
│ \s                │ أي whitespace (space, tab, etc.)         │
│ .                 │ أي حرف (ماعدا newline)                  │
│ ^                 │ بداية النص                               │
│ $                 │ نهاية النص                               │
│ +                 │ مرة أو أكتر                              │
│ *                 │ صفر أو أكتر                              │
│ ?                 │ صفر أو مرة واحدة                         │
└───────────────────┴──────────────────────────────────────────┘
```

---

### `Regex.Replace` Overloads

```csharp
// Overload 1: Basic (Static)
string result = Regex.Replace("Hello World", "[aeiou]", "*");
// "H*ll* W*rld"

// Overload 2: With RegexOptions
string result2 = Regex.Replace("Hello World", "[aeiou]", "*",
    RegexOptions.IgnoreCase);
// "H*ll* W*rld" (case-insensitive)
// يعني مش هتحتاج تكتب [aeiouAEIOU] وتكفيك [aeiou]

// Overload 3: With MatchEvaluator (Function)
string result3 = Regex.Replace("hello 2024", @"\d", m => (int.Parse(m.Value) + 1).ToString());
// "hello 3135" ← بيزود كل رقم بـ 1!

// Overload 4: Instance-based (أسرع لو بتستخدم نفس الـ Pattern كتير)
var regex = new Regex("[aeiouAEIOU]");
string result4 = regex.Replace("Ibrahim", String.Empty);
// "brhm"
```

---

### Other Useful Regex Methods

```csharp
// Regex.IsMatch - هل النص بيطابق الـ Pattern؟
bool isEmail = Regex.IsMatch("test@gmail.com", @"^[\w.-]+@[\w.-]+\.\w+$");
// true

// Regex.Match - إيجاد أول تطابق
Match match = Regex.Match("My phone: 01012345678", @"\d{11}");
if (match.Success)
    Console.WriteLine(match.Value);  // "01012345678"

// Regex.Matches - إيجاد كل التطابقات
MatchCollection matches = Regex.Matches("one 1 two 2 three 3", @"\d");
foreach (Match m in matches)
    Console.WriteLine(m.Value);  // 1, 2, 3

// Regex.Split - تقسيم النص بالـ Pattern
string[] parts = Regex.Split("hello  world   foo", @"\s+");
// ["hello", "world", "foo"]
```

---

### ⚠️ Edge Cases: Invalid Regex Pattern

```csharp
try
{
    // Pattern غلط - Bracket مش متقفل
    var result = Regex.Replace("test", "[aeiou", "");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid pattern: {ex.Message}");
    // ArgumentException: parsing "[aeiou" - Unterminated [] set.
}
```

---

## 9. Projection & Anonymous Types

### Discount Products Example

```csharp
// Fluent Syntax
var discountProducts = ProductsList
    .Select(p => new          // Projection لـ Anonymous Type جديد
    {
        ID              = p.ProductID,
        Name            = p.ProductName,
        ProductCategory = p.Category,
        Count           = p.UnitsInStock,
        NewPrice        = p.UnitPrice * 0.8M   // خصم 20%
    })
    .Where(p => p.Count > 10);  // فلتر على الـ Anonymous Type الجديد
```

---

### ليه `0.8M` وليس `0.8`؟

```
┌──────────────────────────────────────────────────────────────────┐
│                     Decimal Literal Suffix                       │
│                                                                  │
│  0.8    → double (64-bit floating point)                         │
│           ممكن يعمل precision errors في الحسابات المالية         │
│                                                                  │
│  0.8M   → decimal (128-bit high-precision)                       │
│           مناسب للفلوس والحسابات المالية                         │
│                                                                  │
│  UnitPrice هو decimal → لازم نضرب في decimal                    │
│  لو ضربنا في double → Compiler Error!                           │
│                                                                  │
│  مثال:                                                           │
│  double d = 0.1 + 0.2;   → 0.30000000000000004 ❌              │
│  decimal m = 0.1M + 0.2M; → 0.3 ✅                             │
└──────────────────────────────────────────────────────────────────┘
```

---

### Query Syntax with `let` for Projection

```csharp
// المشكلة: في Query Syntax مش ممكن تعمل Select ثم Where على الـ Anonymous Type مباشرة
// الحل: let!

var discountProducts = from p in ProductsList
                       let newProduct = new           // بنحفظ الـ Anonymous Type في let
                       {
                           ID              = p.ProductID,
                           Name            = p.ProductName,
                           ProductCategory = p.Category,
                           Count           = p.UnitsInStock,
                           NewPrice        = p.UnitPrice * 0.8M
                       }
                       where newProduct.Count > 10    // بنفلتر على الـ newProduct
                       select newProduct;             // بنرجع الـ newProduct
```

**Flow:**

```
ProductsList (77 items)
       │
       ▼ let newProduct = new { ID, Name, Category, Count, NewPrice=Price*0.8 }
Anonymous Objects (77 items with discounted price)
       │
       ▼ where newProduct.Count > 10
Filtered Products (count > 10 in stock)
       │
       ▼ select newProduct
Final Result: IEnumerable<Anonymous>
```

---

### 🧪 Full Executable Example

```csharp
using System;
using System.Linq;
using System.Collections.Generic;

// تشغيل مثال كامل
var products = new List<(string Name, string Category, decimal Price, int Stock)>
{
    ("Chai",    "Beverages",  18.00M, 100),
    ("Chang",   "Beverages",  19.00M, 17),
    ("Tofu",    "Produce",    23.25M, 35),
    ("Ikura",   "Seafood",    31.00M, 31),
    ("OutOfStock", "Seafood", 10.00M, 0),
};

var discounted = from p in products
                 let newP = new { p.Name, p.Category, Count = p.Stock, NewPrice = p.Price * 0.8M }
                 where newP.Count > 10
                 orderby newP.NewPrice
                 select newP;

foreach (var p in discounted)
    Console.WriteLine($"{p.Name,-15} {p.Category,-12} Stock:{p.Count,4} NewPrice:{p.NewPrice:F2}");

/* Output:
Chai            Beverages   Stock: 100 NewPrice:14.40
Chang           Beverages   Stock:  17 NewPrice:15.20
Ikura           Seafood     Stock:  31 NewPrice:24.80
Tofu            Produce     Stock:  35 NewPrice:18.60
*/
```

---

## 10. Additional LINQ Methods

### Chunk (.NET 6+)

```csharp
// بيقسم الـ Sequence لـ chunks (قطع) بحجم محدد
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var chunks = numbers.Chunk(3);
// [[1,2,3], [4,5,6], [7,8,9], [10]]

foreach (var chunk in chunks)
    Console.WriteLine(string.Join(", ", chunk));

// Output:
// 1, 2, 3
// 4, 5, 6
// 7, 8, 9
// 10
```

```
[ 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 ]
  \_____/   \_____/   \_____/   \__/
  chunk1    chunk2    chunk3    chunk4(آخر chunk أصغر)
```

---

### DistinctBy (.NET 6+)

```csharp
// بدل ما تعمل GroupBy وتاخد أول عنصر من كل مجموعة
// استخدم DistinctBy مباشرة!
var uniqueCategories = ProductsList.DistinctBy(p => p.Category);
// بيرجع أول منتج من كل Category

// مثال آخر
var people = new[] {
    new { Name = "Ahmed",   City = "Cairo"  },
    new { Name = "Sara",    City = "Cairo"  },
    new { Name = "Mohamed", City = "Alex"   },
};

var uniqueCities = people.DistinctBy(p => p.City);
// [ Ahmed(Cairo), Mohamed(Alex) ]  ← سارة اتشالت لأن Cairo متكررة
```

---

### GroupJoin

```csharp
// زي LEFT OUTER JOIN في SQL
// بيربط كل عنصر من الـ Outer بـ قائمة من الـ Inner

var categories = new[] { "Beverages", "Seafood", "Produce" };

var result = categories.GroupJoin(
    ProductsList,
    category => category,                    // Key من الـ categories
    product  => product.Category,            // Key من الـ products
    (category, products) => new              // بنعمل إيه بيهم
    {
        Category = category,
        Products = products.ToList(),
        Count    = products.Count()
    });

foreach (var group in result)
    Console.WriteLine($"{group.Category}: {group.Count} products");
```

```
Beverages → [Chai, Chang, Guaraná, ...]   (12 products)
Seafood   → [Ikura, Konbu, Salmon, ...]   (9 products)
Produce   → [Tofu, Dried Pears, ...]      (4 products)
```

---

### MinBy / MaxBy (.NET 6+)

```csharp
// بدل ما تعمل OrderBy().First()
var cheapestProduct  = ProductsList.MinBy(p => p.UnitPrice);
var mostExpensive    = ProductsList.MaxBy(p => p.UnitPrice);

Console.WriteLine(cheapestProduct?.ProductName);   // Geitost ($2.50)
Console.WriteLine(mostExpensive?.ProductName);     // Côte de Blaye ($263.50)
```

---

### Aggregate

```csharp
// زي الـ Reduce في JavaScript - بيطبق عملية تراكمية
int[] numbers = { 1, 2, 3, 4, 5 };

int sum = numbers.Aggregate((acc, next) => acc + next);  // 15
int product = numbers.Aggregate((acc, next) => acc * next);  // 120

// مع قيمة ابتدائية
int sumWith100 = numbers.Aggregate(100, (acc, next) => acc + next);  // 115
```

---

## 11. Edge Cases & Exceptions

### Empty Sequences

```csharp
var empty = new List<Product>();

// ✅ هذه المعاملات آمنة مع Empty:
empty.Where(p => p.UnitsInStock > 0)  // → empty sequence
empty.Take(3)                          // → empty sequence
empty.Skip(3)                          // → empty sequence
empty.GroupBy(p => p.Category)        // → empty sequence

// ⚠️ هذه بترمي Exception لو Empty:
empty.First()      // → InvalidOperationException!
empty.Single()     // → InvalidOperationException!
empty.Min()        // → InvalidOperationException!

// ✅ استخدم هذه بدلاً:
empty.FirstOrDefault()   // → null أو default value
empty.SingleOrDefault()  // → null أو default value
empty.MinBy(p => p.UnitPrice)  // → null
```

---

### GroupBy on Null Keys

```csharp
var productsWithNullCategory = new List<Product>
{
    new Product { ProductName = "Test1", Category = null },
    new Product { ProductName = "Test2", Category = "Beverages" },
};

var grouped = productsWithNullCategory.GroupBy(p => p.Category);
// ✅ GroupBy يقبل null كـ Key!
// هيعمل مجموعة واحدة بـ Key = null

foreach (var group in grouped)
    Console.WriteLine(group.Key ?? "NULL");
// NULL
// Beverages
```

---

### Zip with Unequal Lengths

```csharp
// ✅ Zip آمن مع أطوال مختلفة - بيوقف عند الأقصر
var a = new[] { 1, 2, 3, 4, 5 };
var b = new[] { "one", "two" };

var result = a.Zip(b);  // → [(1,"one"), (2,"two")] فقط

// ⚠️ لكن لو أي منهم null:
List<int> nullList = null!;
a.Zip(nullList);  // → NullReferenceException!
```

---

### TakeWhile vs Where: Common Mistake

```csharp
int[] numbers = { 5, 1, 8, 3, 9 };

// ❌ المبتدئون أحياناً يخلطوا بينهم
var wrong = numbers.TakeWhile(n => n > 2);
// Output: [5] فقط! (لأن 1 < 2 فوقف فوراً)

var correct = numbers.Where(n => n > 2);
// Output: [5, 8, 3, 9] (بيفحص كل عنصر)
```

---

## 🎯 Quick Reference Card

```
┌──────────────────────────────────────────────────────────────────────┐
│                    LINQ Quick Reference                              │
├─────────────────┬────────────────────────────────────────────────────┤
│ Zip             │ يجمع عنصرين من ليستين، يوقف عند الأقصر           │
├─────────────────┼────────────────────────────────────────────────────┤
│ GroupBy         │ يقسم لمجموعات → IEnumerable<IGrouping<K,V>>       │
├─────────────────┼────────────────────────────────────────────────────┤
│ Take(n)         │ أول n عناصر                                        │
│ TakeLast(n)     │ آخر n عناصر                                        │
│ Skip(n)         │ تجاهل أول n                                        │
│ SkipLast(n)     │ تجاهل آخر n                                        │
│ TakeWhile(f)    │ خد طالما صح (وقف عند أول فشل)                     │
│ SkipWhile(f)    │ تجاهل طالما صح (خد من أول فشل)                    │
├─────────────────┼────────────────────────────────────────────────────┤
│ Select(v,i)     │ Projection مع Index                                │
│ Where(v,i)      │ Filter مع Index                                    │
├─────────────────┼────────────────────────────────────────────────────┤
│ let             │ Variable مؤقت في نفس الـ Scope                     │
│ into            │ تقطع الـ Query وتبدأ Scope جديد (مطلوب بعد group) │
├─────────────────┼────────────────────────────────────────────────────┤
│ Regex.Replace   │ يبدل النص بناءً على Pattern                       │
│ Regex.IsMatch   │ يتحقق إذا النص يطابق Pattern                     │
│ Regex.Matches   │ يرجع كل التطابقات                                  │
├─────────────────┼────────────────────────────────────────────────────┤
│ Chunk(n)        │ يقسم الـ Sequence لـ chunks (.NET 6+)              │
│ DistinctBy(f)   │ يزيل التكرار بناءً على Key (.NET 6+)              │
│ MinBy/MaxBy(f)  │ أصغر/أكبر عنصر بناءً على Key (.NET 6+)            │
│ GroupJoin       │ LEFT OUTER JOIN                                     │
└─────────────────┴────────────────────────────────────────────────────┘
```

---

## 📁 Project Structure

```
LINQ03/
├── Program.cs           ← Main examples
├── ListGenerator.cs     ← Data models & sample data
├── Customers.xml        ← Customer data (loaded via LINQ to XML)
└── README.md            ← هذا الملف 📖
```

---

## 🚀 Running the Examples

```bash
# Clone the project
git clone <your-repo-url>
cd LINQ03

# Run
dotnet run
```

> **ملاحظة:** الكود في `Program.cs` معظمه في `foreach` معلّقة بـ `//`.  
> شيل الـ Comment عن الـ `foreach` المطلوبة عشان تشوف النتيجة.

---

*📚 Made with ❤️ for Egyptian .NET Developers — Study hard, ship great code!*