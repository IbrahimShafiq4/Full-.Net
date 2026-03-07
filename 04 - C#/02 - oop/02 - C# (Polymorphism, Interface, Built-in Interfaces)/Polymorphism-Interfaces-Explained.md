# شرح OOP في C#

## الفهرس

1. [Encapsulation (التغليف) وإخفاء البيانات](#1-encapsulation-التغليف-وإخفاء-البيانات)
2. [Constructors (البنائين)](#2-constructors-البنائين)
3. [Struct vs Class](#3-struct-vs-class)
4. [Polymorphism (تعدد الأشكال)](#4-polymorphism-تعدد-الأشكال)
   - 4.1 [Method Overloading](#41-method-overloading)
   - 4.2 [Method Overriding](#42-method-overriding)
5. [Interface (الواجهة)](#5-interface-الواجهة)
6. [Built-in Interfaces](#6-built-in-interfaces)
   - 6.1 [ICloneable (نسخ الكائنات)](#61-icloneable-نسخ-الكائنات)
   - 6.2 [IComparable (ترتيب الكائنات)](#62-icomparable-ترتيب-الكائنات)
7. [Operator Overloading (تحميل العوامل)](#7-operator-overloading-تحميل-العوامل)
8. [Object Class Methods](#8-object-class-methods)
9. [الخلاصة](#9-الخلاصة)

---

## 1. Encapsulation (التغليف) وإخفاء البيانات

**Encapsulation** يعني نخفي تفاصيل التنفيذ الداخلية للكلاس ونحمي البيانات من الوصول المباشر من برّا الكلاس. في C# بنستخدم **access modifiers** عشان نحدد مين يقدر يشوف إيه.

### Access Modifiers

- `private`: accessible only داخل نفس الكلاس.
- `public`: accessible من أي مكان.
- `internal`: accessible داخل نفس الـ assembly (المشروع).
- `protected`: accessible في الكلاس نفسه وفي أي كلاس وارث منه.

### Properties

الـ **Property** عبارة عن زوج من الدوال (getter و setter) بتتحكم في الوصول لحقل خاص (private field). ده بيدينا القدرة نضيف logic وقت القراءة أو الكتابة.

#### مثال كامل لـ Class مع Properties

```csharp
using System;

namespace EncapsulationExample
{
    // كلاس بيمثل موظف
    public class Employee
    {
        // حقول خاصة (private fields) - البيانات الفعلية مخفية
        private int id;
        private string name;
        private decimal salary;

        // Constructor (هنشرحه بالتفصيل بعدين)
        public Employee(int id, string name, decimal salary)
        {
            this.id = id;
            this.name = name;
            // هنا استخدمنا الـ property عشان نضمن التحقق
            Salary = salary;
        }

        // Property لـ id (قراءة فقط)
        public int Id
        {
            get { return id; }
        }

        // Property لـ name (قراءة وكتابة مع تحقق)
        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    name = value;
                else
                    throw new ArgumentException("Name cannot be empty");
            }
        }

        // Property لـ salary (مع منطق في الـ set)
        public decimal Salary
        {
            get { return salary; }
            set
            {
                if (value >= 0)
                    salary = value;
                else
                    throw new ArgumentException("Salary must be positive");
            }
        }

        // دالة لعرض البيانات
        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}, Name: {Name}, Salary: {Salary:C}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // إنشاء كائن Employee
            Employee emp = new Employee(1, "Ibrahim", 5000);

            // عرض البيانات
            emp.DisplayInfo();  // ID: 1, Name: Ibrahim, Salary: $5,000.00

            // محاولة تغيير الاسم
            emp.Name = "Ahmed";
            emp.DisplayInfo();  // ID: 1, Name: Ahmed, Salary: $5,000.00

            // محاولة تعيين راتب سالب -> هتسبب Exception
            // emp.Salary = -1000;  // Uncommenting this will throw exception
        }
    }
}
```

**شرح كل سطر:**

- `private int id;` → حقل خاص بالكلاس، مش متاح برّا الكلاس.
- `public int Id { get { return id; } }` → property عامة للقراءة فقط، بتجيب قيمة الحقل `id`.
- `public string Name { get; set; }` → property auto-implemented (C# بيخلق حقل خلفي تلقائيًا)، لكن هنا كتبناها manually عشان نتحكم.
- في setter بتاع `Name` بنتأكد إن القيمة مش فاضية قبل ما نخزنها.
- `public decimal Salary { get; set; }` → نفس الكلام مع التحقق من القيمة الموجبة.
- في الكونستركتور استخدمنا `Salary = salary;` مش `this.salary = salary;` عشان نستفيد من التحقق الموجود في الـ property.
- `emp.Name = "Ahmed";` → هنا استخدمنا الـ setter عشان نعدل الاسم، ولو جبنا القيمة بالـ getter هيرجع الاسم الجديد.

**ليه بنستخدم Encapsulation؟**
- حماية البيانات من القيم الغير صحيحة.
- إخفاء التفاصيل الداخلية، فتقدر تغير التنفيذ جوه الكلاس من غير ما تأثر على الـ code اللي بيستخدم الكلاس.
- سهولة الصيانة والتحكم.

---

## 2. Constructors (البنائين)

الـ **Constructor** هو دالة خاصة بتتنفذ أول ما نعمل `new` من الكلاس، وبتستخدم عشان تهيئ الكائن (initialize). في C# في أكتر من نوع.

### أنواع الـ Constructors

1. **Default Constructor**: من غير باراميترات (لو منكتبش أي Constructor، C# بيضيف واحد افتراضي).
2. **Parameterized Constructor**: بياخد باراميترات عشان نحدد قيم أولية.
3. **Copy Constructor**: بياخد كائن من نفس النوع وينسخ بياناته.
4. **Static Constructor**: بيتنفذ مرة واحدة قبل أي استخدام للكلاس (بيشتغل على مستوى النوع مش الكائن).

#### مثال كامل يوضح كل الأنواع

```csharp
using System;

namespace ConstructorsExample
{
    public class Car
    {
        public string Model { get; set; }
        public int Year { get; set; }
        public static int NumberOfWheels;  // static field

        // Static Constructor (يتنفذ مرة واحدة قبل أي شيء)
        static Car()
        {
            NumberOfWheels = 4;
            Console.WriteLine("Static Constructor called.");
        }

        // Default Constructor
        public Car()
        {
            Model = "Unknown";
            Year = 2020;
            Console.WriteLine("Default Constructor called.");
        }

        // Parameterized Constructor
        public Car(string model, int year)
        {
            Model = model;
            Year = year;
            Console.WriteLine("Parameterized Constructor called.");
        }

        // Copy Constructor
        public Car(Car otherCar)
        {
            Model = otherCar.Model;
            Year = otherCar.Year;
            Console.WriteLine("Copy Constructor called.");
        }

        public void Display()
        {
            Console.WriteLine($"Model: {Model}, Year: {Year}, Wheels: {NumberOfWheels}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // استخدام default constructor
            Car car1 = new Car();
            car1.Display();   // Model: Unknown, Year: 2020, Wheels: 4

            // استخدام parameterized constructor
            Car car2 = new Car("Toyota", 2022);
            car2.Display();   // Model: Toyota, Year: 2022, Wheels: 4

            // استخدام copy constructor
            Car car3 = new Car(car2);
            car3.Display();   // Model: Toyota, Year: 2022, Wheels: 4

            // تعديل car2 ما يأثرش على car3 لأنها نسخة منفصلة (shallow copy للـ properties)
            car2.Model = "Honda";
            Console.WriteLine("After modifying car2:");
            car2.Display();   // Model: Honda, Year: 2022
            car3.Display();   // Model: Toyota, Year: 2022 (لم يتأثر)
        }
    }
}
```

**شرح كل جزء:**

- `static Car()` → static constructor ملهاش access modifier وبتتنفذ مرة واحدة أول ما نستخدم الكلاس (قبل أي constructor عادي أو static member). هنا بنهيئ `NumberOfWheels`.
- `public Car() { }` → default constructor بنضبط فيه قيم افتراضية.
- `public Car(string model, int year)` → parameterized constructor بنستقبل البيانات ونسندها للـ properties.
- `public Car(Car otherCar)` → copy constructor بياخد كائن `Car` تاني وينسخ خصائصه. (دي deep copy للـ value types والـ strings، لكن لو في reference types محتاجين deep copy حقيقي).
- في `Main`، كل constructor بينادي نفسه وبنطبع الرسالة المناسبة.
- `NumberOfWheels` static ظهرت في كل الكائنات بنفس القيمة.

**معلومة إضافية:**  
لو عايز تمنع أي حد يعمل كائن من الكلاس، اعمل constructor خاص `private Car() { }` (مثلاً في Singleton pattern).

---

## 3. Struct vs Class

في C#، الفرق الجوهري بين `struct` و `class` هو إن الـ **struct** نوع قيمة (value type) والـ **class** نوع مرجعي (reference type). الكلام ده بيأثر على الأداء وسلوك النسخ.

### الفروق الأساسية

| الميزة | struct | class |
|--------|--------|-------|
| نوع | قيمة (value type) | مرجعي (reference type) |
| مكان التخزين | Stack (أو داخل الكائن) | Heap |
| النسخ | بنسخ القيم (copy by value) | بنسخ المرجع (copy by reference) |
| الوراثة | مش بيورث من class أو struct تاني (بس بيطبق interface) | بيورث من class واحد وأي عدد interfaces |
| Default constructor | مش بتقدر تعرف default constructor manually (لكن C# 10+ سمح) | بتقدر |
| Destructor | مفيش destructor | فيه destructor (finalizer) |
| Nullability | دايمًا ليها قيمة (لا يمكن تكون null إلا لو nullable) | تقدر تكون null |

#### مثال عملي يوضح الفرق

```csharp
using System;

namespace StructVsClassExample
{
    // تعريف struct
    public struct PointStruct
    {
        public int X;
        public int Y;

        public PointStruct(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Display()
        {
            Console.WriteLine($"Struct Point: ({X}, {Y})");
        }
    }

    // تعريف class
    public class PointClass
    {
        public int X;
        public int Y;

        public PointClass(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Display()
        {
            Console.WriteLine($"Class Point: ({X}, {Y})");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // التعامل مع struct
            PointStruct p1 = new PointStruct(10, 20);
            PointStruct p2 = p1;  // هنا بنسخ القيم

            p2.X = 100;
            Console.WriteLine("After modifying struct copy:");
            p1.Display();  // (10, 20) - لم تتغير
            p2.Display();  // (100, 20)

            // التعامل مع class
            PointClass pc1 = new PointClass(10, 20);
            PointClass pc2 = pc1;  // هنا بنسخ المرجع (الاتنين بيعبروا على نفس الكائن)

            pc2.X = 100;
            Console.WriteLine("\nAfter modifying class reference:");
            pc1.Display();  // (100, 20) - تغيرت مع pc2
            pc2.Display();  // (100, 20)

            // اختبار nullability
            // PointStruct p3 = null; // خطأ: struct لا يمكن أن تكون null (إلا لو Nullable<PointStruct>)
            PointClass pc3 = null; // class يقبل null
        }
    }
}
```

**شرح كل جزء:**

- `PointStruct` هو struct، و `PointClass` هو class، كلاهما فيه إحداثيات X,Y.
- في حالة الـ struct، `p2 = p1` يعمل نسخة جديدة تمامًا من القيم. أي تعديل في `p2` ما يأثرش على `p1`.
- في حالة الـ class، `pc2 = pc1` ينسخ المرجع، فالاتنين يشيروا لنفس الكائن في الـ heap. تعديل `pc2.X` يغير الكائن الأصلي.
- `pc3 = null` مسموح للـ class، لكن الـ struct لا يمكن أن تكون null (إلا لو استخدمنا `Nullable<PointStruct>` أو `PointStruct?`).

**متى نستخدم struct؟**
- لو الكائن صغير الحجم (عادة أقل من 16 بايت) وبيتكرر كتير (زي الأعداد المركبة، النقاط، الألوان).
- لو الكائن immutable (غير قابل للتعديل بعد الإنشاء).
- لو مش محتاج وراثة.
- الأداء: struct أسرع في التخصيص على الـ stack وبتقلل الضغط على Garbage Collector.

**متى نستخدم class؟**
- معظم الحالات، خصوصًا لو الكائن كبير أو محتاج وراثة.
- لو محتاج تمرير بالمرجع وتعديل الكائن من مكان واحد.
- لو محتاج nullability.

---

## 4. Polymorphism (تعدد الأشكال)

**Polymorphism** يعني إن الكائن الواحد يقدر يظهر بأشكال مختلفة. في C# نوعين:

- **Method Overloading** (زيادة التحميل) – compile-time polymorphism.
- **Method Overriding** (التجاوز) – runtime polymorphism.

---

### 4.1 Method Overloading

الـ Overloading هو إننا نعرف أكتر من دالة بنفس الاسم في نفس النطاق، لكن باختلاف في عدد أو نوع الباراميترات. الـ compiler هو اللي يحدد أي دالة تناسب الـ arguments اللي إحنا مديينها.

#### مثال كامل على Overloading

```csharp
using System;

namespace OverloadingExample
{
    class Calculator
    {
        // دالة لجمع عددين صحيحين
        public int Add(int a, int b)
        {
            Console.WriteLine("Add(int, int)");
            return a + b;
        }

        // دالة لجمع ثلاثة أعداد صحيحة (اختلاف في العدد)
        public int Add(int a, int b, int c)
        {
            Console.WriteLine("Add(int, int, int)");
            return a + b + c;
        }

        // دالة لجمع عددين عشريين (اختلاف في النوع)
        public double Add(double a, double b)
        {
            Console.WriteLine("Add(double, double)");
            return a + b;
        }

        // دالة لجمع array من الأعداد (اختلاف في النوع أيضًا)
        public int Add(int[] numbers)
        {
            Console.WriteLine("Add(int[])");
            int sum = 0;
            foreach (int n in numbers)
                sum += n;
            return sum;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Calculator calc = new Calculator();

            Console.WriteLine(calc.Add(5, 10));           // Add(int, int) → 15
            Console.WriteLine(calc.Add(5, 10, 15));        // Add(int, int, int) → 30
            Console.WriteLine(calc.Add(2.5, 3.7));          // Add(double, double) → 6.2
            Console.WriteLine(calc.Add(new int[] { 1, 2, 3, 4 })); // Add(int[]) → 10
        }
    }
}
```

**شرح الكود:**
- عند استدعاء `Add(5,10)`، الـ compiler بيشوف إن الـ arguments اثنين int، فيختار الدالة اللي بتاخد int, int.
- عند استدعاء `Add(2.5, 3.7)`، بيشوف double فيختار الدالة اللي بتاخد double, double.
- الـ overloading بيحصل في **compile-time**، يعني القرار بيتحدد بناءًا على أنواع الـ arguments اللي إحنا كاتبينها مش وقت التشغيل.

**ملاحظة:** مينفعش نعمل overloading بناءًا على نوع الـ return فقط، لازم اختلاف في الباراميترات.

---

### 4.2 Method Overriding

الـ Overriding بيحصل في علاقة الوراثة (inheritance)، حيث إن الكلاس الابن يقدر يعيد تعريف دالة موجودة في الكلاس الأب، بشرط إن الأب يكون عامل الدالة دي **`virtual`** أو **`abstract`**، والابن يستخدم **`override`**.

#### أنواع الـ Binding
- **Static Binding (Early Binding)**: الـ compiler يقرر أي دالة ينادي بناءًا على نوع المرجع (reference type). بيحصل مع الدوال العادية ومع الـ `new` keyword.
- **Dynamic Binding (Late Binding)**: الـ runtime يقرر أي دالة ينادي بناءًا على نوع الكائن الفعلي في الـ heap. بيحصل مع `virtual` و `override`.

#### مثال كامل على Overriding

```csharp
using System;

namespace OverridingExample
{
    // الأب
    class Animal
    {
        public string Name { get; set; }

        public Animal(string name)
        {
            Name = name;
        }

        // دالة عادية (غير virtual) → مش مسموح بتجاوزها
        public void Eat()
        {
            Console.WriteLine($"{Name} is eating (animal version).");
        }

        // دالة virtual → تسمح للابن يعمل override
        public virtual void MakeSound()
        {
            Console.WriteLine("Animal makes a generic sound.");
        }

        // دالة virtual تانية
        public virtual void Sleep()
        {
            Console.WriteLine($"{Name} is sleeping (animal version).");
        }
    }

    // ابن (كلب)
    class Dog : Animal
    {
        public Dog(string name) : base(name) { }

        // استخدام new لإخفاء دالة Eat الخاصة بالأب (static binding)
        public new void Eat()
        {
            Console.WriteLine($"{Name} the dog is eating pedigree.");
        }

        // override لدالة MakeSound (dynamic binding)
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Woof Woof!");
        }

        // override لدالة Sleep
        public override void Sleep()
        {
            Console.WriteLine($"{Name} the dog is sleeping in the kennel.");
        }
    }

    // ابن تاني (قطة)
    class Cat : Animal
    {
        public Cat(string name) : base(name) { }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Meow Meow!");
        }

        // ممكن override أو لا
        public override void Sleep()
        {
            Console.WriteLine($"{Name} the cat is sleeping on the sofa.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // إنشاء كائنات بمراجع من نوع Animal لكن الكائنات الفعلية Dog و Cat
            Animal a1 = new Dog("Bobby");
            Animal a2 = new Cat("Kitty");

            // استدعاء Eat (غير virtual) → static binding على حسب نوع المرجع Animal
            a1.Eat();  // "Bobby is eating (animal version)."
            a2.Eat();  // "Kitty is eating (animal version)."

            // استدعاء MakeSound (virtual) → dynamic binding حسب نوع الكائن الفعلي
            a1.MakeSound();  // "Bobby says: Woof Woof!"
            a2.MakeSound();  // "Kitty says: Meow Meow!"

            // استدعاء Sleep (virtual) → dynamic binding
            a1.Sleep();  // "Bobby the dog is sleeping in the kennel."
            a2.Sleep();  // "Kitty the cat is sleeping on the sofa."

            // لو عايز نستخدم نسخة Dog من Eat لازم نعمل cast
            ((Dog)a1).Eat(); // "Bobby the dog is eating pedigree."
        }
    }
}
```

**شرح الكود بالتفصيل:**

- `Animal` هي الكلاس الأب، فيها `Eat` (غير virtual) و `MakeSound` (virtual) و `Sleep` (virtual).
- `Dog` و `Cat` بيرثوا من `Animal`.
- في `Dog`، عملنا `new void Eat()` عشان نخفي دالة `Eat` الخاصة بالأب. ده معناه إنه لو استخدمنا reference من نوع `Animal` ومشير لكائن `Dog`، استدعاء `Eat()` هينفذ نسخة الأب (static binding). لكن لو استخدمنا reference من نوع `Dog`، هينفذ نسخة `Dog`.
- بالنسبة لـ `MakeSound` و `Sleep`، استخدموا `override`، فـ dynamic binding هو اللي هيشتغل: الـ runtime بيشوف نوع الكائن الفعلي (Dog أو Cat) وينفذ الدالة المناسبة، حتى لو المرجع كان من نوع `Animal`.

**إيه الفرق بين `new` و `override`؟**

| `new` | `override` |
|-------|------------|
| بيخفي تنفيذ الأب (hiding) | بيستبدل تنفيذ الأب (overriding) |
| الـ binding static (حسب نوع المرجع) | الـ binding dynamic (حسب نوع الكائن) |
| مش بيعتبر polymorphism حقيقي | بيدعم polymorphism |

---

## 5. Interface (الواجهة)

الـ **Interface** هو عقد (contract) بيحدد مجموعة من الخصائص والدوال بدون تنفيذ (إلا لو استخدمنا default implementation من C# 8.0). أي كلاس أو struct بيطبق الـ interface لازم يوفر التنفيذ لكل الأعضاء دول.

### إيه المشكلة اللي بيحلها الـ Interface؟

- C# مش بتدعم الوراثة المتعددة (multiple inheritance) للكلاسات، لكن الكلاس يقدر يطبق أكتر من interface. ده بيدي مرونة أكبر.
- بيساعد في فصل العقود عن التنفيذ (program to an interface not implementation) وده بيسهل الـ unit testing والصيانة.
- بيخليك تكتب كود reusable (قابل لإعادة الاستخدام) زي ما هنشوف في مثال `ISeries`.

#### مثال كامل على Interface مع تطبيقاته

```csharp
using System;

namespace InterfaceExample
{
    // تعريف Interface
    public interface IMyType
    {
        // Property signature
        int Id { get; set; }
        string Name { get; set; }

        // Method signature
        void Display();

        // Default implementation (C# 8.0+) - مش لازم الكلاسات المطبقة تنفذها
        void PrintDefault()
        {
            Console.WriteLine("This is a default implementation from IMyType");
        }
    }

    // كلاس بيطبق الـ Interface
    public class Product : IMyType
    {
        // تنفيذ الخصائص
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } // خاصية إضافية

        // Constructor
        public Product(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        // تنفيذ Display
        public void Display()
        {
            Console.WriteLine($"Product: Id={Id}, Name={Name}, Price={Price:C}");
        }

        // اختياري: لو عايز نعمل override للـ default method لازم نكتبها هنا
        public void PrintDefault()
        {
            Console.WriteLine("Overridden default method in Product");
        }
    }

    // كلاس تاني بيطبق نفس الـ Interface
    public class Category : IMyType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void Display()
        {
            Console.WriteLine($"Category: Id={Id}, Name={Name}");
        }

        // مش هنعمل override للـ PrintDefault، هتاخد default implementation
    }

    class Program
    {
        static void Main(string[] args)
        {
            // استخدام الـ Interface كـ reference (polymorphism)
            IMyType item1 = new Product(1, "Laptop", 1200.50m);
            IMyType item2 = new Category(10, "Electronics");

            item1.Display();  // Product: Id=1, Name=Laptop, Price=$1,200.50
            item2.Display();  // Category: Id=10, Name=Electronics

            item1.PrintDefault(); // Overridden default method in Product
            item2.PrintDefault(); // This is a default implementation from IMyType

            // استدعاء الخصائص
            Console.WriteLine($"item1 Id: {item1.Id}, Name: {item1.Name}");
        }
    }
}
```

**شرح الكود:**

- `IMyType` فيه خاصيتين (Id, Name) ودالة Display و default method PrintDefault.
- كلاس `Product` و `Category` بيطبقوا `IMyType`، فلازم ينفذوا Id, Name, Display. (PrintDefault مش إلزامي).
- في `Product`، عملنا override للـ `PrintDefault` عشان نغير السلوك. في `Category`، مش عاملين override، فلما نناديها من reference `IMyType`، هتشتغل default implementation.
- في `Main`، عملنا references من نوع `IMyType` وأشرناهم لكائنات `Product` و `Category`. ده polymorphism باستخدام interface: نفس النوع (`IMyType`) بيشير لكائنات مختلفة، واستدعاء `Display()` بيشتغل حسب نوع الكائن الفعلي.

#### فوائد الـ Interface – مثال ISeries المتقدم

في الكود الأصلي، كان فيه مثال رائع لـ interface `ISeries` بيوضح إزاي بنوحد المعالجة لأي series.

```csharp
using System;

namespace ISeriesExample
{
    // تعريف interface للسلسلة (series)
    public interface ISeries
    {
        int Current { get; set; }
        void GetNext();
        void Reset() => Current = 0; // default method
    }

    // تنفيذ سلسة بزيادة 2
    public class SeriesByTwo : ISeries
    {
        public int Current { get; set; }

        public void GetNext()
        {
            Current += 2;
        }
        // Reset مش محتاج ننفذها لأن ليها default implementation
    }

    // تنفيذ سلسلة بزيادة 3
    public class SeriesByThree : ISeries
    {
        public int Current { get; set; }

        public void GetNext()
        {
            Current += 3;
        }
    }

    // تنفيذ سلسلة بزيادة 0.5 (لو افترضنا إن Current ممكن يكون double، لكن هنا int للتبسيط)
    // لكن لو عايز double، هنحتاج نغير interface، بس ده مثال للفكرة

    class Program
    {
        // دالة بتشتغل على أي object من type ISeries
        public static void PrintSeries(ISeries series, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write($"{series.Current} ");
                series.GetNext();
            }
            Console.WriteLine();
            series.Reset(); // استدعاء default method
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Series by two:");
            PrintSeries(new SeriesByTwo(), 10); // 0 2 4 6 8 10 12 14 16 18

            Console.WriteLine("Series by three:");
            PrintSeries(new SeriesByThree(), 10); // 0 3 6 9 12 15 18 21 24 27
        }
    }
}
```

**ليه ده مفيد؟**
- لو معملناش interface، كنا هنحتاج نكتب دالة لكل نوع على حدة (SeriesByTwo, SeriesByThree، ...) → تكرار كود كبير.
- باستخدام interface، دالة `PrintSeries` واحدة بتشتغل مع أي class بيطبق `ISeries`. ده اسمه **polymorphism**.
- لو جابوا series جديد (زي بالضرب في 2)، هيعمل class جديد يطبق `ISeries` و `PrintSeries` هتشتغل معاه على طول بدون تعديل.

---

## 6. Built-in Interfaces

الـ C# فيه مجموعة من الـ interfaces الجاهزة اللي بنستخدمها كتير. أشهرهم `ICloneable` و `IComparable`.

### 6.1 ICloneable (نسخ الكائنات)

الـ `ICloneable` فيه دالة واحدة `Clone()` بترجع `object`. الهدف منها إنشاء نسخة من الكائن. لكن الفكرة إن إحنا بنتحكم في نوع النسخة: **Shallow Copy** (سطحية) ولا **Deep Copy** (عميقة).

- **Shallow Copy**: بنسخ الـ value types، لكن لو في reference types، بنسخ المرجع بس (pointer) يبقى الكائن الأصلي والنسخة بيشيروا لنفس الحاجة.
- **Deep Copy**: بنسخ كل حاجة، وبعمل كائنات جديدة للـ reference types عشان يكونوا مستقلين تمامًا.

#### مثال كامل لـ Employee مع ICloneable باستخدام Copy Constructor

```csharp
using System;

namespace CloneableExample
{
    // كلاس Employee يطبق ICloneable
    public class Employee : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public Address EmpAddress { get; set; } // reference type

        // Constructor
        public Employee(int id, string name, decimal salary, Address address)
        {
            Id = id;
            Name = name;
            Salary = salary;
            EmpAddress = address;
        }

        // Copy Constructor (لعمل Deep Copy)
        public Employee(Employee other)
        {
            Id = other.Id;
            Name = other.Name;
            Salary = other.Salary;
            // نعمل new Address من الكائن الأصلي (Deep Copy)
            EmpAddress = new Address(other.EmpAddress);
        }

        // تنفيذ Clone
        public object Clone()
        {
            // استخدام copy constructor
            return new Employee(this);
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Salary: {Salary:C}, Address: {EmpAddress}";
        }
    }

    // كلاس Address (reference type)
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }

        public Address(string street, string city)
        {
            Street = street;
            City = city;
        }

        // Copy constructor لـ Address
        public Address(Address other)
        {
            Street = other.Street;
            City = other.City;
        }

        public override string ToString() => $"{Street}, {City}";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Address addr = new Address("10 Tahrir St", "Cairo");
            Employee emp1 = new Employee(1, "Ibrahim", 5000, addr);

            // استنساخ باستخدام Clone (Deep Copy)
            Employee emp2 = (Employee)emp1.Clone();

            // تعديل بيانات emp2
            emp2.Name = "Ahmed";
            emp2.Salary = 6000;
            emp2.EmpAddress.Street = "15 Nile St"; // تعديل العنوان

            Console.WriteLine("emp1: " + emp1); // العنوان لم يتغير
            Console.WriteLine("emp2: " + emp2);
        }
    }
}
```

**شرح الكود:**

- `Employee` فيه property `EmpAddress` من نوع `Address` (class).
- في copy constructor بتاع `Employee`، عملنا `EmpAddress = new Address(other.EmpAddress)` عشان ننشئ كائن Address جديد (deep copy).
- في `Clone()`، بنرجع `new Employee(this)` اللي بينادي copy constructor.
- في `Main`، بعد التعديل على `emp2.EmpAddress.Street`، العنوان في `emp1` مأثرش لأن كل واحد عنده كائن Address خاص بيه. ده دليل إن النسخة deep copy.

لو كنا استخدمنا `MemberwiseClone()` (اللي هو protected method من الـ Object class)، كانت هتعمل shallow copy، وكان التعديل على العنوان هيأثر على emp1.

**ملحوظة:** `ICloneable` مش محبوب أوي في بعض الحالات لأنها مش واضحة نوع النسخة، فبتلاقي ناس بتعمل methods خاصة بـ DeepCopy و ShallowCopy.

---

### 6.2 IComparable (ترتيب الكائنات)

الـ `IComparable` بيسمح للكائن إنه يقارن نفسه مع كائن تاني، وده بيساعد في ترتيب (sorting) الكائنات. فيه دالة واحدة `CompareTo(object obj)`.

#### قيم الرجوع من CompareTo:
- أقل من 0: الكائن الحالي (this) أقل من اللي بنقارن بيه.
- 0: متساويين.
- أكبر من 0: الكائن الحالي أكبر.

#### مثال كامل لـ Employee مع IComparable (ترتيب حسب الراتب)

```csharp
using System;

namespace ComparableExample
{
    public class Employee : IComparable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }

        public Employee(int id, string name, decimal salary)
        {
            Id = id;
            Name = name;
            Salary = salary;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Salary: {Salary:C}";
        }

        // تنفيذ IComparable
        public int CompareTo(object obj)
        {
            if (obj == null) return 1; // this أكبر من null

            Employee other = obj as Employee;
            if (other == null)
                throw new ArgumentException("Object is not an Employee");

            // مقارنة حسب الراتب (ترتيب تصاعدي)
            // لو عايز ترتيب تنازلي، نعكس الإشارة
            if (this.Salary < other.Salary)
                return -1;
            else if (this.Salary > other.Salary)
                return 1;
            else
                return 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Employee[] employees = new Employee[]
            {
                new Employee(3, "Ahmed", 7000),
                new Employee(1, "Ibrahim", 3000),
                new Employee(2, "Shafiq", 5000),
                new Employee(5, "Mona", 11000),
                new Employee(4, "Sara", 9000)
            };

            Console.WriteLine("Before sorting:");
            foreach (var emp in employees)
                Console.WriteLine(emp);

            // الترتيب باستخدام IComparable
            Array.Sort(employees);

            Console.WriteLine("\nAfter sorting by Salary (ascending):");
            foreach (var emp in employees)
                Console.WriteLine(emp);

            // ترتيب تنازلي (باستخدام Array.Reverse بعد Sort)
            Array.Reverse(employees);
            Console.WriteLine("\nAfter sorting descending:");
            foreach (var emp in employees)
                Console.WriteLine(emp);
        }
    }
}
```

**شرح الكود:**

- `Employee` يطبق `IComparable`، وطبقنا `CompareTo` بناءًا على `Salary`.
- في `Main`، أنشأنا مصفوفة من الموظفين.
- `Array.Sort(employees)` بيستخدم `CompareTo` عشان يرتب المصفوفة.
- الترتيب الناتج تصاعدي (حسب الراتب).
- بعدين استخدمنا `Array.Reverse` عشان نحصل على ترتيب تنازلي.

**ملاحظة:** لو عايز ترتيب تصاعدي بناءًا على حقل تاني (زي الاسم)، نقدر نغير `CompareTo`. أو نستخدم `IComparer` لو عايز طرق ترتيب متعددة.

---

## 7. Operator Overloading (تحميل العوامل)

الـ Operator Overloading معناه إننا نقدر نعطي معنى جديد للعامل (operator) زي `+`, `-`, `*`, `/`, `==`, `!=` مع الكلاسات بتاعتنا. يعني نقدر نجمع كائنين من نفس النوع زي ما بنجمع الأرقام.

### قواعد أساسية
- لازم الدالة تكون `public static`.
- بتاخد باراميترات من النوع اللي بنطبق عليه.
- بنستخدم الكلمة `operator` متبوعة بالعامل المطلوب.
- مينفعش نعمل overload لكل العوامل (في عوامل مش مسموح زى `=` و `?:` و `new`).

#### مثال كامل لـ Complex Number مع Overloading للعوامل الحسابية والمقارنة

```csharp
using System;

namespace OperatorOverloadingExample
{
    // كلاس يمثل عدد مركب (real + imaginary)
    public class Complex
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        // Overload لعامل الجمع +
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }

        // Overload لعامل الطرح -
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }

        // Overload لعامل الضرب *
        public static Complex operator *(Complex c1, Complex c2)
        {
            // (a+bi)(c+di) = (ac - bd) + (ad + bc)i
            double realPart = c1.Real * c2.Real - c1.Imaginary * c2.Imaginary;
            double imagPart = c1.Real * c2.Imaginary + c1.Imaginary * c2.Real;
            return new Complex(realPart, imagPart);
        }

        // Overload لعامل المساواة == (يجب عمل != أيضًا)
        public static bool operator ==(Complex c1, Complex c2)
        {
            // لو الاتنين null
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return true;
            // لو واحد null
            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
                return false;

            return c1.Real == c2.Real && c1.Imaginary == c2.Imaginary;
        }

        // Overload لعامل عدم المساواة !=
        public static bool operator !=(Complex c1, Complex c2)
        {
            return !(c1 == c2);
        }

        // Override ToString عشان نعرض العدد المركب بشكل مناسب
        public override string ToString()
        {
            if (Imaginary >= 0)
                return $"{Real} + {Imaginary}i";
            else
                return $"{Real} - {Math.Abs(Imaginary)}i";
        }

        // Override Equals عشان يتوافق مع == (إلزامي)
        public override bool Equals(object obj)
        {
            Complex other = obj as Complex;
            if (other == null) return false;
            return this == other;
        }

        // Override GetHashCode عشان يتوافق مع Equals
        public override int GetHashCode()
        {
            return Real.GetHashCode() ^ Imaginary.GetHashCode();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Complex c1 = new Complex(3, 4);
            Complex c2 = new Complex(1, 2);

            Complex sum = c1 + c2;
            Complex diff = c1 - c2;
            Complex product = c1 * c2;

            Console.WriteLine($"c1 = {c1}");               // 3 + 4i
            Console.WriteLine($"c2 = {c2}");               // 1 + 2i
            Console.WriteLine($"c1 + c2 = {sum}");         // 4 + 6i
            Console.WriteLine($"c1 - c2 = {diff}");        // 2 + 2i
            Console.WriteLine($"c1 * c2 = {product}");     // (3*1 - 4*2) + (3*2 + 4*1)i = (3-8) + (6+4)i = -5 + 10i

            // اختبار المساواة
            Complex c3 = new Complex(3, 4);
            Console.WriteLine($"c1 == c2 ? {c1 == c2}");   // False
            Console.WriteLine($"c1 == c3 ? {c1 == c3}");   // True
            Console.WriteLine($"c1 != c2 ? {c1 != c2}");   // True
        }
    }
}
```

**شرح الكود:**

- `Complex` فيه Real و Imaginary.
- `operator +` بترجع Complex جديد بجمع المركبتين.
- `operator *` بتنفذ عملية ضرب الأعداد المركبة حسب القانون الرياضي.
- `operator ==` لازم نعملها زوج مع `!=`. هنا بنقارن Real و Imaginary.
- لاحظ إحنا عملنا override لـ `Equals` و `GetHashCode` عشان لما نستخدم `==` مع الكائنات، يشتري بشكل صحيح. (لو عملنا overload للـ `==`، لازم نعمل override لـ Equals و GetHashCode).
- في `Main`، جربنا كل العوامل.

**فوائد Overloading:**
- الكود بيبقى أكتر readable (زي `c1 + c2` بدل `c1.Add(c2)`).
- بنقدر نستخدم الكلاسات بتاعتنا بنفس سهولة الأنواع البدائية.

---

## 8. Object Class Methods

كل كلاس في C# بيرث من الكلاس الأساسي `System.Object`، وده فيه شوية دوال بنقدر نعملها override عشان نخصص السلوك.

### أهم دوال Object:
- `ToString()`: بترجع string تمثيل للكائن. افتراضيًا بترجع اسم النوع، لكن بنقدر نغيرها عشان تعرض بيانات مفيدة.
- `Equals(object obj)`: بتقارن الكائن الحالي مع كائن تاني. افتراضيًا بتقارن المراجع (للكلاسات) والقيم (لـ struct). بنقدر نغيرها عشان نعمل مقارنة منطقية.
- `GetHashCode()`: بترجع رقم (hash code) للكائن، بيستخدم في الـ collections زي Dictionary و HashSet. لازم تكون متوافقة مع `Equals`.
- `GetType()`: بترجع نوع الكائن (Type) في وقت التشغيل.
- `MemberwiseClone()`: protected method، بتعمل shallow copy للكائن الحالي.

#### مثال كامل على Override لـ ToString, Equals, GetHashCode

```csharp
using System;
using System.Collections.Generic;

namespace ObjectMethodsExample
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        // override ToString
        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}";
        }

        // override Equals
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Person))
                return false;

            Person other = (Person)obj;
            return this.Name == other.Name && this.Age == other.Age;
        }

        // override GetHashCode
        public override int GetHashCode()
        {
            // استخدام HashCode.Combine (موجود في .NET Standard 2.1+)
            // أو ندمج يدويًا
            return HashCode.Combine(Name, Age);
            // طريقة يدوية قديمة:
            // return (Name?.GetHashCode() ?? 0) ^ Age.GetHashCode();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person p1 = new Person("Ibrahim", 25);
            Person p2 = new Person("Ibrahim", 25);
            Person p3 = new Person("Ahmed", 30);

            Console.WriteLine(p1.ToString());  // Name: Ibrahim, Age: 25

            Console.WriteLine($"p1 Equals p2? {p1.Equals(p2)}"); // True (لأننا override)
            Console.WriteLine($"p1 == p2? {p1 == p2}"); // False (لأن الـ == افتراضيًا بتقارن المرجع، ما لم نعمل overload)

            // استخدام Dictionary مع Person
            var dict = new Dictionary<Person, string>();
            dict[p1] = "Employee";
            Console.WriteLine(dict.ContainsKey(p2)); // True لو GetHashCode و Equals متوافقين
        }
    }
}
```

**شرح الكود:**

- `ToString()` عرفناها عشان تطبع الاسم والعمر بدل اسم النوع.
- `Equals()` عرفناها عشان تقارن بناءًا على الاسم والعمر مش على المرجع.
- `GetHashCode()` عرفناها باستخدام `HashCode.Combine` عشان تنتج نفس الـ hash code لكائنين متساويين (شرط أساسي).
- في `Main`، `p1.Equals(p2)` بترجع true لأن البيانات واحدة.
- لكن `p1 == p2` بترجع false لأننا ما عملناش overload للـ `==`. لو عايزينها تشتغل زى `Equals` لازم نعمل overload للـ `==` و `!=` زي ما عملنا في Complex.

**ملاحظة مهمة:** أي class تستخدمه كمفتاح في Dictionary أو HashSet لازم يكون عنده `Equals` و `GetHashCode` متوافقين. يعني إذا كان `Equals` يقولوا إن كائنين متساويين، `GetHashCode` لازم يرجع نفس الرقم.

---

## 9. الخلاصة

في الشرح ده، غطينا أهم مفاهيم OOP في C# اللي موجودة في الريبو بتاعك:

- **Encapsulation**: إخفاء البيانات باستخدام properties والتحكم فيها.
- **Constructors**: أنواعهم المختلفة وازاي نستخدمهم لتهيئة الكائنات.
- **Struct vs Class**: الفرق بين القيمة والمرجع، وأداء كل واحد.
- **Polymorphism**: overloading (compile-time) و overriding (runtime) مع شرح virtual, override, new.
- **Interface**: كعقد (contract) وازاي بيساعد في الـ polymorphism وتوحيد المعالجة.
- **Built-in Interfaces**: ICloneable للنسخ العميق، IComparable للترتيب.
- **Operator Overloading**: إعطاء معنى جديد للعوامل زي + و - للكلاسات المخصصة.
- **Object Class Methods**: ToString, Equals, GetHashCode, ودورهم.