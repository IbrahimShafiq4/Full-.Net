# 🏗️ شرح معمق لمشروع الأحداث (Events) في C# - دليل شامل من Senior Architect

أهلاً بيك في الشرح المتكامل لمشروع `EventDemo`. هنتكلم هنا عن كل حاجة متعلقة بالأحداث (Events) والدليجيت (Delegates) في لغة C#. هنستخدم المثال بتاع الكورة (Ball) واللاعبين (Players) والحكم (Refree) عشان نفهم الفكرة بشكل عملي.

الهدف إننا نوصل للمستوى الفهمي العميق جداً، بحيث تقدر تمسك أي كود بيستخدم Events وتفهمه وتحلله وتكتبه بنفسك بإحترافية.

---

## 📚 فهرس المحتويات

- [🏗️ شرح معمق لمشروع الأحداث (Events) في C# - دليل شامل من Senior Architect](#️-شرح-معمق-لمشروع-الأحداث-events-في-c---دليل-شامل-من-senior-architect)
  - [📚 فهرس المحتويات](#-فهرس-المحتويات)
  - [1️⃣ ما هو الـ Delegate؟](#1️⃣-ما-هو-الـ-delegate)
    - [ليه بنستخدمه؟](#ليه-بنستخدمه)
    - [التشبيه](#التشبيه)
    - [مثال بسيط](#مثال-بسيط)
    - [شرح المثال](#شرح-المثال)
  - [2️⃣ الفرق بين Action, Func, Predicate](#2️⃣-الفرق-بين-action-func-predicate)
    - [مثال عملي](#مثال-عملي)
    - [ليه بنستخدمهم؟](#ليه-بنستخدمهم)
  - [3️⃣ ما هو الـ Event؟](#3️⃣-ما-هو-الـ-event)
    - [المشكلة اللي بيحلها](#المشكلة-اللي-بيحلها)
    - [Syntax](#syntax)
    - [الـ Multicast Delegate](#الـ-multicast-delegate)
  - [4️⃣ الفرق بين Delegate و Event](#4️⃣-الفرق-بين-delegate-و-event)
    - [مثال يوضح الفرق](#مثال-يوضح-الفرق)
  - [5️⃣ الـ Subscription والـ Unsubscription (+= و -=)](#5️⃣-الـ-subscription-والـ-unsubscription--و--)
    - [ليه لازم Unsubscribe؟](#ليه-لازم-unsubscribe)
    - [مثال](#مثال)
  - [6️⃣ إستدعاء الحدث (Invocation)](#6️⃣-إستدعاء-الحدث-invocation)
    - [شرح](#شرح)
    - [ليه بنستخدم `?.Invoke`؟](#ليه-بنستخدم-invoke)
  - [7️⃣ تشبيه واقعي (Real Life Analogy)](#7️⃣-تشبيه-واقعي-real-life-analogy)
  - [8️⃣ شرح الكود سطر سطر (Code Walkthrough)](#8️⃣-شرح-الكود-سطر-سطر-code-walkthrough)
    - [كلاس Ball](#كلاس-ball)
      - [شرح](#شرح-1)
    - [Struct Location](#struct-location)
      - [شرح](#شرح-2)
    - [كلاس Player و Refree](#كلاس-player-و-refree)
      - [شرح](#شرح-3)
    - [برنامج Program](#برنامج-program)
      - [شرح خطوة بخطوة](#شرح-خطوة-بخطوة)
  - [9️⃣ شرح الـ Property مع Trigger](#9️⃣-شرح-الـ-property-مع-trigger)
    - [ليه بنقارن value != ballLocation؟](#ليه-بنقارن-value--balllocation)
  - [1️⃣0️⃣ Operator Overloading في Location](#1️⃣0️⃣-operator-overloading-في-location)
    - [ليه عملنا overload لـ `==` و `!=`؟](#ليه-عملنا-overload-لـ--و-)
    - [إزاي شغالة؟](#إزاي-شغالة)
  - [1️⃣1️⃣ سلوك الـ Multicast Delegate](#1️⃣1️⃣-سلوك-الـ-multicast-delegate)
    - [ترتيب التنفيذ](#ترتيب-التنفيذ)
    - [لو حصل Exception](#لو-حصل-exception)
  - [1️⃣2️⃣ نمط EventHandler (الباترن الرسمي من مايكروسوفت)](#1️⃣2️⃣-نمط-eventhandler-الباترن-الرسمي-من-مايكروسوفت)
    - [إزاي نطبق الـ EventHandler؟](#إزاي-نطبق-الـ-eventhandler)
      - [الطريقة الأولى: استخدام `EventHandler<Location>`](#الطريقة-الأولى-استخدام-eventhandlerlocation)
      - [الطريقة الثانية: إنشاء كلاس EventArgs خاص](#الطريقة-الثانية-إنشاء-كلاس-eventargs-خاص)
    - [تعديل المشروع ليستخدم EventHandler](#تعديل-المشروع-ليستخدم-eventhandler)
  - [1️⃣3️⃣ مشروع جديد كلياً - نظام الطلبات (Order System)](#1️⃣3️⃣-مشروع-جديد-كلياً---نظام-الطلبات-order-system)
    - [الكود كامل](#الكود-كامل)
      - [OrderEventArgs](#ordereventargs)
      - [Order (Publisher)](#order-publisher)
      - [OrderItem (simple)](#orderitem-simple)
      - [EmailService (Subscriber)](#emailservice-subscriber)
      - [InventoryService (Subscriber)](#inventoryservice-subscriber)
      - [ShippingService (Subscriber)](#shippingservice-subscriber)
      - [Program (Testing)](#program-testing)
    - [شرح المشروع الجديد](#شرح-المشروع-الجديد)
  - [1️⃣4️⃣ أفضل الممارسات (Best Practices)](#1️⃣4️⃣-أفضل-الممارسات-best-practices)
  - [1️⃣5️⃣ الأخطاء الشائعة (Common Mistakes)](#1️⃣5️⃣-الأخطاء-الشائعة-common-mistakes)
  - [🎯 خلاصة](#-خلاصة)

---

## 1️⃣ ما هو الـ Delegate؟

الـ **Delegate** هو عبارة عن **Type-safe function pointer**. يعني هو نوع بيحدد توقيع دالة معينة (الباراميترات ونوع الرجوع)، وبعدين نقدر نستخدمه عشان نشاور على أي دالة عندها نفس التوقيع ده.

### ليه بنستخدمه؟
- عشان نقدر نمرر الدوال كـ Parameters (Callback).
- عشان نبني أنظمة Events.
- عشان نفصل بين الكود اللي بيطلب حاجة والكود اللي بينفذها.

### التشبيه
زي ما يكون عندك جهاز تحكم (Remote Control) تقدر تسجل عليه أزرار معينة. كل زر شغال على جهاز معين. الـ Delegate هو نوع الزر اللي بيحدد شكل التشغيل.

### مثال بسيط

```csharp
// تعريف Delegate
public delegate void Notify(string message);

// دالة مطابقة للتوقيع
public static void ShowMessage(string msg)
{
    Console.WriteLine(msg);
}

public static void Main()
{
    // إنشاء instance من الـ Delegate
    Notify del = ShowMessage;

    // استدعاء الدالة عن طريق الـ Delegate
    del("Hello from Delegate!");  // Hello from Delegate!
}
```

### شرح المثال
- عرفنا Delegate اسمه `Notify` بياخد `string` ويرجع `void`.
- الدالة `ShowMessage` مطابقة للتوقيع ده.
- أنشأنا متغير `del` من النوع `Notify` وربطناه بالدالة `ShowMessage`.
- استدعينا الدالة عن طريق `del(...)`.

---

## 2️⃣ الفرق بين Action, Func, Predicate

بدل ما نعرف Delegate مخصوص في كل مرة، C# بتوفر لنا Delegates جاهزة:

| النوع | الوصف | مثال |
|-------|-------|------|
| **Action** | بياخد parameters ويرجع `void` | `Action<int> a = PrintNumber;` |
| **Func** | بياخد parameters ويرجع قيمة (آخر type هو الرجوع) | `Func<int, int, string> f = SumToString;` |
| **Predicate** | بياخد parameter واحد ويرجع `bool` (زي Func<T, bool>) | `Predicate<int> isEven = x => x % 2 == 0;` |

### مثال عملي

```csharp
Action<string> print = Console.WriteLine;
print("Hello");

Func<int, int, int> add = (x, y) => x + y;
Console.WriteLine(add(3, 5));  // 8

Predicate<int> isPositive = x => x > 0;
Console.WriteLine(isPositive(10)); // True
```

### ليه بنستخدمهم؟
- تقليل كتابة الكود.
- توحيد المعايير.
- سهلين مع Lambda Expressions.

في مشروعنا استخدمنا `Action<Location>` بدل ما نعرف Delegate جديد.

---

## 3️⃣ ما هو الـ Event؟

الـ **Event** هو آلية في C# بتسمح للكلاس (Publisher) إنه ينبه كلاسات تانية (Subscribers) لما يحصل حاجة معينة.

### المشكلة اللي بيحلها
من غير Events، لو عايز كلاس يعرف كلاسات تانية بالتغيير، هتضطر إنه يمسك مراجع لكل الكلاسات دي ويكلمها بشكل مباشر. ده بيخلي الكود tightly coupled وصعب الصيانة. Events بتفصل الـ Publisher عن الـ Subscriber.

### Syntax

```csharp
public event Action<Location> OnLocationChanged;
```

- `event`: كلمة مفتاحية بتخلي الـ Delegate ده يتصرف كـ Event.
- `Action<Location>`: نوع الـ Delegate اللي هيتخزن فيه الدوال.
- `OnLocationChanged`: اسم الحدث.

### الـ Multicast Delegate
الـ Event في C# هو في الأساس Multicast Delegate، يعني يقدر يشير أكتر من دالة (زي ما بنشوف في الـ `+=`).

---

## 4️⃣ الفرق بين Delegate و Event

الفرق الجوهري هو **الـ Encapsulation** (التغليف).

| الخاصية | Delegate | Event |
|---------|----------|-------|
| **الاستدعاء** | ممكن تستدعي الـ Delegate من بره الكلاس (أي حتة) | مش ممكن تستدعي الحدث إلا من جوه الكلاس اللي معرف فيه (فقط الـ Publisher) |
| **التعيين** | تقدر تعمل `=` (Overwrite) | تقدر بس تعمل `+=` و `-=` (لا يمكنك overwrite) |
| **الأمان** | أقل أماناً (أي حد يقدر ينفذ الدوال المسجلة) | أكتر أماناً (الـ Publisher هو المسيطر على وقت التنفيذ) |

### مثال يوضح الفرق

```csharp
public class Test
{
    public Action<string> NotDelegate;
    public event Action<string> NotEvent;

    public void RaiseEvent()
    {
        NotEvent?.Invoke("Hi"); // مسموح جوه الكلاس
    }
}

// في بره الكلاس
Test t = new Test();
t.NotDelegate = Console.WriteLine; // مسموح
t.NotDelegate("Hello"); // مسموح

t.NotEvent = Console.WriteLine; // ❌ خطأ - مش مسموح
t.NotEvent += Console.WriteLine; // ✅ مسموح
t.NotEvent -= Console.WriteLine; // ✅ مسموح
t.NotEvent?.Invoke("Hi"); // ❌ خطأ - مش مسموح بره الكلاس
```

---

## 5️⃣ الـ Subscription والـ Unsubscription (+= و -=)

عشان أي Subscriber يسمع الحدث، لازم يعمل **Subscribe** باستخدام `+=`. ولما يبقى عاوز يوقف سماع، يعمل **Unsubscribe** باستخدام `-=`.

### ليه لازم Unsubscribe؟
لو نسيت تعمل Unsubscribe، الـ Publisher لسه شايل reference للـ Subscriber، وده بيمنع Garbage Collector إنه يمسح الـ Subscriber من الذاكرة، وبالتالي يحصل **Memory Leak**.

### مثال

```csharp
Player p = new Player() { Name = "Messi" };
ball.OnLocationChanged += p.Run;   // Subscribe
ball.OnLocationChanged -= p.Run;   // Unsubscribe
```

---

## 6️⃣ إستدعاء الحدث (Invocation)

في الكود شايفين السطر ده:

```csharp
OnLocationChanged?.Invoke(value);
```

### شرح
- `OnLocationChanged` هو الحدث نفسه.
- `?.` هو الـ null-conditional operator. معناه: لو الحدث مش null (أي فيه subscribers)، نفذ الـ `Invoke`. لو null (مفيش subscribers)، متعملش حاجة.
- `Invoke(value)` بتنادي كل الدوال المسجلة (الـ subscribers) وتمررلها `value` اللي هو موقع الكرة الجديد.

### ليه بنستخدم `?.Invoke`؟
عشان نتجنب `NullReferenceException` لو مفيش أي Subscriber اشترك في الحدث.

---

## 7️⃣ تشبيه واقعي (Real Life Analogy)

خلي بالك من التشبيه ده، هيفهمك الصورة الكاملة:

- **الكرة (Ball)** هي الـ Publisher. هي اللي بتتحرك، ولما تتحرك بتعمل حدث "أنا اتغير مكاني".
- **اللاعبين (Players)** و **الحكم (Refree)** هم الـ Subscribers. هما مهتمين بمكان الكرة.
- **الملعب** هو التطبيق.
- **الاشتراك (Subscription)** هو إن كل لاعب يقول للكرة: "أنا عاوز أعرف كل ما مكانك يتغير".
- **الحدث (Event)** هو الصوت اللي الكرة بتعمله لما تتحرك، وكل لاعب مسجل بيسمع الصوت ده ويروح يجري ناحيتها.

---

## 8️⃣ شرح الكود سطر سطر (Code Walkthrough)

هنشرح الكلاسات اللي في المشروع بالتفصيل الممل.

### كلاس Ball

```csharp
public class Ball
{
    public int Id { get; set; }

    private Location ballLocation;
    public Location BallLocation
    {
        get => ballLocation;
        set 
        {
            if (value != ballLocation)
            {
                ballLocation = value;
                OnLocationChanged?.Invoke(value);
            }
        }
    }

    public event Action<Location> OnLocationChanged;
    
    public override string ToString()
        => $"Id = {Id}, Location = {ballLocation}";
}
```

#### شرح
- `Id` مجرد رقم لتمييز الكرة.
- `ballLocation` حقل خاص (private) بيخزن الموقع الحالي للكرة.
- **الـ Property `BallLocation`**:
  - `get`: بيرجع القيمة المخزنة.
  - `set`: أول شيء بيتأكد إن القيمة الجديدة مختلفة عن القديمة (عن طريق `!=` اللي هنشرحها بعدين). لو مختلفة، يحدّث الحقل وبعدين يطلق الحدث `OnLocationChanged` مع القيمة الجديدة.
- **الحدث `OnLocationChanged`**: من نوع `Action<Location>`، يعني أي دالة بتاخد `Location` وترجع `void` تقدر تشترك فيه.
- **ToString**: عشان نطبع معلومات الكرة بشكل منسق.

### Struct Location

```csharp
public struct Location
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    
    public override string ToString()
        => $"Location: {X} :: {Y} :: {Z}";

    public static bool operator ==(Location left, Location right)
        => left.X == right.X &&
           left.Y == right.Y &&
           left.Z == right.Z;

    public static bool operator !=(Location left, Location right)
        => !(left == right);
}
```

#### شرح
- Location عبارة عن Struct (نوع قيمة) بيحتوي على 3 إحداثيات X, Y, Z.
- عملنا overload للمقارنة `==` و `!=` عشان نقارن بين موقعين بناءً على قيمهم (وليس المرجع). ده مهم جداً في الـ Property بتاعت Ball عشان نعرف إذا الموقع اتغير فعلاً ولا لأ.
- من غير overload دي، المقارنة `value != ballLocation` كانت هتقارن المراجع (ولو هما Structs فالمقارنة الافتراضية بترجع false لو فيهم fields مختلفة؟ في Struct الـ default behavior بيقارن كل field، لكن احنا فضلنا نكتب overload عشان نوضح ونتحكم).

### كلاس Player و Refree

```csharp
public class Player
{
    public string Name { get; set; }
    public string Team { get; set; }
    public override string ToString() => $"{Name}, {Team}";
    public void Run(Location location)
        => Console.WriteLine($"{this} is Runing towards {location}");
}

public class Refree
{
    public string Name { get; set; }
    public override string ToString() => $"Refree: {Name}";
    public void Look(Location location)
        => Console.WriteLine($"{this} is Looking At {location}");
}
```

#### شرح
- **Player**: عنده اسم وفريق. عنده دالة `Run` بتطبع رسالة إنه بيجري ناحية موقع معين. الدالة دي مطابقة لـ `Action<Location>`.
- **Refree**: عنده اسم، ودالة `Look` بتطبع إنه بيبص للمكان. برضه مطابقة لـ `Action<Location>`.
- كل واحد فيهم يقدر يشترك في حدث الكرة لأن عنده دالة بنفس التوقيع.

### برنامج Program

```csharp
internal class Program
{
    static void Main(string[] args)
    {
        Ball ball = new Ball();
        Player p01 = new Player() { Name = "Messi", Team = "Miami" };
        Player p02 = new Player() { Name = "Martinaze", Team = "Miami" };
        Player p03 = new Player() { Name = "Perdi", Team = "Barca" };
        Player p04 = new Player() { Name = "Gavi", Team = "Barca" };
        Refree refree = new Refree() { Name = "Ibrahim Nour Eldee" };

        Console.WriteLine("==================== Before Subscription ====================");
        // لحد دلوقتي محدش مشترك في الحدث
        ball.BallLocation = new Location() { X = 1, Y = 2, Z = 3 }; // مش هيعمل حاجة لأنه مفيش subscribers

        // الاشتراكات
        ball.OnLocationChanged += p01.Run;
        ball.OnLocationChanged += p02.Run;
        ball.OnLocationChanged += p03.Run;
        ball.OnLocationChanged += p04.Run;
        ball.OnLocationChanged += refree.Look;

        Console.WriteLine("==================== After Subscription ====================");
        ball.BallLocation = new Location() { X = 10, Y = 20, Z = 30 };
        
        Console.WriteLine("==================== The Ball Moved ====================");
        ball.BallLocation = new Location() { X = 10, Y = 10, Z = 30 };
        
        Console.WriteLine("==================== The Ball Moved ====================");
        ball.BallLocation = new Location() { X = 0, Y = 5, Z = 40 };
    }
}
```

#### شرح خطوة بخطوة

1. **إنشاء الكائنات**: بنعمل object من Ball، وأربعة لاعبين، وحكم.
2. **Before Subscription**: بنحاول نغير مكان الكرة. لكن بما أنه مفيش أي Subscriber اشترك في الحدث، الحدث هيبقى `null`، والـ `?.Invoke` مش هينفذ حاجة. مش هنشوف أي رسالة.
3. **Subscription**: بنضيف دوال اللاعبين والحكم للحدث باستخدام `+=`. دلوقتي الحدث عنده 5 دوال في قائمة الاستدعاء.
4. **After Subscription**: أول تغيير للمكان (`10,20,30`) هيشغل الحدث، وكل المشتركين هيتم استدعائهم. هنشوف رسائل من كل لاعب والحكم إنهم بيتفاعلوا.
5. **The Ball Moved**: تغيير ثاني (`10,10,30`) - الحدث هيشتغل تاني.
6. **تغيير تالت**: (`0,5,40`) - هيشتغل تاني.

---

## 9️⃣ شرح الـ Property مع Trigger

```csharp
set 
{
    if (value != ballLocation)
    {
        ballLocation = value;
        OnLocationChanged?.Invoke(value);
    }
}
```

### ليه بنقارن value != ballLocation؟
- عشان نتجنب إطلاق الحدث لو المكان متغيرش فعلياً. مثلاً لو حد ندى نفس القيمة تاني، مش هنعمل Invoke. ده بيفيد في الأداء ويمنع التكرار غير الضروري.
- المقارنة `!=` هنا استخدمت الـ overload اللي عملناه في Location عشان نقارن المحتوى مش المرجع.

---

## 1️⃣0️⃣ Operator Overloading في Location

### ليه عملنا overload لـ `==` و `!=`؟
- لأن Location هو Struct، والمقارنة الافتراضية في Struct بتقارن كل field على حدة. لكن لو عاوز نضمن إن المقارنة واضحة ومنطقية (خاصة إننا هنستخدمها كتير)، بنفضل نكتب overload.
- كمان لو Location كان Class (نوع مرجعي)، المقارنة الافتراضية `==` كانت هتقارن المراجع (العناوين) مش المحتوى، وكان لازم نعمل overload.

### إزاي شغالة؟
```csharp
public static bool operator ==(Location left, Location right)
    => left.X == right.X && left.Y == right.Y && left.Z == right.Z;
```
- بنقارن كل إحداثي على حدة. لو كلهم متساويين، يرجع true.

---

## 1️⃣1️⃣ سلوك الـ Multicast Delegate

لما تعمل `+=` أكتر من مرة، الـ Delegate بيشتغل كـ **Multicast**. يعني لما تنادي `OnLocationChanged?.Invoke(value)`، كل الدوال المسجلة بتتنفذ واحدة ورا التانية.

### ترتيب التنفيذ
الترتيب هو نفس ترتيب الإضافة. أول ما أضيف `p01.Run`، بعدين `p02.Run`، وهكذا. أول واحد هيشتغل هو `p01`.

### لو حصل Exception
لو وحدة من الدوال رمت Exception، التنفيذ بيقف وما بقيت الدوال بتتنفذش. عشان كده لازم نكون حريصين إن الـ subscribers يتعاملوا مع الأخطاء جوا دوالهم.

---

## 1️⃣2️⃣ نمط EventHandler (الباترن الرسمي من مايكروسوفت)

استخدام `Action<Location>` مباشرة مش غلط، لكن微软 (Microsoft) بتوصي باستخدام `EventHandler<TEventArgs>` للأسباب التالية:
- بيوفر extensibility: تقدر تضيف بيانات إضافية في المستقبل.
- بيتضمن `object sender` عشان الـ subscriber يعرف مين اللي بعث الحدث.
- متوافق مع الـ .NET Framework standards.

### إزاي نطبق الـ EventHandler؟

#### الطريقة الأولى: استخدام `EventHandler<Location>`
```csharp
public event EventHandler<Location> OnLocationChanged;
```
لكن لازم Location يكون من نوع EventArgs أو يرث منه. Location هنا struct، مش مشتقة من EventArgs. ينفع نستخدم EventHandler<Location> لو Location مش من EventArgs؟ عادي، لأن EventHandler<T> مش شرط T يكون EventArgs، لكن الأفضل نتبع convention.

#### الطريقة الثانية: إنشاء كلاس EventArgs خاص
```csharp
public class BallEventArgs : EventArgs
{
    public Location Location { get; set; }
    // ممكن تضيف properties تانية زي سرعة الكرة
}
```

وبعدين نعدل الحدث:
```csharp
public event EventHandler<BallEventArgs> OnLocationChanged;
```

والرفع:
```csharp
OnLocationChanged?.Invoke(this, new BallEventArgs { Location = value });
```

### تعديل المشروع ليستخدم EventHandler

هنعمل refactor للكود:

**كلاس BallEventArgs**
```csharp
public class BallEventArgs : EventArgs
{
    public Location Location { get; set; }
}
```

**كلاس Ball**
```csharp
public event EventHandler<BallEventArgs> OnLocationChanged;

private void RaiseLocationChanged(Location newLocation)
{
    OnLocationChanged?.Invoke(this, new BallEventArgs { Location = newLocation });
}
```

**في setter**:
```csharp
set 
{
    if (value != ballLocation)
    {
        ballLocation = value;
        RaiseLocationChanged(value);
    }
}
```

**الـ Subscribers** لازم تعدل دوالها عشان تستقبل (object sender, BallEventArgs e):
```csharp
public void Run(object sender, BallEventArgs e)
{
    Console.WriteLine($"{this} is Running towards {e.Location}");
}
```

وكذلك في Refree.

**الاشتراك**:
```csharp
ball.OnLocationChanged += p01.Run;
```

كده بقينا متوافقين مع الـ pattern الرسمي.

---

## 1️⃣3️⃣ مشروع جديد كلياً - نظام الطلبات (Order System)

هنطبق الـ Events في سيناريو مختلف: نظام تجارة إلكترونية. لما عميل يطلب طلب (Order)، يحصل أحداث:
- إرسال إيميل تأكيد للعميل.
- إشعار المخزن لتجهيز الطلب.
- إشعار نظام الشحن.

### الكود كامل

#### OrderEventArgs
```csharp
public class OrderEventArgs : EventArgs
{
    public int OrderId { get; set; }
    public string CustomerEmail { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
```

#### Order (Publisher)
```csharp
public class Order
{
    private static int _lastId = 0;
    public int OrderId { get; private set; }
    public string CustomerEmail { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.Price * i.Quantity);
    public DateTime OrderDate { get; private set; }

    public event EventHandler<OrderEventArgs> OrderPlaced;

    public Order()
    {
        OrderId = ++_lastId;
        OrderDate = DateTime.Now;
    }

    public void PlaceOrder()
    {
        Console.WriteLine($"Order #{OrderId} placed at {OrderDate}");
        // Trigger the event
        OnOrderPlaced();
    }

    protected virtual void OnOrderPlaced()
    {
        OrderPlaced?.Invoke(this, new OrderEventArgs
        {
            OrderId = this.OrderId,
            CustomerEmail = this.CustomerEmail,
            TotalAmount = this.TotalAmount,
            OrderDate = this.OrderDate
        });
    }
}
```

#### OrderItem (simple)
```csharp
public class OrderItem
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```

#### EmailService (Subscriber)
```csharp
public class EmailService
{
    public void SendConfirmation(object sender, OrderEventArgs e)
    {
        Console.WriteLine($"[EmailService] Sending confirmation email to {e.CustomerEmail} for order #{e.OrderId}, total: {e.TotalAmount:C}");
    }
}
```

#### InventoryService (Subscriber)
```csharp
public class InventoryService
{
    public void ReserveItems(object sender, OrderEventArgs e)
    {
        Console.WriteLine($"[InventoryService] Reserving items for order #{e.OrderId}");
    }
}
```

#### ShippingService (Subscriber)
```csharp
public class ShippingService
{
    public void PrepareShipment(object sender, OrderEventArgs e)
    {
        Console.WriteLine($"[ShippingService] Preparing shipment for order #{e.OrderId}");
    }
}
```

#### Program (Testing)
```csharp
class Program
{
    static void Main()
    {
        // Create publisher
        Order order = new Order
        {
            CustomerEmail = "customer@example.com",
            Items = new List<OrderItem>
            {
                new OrderItem { ProductName = "Laptop", Quantity = 1, Price = 1500m },
                new OrderItem { ProductName = "Mouse", Quantity = 2, Price = 25m }
            }
        };

        // Create subscribers
        EmailService email = new EmailService();
        InventoryService inventory = new InventoryService();
        ShippingService shipping = new ShippingService();

        // Subscribe
        order.OrderPlaced += email.SendConfirmation;
        order.OrderPlaced += inventory.ReserveItems;
        order.OrderPlaced += shipping.PrepareShipment;

        // Place order (this will trigger events)
        order.PlaceOrder();

        // Unsubscribe (optional)
        order.OrderPlaced -= email.SendConfirmation;

        // Try another order
        Order order2 = new Order { CustomerEmail = "another@example.com" };
        order2.OrderPlaced += email.SendConfirmation;
        order2.OrderPlaced += inventory.ReserveItems;
        order2.PlaceOrder();
    }
}
```

### شرح المشروع الجديد
- الـ `Order` هو Publisher، عنده حدث `OrderPlaced` من نوع `EventHandler<OrderEventArgs>`.
- كل Service (Email, Inventory, Shipping) هو Subscriber، وكل واحد عنده دالة بنفس التوقيع `(object sender, OrderEventArgs e)`.
- لما ننادي `PlaceOrder`، بنشغل `OnOrderPlaced` اللي بدوره يشغل الحدث، وكل service يستجيب.

السيناريو ده واقعي جداً، وبيوضح قوة الـ Events في فصل المسؤوليات.

---

## 1️⃣4️⃣ أفضل الممارسات (Best Practices)

1. **استخدم EventHandler<T> بدل Action** عشان توفر الـ sender وتكون متوافق مع المكتبات.
2. **سمِّ الأحداث بإسم بيدل على الحدث وبلاحقة `EventHandler` لو كان نوعه، أو `On...` للطريقة المحمية**. مثال: `OrderPlaced` للحدث، `OnOrderPlaced` للطريقة اللي بتشغله.
3. **دايماً اعمل null check قبل الـ Invoke** (استخدم `?.Invoke`).
4. **Unsubscribe من الأحداث** (خصوصاً لو الـ Subscriber هيبقى活得 أطول من الـ Publisher، أو العكس، عشان تمنع memory leaks).
5. **خلي الـ EventArgs immutable** (يعني properties قابلة للقراءة فقط) لو أمكن، عشان الـ subscribers ما يقدروش يغيروا البيانات.
6. **استخدم `protected virtual void On...`** عشان تسمح للكلاسات الابنة إنها تoverride سلوك رفع الحدث.
7. **لا تستخدم الأحداث إلا لو فيه اتصال واحد لأكثر من كائن**، ولو العلاقة one-to-one ممكن تستخدم Interface أو callback مباشر.

---

## 1️⃣5️⃣ الأخطاء الشائعة (Common Mistakes)

1. **نسيان null check**:
   ```csharp
   // خطأ
   OnLocationChanged.Invoke(value); // ممكن يرمي NullReferenceException لو مفيش subscribers
   // صح
   OnLocationChanged?.Invoke(value);
   ```

2. **عدم unsubscription**:
   لو Subscriber object مفروض يتجمع من الذاكرة، ولسه subscribed في حدث، الـ GC مش هيقدر يجمعه لأن الـ Publisher لسه شايل reference له. ده يؤدي لـ Memory Leak.

3. **استخدام `=` مع الحدث بره الكلاس**:
   ```csharp
   ball.OnLocationChanged = p01.Run; // ❌ خطأ (لأن event مش بيسمح بالتعيين المباشر بره الكلاس)
   ```

4. **الاعتماد على ترتيب تنفيذ subscribers**:
   مينفعش تعتمد على ترتيب معين، لأن ممكن حد يضيف أو يشيل subscribers بشكل ديناميكي.

5. **رفع الحدث أكتر من اللازم**:
   في مثال الكرة، لو مقارناش القيمة القديمة بالجديدة، كل set كان هيشغل الحدث حتى لو القيمة واحدة. ده بيسبب استهلاك موارد.

---

## 🎯 خلاصة

إحنا شرحنا في هذا الدليل:
- ماهية Delegates والأنواع الجاهزة (Action, Func, Predicate).
- الفرق بين Delegate و Event وأهمية التغليف.
- كيفية الاشتراك والانسحاب من الأحداث.
- آلية إطلاق الحدث والتحقق من null.
- تحليل كود كامل لمشروع Ball.
- استعراض لمزايا Operator Overloading.
- الترقية إلى نمط EventHandler الرسمي.
- مشروع جديد (نظام الطلبات) لتطبيق المفاهيم.
- نصائح وأخطاء شائعة.