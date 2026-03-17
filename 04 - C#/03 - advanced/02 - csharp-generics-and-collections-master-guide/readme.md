## المقدمة

النهاردة هنتكلم عن حاجة مهمة جدًا في لغة #C اسمها **Delegate**. دي بتبقى حاجة زي المؤشر (pointer) للدوال (functions). يعني بتقدر تخزن فيها اسم دالة معينة وتستخدمها زي ما تكون أنت بتستدعي الدالة دي بنفسك. تخيل إنك معاك ريموت كنترول تقدر تضغط على زرار معين فتنفذ أكتر من حاجة في نفس الوقت. الـ Delegate بيعمل كده بالظبط.

الموضوع مش صعب، لكن محتاج تركيز. هنشرح بالعامية المصرية وبالتفصيل الممل، وهنطبق على الكود اللي معانا.

---

## إيه هو الـ Delegate؟

الـ Delegate هو **كلاس** بيشتغل كـ **مرجع (reference)** لدالة (method). يعني أي دالة عندها نفس التوقيع (signature) بتاع الـ Delegate (نفس عدد ونوع الباراميترات ونفس نوع الرجوع) تقدر تخلي الـ Delegate يشاور عليها.

استخداماته:
1. إنك تقدر تمرر الدالة كـ parameter لدالة تانية (ده بيخلي الكود مرن جدًا).
2. تقدر تخزن أكتر من دالة في نفس الـ Delegate وتنفذهم كلهم مرة واحدة (Multicast).
3. بيساعد في التعامل مع الأحداث (Events) زي الـ Notifications.

---

## إزاي نعرف Delegate؟

الـ Syntax بسيط:

```csharp
[Access Modifier] delegate [return type] [Delegate Name] ([parameters]);
```

مثال:
```csharp
public delegate int StringMethodDelegate(string word);
```

دي معناها إن أي دالة بتاخد `string` وترجع `int` تقدر تشاور عليها بـ `StringMethodDelegate`.

---

## مثال عملي من الكود

عندنا في الكود كلاس `StringMethods` فيه دوال بتشتغل على الـ string:

```csharp
public class StringMethods
{
    public static int GetCountOfUpperChars(string word)
    {
        int count = 0;
        for (int i = 0; i < word?.Length; i++)
            if (char.IsUpper(word[i]))
                count++;
        return count;
    }

    public static int GetCountOfLowerChars(string word)
    {
        int count = 0;
        for (int i = 0; i < word?.Length; i++)
            if (char.IsLower(word[i]))
                count++;
        return count;
    }
}
```

ودول الاتنين نفس التوقيع: بياخدوا `string` ويرجعوا `int`. يبقى أقدر استخدم `StringMethodDelegate` معاهم.

في الـ `Main` بنعمل كده:

```csharp
StringMethodDelegate stringMethodDelegate = StringMethods.GetCountOfUpperChars;
int result = stringMethodDelegate.Invoke("Ibrahim");
Console.WriteLine(result); // هتطبع عدد الحروف الكبيرة
```

لو عايز أضيف دالة تانية لنفس الـ Delegate (Multicast) بستخدم `+=`:

```csharp
stringMethodDelegate += StringMethods.GetCountOfLowerChars;
result = stringMethodDelegate.Invoke("Ibrahim");
```

هنا الـ Delegate هينفذ الدالة الأولى وبعدين التانية، لكن هيرجع قيمة آخر واحدة بس (لأن الـ return type مش void). لو عايز ترجع كل النتايج يبقى لازم الدوال تكون `void`.

---

## استخدام Delegate في Sorting Algorithms

في الكود عندنا `SortingAlgorithm` فيه BubbleSort بيتحكم في الترتيب باستخدام Delegate. الفكرة إن إحنا مش عايزين نكرر كود BubbleSort لكل حالة (تصاعدي أو تنازلي)، عشان كده بنمرر دالة المقارنة عن طريق Delegate.

الـ Delegate بتاع المقارنة:

```csharp
public delegate bool CompareDelegate(int Left, int Right);
```

ودي معناها أي دالة بتاخد رقمين وترجع bool (True لو المطلوب تحقق، False لو لأ).

في `SortingAlgorithm` عندنا `BubbleSort` بيستقبل `CompareDelegate`:

