
---

````markdown
# 🎯 أساسيات C# بطريقة

> هذا الدليل مخصص لفهم أساسيات C# عمليًا، مش مجرد حفظ دوال. ستتعلم كيف تستخدم كل ميزة في الكود بطريقة صحيحة وفعالة.

---

## 🔰 قبل ما نبدأ - ما الذي ستتعلمه؟

ستتعلم في هذا الملف **10 مواضيع أساسية**:

1. تنسيق النصوص (Strings) بثلاث طرق مختلفة.
2. الفرق بين `String` و `StringBuilder` وأفضل استخدام لكل منهما.
3. أهم دوال `String` التي ستستخدمها يوميًا.
4. دوال `StringBuilder` للنصوص الديناميكية.
5. التحكم في مسار البرنامج باستخدام الجمل الشرطية.
6. الحلقات التكرارية (`for`, `while`, `do-while`, `foreach`).
7. التعامل مع المصفوفات (Arrays) أحادية البعد.
8. الفرق بين النسخ السطحي والعميق (Shallow vs Deep Copy).
9. المصفوفات ثنائية الأبعاد (2D Arrays).
10. دوال المصفوفات الجاهزة وأهمها + LINQ.

---

## 📝 1️⃣ تنسيق النصوص - 3 طرق

### الطريقة القديمة (Concatenation)
```csharp
int x = 4, y = 2;
Console.WriteLine("Equation : " + x + " + " + y + " = " + (x + y));
````

> ⚠️ **ملاحظة**: هذه الطريقة ثقيلة للقراءة والأداء لو النص طويل.

---

### الطريقة الأفضل (Composite Formatting)

```csharp
Console.WriteLine("Equation : {0} + {1} = {2}", x, y, x + y);
```

> ✅ `{0}`, `{1}`, `{2}` تمثل المتغيرات بالترتيب.

---

### الطريقة الحديثة (String Interpolation) - الأحسن

```csharp
Console.WriteLine($"Equation : {x} + {y} = {x + y}");
```

> ✅ **مميزات**:
>
> * أسهل للقراءة والفهم.
> * تدمج المتغيرات داخل النص مباشرة.
> * ممتازة للنصوص المعقدة أو المزدوجة اللغات.

---

## 🧠 2️⃣ String vs StringBuilder

### أولا: String - النص العادي

```csharp
string name = "Ibrahim";
Console.WriteLine(name.GetHashCode()); // مثال: 12345

name = "Ahmed";
Console.WriteLine(name.GetHashCode()); // مثال: 67890
```

> 💡 **الخلاصة**:
>
> * كل تعديل ينشئ نسخة جديدة في الذاكرة.
> * الأداء سيقل عند تعديل النصوص الكثيرة.

---

### ثانياً: StringBuilder - النص الذكي

```csharp
StringBuilder message = new StringBuilder("Hello, Ibrahim");
Console.WriteLine(message.GetHashCode()); // نفس الرقم

message.Append(", Ahmed");
Console.WriteLine(message.GetHashCode()); // لا يتغير
```

> 💡 **الخلاصة**:
>
> * يتم التعديل على نفس الموقع في الذاكرة.
> * أفضل للأداء عند التكرار أو بناء نصوص كبيرة (HTML, JSON, Reports).

---

## 🔧 3️⃣ أهم دوال String اليومية

### دوال التنظيف والتنسيق

```csharp
string name = "  Ibrahim  ";
Console.WriteLine(name.Trim());    // "Ibrahim"
Console.WriteLine(name.ToUpper()); // "  IBRAHIM  "
Console.WriteLine(name.ToLower()); // "  ibrahim  "
```

### دوال البحث

```csharp
string sentence = "Ibrahim is a software developer";
Console.WriteLine(sentence.Contains("Ibrahim"));  // True
Console.WriteLine(sentence.IndexOf("software"));  // 11
string text = "Hello, World! Hello!";
Console.WriteLine(text.LastIndexOf("Hello"));     // 13
```

### دوال التقطيع والدمج

```csharp
string text = "Hello, World!";
Console.WriteLine(text.Substring(7));        // "World!"
Console.WriteLine(text.Substring(0, 5));     // "Hello"
Console.WriteLine(text.Replace("World", "C#")); // "Hello, C#!"
string data = "A, B, C";
string[] arr = data.Split(", ");  // ["A", "B", "C"]
```

### دوال التحقق من النصوص

```csharp
string empty = "";
string space = "   ";
Console.WriteLine(string.IsNullOrEmpty(empty));      // True
Console.WriteLine(string.IsNullOrEmpty(space));      // False
Console.WriteLine(string.IsNullOrWhiteSpace(space)); // True
```

---

## 🏗️ 4️⃣ دوال StringBuilder للنصوص الديناميكية

```csharp
StringBuilder sb = new StringBuilder("Hello");

// إضافة
sb.Append(" Ibrahim");
sb.AppendLine(" is a developer");

// إدراج في أي مكان
sb.Insert(0, "Welcome! ");

// حذف واستبدال
sb.Remove(0, 9);
sb.Replace("Ibrahim", "Shafiq");

