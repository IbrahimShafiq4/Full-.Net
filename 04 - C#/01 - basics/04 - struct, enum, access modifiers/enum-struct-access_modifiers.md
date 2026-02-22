# 📘 شرح الـ Enum و Struct و Access Modifiers في C#  

## 📌 جدول المحتويات
1.  **الـ Enum**
    - تعريف
    - العمليات الأساسية
    - أهم 10 Methods مع أمثلة شاملة
    - التطبيقات المتقدمة (Flags, Attributes, Extension Methods)
2.  **الـ Struct**
    - تعريف وخصائص
    - الفروق الجوهرية عن الـ Class
    - الـ Record Struct (الجديد)
    - أهم 10 Methods مع أمثلة شاملة
    - حالات الاستخدام الواقعية
3.  **الـ Access Modifiers** (بالتفصيل الممل)
4.  **الخلاصة والمرجع السريع**

---

# 🧩 الفصل الأول: الـ Enum (التعداد)

## ١.١ يعني إيه Enum؟

الـ **Enum** هو نوع من الـ **Value Types** (أنواع القيم) بيستخدم لتمثيل مجموعة من **الثوابت (Constants)** المرتبطة ببعضها. هو عبارة عن أسماء (Labels) ليها قيم رقمية (integer) تحت الغطاء.

**مثال من الحياة:** أيام الأسبوع، شهور السنة، أنواع المستخدمين (Admin, User, Guest)، حالات الطلب (Pending, Shipped, Delivered).

```csharp
// تعريف Enum بسيط
public enum DaysOfWeek
{
    Sunday,    // القيمة الافتراضية = 0
    Monday,    // = 1
    Tuesday,   // = 2
    Wednesday, // = 3
    Thursday,  // = 4
    Friday,    // = 5
    Saturday   // = 6
}
```

## ١.٢ تغيير القيم الافتراضية

```csharp
public enum HttpStatusCode
{
    Continue = 100,
    OK = 200,
    Created = 201,
    Accepted = 202,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServerError = 500
}
```

ممكن تخلي أرقام مش متتالية عادي، وممكن تخلي قيم مكررة (بس بلاش عشان الهيصة).

## ١.٣ تغيير النوع الأساسي (Underlying Type)

افتراضيًا هو `int`. لكن تقدر تغيره لأي نوع رقمي: `byte`, `sbyte`, `short`, `ushort`, `uint`, `long`, `ulong`.

```csharp
public enum Permissions : byte  // عشان نوفر في المساحة
{
    Read = 1,
    Write = 2,
    Execute = 4,
    Delete = 8
}
```

## ١.٤ أهم Built-in Methods في Enum (أكتر من ١٠ طرق)

هناخد أمثلة كاملة على كل Method.

### 1️⃣ `Enum.Parse()`
بتحول `string` إلى `Enum` value. لازم الـ string يطابق اسم عضو موجود، وإلا هيحصل `ArgumentException`.

```csharp
using System;

public class Program
{
    public enum Color { Red, Green, Blue, Yellow }

    public static void Main()
    {
        string colorName = "Green";

        try
        {
            // التحويل
            Color myColor = (Color)Enum.Parse(typeof(Color), colorName);
            Console.WriteLine($"Parsed Successfully: {myColor}"); // Output: Green
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Try with a name that doesn't exist
        colorName = "Black";
        try
        {
            Color myColor = (Color)Enum.Parse(typeof(Color), colorName); // هيبوظ هنا
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"'{colorName}' is not a valid color.");
        }
    }
}
```

### 2️⃣ `Enum.TryParse()` 🥇 (الأهم والأأمن)
بتعمل نفس الوظيفة بس مترميش Exception. بترجع `bool` عشان تعرف نجحت ولا لأ.

```csharp
public static void Main()
{
    string input = "Blue";
    if (Enum.TryParse(input, out Color result))
    {
        Console.WriteLine($"Parsed: {result}"); // Blue
    }
    else
    {
        Console.WriteLine("Invalid color name.");
    }

    input = "123"; // TryParse مش هتعتبر ده رقم، هتعتبره اسم. لو عايز ترقم استخدم IsDefined.
    if (Enum.TryParse(input, out result))
    {
        // غريب! TryParse بتفلح لو الرقم موجود في نطاق الـ underlying type
        // حتى لو مش عضو. لازم نستخدم Enum.IsDefined بعدها.
        Console.WriteLine($"Parsed number: {result}"); // هيطبع 123
    }
}
```
**ملحوظة خطيرة:** `TryParse` بتفلح حتى لو دخلت رقم مش موجود في الـ Enum. عشان تتأكد إنه عضو حقيقي، استخدم `Enum.IsDefined`.

