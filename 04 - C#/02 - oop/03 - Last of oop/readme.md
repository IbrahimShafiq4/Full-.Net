#  (OOP) باستخدام C#

## إيه اللي هتتعلمه هنا؟

- **Operator Overloading** - ازاي تخلي العمليات الحسابية تشتغل على الكلاسات بتاعتك
- **Implicit و Explicit Operators** - تحويل الكلاسات لأنواع تانية بشكل تلقائي أو يدوي
- **Casting بين كلاسات مختلفة** - تحويل كلاس لآخر مش مربوطين ببعض
- **Static Classes و Static Constructor** - الكلاسات الثابتة وفائدتها
- **Stateful vs Stateless** - الفرق بين الكلاسات اللي بتخزن حالة واللي مش بتخزن
- **Abstract Classes و Inheritance و Polymorphism** - أساسيات الوراثة والشكل التجريدي
- **Built-in Methods** - زي ToString, Equals, GetHashCode, GetType
- **Built-in Interfaces** - زي ICloneable و IComparable

هبدأ معاك من الصفر وبعد كل مثال هشرح السطر بسطر عشان تفهم كل حاجة.

---

## 1. Operator Overloading - تحميل العوامل الحسابية

في المثال ده هانعمل كلاس اسمه `Fraction` (كسر) وهانخليه يقدر يتجمع مع كسر تاني باستعمال علامة `+`. كمان هانحمّل علامات تانية زي `<` و `>`.

### البرنامج الكامل

```csharp
using System;

namespace OperatorOverloadingExample
{
    class Fraction
    {
        public int Numerator { get; set; }   // البسط
        public int Denominator { get; set; } // المقام

        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        // تحميل علامة + لجمع كسرين
        public static Fraction operator +(Fraction a, Fraction b)
        {
            int newNumerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
            int newDenominator = a.Denominator * b.Denominator;
            return new Fraction(newNumerator, newDenominator);
        }

        // تحميل علامة - لطرح كسرين
        public static Fraction operator -(Fraction a, Fraction b)
        {
            int newNumerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
            int newDenominator = a.Denominator * b.Denominator;
            return new Fraction(newNumerator, newDenominator);
        }

        // تحميل علامة < للمقارنة حسب القيمة العشرية
        public static bool operator <(Fraction a, Fraction b)
        {
            return (double)a.Numerator / a.Denominator < (double)b.Numerator / b.Denominator;
        }

        public static bool operator >(Fraction a, Fraction b)
        {
            return (double)a.Numerator / a.Denominator > (double)b.Numerator / b.Denominator;
        }

        // لازم نعمل ToString عشان نطبع الكسر بشكل مرتب
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Fraction f1 = new Fraction(1, 2); // 1/2
            Fraction f2 = new Fraction(1, 3); // 1/3

            Fraction sum = f1 + f2;           // 1/2 + 1/3 = 5/6
            Fraction diff = f1 - f2;          // 1/2 - 1/3 = 1/6

            Console.WriteLine($"{f1} + {f2} = {sum}");
            Console.WriteLine($"{f1} - {f2} = {diff}");

            if (f1 > f2)
                Console.WriteLine($"{f1} أكبر من {f2}");
            else
                Console.WriteLine($"{f1} أصغر من {f2}");
        }
    }
}
```

### شرح الكود

1. **class Fraction**  
   - عندنا خاصيتين: `Numerator` (البسط) و `Denominator` (المقام).  
   - الكونستراكتور بياخد البسط والمقام ويحطهم في الخواص.

2. **operator +**  
   - دي فنكشن static بترجع `Fraction` وبتاخد اثنين `Fraction`.  
   - بنستخدمها عشان نقدر نكتب `f1 + f2`.  
   - القانون الرياضي: `(a/b) + (c/d) = (a*d + c*b) / (b*d)`.  
   - بنحسب البسط الجديد والمقام الجديد ونرجع كسر جديد.

3. **operator -**  
   - نفس الفكرة لكن بالطرح.

