# 1️⃣ Divisible by 3 and 4

## 💻 Code

```csharp
Console.Write("Enter a number: ");
int num = int.Parse(Console.ReadLine());

if (num % 3 == 0 && num % 4 == 0)
    Console.WriteLine("Yes");
else
    Console.WriteLine("No");
```

## 🧠 شرح الكود

* `Console.Write` → بيطلب من المستخدم يدخل رقم.
* `Console.ReadLine()` → بياخد الإدخال كـ string.
* `int.Parse()` → بيحوّل الـ string لـ int.
* `num % 3` → بيحسب باقي القسمة.
* لو باقي القسمة على 3 = 0 **وكمان** باقي القسمة على 4 = 0
  يبقى الرقم يقبل القسمة عليهم معًا.
* `&&` → معناها لازم الشرطين يتحققوا.

---

# 2️⃣ Positive or Negative

```csharp
Console.Write("Enter a number: ");
int num = int.Parse(Console.ReadLine());

if (num < 0)
    Console.WriteLine("negative");
else
    Console.WriteLine("positive");
```

## 🧠 الشرح

* لو الرقم أقل من صفر → سالب.
* أي رقم مش أقل من صفر → موجب (بما فيهم الصفر).

---

# 3️⃣ Max and Min of 3 Numbers

```csharp
Console.Write("Enter three numbers: ");
string[] input = Console.ReadLine().Split(' ');

int a = int.Parse(input[0]);
int b = int.Parse(input[1]);
int c = int.Parse(input[2]);

int max = Math.Max(a, Math.Max(b, c));
int min = Math.Min(a, Math.Min(b, c));

Console.WriteLine($"Max element = {max}");
Console.WriteLine($"Min element = {min}");
```

## 🧠 الشرح

* `Split(' ')` → بيقسم الإدخال عند المسافات ويرجعه Array.
* `input[0]` → أول رقم.
* `Math.Max(a, b)` → بيرجع الأكبر.
* استخدمنا `Math.Max` متداخلة علشان نقارن 3 أرقام.
* نفس الفكرة في `Math.Min`.

---

# 4️⃣ Even or Odd

```csharp
int num = int.Parse(Console.ReadLine());

if (num % 2 == 0)
    Console.WriteLine("Even");
else
    Console.WriteLine("Odd");
```

## 🧠 الشرح

* أي رقم زوجي باقي قسمته على 2 = صفر.
* لو مش صفر → فردي.

---

# 5️⃣ Vowel or Consonant

```csharp
char ch = char.ToLower(Console.ReadLine()[0]);

if ("aeiou".Contains(ch))
    Console.WriteLine("vowel");
else
    Console.WriteLine("consonant");
```

## 🧠 الشرح

* `[0]` → بناخد أول حرف بس.
* `ToLower()` → نحول الحرف لصغير علشان المقارنة تبقى سهلة.
* `"aeiou".Contains(ch)` → يشوف الحرف موجود في الحروف الصوتية ولا لأ.

---

# 6️⃣ Print 1 to N

```csharp
int n = int.Parse(Console.ReadLine());

for (int i = 1; i <= n; i++)
    Console.Write(i + " ");
```

## 🧠 الشرح

* `for` loop بتبدأ من 1.
* تستمر لحد ما توصل لـ n.
* `i++` → تزود 1 كل مرة.

---

# 7️⃣ Multiplication Table

```csharp
int n = int.Parse(Console.ReadLine());

for (int i = 1; i <= 12; i++)
    Console.Write(n * i + " ");
```

## 🧠 الشرح

* بنلف 12 مرة.
* كل مرة نضرب الرقم في i.
* نطبع الناتج.

---

# 8️⃣ Even Numbers from 1 to N

```csharp
int n = int.Parse(Console.ReadLine());

for (int i = 1; i <= n; i++)
{
    if (i % 2 == 0)
        Console.Write(i + " ");
}
```

## 🧠 الشرح

* بنعدي على كل رقم من 1 لـ n.
* لو الرقم زوجي نطبعه.
* استخدمنا شرط داخل اللوب.

---

# 9️⃣ Power Without Math.Pow