### 3️⃣ `Enum.IsDefined()`
بتتأكد إن القيمة (رقم أو اسم) موجودة فعلاً في الـ Enum.

```csharp
public static void Main()
{
    int value = 2;
    bool isDefined = Enum.IsDefined(typeof(Color), value);
    Console.WriteLine($"Is 2 defined in Color? {isDefined}"); // True (Blue)

    value = 10;
    isDefined = Enum.IsDefined(typeof(Color), value);
    Console.WriteLine($"Is 10 defined? {isDefined}"); // False

    string name = "Green";
    isDefined = Enum.IsDefined(typeof(Color), name);
    Console.WriteLine($"Is 'Green' defined? {isDefined}"); // True

    name = "Purple";
    isDefined = Enum.IsDefined(typeof(Color), name);
    Console.WriteLine($"Is 'Purple' defined? {isDefined}"); // False
}
```

### 4️⃣ `Enum.GetNames()`
بيرجع array من strings بأسماء كل القيم في الـ Enum.

```csharp
public static void Main()
{
    string[] colorNames = Enum.GetNames(typeof(Color));

    Console.WriteLine("All Color Names:");
    foreach (string name in colorNames)
    {
        Console.WriteLine($"  - {name}");
    }
    // Output:
    // All Color Names:
    //   - Red
    //   - Green
    //   - Blue
    //   - Yellow
}
```

### 5️⃣ `Enum.GetValues()`
بيرجع array من objects (لازم تعمل Cast) بكل قيم الـ Enum.

```csharp
public static void Main()
{
    Array colorValues = Enum.GetValues(typeof(Color));

    Console.WriteLine("All Color Values:");
    foreach (Color color in colorValues) // C# بتعمل Cast ضمني
    {
        Console.WriteLine($"  - {color} = {(int)color}");
    }
    // Output:
    // All Color Values:
    //   - Red = 0
    //   - Green = 1
    //   - Blue = 2
    //   - Yellow = 3
}
```

### 6️⃣ `Enum.GetName()`
بتديلها قيمة (رقم)، وترجعلك اسم العضو كـ string.

```csharp
public static void Main()
{
    string name = Enum.GetName(typeof(Color), 2);
    Console.WriteLine($"The color with value 2 is: {name}"); // Blue

    name = Enum.GetName(typeof(Color), 10);
    Console.WriteLine($"The color with value 10 is: {name ?? "null"}"); // null
}
```

### 7️⃣ `Enum.GetUnderlyingType()`
بيرجعلك الـ Type بتاع النوع الأساسي للـ Enum.

```csharp
public enum MyEnum : short { A, B }

public static void Main()
{
    Type underlyingType = Enum.GetUnderlyingType(typeof(MyEnum));
    Console.WriteLine(underlyingType); // System.Int16

    underlyingType = Enum.GetUnderlyingType(typeof(Color));
    Console.WriteLine(underlyingType); // System.Int32
}
```

### 8️⃣ `Enum.ToObject()`
بتديلها قيمة رقمية، بتحولها لـ Enum object.

```csharp
public static void Main()
{
    int value = 1;
    Color color = (Color)Enum.ToObject(typeof(Color), value);
    Console.WriteLine(color); // Green

    byte b = 3;
    color = (Color)Enum.ToObject(typeof(Color), b);
    Console.WriteLine(color); // Yellow
}
```

### 9️⃣ `Enum.Format()`
بتديلها Enum value وتحدد إزاي تعرضها: `"G"` للاسم، `"D"` للرقم، `"X"` للـ hexadecimal.

```csharp
public static void Main()
{
    Color c = Color.Blue;

    string nameFormat = Enum.Format(typeof(Color), c, "G");
    Console.WriteLine(nameFormat); // Blue

    string decimalFormat = Enum.Format(typeof(Color), c, "D");
    Console.WriteLine(decimalFormat); // 2

    string hexFormat = Enum.Format(typeof(Color), c, "X");
    Console.WriteLine(hexFormat); // 00000002 (حسب طول الـ int)
}
```