4. **operator < و >**  
   - عشان نقارن بين كسرين بناءً على قيمتهم العشرية.  
   - بنحول الكسر لـ double عن طريق قسمة البسط على المقام.  
   - لو عايز تستخدم `<=` أو `>=` لازم تعملهم برضه (هما مرتبطين).

5. **ToString()**  
   - بنoverride عشان لو طبعنا الكسر يظهر بشكل `"بسط/مقام"` مش اسم الكلاس.

6. **Main**  
   - بنعمل كسرين `1/2` و `1/3`.  
   - بنجمعهم ونطرحهم ونطبع النتيجة.  
   - بنقارن بينهم ونطبع مين الأكبر.

---

## 2. Implicit و Explicit Operators - تحويل الأنواع

في المثال ده هانعمل كلاس `Temperature` (درجة حرارة) ونخليه يتحول إلى double بشكل تلقائي (Implicit) ولسهولة التحويل من int لـ Temperature بشكل يدوي (Explicit).

### البرنامج الكامل

```csharp
using System;

namespace ConversionExample
{
    class Temperature
    {
        public double Celsius { get; set; }

        public Temperature(double celsius)
        {
            Celsius = celsius;
        }

        // implicit conversion من Temperature إلى double
        public static implicit operator double(Temperature t)
        {
            return t.Celsius;
        }

        // explicit conversion من double إلى Temperature
        public static explicit operator Temperature(double celsius)
        {
            return new Temperature(celsius);
        }

        // ToString عشان نطبع بشكل مرتب
        public override string ToString()
        {
            return $"{Celsius}°C";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Temperature temp = new Temperature(25.5);

            // implicit: Temperature تتحول إلى double من غير cast
            double celsiusValue = temp;
            Console.WriteLine($"قيمة الحرارة بالـ double: {celsiusValue}");

            // explicit: لازم نستخدم cast عشان نحول double إلى Temperature
            Temperature newTemp = (Temperature)30.0;
            Console.WriteLine($"الجديدة: {newTemp}");
        }
    }
}
```

### شرح الكود

- **implicit operator double**  
   - بتسمح بتحويل كائن `Temperature` إلى `double` مباشرةً من غير كتابة `(double)`.  
   - الفكرة هنا أن درجة الحرارة هي قيمة عددية فمفيش مشكلة في التحويل التلقائي.

- **explicit operator Temperature**  
   - عشان نحول من `double` إلى `Temperature` لازم نستخدم `(Temperature)`.  
   - ليه؟ لأن التحويل العكسي مش دايمًا آمن (ممكن المستخدم يفتكر أنه بيحول حاجة غلط)، فبنخليه يدوي.

- في الـ Main:  
   - `double d = temp;` → دي implicit.  
   - `Temperature t = (Temperature)30.0;` → دي explicit.

---

## 3. Casting بين كلاسات مختلفة

في المثال ده هانعمل كلاس `Employee` (موظف) وكلاس `Person` (شخص) وهانحول الموظف لشخص باستخدام implicit operator في كلاس Person.

### البرنامج الكامل

```csharp
using System;

namespace CastingExample
{
    class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal Salary { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, الاسم: {FullName}, الراتب: {Salary}";
        }
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        // implicit conversion من Employee إلى Person
        public static implicit operator Person(Employee emp)
        {
            string[] parts = emp.FullName.Split(' ');
            return new Person { FirstName = parts[0], LastName = parts[1] };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Employee emp = new Employee
            {
                Id = 101,
                FullName = "محمد علي",
                Salary = 5000
            };

            // تحويل implicit من Employee إلى Person
            Person person = emp;
            Console.WriteLine($"بيانات الشخص: {person}");
        }
    }
}
```

### شرح الكود

- كلاس `Employee` فيه الاسم الكامل (FullName).  
- كلاس `Person` فيه الاسم الأول والأخير.  
- في كلاس `Person` عرفنا `implicit operator Person(Employee emp)` اللي بياخد موظف ويرجع شخص.  
   - بنقسم الاسم الكامل (بافتراض أنه اسم أول واسم أخير) ونعمل Person جديد.  
