# 📊 RANKING FUNCTIONS IN SQL

## 🔹 ليه بنستخدم Ranking Functions؟

علشان:

* نرتّب البيانات
* نجيب Top / Nth record
* نطلع ترتيب منطقي أو تسلسلي

---

## 1️⃣ ROW_NUMBER()

### الفكرة

* بيدّي **رقم فريد لكل صف**
* **مش بيحسب التكرار**
* دايمًا أرقام متسلسلة

```sql
SELECT Ins_Name, Salary,
ROW_NUMBER() OVER (ORDER BY Salary DESC) AS RN
FROM Instructor;
```

### شكل النتيجة

```
Salary | RN
--------------
5000   | 1
5000   | 2
4000   | 3
3000   | 4
```

📌 حتى لو المرتب متكرر → الرقم مختلف

### الاستخدام

* Top N
* Pagination
* مجرد عدّ صفوف

---

## 2️⃣ DENSE_RANK()

### الفكرة

* **بيحسب التكرار**
* **مفيش فجوات**

```sql
SELECT Ins_Name, Salary,
DENSE_RANK() OVER (ORDER BY Salary DESC) AS DR
FROM Instructor;
```

### شكل النتيجة

```
Salary | DR
--------------
5000   | 1
5000   | 1
4000   | 2
3000   | 3
```

### الاستخدام

* ترتيب منطقي
* مراكز بدون أرقام ضايعة

---

## 3️⃣ RANK()

### الفكرة

* **بيحسب التكرار**
* **بيسيب فجوات**

```sql
SELECT Ins_Name, Salary,
RANK() OVER (ORDER BY Salary DESC) AS R
FROM Instructor;
```

### شكل النتيجة

```
Salary | R
--------------
5000   | 1
5000   | 1
4000   | 3
3000   | 4
```

📌 الرقم 2 اختفى

---

## 🧠 مقارنة سريعة

| Function   | التكرار | فجوات |
| ---------- | ------- | ----- |
| ROW_NUMBER | ❌       | ❌     |
| DENSE_RANK | ✅       | ❌     |
| RANK       | ✅       | ✅     |

---

# 🧩 PARTITION BY vs GROUP BY

## 🔸 GROUP BY

* بيقلّل عدد الصفوف
* لازم Aggregation

```sql
SELECT Dept_Id, MIN(St_Age)
FROM Student
GROUP BY Dept_Id;
```

📌 النتيجة صف واحد لكل Department

---

## 🔸 PARTITION BY

* مش بيقلّل الصفوف
* بيقسّم البيانات منطقيًا

```sql
ROW_NUMBER() OVER (
  PARTITION BY Dept_Id
  ORDER BY St_Age
)
```

📌 كل Department يبدأ ترتيب من 1

---

## ✔️ أصغر طالب في كل قسم (PARTITION BY)

```sql
SELECT *
FROM (
  SELECT Dept_Id, St_Fname, St_Age,
         ROW_NUMBER() OVER (
           PARTITION BY Dept_Id
           ORDER BY St_Age
         ) AS RN
  FROM Student
) T
WHERE RN = 1;
```

---

## ✔️ نفس الفكرة باستخدام JOIN

```sql
SELECT s.*
FROM Student s
JOIN (
  SELECT Dept_Id, MIN(St_Age) AS MinAge
  FROM Student
  GROUP BY Dept_Id
) m
ON s.Dept_Id = m.Dept_Id
AND s.St_Age = m.MinAge;
```

---

# 📐 NTILE()

## الفكرة

* بتقسّم الصفوف لـ `n` مجموعات متقاربة في العدد

```sql
SELECT Ins_Name, Salary,
NTILE(3) OVER (ORDER BY Salary DESC) AS G
FROM Instructor;
```

### مثال بصري

```
Rows = 9
NTILE(3)

Group 1 → 1,2,3
Group 2 → 4,5,6
Group 3 → 7,8,9
```

---

## 📏 معادلة NTILE

```text
NTILE = CEILING( row_number * n / R )

R = عدد الصفوف
n = عدد الجروبات
```

---

# ⏱ EXECUTION ORDER

```text
FROM
ON
JOIN
WHERE
GROUP BY
HAVING
SELECT
DISTINCT
ORDER BY
TOP
```

---

## ❌ ليه FullName مينفعش في WHERE؟

```sql
SELECT CONCAT(St_Fname,' ',St_Lname) AS FullName
FROM Student
WHERE FullName = 'Ahmed Hassan';
```