### 🔟 `HasFlag()` (مهم مع الـ Flags)
بتستخدم مع Enums عليها `[Flags]` عشان تشوف إذا كانت قيمة معينة موجودة في مجموعة.

```csharp
[Flags]
public enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}

public static void Main()
{
    FilePermissions perms = FilePermissions.Read | FilePermissions.Execute;

    bool canRead = perms.HasFlag(FilePermissions.Read);
    bool canWrite = perms.HasFlag(FilePermissions.Write);

    Console.WriteLine($"Can Read? {canRead}");   // True
    Console.WriteLine($"Can Write? {canWrite}"); // False
}
```

---

## ١.٥ الـ Flags Attribute (موضوع متقدم)

لما تحط `[Flags]` على Enum، بتقدر تجمع أكتر من قيمة في متغير واحد باستخدام الـ Bitwise OR.

**ليه بنستخدم أرقام 1, 2, 4, 8؟**
عشان كل قيمة تمثل بت واحد (bit) مختلف.
- 1 = 0001
- 2 = 0010
- 4 = 0100
- 8 = 1000

كدا مفيش تداخل، وتقدر تعرف أي البتات الشغالة.

```csharp
[Flags]
public enum PizzaToppings
{
    None = 0,
    Cheese = 1,      // 0001
    Pepperoni = 2,   // 0010
    Mushrooms = 4,   // 0100
    Onions = 8,      // 1000
    Olives = 16      // 0001 0000
}

class Program
{
    static void Main()
    {
        // هنعمل بيتزا بالجبنة والبيبروني والفطر
        PizzaToppings myPizza = PizzaToppings.Cheese | PizzaToppings.Pepperoni | PizzaToppings.Mushrooms;

        Console.WriteLine(myPizza); // Cheese, Pepperoni, Mushrooms

        // إضافة topping
        myPizza |= PizzaToppings.Onions;
        Console.WriteLine(myPizza); // Cheese, Pepperoni, Mushrooms, Onions

        // إزالة topping
        myPizza &= ~PizzaToppings.Pepperoni;
        Console.WriteLine(myPizza); // Cheese, Mushrooms, Onions

        // التحقق
        bool hasCheese = (myPizza & PizzaToppings.Cheese) == PizzaToppings.Cheese;
        bool hasOlives = (myPizza & PizzaToppings.Olives) == PizzaToppings.Olives;
        Console.WriteLine($"Has Cheese? {hasCheese}"); // True
        Console.WriteLine($"Has Olives? {hasOlives}"); // False

        // باستخدام HasFlag (أسهل لكن أبطأ)
        Console.WriteLine(myPizza.HasFlag(PizzaToppings.Mushrooms)); // True
    }
}
```

---

## ١.٦ طرق متقدمة للتعامل مع Enum (Extension Methods)

لما تحس إنك محتاج تضيف وظيفة معينة للـ Enum بتاعك، استخدم **Extension Methods**. ده مش built-in، لكنه من أعظم الحاجات اللي هتستخدمها في الشغل.

```csharp
public enum Grade
{
    F = 0,
    D = 1,
    C = 2,
    B = 3,
    A = 4
}

// Extension method للـ Grade
public static class GradeExtensions
{
    public static bool IsPassing(this Grade grade, Grade minPassing = Grade.D)
    {
        return grade >= minPassing;
    }

    public static string ToLetterGrade(this Grade grade)
    {
        return grade switch
        {
            Grade.A => "A (Excellent)",
            Grade.B => "B (Good)",
            Grade.C => "C (Average)",
            Grade.D => "D (Below Average)",
            Grade.F => "F (Failing)",
            _ => "Unknown"
        };
    }
}

class Program
{
    static void Main()
    {
        Grade studentGrade = Grade.C;

        // كأنها method جوه الـ Enum نفسه!
        if (studentGrade.IsPassing())
        {
            Console.WriteLine($"Student passed with grade: {studentGrade.ToLetterGrade()}");
        }
        else
        {
            Console.WriteLine("Student failed.");
        }

        // Output: Student passed with grade: C (Average)
    }
}
```

---

# 🧱 الفصل الثاني: الـ Struct (الهيكل)

## ٢.١ يعني إيه Struct؟