- في الـ Main:  
   - بنعمل موظف وبعدين نسنده لـ Person مباشرة من غير cast.  
   - التحويل بيحصل تلقائي ويظهر الشخص بالاسم الأول والأخير.

**ملحوظة:** ممكن تعمل التحويل `explicit` لو عايز تخلي الشخص يظهر بس بعد كتابة `(Person)emp`.

---

## 4. Static Classes و Static Constructor

الكلاسات الثابتة (Static Classes) بتستخدم عادةً لعمل دوال مساعدة (Helper Methods) من غير ما تحتاج تعمل object منها. كمان عندنا الـ static constructor وبيتشغل مرة واحدة في عمر البرنامج.

### البرنامج الكامل

```csharp
using System;

namespace StaticExample
{
    // كلاس static مش هنقدر نعمل منه object
    public static class StringHelper
    {
        private static string _prefix;

        // static constructor: بيتنفذ مرة واحدة قبل أي استخدام للكلاس
        static StringHelper()
        {
            _prefix = "[مُعالَج] ";
            Console.WriteLine("Static Constructor اشتغل!");
        }

        public static string AddPrefix(string original)
        {
            return _prefix + original;
        }

        public static string Reverse(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // من غير ما نعمل new StringHelper()، بنستخدم الدوال static مباشرة
            string name = "Ahmed";
            string processed = StringHelper.AddPrefix(name);
            Console.WriteLine(processed);

            string reversed = StringHelper.Reverse("C# Programming");
            Console.WriteLine(reversed);
        }
    }
}
```

### شرح الكود

- **static class StringHelper**  
   - كل ما فيه لازم يكون static (خواص، دوال، constructor).  
   - مش بنقدر نعمل `new StringHelper()`.

- **static constructor**  
   - ملهوش access modifier (public/private) ومالوش parameters.  
   - بيتنفذ مرة واحدة بس أول مرة نستخدم أي حاجة من الكلاس.  
   - هنا بنضبط قيمة `_prefix`.

- في الـ Main:  
   - بنستخدم الدوال مباشرة باسم الكلاس.  
   - أول مرة نستخدم الكلاس، الـ static constructor بيشتغل ويطبع الجملة.

---

## 5. Stateful vs Stateless Classes

الفرق بين الكلاس اللي بيحتفظ بحالة (Stateful) واللي بيشتغل من غير حالة (Stateless) هو إن Stateful بيعتمد على البيانات المخزنة جواه، أما Stateless فبياخد كل البيانات من الباراميترات ومش بيخزن حاجة.

### Stateful Class - مثال

```csharp
using System;

namespace StatefulExample
{
    class BankAccount
    {
        public string AccountHolder { get; set; }
        private decimal _balance;

        public BankAccount(string holder, decimal initialBalance)
        {
            AccountHolder = holder;
            _balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            _balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= _balance)
                _balance -= amount;
            else
                Console.WriteLine("الرصيد غير كافٍ");
        }

        public void Display()
        {
            Console.WriteLine($"{AccountHolder}: {_balance} جنيه");
        }
    }

    class Program
    {
        static void Main()
        {
            BankAccount acc = new BankAccount("Ahmed", 1000);
            acc.Deposit(500);
            acc.Withdraw(200);
            acc.Display(); // الحالة اتغيرت
        }
    }
}
```

- هنا `_balance` بتتغير مع كل عملية، فده Stateful class.

### Stateless Class - مثال

```csharp
using System;

namespace StatelessExample
{
    static class MathOperations
    {
        public static double Add(double a, double b) => a + b;
        public static double Multiply(double a, double b) => a * b;
    }

    class Program
    {
        static void Main()
        {
            double x = 10, y = 5;
            Console.WriteLine(MathOperations.Add(x, y));  // 15
            Console.WriteLine(MathOperations.Multiply(x, y)); // 50
        }
    }
}
```

- MathOperations مش بيخزن أي بيانات، كل دالة بتشتغل على الباراميترات. ده Stateless.

---

## 6. Abstract Classes, Inheritance, Polymorphism