```csharp
public static void BubbleSort(int[] Arr, CompareDelegate compare)
{
    for (int i = 0; i < Arr?.Length; i++)
        for (int j = 0; j < Arr?.Length - i - 1; j++)
            if (compare.Invoke(Arr[j], Arr[j+1]))
                Swap(ref Arr[j], ref Arr[j + 1]);
}
```

دلوقتي أقدر أمرر دالة مقارنة مختلفة:

```csharp
CompareDelegate compare = CompareMethods.CompareGreaterThan;
SortingAlgorithm.BubbleSort(numbers, compare);
```

يعني لو عايز ترتيب تصاعدي هديله دالة بتقول `Left > Right`، ولو تنازلي هديله `Left < Right`. ده خلانا منغير ما نكرر الكود.

---

## Generic Delegates

عشان نخلي الكود مرن أكتر ونشتغل مع أي نوع بيانات، بنستخدم Generics. في الكود عندنا `GenericSortingAlgo<T>` و `CompareGenericDelegate`.

تعريف Generic Delegate:

```csharp
public delegate TResult CompareGenericDelegate<in T1, in T2, out TResult>(T1 Left, T1 Right);
```

الكلمات `in` و `out` دي عشان الـ covariance والـ contravariance (موضوع متقدم، بس المهم إننا بنحدد إن T1 و T2 مدخلات، و TResult مخرجات).

وفي `GenericSortingAlgo<T>` بنستخدم الـ Delegate ده:

```csharp
public static void BubbleSort(T[] Arr, Func<T, T, bool> condition)
{
    for (int i = 0; i < Arr?.Length; i++)
        for (int j = 0; j < Arr?.Length - i - 1; j++)
            if (condition.Invoke(Arr[j], Arr[j + 1]))
                Swap(ref Arr[i], ref Arr[j + 1]);
}
```

هنا استخدمنا `Func<T, T, bool>` بدل ما نعرف Delegate جديد. ده واحد من الـ built-in delegates.

---

## Built-in Delegates في C#

عشان تسهل علينا الحياة، #C فيها 3 أنواع أساسية من الـ Delegates الجاهزة:

### 1. Predicate<T>
- بيشاور على دالة بترجع `bool` وبتاخد باراميتر واحد من أي نوع T.
- استخدامه: عادة في عمليات الفلترة (Filtering).
- مثال من الكود:
```csharp
public static List<T> GetFilteredElementsBasedOnCondition<T>(List<T> numbers, Predicate<T> condition)
{
    List<T> result = new List<T>();
    for (int i = 0; i < numbers?.Count; i++)
        if (condition.Invoke(numbers[i]))
            result.Add(numbers[i]);
    return result;
}
```
استخدام:
```csharp
Predicate<int> condition = ConditionMethods.CheckOdd;
List<int> result = GetFilteredElementsBasedOnCondition(numbers, condition);
```

### 2. Func<T, TResult>
- بيشاور على دالة بترجع قيمة (مش void) وبتاخد من 0 لـ 16 باراميتر.
- آخر باراميتر في الـ Func دايمًا هو نوع الرجوع.
- أمثلة:
  - `Func<int, bool>`: دالة بتاخد int وترجع bool.
  - `Func<string, string, int>`: دالة بتاخد string و string وترجع int.
- في الكود استخدمناه في GenericSortingAlgo:
```csharp
public static void BubbleSort(T[] Arr, Func<T, T, bool> condition)
```
ودي دالة بتاخد اثنين T وترجع bool (زي ما احنا عايزين في المقارنة).

### 3. Action<T>
- بيشاور على دالة مبتجيبش حاجة (void) وبتاخد من 0 لـ 16 باراميتر.
- مثال: `Action<string>`: دالة بتاخد string ومش بترجع حاجة.
- في الكود عندنا `StringMethods.Print`:
```csharp
public static void Print(string word)
{
    Console.WriteLine(word);
}
```
استخدام:
```csharp
Action<string> action = StringMethods.Print;
action.Invoke("Hello world");
```

---

## شرح متعمق لبعض الدوال في الكود

### 1. `List<T>.ForEach`
في آخر الكود في `Main`:
```csharp
numbers.ForEach(x => Console.WriteLine(x));
```
ده method موجود في الـ `List<T>`، بياخد `Action<T>` كباراميتر. معناه: نفذ الـ Action ده على كل عنصر في اللستة.

