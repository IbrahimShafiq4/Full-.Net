## مقدمة

الكود اللي قدامنا عبارة عن تطبيق لـ **C# Advanced Concepts**، خصوصاً **الـ Generics** و **الـ Collections** (غير العامة والعامة). هنشرح كل حاجة بالتفصيل الممل، بأسلوب مبسط، وكل جزء هيكون فيه أمثلة عملية تقدّر تفهم منها كل فكرة وتستخدمها في مشاريعك أو في المقابلات.

---

## 1. Generics في C# (الأنواع العامة)

### ليه الـ Generics أصلاً موجودة؟

قبل الـ Generics، لو عايز تعمل دوال أو كلاسات تشتغل مع أنواع بيانات مختلفة، كنت مضطر تعمل **Overloading** لكل نوع. ده كان بيسبب تكرار في الكود وصعوبة في الصيانة.

**مثال قبل الـ Generics:**
```csharp
public static void Swap(ref int a, ref int b)
{
    int temp = a;
    a = b;
    b = temp;
}

public static void Swap(ref double a, ref double b)
{
    double temp = a;
    a = b;
    b = temp;
}
```
لو عندك 10 أنواع، هتعمل 10 دوال! ده بيخالف مبدأ **DRY** (Don't Repeat Yourself).

### الـ Generics بيحل المشكلة دي إزاي؟

الـ Generics بتخلّي تكتب **قالب (Template)** للدالة أو الكلاس، والنوع (Type) بيتحدد بعدين على حسب الاستخدام. يعني بنكتب الكود مرة واحدة وبنستخدمه مع أي نوع.

### أنواع الـ Generics في الكود:

#### أ. Generic Class
```csharp
public class Helper<T>
```
- هنا الكلاس بيستقبل نوع عام `T`، وكل الدوال جواه تقدر تستخدم `T`.
- لازم لما تستخدم الكلاس ده، تحدد النوع، زي:
```csharp
Helper<int>.Swap(ref x, ref y);
```

#### ب. Generic Method
```csharp
public static void Swap<T>(ref T x, ref T y)
```
- الدالة نفسها هي اللي عامة، مش لازم الكلاس يكون عام.
- الـ `T` بيتحدد من الباراميترات اللي بتدّيها.

#### ج. Type Parameters
- الـ `T` مجرد اسم، ممكن تسميه أي حاجة زي `TType` أو `TKey`.

### Constraints (القيود على الأنواع)

في الكود شايفين قيود كتير على الـ `T`، دي بتحدد إيه الأنواع المسموح استخدامها. تنقسم لـ 3 أنواع:

| نوع القيد | مثال | شرح |
|-----------|------|------|
| Primary Constraint | `where T : class` | النوع لازم يكون **class** (مرجعي) |
| Primary Constraint | `where T : struct` | النوع لازم يكون **struct** (قيمي) |
| Primary Constraint | `where T : enum` | النوع لازم يكون **Enum** (من C# 7.0) |
| Special Constraint | `where T : Point` | النوع لازم يكون Point أو أي كلاس وارث منه |
| Interface Constraint | `where T : IComparable` | النوع لازم يطبّق interface معين |
| Constructor Constraint | `where T : new()` | النوع لازم يكون عنده Constructor بدون باراميترات |

#### أمثلة على كل قيد:

```csharp
// Primary: class
public class MyClass<T> where T : class
{
    public T Data { get; set; }
}

// Primary: struct
public class MyStruct<T> where T : struct
{
    public T Value { get; set; }
}

// Interface: IComparable
public class Sorter<T> where T : IComparable
{
    public void Sort(T[] arr) { /* استخدام CompareTo */ }
}

// Constructor
public class Factory<T> where T : new()
{
    public T Create() => new T();
}
```

**ملاحظة:** القيود بتتكتب بعد اسم الكلاس أو الدالة، وبتساعد المترجم إنه يسمحلك تستخدم عمليات معينة جوة الكود (زي `new T()` أو `CompareTo`).

---

## 2. مبدأ DRY (Don't Repeat Yourself)

المبدأ ده بيقول: **متكررش نفس الكود أكتر من مرة**. الـ Generics هو أداة قوية لتحقيق المبدأ ده.

**قبل الـ Generics:**
- دوال منفصلة لكل نوع.

**بعد الـ Generics:**
- دالة واحدة تشتغل مع كل الأنواع.

وده اللي شايفينه في دوال `Swap` و `SearchArray` و `BubbleSort` في الكود.

---

## 3. Generic Swap Method

### قبل الـ Generics (Non-Generic Swap)

في الكود، كان في دوال منفصلة لكل نوع:
```csharp
public static void Swap(ref int X, ref int Y) { ... }
public static void Swap(ref double X, ref double Y) { ... }
public static void Swap(ref string X, ref string Y) { ... }
public static void Swap(ref Point X, ref Point Y) { ... }
```
ده تكبير رهيب للكود، ولو عايز تضيف نوع جديد هتضيف دالة جديدة.

### بعد الـ Generics (Generic Swap)

دالة واحدة بتشتغل مع أي نوع:
```csharp
public static void Swap<T>(ref T X, ref T Y)
{
    T temp = X;
    X = Y;
    Y = temp;
}
```
**شرح الكود:**
- `ref T X, ref T Y` => بنستقبل الباراميترات بالمرجع عشان نعدل في القيم الأصلية.
- `T temp` => متغير مؤقت من نفس النوع.
- التبديل عادي.

#### مثال كامل للاستخدام مع أنواع مختلفة:

```csharp
int a = 10, b = 20;
Helper.Swap(ref a, ref b);
Console.WriteLine($"a={a}, b={b}"); // a=20, b=10

double d1 = 1.5, d2 = 2.5;
Helper.Swap(ref d1, ref d2);
Console.WriteLine($"d1={d1}, d2={d2}"); // d1=2.5, d2=1.5

string s1 = "Hello", s2 = "World";
Helper.Swap(ref s1, ref s2);
Console.WriteLine($"s1={s1}, s2={s2}"); // s1=World, s2=Hello

Point p1 = new Point(1,2), p2 = new Point(3,4);
Helper.Swap(ref p1, ref p2);
Console.WriteLine($"p1={p1}, p2={p2}"); // p1=(3,4), p2=(1,2)
```

**ليه الـ Generic أحسن؟**
- لا تكرار.
- نوع-آمن (type-safe) مش هتستخدم نوع غلط بالغلط.
- أداء أفضل من استخدام `object` لأن مفيش boxing/unboxing.

---

## 4. Generic Search Method (SearchArray)

الدالة `SearchArray<T>` بتدور على قيمة في array وترجع الـ index أول مرة تظهر فيها.

```csharp
public static int SearchArray<T>(T[] Arr, T value)
{
    for (int i = 0; i < Arr?.Length; i++)
        if (EqualityComparer<T>.Default.Equals(Arr[i], value))
            return i;
    return -1;
}
```

### التحدي: إزاي نقارن قيم من نوع غير معروف؟

في الـ C#، مش كل الأنواع بتدعم استخدام `==` (مثلاً الـ structs لو معملتش overload). عشان كدا بنستخدم بدائل:

1. **`.Equals()`**:
   ```csharp
   if (Arr[i]!.Equals(value))
   ```
   - كل الأنواع بتورث من `object`، فكلها عندها `Equals`.
   - بس لو القيمة `null` ممكن ترمي استثناء، وبنستخدم `!` عشان نقول للمترجم إنها مش null (بمسؤوليتنا).

2. **`EqualityComparer<T>.Default.Equals`**:
   ```csharp
   if (EqualityComparer<T>.Default.Equals(Arr[i], value))
   ```
   - دي الطريقة الأحسن، لأنها بتستخدم الـ EqualityComparer المناسب للنوع (بتشتغل مع `null` كويس، وبتستخدم `IEquatable<T>` لو النوع مطبقها).

**ليه بنفضل `EqualityComparer`؟**
- بتتعامل مع `null` بشكل آمن.
- بتدعم `IEquatable<T>` (أداء أحسن).
- بتتجنب مشاكل الـ polymorphism اللي ممكن تحصل مع `Equals` العادي.

### مثال كامل على `SearchArray`:

```csharp
int[] numbers = { 10, 20, 30, 40, 20 };
int index = Helper.SearchArray(numbers, 20);
Console.WriteLine(index); // 1 (أول ظهور)

string[] names = { "Ali", "Omar", "Hassan" };
index = Helper.SearchArray(names, "Omar");
Console.WriteLine(index); // 1

Point[] points = { new Point(1,2), new Point(3,4) };
index = Helper.SearchArray(points, new Point(3,4));
Console.WriteLine(index); // 1 (لما يكون Equals معمولله Override)
```

**ملاحظة:** لو النوع (زي `Point`) معملش Override لـ `Equals`، المقارنة هتكون بالمرجع (reference equality) مش بالقيمة. عشان كدا بنحتاج نعمل Override عشان نقارن المحتوى.

---

## 5. Sorting with Generics (BubbleSort)

الكود فيه دالة `BubbleSort` عامة، لكنها مشتغلتش كويس في الكود الأصلي بسبب استخدام `Arr[i]` بدل `Arr[j]` في المقارنة! خلينا نصححها ونفهمها:

```csharp
public static void BubbleSort<T>(T[] Arr) where T : IComparable
{
    if (Arr == null) return;
    for (int i = 0; i < Arr.Length - 1; i++)
        for (int j = 0; j < Arr.Length - i - 1; j++)
            if (Arr[j].CompareTo(Arr[j + 1]) > 0)
                Swap(ref Arr[j], ref Arr[j + 1]);
}
```

### المشكلة: ليه منقدرش نستخدم `>` مع `T`؟
لأن `T` ممكن يكون أي نوع، ومش كل الأنواع بتدعم العامل `>` (إلا لو عامل overload). الحل: نستخدم `IComparable`.

### IComparable
- عبارة عن interface فيه دالة واحدة `CompareTo`.
- ترجع:
  - **أقل من 0** => الكائن الحالي أصغر من اللي بنقارن به.
  - **0** => متساويين.
  - **أكبر من 0** => الكائن الحالي أكبر.

### تطبيق `IComparable` في المثال

في الكود، الـ `Helper` مقيد بـ `where T : IComparable` عشان نقدر نستخدم `CompareTo`.

**طريقة تانية (أحسن) باستخدام `Comparer<T>.Default`**:
```csharp
if (Comparer<T>.Default.Compare(Arr[j], Arr[j + 1]) > 0)
```
ده بيدعم `IComparable<T>` كمان.

### مثال كامل لـ BubbleSort مع أرقام:

```csharp
int[] arr = { 5, 2, 8, 1 };
Helper.BubbleSort(arr);
Console.WriteLine(string.Join(", ", arr)); // 1, 2, 5, 8
```

---

## 6. IComparable vs IComparable<T>

| IComparable (غير العام) | IComparable<T> (العام) |
|-------------------------|------------------------|
| `CompareTo(object? obj)` | `CompareTo(T? other)` |
| بيعمل boxing للـ value types (performance overhead) | مفيش boxing لأن النوع محدد |
| مش type-safe (ممكن تدخل نوع غلط) | type-safe |
| لازم تعمل casting | من غير casting |

### ليه الأفضل تنفذ الاتنين؟

في الكود، الـ `Point` طبقت الاتنين:
```csharp
public class Point : IComparable, IComparable<Point>
```
- `IComparable` للتوافق مع الكود القديم (legacy).
- `IComparable<Point>` للأداء الأحسن والنوع-آمن.

---

## 7. تنفيذ IComparable في Point class

### 1. `CompareTo(object? obj)`
```csharp
public int CompareTo(object? obj)
{
    Point Right = obj as Point;
    if (Right is null) return -1; // null أصغر من أي حاجة
    if (X == Right.X)
        return Y.CompareTo(Right.Y);
    else
        return X.CompareTo(Right.X);
}
```
**طرق Casting:**
- **Explicit cast:** `(Point)obj` – لو فشل يرمي استثناء.
- **Pattern matching:** `if (obj is Point Right)` – آمن، وبياخد null في الاعتبار.
- **`as` keyword:** `obj as Point` – لو فشل يرجع null، أحسن.

في الكود، استخدم `as` عشان يتجنب الاستثناء.

### 2. `CompareTo(Point? point)`
```csharp
public int CompareTo(Point? point)
{
    if (point is null) return -1;
    if (X == point.X)
        return Y.CompareTo(point.Y);
    else
        return X.CompareTo(point.X);
}
```
أحسن وأسرع ومفيش casting.

### منطق المقارنة:
- بنقارن أولاً حسب `X`.
- لو `X` متساوي، بنقارن حسب `Y`.
- لو الـ object اللي جاي `null`، بنعتبره أصغر (يرجع -1).

---

## 8. Operator Overloading في Employee struct

```csharp
public struct Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public static bool operator ==(Employee left, Employee right) =>
        left.Id == right.Id && left.Name == right.Name;
    public static bool operator !=(Employee left, Employee right) =>
        left.Id != right.Id && left.Name != right.Name;
}
```

### ليه نعمل Overload للـ `==` و `!=`؟
- الـ structs مش بتورث `==` من `object` بنفس طريقة الـ classes. لو معملتش overload، `==` هتقارن بالمراجع (ولو structs قيمية، ده مش صح).
- عشان نقدر نقارن القيم (Id و Name) بطريقة طبيعية.

### إزاي بنستخدمهم؟
```csharp
Employee e1 = new Employee { Id = 1, Name = "Ali" };
Employee e2 = new Employee { Id = 1, Name = "Ali" };
Console.WriteLine(e1 == e2); // True
```

**تحذير:** لما تعمل overload لـ `==`، لازم تعمل overload لـ `!=` كمان.

---

## 9. Non-Generic Collections – ArrayList

`ArrayList` موجودة في `System.Collections`. هي عبارة عن array dynamic (بتكبر نفسها).

### خصائصها:
- بتخزن أي نوع (`object`)، فممكن تحط int, string, bool في نفس القائمة.
- فيها `Count` (عدد العناصر الفعلية) و `Capacity` (السعة الفعلية للـ array الداخلية).

### أشهر الطرق مع أمثلة:

```csharp
ArrayList list = new ArrayList();
Console.WriteLine($"Count={list.Count}, Capacity={list.Capacity}"); // 0,0

// Add – إضافة عنصر
list.Add(10);
list.Add("Hello");
list.Add(true);
Console.WriteLine($"Count={list.Count}, Capacity={list.Capacity}"); // 3,4

// AddRange – إضافة مجموعة
list.AddRange(new int[] { 1, 2, 3 });

// Remove – إزالة عنصر (أول ظهور)
list.Remove(10); // يرفع 10

// RemoveAt – إزالة حسب index
list.RemoveAt(1); // يرفع العنصر اللي في index=1

// Insert – إدراج في مكان معين
list.Insert(2, 99);

// Contains – هل بيحتوي على قيمة؟
bool hasHello = list.Contains("Hello"); // true

// IndexOf – إرجاع index أول ظهور
int idx = list.IndexOf(99);

// Clear – مسح الكل
list.Clear();

// TrimToSize – يقلل Capacity لـ Count
list.TrimToSize();
```

### مشاكل الـ ArrayList:
1. **Boxing/Unboxing:** لو حطيت قيمة قيمية (int)، هتتعمل boxing، وده بيأثر على الأداء.
2. **Type safety:** ممكن تحط أنواع مختلفة بالغلط، ولما تجي تشتغل على العناصر كمثل أرقام، هتاخد استثناء.

**مثال على الخطأ:**
```csharp
ArrayList numbers = new ArrayList();
numbers.Add(10);
numbers.Add(20);
numbers.Add("30"); // ده غلط منطقياً
int sum = 0;
foreach (int item in numbers) // InvalidCastException
    sum += item;
```

لحد هنا، احنا بنشوف ليه الـ Generic Collections هي الحل.

---

## 10. Generic Collections – List<T>

`List<T>` موجودة في `System.Collections.Generic`. هي نفس فكرة ArrayList لكن مع تحديد النوع.

### المزايا:
- Type-safe: مش هتقدر تحط غير النوع اللي حددته.
- مفيش boxing/unboxing مع الأنواع القيمية.
- أداء أحسن.

### أهم الطرق مع أمثلة:

```csharp
List<int> numbers = new List<int>();
Console.WriteLine($"Count={numbers.Count}, Capacity={numbers.Capacity}"); // 0,0

// Add
numbers.Add(1);
numbers.Add(2);

// AddRange
numbers.AddRange(new int[] { 3, 4, 5 });

// Remove
numbers.Remove(3); // يرفع أول 3

// RemoveAt
numbers.RemoveAt(1); // يرفع الـ index 1

// Insert
numbers.Insert(2, 99);

// Contains
bool exists = numbers.Contains(99); // true

// IndexOf
int idx = numbers.IndexOf(99);

// Find – يرجع أول عنصر يحقق شرط
int firstEven = numbers.Find(x => x % 2 == 0);

// FindAll – يرجع كل العناصر اللي تحقق شرط
List<int> evens = numbers.FindAll(x => x % 2 == 0);

// Exists – هل في عنصر يحقق شرط؟
bool hasEven = numbers.Exists(x => x % 2 == 0);

// ForEach – تنفيذ أكشن على كل عنصر
numbers.ForEach(x => Console.WriteLine(x));

// Sort – ترتيب
numbers.Sort();

// Reverse – عكس الترتيب
numbers.Reverse();

// Clear
numbers.Clear();

// TrimExcess – يقلل Capacity لو الفائض أقل من 10%
numbers.TrimExcess();
```

### ليه `List<T>` أحسن من ArrayList؟
- Type safety وقت الترجمة.
- أداء أفضل.
- سهولة الاستخدام مع LINQ.

---

## 11. Generic Collections – Dictionary<TKey, TValue>

`Dictionary<TKey, TValue>` عبارة عن مجموعة من الأزواج (key-value)، وكل key لازم يكون unique.

### طرق مهمة:

```csharp
Dictionary<string, int> ages = new Dictionary<string, int>();

// Add – إضافة زوج
ages.Add("Ali", 25);
ages.Add("Omar", 30);

// TryAdd – تحاول تضيف، لو key موجود ترجع false من غير استثناء
bool added = ages.TryAdd("Ali", 40); // false لأن Ali موجود

// Access using indexer
ages["Hassan"] = 22; // لو key مش موجود، بيضيفه، لو موجود بيعدل القيمة
int ageOfAli = ages["Ali"]; // 25

// ContainsKey – هل key موجود
if (ages.ContainsKey("Omar"))
    Console.WriteLine("Omar موجود");

// ContainsValue – هل value موجود (بطيئة لأنها بتدور على كل القيم)
bool hasAge30 = ages.ContainsValue(30);

// TryGetValue – تحاول تجيب قيمة key معين بدون استثناء
if (ages.TryGetValue("Mahmoud", out int age))
    Console.WriteLine(age);
else
    Console.WriteLine("Mahmoud مش موجود");

// Remove – حذف key
ages.Remove("Ali");

// Clear – مسح الكل
ages.Clear();

// Count – عدد الأزواج
Console.WriteLine(ages.Count);

// Keys – كل الـ keys
foreach (string key in ages.Keys)
    Console.WriteLine(key);

// Values – كل الـ values
foreach (int val in ages.Values)
    Console.WriteLine(val);
```

### الفهرس (Indexer)
- `dictionary[key]` بيستخدم للـ get/set.
- لو جبت قيمة بـ key مش موجود، هيرمي `KeyNotFoundException`.
- عشان كدا بنستخدم `TryGetValue` أو `ContainsKey` قبلها.

---

## 12. Iterating Collections (التكرار)

### foreach مع List
```csharp
List<int> list = new List<int> { 1, 2, 3 };
foreach (int item in list)
    Console.WriteLine(item);
```

### foreach مع Dictionary
```csharp
Dictionary<string, int> dict = new Dictionary<string, int>
{
    { "Ali", 25 },
    { "Omar", 30 }
};

// كل عنصر من نوع KeyValuePair<TKey,TValue>
foreach (KeyValuePair<string, int> pair in dict)
{
    Console.WriteLine($"Key: {pair.Key}, Value: {pair.Value}");
}
```

---

## 13. Best Practices (أفضل الممارسات)

### متى تستخدم List<T>؟
- لما تحتاج مجموعة من العناصر قابلة للتعديل (إضافة، حذف).
- لما تحتاج ترتيب معين أو وصول عشوائي (بالـ index).

### متى تستخدم Dictionary<TKey,TValue>؟
- لما تحتاج البحث السريع باستخدام key.
- لما عندك علاقة زوجية (key-value).

### متى تستخدم Generics؟
- دايمًا! إلا لو شغال مع كود قديم جدًا. استخدم `List<T>` بدل `ArrayList`، و `Dictionary<TKey,TValue>` بدل `Hashtable`.

### أداء:
- `List<T>` مع value types مفيش boxing/unboxing.
- `Dictionary` البحث فيه O(1) في المتوسط.
- تجنب `ContainsValue` لو القاموس كبير.

---

## خاتمة

الـ Generics والـ Collections العامة هي أساس كتابة كود نظيف، قابل للصيانة، و type-safe. الكود اللي شرحناه بيغطي معظم المفاهيم المهمة اللي هتحتاجها في مقابلات العمل أو في مشاريعك الفعلية.

**أهم حاجة تفتكرها:**
- Generics = reusable + type-safe
- IComparable => للمقارنة والترتيب
- List<T> و Dictionary<TKey,TValue> هما بديلك الأمثل عن ArrayList و Hashtable