📌 `WHERE` بتتنفذ قبل `SELECT`

---

## ✔️ الحلول الصحيحة

### الحل 1

```sql
WHERE CONCAT(St_Fname,' ',St_Lname) = 'Ahmed Hassan'
```

### الحل 2

```sql
SELECT *
FROM (
  SELECT CONCAT(St_Fname,' ',St_Lname) AS FullName
  FROM Student
) T
WHERE FullName = 'Ahmed Hassan';
```

---

# 🏗 SCHEMA

## المشاكل اللي حلّها

1️⃣ تكرار أسماء الجداول
2️⃣ تنظيم منطقي
3️⃣ Permissions

### مثال

```sql
CREATE SCHEMA HR;
ALTER SCHEMA HR TRANSFER Student;
```

📌 أي User له صلاحية HR → يشوف كل الجداول جوّه HR

---

# 🔁 UNION FAMILY (شرح + أمثلة)

## شروط UNION

1️⃣ نفس عدد الأعمدة
2️⃣ نفس نوع البيانات

---

## 🔹 UNION (يشيل التكرار)

```sql
SELECT St_Id, St_Fname
FROM Student
WHERE St_Id IS NOT NULL
UNION
SELECT Ins_Id, Ins_Name
FROM Instructor;
```

📌 لو نفس القيمة اتكررت → تظهر مرة واحدة

---

## 🔹 UNION ALL (يجيب الكل)

```sql
SELECT St_Id, St_Fname
FROM Student
UNION ALL
SELECT Ins_Id, Ins_Name
FROM Instructor;
```

📌 أسرع – **لا يشيل التكرار**

---

## 🔹 INTERSECT (المشترك)

```sql
SELECT St_Id, St_Fname
FROM Student
INTERSECT
SELECT Ins_Id, Ins_Name
FROM Instructor;
```

📌 بيرجع القيم اللي موجودة في الاتنين

---

## 🔹 EXCEPT (الفرق)

```sql
SELECT St_Id, St_Fname
FROM Student
EXCEPT
SELECT Ins_Id, Ins_Name
FROM Instructor;
```

📌 اللي في Student ومش موجود في Instructor

---

## 🧠 شكل بصري

```
Student        Instructor
 A  B  C        B  C  D

INTERSECT → B C
EXCEPT    → A
UNION     → A B C D
```

---

# 🧠 USER DEFINED FUNCTIONS

---

## 1️⃣ Scalar Function

### بترجع قيمة واحدة

```sql
CREATE FUNCTION GetStudentNameByStudentId(@Id INT)
RETURNS VARCHAR(20)
BEGIN
  DECLARE @Name VARCHAR(20)
  SELECT @Name = St_Fname
  FROM Student
  WHERE St_Id = @Id
  RETURN @Name
END
```

```sql
SELECT dbo.GetStudentNameByStudentId(10);
```

---

## أمثلة Scalar إضافية

### ✔️ عدد طلاب القسم

```sql
CREATE FUNCTION GetStudentsCountByDept(@DeptId INT)
RETURNS INT
BEGIN
  DECLARE @C INT
  SELECT @C = COUNT(*)
  FROM Student
  WHERE Dept_Id = @DeptId
  RETURN @C
END
```

---

## 2️⃣ Inline Table Function

```sql
CREATE FUNCTION GetInstructorsByDept(@DeptId INT)
RETURNS TABLE
AS
RETURN (
  SELECT Ins_Name, Salary
  FROM Instructor
  WHERE Dept_Id = @DeptId
);
```

📌 أفضل Performance

---

## 3️⃣ Multi Statement Table Function

```sql
CREATE FUNCTION GetStudentDataBasedOnFormat(@format VARCHAR(20))
RETURNS @T TABLE (Id INT, Name VARCHAR(100))
AS
BEGIN
  IF @format = 'first'
    INSERT INTO @T
    SELECT St_Id, ISNULL(St_Fname,'unknown')
    FROM Student;

  ELSE IF @format = 'full'
    INSERT INTO @T
    SELECT St_Id,
           CONCAT(ISNULL(St_Fname,'unknown'),' ',
                  ISNULL(St_Lname,'unknown'))
    FROM Student;

  RETURN;
END
```

---

## 🧠 مقارنة Functions

| Type            | Return | Speed |
| --------------- | ------ | ----- |
| Scalar          | Value  | ❌     |
| Inline Table    | Table  | ✅     |
| Multi Statement | Table  | ⚠️    |