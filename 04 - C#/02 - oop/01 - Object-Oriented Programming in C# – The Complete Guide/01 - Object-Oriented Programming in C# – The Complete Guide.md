# شرح OOP

## مقدمة: إيه هي الـ OOP وليه بنستخدمها في الباك إند؟

الـ OOP هي طريقة لتنظيم الكود بتقسيمه إلى **كائنات** (Objects) كل كائن عبارة عن **صفات** (Attributes) و **سلوكيات** (Methods). في الباك إند، بنستخدم OOP عشان نموذج **العالم الحقيقي** داخل الكود. مثال:

- عندنا **موظف** (Employee): ليه صفات زي الاسم، المرتب، القسم. وليه سلوكيات زي احتساب الراتب، تقديم إجازة.
- عندنا **منتج** (Product) في متجر إلكتروني: ليه سعر، كمية، وفيه سلوك زي خصم الكمية من المخزون.

باستخدام OOP، الكود بيكون **منظم**، **قابل للتوسع**، و**سهل الصيانة**. لو جه مدير جديد وطلب إضافة ميزة، تقدر تعدل في جزء صغير من غير ما تهد النظام كله.

---

## 1. الكلاس (Class) والكائن (Object)

الكلاس هو **القالب** أو **الرسم الهندسي**، والكائن هو **المنتج النهائي** اللي اتصنع من القالب ده.

**مثال واقعي:** عندنا كلاس اسمه `Car` (القالب)، ومنه بنصنع كائنات عربيات حقيقية: `myCar`، `yourCar`. كل عربية ليها لون وسنة وموديل مختلف.

### الكود:
```csharp
// تعريف الكلاس (القالب)
public class Car
{
    // الصفات (Attributes) - هنخليها private عشان التغليف (Encapsulation)
    private string model;
    private int year;
    private string color;

    // Constructor (الطريقة اللي بنصنع بيها الكائن)
    public Car(string model, int year, string color)
    {
        this.model = model;
        this.year = year;
        this.color = color;
    }

    // سلوك (Method) لعرض معلومات العربية
    public void DisplayInfo()
    {
        Console.WriteLine($"موديل: {model}, سنة: {year}, لون: {color}");
    }
}

// في الـ Main (مكان الاستخدام)
class Program
{
    static void Main()
    {
        // صنع كائن من الكلاس Car (كائن حقيقي)
        Car myCar = new Car("BMW", 2022, "أسود");
        myCar.DisplayInfo(); // المخرجات: موديل: BMW, سنة: 2022, لون: أسود
    }
}
```

---

## 2. التغليف (Encapsulation) – حماية البيانات 🛡️

التغليف معناه إننا **نخبئ التفاصيل الداخلية** للكلاس (الحقول) و**نسمح بالوصول** ليها من خلال وسائل متحكم فيها (Properties أو Methods). ده بيمنع أي تعديل عشوائي على البيانات ويسمح بعمل **تحقق (validation)**.

### مثال من تطبيق باك إند: إدارة الموظفين
عندنا موظف، ومينفعش أي حد يغير مرتبه براحته. لازم يكون فيه قواعد، مثلاً المرتب مايزيدش عن 10000.

### الكود باستخدام **Properties** (أفضل وسيلة للتغليف):
```csharp
public class Employee
{
    // حقول خاصة (private) – محدش يشوفها من بره
    private int id;
    private string name;
    private decimal salary;

    // Constructor
    public Employee(int id, string name, decimal salary)
    {
        this.id = id;
        this.name = name;
        Salary = salary; // استخدم الـ Property مش الحقل عشان الـ validation يشتغل
    }

    // Property for Id (قراءة فقط – ماينفعش يتغير بعد ما اتعمل)
    public int Id
    {
        get { return id; }
    }

    // Property for Name (مع validation)
    public string Name
    {
        get { return name; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                name = value;
            else
                throw new ArgumentException("الاسم لا يمكن أن يكون فارغًا");
        }
    }

    // Property for Salary (مع validation)
    public decimal Salary
    {
        get { return salary; }
        set
        {
            if (value >= 3000 && value <= 10000)
                salary = value;
            else
                throw new ArgumentOutOfRangeException("المرتب يجب أن يكون بين 3000 و 10000");
        }
    }

    // طريقة لعرض البيانات
    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Salary: {Salary:C}";
    }
}
```

