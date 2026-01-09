# 🧠 SQL Functions

---

## 🔹 أولًا: Built-in Functions

دي دوال SQL الجاهزة اللي بنستخدمها مباشرة من غير ما نكتب كود خاص بينا.

---

# 1️⃣ Aggregate Functions (دوال تجميعية)

> **بتطلع قيم مش موجودة فعليًا في الجدول**
> يعني بتحسب من الداتا: عدد – مجموع – متوسط – أكبر – أصغر

---

## ✅ COUNT()

### 🔸 عدّ كل الصفوف (حتى لو فيها NULL)

```sql
SELECT COUNT(*)
FROM Student;
```

📌 بيحسب **كل الطلاب** حتى اللي عنده Age = NULL

---

### 🔸 عدّ القيم غير الـ NULL بس

```sql
SELECT COUNT(St_Age)
FROM Student;
```

📌 بيعد الطلاب اللي **عندهم Age فعليًا**

---

### 🔸 عدّ الطلاب اللي ليهم Last Name

```sql
SELECT COUNT(St_Lname) AS NumberOfLastName
FROM Student;
```

---

### 🔸 عدّ أكتر من عمود

```sql
SELECT 
	COUNT(St_Lname) AS NumberOfLastName,
	COUNT(St_Address) AS [Number of Address]
FROM Student;
```

📌 أي عمود فيه NULL **مش بيتحسب**

---

## ✅ SUM()

```sql
SELECT SUM(Salary)
FROM Instructor;
```

📌 تجمع المرتبات
⚠️ لازم العمود يكون **رقم**

---

## ✅ AVG()

```sql
SELECT AVG(Salary)
FROM Instructor;
```

```sql
SELECT AVG(St_Age)
FROM Student;
```

📌 = مجموع القيم / عددهم (من غير NULL)

---

## ✅ MIN / MAX

```sql
SELECT 
	MAX(Salary) AS MaxSalary,
	MIN(Salary) AS MinSalary
FROM Instructor;
```

---

# 2️⃣ GROUP BY

> بنستخدمها **مع Aggregate Functions**
> ونجمع على العمود اللي جنب الدالة

---

## 🔸 أقل مرتب في كل Department

❌ غلط (Cartesian Product):

```sql
FROM Instructor, Department
```

✅ الصح:

```sql
SELECT d.Dept_Name, MIN(i.Salary) AS MinimumSalary
FROM Instructor i
JOIN Department d ON i.Dept_Id = d.Dept_Id
GROUP BY d.Dept_Name;
```

---

## 🔸 أعلى مرتب لكل Department

```sql
SELECT Dept_Name, MAX(Salary) AS MaximumSalary
FROM Instructor i
JOIN Department d ON i.Dept_Id = d.Dept_Id
GROUP BY Dept_Name;
```

---

## 🔸 متوسط المرتبات

```sql
SELECT Dept_Id, AVG(Salary) AS AverageSalary
FROM Instructor
GROUP BY Dept_Id;
```

---

## 🔸 GROUP BY + HAVING

> HAVING = شرط على نتيجة التجميع

```sql
SELECT Dept_Id, St_Address, COUNT(St_Id) AS NumberOfStudents
FROM Student
WHERE Dept_Id IS NOT NULL
GROUP BY Dept_Id, St_Address
HAVING COUNT(St_Id) > 2;
```

📌 **WHERE قبل GROUP**
📌 **HAVING بعد GROUP**

---

## 🔸 عدد الطلاب في كل Department

```sql
SELECT d.Dept_Id, d.Dept_Name, COUNT(s.St_Id)
FROM Student s
JOIN Department d ON s.Dept_Id = d.Dept_Id
GROUP BY d.Dept_Id, d.Dept_Name
HAVING COUNT(s.St_Id) > 2;
```

---

## 🔸 Self Join (مشرفين)

```sql
SELECT 
	s.St_Fname + ' ' + s.St_Lname AS FullName,
	COUNT(st.St_Id) AS SupervisedStudents
FROM Student s
JOIN Student st ON s.St_Id = st.St_super
GROUP BY s.St_Fname + ' ' + s.St_Lname;
```

---

# 3️⃣ NULL Functions

---

## ✅ ISNULL()

```sql
SELECT St_Fname, ISNULL(St_Lname, 'No Last Name')
FROM Student;
```

📌 لو St_Lname = NULL → يحط النص

---

## ✅ COALESCE() (أفضل)

```sql
SELECT COALESCE(St_Lname, St_Fname, 'Unknown')
FROM Student;
```

📌 بياخد **أكتر من بديل**
📌 أول قيمة مش NULL هي اللي بترجع

---

# 4️⃣ Casting & Conversion

---

## ❌ مشكلة الـ NULL مع +

```sql
SELECT St_Fname + ' ' + St_Lname
FROM Student;
```

📌 لو واحد NULL → النتيجة كلها NULL

---

## ✅ الحل: CONVERT + ISNULL

```sql
SELECT 
	ISNULL(St_Fname, 'Unknown') + ' ' +
	CONVERT(VARCHAR(20), ISNULL(St_Age, 0))
FROM Student;
```