// تحويل إلى String عادي
string result = sb.ToString();
```

---

## 🚦 5️⃣ الجمل الشرطية

### If

```csharp
Console.Write("Enter month (1-3): ");
if (int.TryParse(Console.ReadLine(), out int month))
{
    if (month == 1) Console.WriteLine("Jan");
    else if (month == 2) Console.WriteLine("Feb");
    else if (month == 3) Console.WriteLine("Mar");
    else Console.WriteLine("Out of range");
}
else Console.WriteLine("Please enter a valid number!");
```

### Switch

```csharp
switch (month)
{
    case 1: Console.WriteLine("Jan"); break;
    case 2: Console.WriteLine("Feb"); break;
    case 3: Console.WriteLine("Mar"); break;
    default: Console.WriteLine("Invalid"); break;
}
```

### Switch with conditions

```csharp
int age = 25;
switch (age)
{
    case > 20 and <= 40: Console.WriteLine("Adult"); break;
    case <= 20 and >= 10: Console.WriteLine("Teenager"); break;
    default: Console.WriteLine("Other"); break;
}
```

### Goto (استخدم بحذر)

```csharp
int budget = 3000;
switch (budget)
{
    case 3000: Console.WriteLine("Option 03"); goto case 2000;
    case 2000: Console.WriteLine("Option 02"); goto case 1000;
    case 1000: Console.WriteLine("Option 01"); break;
    default: Console.WriteLine("Invalid"); break;
}
```

---

## 🔄 6️⃣ الحلقات التكرارية

| الحلقة     | متى تستخدم                       | مثال                                                    |
| ---------- | -------------------------------- | ------------------------------------------------------- |
| `for`      | تعرف عدد التكرارات               | `for(int i=1;i<=10;i++){Console.WriteLine(i);}`         |
| `while`    | عدد غير معروف، الشرط قبل التنفيذ | `while(j<=10){Console.WriteLine(j); j++;}`              |
| `do-while` | نفذ مرة واحدة على الأقل          | `do{Console.WriteLine(k); k++;} while(k<=10);`          |
| `foreach`  | للمصفوفات والمجموعات             | `foreach(char ch in "Ibrahim"){Console.WriteLine(ch);}` |

---

## 📊 7️⃣ المصفوفات (Arrays)

```csharp
int[] numbers1 = {1,2,3,4,5};
int[] numbers2 = new int[3] {10,20,30};
```

### معلومات مهمة

```csharp
Console.WriteLine(numbers.Length);           // عدد العناصر
Console.WriteLine(numbers.Rank);             // عدد الأبعاد
Console.WriteLine(numbers[0]);               // أول عنصر
Console.WriteLine(numbers[numbers.Length-1]); // آخر عنصر
```

---

## 🧬 8️⃣ Shallow vs Deep Copy

### Shallow Copy

```csharp
int[] Arr01 = {1,2,3};
int[] Arr02 = Arr01;
Arr01[0] = 100;
Console.WriteLine(Arr02[0]); // 100
```

### Deep Copy

```csharp
int[] Arr02 = (int[])Arr01.Clone();
Arr01[0] = 100;
Console.WriteLine(Arr02[0]); // 1
```

---

## 🗂️ 9️⃣ المصفوفات ثنائية الأبعاد

```csharp
int[,] Marks = new int[3,5];
for(int s=0;s<Marks.GetLength(0);s++){
    for(int c=0;c<Marks.GetLength(1);c++){
        Marks[s,c] = int.Parse(Console.ReadLine());
    }
}
```

### عرض البيانات

```csharp
for(int s=0;s<Marks.GetLength(0);s++){
    for(int c=0;c<Marks.GetLength(1);c++){
        Console.Write(Marks[s,c]+"\t");
    }
    Console.WriteLine();
}
```

### خدعة حلقة واحدة

```csharp
int[,] matrix = {{1,2},{3,4},{5,6}};
for(int i=0;i<matrix.Length;i++){
    int row = i / matrix.GetLength(1);
    int col = i % matrix.GetLength(1);
    Console.WriteLine($"matrix[{row},{col}] = {matrix[row,col]}");
}
```

---

## 🛠️ 10️⃣ دوال المصفوفات الجاهزة

* **الترتيب والقلب**

```csharp
Array.Sort(arr);
Array.Reverse(arr);
```

* **البحث**

```csharp
Array.IndexOf(arr, 20);
Array.LastIndexOf(arr, 20);
Array.BinarySearch(arr, 30);
```

* **النسخ**

```csharp
Array.Copy(source,dest,3);
int[] copy = (int[])source.Clone();
```

* **البحث الشرطي**

```csharp
Array.Exists(numbers, x => x%2==0);
Array.Find(numbers, x => x>6);
Array.FindAll(numbers, x => x>6);
Array.FindIndex(numbers, x => x==3);
```

* **التعديل**

```csharp
Array.Resize(ref arr,5);
Array.Clear(arr,0,arr.Length);
```

* **التطبيق على كل عنصر**

```csharp
Array.ForEach(arr,x=>Console.WriteLine(x*2));
```

* **خصائص المصفوفات**

```csharp
marks.Length;
marks.Rank;
marks.GetLength(0);
marks.GetLength(1);
marks.GetUpperBound(0);
```

* **LINQ**

```csharp
using System.Linq;
arr.Contains(3);
arr.Sum();
arr.Average();
arr.Max();
arr.Min();
arr.Count(x => x>3);
```