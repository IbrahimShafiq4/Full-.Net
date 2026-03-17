# شرح الكود بالكامل (مرجع شامل) - Delegates, Generics, Sorting Algorithms في C#

**النسخة:** Advanced C# Concepts  

---

## الفهرس
1. [Delegates (المؤشرات للدوال)](#1-delegates-المؤشرات-للدوال)  
   1.1 [إيه هو الـ Delegate؟](#11-إيه-هو-الـ-delegate)  
   1.2 [ليه بنستخدمه؟](#12-ليه-بنستخدمه)  
   1.3 [إزاي بنعرف delegate؟](#13-إزاي-بنعرف-delegate)  
   1.4 [الـ Delegate Internal (بشكل مبسط)](#14-الـ-delegate-internal-بشكل-مبسط)  
   1.5 [مثال من الكود](#15-مثال-من-الكود)  
   1.6 [Multicast Delegate](#16-multicast-delegate)  
2. [Built-in Delegates (الجاهزة)](#2-built-in-delegates-الجاهزة)  
   2.1 [Predicate&lt;T&gt;](#21-predicatet)  
   2.2 [Func&lt;T1, T2, ..., TResult&gt;](#22-funct1-t2--tresult)  
   2.3 [Action&lt;T&gt;](#23-actiont)  
   2.4 [مقارنة بينهم](#24-مقارنة-بينهم)  
3. [Generics (البرمجة العامة)](#3-generics-البرمجة-العامة)  
   3.1 [يعني إيه Generics؟](#31-يعني-إيه-generics)  
   3.2 [ليه مهمة؟](#32-ليه-مهمة)  
   3.3 [مشاكل بدون Generics](#33-مشاكل-بدون-generics)  
   3.4 [Generics في الكود](#34-generics-في-الكود)  
4. [Generic Delegate مع in و out](#4-generic-delegate-مع-in-و-out)  
5. [Sorting Algorithms (Bubble Sort)](#5-sorting-algorithms-bubble-sort)  
   5.1 [شرح Bubble Sort](#51-شرح-bubble-sort)  
   5.2 [المشكلة في الكود الأول (Asc و Desc)](#52-المشكلة-في-الكود-الأول-asc-و-desc)  
   5.3 [الحل باستخدام Delegate](#53-الحل-باستخدام-delegate)  
   5.4 [الحل باستخدام Generics + Built-in Delegate (Func)](#54-الحل-باستخدام-generics--built-in-delegate-func)  
6. [Lambda Expressions](#6-lambda-expressions)  
   6.1 [إيه هو Lambda Expression؟](#61-إيه-هو-lambda-expression)  
   6.2 [علاقته بالـ Delegate](#62-علاقته-بالـ-delegate)  
   6.3 [استخدامات في الكود](#63-استخدامات-في-الكود)  
7. [Anonymous Methods (الطرق المجهولة)](#7-anonymous-methods-الطرق-المجهولة)  
   7.1 [يعني إيه؟](#71-يعني-إيه)  
   7.2 [الفرق بينها وبين Lambda](#72-الفرق-بينها-وبين-lambda)  
   7.3 [أمثلة إضافية](#73-أمثلة-إضافية)  
8. [List Built-in Methods](#8-list-built-in-methods)  
   8.1 [ForEach](#81-foreach)  
   8.2 [All](#82-all)  
   8.3 [Any](#83-any)  
   8.4 [Count](#84-count)  
   8.5 [Where (Linq)](#85-where-linq)  
   8.6 [Select (Linq)](#86-select-linq)  
   8.7 [استخدامات في الكود](#87-استخدامات-في-الكود)  
9. [Enumerable.Range](#9-enumerablerange)  
   9.1 [إيه هي؟](#91-إيه-هي)  
   9.2 [ليه بنستخدمها؟](#92-ليه-بنستخدمها)  
10. [شرح كل Method في الكود](#10-شرح-كل-method-في-الكود)  
    10.1 [GetOddNumbers و GetEvenNumbers](#101-getoddnumbers-و-getevennumbers)  
    10.2 [GetFilteredNumbers](#102-getfilterednumbers)  
    10.3 [GetFilteredElementsBasedOnCondition](#103-getfilteredelementsbasedoncondition)  
    10.4 [GetSpecifedSequnceNumbers](#104-getspecifedsequncenumbers)  
    10.5 [StringMethods.GetCountOfUpperChars و GetCountOfLowerChars](#105-stringmethodsgetcountofupperchars-و-getcountoflowerchars)  
    10.6 [ConditionMethods.CheckOdd و CheckEven (مع تصحيح الخطأ)](#106-conditionmethodscheckodd-و-checkeven-مع-تصحيح-الخطأ)  
    10.7 [SortingAlgorithm Classes](#107-sortingalgorithm-classes)  
    10.8 [GenericSortingAlgo&lt;T&gt;](#108-genericsortingalgot)  
    10.9 [CompareGenericMethods&lt;T&gt;](#109-comparegenericmethodst)  
11. [Debug & Mistakes (الأخطاء وتصحيحها)](#11-debug--mistakes-الأخطاء-وتصحيحها)  
    11.1 [خطأ في ConditionMethods](#111-خطأ-في-conditionmethods)  
    11.2 [خطأ في GenericSortingAlgo.BubbleSort](#112-خطأ-في-genericsortingalgobubblesort)  
    11.3 [خطأ في CompareGenericMethods.CompareLength](#113-خطأ-في-comparegenericmethodscomparelength)  
12. [أمثلة كاملة للتشغيل (بعد التصحيح)](#12-أمثلة-كاملة-للتشغيل-بعد-التصحيح)  
13. [الخاتمة](#13-الخاتمة)  

---

## 1. Delegates (المؤشرات للدوال)

### 1.1 إيه هو الـ Delegate؟
ببساطة، الـ **Delegate** هو **نوع (type)** بيشتغل كمرجع (reference) لدالة (method). زي ما يكون عندك متغير من نوع int بيخزن رقم، الـ delegate بيخزن اسم دالة عشان تقدر تستدعيها من خلاله.

**مثال حياتي:**  
تخيل إن معاك **ريموت كنترول**، كل زرار في الريموت هو دالة معينة (زي تشغيل التكييف، إطفاء النور). الريموت نفسه هو الـ delegate اللي بيساعدك تشير للدالة اللي عايز تنفذها. لو ضغطت على زرار معين، الريموت بيشاور على الدالة المناسبة وينفذها.

### 1.2 ليه بنستخدمه؟
- **مرونة (Flexibility):** تقدر تمرر الدالة كـ parameter لدالة تانية. ده بيخلي الكود reusable أكتر.
- **الأحداث (Events):** الـ Events في C# مبنية على الـ Delegates. مثلاً لما تعمل Subscribe لقناة على YouTube، القناة لما بتنزل فيديو جديد بتستدعي delegate معين عشان تنبهك.
- **Multicast:** تقدر تخزن أكتر من دالة في نفس الـ delegate وتنفذهم كلهم مرة واحدة.

### 1.3 إزاي بنعرف delegate؟
الـ Syntax بسيط:
```csharp
[Access Modifier] delegate [Return Type] [Delegate Name] ([Parameters]);
```

مثال من الكود:
```csharp
public delegate int StringMethodDelegate(string word);
```
معناها: أي دالة بتاخد `string` وترجع `int` تقدر تشاور عليها باستخدام `StringMethodDelegate`.

### 1.4 الـ Delegate Internal (بشكل مبسط)
- الـ Compiler بيحول الـ delegate لـ **كلاس (class)** عادي بيشتق من `System.MulticastDelegate`.
- جواه خاصية اسمها **Invocation List**، وهي عبارة عن قائمة بالدوال اللي بيشاور عليها.
- لما تستخدم `+=` بتضيف دالة للقائمة.
- لما تستخدم `-=` بتشيل دالة.
- لما تستدعي الـ delegate (باستخدام `Invoke` أو زي ما تستدعي دالة عادية) بيمر على كل الدوال في القائمة بالترتيب وينفذهم.

### 1.5 مثال من الكود
في الكود عندنا كلاس `StringMethods` فيه دوال:
```csharp
public class StringMethods
{
    public static int GetCountOfUpperChars(string word) { ... }
    public static int GetCountOfLowerChars(string word) { ... }
    public static void Print(string word) { ... }
}
```

وفي `Main` (مع إنه معلق) كان فيه شرح للخطوات:
1. **تعريف reference من الـ delegate:**
   ```csharp
   StringMethodDelegate stringMethodDelegate;
   ```
2. **Initialization (إسناد دالة له):**
   ```csharp
   stringMethodDelegate = new StringMethodDelegate(StringMethods.GetCountOfUpperChars);
   // أو اختصارًا:
   stringMethodDelegate = StringMethods.GetCountOfUpperChars;
   ```
3. **إضافة دالة تانية (Multicast):**
   ```csharp
   stringMethodDelegate += StringMethods.GetCountOfLowerChars;
   ```
4. **الاستدعاء:**
   ```csharp
   int result = stringMethodDelegate.Invoke("Ibrahim");
   // أو
   int result = stringMethodDelegate("Ibrahim");
   ```
   - هنا الـ `Invoke` هيشغل الدالة الأولى `GetCountOfUpperChars` على الكلمة "Ibrahim" (ترجع عدد الحروف الكبيرة)، وبعدين الدالة التانية `GetCountOfLowerChars` (ترجع عدد الحروف الصغيرة)، لكن القيمة اللي هتترجع في `result` هي **آخر قيمة** (من التانية) لأن الـ return type مش void.

### 1.6 Multicast Delegate
- لو الـ delegate return type غير void، آخر دالة بس هي اللي بترجع قيمتها.
- عشان كده لو عايز تشغل أكتر من دالة وتجمع نتايجهم، الأفضل يكون الـ return type **void** وتستخدم `ref` parameter أو حقل خارجي.

**مثال Multicast مع void:**
```csharp
Action<string> actions = StringMethods.Print;
actions += Console.WriteLine; // Console.WriteLine برضه بتاخد string وترجع void
actions("Hello"); // هتطبع Hello مرتين (مرة من Print ومرة من WriteLine)
```

---

## 2. Built-in Delegates (الجاهزة)

عشان تسهل علينا الحياة، #C فيها 3 أنواع أساسية من الـ Delegates الجاهزة (built-in). دول موجودين في namespace `System`.

### 2.1 Predicate&lt;T&gt;
- **التعريف:** بيشاور على دالة بترجع `bool` وبتاخد باراميتر واحد من نوع `T`.
- **الاستخدام:** الفلترة (Filtering)، يعني بتحدد إذا كان العنصر محقق شرط معين ولا لأ.
- **المثال في الكود:**
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
  - هنا `condition` هي Predicate بتاخد عنصر وترجع `true` لو محقق الشرط.
  - استخدام:
    ```csharp
    Predicate<int> isOdd = ConditionMethods.CheckOdd; // بعد التصحيح
    List<int> oddNumbers = GetFilteredElementsBasedOnCondition(numbers, isOdd);
    ```

### 2.2 Func&lt;T1, T2, ..., TResult&gt;
- **التعريف:** بيشاور على دالة بترجع قيمة (TResult) وبتاخد من 0 لـ 16 باراميتر.
- **الصيغة:** `Func<T1, T2, ..., TResult>` (آخر نوع دائمًا هو نوع الرجوع).
- **أمثلة:**
  - `Func<int, bool>`: دالة بتاخد `int` وترجع `bool`.
  - `Func<string, string, int>`: دالة بتاخد اثنين `string` وترجع `int`.
- **المثال في الكود:**
  ```csharp
  public static List<T> GetSpecifedSequnceNumbers<T>(List<T> numbers, Func<T, bool> condition)
  {
      List<T> result = new List<T>();
      for (int i = 0; i < numbers?.Count; i++)
          if (condition(numbers[i]))
              result.Add(numbers[i]);
      return result;
  }
  ```
  - هنا `Func<T, bool>` نفس وظيفة Predicate (ممكن نستخدم أيًا منهم).
  - استخدام:
    ```csharp
    List<int> result = GetSpecifedSequnceNumbers(numbers, x => x % 2 == 0);
    ```

- **كمان في BubbleSort:**
  ```csharp
  public static void BubbleSort(T[] Arr, Func<T, T, bool> condition)
  ```
  - `Func<T, T, bool>`: دالة بتاخد اثنين `T` وترجع `bool` (ده شرط المقارنة).

### 2.3 Action&lt;T&gt;
- **التعريف:** بيشاور على دالة **void** (مش بترجع حاجة) وبتاخد من 0 لـ 16 باراميتر.
- **الاستخدام:** تنفيذ إجراء (side effect) على العناصر، زي الطباعة.
- **المثال في الكود:**
  ```csharp
  Action<string> action = StringMethods.Print;
  action.Invoke("Hello world");
  ```
  - `Print` هي دالة void بتاخد string.

### 2.4 مقارنة بينهم

| الـ Delegate | الـ Return | الـ Parameters | استخدامات شائعة |
|--------------|------------|----------------|------------------|
| **Predicate&lt;T&gt;** | bool       | 1 (T)          | فلترة (Where, All, Any) |
| **Func&lt;...&gt;**    | أي نوع     | 0-16           | تحويل (Select), مقارنة (Sort) |
| **Action&lt;...&gt;**  | void       | 0-16           | تنفيذ إجراء (ForEach) |

---

## 3. Generics (البرمجة العامة)

### 3.1 يعني إيه Generics؟
هي فكرة إنك تكتب **كود واحد** يشتغل مع **أي نوع بيانات**، بدون ما تعيد كتابة نفس الكود لكل نوع. بنستخدم `<T>` (أي حرف) كمكان للنوع، وبعدين لما نستخدم الكلاس أو الميثود بنحدد النوع الفعلي.

### 3.2 ليه مهمة؟
- **تقليل Duplication:** بدل ما تعمل BubbleSort للـ int، وللـ string، وللـ double، تعمل Generic واحد.
- **Type Safety:** بتمنع الأخطاء اللي ممكن تحصل مع `ArrayList` مثلاً (اللي بيخزن object ويتطلب casting).
- **أداء أفضل:** مش محتاج Boxing/Unboxing (تحويل value type لـ object).

### 3.3 مشاكل بدون Generics
في الكود الأول (قبل Generics) كان في `SortingAlgorithm` مخصص للـ int فقط. لو عايزين نرتب `string` هنضطر نعمل كلاس تاني أو نكرر الكود.

### 3.4 Generics في الكود
- **`GenericSortingAlgo<T>`:** كلاس Generic بيعرف `BubbleSort` و `Swap` لأي نوع T.
- **`CompareGenericMethods<T>`:** كلاس Generic فيه دوال مقارنة زي `CompareGreaterThan` و `CompareLessThan`. وشرط `where T : IComparable<T>` عشان نقدر نستخدم `CompareTo`.
- **`CompareGenericDelegate`:** (هنشرحه لوحده).

**مثال على استخدام Generic Sorting:**
```csharp
string[] names = { "Ibrahim", "Shafiq", "Abd-Elshafy", "Muhammad" };
Func<string, string, bool> compare = CompareGenericMethods<string>.CompareLength; // دي مش Generic للأسف (شرح الخطأ)
GenericSortingAlgo<string>.BubbleSort(names, compare);
```

---

## 4. Generic Delegate مع in و out

في الكود معرف delegate كده:
```csharp
public delegate TResult CompareGenericDelegate<in T1, in T2, out TResult>(T1 Left, T1 Right);
```
- **`in T1, in T2`:** معناها إن T1 و T2 مدخلات (contravariant)، يعني تقدر تمرر نوع أقل اشتقاقًا (أب) من المطلوب. (موضوع متقدم شوية).
- **`out TResult`:** معناها إن TResult مخرج (covariant)، يعني تقدر ترجع نوع أكثر اشتقاقًا (ابن) من المطلوب.
- **الفكرة:** إن الـ delegate ده يقدر يشتغل مع أي نوعين كمدخلات وأي نوع كمخرج. لكن في الكود بعد كده استخدموا `Func<T, T, bool>` مباشرة بدل ما يستخدموا الـ delegate ده (لأنه أسهل).

**متى نستخدم `in` و `out`؟**  
لما نكون بنعمل مكتبات عامة وبنحتاج مرونة زيادة، زي LINQ أو EventHandlers.

---

## 5. Sorting Algorithms (Bubble Sort)

### 5.1 شرح Bubble Sort
Bubble Sort هي خوارزمية ترتيب بسيطة. الفكرة:
- نقارن كل عنصرين متجاورين (j و j+1).
- لو هما مش في الترتيب المطلوب، نبدل أماكنهم.
- بنكرر ده على كل عناصر الـ array أكتر من مرة لحد ما نلاقي إن مفيش تبديلات حصلت (يبقى اترتبت).

**مثال:**  
Arr = {8, 6, 1}  
المرور الأول:  
- قارن 8 و 6 => 8 > 6 => نبدل => {6, 8, 1}  
- قارن 8 و 1 => 8 > 1 => نبدل => {6, 1, 8}  
المرور الثاني:  
- قارن 6 و 1 => 6 > 1 => نبدل => {1, 6, 8}  
- قارن 6 و 8 => 6 < 8 => مفيش تبديل  
المرور الثالث: مفيش تبديلات => خلاص.

### 5.2 المشكلة في الكود الأول (Asc و Desc)
كان في دوال منفصلة:
- `BubbleSortAsc` (if Arr[j] > Arr[j+1] => swap)
- `BubbleSortDesc` (if Arr[j] < Arr[j+1] => swap)

نفس الكود بالظبط إلا شرط المقارنة. ده **تكرار** (Duplication) مش حلو. لو عايز نضيف ترتيب حسب معيار تاني (زي الطول) هنكرر برضه.

### 5.3 الحل باستخدام Delegate
عملوا `CompareDelegate` وعدلوا `BubbleSort` عشان ياخد delegate:
```csharp
public static void BubbleSort(int[] Arr, CompareDelegate compare)
{
    for (int i = 0; i < Arr?.Length; i++)
        for (int j = 0; j < Arr?.Length - i - 1; j++)
            if (compare.Invoke(Arr[j], Arr[j+1]))
                Swap(ref Arr[j], ref Arr[j + 1]);
}
```
دلوقتي نقدر نمرر أي دالة مقارنة:
```csharp
CompareDelegate compare = CompareMethods.CompareGreaterThan;
SortingAlgorithm.BubbleSort(numbers, compare); // ترتيب تنازلي (لو Left > Right نبدل)
```
كده الكود بقى reusable ومرن.

### 5.4 الحل باستخدام Generics + Built-in Delegate (Func)
في `GenericSortingAlgo<T>` استخدموا `Func<T, T, bool>`:
```csharp
public static void BubbleSort(T[] Arr, Func<T, T, bool> condition)
{
    for (int i = 0; i < Arr?.Length; i++)
        for (int j = 0; j < Arr?.Length - i - 1; j++)
            if (condition.Invoke(Arr[j], Arr[j + 1]))
                Swap(ref Arr[j], ref Arr[j + 1]);
}
```
ده أحسن حاجة:
- Generic: يشتغل مع أي نوع.
- Built-in delegate: مش محتاجين نعرف delegate جديد.

---

## 6. Lambda Expressions

### 6.1 إيه هو Lambda Expression؟
هي **طريقة مختصرة** لكتابة دالة مجهولة (anonymous method) بدون اسم. الشكل الأساسي:
```csharp
(parameters) => expression
// أو
(parameters) => { statements; }
```
- الـ `=>` اسمها "fat arrow" وبتتقرأ "goes to".

### 6.2 علاقته بالـ Delegate
الـ Lambda بتتحول لدالة عادية خلف الكواليس، وبعدين تقدر تسندها لمتغير من نوع delegate مناسب.

**مثال:**
```csharp
Func<int, bool> isEven = x => x % 2 == 0;
```
ده بيساوي:
```csharp
bool isEven(int x) { return x % 2 == 0; }
```

### 6.3 استخدامات في الكود
- في `ForEach`:
  ```csharp
  numbers.ForEach(x => Console.WriteLine(x));
  ```
- في `Count`:
  ```csharp
  int evenCount = numbers.Count(x => x % 2 == 0);
  ```
- في `GetSpecifedSequnceNumbers`:
  ```csharp
  List<int> resultNum = GetSpecifedSequnceNumbers(SpecifiedNumbers, x => x % 50 == 0);
  ```

---

## 7. Anonymous Methods (الطرق المجهولة)

### 7.1 يعني إيه؟
في C# 2.0 ظهرت حاجة اسمها Anonymous Methods، وهي طريقة لكتابة دالة inline من غير اسم، باستخدام الكلمة `delegate`.
```csharp
Action<string> action = delegate (string s) { Console.WriteLine(s); };
```

### 7.2 الفرق بينها وبين Lambda
- Lambda أسهل في الكتابة (C# 3.0) وأقوى.
- Anonymous methods بتدعم `params` parameters (مثلاً `delegate (params int[] numbers) { }`)، لكن Lambda مش بتدعمها بشكل مباشر.
- دلوقتي كل الناس بتستخدم Lambda.

### 7.3 أمثلة إضافية
```csharp
// Anonymous Method
Func<int, int> square = delegate (int x) { return x * x; };

// Lambda (أفضل)
Func<int, int> squareLambda = x => x * x;
```

---

## 8. List Built-in Methods

الكثير من دوال `List<T>` بتعتمد على الـ Delegates. خلينا نشرحهم بالتفصيل:

### 8.1 ForEach
- **التعريف:** بتنفذ `Action<T>` على كل عنصر في الـ List.
- **التوقيع:** `void ForEach(Action<T> action)`
- **مثال:**
  ```csharp
  List<int> nums = new List<int> { 1, 2, 3 };
  nums.ForEach(x => Console.WriteLine(x * 2)); // يطبع 2, 4, 6
  ```

### 8.2 All
- **التعريف:** بتتحقق إذا كانت كل العناصر تحقق شرط معين.
- **التوقيع:** `bool All(Predicate<T> match)`
- **مثال:**
  ```csharp
  List<int> nums = new List<int> { 2, 4, 6 };
  bool allEven = nums.All(x => x % 2 == 0); // true
  ```

### 8.3 Any
- **التعريف:** بتتحقق إذا كان في عنصر واحد على الأقل بتحقق الشرط.
- **التوقيع:** `bool Any(Predicate<T> match)`
- **مثال:**
  ```csharp
  bool anyEven = nums.Any(x => x % 2 == 0); // true
  ```

### 8.4 Count
- **التعريف:** بترجع عدد العناصر اللي بتحقق الشرط (أو العدد الكلي لو مفيش شرط).
- **التوقيع:** `int Count(Predicate<T> match)`
- **مثال:**
  ```csharp
  int evenCount = nums.Count(x => x % 2 == 0); // 3
  ```

### 8.5 Where (Linq)
- **التعريف:** بترجع `IEnumerable<T>` بالعناصر اللي بتحقق الشرط.
- **التوقيع:** `IEnumerable<T> Where(Func<T, bool> predicate)`
- **مثال:**
  ```csharp
  var evens = nums.Where(x => x % 2 == 0); // evens = {2,4,6}
  ```

### 8.6 Select (Linq)
- **التعريف:** بتحول (project) كل عنصر لشيء تاني.
- **التوقيع:** `IEnumerable<TResult> Select<TSource, TResult>(Func<TSource, TResult> selector)`
- **مثال:**
  ```csharp
  var squares = nums.Select(x => x * x); // {1,4,9}
  ```

### 8.7 استخدامات في الكود
- في الكود استخدم `ForEach` و `Count` في أمثلة معلقة، وشرحناها في التعليقات.

---

## 9. Enumerable.Range

### 9.1 إيه هي؟
`Enumerable.Range` هي method في namespace `System.Linq` بتولّد أرقام متسلسلة.
```csharp
public static IEnumerable<int> Range(int start, int count);
```
- `start`: أول رقم.
- `count`: عدد الأرقام (مش الرقم الأخير).

**مثال:**
```csharp
var nums = Enumerable.Range(0, 5); // 0,1,2,3,4
```
لاحظ إن `5` مش داخل، count = 5 يعني 5 أرقام: 0 إلى 4.

### 9.2 ليه بنستخدمها؟
- عشان نولّد بيانات بسرعة للاختبار.
- عشان نستخدمها مع دوال Linq (زي Where, Select).
في الكود:
```csharp
List<int> numbers = Enumerable.Range(0, 100).ToList<int>();
```
ده هيديك ليستة من 0 لـ 99.

---

## 10. شرح كل Method في الكود

### 10.1 GetOddNumbers و GetEvenNumbers
```csharp
public static List<int> GetOddNumbers(List<int> numbers)
{
    List<int> result = new List<int>();
    for (int i = 0; i < numbers?.Count; i++)
        if (numbers[i] % 2 == 1)
            result.Add(numbers[i]);
    return result;
}
```
- بتاخد ليستة أرقام وترجع ليستة فيها الأرقام الفردية (odd).
- `numbers?.Count` بمعنى لو `numbers` == null متعمليش loop.
- الشرط `numbers[i] % 2 == 1` (بس في الكود الأصلي `CheckOdd` كان غلط، هنصلحه).

نفس الكلام لـ `GetEvenNumbers` بشرط `% 2 == 0`.

**المشكلة:** لو عايز ترشيحات تانية (زي الأرقام اللي أكبر من 10) هنكرر نفس الكود. عشان كده عملوا الدوال اللي بعدها.

### 10.2 GetFilteredNumbers
```csharp
public static List<int> GetFilteredNumbers(List<int> numbers, ConditionDelegate condition)
{
    List<int> result = new List<int>();
    for (int i = 0; i < numbers?.Count; i++)
        if (condition.Invoke(numbers[i]))
            result.Add(numbers[i]);
    return result;
}
```
- هنا بنمرر delegate `ConditionDelegate` (اللي عرفناها في الأعلى: `delegate bool ConditionDelegate(int X)`) اللي بتحدد الشرط.
- كده نقدر نستخدم نفس الكود لأي شرط (odd, even, divisible by 5, ...).

**مثال استخدام:**
```csharp
List<int> odds = GetFilteredNumbers(numbers, ConditionMethods.CheckOdd); // بعد التصحيح
```

### 10.3 GetFilteredElementsBasedOnCondition
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
- Generic: بتشتغل مع أي نوع T.
- استخدمت `Predicate<T>` (built-in) بدل delegate مخصص.
- أحسن لأنها معروفة لكل المبرمجين.

### 10.4 GetSpecifedSequnceNumbers
```csharp
public static List<T> GetSpecifedSequnceNumbers<T>(List<T> numbers, Func<T, bool> condition)
{
    List<T> result = new List<T>();
    for (int i = 0; i < numbers?.Count; i++)
        if (condition(numbers[i]))
            result.Add(numbers[i]);
    return result;
}
```
- نفس الفكرة لكن باستخدام `Func<T, bool>`.
- هنا استخدم `condition(numbers[i])` بدون `Invoke` (اختصار).

### 10.5 StringMethods.GetCountOfUpperChars و GetCountOfLowerChars
```csharp
public static int GetCountOfUpperChars(string word)
{
    int count = 0;
    for (int i = 0; i < word?.Length; i++)
        if (char.IsUpper(word[i]))
            count++;
    return count;
}
```
- بتعد على كل حرف في الكلمة.
- `char.IsUpper` بترجع true لو الحرف كبير.
- نفس الكلام لـ `GetCountOfLowerChars` مع `char.IsLower`.

### 10.6 ConditionMethods.CheckOdd و CheckEven (مع تصحيح الخطأ)
في الكود الأصلي كان مكتوب:
```csharp
public static bool CheckOdd(int X) { return X % 2 == 0; } // غلط: ده شرط الزوجي
public static bool CheckEven(int X) { return (X % 2 == 1); } // غلط: ده شرط الفردي
```
ده خطأ في التسمية. الصحيح:
```csharp
public static bool CheckOdd(int X) => X % 2 == 1;
public static bool CheckEven(int X) => X % 2 == 0;
```

### 10.7 SortingAlgorithm Classes
- **`BubbleSortAsc`** و **`BubbleSortDesc`** (مكررين).
- **`BubbleSort`** مع `CompareDelegate` (الحل).
- **`CompareMethods`**: دوال المقارنة (`CompareGreaterThan` و `CompareLessThan`).

### 10.8 GenericSortingAlgo&lt;T&gt;
- **`BubbleSort`** مع `Func<T, T, bool>`.
- **`Swap`**: بتبدل قيمتين ref.

**ملاحظة مهمة:** في الكود الأصلي كان في خطأ في `BubbleSort` (هنوضحه في الأخطاء).

### 10.9 CompareGenericMethods&lt;T&gt;
- **`CompareGreaterThan`** و **`CompareLessThan`**:
  ```csharp
  public static bool CompareGreaterThan(T Left, T Right) => Left.CompareTo(Right) > 0;
  ```
  - بيفترض إن `T` يطبق `IComparable<T>` عشان نقدر نستخدم `CompareTo`.
- **`CompareLength`**: للأسف مش Generic (بتاخد string). لو عايزينها Generic نستخدم مثلاً `IComparer<T>` أو نعمل overload.

---

## 11. Debug & Mistakes (الأخطاء وتصحيحها)

### 11.1 خطأ في ConditionMethods
- **الخطأ:** `CheckOdd` بترجع `X % 2 == 0` (زوجي) و `CheckEven` بترجع `X % 2 == 1` (فردي).
- **التأثير:** لو استخدمنا الدوال هتدينا نتايج عكسية.
- **التصحيح:**
  ```csharp
  public static bool CheckOdd(int X) => X % 2 == 1;
  public static bool CheckEven(int X) => X % 2 == 0;
  ```

### 11.2 خطأ في GenericSortingAlgo.BubbleSort
- في الكود الأصلي:
  ```csharp
  if (condition.Invoke(Arr[j], Arr[j + 1]))
      Swap(ref Arr[i], ref Arr[j + 1]); // غلط
  ```
- المفروض نبدل `Arr[j]` مع `Arr[j+1]` مش `Arr[i]` مع `j+1`.
- **التصحيح:**
  ```csharp
  if (condition.Invoke(Arr[j], Arr[j + 1]))
      Swap(ref Arr[j], ref Arr[j + 1]);
  ```

### 11.3 خطأ في CompareGenericMethods.CompareLength
- الدالة مش Generic، لكن الكلاس Generic. ده مش خطأ صريح لكنه غير متسق.
- الحل: إما نستبعد الدالة من الكلاس، أو نعمل overload يقبل `IComparable`.
- **بديل:** نستخدم `IComparer<string>` خارجيًا.

---

## 12. أمثلة كاملة للتشغيل (بعد التصحيح)

هنكتب مثال كامل في `Main` يشغل كل المفاهيم بعد التصحيح.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedC_02.SortingAlgorithms;
using AdvancedC_02.GenericSortingAlgorithm;

namespace AdvancedC_02
{
    // Delegates تعريف
    public delegate int StringMethodDelegate(string word);
    public delegate bool ConditionDelegate(int X);

    class Program
    {
        static void Main(string[] args)
        {
            // -------------------- 1. Delegate Example (String) --------------------
            Console.WriteLine("=== String Delegate Example ===");
            StringMethodDelegate stringDel = StringMethods.GetCountOfUpperChars;
            stringDel += StringMethods.GetCountOfLowerChars;
            int result = stringDel.Invoke("Ibrahim Shafiq");
            Console.WriteLine($"Last delegate result (lower count): {result}"); // 9? حسب النص

            // -------------------- 2. Built-in Delegates --------------------
            Console.WriteLine("\n=== Built-in Delegates ===");
            List<int> numbers = Enumerable.Range(1, 10).ToList(); // 1..10

            // Predicate
            Predicate<int> isOdd = ConditionMethods.CheckOdd; // بعد التصحيح
            List<int> oddNumbers = GetFilteredElementsBasedOnCondition(numbers, isOdd);
            Console.WriteLine("Odd numbers: " + string.Join(", ", oddNumbers));

            // Func
            Func<int, bool> isEven = x => x % 2 == 0;
            List<int> evenNumbers = GetSpecifedSequnceNumbers(numbers, isEven);
            Console.WriteLine("Even numbers: " + string.Join(", ", evenNumbers));

            // Action
            Action<string> print = StringMethods.Print;
            print("Hello from Action!");

            // -------------------- 3. Non-Generic Sorting with Delegate --------------------
            Console.WriteLine("\n=== Non-Generic BubbleSort with Delegate ===");
            int[] intArr = { 8, 6, 1, 4, 12, 5, 3 };
            CompareDelegate compare = CompareMethods.CompareGreaterThan; // تنازلي
            SortingAlgorithm.BubbleSort(intArr, compare);
            Console.WriteLine("Descending: " + string.Join(", ", intArr));

            compare = CompareMethods.CompareLessThan; // تصاعدي
            SortingAlgorithm.BubbleSort(intArr, compare);
            Console.WriteLine("Ascending: " + string.Join(", ", intArr));

            // -------------------- 4. Generic Sorting with Func --------------------
            Console.WriteLine("\n=== Generic BubbleSort with Func ===");
            string[] names = { "Ibrahim", "Shafiq", "Abd-Elshafy", "Muhammad" };
            // مقارنة حسب الطول (دالة مش generic)
            Func<string, string, bool> compareLength = (a, b) => a.Length > b.Length;
            GenericSortingAlgo<string>.BubbleSort(names, compareLength);
            Console.WriteLine("Sorted by length (desc): " + string.Join(", ", names));

            // مقارنة حسب الأبجدية باستخدام CompareTo
            Func<string, string, bool> compareAlphabetical = (a, b) => a.CompareTo(b) > 0;
            GenericSortingAlgo<string>.BubbleSort(names, compareAlphabetical);
            Console.WriteLine("Alphabetical descending: " + string.Join(", ", names));

            // -------------------- 5. List Built-in Methods --------------------
            Console.WriteLine("\n=== List Methods with Delegates ===");
            List<int> list = new List<int> { 0, 2, 3, 4 };
            list.ForEach(x => Console.Write(x + " "));
            Console.WriteLine();

            bool allEven = list.All(x => x % 2 == 0);
            Console.WriteLine("All even? " + allEven);

            int countEven = list.Count(x => x % 2 == 0);
            Console.WriteLine("Count of even: " + countEven);

            // -------------------- 6. Anonymous Method & Lambda --------------------
            Console.WriteLine("\n=== Anonymous & Lambda ===");
            Action<string> anon = delegate (string s) { Console.WriteLine("Anonymous: " + s); };
            anon("Test");

            Func<int, int> square = x => x * x;
            Console.WriteLine("Square of 5: " + square(5));

            Console.WriteLine("\nDone.");
        }

        // تعريف دوال الفلترة (مثل الكود)
        public static List<T> GetFilteredElementsBasedOnCondition<T>(List<T> items, Predicate<T> condition)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < items?.Count; i++)
                if (condition.Invoke(items[i]))
                    result.Add(items[i]);
            return result;
        }

        public static List<T> GetSpecifedSequnceNumbers<T>(List<T> numbers, Func<T, bool> condition)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < numbers?.Count; i++)
                if (condition(numbers[i]))
                    result.Add(numbers[i]);
            return result;
        }
    }

    // كلاس StringMethods (بدون تغيير)
    public class StringMethods
    {
        public static int GetCountOfUpperChars(string word)
        {
            int count = 0;
            for (int i = 0; i < word?.Length; i++)
                if (char.IsUpper(word[i])) count++;
            return count;
        }

        public static int GetCountOfLowerChars(string word)
        {
            int count = 0;
            for (int i = 0; i < word?.Length; i++)
                if (char.IsLower(word[i])) count++;
            return count;
        }

        public static void Print(string word) => Console.WriteLine(word);
    }

    // كلاس ConditionMethods بعد التصحيح
    public class ConditionMethods
    {
        public static bool CheckOdd(int X) => X % 2 == 1;
        public static bool CheckEven(int X) => X % 2 == 0;
    }
}
```

**Output متوقع:**
```
=== String Delegate Example ===
Last delegate result (lower count): 9
=== Built-in Delegates ===
Odd numbers: 1, 3, 5, 7, 9
Even numbers: 2, 4, 6, 8, 10
Hello from Action!
=== Non-Generic BubbleSort with Delegate ===
Descending: 12, 8, 6, 5, 4, 3, 1
Ascending: 1, 3, 4, 5, 6, 8, 12
=== Generic BubbleSort with Func ===
Sorted by length (desc): Abd-Elshafy, Muhammad, Ibrahim, Shafiq
Alphabetical descending: Shafiq, Muhammad, Ibrahim, Abd-Elshafy
=== List Methods with Delegates ===
0 2 3 4 
All even? False
Count of even: 3
=== Anonymous & Lambda ===
Anonymous: Test
Square of 5: 25

Done.
```

---

## 13. الخاتمة

في الشرح ده ركزنا على:
- **Delegates** إزاي تخزن دوال وتمررها.
- **Built-in Delegates** (Func, Action, Predicate) وأمثلة لكل واحد.
- **Generics** عشان الكود يشتغل مع أي نوع.
- **Sorting Algorithms** وتطويرها من تكرار لمرونة باستخدام Delegate.
- **Lambda و Anonymous Methods** كطرق مختصرة.
- **List Methods** اللي بتعتمد على Delegates.
- **تصحيح الأخطاء** اللي كانت في الكود الأصلي.

---