---

## 📅 CONVERT مع التاريخ

```sql
SELECT CONVERT(VARCHAR(50), GETDATE(), 103);
```

---

## ✅ CONCAT (أسهل حل)

```sql
SELECT CONCAT(St_Fname, ' ', St_Age)
FROM Student;
```

📌 NULL → Empty String
📌 أنضف وأأمن

---

## ✅ FORMAT (تنسيق التاريخ)

```sql
SELECT FORMAT(GETDATE(), 'dd-MM-yyyy');
SELECT FORMAT(GETDATE(), 'hh tt');
SELECT FORMAT(GETDATE(), 'dddd', 'ar');
```

⚠️

* `MM` = Month
* `mm` = Minutes

---

## ✅ CAST

```sql
SELECT CAST(GETDATE() AS VARCHAR(50));
```

---

# 5️⃣ DateTime Functions

```sql
SELECT GETDATE();
SELECT DAY(GETDATE());
SELECT MONTH(GETDATE());
SELECT EOMONTH(GETDATE());
```

---

# 6️⃣ String Functions

---

## LOWER / UPPER

```sql
SELECT LOWER(St_Fname), UPPER(St_Lname)
FROM Student;
```

---

## SUBSTRING

```sql
SELECT SUBSTRING(St_Fname, 1, 3)
FROM Student;
```

⚠️ SQL Server غريب شوية:

* start = 0
* start = -1
  بيحسب قبل الكلمة 😄

---

## LEN

```sql
SELECT St_Fname, LEN(St_Fname)
FROM Student;
```

---

# 7️⃣ Subquery (الاستعلامات الداخلية)

---

## ❌ غلط

```sql
WHERE St_Age > AVG(St_Age)
```

---

## ✅ صح

```sql
SELECT *
FROM Student
WHERE St_Age >
(
	SELECT AVG(St_Age)
	FROM Student
);
```

---

## 🔸 Subquery كعمود

```sql
SELECT *,
	(SELECT COUNT(*) FROM Student) AS TotalStudents
FROM Student;
```

📌 النتيجة:

```
1 | Ahmed | 20
2 | Ali   | 20
3 | Sara  | 20
```

---

## 🔸 JOIN vs Subquery

### JOIN (أفضل)

```sql
SELECT DISTINCT d.Dept_Name
FROM Department d
JOIN Student s ON d.Dept_Id = s.Dept_Id;
```

### Subquery

```sql
SELECT Dept_Name
FROM Department
WHERE Dept_Id IN (
	SELECT Dept_Id
	FROM Student
);
```

📌 JOIN أسرع وأوضح

---

# 8️⃣ Math Functions

```sql
SELECT Salary, POWER(Salary, 2) FROM Instructor;
SELECT Salary, SQRT(Salary) FROM Instructor;
SELECT LOG(Salary) FROM Instructor;
SELECT SIN(30), COS(30), TAN(30);
```

---

# 9️⃣ System Functions

```sql
SELECT DB_NAME();
SELECT SUSER_NAME();
SELECT @@SERVERNAME;
```

---

# 🔟 DML + Subquery

---

## UPDATE

```sql
UPDATE Stud_Course
SET Grade += 10
WHERE St_Id IN (
	SELECT St_Id
	FROM Student
	WHERE St_Address = 'Cairo'
);
```

---

## DELETE (Subquery)

```sql
DELETE FROM Stud_Course
WHERE St_Id IN (
	SELECT St_Id FROM Student WHERE St_Address = 'Cairo'
);
```

---

## DELETE (JOIN – أفضل)

```sql
DELETE SC
FROM Student s
JOIN Stud_Course SC ON s.St_Id = SC.St_Id
WHERE s.St_Address = 'Cairo';
```

---

# 1️⃣1️⃣ TOP

---

## أعلى مرتبات

```sql
SELECT TOP(2) Salary
FROM Instructor
ORDER BY Salary DESC;
```

---

## ثاني أعلى مرتب

```sql
SELECT MAX(Salary)
FROM Instructor
WHERE Salary <
(
	SELECT MAX(Salary)
	FROM Instructor
);
```

---

## ❌ سبب الخطأ

```sql
SELECT TOP(2) Salary, MAX(Salary)
FROM Instructor;
```

📌 **MAX محتاجة GROUP BY**

---

## ✅ الحل

```sql
SELECT TOP(2) Salary, MAX(Salary) AS SalaryCombined
FROM Instructor
GROUP BY Salary;
```

---

## TOP WITH TIES

```sql
SELECT TOP(5) WITH TIES St_Age
FROM Student
ORDER BY St_Age DESC;
```

---

# 1️⃣2️⃣ Random Select 🎲

---

## اختيار عشوائي

```sql
SELECT TOP(3) *
FROM Student
ORDER BY NEWID();
```

📌 `NEWID()` بيعمل GUID عشوائي
📌 ORDER BY عليه = ترتيب عشوائي

---

## استخدامه في Quiz / Exam

```sql
SELECT *
FROM Questions
ORDER BY NEWID();
```