الكلاس المجرد (Abstract) هو كلاس مش بنقدر نعمل منه object، بنستخدمه كـ blueprint لكلاساته الفرعية. وفيه بنقدر نعرّف دوال مجردة (abstract methods) لازم الكلاسات اللي بتورثه تنفذها.

### البرنامج الكامل

```csharp
using System;

namespace AbstractExample
{
    // كلاس مجرد مش هنقدر نعمل منه new
    abstract class Shape
    {
        public abstract double GetArea();  // مجرد: مفيش implementation هنا

        public void DisplayType()
        {
            Console.WriteLine("أنا شكل هندسي");
        }
    }

    class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(double r)
        {
            Radius = r;
        }

        public override double GetArea()
        {
            return Math.PI * Radius * Radius;
        }
    }

    class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double w, double h)
        {
            Width = w;
            Height = h;
        }

        public override double GetArea()
        {
            return Width * Height;
        }
    }

    class Program
    {
        static void Main()
        {
            // Shape s = new Shape(); // خطأ! abstract مش مسموح

            Shape myCircle = new Circle(5);
            Shape myRect = new Rectangle(4, 6);

            myCircle.DisplayType();
            Console.WriteLine($"مساحة الدايرة: {myCircle.GetArea()}");

            myRect.DisplayType();
            Console.WriteLine($"مساحة المستطيل: {myRect.GetArea()}");
        }
    }
}
```

### شرح الكود

- `abstract class Shape` فيها `abstract double GetArea()` و `void DisplayType()` العادية.  
- `Circle` و `Rectangle` بيرثوا من Shape وبيطبقوا `GetArea()` (إلزامي).  
- في الـ Main: بنستخدم polymorphism عن طريق تعريف متغيرات من نوع Shape لكن بنحط فيها كائنات من Circle و Rectangle.  
- `myCircle.GetArea()` بيشتغل حسب نوع الكائن الحقيقي (Circle أو Rectangle).

---

## 7. Built-in Methods: ToString, Equals, GetHashCode, GetType

في C# في كلاس اسمه `Object` وكل الكلاسات بتورث منه. جواه دوال بنقدر نغير سلوكها (override) عشان تناسب الكلاس بتاعنا.

### البرنامج الكامل

```csharp
using System;

namespace BuiltInMethodsExample
{
    class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }

        // ToString عشان نطبع بيانات الطالب
        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}";
        }

        // Equals عشان نقارن بين طالبين بناءً على ID
        public override bool Equals(object obj)
        {
            if (obj is Student other)
                return this.ID == other.ID;
            return false;
        }

        // GetHashCode عشان الـ Dictionary وغيره يشتغل صح
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }

    class Program
    {
        static void Main()
        {
            Student s1 = new Student { ID = 1, Name = "Ali" };
            Student s2 = new Student { ID = 1, Name = "Ali" };
            Student s3 = new Student { ID = 2, Name = "Mona" };

            Console.WriteLine(s1.ToString()); // ID: 1, Name: Ali

            Console.WriteLine(s1.Equals(s2)); // True (لأن ID واحد)
            Console.WriteLine(s1.Equals(s3)); // False

            // GetType بيجيب نوع الكائن
            Console.WriteLine(s1.GetType());  // BuiltInMethodsExample.Student

            // GetHashCode بينتج رقم
            Console.WriteLine($"HashCode s1: {s1.GetHashCode()}");
        }
    }
}
```

### شرح الكود

- **ToString()** بنغيرها عشان نطبع حاجة مفيدة بدل اسم الكلاس.
- **Equals()** بنغيرها عشان نقارن الطلاب بناءً على ID مش على الـ reference.
- **GetHashCode()** بنغيرها عشان لو حطينا الكائن في Dictionary مثلاً، الهاش بتاعه يكون متوافق مع الـ Equals. لازم لو objectين Equal، الـ hash بتاعهم يبقى واحد.
- **GetType()** بتجيب نوع الكائن الحقيقي وقت التشغيل.

---

## 8. Built-in Interfaces: ICloneable و IComparable

الإنترفيسات دي بتخلينا نضيف سلوك معين للكلاس: النسخ (Clone) والمقارنة (Compare).