### ملحوظة:
- الـ `Id` فيه `get` فقط، يبقى **read-only** (ماينفعش نغيره بعد ما اتعمل الكائن).
- الـ `Name` و `Salary` فيهم `set` مع validation.

### استخدام الكلاس في Main:
```csharp
try
{
    Employee emp = new Employee(1, "أحمد", 5000);
    Console.WriteLine(emp);

    // emp.Id = 2; // خطأ: readonly property
    emp.Salary = 12000; // خطأ: validation هيرمى Exception
}
catch (Exception ex)
{
    Console.WriteLine($"خطأ: {ex.Message}");
}
```

### إيه الفرق بين الـ Property والـ Field العادي؟
- الـ Field هو مجرد متغير (public int age;).
- الـ Property زي واجهة (interface) للـ field، تقدر تتحكم في الـ get و الـ set وتضيف منطق.

---

## 3. أنواع الـ Properties

### 3.1. Full Property (خاصية كاملة)
زي المثال اللي فوق، بنكتب الـ backing field والـ get, set.

### 3.2. Auto-Implemented Property (خاصية مختصرة)
لو مش محتاج validation، الـ compiler هو اللي يخلق الـ backing field.

```csharp
public string Address { get; set; } = "غير محدد"; // Initial value ممكن نضيفها
```

### 3.3. Read-only Property (للقراءة فقط)
```csharp
public decimal Tax
{
    get { return Salary * 0.1m; } // مفيش set، يعني مش هنقدر نغير الضريبة من بره
}
```

---

## 4. الـ Constructors – الورشة اللي بتصنع الكائن 🏗️

الـ Constructor هو method بيتنفذ أول لما نعمل `new` من الكلاس. بيستخدم عشان نحدد القيم الابتدائية.

### أنواع الـ Constructors:

#### 4.1. Parameterless Constructor (افتراضي)
لو منكتبش constructor، الـ compiler بيزود واحد **بدون parameters** بيفضى الحقول بالقيم الافتراضية (0, null, false).

#### 4.2. Parameterized Constructor (بمعاملات)
اللي بياخد بيانات عشان يملأ الحقول.

#### 4.3. Copy Constructor
بياخد كائن من نفس النوع ويعمل نسخة منه.

#### 4.4. Static Constructor
بيتنفذ مرة واحدة بس قبل أول استخدام للكلاس (عشان تهيئة أعضاء static).

### مثال على كل الأنواع:
```csharp
public class Product
{
    private int code;
    private string name;
    private decimal price;
    private static int totalProducts; // static field

    // Static Constructor (يتنفذ مرة واحدة)
    static Product()
    {
        totalProducts = 0;
        Console.WriteLine("Static Constructor: تم تهيئة عداد المنتجات");
    }

    // Parameterized Constructor
    public Product(int code, string name, decimal price)
    {
        this.code = code;
        this.name = name;
        this.price = price;
        totalProducts++; // نزيد العداد مع كل منتج جديد
    }

    // Copy Constructor (ينشئ منتج جديد من منتج موجود)
    public Product(Product other)
    {
        this.code = other.code;
        this.name = other.name;
        this.price = other.price;
        totalProducts++; // نعتبره منتج جديد برضه
    }

    // Read-only property لقراءة totalProducts
    public static int TotalProducts => totalProducts;

    public override string ToString()
    {
        return $"Code: {code}, Name: {name}, Price: {price:C}";
    }
}

// الاستخدام
class Program
{
    static void Main()
    {
        Product p1 = new Product(101, "لابتوب", 15000m);
        Console.WriteLine(p1);
        Console.WriteLine($"إجمالي المنتجات: {Product.TotalProducts}");

        Product p2 = new Product(p1); // استخدام copy constructor
        Console.WriteLine(p2);
        Console.WriteLine($"إجمالي المنتجات: {Product.TotalProducts}");
    }
}
```

