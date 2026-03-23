# 🚀 LINQ

> **مرجع شامل للـ LINQ في C# — اذاكر منه قبل أي شغل أو Interview**

---

## 📋 جدول المحتويات

| # | الموضوع |
|---|---------|
| 1 | [ما هو LINQ؟](#-ما-هو-linq) |
| 2 | [Query Syntax vs Fluent Syntax](#-query-syntax-vs-fluent-syntax) |
| 3 | [Projection Operators - Select](#-select) |
| 4 | [Projection Operators - SelectMany](#-selectmany) |
| 5 | [Filtering - Where & OfType](#-filtering-operators) |
| 6 | [Ordering Operators](#-ordering-operators) |
| 7 | [Element Operators](#-element-operators) |
| 8 | [Aggregate Operators](#-aggregate-operators) |
| 9 | [Set Operators](#-set-operators) |
| 10 | [Generator Operators](#-generator-operators) |
| 11 | [Casting Operators](#-casting-operators) |
| 12 | [Quantifiers](#-quantifiers) |
| 13 | [Zip Operator](#-zip-operator) |
| 14 | [Hybrid Syntax](#-hybrid-syntax) |
| 15 | [Anonymous Types](#-anonymous-types) |
| 16 | [Exceptions in LINQ](#-exceptions-in-linq) |

---

## 🧠 ما هو LINQ؟

**LINQ = Language Integrated Query**

ببساطة، LINQ هو طريقة بتقدر بيها تعمل **استعلامات (Queries)** على أي Collection أو DataSource في C# وكأنك بتكتب SQL — بس في C# نفسها!

### ليه اتعمل LINQ؟

قبل LINQ كنت لازم تكتب كود زي ده عشان تفلتر List:

```csharp
// ❌ الطريقة القديمة — قبل LINQ
List<Product> cheapProducts = new List<Product>();
foreach (var product in ProductsList)
{
    if (product.UnitPrice < 20)
        cheapProducts.Add(product);
}
```

بعد LINQ:

```csharp
// ✅ بعد LINQ — سطر واحد!
var cheapProducts = ProductsList.Where(p => p.UnitPrice < 20).ToList();
```

### LINQ بيشتغل على إيه؟

```
IEnumerable<T>  →  أي Collection: List, Array, HashSet, ...
IQueryable<T>   →  Databases, Entity Framework
XDocument       →  XML
```

### LINQ Pipeline — إزاي بيشتغل

```
┌─────────────────────────────────────────────────────────┐
│                    LINQ Pipeline                        │
│                                                         │
│  Data Source  →  Operators  →  Operators  →  Result    │
│  (List/Array)    (Where)       (Select)    (Execute)   │
│                                                         │
│  ProductsList → .Where(...)  → .Select(...) → .ToList()│
└─────────────────────────────────────────────────────────┘
```

> 💡 **مهم جداً:** LINQ بيستخدم **Deferred Execution** — يعني الكود مش بيتنفذ لحد ما تعمل `.ToList()` أو `foreach` فعلي. ده بيخليه efficient جداً!

---

## ⚖️ Query Syntax vs Fluent Syntax

في LINQ عندك طريقتين تكتب بيهم:

### 1️⃣ Fluent Syntax (Method Syntax)

```csharp
var result = ProductsList
    .Where(p => p.UnitPrice > 20)
    .Select(p => p.ProductName)
    .OrderBy(p => p);
```

### 2️⃣ Query Syntax (SQL-like)

```csharp
var result = from product in ProductsList
             where product.UnitPrice > 20
             orderby product.ProductName
             select product.ProductName;
```

### مقارنة بين الاتنين

| الخاصية | Fluent Syntax | Query Syntax |
|---------|--------------|--------------|
| شكله | `.Where().Select()` | `from...where...select` |
| بيشبه | Method Chaining | SQL |
| مرونة | أعلى | أقل |
| وضوح للمبتدئين | أقل | أكتر |
| كل Operators متاحة؟ | ✅ نعم | ❌ مش كلها |
| بيتحول لـ Fluent تحت الغطاء؟ | — | ✅ نعم دايماً |

> 💡 **نصيحة Interview:** القى عليك هتتسأل عن الفرق — احفظ إن Query Syntax بتتحول automatically لـ Fluent Syntax وقت الـ Compilation.

---

## 🔄 Projection Operators

### 📌 Select

**Select** بيأخذ كل element في الـ sequence ويحوله لشيء تاني.

**Signature:**
```csharp
public static IEnumerable<TResult> Select<TSource, TResult>(
    this IEnumerable<TSource> source,
    Func<TSource, TResult> selector
);

// Overload with index
public static IEnumerable<TResult> Select<TSource, TResult>(
    this IEnumerable<TSource> source,
    Func<TSource, int, TResult> selector  // TSource + Index → TResult
);
```

#### مثال 1 — جيب أسماء المنتجات بس

```csharp
// Fluent Syntax
var productNames = ProductsList.Select(product => product.ProductName);

// Query Syntax
var productNames = from product in ProductsList
                   select product.ProductName;

// Output:
// Chai
// Chang
// Aniseed Syrup
// ...
```

#### مثال 2 — Anonymous Type (أكتر من خاصية)

```csharp
// Fluent Syntax
var result = ProductsList.Select(product => new 
{ 
    Name = product.ProductName, 
    Price = product.UnitPrice 
});

// Query Syntax
var result = from product in ProductsList
             select new { Name = product.ProductName, Price = product.UnitPrice };

// Output:
// { Name = Chai, Price = 18.00 }
// { Name = Chang, Price = 19.00 }
```

#### مثال 3 — عمل Discount على المنتجات

```csharp
// عايز أشوف المنتجات اللي في المخزن منها >= 50 بسعر جديد (خصم 20%)
var discountedProducts = ProductsList
    .Select(product => new 
    {
        Id          = product.ProductID,
        Name        = product.ProductName,
        Category    = product.Category,
        Stock       = product.UnitsInStock,
        OldPrice    = product.UnitPrice,
        NewPrice    = product.UnitPrice * 0.8M  // خصم 20%
    })
    .Where(product => product.Stock >= 50);

foreach (var item in discountedProducts)
    Console.WriteLine($"{item.Name} | Old: {item.OldPrice} | New: {item.NewPrice}");

// Output:
// Chai | Old: 18.00 | New: 14.40
// Grandma's Boysenberry Spread | Old: 25.00 | New: 20.00
// ...
```

#### مثال 4 — Query Syntax مع `into`

```csharp
// لو عايز تعمل where بعد select في Query Syntax، استخدم into
var result = from product in ProductsList
             select new
             {
                 Name     = product.ProductName,
                 Stock    = product.UnitsInStock,
                 NewPrice = product.UnitPrice * 0.8M,
                 OldPrice = product.UnitPrice
             } into NewProduct              // ← احفظ النتيجة في متغير جديد
             where NewProduct.Stock >= 50   // ← دلوقتي تقدر تعمل where عليه
             select NewProduct;
```

#### مثال 5 — Indexed Select

```csharp
// Select بيجبلك Index كمان!
var productsWithIndex = ProductsList
    .Select((product, index) => new 
    { 
        Index       = index, 
        ProductName = product.ProductName 
    })
    .Where(p => p.Index <= 10); // أول 11 منتج بس

foreach (var item in productsWithIndex)
    Console.WriteLine($"[{item.Index}] {item.ProductName}");

// Output:
// [0] Chai
// [1] Chang
// [2] Aniseed Syrup
// ...
// [10] Queso Cabrales
```

**تصوّر الأمر هكذا:**

```
ProductsList                    بعد Select
┌─────────────────────┐        ┌──────────────────┐
│ Product { ... }     │   →    │ "Chai"           │
│ Product { ... }     │   →    │ "Chang"          │
│ Product { ... }     │   →    │ "Aniseed Syrup"  │
└─────────────────────┘        └──────────────────┘
  List<Product>                  IEnumerable<string>
```

---

### 📌 SelectMany

**SelectMany** بيعمل **Flattening** — يعني لو عندك List of Lists، بيحولها لـ List واحدة مسطحة.

**Signature:**
```csharp
public static IEnumerable<TResult> SelectMany<TSource, TResult>(
    this IEnumerable<TSource> source,
    Func<TSource, IEnumerable<TResult>> selector
);

// Overload مع Result Selector
public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
    this IEnumerable<TSource> source,
    Func<TSource, IEnumerable<TCollection>> collectionSelector,
    Func<TSource, TCollection, TResult> resultSelector
);
```

#### الفرق بين Select و SelectMany

```
قبل (مع Select):
┌──────────────────────────────────────────────┐
│ Customer A  →  [Order1, Order2, Order3]      │  List<Order[]>
│ Customer B  →  [Order4, Order5]              │  (nested)
│ Customer C  →  [Order6]                      │
└──────────────────────────────────────────────┘

بعد (مع SelectMany):
┌──────────────────────────────────────────────┐
│ Order1                                       │  List<Order>
│ Order2                                       │  (flat!)
│ Order3                                       │
│ Order4                                       │
│ Order5                                       │
│ Order6                                       │
└──────────────────────────────────────────────┘
```

#### مثال 1 — جيب كل الـ Orders من كل Customers

```csharp
// Fluent Syntax — Simple
var allOrders = CustomersList.SelectMany(customer => customer.Orders);

foreach (var order in allOrders)
    Console.WriteLine(order);

// Output:
// Order Id: 10643, Date: 25/08/1997, Total: 814.50
// Order Id: 10692, Date: 03/10/1997, Total: 878.00
// ...
```

#### مثال 2 — SelectMany مع Result Selector (معاك Customer + Order)

```csharp
// Fluent Syntax — مع Result Selector
var result = CustomersList.SelectMany(
    customer => customer.Orders,                  // جيب الـ Orders من كل Customer
    (customer, order) => new                      // اعمل Object جديد بيجمع الاتنين
    {
        CustomerID = customer.CustomerID,
        Order      = order
    });

foreach (var item in result)
    Console.WriteLine($"Customer: {item.CustomerID} | {item.Order}");
```

#### مثال 3 — Query Syntax لـ SelectMany

```csharp
// في Query Syntax، SelectMany بيتكتب بـ from...from
var result = from customer in CustomersList
             from order in customer.Orders      // ← ده هو SelectMany
             select new 
             { 
                 Customer = customer.CustomerID, 
                 Order    = order 
             };
```

---

## 🔍 Filtering Operators

### 📌 Where

**Where** بيفلتر العناصر اللي بتحقق شرط معين.

**Signature:**
```csharp
public static IEnumerable<TSource> Where<TSource>(
    this IEnumerable<TSource> source,
    Func<TSource, bool> predicate
);

// Overload مع Index
public static IEnumerable<TSource> Where<TSource>(
    this IEnumerable<TSource> source,
    Func<TSource, int, bool> predicate
);
```

#### مثال — المنتجات المنتهية من المخزن

```csharp
// Fluent
var outOfStock = ProductsList.Where(p => p.UnitsInStock == 0);

// Query Syntax
var outOfStock = from p in ProductsList
                 where p.UnitsInStock == 0
                 select p;

foreach (var p in outOfStock)
    Console.WriteLine($"{p.ProductName} — OUT OF STOCK");

// Output:
// Chef Anton's Gumbo Mix — OUT OF STOCK
// Alice Mutton — OUT OF STOCK
// Thüringer Rostbratwurst — OUT OF STOCK
// ...
```

#### مثال — Where مع Index

```csharp
// جيب المنتجات من index 5 لـ 10
var selected = ProductsList.Where((product, index) => index >= 5 && index <= 10);
```

---

### 📌 OfType

**OfType** بيفلتر العناصر بالـ Type — مفيد لو عندك Mixed Collection.

**Signature:**
```csharp
public static IEnumerable<TResult> OfType<TResult>(
    this IEnumerable source
);
```

#### مثال

```csharp
var mixedList = new List<object> { 1, "Ibrahim", 3.14, "Shafiq", 42, true };

// جيب الـ strings بس
var strings = mixedList.OfType<string>();
// Output: "Ibrahim", "Shafiq"

// جيب الـ integers بس
var ints = mixedList.OfType<int>();
// Output: 1, 42
```

---

## 🔢 Ordering Operators

### 📌 OrderBy

ترتيب تصاعدي (من الأصغر للأكبر).

**Signature:**
```csharp
public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
    this IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector
);
```

#### مثال

```csharp
// Fluent
var ordered = ProductsList
    .Select(p => new { p.ProductName, p.UnitsInStock })
    .OrderBy(p => p.UnitsInStock);

// Query Syntax
var ordered = from p in ProductsList
              orderby p.UnitsInStock
              select new { p.ProductName, p.UnitsInStock };

foreach (var item in ordered)
    Console.WriteLine($"{item.ProductName}: {item.UnitsInStock}");

// Output (من الأقل stock للأكتر):
// Chef Anton's Gumbo Mix: 0
// Alice Mutton: 0
// Gorgonzola Telino: 0
// ...
// Rhönbräu Klosterbier: 125
```

---

### 📌 OrderByDescending

ترتيب تنازلي (من الأكبر للأصغر).

```csharp
// Fluent
var orderedDesc = ProductsList.OrderByDescending(p => p.UnitPrice);

// Query Syntax
var orderedDesc = from p in ProductsList
                  orderby p.UnitPrice descending
                  select p;
```

---

### 📌 ThenBy و ThenByDescending

ترتيب ثانوي — بيشتغل بعد `OrderBy`.

**Signature:**
```csharp
public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
    this IOrderedEnumerable<TSource> source,
    Func<TSource, TKey> keySelector
);
```

#### مثال — رتّب بالـ Category الأول، وجوا كل Category رتّب بالـ Price

```csharp
var result = ProductsList
    .OrderBy(p => p.Category)             // الترتيب الرئيسي
    .ThenBy(p => p.UnitPrice)             // الترتيب الثانوي

// Query Syntax
var result = from p in ProductsList
             orderby p.Category, p.UnitPrice  // بالفاصلة!
             select p;

// لو عايز الثانوي تنازلي:
var result = ProductsList
    .OrderBy(p => p.Category)
    .ThenByDescending(p => p.UnitPrice);

// Query Syntax
var result = from p in ProductsList
             select new { p.ProductName, p.Category, p.UnitPrice } into TransformedP
             orderby TransformedP.Category, TransformedP.UnitPrice descending
             select TransformedP;
```

**تصوّر الترتيب:**

```
Before OrderBy:                    After OrderBy(Category) + ThenBy(Price):
┌──────────────────────┐          ┌──────────────────────────────┐
│ Chai      (Bev) $18  │    →     │ Guaraná   (Bev) $4.5  ← أرخص │
│ Tofu      (Pro) $23  │    →     │ Rhönbräu  (Bev) $7.75        │
│ Guaraná   (Bev) $4.5 │    →     │ Chai      (Bev) $18          │
│ Ikura     (Sea) $31  │    →     │ Tofu      (Pro) $23   ← أول pro│
│ Rhönbräu  (Bev) $7.75│    →     │ Ikura     (Sea) $31          │
└──────────────────────┘          └──────────────────────────────┘
```

---

### 📌 Reverse

عكس الترتيب الحالي للـ Sequence.

```csharp
var reversed = ProductsList
    .Select(p => new { p.ProductName, p.UnitsInStock })
    .Reverse();

// ملاحظة: Reverse() بتعمل Immediate Execution على بعض Types
// وبترجع IEnumerable اللي مقلوب
```

---

## 🎯 Element Operators

> ⚡ **دول بيعملوا Immediate Execution** — يعني بينفذوا على طول ويرجعوا نتيجة واحدة.

### جدول المقارنة

| Method | Sequence فاضي؟ | أكتر من element؟ | Safe؟ |
|--------|---------------|-----------------|-------|
| `First()` | ❌ Exception | يرجع الأول | ❌ |
| `FirstOrDefault()` | ✅ null/default | يرجع الأول | ✅ |
| `Last()` | ❌ Exception | يرجع الآخر | ❌ |
| `LastOrDefault()` | ✅ null/default | يرجع الآخر | ✅ |
| `Single()` | ❌ Exception | ❌ Exception | ❌ |
| `SingleOrDefault()` | ✅ null/default | ❌ Exception | ⚠️ |
| `ElementAt(i)` | ❌ Exception | يرجع index i | ❌ |
| `ElementAtOrDefault(i)` | ✅ null/default | يرجع index i | ✅ |

---

### 📌 First و FirstOrDefault

```csharp
// Signatures
public static TSource First<TSource>(this IEnumerable<TSource> source);
public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source);
public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
```

```csharp
// ✅ جيب أول منتج
var first = ProductsList.First();
Console.WriteLine(first);
// Output: ProductID:1, ProductName:Chai, ...

// ✅ جيب أول منتج سعره 10
var firstAt10 = ProductsList.First(p => p.UnitPrice == 10.0000M);
Console.WriteLine(firstAt10);
// Output: ProductID:3, ProductName:Aniseed Syrup, ...

// ✅ آمن — لو مش موجود يرجع null
var safeFirst = ProductsList.FirstOrDefault(p => p.UnitPrice == 99999);
Console.WriteLine(safeFirst);  // Output: (nothing — null)

// ❌ خطير — لو List فاضية هيحصل Exception!
List<Product> empty = new List<Product>();
var danger = empty.First();  // ❌ InvalidOperationException!
```

---

### 📌 Last و LastOrDefault

```csharp
// Signatures
public static TSource Last<TSource>(this IEnumerable<TSource> source);
public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

public static TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source);
public static TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
```

```csharp
// جيب آخر منتج
var last = ProductsList.Last();

// جيب آخر منتج سعره 10
var lastAt10 = ProductsList.Last(p => p.UnitPrice == 10.0000M);
Console.WriteLine(lastAt10);
// Output: ProductID:74, ProductName:Longlife Tofu, UnitPrice:10.00

// آمن
var safeLast = ProductsList.LastOrDefault(p => p.UnitPrice == 99999);
// Output: null
```

---

### 📌 Single و SingleOrDefault

**Single** = لازم يكون في element واحدة بالظبط — لا أقل ولا أكتر.

```csharp
// Signatures
public static TSource Single<TSource>(this IEnumerable<TSource> source);
public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source);
public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
```

```csharp
// ✅ صح — جيب المنتج اللي index بتاعه 0
var single = ProductsList
    .Select((p, i) => new { p.ProductName, Index = i })
    .Single(p => p.Index == 0);
Console.WriteLine(single);
// Output: { ProductName = Chai, Index = 0 }

// ✅ Overload مباشر
var singleDirect = ProductsList
    .Select((p, i) => new { p.ProductName, Index = i })
    .Single(p => p.Index == 4);
// Output: { ProductName = Chef Anton's Gumbo Mix, Index = 4 }

// ❌ لو في أكتر من element بتحقق الشرط
var error = ProductsList.Single(p => p.Category == "Beverages");
// ❌ InvalidOperationException: Sequence contains more than one element

// ❌ لو Sequence فاضية
List<Product> empty = new();
var error2 = empty.Single();
// ❌ InvalidOperationException: Sequence contains no elements

// ✅ SingleOrDefault آمن لو فاضية — بيرجع null
var safe = ProductsList
    .Where(p => p.ProductID == 99999)
    .SingleOrDefault();
Console.WriteLine(safe);  // Output: null

// ❌ لكن لو أكتر من واحد بيتحقق → Exception حتى مع OrDefault!
```

---

### 📌 ElementAt و ElementAtOrDefault

```csharp
// Signatures
public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index);
public static TSource? ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index);
```

```csharp
// جيب المنتج في الـ index 0
var el = ProductsList.ElementAt(0);
Console.WriteLine(el);
// Output: ProductID:1, ProductName:Chai, ...

// ❌ Index خارج النطاق
var bad = ProductsList.ElementAt(9999);
// ❌ ArgumentOutOfRangeException!

// ✅ آمن
var safe = ProductsList.ElementAtOrDefault(9999);
Console.WriteLine(safe);  // Output: null
```

---

## ➕ Aggregate Operators

> ⚡ **دول بيعملوا Immediate Execution** — بيرجعوا قيمة واحدة.

### 📌 Count

```csharp
// Signatures
public static int Count<TSource>(this IEnumerable<TSource> source);
public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
```

```csharp
// كم منتج عندنا؟
int total = ProductsList.Count();
Console.WriteLine(total);  // Output: 77

// أو ممكن تستخدم Property بدل Method
int totalProp = ProductsList.Count;  // لـ List فقط

// كم منتج ناقص من المخزن؟
int outOfStock = ProductsList.Count(p => p.UnitsInStock == 0);
Console.WriteLine(outOfStock);  // Output: 5
```

---

### 📌 Min و Max

```csharp
// Signatures
public static TResult? Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector);
public static TResult? Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector);
```

```csharp
// أعلى سعر
decimal maxPrice = ProductsList.Max(p => p.UnitPrice);
Console.WriteLine(maxPrice);  // Output: 263.50

// أقل عدد في المخزن
int minStock = ProductsList.Min(p => p.UnitsInStock);
Console.WriteLine(minStock);  // Output: 0

// ⚠️ بدون Selector → بيحتاج IComparable
// ProductsList.Max();  ← ده شغال لأن Product implements IComparable<Product>
```

> 💡 **ملاحظة مهمة:** `Max()` و `Min()` بدون Selector بيحتاجوا إن الـ Class يعمل implement للـ `IComparable` interface.

```csharp
// Product بتعمل implement IComparable وبتقارن بالـ UnitPrice
class Product : IComparable<Product>
{
    public int CompareTo(Product? other)
        => this.UnitPrice.CompareTo(other?.UnitPrice);
}

// دلوقتي ينفع تعمل:
var maxProduct = ProductsList.Max();  // ✅ بيرجع Product بأعلى UnitPrice
```

---

### 📌 MinBy و MaxBy

الفرق الجوهري بين `Min/Max` و `MinBy/MaxBy`:

| | Min/Max | MinBy/MaxBy |
|-|---------|-------------|
| بيرجع | **قيمة** (decimal, int, ...) | **Object كامل** (Product, ...) |
| مثال | `263.5` | `Product { Côte de Blaye, 263.5 }` |

```csharp
// Signatures
public static TSource? MaxBy<TSource, TKey>(
    this IEnumerable<TSource> source, 
    Func<TSource, TKey> keySelector
);

public static TSource? MinBy<TSource, TKey>(
    this IEnumerable<TSource> source, 
    Func<TSource, TKey> keySelector
);
```

```csharp
// جيب المنتج بأعلى stock كـ Object كامل
var maxProduct = ProductsList.MaxBy(p => p.UnitsInStock);
Console.WriteLine(maxProduct);
// Output: ProductID:40, ProductName:Boston Crab Meat, UnitsInStock:123, ...

// جيب المنتج بأقل stock كـ Object كامل
var minProduct = ProductsList.MinBy(p => p.UnitsInStock);
Console.WriteLine(minProduct);
// Output: ProductID:5, ProductName:Chef Anton's Gumbo Mix, UnitsInStock:0
```

---

### 📌 Sum و Average

```csharp
// Signatures
public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector);
public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector);
```

```csharp
// مجموع أسعار كل المنتجات
decimal totalPrice = ProductsList.Sum(p => p.UnitPrice);
Console.WriteLine(totalPrice);
// Output: 2222.71

// أو بطريقة Select أولاً
decimal totalPrice2 = ProductsList.Select(p => p.UnitPrice).Sum();

// متوسط عدد المنتجات في المخزن
double avgStock = ProductsList.Average(p => p.UnitsInStock);
Console.WriteLine(avgStock);
// Output: 40.5...
```

---

### 📌 Aggregate

**Aggregate** هو أقوى واحد — بيديك تحكم كامل في عملية التجميع.

```csharp
// Signature
public static TAccumulate Aggregate<TSource, TAccumulate>(
    this IEnumerable<TSource> source,
    TAccumulate seed,
    Func<TAccumulate, TSource, TAccumulate> func
);

// Simple overload (بدون seed)
public static TSource Aggregate<TSource>(
    this IEnumerable<TSource> source,
    Func<TSource, TSource, TSource> func
);
```

#### مثال — دمج أسماء

```csharp
string[] names = { "Ibrahim", "Shafiq", "Abd-Elshafy", "Muhammad" };

var result = names.Aggregate((str1, str2) => $"{str1} {str2}");
Console.WriteLine(result);
// Output: Ibrahim Shafiq Abd-Elshafy Muhammad

// إزاي بيشتغل؟
// Round 1: str1="Ibrahim"       + str2="Shafiq"       = "Ibrahim Shafiq"
// Round 2: str1="Ibrahim Shafiq"+ str2="Abd-Elshafy"  = "Ibrahim Shafiq Abd-Elshafy"
// Round 3: str1="Ibrahim Shafiq Abd-Elshafy" + str2="Muhammad" = "Ibrahim Shafiq Abd-Elshafy Muhammad"
```

**تصوّر Aggregate:**

```
names = ["Ibrahim", "Shafiq", "Abd-Elshafy", "Muhammad"]

Step 1:  "Ibrahim"                 + "Shafiq"       → "Ibrahim Shafiq"
Step 2:  "Ibrahim Shafiq"          + "Abd-Elshafy"  → "Ibrahim Shafiq Abd-Elshafy"
Step 3:  "Ibrahim Shafiq Abd-Elshafy" + "Muhammad"  → "Ibrahim Shafiq Abd-Elshafy Muhammad"
                                                              ↑
                                                         Final Result
```

#### مثال — Aggregate مع Seed

```csharp
// احسب مجموع UnitPrice ابتداءً من 1000
decimal startingBudget = 1000M;
decimal totalWithBudget = ProductsList.Aggregate(
    startingBudget,                              // ← Seed (القيمة الابتدائية)
    (accumulator, product) => accumulator + product.UnitPrice
);
Console.WriteLine(totalWithBudget);
// Output: 3222.71  (1000 + 2222.71)
```

---

## 🔗 Set Operators

```
دول بيتعاملوا مع أكتر من Sequence وبيرجعوا نتيجة Set Operations
```

### 📌 Union — اتحاد بدون تكرار

```csharp
var seq01 = Enumerable.Range(0, 100);   // 0 .. 99
var seq02 = Enumerable.Range(50, 150);  // 50 .. 199

var union = seq01.Union(seq02);
// النتيجة: 0, 1, 2, ..., 99, 100, 101, ..., 199 (بدون تكرار!)
// العناصر المشتركة (50-99) بتظهر مرة واحدة بس
```

**تصوّر Venn Diagram:**
```
     seq01         seq02
  ┌─────────────────────────┐
  │  0-49  │  50-99  │ 100-199 │
  │        │(مشترك) │        │
  └─────────────────────────┘
  Union = كل شيء بدون تكرار (0 → 199)
```

---

### 📌 Concat — دمج مع تكرار (UnionAll)

```csharp
var concat = seq01.Concat(seq02);
// النتيجة: 0, 1, ..., 99, 50, 51, ..., 199 (مع تكرار العناصر المشتركة!)

// لو عايز Concat بدون تكرار → استخدم Distinct()
var concatDistinct = seq01.Concat(seq02).Distinct();
// النتيجة: 0 → 199 (بدون تكرار)
```

---

### 📌 Intersect — التقاطع (المشترك بس)

```csharp
var intersect = seq01.Intersect(seq02);
// النتيجة: 50, 51, 52, ..., 99 (العناصر المشتركة بس)
```

```
     seq01         seq02
  ┌─────────────────────────┐
  │  0-49  │  50-99  │ 100-199 │
  │        │ ← هنا  │        │
  └─────────────────────────┘
  Intersect = 50-99 بس
```

---

### 📌 Except — الفرق (في الأول مش في الثاني)

```csharp
var except = seq01.Except(seq02);
// النتيجة: 0, 1, 2, ..., 49 (موجودة في seq01 ومش موجودة في seq02)
```

```
     seq01         seq02
  ┌─────────────────────────┐
  │  0-49  │  50-99  │ 100-199 │
  │ ← هنا │        │        │
  └─────────────────────────┘
  Except = 0-49 بس
```

---

### 📌 Distinct — إزالة التكرار

```csharp
var numbers = new List<int> { 1, 2, 2, 3, 3, 3, 4 };
var distinct = numbers.Distinct();
// النتيجة: 1, 2, 3, 4
```

### ملخص Set Operators

| Operator | الوصف | مثال |
|---------|-------|------|
| `Union` | كل العناصر بدون تكرار | {1,2,3} ∪ {2,3,4} = {1,2,3,4} |
| `Concat` | كل العناصر مع تكرار | {1,2,3} + {2,3,4} = {1,2,3,2,3,4} |
| `Intersect` | المشترك بس | {1,2,3} ∩ {2,3,4} = {2,3} |
| `Except` | في الأول مش في الثاني | {1,2,3} - {2,3,4} = {1} |
| `Distinct` | إزالة التكرار | {1,2,2,3} = {1,2,3} |

---

## ⚙️ Generator Operators

**دول بيولّدوا Sequences من الصفر** — مش بتبدأ بـ Collection موجودة.

```csharp
// دول Static Methods على الـ Enumerable Class
```

### 📌 Range

```csharp
// Signature
public static IEnumerable<int> Range(int start, int count);
```

```csharp
// أرقام من 1 لـ 100
var numbers = Enumerable.Range(1, 100);
// Output: 1, 2, 3, ..., 100

// أرقام من 50 لـ 59 (عشرة أرقام)
var tens = Enumerable.Range(50, 10);
// Output: 50, 51, 52, ..., 59

// Range مع Select — جدول ضرب 5
var multiplicationTable = Enumerable.Range(1, 10)
    .Select(i => new { Number = i, Result = i * 5 });

foreach (var item in multiplicationTable)
    Console.WriteLine($"5 × {item.Number} = {item.Result}");
```

---

### 📌 Repeat

```csharp
// Signature
public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count);
```

```csharp
// كرر "$" عشرين مرة
var dollarSigns = Enumerable.Repeat("$", 20);
foreach (var s in dollarSigns)
    Console.Write(s);
// Output: $$$$$$$$$$$$$$$$$$$$

// كرر Product معينة 5 مرات
var repeatedProduct = Enumerable.Repeat(new Product { ProductName = "Test" }, 5);
```

---

### 📌 Empty

```csharp
// Signature
public static IEnumerable<TResult> Empty<TResult>();
```

```csharp
// Sequence فاضية من نوع Product
var emptyProducts = Enumerable.Empty<Product>();
// بتتساوى مع: new List<Product>() { }

var emptyList = Enumerable.Empty<Product>().ToList();
// النتيجة: List<Product> فاضية

// بيستخدم فين؟ — كـ Default Value أو في Unit Testing
IEnumerable<Product> GetProducts()
{
    if (someCondition)
        return Enumerable.Empty<Product>();
    return ProductsList;
}
```

---

## 🔄 Casting Operators

**دول بيحولوا الـ IEnumerable لـ Types مختلفة وبيعملوا Immediate Execution.**

### 📌 ToList

```csharp
// Signature
public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source);
```

```csharp
// المنتجات المنتهية كـ List
List<Product> outOfStock = ProductsList
    .Where(p => p.UnitsInStock == 0)
    .ToList();

Console.WriteLine(outOfStock.Count);  // Output: 5
```

---

### 📌 ToArray

```csharp
// Signature
public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source);
```

```csharp
// المنتجات كـ Array
Product[] productsArray = ProductsList
    .Where(p => p.UnitsInStock == 0)
    .ToArray();

Console.WriteLine(productsArray.Length);  // Output: 5
```

---

### 📌 ToDictionary

```csharp
// Signatures
public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(
    this IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector
);

public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
    this IEnumerable<TSource> source,
    Func<TSource, TKey> keySelector,
    Func<TSource, TElement> elementSelector
);
```

```csharp
// Dictionary<string, Product> — ProductName هو الـ Key
Dictionary<string, Product> byName = ProductsList
    .Where(p => p.UnitsInStock == 0)
    .ToDictionary(p => p.ProductName);

foreach (var kv in byName)
    Console.WriteLine($"Key: {kv.Key} → {kv.Value}");

// Dictionary<long, Product> — ProductID هو الـ Key
Dictionary<long, Product> byId = ProductsList
    .ToDictionary(p => p.ProductID);

// Dictionary<long, string> — Key: ID, Value: Name
Dictionary<long, string> idToName = ProductsList
    .Where(p => p.UnitsInStock == 0)
    .ToDictionary(
        p => p.ProductID,     // Key
        p => p.ProductName    // Value
    );

Console.WriteLine(idToName[5]);  // Output: Chef Anton's Gumbo Mix
```

---

### 📌 ToHashSet

```csharp
// Signature
public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source);
```

```csharp
// Categories كـ HashSet (بدون تكرار تلقائياً)
HashSet<string> categories = ProductsList
    .Select(p => p.Category)
    .ToHashSet();

foreach (var cat in categories)
    Console.WriteLine(cat);

// Output (بدون تكرار):
// Beverages
// Condiments
// Produce
// Meat/Poultry
// ...
```

**مقارنة Casting:**

```
IEnumerable<Product>        بعد Casting
┌──────────────┐     ToList()    → List<Product>
│ [lazy query] │ →   ToArray()   → Product[]
│              │     ToHashSet() → HashSet<Product>
└──────────────┘     ToDictionary() → Dictionary<K,V>
```

---

## ✅ Quantifiers

**دول بيرجعوا `bool`** — بيسألوا سؤال ويجاوبوا True أو False.

### 📌 Any

```csharp
// Signatures
public static bool Any<TSource>(this IEnumerable<TSource> source);
public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
```

```csharp
// هل في أي منتجات خالص؟
bool hasProducts = ProductsList.Any();
Console.WriteLine(hasProducts);  // Output: True

// هل في منتج اسمه يحتوي على "a"؟
bool hasProductWithA = ProductsList.Any(p => p.ProductName.Contains("a"));
Console.WriteLine(hasProductWithA);  // Output: True

// هل في منتج سعره أكتر من 200؟
bool hasExpensive = ProductsList.Any(p => p.UnitPrice > 200);
Console.WriteLine(hasExpensive);  // Output: True (Côte de Blaye = 263.5)
```

---

### 📌 All

```csharp
// Signature
public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
```

```csharp
// هل كل المنتجات سعرها > 0؟
bool allPositivePrice = ProductsList.All(p => p.UnitPrice > 0);
Console.WriteLine(allPositivePrice);  // Output: True

// هل كل المنتجات في المخزن؟
bool allInStock = ProductsList.All(p => p.UnitsInStock > 0);
Console.WriteLine(allInStock);  // Output: False (في 5 منتجات مخزنهم = 0)
```

> 💡 **ملاحظة:** لو الـ Sequence فاضية، `All()` بيرجع `true` دايماً! (Vacuous Truth)

---

### 📌 SequenceEqual

```csharp
// Signatures
public static bool SequenceEqual<TSource>(
    this IEnumerable<TSource> first, 
    IEnumerable<TSource> second
);

// مع Custom Comparer
public static bool SequenceEqual<TSource>(
    this IEnumerable<TSource> first,
    IEnumerable<TSource> second,
    IEqualityComparer<TSource>? comparer
);
```

```csharp
var seqOne = Enumerable.Range(0, 100);   // 0 .. 99
var seqTwo = Enumerable.Range(50, 150);  // 50 .. 199

// هل الاتنين متطابقين تماماً؟
bool areEqual = seqOne.SequenceEqual(seqTwo);
Console.WriteLine(areEqual);  // Output: False (مختلفين في العناصر والطول)

// مثال متطابق
var a = new List<int> { 1, 2, 3 };
var b = new List<int> { 1, 2, 3 };
bool sameSeq = a.SequenceEqual(b);
Console.WriteLine(sameSeq);  // Output: True

// ترتيب مختلف → False
var c = new List<int> { 3, 2, 1 };
bool diffOrder = a.SequenceEqual(c);
Console.WriteLine(diffOrder);  // Output: False
```

---

## 🤐 Zip Operator

**Zip** بيجمع عناصر Sequence اتنين مع بعض element بـ element.

**Signatures:**
```csharp
public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
    this IEnumerable<TFirst> first,
    IEnumerable<TSecond> second,
    Func<TFirst, TSecond, TResult> resultSelector
);

// C# 8+ — Tuple بدون Selector
public static IEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(
    this IEnumerable<TFirst> first,
    IEnumerable<TSecond> second
);

// C# 9+ — ثلاث Sequences
public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(
    this IEnumerable<TFirst> first,
    IEnumerable<TSecond> second,
    IEnumerable<TThird> third
);
```

**تصوّر Zip:**

```
seq1:    ["Ibrahim", "Shafiq",  "Muhammad"]
seq2:    [     1,        2,          3    ]
          ↓             ↓            ↓
Result:  (Ibrahim,1)  (Shafiq,2)  (Muhammad,3)
```

#### مثال 1 — Zip مع Result Selector

```csharp
var names   = new List<string> { "Ibrahim", "Shafiq", "Muhammad" };
var scores  = new List<int>    { 95, 87, 92 };

var result = names.Zip(scores, (name, score) => new 
{
    Name  = name,
    Score = score
});

foreach (var item in result)
    Console.WriteLine($"{item.Name}: {item.Score}");

// Output:
// Ibrahim: 95
// Shafiq: 87
// Muhammad: 92
```

#### مثال 2 — Zip كـ Tuples (C# 8+)

```csharp
var zipped = names.Zip(scores);  // ← بيرجع (string, int) Tuples

foreach (var (name, score) in zipped)
    Console.WriteLine($"{name} → {score}");

// Output:
// Ibrahim → 95
// Shafiq → 87
// Muhammad → 92
```

#### مثال 3 — Zip مع Sequences بأحجام مختلفة

```csharp
// ⚠️ لو Sequences مختلفة الحجم → Zip بيقف عند الأقصر!
var long  = new[] { 1, 2, 3, 4, 5 };
var short = new[] { "a", "b", "c" };

var result = long.Zip(short, (n, s) => $"{n}-{s}");
// Output: "1-a", "2-b", "3-c"  ← بس! 4 و 5 اتخطوا
```

#### مثال 4 — Products و Discounts

```csharp
var products  = ProductsList.Take(5);
var discounts = new[] { 0.1M, 0.2M, 0.15M, 0.05M, 0.25M };

var discountedProducts = products.Zip(discounts, (product, discount) => new
{
    Name       = product.ProductName,
    OldPrice   = product.UnitPrice,
    Discount   = $"{discount * 100}%",
    NewPrice   = product.UnitPrice * (1 - discount)
});

foreach (var item in discountedProducts)
    Console.WriteLine($"{item.Name}: {item.OldPrice} → {item.NewPrice} ({item.Discount} off)");

// Output:
// Chai: 18.00 → 16.20 (10% off)
// Chang: 19.00 → 15.20 (20% off)
// Aniseed Syrup: 10.00 → 8.50 (15% off)
// ...
```

---

## 🔀 Hybrid Syntax

**Hybrid Syntax** = Query Syntax + Fluent Syntax في نفس الوقت!

### ليه بنستخدمه؟

بعض Methods زي `First()`, `Last()`, `Single()`, `Count()` **مش موجودة في Query Syntax** — فبنضيفهم في الآخر كـ Fluent.

### الشكل العام

```csharp
var result = (from element in Collection
              where condition
              select something)
             .FluentMethod();  // ← هنا بتضيف الـ Fluent
//           ↑
//    القوسين مهمين!
```

#### أمثلة عملية

```csharp
// جيب أول منتج ID بتاعه 10
var firstResult = (from p in ProductsList
                   where p.ProductID == 10
                   select new { p.ProductID, p.ProductName })
                  .FirstOrDefault();

Console.WriteLine(firstResult);
// Output: { ProductID = 10, ProductName = Ikura }

// جيب آخر منتج ID بتاعه 10
var lastResult = (from p in ProductsList
                  where p.ProductID == 10
                  select new { p.ProductID, p.ProductName })
                 .LastOrDefault();

// استخدام Single
var singleResult = (from p in ProductsList
                    where p.ProductID == 10
                    select new { p.ProductID, p.ProductName })
                   .Single();

// Single مع Overload
var singleOverload = (from p in ProductsList
                      where p.ProductID == 10
                      select new { p.ProductID, p.ProductName })
                     .Single(p => p.ProductID == 10);

// SingleOrDefault مع Condition
var safeResult = (from p in ProductsList
                  where p.ProductID == 10
                  select new { p.ProductID, p.ProductName })
                 .SingleOrDefault(p => p.ProductName.Contains("a"));
// Ikura يحتوي "a" فهيرجع النتيجة

Console.WriteLine(safeResult);
// Output: { ProductID = 10, ProductName = Ikura }
```

---

## 👻 Anonymous Types

**Anonymous Types** هي Objects من غير ما تعمل ليها Class.

```csharp
var product = new { Name = "Chai", Price = 18.00M };
// ↑ مش محتاج تعرّف Class لده!

Console.WriteLine(product.Name);   // Output: Chai
Console.WriteLine(product.Price);  // Output: 18.00
```

### ليه هي Readonly؟

```csharp
var person = new { Name = "Ibrahim", Age = 25 };

person.Name = "Shafiq";  // ❌ Compile Error!
// Anonymous Types كل خصائصها readonly
// مش ممكن تغيرها بعد ما اتعملت
```

### ليه هي Local Scope بس؟

```csharp
// ❌ مش ممكن ترجع Anonymous Type من Method!
public ??? GetProduct()  // مش عارف تكتب النوع
{
    return new { Name = "Chai", Price = 18M };  // ❌ مشكلة!
}

// ✅ الحل — استخدم object أو dynamic (بس مش recommended)
public object GetProduct()
{
    return new { Name = "Chai", Price = 18M };
}

// ✅ الحل الأفضل — عرّف Class أو استخدم Tuple
public (string Name, decimal Price) GetProduct()
{
    return ("Chai", 18M);
}
```

### متى نستخدم Anonymous Types؟

```csharp
// ✅ جوا نفس الـ Method — تمام!
var result = ProductsList.Select(p => new 
{ 
    p.ProductName, 
    p.UnitPrice 
});

foreach (var item in result)
    Console.WriteLine($"{item.ProductName}: {item.UnitPrice}");
```

---

## ⚠️ Exceptions in LINQ

### 1️⃣ InvalidOperationException

**متى بيحصل؟** لما بتاخد element من Sequence بس فيه مشكلة في عدد العناصر.

```csharp
// Scenario 1: Sequence فاضية مع First() أو Last() أو Single()
List<Product> empty = new List<Product>();

try
{
    var result = empty.First();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Sequence contains no elements
}

// Scenario 2: Single() مع أكتر من element
try
{
    var result = ProductsList.Single(p => p.Category == "Beverages");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Sequence contains more than one element
}

// Scenario 3: SingleOrDefault() مع أكتر من element
try
{
    var result = ProductsList.SingleOrDefault(p => p.Category == "Beverages");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Sequence contains more than one matching element
}
```

**الحل:**

```csharp
// بدل First() → FirstOrDefault()
// بدل Last() → LastOrDefault()
// بدل Single() → SingleOrDefault() (لو مش متأكد من عدد العناصر)
```

---

### 2️⃣ ArgumentNullException

**متى بيحصل؟** لما تعمل LINQ على `null` source.

```csharp
List<Product> nullList = null;

try
{
    var result = nullList.Where(p => p.UnitPrice > 10);
    var count = result.Count();  // هنا بيحصل
}
catch (ArgumentNullException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Value cannot be null. (Parameter 'source')
}

// ❌ كمان ممكن يحصل لو الـ predicate نفسه null
try
{
    Func<Product, bool> pred = null;
    var result = ProductsList.Where(pred);  // ← ArgumentNullException هنا
}
catch (ArgumentNullException ex)
{
    Console.WriteLine(ex.Message);
}
```

**الحل:**

```csharp
// دايماً تأكد إن الـ List مش null قبل LINQ
if (myList != null)
{
    var result = myList.Where(p => p.UnitPrice > 10);
}

// أو استخدم Null-conditional
var result = myList?.Where(p => p.UnitPrice > 10) ?? Enumerable.Empty<Product>();
```

---

### 3️⃣ ArgumentOutOfRangeException

**متى بيحصل؟** لما تطلب Index مش موجود.

```csharp
try
{
    var result = ProductsList.ElementAt(9999);  // Index مش موجود!
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine(ex.Message);
    // Output: Index was out of range. Must be non-negative and less than the size of the collection.
    //         (Parameter 'index')
}
```

**الحل:**

```csharp
// استخدم ElementAtOrDefault()
var safeResult = ProductsList.ElementAtOrDefault(9999);
Console.WriteLine(safeResult);  // Output: null (بدل Exception)
```

---

### ملخص Exceptions

| Exception | السبب | الحل الآمن |
|-----------|-------|------------|
| `InvalidOperationException` | `First/Last/Single` على Sequence فاضية أو `Single` على أكتر من element | استخدم `OrDefault` variants |
| `ArgumentNullException` | Source أو Predicate = null | Check for null قبل LINQ |
| `ArgumentOutOfRangeException` | `ElementAt` بـ Index مش موجود | استخدم `ElementAtOrDefault` |

---

## 📚 Quick Reference — كل Operators في مكان واحد

### Projection
| Method | الوصف |
|--------|-------|
| `Select(x => ...)` | حوّل كل element لشيء تاني |
| `Select((x, i) => ...)` | حوّل مع Index |
| `SelectMany(x => x.List)` | افرد Nested Lists |

### Filtering
| Method | الوصف |
|--------|-------|
| `Where(x => condition)` | فلتر بشرط |
| `OfType<T>()` | فلتر بالنوع |

### Ordering
| Method | الوصف |
|--------|-------|
| `OrderBy(x => key)` | ترتيب تصاعدي |
| `OrderByDescending(x => key)` | ترتيب تنازلي |
| `ThenBy(x => key)` | ترتيب ثانوي تصاعدي |
| `ThenByDescending(x => key)` | ترتيب ثانوي تنازلي |
| `Reverse()` | عكس الترتيب |

### Element
| Method | Throws? | الوصف |
|--------|---------|-------|
| `First()` | ✅ إذا فاضي | أول element |
| `FirstOrDefault()` | ❌ | أول element أو default |
| `Last()` | ✅ إذا فاضي | آخر element |
| `LastOrDefault()` | ❌ | آخر element أو default |
| `Single()` | ✅ إذا 0 أو >1 | element واحدة بالظبط |
| `SingleOrDefault()` | ✅ إذا >1 | element واحدة أو default |
| `ElementAt(i)` | ✅ إذا خارج Range | element في Index i |
| `ElementAtOrDefault(i)` | ❌ | element في Index i أو default |

### Aggregation
| Method | الوصف |
|--------|-------|
| `Count()` / `Count(pred)` | عدد العناصر |
| `Sum(x => ...)` | المجموع |
| `Average(x => ...)` | المتوسط |
| `Min(x => ...)` | أصغر قيمة |
| `Max(x => ...)` | أكبر قيمة |
| `MinBy(x => key)` | Object بأصغر قيمة |
| `MaxBy(x => key)` | Object بأكبر قيمة |
| `Aggregate(func)` | تجميع مخصص |

### Set
| Method | الوصف |
|--------|-------|
| `Union(seq2)` | اتحاد بدون تكرار |
| `Concat(seq2)` | دمج مع تكرار |
| `Intersect(seq2)` | التقاطع |
| `Except(seq2)` | الفرق |
| `Distinct()` | إزالة التكرار |

### Generator
| Method | الوصف |
|--------|-------|
| `Enumerable.Range(start, count)` | Sequence أرقام |
| `Enumerable.Repeat(element, count)` | تكرار element |
| `Enumerable.Empty<T>()` | Sequence فاضية |

### Casting
| Method | الوصف |
|--------|-------|
| `ToList()` | → `List<T>` |
| `ToArray()` | → `T[]` |
| `ToDictionary(key)` | → `Dictionary<K,V>` |
| `ToHashSet()` | → `HashSet<T>` |

### Quantifiers
| Method | الوصف |
|--------|-------|
| `Any()` | في أي element? |
| `Any(pred)` | في element بيحقق الشرط? |
| `All(pred)` | كلهم بيحققوا الشرط? |
| `SequenceEqual(seq2)` | الاتنين متطابقين؟ |

### Zip
| Method | الوصف |
|--------|-------|
| `Zip(seq2, (a,b) => ...)` | دمج عنصر بعنصر |
| `Zip(seq2)` | دمج كـ Tuples |

---

## 🎯 نصائح Interview

1. **Deferred vs Immediate Execution:**
   - **Deferred:** `Where`, `Select`, `OrderBy`, `SelectMany` — مش بيتنفذوا لحد ما تعمل `foreach` أو `ToList()`
   - **Immediate:** `Count`, `Sum`, `First`, `ToList`, `ToArray` — بينفذوا على طول

2. **Query Syntax بتتحول لـ Fluent تحت الغطاء** — الـ Compiler بيعمله ده تلقائياً.

3. **`Single` vs `First`:**
   - `First` → يرجع أول واحد حتى لو في أكتر
   - `Single` → يتأكد إن في واحد بالظبط وإلا Exception

4. **Anonymous Types → Always Readonly and Local Scope**

5. **`MinBy/MaxBy` → ترجع Object | `Min/Max` → ترجع Value**

---

> 📌 **تذكر:** LINQ مش بس Syntax جميل — هو تعبير عن المقصود (declarative) مش طريقة التنفيذ (imperative). ده بيخليه أوضح وأسهل في القراءة والصيانة. 🚀