### ICloneable مثال

```csharp
using System;

namespace CloneableExample
{
    class Product : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        // ICloneable
        public object Clone()
        {
            // بنرجع memberwise clone (نسخة سطحية)
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Price: {Price}";
        }
    }

    class Program
    {
        static void Main()
        {
            Product p1 = new Product { Id = 1, Name = "Laptop", Price = 15000 };
            Product p2 = (Product)p1.Clone(); // نسخة جديدة

            p2.Name = "Desktop"; // تعديل النسخة مش بيأثر على الأصل

            Console.WriteLine("p1: " + p1);
            Console.WriteLine("p2: " + p2);
        }
    }
}
```

- `MemberwiseClone()` بيعمل نسخة سطحية (shallow copy) من الكائن، ولو في خصائص reference لازم نتعامل معاها لو عايزين deep copy.

### IComparable مثال

```csharp
using System;
using System.Collections.Generic;

namespace ComparableExample
{
    class Employee : IComparable<Employee>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }

        public int CompareTo(Employee other)
        {
            if (other == null) return 1;
            // مقارنة حسب الراتب تصاعدي
            return this.Salary.CompareTo(other.Salary);
        }

        public override string ToString()
        {
            return $"{Name} (راتب: {Salary})";
        }
    }

    class Program
    {
        static void Main()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee { Id = 3, Name = "Ahmed", Salary = 5000 },
                new Employee { Id = 1, Name = "Sara", Salary = 7000 },
                new Employee { Id = 2, Name = "Mona", Salary = 4000 }
            };

            employees.Sort(); // هنا هيرتب حسب الـ CompareTo اللي عملناه

            foreach (var emp in employees)
                Console.WriteLine(emp);
        }
    }
}
```

- `CompareTo` بتحدد الترتيب. بنستخدم `Salary.CompareTo` لأن decimal أصلاً فيه CompareTo جاهز.  
- بعد كده لما نستخدم `employees.Sort()` هيرتب بناءً على الراتب.

---
آسف على التأخير، فعلاً فاتني شرح الـ partial والـ sealed في الـ README السابق. هكمل دلوقتي معاك وأضيفهم بالتفصيل، بنفس الأسلوب: شرح بالعامية المصرية، مع أمثلة كاملة (برامج متكاملة) وشرح كل سطر.

---

## 9. Partial Classes - الكلاسات الجزئية

الـ `partial class` هي كلاس مقسوم على أكتر من ملف. يعني ممكن تكتب جزء من الكلاس في ملف، والجزء التاني في ملف تاني، والكومبايلر لما يشوف الكود لمّهم مع بعض ويعتبرهم كلاس واحد.

### إمتى بنستخدمها؟
- لو شغال في مشروع كبير وفيه أكتر من مطور على نفس الكلاس.
- لو الكلاس بيتولد بشكل تلقائي (مثلاً من Entity Framework أو Windows Forms) وعايز تضيف فيه حاجات من غير ما تعدل على الملف المتولد.

### مثال: كلاس جزئي لجزئين

هنعمل برنامج بسيط لحساب مرتب موظف، جزء منه في ملف و جزء في ملف تاني. هنا هكتبهم في نفس الملف عشان المثال مختصر، بس الفكرة إنهم ممكن يكونوا في ملفين فعليين.

**البرنامج الكامل:**

```csharp
using System;

namespace PartialClassExample
{
    // الجزء الأول من الكلاس
    public partial class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // الجزء التاني من الكلاس (ممكن يكون في ملف تاني)
    public partial class Employee
    {
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }

        public decimal CalculateTotalSalary()
        {
            return BasicSalary + Bonus;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"الموظف: {Name} (ID: {Id})");
            Console.WriteLine($"الراتب الأساسي: {BasicSalary}, المكافأة: {Bonus}");
            Console.WriteLine($"الإجمالي: {CalculateTotalSalary()}");
        }
    }

    class Program
    {
        static void Main()
        {
            Employee emp = new Employee
            {
                Id = 101,
                Name = "أحمد محمود",
                BasicSalary = 5000,
                Bonus = 1000
            };

            emp.DisplayInfo();
        }
    }
}
```