### الـ Constructor Chaining (تسلسل الـ Constructors)
استخدام `this()` عشان نستدعي constructor تاني من نفس الكلاس، و`base()` عشان نستدعي constructor من الأب. ده بيمنع تكرار الكود.

```csharp
public class Product
{
    public Product(int code, string name) : this(code, name, 0) { }
    public Product(int code, string name, decimal price) { /* الكود الرئيسي */ }
}
```

---

## 5. الفرق بين الـ Class والـ Struct 🆚

الـ Struct هو نوع قيمة (Value Type) زي الـ int والـ bool، أما الـ Class فهو نوع مرجع (Reference Type).

### متى نستخدم Struct؟
- الكائن صغير الحجم (مثلاً Point, Color).
- البيانات غير قابلة للتعديل (immutable) غالباً.
- العدد المتوقع كبير جداً (عشان الأداء – لأن الـ Struct بيتخزن في Stack أسرع).

### متى نستخدم Class؟
- الكائن كبير (زي Employee, Order).
- محتاج وراثة.
- محتاج تمرير بالمرجع (Reference) عشان التعديلات تنعكس في كل مكان.

### مثال على Struct:
```csharp
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Display() => Console.WriteLine($"({X}, {Y})");
}

// الاستخدام:
Point p1 = new Point(10, 20);
Point p2 = p1; // هنا بنسخ القيم (Copy by value)
p2.X = 100;
Console.WriteLine(p1.X); // لسه 10، لأن p2 نسخة مستقلة
```

---

## 6. الـ Indexer – خلي الكائن يتصرف زي Array 📇

الـ Indexer هو property خاصة بتخلينا نستخدم الكائن نفسه مع أقواس `[]` عشان نوصله كأنه array. مفيدة جداً في الكلاسات اللي بتخزن مجموعات زي PhoneBook.

### مثال واقعي: PhoneBook (دليل التليفونات)
عايزين نستخدم `phoneBook["Ahmed"]` عشان نجيب رقم أحمد، أو نحدثه بـ `phoneBook["Ahmed"] = 123456`.

```csharp
public class PhoneBook
{
    private string[] names;
    private long[] numbers;
    private int size;

    public PhoneBook(int capacity)
    {
        names = new string[capacity];
        numbers = new long[capacity];
        size = 0;
    }

    // Indexer بالاسم (string)
    public long this[string name]
    {
        get
        {
            for (int i = 0; i < size; i++)
            {
                if (names[i] == name)
                    return numbers[i];
            }
            throw new KeyNotFoundException($"الاسم '{name}' غير موجود");
        }
        set
        {
            for (int i = 0; i < size; i++)
            {
                if (names[i] == name)
                {
                    numbers[i] = value;
                    return;
                }
            }
            // لو الاسم مش موجود، نضيفه (اختياري)
            if (size < names.Length)
            {
                names[size] = name;
                numbers[size] = value;
                size++;
            }
            else
                throw new Exception("الدفتر ممتلئ");
        }
    }

    // Indexer بالرقم (long) عشان نجيب الاسم
    public string this[long number]
    {
        get
        {
            for (int i = 0; i < size; i++)
            {
                if (numbers[i] == number)
                    return names[i];
            }
            throw new KeyNotFoundException("الرقم غير موجود");
        }
    }

    // طريقة لعرض الكل
    public void DisplayAll()
    {
        for (int i = 0; i < size; i++)
        {
            Console.WriteLine($"{names[i]}: {numbers[i]}");
        }
    }
}

// الاستخدام
PhoneBook book = new PhoneBook(5);
book["Ahmed"] = 123456789;   // استخدام setter في indexer
book["Sara"] = 987654321;
Console.WriteLine(book["Ahmed"]); // 123456789
Console.WriteLine(book[987654321]); // Sara
book.DisplayAll();
```

---

## 7. الوراثة (Inheritance) – خللي كلاس يورث من كلاس تاني 👨‍👦

