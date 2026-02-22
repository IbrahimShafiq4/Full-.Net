الملف ده فيه شرح لكل حاجة موجودة في الكود بتاع `Program.cs`. الكود بيناقش مواضيع مهمة في لغة C# زي الدوال (Methods)، إزاي بنمرر الباراميترز (Passing Parameters)، الـ Nullable types، الـ Boxing و Unboxing، الـ params، وحلول لمسائل بسيطة.

## محتويات الشرح

- [محتويات الشرح](#محتويات-الشرح)
- [namespace والـ class](#namespace-والـ-class)
- [منطقة Methods (الدوال)](#منطقة-methods-الدوال)
  - [إزاي نعرف دالة](#إزاي-نعرف-دالة)
  - [الباراميترز الاختيارية (Optional Parameters) والـ Named Arguments](#الباراميترز-الاختيارية-optional-parameters-والـ-named-arguments)
- [Boxing vs UnBoxing](#boxing-vs-unboxing)
- [أنواع Null (Nullable Types)](#أنواع-null-nullable-types)
  - [إزاي نخلي الـ value type يقبل null](#إزاي-نخلي-الـ-value-type-يقبل-null)
  - [العامل ?? (Null-Coalescing)](#العامل--null-coalescing)
- [العامل ?. (Null Propagation)](#العامل--null-propagation)
- [أمثلة على استدعاء الدوال](#أمثلة-على-استدعاء-الدوال)
- [تمرير الباراميترز بـ Value (لـ Value Type)](#تمرير-الباراميترز-بـ-value-لـ-value-type)
- [تمرير الباراميترز بـ Reference (لـ Value Type)](#تمرير-الباراميترز-بـ-reference-لـ-value-type)
- [تمرير الـ Reference Types (زي الـ Array)](#تمرير-الـ-reference-types-زي-الـ-array)
  - [By Value](#by-value)
  - [By Reference](#by-reference)
- [الـ Out Keyword](#الـ-out-keyword)
- [الـ Params Keyword](#الـ-params-keyword)

---

## namespace والـ class

```csharp
namespace SESSION_1
{
    internal class Program
    {
        // ... هنا الدوال و main
    }
}
```

- **namespace**: دي بادئة بنستخدمها عشان ننظم الكود بتاعنا، زي فولدر للمشروع.
- **class**: كل حاجة في C# لازم تكون جوه class. الـ `Program` ده اسم الكلاس الأساسي اللي فيه الدالة `Main` (البداية).

---

## منطقة Methods (الدوال)

### إزاي نعرف دالة

الدالة (أو method) هي مجموعة أوامر بنقدر ننفذها أكتر من مرة من غير ما نكرر الكتابة.  
- الدالة لازم تكون جوه class.
- ممكن الدالة ترجعلك قيمة (زي int أو string) أو مترجعهش حاجة (void).

مثال:
```csharp
public static void Print()
{
    Console.WriteLine("Hello World");
}
```

- **public**: معناها إن أي حد في المشروع يقدر يستخدم الدالة دي.
- **static**: عشان نقدر ننادي عليها من جوه `Main` (لأن `Main` static).

### الباراميترز الاختيارية (Optional Parameters) والـ Named Arguments

- ممكن تعمل باراميتر له قيمة افتراضية، فتبقى مش مضطر تديه قيمة كل ما تنادي الدالة.  
- الباراميتر الاختياري لازم يجي في الآخر بعد كل الباراميترز الإجبارية.

مثال:
```csharp
public static void Print(int number, string pattern = "$")
{
    for (int i = 0; i < number; i++)
        Console.Write(pattern + " ");
}
```

لما نناديها:
```csharp
Print(5, "#");           // عادي
Print(pattern: "#", number: 20); // باستخدام named arguments (مش مهم الترتيب)
Print(number: 20);       // استخدم القيمة الافتراضية "$" للـ pattern
```

---

## Boxing vs UnBoxing

الموضوع ده بين الـ value types (زي int, double) والـ reference types (زي object).

- **Boxing**: تحويل value type لـ object. بيتم بشكل ضمني (آمن).
  ```csharp
  int x = 10;
  object obj = x; // boxing
  ```
- **Unboxing**: العكس، استخراج القيمة من object. لازم تعمله بشكل صريح (casting) ولو نوع البيانات مش مطابق هيحصل خطأ.
  ```csharp
  int y = (int)obj; // unboxing
  ```

**ملاحظة**: لو الـ object كان فيه double مثلًا وحاولت تخرجه int، هيظهر استثناء `InvalidCastException`.

---

## أنواع Null (Nullable Types)

### إزاي نخلي الـ value type يقبل null

الأنواع الأساسية زي `int` مش بتقبل `null`. لو عاوزها تقبل null، استخدم `Nullable<T>` أو أضف علامة `?`.

```csharp
Nullable<int> x = null;
int? y = null;   // الطريقة الأسهل
```

- الخاصية `HasValue`: بتقولك فيه قيمة ولا null.
- الخاصية `Value`: بترجع القيمة (لو null هتطلع خطأ).

```csharp
if (y.HasValue)
    Console.WriteLine(y.Value);
```

### العامل ?? (Null-Coalescing)

العامل ده بيديك قيمة بديلة لو المتغير كان null.

```csharp
int? y = null;
int? z = null;
int x = y ?? z ?? 0;  // لو y و z null، يبقى x = 0
```

لو `y` مش nullable (يعني int عادي) ممكن تعمل cast:
```csharp
x = (int?)y ?? z ?? 0;
```

---

## العامل ?. (Null Propagation)

العامل ده بيحميك من استثناء الـ NullReferenceException. لو الكائن اللي بتستخدمه null، مش هيكمل.

مثال:
```csharp
int[] arr = null;
for (int i = 0; i < arr?.Length; i++) // لو arr null، arr?.Length تبقى null، والشرط يبقى false
{
    Console.WriteLine(arr[i]);
}
```

---

## أمثلة على استدعاء الدوال

ده مثال لاستخدام الدوال اللي عرفناها فوق:

```csharp
Print();                     // بتشتغل النسخة اللي من غير باراميترز
Print("Hello world");        // النسخة اللي بتاخد string
Print(5, "#");               // النسخة اللي بتاخد int و string
Print(pattern: "#", number: 20); // استخدام named arguments
Print(number: 20);           // استخدم القيمة الافتراضية للـ pattern
```

---

## تمرير الباراميترز بـ Value (لـ Value Type)

لما تبعت value type من غير `ref`، الدالة بتاخد نسخة من القيمة. أي تغيير جوه الدالة مش بيأثر على المتغير الأصلي.

```csharp
public static void Swap(int fNum, int lNum)
{
    int tNum = fNum;
    fNum = lNum;
    lNum = tNum;
    Console.WriteLine($"جوه الدالة: fNum={fNum}, lNum={lNum}");
}

// الاستدعاء
int num01 = 5, num02 = 10;
Swap(num01, num02);
Console.WriteLine($"برا الدالة: num01={num01}, num02={num02}"); // لسه 5 و 10
```

---

## تمرير الباراميترز بـ Reference (لـ Value Type)

لما تستخدم `ref`، بتبعت عنوان المتغير نفسه في الذاكرة. أي تغيير جوه الدالة بيأثر على الأصل.

```csharp
public static void Swap(ref int x, ref int y)
{
    int temp = x;
    x = y;
    y = temp;
}

// الاستدعاء
int num01 = 5, num02 = 10;
Swap(ref num01, ref num02);
Console.WriteLine($"بعد الـ swap: num01={num01}, num02={num02}"); // بقت 10 و 5
```

---

## تمرير الـ Reference Types (زي الـ Array)

### By Value

لما تبعت array (أو أي reference type) من غير `ref`، بتبعت نسخة من المرجع (المرجع بيفضل شايف نفس المكان في الـ heap). يعني لو غيرت محتويات الكائن (زي أول عنصر في الـ array)، التغيير بيبان برا الدالة. لكن لو حاولت تعمل `new array` جوه الدالة، المرجع الأصلي لسه شايف القديم.

```csharp
public static int SumArray(int[] Arr)
{
    Arr[0] = 100;          // ده بيأثر على الأصل
    int sum = 0;
    for (int i = 0; i < Arr.Length; i++)
        sum += Arr[i];
    return sum;
}

// الاستدعاء
int[] Arr = { 1, 2, 3 };
Console.WriteLine(SumArray(Arr)); // الناتج 106
Console.WriteLine(Arr[0]);         // بقى 100 (اتغير)
```

### By Reference

لما تستخدم `ref` مع array، بتبعت المرجع نفسه. يعني لو عملت `new array` جوه الدالة، الأصل يبقى شايف الجديد.

```csharp
public static int SumArray(ref int[] Arr)
{
    Arr = new int[] { 4, 5, 6 };   // الأصل هيتغير ويشاور على الـ array الجديد
    int sum = 0;
    for (int i = 0; i < Arr.Length; i++)
        sum += Arr[i];
    return sum;
}

// الاستدعاء
int[] Arr = { 1, 2, 3 };
Console.WriteLine(SumArray(ref Arr)); // الناتج 15
Console.WriteLine(Arr[0]);             // بقى 4 (الأصل بقى يشاور على الـ array الجديد)
```

---

## الـ Out Keyword

الـ `out` شبه `ref` لكن:
- المتغير اللي بتبعته مش محتاج يكون initialized قبل الاستدعاء.
- الدالة لازم تحط قيمة للـ out parameter قبل ما تخلص.

مثال:
```csharp
public static void SumMul(int x, int y, out int Sum, out int Mul)
{
    Sum = x + y;
    Mul = x * y;
}

// الاستدعاء
int a = 5, b = 10;
SumMul(a, b, out int sumResult, out int mulResult);
Console.WriteLine($"المجموع={sumResult}, الضرب={mulResult}");

// لو عاوز تتجاهل باراميتر معين استخدم _
SumMul(a, b, out sumResult, out _); // mulResult مش هتستخدمه
```

---

## الـ Params Keyword

الكلمة دي بتخلي الدالة تقبل عدد متغير من الباراميترز من نفس النوع. لازم يكون آخر باراميتر في الدالة.

```csharp
public static int SumArray(int sum, string message = "hello", params int[] Arr)
{
    sum = 0;
    for (int i = 0; i < Arr?.Length; i++)
        sum += Arr[i];
    return sum;
}

// الاستدعاء
int result = SumArray(0, "test", 1, 2, 3, 4, 5); // Arr = {1,2,3,4,5}
// أو استخدم named argument عشان تتخطى الـ message
result = SumArray(0, Arr: new int[] { 1, 2, 3 });
```