### شرح الكود

- الكلمة المفتاحية `partial` قبل `class` بتخبر الكومبايلر إن ده جزء من كلاس أكبر.
- عندنا جزئين للكلاس `Employee`: الجزء الأول فيه الخواص الأساسية (Id, Name)، والتاني فيه الخواص التانية ودالة الحساب.
- في `Main` بنستخدم الكلاس بشكل طبيعي جداً، كأنه مكتوب في ملف واحد.
- الفايدة الحقيقية تبان في المشاريع الكبيرة أو مع الكود المتولد آلياً.

---

## 10. Sealed Classes - الكلاسات المختومة

الـ `sealed class` هي كلاس ممنوع التوريث منه. يعني أي كلاس تاني ميقدرش يعمله `inherit`.

### إمتى بنستخدمها؟
- لو عايز تمنع أي تعديل على سلوك الكلاس عن طريق الوراثة.
- لو الكلاس معمول بطريقة معينة والتوريث ممكن يسبب مشاكل (مثلاً لأسباب أمنية أو أداء).
- بعض الكلاسات الجاهزة في C# زي `String` هي `sealed`.

### مثال: كلاس مختوم

هنعمل كلاس `MathOperations` فيه دوال حسابية، ونخليه sealed. بعد كده نحاول نعمل كلاس تاني يرث منه هنلاقي الخطأ.

**البرنامج الكامل:**

```csharp
using System;

namespace SealedClassExample
{
    // كلاس مختوم
    public sealed class MathOperations
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }
    }

    // لو حاولنا نعمل inheritance هنا هنشوف خطأ
    // public class AdvancedMath : MathOperations { } // Compiler Error: cannot derive from sealed type

    class Program
    {
        static void Main()
        {
            MathOperations math = new MathOperations();
            Console.WriteLine($"الجمع: {math.Add(10, 5)}");
            Console.WriteLine($"الضرب: {math.Multiply(10, 5)}");
        }
    }
}
``` 

### شرح الكود

- `public sealed class MathOperations` معناها إن الكلاس ده مش هيورث.
- لو حاولت تعمل كلاس تاني يرث منه، الكومبايلر هيقولك خطأ.
- في الـ `Main` بنستخدم الكلاس بشكل طبيعي، نعمل object ونستخدم دواله.
- استخدام `sealed` بيساعد في منع التعديل غير المقصود على الكلاس.

---

## 11. الفرق بين Abstract, Sealed, Static, Partial, Concrete

خلينا نعمل مقارنة سريعة:

| نوع الكلاس | يقدر نعمل منه object؟ | يقدر يتورث منه؟ | يقدر يورث من كلاس تاني؟ |
|---|---|---|---|
| **Concrete** (عادي) | أيوه | أيوه | أيوه |
| **Abstract** | لأ (لازم ابن يرث) | أيوه | أيوه |
| **Sealed** | أيوه | لأ (محدش يورث منه) | أيوه |
| **Static** | لأ | لأ | لأ (بيحمل static members بس) |
| **Partial** | (مجرد تقسيم ملفات) نفس الكلاس الأصلي | حسب الكلاس نفسه | حسب الكلاس نفسه |

## خلاصة

في الدليل ده اتعرفنا على:

- **Operator Overloading** - إزاي نخلي الكلاسات بتاعتنا تتصرف مع العمليات الحسابية.
- **Implicit و Explicit Operators** - تحويل الكلاسات لأنواع تانية.
- **Casting بين كلاسات مختلفة** - تحويل من Employee إلى Person.
- **Static Classes** - الكلاسات المساعدة اللي مش بنعمل منها objects.
- **Stateful vs Stateless** - الفرق بين الكلاس اللي بيحتفظ بحالة واللي بيشتغل بالدوال بس.
- **Abstract Classes و Inheritance و Polymorphism** - أساسيات الوراثة والشكل التجريدي.
- **Built-in Methods** - ToString, Equals, GetHashCode, GetType.
- **Built-in Interfaces** - ICloneable, IComparable.