في الباك إند، بنستخدم الوراثة عشان نعمل **كلاسات عامة** و**كلاسات متخصصة**. مثال: عندنا كلاس أساسي اسمه `User`، ومنه نعمل `Admin` و `Customer`.

### مثال: نظام المستخدمين
```csharp
// Parent Class (Base Class)
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public User(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public virtual void DisplayInfo() // virtual عشان نقدر نعمل override في child
    {
        Console.WriteLine($"User: {Name}, Email: {Email}");
    }
}

// Child Class (Derived Class) – Admin
public class Admin : User
{
    public string Role { get; set; } // خاصية إضافية للأدمن

    public Admin(int id, string name, string email, string role) : base(id, name, email)
    {
        Role = role;
    }

    // override للـ DisplayInfo عشان نضيف الدور
    public override void DisplayInfo()
    {
        base.DisplayInfo(); // نستدعي method الأب عشان نطبع البيانات الأساسية
        Console.WriteLine($"Role: {Role} (Admin)");
    }

    public void ManageUsers()
    {
        Console.WriteLine("Admin is managing users...");
    }
}

// Child Class – Customer
public class Customer : User
{
    public DateTime RegistrationDate { get; set; }

    public Customer(int id, string name, string email, DateTime regDate) : base(id, name, email)
    {
        RegistrationDate = regDate;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Registered on: {RegistrationDate.ToShortDateString()}");
    }
}

// الاستخدام
class Program
{
    static void Main()
    {
        Admin admin = new Admin(1, "محمد", "mohamed@site.com", "SuperAdmin");
        Customer customer = new Customer(2, "فاطمة", "fatma@site.com", DateTime.Now);

        admin.DisplayInfo();
        admin.ManageUsers();

        customer.DisplayInfo();

        // Polymorphism: ممكن نخزنهم في قائمة من النوع User
        List<User> users = new List<User> { admin, customer };
        foreach (var user in users)
        {
            user.DisplayInfo(); // كل واحد هيشتغل الـ override بتاعه
        }
    }
}
```

### الـ base keyword:
- بنستخدم `base()` في الـ constructor عشان ننادي constructor الأب.
- بنستخدم `base.MethodName()` عشان ننادي method الأب من جوه الـ child (زي ما عملنا في `DisplayInfo`).

---

## 8. تعدد الأشكال (Polymorphism) – الحاجة الواحدة بأشكال مختلفة 🎭

في نوعين أساسيين:

### 8.1. Overloading – نفس الاسم، معاملات مختلفة
مثال: آلة حاسبة فيها أكتر من `Sum`.

```csharp
public class Calculator
{
    public int Sum(int a, int b) => a + b;
    public int Sum(int a, int b, int c) => a + b + c;
    public double Sum(double a, double b) => a + b;
}
```

### 8.2. Overriding – إعادة تعريف method موجودة في الأب
لازم الـ method في الأب تكون `virtual` أو `abstract`، وفي الابن نستخدم `override`.

### الفرق بين `override` و `new`
- **override:** بيعيد تعريف الـ method الحقيقية، والكائن اللي type الأب بيشاور على ابن هيستخدم method الابن (runtime polymorphism).
- **new:** بيعمل method جديدة بتخفي method الأب، لكن لو استخدمت type الأب هتشتغل method الأب.

مثال يوضح الفرق:
```csharp
public class Parent
{
    public virtual void Show() => Console.WriteLine("Parent Show");
}

public class Child1 : Parent
{
    public override void Show() => Console.WriteLine("Child1 Show (override)");
}

public class Child2 : Parent
{
    public new void Show() => Console.WriteLine("Child2 Show (new)");
}

// الاستخدام
Parent p1 = new Child1();
Parent p2 = new Child2();
p1.Show(); // Child1 Show (override)
p2.Show(); // Parent Show (new)
```

---

## 9. الـ Sealed Class – كلاس مقفول 🔒

بتمنع أي كلاس تاني يورث منه. استخداماتها: أمان، أداء (بعض التحسينات).

```csharp
public sealed class Configuration
{
    public string ConnectionString { get; set; }
}

// class MyConfig : Configuration { } // خطأ مش هينفع
```