الـ `struct` هو **Value Type**، بيتم تخزينه في الـ **Stack** (في الغالب)، وده بيخليه أسرع في الوصول من الـ Class. بنستخدمه عشان نمثل **كائنات صغيرة الحجم وخفيفة**، زي نقطة (Point)، لون (Color)، أو رقم مركب (Complex Number).

## ٢.٢ الفروق الجوهرية بين Struct و Class

| الخاصية | Struct (Value Type) | Class (Reference Type) |
| :--- | :--- | :--- |
| **مكان التخزين** | Stack (أو جزء من Heap لو كان جزء من Class) | Heap |
| **النسخ** | بنسخ القيمة (Copy by value) | بنسخ العنوان (Copy by reference) |
| **الـ Default** | Not nullable بشكل مباشر (إلا باستخدام `Nullable<T>`)| Nullable |
| **الوراثة** | لا يورث ولا يورث منه (باستثناء الـ Interfaces) | بيدعم الوراثة |
| **الـ Constructor** | مينفعش تعرف parameterless constructor (إلى حد قريب) | ينفع |
| **الأداء** | أسرع للكائنات الصغيرة | مناسب للكائنات الكبيرة |
| **التسليم لدالة** | بينسخ (pass by value) | بيمرر العنوان (pass by reference) |

## ٢.٣ تعريف Struct

```csharp
public struct Point
{
    // Fields
    public int X;
    public int Y;

    // Constructor (لازم ت initialize كل الـ fields)
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Method
    public double DistanceTo(Point other)
    {
        int dx = X - other.X;
        int dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    // Property
    public int Sum => X + Y;

    // Override Method من Object
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
```

## ٢.٤ استخدام Struct مع `new` وبدون `new`

```csharp
public static void Main()
{
    // باستخدام new (بيستدعي الـ constructor)
    Point p1 = new Point(3, 5);
    Console.WriteLine(p1); // (3, 5)

    // بدون new (لازم ت initialize كل الحقول قبل الاستخدام)
    Point p2;
    p2.X = 10;
    p2.Y = 20;
    Console.WriteLine(p2); // (10, 20)

    // لو حاولت تستخدم p2 من غير ما تعين Y هيديك Compile Error
    // Point p3;
    // Console.WriteLine(p3.X); // Error! Use of unassigned local variable 'p3'
}
```

## ٢.٥ الـ Record Struct (C# 10+)

الـ `record struct` هي ميزة جامدة جدًا، بتخليني أعرف Struct بيدعم **Value-based equality** وبتولّدلي كود كتير تلقائيًا (زي ToString, Deconstruct, etc.).

```csharp
public readonly record struct Coordinates(double Latitude, double Longitude);

class Program
{
    static void Main()
    {
        Coordinates c1 = new(30.0444, 31.2357);
        Coordinates c2 = new(30.0444, 31.2357);

        // Value equality (True)
        Console.WriteLine(c1 == c2); // True

        // ToString automatically generated
        Console.WriteLine(c1); // Coordinates { Latitude = 30.0444, Longitude = 31.2357 }

        // Deconstruction
        var (lat, lon) = c1;
        Console.WriteLine($"Lat: {lat}, Lon: {lon}");

        // Nondestructive mutation (with expression)
        Coordinates c3 = c1 with { Longitude = 31.5 };
        Console.WriteLine(c3); // Coordinates { Latitude = 30.0444, Longitude = 31.5 }
    }
}
```
الـ `readonly` هنا بتخلي الحقول Immutable.

## ٢.٦ Struct بتنفذ Interface

الـ Struct يقدر ينفذ Interfaces زي الـ Class بالظبط.

```csharp
public interface IShape
{
    double Area();
}

public struct Square : IShape
{
    public int SideLength;

    public Square(int side) => SideLength = side;

    public double Area()
    {
        return SideLength * SideLength;
    }
}

public static void Main()
{
    Square s = new Square(5);
    Console.WriteLine(s.Area()); // 25

    // Boxing happens when casting to interface
    IShape shape = s; // هنا s بتتعمل لها Boxing عشان تتحط في Heap
}
```

## ٢.٧ أهم Built-in Methods في Struct (وارثهم من System.ValueType)

كل Struct بيورث من `System.ValueType`، والـ `System.ValueType` بيورث من `System.Object`. لكن `ValueType` بيعمل Override لبعض الدوال عشان تخليها مناسبة للـ Value Types.

