# Views, Stored Procedures, Security & Relationships

---

## 🧠 ليه SQL مهم جدًا لِـ .NET Developer؟

في الشغل الحقيقي:

* الـ **API** (ASP.NET / Web API) ❌ ما بيتعاملش مع Tables مباشرة
* بيتعامل مع:

  * **Views**
  * **Stored Procedures**

ليه؟

* أمان
* Performance
* Business Logic جوه DB
* تقليل SQL Injection

---

# ===============================

# 🔹 VIEWS

# ===============================

## 📌 يعني إيه View؟

* **Virtual Table**
* مش بتخزن داتا
* بتخزن **SELECT Statement فقط**
* الداتا دايمًا جاية من Tables حقيقية

> View = Layer بين الـ Database والـ Application (.NET)

---

## ✅ استخدامات الـ View في الشغل

### 1️⃣ Security (الأمان)

في .NET:

* الـ Backend Developer ❌ مش عايز Frontend يعرف:

  * اسم الجدول الحقيقي
  * كل الأعمدة

📌 الحل:

```sql
CREATE VIEW PublicStudentsView
AS
SELECT St_Id, St_Fname
FROM Student;
```

الـ API يستخدم:

```sql
SELECT * FROM PublicStudentsView;
```

✔ أمان
✔ تحكم في الداتا
✔ أقل Exposure

---

### 2️⃣ SQL Injection (مهمة جدًا في .NET 🔥)

#### المشكلة

لو كتبت Query مباشرة:

```csharp
string query = "SELECT * FROM Student WHERE St_Id = " + userInput;
```

المستخدم يبعت:

```sql
1 OR 1=1
```

💥 كل الداتا اتعرضت

---

### الحل باستخدام View

```sql
SELECT * FROM StudentsView;
```

* المستخدم مش بيتحكم في WHERE
* Query ثابت
* مفيش String Concatenation

✔ View بتقفل باب SQL Injection جزئيًا
✔ مع Parameterized Queries = أمان عالي جدًا

---

### 3️⃣ Simplify Complex Queries

بدل Join طويل في كل Endpoint 👇

```sql
SELECT s.St_Id, s.St_Fname, d.Dept_Name
FROM Student s
JOIN Department d ON s.Dept_Id = d.Dept_Id;
```

تعمل View:

```sql
CREATE VIEW StudentsAndDepartments
AS
SELECT s.St_Id, s.St_Fname, d.Dept_Name
FROM Student s
JOIN Department d ON s.Dept_Id = d.Dept_Id;
```

وفي .NET:

```csharp
SELECT * FROM StudentsAndDepartments
```

✔ Cleaner Code
✔ Maintainable
✔ Reusable

---

## 🧩 Types of Views

### 1️⃣ Standard View

* SELECT واحد فقط

```sql
CREATE VIEW StudentsView
AS
SELECT * FROM Student;
```

---

### 2️⃣ Partitioned View

* أكتر من SELECT
* غالبًا باستخدام `UNION`

```sql
CREATE VIEW CairoAndAlexStudents
AS
SELECT * FROM CairoStudentsView
UNION
SELECT * FROM AlexStudentsView;
```

📌 مفيد لما:

* Data موزعة
* أو عايز Logical Merge

---

### 3️⃣ Indexed View

* View + Index
* أسرع في SELECT
* أبطأ في INSERT / UPDATE

⚠️ تستخدم بحذر في Production

---

## 🔐 WITH ENCRYPTION

### يعني إيه؟

* يخفي **Definition** بتاع:

  * View
  * Stored Procedure
  * Function

```sql
WITH ENCRYPTION
```

لو حد حاول:

```sql
SP_HELPTEXT 'ViewName'
```

❌ هيطلع:

> The text for object is encrypted

📌 **مهم**

* مش تشفير داتا
* تشفير كود فقط

---

## ✔ WITH CHECK OPTION