```csharp
string[] input = Console.ReadLine().Split(' ');

int baseNum = int.Parse(input[0]);
int power = int.Parse(input[1]);

int result = 1;

for (int i = 0; i < power; i++)
    result *= baseNum;

Console.WriteLine(result);
```

## 🧠 الشرح

* بدأنا `result = 1` لأن أي رقم ×1 = نفسه.
* لفينا عدد مرات يساوي قيمة القوة.
* كل مرة نضرب الناتج في الرقم الأساسي.

مثال: 4^3
1×4 = 4
4×4 = 16
16×4 = 64

---

# 🔟 Marks Total, Average, Percentage

```csharp
string[] marks = Console.ReadLine().Split(' ');

int total = 0;

for (int i = 0; i < 5; i++)
    total += int.Parse(marks[i]);

double average = total / 5.0;
double percentage = average;

Console.WriteLine($"Total marks = {total}");
Console.WriteLine($"Average Marks = {average}");
Console.WriteLine($"Percentage = {percentage}");
```

## 🧠 الشرح

* جمعنا الدرجات في متغير total.
* قسمنا على 5.0 علشان النتيجة تطلع double مش int.
* النسبة = المتوسط لأن كل مادة من 100.

---

# 1️⃣1️⃣ Days in Month

```csharp
int month = int.Parse(Console.ReadLine());

int days = month switch
{
    2 => 28,
    4 or 6 or 9 or 11 => 30,
    _ => 31
};

Console.WriteLine(days);
```

## 🧠 الشرح

* استخدمنا `switch expression`.
* لو شهر 2 → 28 يوم.
* الشهور 4 و6 و9 و11 → 30.
* غير كده → 31.

---

# 1️⃣5️⃣ Prime Numbers in Range (مهم جدًا)

```csharp
for (int i = start; i <= end; i++)
{
    if (i < 2) continue;

    bool isPrime = true;

    for (int j = 2; j <= Math.Sqrt(i); j++)
    {
        if (i % j == 0)
        {
            isPrime = false;
            break;
        }
    }

    if (isPrime)
        Console.Write(i + " ");
}
```

## 🧠 شرح 

* أي رقم أقل من 2 مش أولي.
* نفترض إن الرقم أولي (`isPrime = true`).
* نجرب نقسمه على الأرقام من 2 لحد الجذر التربيعي.
* لو اتقسم بدون باقي → مش أولي.
* `break` → نخرج بدري علشان نوفر وقت.
* لو فضل `isPrime = true` نطبعه.

⚡ استخدام `Math.Sqrt(i)` بيقلل عدد العمليات بدل ما نلف لحد i.

---

# 1️⃣6️⃣ Decimal to Binary

```csharp
string binary = "";

while (num > 0)
{
    binary = (num % 2) + binary;
    num /= 2;
}
```

## 🧠 الشرح

* بنقسم الرقم على 2.
* ناخد باقي القسمة.
* نضيفه في أول النص.
* نكرر لحد ما الرقم يبقى صفر.

---

# 1️⃣9️⃣ Identity Matrix

```csharp
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        if (i == j)
            Console.Write("1 ");
        else
            Console.Write("0 ");
    }
    Console.WriteLine();
}
```

## 🧠 الشرح

* استخدمنا Nested Loop.
* الصف = i
* العمود = j
* لو الصف = العمود → نحط 1
* غير كده → 0

---

# 2️⃣5️⃣ Longest Distance Between Equal Cells (مهمة جدًا)

```csharp
for (int i = 0; i < arr.Length; i++)
{
    for (int j = arr.Length - 1; j > i; j--)
    {
        if (arr[i] == arr[j])
        {
            int distance = j - i - 1;
            if (distance > maxDistance)
                maxDistance = distance;
            break;
        }
    }
}
```

## 🧠 شرح 

* اللوب الأول يمشي على كل عنصر.
* اللوب التاني يبدأ من الآخر ويرجع لحد i.
* أول ما يلاقي نفس القيمة:

  * يحسب المسافة = الفرق بينهم - 1
  * يقارنها بأكبر مسافة.
  * يعمل break علشان يجيب أبعد نقطة مباشرة.