### 1️⃣ `ToString()`
الافتراضي فيها بتجيب اسم الـ Type. أحسن حاجة إنك دايماً تعملها Override.

```csharp
public struct Person
{
    public string Name;
    public int Age;

    public override string ToString() => $"{Name} ({Age} years old)";
}

public static void Main()
{
    Person p = new Person { Name = "Omar", Age = 30 };
    Console.WriteLine(p.ToString()); // Omar (30 years old)
}
```

### 2️⃣ `Equals(object obj)`
الافتراضي في `ValueType` بيستخدم Reflection عشان يقارن كل حقل بحقل. ده بطيء. دايماً حاول تعمل Override وتحسن الأداء.

```csharp
public struct Money : IEquatable<Money>
{
    public decimal Amount;
    public string Currency;

    public override bool Equals(object obj)
    {
        return obj is Money other && Equals(other);
    }

    public bool Equals(Money other)
    {
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }
}
```

### 3️⃣ `GetHashCode()`
لو عايز تستخدم Struct بتاعك كمفتاح في Dictionary أو HashSet، لازم ت implement `GetHashCode` كويس.

```csharp
public readonly struct Point3D
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public Point3D(int x, int y, int z)
    {
        X = x; Y = y; Z = z;
    }

    public override int GetHashCode()
    {
        // طريقة متطورة عشان تقلل الـ collisions
        return HashCode.Combine(X, Y, Z);
    }
}
```

### 4️⃣ `GetType()`
بتجيب الـ Type بتاع الـ Struct. دي Method مش Override-able.

```csharp
public static void Main()
{
    Point p = new Point(1, 2);
    Type t = p.GetType();
    Console.WriteLine(t); // Point
}
```

### 5️⃣ `ReferenceEquals()` (Static)
بتقارن إذا كان object معين هو نفسه. مع الـ Struct دايمًا بترجع `false` لأن الـ Struct بتتعمل Boxing وبتتبعت بقيمتها.

```csharp
Point p1 = new Point(1, 1);
Point p2 = p1;

// هترجع false لأنهم boxed في objects مختلفة
Console.WriteLine(object.ReferenceEquals(p1, p2)); // False
```

### 6️⃣ `MemberwiseClone()` (Protected)
بتعمل نسخة سطحية (Shallow Copy) من الـ Struct. مش هتشوفها كتير لأن الـ Struct أصلاً بيتنسخ بالـ assignment.

## ٢.٨ قيود على Struct