### 2. `Enumerable.Range`
```csharp
List<int> numbers = Enumerable.Range(0, 100).ToList<int>();
```
ده method بيولّد أرقام متسلسلة من 0 إلى 99. الأول باراميتر هو بداية الرينج، والتاني هو العدد. يعني Range(0,100) هيديلك 100 رقم من 0 لـ 99.

### 3. `List<T>.Count`
```csharp
numbers.Count(x => x % 2 == 0)
```
بيحسب عدد العناصر اللي بتحقق شرط معين (هنا الأرقام الزوجية). بياخد `Predicate<T>` كمدخل.

### 4. `List<T>.All`
```csharp
bool result = numbers.All(x => x > 0);
```
بيتأكد إن كل العناصر بتحقق الشرط. لو كل العناصر أكبر من 0 يرجع true، غير كده false.

---

## شرح الكود كاملًا مع إعادة كتابته

دلوقتي نجمع كل حاجة ونكتب الكود كامل بشكل مرتب.

### Program.cs (الرئيسي)
```csharp
using AdvancedC_02.GenericSortingAlgorithm;
using AdvancedC_02.SortingAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedC_02
{
    // تعريف Delegate مخصص
    public delegate int StringMethodDelegate(string word);
    public delegate bool ConditionDelegate(int X);

    internal class Program
    {
        // دوال للفلترة بدون Delegate (مكررة)
        public static List<int> GetOddNumbers(List<int> numbers)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < numbers?.Count; i++)
                if (numbers[i] % 2 == 1)
                    result.Add(numbers[i]);
            return result;
        }

        public static List<int> GetEvenNumbers(List<int> numbers)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < numbers?.Count; i++)
                if (numbers[i] % 2 == 0)
                    result.Add(numbers[i]);
            return result;
        }

        // دوال فلترة باستخدام Delegate
        public static List<int> GetFilteredNumbers(List<int> numbers, ConditionDelegate condition)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < numbers?.Count; i++)
                if (condition.Invoke(numbers[i]))
                    result.Add(numbers[i]);
            return result;
        }

        // فلترة عامة باستخدام Predicate
        public static List<T> GetFilteredElementsBasedOnCondition<T>(List<T> items, Predicate<T> condition)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < items?.Count; i++)
                if (condition.Invoke(items[i]))
                    result.Add(items[i]);
            return result;
        }

        // فلترة عامة باستخدام Func
        public static List<T> GetSpecifedSequnceNumbers<T>(List<T> numbers, Func<T, bool> condition)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < numbers?.Count; i++)
                if (condition(numbers[i]))
                    result.Add(numbers[i]);
            return result;
        }

        static void Main(string[] args)
        {
            #region Delegate Example 01 (String Delegate)
            StringMethodDelegate stringMethodDelegate = StringMethods.GetCountOfUpperChars;
            stringMethodDelegate += StringMethods.GetCountOfLowerChars;
            int result = stringMethodDelegate.Invoke("Ibrahim Shafiq");
            Console.WriteLine($"Number of Lower Letters in Ibrahim Shafiq is = {result}");
            #endregion

            #region Delegate Example 02 (Non-Generic BubbleSort)
            int[] numbers = { 8, 6,   , 1, 4, 12, 5, 3 };
            CompareDelegate compare = CompareMethods.CompareGreaterThan;
            SortingAlgorithm.BubbleSort(numbers, compare);
            Console.WriteLine("Sorted using CompareGreaterThan (Descending):");
            foreach (var i in numbers) Console.Write(i + " ");
            Console.WriteLine();
            #endregion

            #region Delegate Example 03 (Generic BubbleSort with Func)
            string[] names = { "Ibrahim", "Shafiq", "Abd-Elshafy", "Muhammad" };
            Func<string, string, bool> compareStrings = CompareGenericMethods<string>.CompareLength;
            GenericSortingAlgo<string>.BubbleSort(names, compareStrings);
            Console.WriteLine("Sorted by name length using Func:");
            foreach (var name in names) Console.Write(name + ", ");
            Console.WriteLine();
            #endregion

            #region Example 04 (Filtering with Delegates)
            List<int> rangeNumbers = Enumerable.Range(0, 101).ToList();
            // استخدام Func في الفلترة
            List<int> filteredNumbers = GetSpecifedSequnceNumbers(rangeNumbers, x => x % 10 == 0);
            Console.WriteLine("Numbers divisible by 10:");
            filteredNumbers.ForEach(x => Console.Write(x + " "));
            Console.WriteLine();
            #endregion

            #region Built-in Delegates Demo
            // Predicate
            Predicate<int> oddPredicate = ConditionMethods.CheckOdd;
            List<int> oddNumbers = GetFilteredElementsBasedOnCondition(rangeNumbers, oddPredicate);
            Console.WriteLine($"Count of odd numbers: {oddNumbers.Count}");

            // Action
            Action<string> printAction = StringMethods.Print;
            printAction.Invoke("Hello from Action!");

            // Func with List.All
            bool allPositive = rangeNumbers.All(x => x >= 0);
            Console.WriteLine($"All numbers are non-negative: {allPositive}");

            // List.ForEach with Lambda
            Console.WriteLine("All numbers from 0 to 10:");
            Enumerable.Range(0, 11).ToList().ForEach(x => Console.Write(x + " "));
            Console.WriteLine();
            #endregion
        }
    }

    public class StringMethods
    {
        public static int GetCountOfUpperChars(string word)
        {
            int count = 0;
            for (int i = 0; i < word?.Length; i++)
                if (char.IsUpper(word[i]))
                    count++;
            return count;
        }

        public static int GetCountOfLowerChars(string word)
        {
            int count = 0;
            for (int i = 0; i < word?.Length; i++)
                if (char.IsLower(word[i]))
                    count++;
            return count;
        }

        public static void Print(string word) => Console.WriteLine(word);
    }

    public class ConditionMethods
    {
        public static bool CheckOdd(int X) => X % 2 == 1;
        public static bool CheckEven(int X) => X % 2 == 0;
    }
}
```