---

## 10. الـ Abstract Class vs Interface – العقد والمخططات 📜

### Abstract Class
- كلاس **مش هنقدر نعمل منه object** (new).
- ممكن يحتوي على methods فيها كود (implementation) وممكن methods بدون كود (abstract).
- بنستخدمه لما نحتاج نشارك كود مشترك بين كلاسيز مع إجبارهم على تنفيذ حاجات معينة.

### Interface
- هو مجرد **تعاقد** (contract) بيحدد methods و properties لازم الكلاس اللي بيطبقه ينفذها (كلها abstract).
- مفيش فيها implementation (قبل C# 8).
- الكلاس الواحد يقدر يطبق أكتر من Interface (بديل للوراثة المتعددة).

### مثال: نظام دفع فيه PaymentMethod
```csharp
// Interface بيحدد العمليات الأساسية لأي طريقة دفع
public interface IPayment
{
    void ProcessPayment(decimal amount);
    string GetPaymentDetails();
}

// Abstract class للدفع الإلكتروني (فيها common code)
public abstract class ElectronicPayment : IPayment
{
    public string TransactionId { get; set; }

    public abstract void ProcessPayment(decimal amount); // لسه مش عارفين التفاصيل

    public string GetPaymentDetails()
    {
        return $"Transaction ID: {TransactionId}";
    }
}

// كلاس للفيزا (بيقدر ينفذ الـ abstract method)
public class VisaPayment : ElectronicPayment
{
    public string CardNumber { get; set; }

    public override void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing Visa payment of {amount:C} using card {CardNumber}");
        TransactionId = Guid.NewGuid().ToString();
    }
}

// كلاس للكاش (بيطبق الـ interface مباشرة)
public class CashPayment : IPayment
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Cash payment of {amount:C} received.");
    }

    public string GetPaymentDetails()
    {
        return "Cash payment - no transaction ID";
    }
}
```

---

## 11. Override for Equals, GetHashCode, and Operators == !=

في الباك إند، بنحتاج نقارن الكائنات حسب **القيمة** مش المرجع (العنوان في الذاكرة). مثلاً عايزين نشوف إذا كان موظفين ليهم نفس البيانات (الرقم القومي) يعتبروا واحد.

### ليه نعمل Override؟
- عشان نقارن الكائنات في `List.Contains` أو `Dictionary` كمفتاح.
- عشان الـ `==` يعمل مقارنة قيمة مش مرجع.

### خطوات Override صح:

#### 1. Override `Equals(object obj)`
#### 2. Override `GetHashCode()` (لازم)
#### 3. Overload `==` و `!=`

### مثال: كلاس Person
```csharp
public class Person
{
    public int NationalId { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }

    // 1. Equals
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        Person other = (Person)obj;
        return this.NationalId == other.NationalId; // نفرض إن الرقم القومي هو المعرف الفريد
    }

    // 2. GetHashCode (لازم يكون متوافق مع Equals)
    public override int GetHashCode()
    {
        return NationalId.GetHashCode(); // استخدام الحقل نفسه المستخدم في المقارنة
    }

    // 3. == operator
    public static bool operator ==(Person left, Person right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            return true;
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(Person left, Person right)
    {
        return !(left == right);
    }
}
```

### استخدام Equals و ==
```csharp
Person p1 = new Person { NationalId = 123, Name = "Ahmed" };
Person p2 = new Person { NationalId = 123, Name = "Ahmed" };
Person p3 = new Person { NationalId = 456, Name = "Sara" };

Console.WriteLine(p1 == p2); // True (بعد override)
Console.WriteLine(p1.Equals(p2)); // True

// استخدام في Dictionary
Dictionary<Person, string> dict = new Dictionary<Person, string>();
dict[p1] = "موجود";
Console.WriteLine(dict.ContainsKey(p2)); // True (لأنهم متساويين في GetHashCode)
```

### ملاحظات مهمة:
- لو عملت override لـ `Equals`، لازم تعمل override لـ `GetHashCode`، وإلا الـ collections اللي بتستخدم hash table ممكن تتصرف بشكل غلط.
- في الـ structs، الـ `ValueType.Equals` الافتراضي بيستخدم reflection (بطيء)، فالأفضل تعمل override.
- دايماً لما تعمل `==` overload، اعمل `!=` كمان.

---

## 12. Boxing and Unboxing – تحويلات القيمة والمرجع 📦

- **Boxing:** تحويل value type (int, struct) إلى reference type (object).
- **Unboxing:** العكس.

### مثال:
```csharp
int x = 10;
object obj = x;       // Boxing (x اتحطت في object)
int y = (int)obj;     // Unboxing (استرجاع القيمة)
```

### تأثير الأداء:
Boxing/Unboxing بيعملوا نسخ ويستهلكوا وقت. في التطبيقات الكبيرة (مليون عملية) الفرق كبير. حاول تتجنبهم باستخدام generics.

---

## 13. مقدمة عن Dependency Injection (DI) 💉

في الباك إند، بنستخدم DI عشان نقلل الـ coupling بين الكلاسات. بدل ما كلاس يعمل `new` من كلاس تاني، بنحط الكلاس التاني في الـ constructor كمعامل.

### مثال: خدمة البريد الإلكتروني

**من غير DI (اقتران شديد):**
```csharp
public class EmailService
{
    public void SendEmail(string to, string subject)
    {
        Console.WriteLine($"Sending email to {to}");
    }
}

public class OrderProcessor
{
    private EmailService _emailService = new EmailService(); // مرتبط بـ EmailService بقوة

    public void ProcessOrder(int orderId)
    {
        // ... logic
        _emailService.SendEmail("customer@site.com", "Order processed");
    }
}
```

**مع DI (اقتران ضعيف):**
```csharp
public interface IEmailService
{
    void SendEmail(string to, string subject);
}

public class EmailService : IEmailService
{
    public void SendEmail(string to, string subject)
    {
        Console.WriteLine($"Sending email to {to}");
    }
}

public class OrderProcessor
{
    private readonly IEmailService _emailService;

    public OrderProcessor(IEmailService emailService) // DI via constructor
    {
        _emailService = emailService;
    }

    public void ProcessOrder(int orderId)
    {
        // ...
        _emailService.SendEmail("customer@site.com", "Order processed");
    }
}
```

الفوائد:
- سهولة اختبار (Unit Testing) باستخدام Mock.
- سهولة تغيير التطبيق (لو عايز تغير EmailService لـ SmtpService).

---

## 14. أسئلة نظرية وعملية (بالحلول) 🧪

### س 1 (نظري): إيه الفرق بين الـ abstract class والـ interface؟ ومتى نستخدم كل واحد؟

**الحل:**
- **Abstract class:** تستخدم لما فيه **كود مشترك** بين الكلاسات ومحتاج تشاركه (مثلاً كلاسات Payment فيها TransactionId). وفيه methods ممكن تكون implemented وجاهزة.
- **Interface:** تستخدم لما عايز تحدد **عقد** فقط، بدون أي تنفيذ. وبنستخدمها عشان نضمن إن الكلاسات المختلفة تقدم نفس الخدمة (مثلاً كل طرق الدفع ليها ProcessPayment). كمان الكلاس الواحد يقدر يطبق أكتر من interface.

### س 2 (عملي): الكود ده فيه خطأ، حدده وصححه.
```csharp
public struct Point
{
    public int X;
    public int Y;
    public Point(int x, int y)
    {
        X = x;
    }
}
```

**الحل:**
- الخطأ: الـ constructor مش بيحدد قيمة لـ `Y`، والـ struct لازم **كل الحقول تتعين** في constructor قبل الخروج.
- التصحيح: نضيف `Y = y;` أو نحط قيمة افتراضية.
```csharp
public Point(int x, int y)
{
    X = x;
    Y = y;
}
```

### س 3 (نظري): إيه الفرق بين `override` و `new` في الوراثة؟

**الحل:**
- `override` بيعيد تعريف method موجودة في الأب (اللي معمولها virtual). لو استخدمت polymorphism (object من نوع الأب بيمثل ابن)، هتتنفذ method الابن.
- `new` بيعمل method جديدة تخفي method الأب. لو استخدمت polymorphism، هتتنفذ method الأب. `new` بتستخدم عشان تتعامل مع method مش virtual عايز تعرفها من جديد.

### س 4 (عملي): أكمل الكود عشان يحسب عدد الكائنات التي تم إنشاؤها من كلاس `Order`.
```csharp
public class Order
{
    // هنا اكتب الكود المطلوب
    private static int _count = 0;

    public Order()
    {
        // اكتب هنا
        _count++;
    }

    public static int GetCount() => _count;
}
```

**الحل:**
نحتاج static field يزيد في الـ constructor. الكود مكتمل.

### س 5 (نظري): ليه لما نعمل override لـ `Equals` لازم نعمل override لـ `GetHashCode`؟

**الحل:**
لأن بعض الكوليكشنز زي `Dictionary` و `HashSet` بتستخدم `GetHashCode` أولاً عشان تبحث بسرعة. لو objectين متساويين (`Equals` true) وليهم hash codes مختلفة، الكوليكشن مش هتلاقيهم صح. لازم نضمن إن `GetHashCode` يرجع نفس القيمة للمتساويين.

### س 6 (عملي): ما هي قيمة `z` بعد تنفيذ الكود التالي؟
```csharp
int x = 5;
object obj = x;
int y = (int)obj;
int z = y + 10;
```

**الحل:**
- `x` بتتحول لـ object (boxing).
- `obj` بيرجع لـ int (unboxing) في `y`.
- `y` = 5، إذن `z` = 15.
السؤال بيختبر فهم boxing/unboxing.

### س 7 (نظري): إيه الـ Constructor Chaining؟ أعط مثالاً.

**الحل:**
هو استدعاء constructor من constructor آخر في نفس الكلاس (باستخدام `this`) أو من الأب (باستخدام `base`). بيمنع تكرار الكود. مثال:
```csharp
public class Product
{
    public Product(int id) : this(id, "Unknown") { }
    public Product(int id, string name) { /* code */ }
}
```

### س 8 (عملي): اكتب Indexer لكلاس `Library` يسمح بالوصول للكتب عن طريق الـ ISBN (string).

**الحل:**
```csharp
public class Library
{
    private Dictionary<string, Book> books = new Dictionary<string, Book>();

    public Book this[string isbn]
    {
        get => books.ContainsKey(isbn) ? books[isbn] : null;
        set => books[isbn] = value;
    }
}
```

### س 9 (نظري): متى نستخدم `sealed` class؟

**الحل:**
- لمنع أي كلاس من الوراثة من هذا الكلاس (أمان).
- في بعض الحالات لتحسين الأداء (virtual methods في sealed classes ممكن الـ JIT يحسنها).
- إذا كان الكلاس مصمم بدون أي extensibility points.

### س 10 (عملي): صحح الكود التالي (يتعلق بالـ property validation).
```csharp
public class Account
{
    private decimal balance;
    public decimal Balance
    {
        get { return balance; }
        set { balance = value; }
    }
}
```
**الطلب:** نضيف validation تمنع الـ balance يكون أقل من صفر.

**الحل:**
```csharp
public decimal Balance
{
    get { return balance; }
    set
    {
        if (value < 0)
            throw new ArgumentException("الرصيد لا يمكن أن يكون أقل من صفر");
        balance = value;
    }
}
```

---

## خلاصة 🎯

الـ OOP هو الأساس لأي تطبيق باك إند قوي وقابل للتطوير. فهمت إزاي:
- تحمي البيانات (Encapsulation).
- تصمم كلاسات قابلة للتوسع (Inheritance).
- تخلي الكود مرن (Polymorphism).
- تخفي التعقيد (Abstraction).
- تكتب كود نظيف باستخدام Properties, Indexers, Constructors.
- تتعامل مع المقارنات بشكل صحيح (Equals, GetHashCode, ==).
- تفرق بين Class و Struct ومتى تستخدم كل واحد.
- تبدأ تفكر في Dependency Injection.