1. **مينفعش تعرف parameterless constructor** (إلى وقت قريب). في C# 10، بقى ينفع تعرف parameterless constructor، لكن لازم تستدعيه بـ `new`.
2. **مينفعش تعمل inheritance**.
3. **مينفعش تـ initialize fields مباشرة** (إلا مع C# 10+ باستخدام parameterless constructor أو `field = value` في مكان التعريف).
4. **مش مناسب للكائنات الكبيرة** (أكبر من 16-24 بايت)، لأن النسخ كتير هيكلف أداء.

## ٢.٩ إمتى أستخدم Struct؟

- لما الكائن صغير (حجمه أقل من 16 بايت).
- لما الكائن immutable (مايتغيرش بعد ما اتعمل).
- لما محتاج أداء عالي جدًا ومش عاوز GC pressure.
- لما الكائن بيتم استخدامه في Arrays كتير (زي arrays من النقط).
- لما الكائن logically عبارة عن value واحدة زي `int` أو `double`.

**أمثلة من الـ .NET نفسها:**
- `int`, `double`, `bool` (كلها structs)
- `DateTime`
- `TimeSpan`
- `Guid`
- `KeyValuePair<TKey, TValue>`
- `Complex`

---

# 🔐 الفصل الثالث: Access Modifiers (معدّلات الوصول)

دي حاجة أساسية جدًا في تنظيم الكود وحمايته.

| المعدّل | الوصف | الاستخدام |
| :--- | :--- | :--- |
| **`public`** | أي كلاس في أي مكان يقدر يوصل. | API, Services, Interfaces |
| **`private`** | متاح بس جوه نفس الـ class أو struct. | Helper methods, internal data |
| **`protected`** | متاح جوه نفس الكلاس وأي كلاس وارث منه. | Methods اللي محتاجها الـ children |
| **`internal`** | متاح جوه نفس الـ Assembly (Project) بس. | Implementation details |
| **`protected internal`** | متاح جوه نفس الـ Assembly أو لأي وارث (حتى لو في Assembly تاني). | Advanced scenarios |
| **`private protected`** (C# 7.2+) | متاح جوه نفس الـ Assembly وجوه الـ derived classes. | أكثر تقييدًا من `protected` |

## مثال شامل لكل الحالات

```csharp
public class MyBaseClass
{
    public int PublicField = 1;
    private int PrivateField = 2;
    protected int ProtectedField = 3;
    internal int InternalField = 4;
    protected internal int ProtectedInternalField = 5;
    private protected int PrivateProtectedField = 6;

    public void ShowFields()
    {
        Console.WriteLine(PublicField);           // OK
        Console.WriteLine(PrivateField);          // OK
        Console.WriteLine(ProtectedField);        // OK
        Console.WriteLine(InternalField);         // OK
        Console.WriteLine(ProtectedInternalField);// OK
        Console.WriteLine(PrivateProtectedField); // OK
    }
}

public class DerivedClassInSameAssembly : MyBaseClass
{
    public void ShowFields()
    {
        Console.WriteLine(PublicField);           // OK
        // Console.WriteLine(PrivateField);       // Error
        Console.WriteLine(ProtectedField);        // OK
        Console.WriteLine(InternalField);         // OK (نفس الـ assembly)
        Console.WriteLine(ProtectedInternalField);// OK
        Console.WriteLine(PrivateProtectedField); // OK (نفس الـ assembly)
    }
}

// في Assembly تاني
public class DerivedClassInOtherAssembly : MyBaseClass
{
    public void ShowFields()
    {
        Console.WriteLine(PublicField);           // OK
        // Console.WriteLine(PrivateField);       // Error
        Console.WriteLine(ProtectedField);        // OK
        // Console.WriteLine(InternalField);      // Error (مش نفس الـ assembly)
        Console.WriteLine(ProtectedInternalField);// OK (لأن protected)
        // Console.WriteLine(PrivateProtectedField); // Error (private protected)
    }
}
```

---

# ✅ الخلاصة النهائية والمرجع السريع

## 📌 تذكر دائمًا:

### Enum
- استخدم `Enum.TryParse()` بدل `Parse()` عشان تتجنب الـ Exceptions.
- استخدم `Enum.IsDefined()` لو كنت بتتعامل مع أرقام من مصادر خارجية.
- استخدم `[Flags]` مع أرقام 1,2,4,8 عشان تمثل مجموعات.
- استخدم Extension Methods عشان تضيف وظائف للـ Enum بتاعك.

### Struct
- استخدم Struct للكائنات الصغيرة (<16 بايت) واللي ملهاش داعي للوراثة.
- دائمًا override `ToString()` و `Equals()` و `GetHashCode()` عشان تحسن الأداء.
- خلي الـ Struct **immutable** (استخدم `readonly` fields).
- فكر في `readonly record struct` لو محتاج Value semantics و ToString مجاني.

### Access Modifiers
- **ابدأ بـ `private`** ووسع المدى لو محتاج.
- استخدم `internal` عشان تخفي الـ implementation.
- استخدم `protected` للـ inheritance hierarchies.
- استخدم `public` فقط للـ API اللي المفروض المستخدم يشوفها.

---

## 🎯 أهم Built-in Methods اللي لازم تحفظها (Reference Card)

### في الـ Enum:
| Method | الهدف |
|--------|-------|
| `Enum.TryParse<>()` | آمن وأفضل طريقة للتحويل من string |
| `Enum.IsDefined()` | التحقق من صحة القيمة |
| `Enum.GetValues()` | جلب جميع القيم (لعمل Dropdown مثلًا) |
| `Enum.GetNames()` | جلب جميع الأسماء |
| `HasFlag()` | التحقق من وجود Flag في مجموعة |

### في الـ Struct:
| Method | الهدف |
|--------|-------|
| `ToString()` | تمثيل readable للكائن |
| `Equals()` | مقارنة القيم (أعد كتابتها دائمًا) |
| `GetHashCode()` | للاستخدام في الـ Dictionary/HashSet |
| `GetType()` | معرفة نوع الكائن في وقت التشغيل |