### SortingAlgorithms.cs (غير الجينيريك)
```csharp
namespace AdvancedC_02.SortingAlgorithms
{
    public delegate bool CompareDelegate(int Left, int Right);

    public static class SortingAlgorithm
    {
        public static void BubbleSort(int[] Arr, CompareDelegate compare)
        {
            for (int i = 0; i < Arr?.Length; i++)
                for (int j = 0; j < Arr?.Length - i - 1; j++)
                    if (compare.Invoke(Arr[j], Arr[j + 1]))
                        Swap(ref Arr[j], ref Arr[j + 1]);
        }

        private static void Swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }
    }

    public static class CompareMethods
    {
        public static bool CompareGreaterThan(int Left, int Right) => Left > Right;
        public static bool CompareLessThan(int Left, int Right) => Left < Right;
    }
}
```

### GenericSortingAlgorithm.cs (الجينيريك)
```csharp
using System;

namespace AdvancedC_02.GenericSortingAlgorithm
{
    // Generic Delegate (يمكن استخدامه بدل Func لكن استخدمنا Func مباشرة)
    public delegate TResult CompareGenericDelegate<in T1, in T2, out TResult>(T1 Left, T1 Right);

    public class GenericSortingAlgo<T>
    {
        public static void BubbleSort(T[] Arr, Func<T, T, bool> condition)
        {
            for (int i = 0; i < Arr?.Length; i++)
                for (int j = 0; j < Arr?.Length - i - 1; j++)
                    if (condition.Invoke(Arr[j], Arr[j + 1]))
                        Swap(ref Arr[j], ref Arr[j + 1]); // لاحظ تم تعديل الـ Swap هنا (كان فيه خطأ في الكود الأصلي)
        }

        private static void Swap(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }
    }

    public class CompareGenericMethods<T> where T : IComparable<T>
    {
        public static bool CompareGreaterThan(T Left, T Right) => Left.CompareTo(Right) > 0;
        public static bool CompareLessThan(T Left, T Right) => Left.CompareTo(Right) < 0;
        public static bool CompareLength(string Left, string Right) => Left?.Length > Right?.Length;
    }
}
```

---

## الخلاصة

الـ Delegates أداة قوية في C# بتخلي الكود مرن وقابل للتوسع. باستخدامها تقدر تمرر دوال كباراميترات، وتنفذ أكتر من دالة بنفس الـ reference، وتشتغل مع الأحداث. والـ built-in delegates (Predicate, Func, Action) بتوفرلك الوقت بدل ما تعرف Delegate جديد كل مرة.

النقطة الأهم: **افهم التوقيع (signature) كويس** عشان تعرف تستخدمهم صح.