### المشكلة

```sql
INSERT INTO CairoStudentsView
VALUES (1, 'Ali', 'Tanta');
```

✔ يدخل في الجدول
❌ مش يظهر في View

---

### الحل

```sql
WITH CHECK OPTION
```

* أي Insert / Update
* لازم يحقق شرط الـ WHERE

🔥 دي مهمة جدًا في:

* Data Integrity
* APIs

---

## ✏️ DML مع Views

### View مبنية على Table واحدة

| Operation | Allowed |
| --------- | ------- |
| INSERT    | ✔       |
| UPDATE    | ✔       |
| DELETE    | ✔       |

---

### View مبنية على أكتر من Table

| Operation | Result          |
| --------- | --------------- |
| INSERT    | ❌               |
| DELETE    | ❌               |
| UPDATE    | ⚠️ (محدود جدًا) |

📌 SQL Server مش هيخمن:

* يحذف من أنهي Table؟

---

# ===============================

# 🔹 STORED PROCEDURES

# ===============================

## يعني إيه Stored Procedure؟

* Block من SQL
* محفوظ في Database
* **Precompiled**

في .NET:

```csharp
EXEC SP_GetStudentById @id
```

---

## ⚡ Performance (ليه أسرع؟)

Query عادي:

1. Parse
2. Optimize
3. Execution Plan
4. Execute

Stored Procedure:

* أول مرة بس
* بعد كده Execution Plan جاهز

✔ Faster
✔ Less CPU
✔ Better Scalability

---

## 🔒 Security في SP

* تخبي:

  * Tables
  * Columns
  * Business Rules

.NET API يشوف:

```sql
EXEC SP_GetStudentById 5
```

بس ❌ مش عارف:

* Student Table
* Column Names

---

## 🚨 Error Handling (TRY / CATCH)

بدل Error خام:

```sql
Msg 547, Level 16...
```

نرجّع:

```sql
Error While Deleting Topic [Math] with Id 1
```

✔ User Friendly
✔ API Friendly
✔ Professional

---

## 🔄 Input Parameters

```sql
SP_SumNumbers @X INT, @Y INT = 10
```

* Default Values
* Passing by Position
* Passing by Name

📌 في .NET:

```csharp
cmd.Parameters.AddWithValue("@X", 10);
```

---

## ⚠️ Dynamic SQL (Worst Case)

```sql
EXEC(
 'SELECT ' + @Column + ' FROM ' + @Table
)
```

❌ SQL Injection
❌ Schema Exposure

---

### الحل الصح

```sql
QUOTENAME(@Table)
```

✔ زي مثالك بالظبط
✔ Production-safe نسبيًا

---

## 📤 Output Parameters

بدل ما ترجع Table:

* ترجع Value

```sql
@Name OUTPUT
@Age OUTPUT
```

في .NET:

```csharp
cmd.Parameters["@Name"].Value
```

✔ سريع
✔ مناسب للـ Business Logic

---

# ===============================

# 🔹 RELATIONSHIPS & DELETE RULES

# ===============================

## طرق التعامل

### 1️⃣ Manual Queries

* Update Students
* Update Instructors
* Delete Department

✔ تحكم كامل
❌ كود أكتر

---

### 2️⃣ Wizard (Database Diagram)

#### Delete Rules:

| Rule        | Behavior     |
| ----------- | ------------ |
| CASCADE     | حذف الكل     |
| SET NULL    | يفصل العلاقة |
| SET DEFAULT | يرجع Default |

📌 في الشغل:

* CASCADE خطر
* SET NULL آمن أكتر

---

# 🧠 الخلاصة (.NET Mindset)

## ✔ Views

* Security Layer
* Abstraction
* Reusability

## ✔ Stored Procedures

* Performance
* Security
* Business Logic
* Enterprise Standard

❌ Dynamic SQL = آخر حل
❌ Direct Table Access = Red Flag في Interviews