
# 1️⃣ Passing Parameters in C#

## 🔹 أولًا: Value Type Parameters

### 📌 ما هي الـ Value Types؟

هي أنواع البيانات التي تخزن القيمة نفسها داخل المتغير، مثل:

```
int, double, char, bool, struct
```

---

## ✳️ Passing Value Type By Value (الوضع الافتراضي)

### ماذا يحدث؟

يتم إرسال **نسخة من القيمة** إلى الدالة.
أي تعديل داخل الدالة لا يؤثر على المتغير الأصلي.

### مثال:

```csharp
using System;

class Program
{
    static void ChangeValue(int x)
    {
        x = 100;
    }

    static void Main()
    {
        int number = 10;
        ChangeValue(number);
        Console.WriteLine(number);
    }
}
```

### 🔍 شرح الكود:

* `int number = 10;` → المتغير الأصلي قيمته 10
* عند استدعاء `ChangeValue(number);`

  * يتم إرسال **نسخة** من الرقم 10
* داخل الدالة:

  * `x = 100;` → يتم تعديل النسخة فقط
* عند الطباعة → يظل `number = 10`

✅ الناتج:

```
10
```

---

## ✳️ Passing Value Type By Reference (باستخدام ref)

### ماذا يحدث؟

يتم إرسال **العنوان نفسه في الذاكرة** وليس نسخة.

### مثال:

```csharp
using System;

class Program
{
    static void ChangeValue(ref int x)
    {
        x = 100;
    }

    static void Main()
    {
        int number = 10;
        ChangeValue(ref number);
        Console.WriteLine(number);
    }
}
```

### 🔍 شرح مهم:

* استخدمنا `ref` في:

  * تعريف الدالة
  * استدعاء الدالة
* الآن أي تعديل على `x` يؤثر على `number`

✅ الناتج:

```
100
```

---

# 2️⃣ Passing Reference Type Parameters

## 📌 ما هي Reference Types؟

هي أنواع البيانات التي تخزن **عنوان في الذاكرة** مثل:

```
class, string, array
```

---

## ✳️ Passing Reference Type By Value

### ماذا يحدث؟

يتم إرسال نسخة من العنوان.

```csharp
using System;

class Person
{
    public string Name;
}

class Program
{
    static void ChangeName(Person p)
    {
        p.Name = "Ali";
    }

    static void Main()
    {
        Person obj = new Person();
        obj.Name = "Ahmed";

        ChangeName(obj);
        Console.WriteLine(obj.Name);
    }
}
```

### 🔍 التفسير:

* يتم إرسال نسخة من العنوان
* لكن النسخة تشير لنفس الكائن
* تعديل الخصائص يؤثر على الكائن الأصلي

✅ الناتج:

```
Ali
```

---

## ✳️ Passing Reference Type By Reference

```csharp
static void ChangeObject(ref Person p)
{
    p = new Person();
    p.Name = "New Person";
}
```

### الفرق هنا:

* يمكن تغيير الكائن نفسه بالكامل
* وليس فقط خصائصه

---

# 3️⃣ Function تحسب الجمع والطرح

```csharp
static (int sum, int sub) Calculate(int a, int b, int c, int d)
{
    int sum = a + b;
    int sub = c - d;
    return (sum, sub);
}
```

### شرح:

* الدالة تستقبل 4 أرقام
* تحسب:

  * sum = a + b
  * sub = c - d
* ترجع القيمتين باستخدام Tuple

---

# 4️⃣ Sum of Digits

```csharp
static int SumDigits(int number)
{
    int sum = 0;

    while (number != 0)
    {
        sum += number % 10;
        number /= 10;
    }

    return sum;
}
```

### شرح:

* `% 10` → يأخذ آخر رقم
* `/= 10` → يحذف آخر رقم
* نكرر حتى يصبح الرقم صفر

مثال:
25
2 + 5 = 7

---

# 5️⃣ IsPrime Function

```csharp
static bool IsPrime(int number)
{
    if (number <= 1)
        return false;

    for (int i = 2; i <= Math.Sqrt(number); i++)
    {
        if (number % i == 0)
            return false;
    }

    return true;
}
```

### شرح:

* العدد <= 1 ليس أولي
* نفحص القسمة حتى الجذر التربيعي
* إذا لم يقبل القسمة → عدد أولي

---

# 6️⃣ MinMaxArray باستخدام ref

```csharp
static void MinMaxArray(int[] arr, ref int min, ref int max)
{
    min = arr[0];
    max = arr[0];

    foreach (int num in arr)
    {
        if (num < min)
            min = num;

        if (num > max)
            max = num;
    }
}
```

### لماذا استخدمنا ref؟

لكي نعيد قيمتين خارج الدالة.

---

# 7️⃣ Factorial (Iterative)

```csharp
static long Factorial(int n)
{
    long result = 1;

    for (int i = 1; i <= n; i++)
    {
        result *= i;
    }

    return result;
}
```

### شرح:

5! = 5 × 4 × 3 × 2 × 1
نستخدم حلقة بدلاً من recursion

---

# 8️⃣ ChangeChar Function

```csharp
static string ChangeChar(string text, int position, char newChar)
{
    char[] chars = text.ToCharArray();
    chars[position] = newChar;
    return new string(chars);
}
```

### شرح:

* لا يمكن تعديل string مباشرة (Immutable)
* نحوله إلى char array
* نعدل الحرف
* نعيد إنشاء string جديد

مثال:

```
Input:  "Hello", 1, 'a'
Output: "Hallo"
```

---

# 🎯 خلاصة مهمة

| النوع          | By Value        | By Reference           |
| -------------- | --------------- | ----------------------    |
| Value Type     | نسخة من القيمة  | نفس العنوان            |
| Reference Type | نسخة من العنوان | يمكن تغيير الكائن نفسه |