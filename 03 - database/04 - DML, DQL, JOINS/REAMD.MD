# 📘 SQL DML & DQL & JOINS

---

## 🟢 أولاً: DML

### **Data Manipulation Language**

يعني أوامر **التعامل مع الداتا نفسها**
👉 إدخال – تعديل – حذف

---

## 1️⃣ INSERT

### 🔹 Simple Insert

### 📌 Syntax

```sql
INSERT INTO TableName
VALUES (value1, value2, value3, ...)
```

> ⚠️ لازم القيم تكون **بنفس ترتيب الأعمدة في الجدول**

### 📌 Example

```sql
INSERT INTO Employee
VALUES ('Ibrahim', 'Shafiq', 1, '2000-03-17', 'Cairo', 'Male', 2000, NULL, NULL);
```

---

### 🔹 Insert مع تحديد الأعمدة (أفضل)

### 📌 Syntax

```sql
INSERT INTO TableName (col1, col2, col3)
VALUES (val1, val2, val3)
```

### 📌 Example

```sql
INSERT INTO Employee (Fname, Lname, SSN, Superssn, Dno)
VALUES ('Muhmad', 'Shafiq', 1456, 321654, 10);
```

✔️ هنا تقدر تسيب أعمدة
✔️ الترتيب مش مهم طالما الأعمدة محددة

---

### 🔹 Insert Multiple Rows

### 📌 Syntax

```sql
INSERT INTO TableName
VALUES
 (..),
 (..),
 (..);
```

### 📌 Example

```sql
INSERT INTO Employee
VALUES 
('Mahmoud', 'Shafiq', 3, '1992-09-25', 'Cairo', 'Male', 100000, 223344, 10),
('Shafiq', 'Abd-Elshafy', 4, '1953-09-28', 'Cairo', 'Male', 1000000, 223344, 10);
```

---

## 2️⃣ UPDATE

### تعديل بيانات موجودة

### 📌 Syntax

```sql
UPDATE TableName
SET column = value
WHERE condition;
```

> ⚠️ **الـ WHERE مهم جداً**
> من غيره هيعدّل كل الجدول 😱

### 📌 Example

```sql
UPDATE Employee
SET Salary = 5000
WHERE SSN = 1;
```

---

### 🔹 Update أكتر من عمود

```sql
UPDATE Employee
SET Salary = 1000000,
    Address = 'Maadi, Cairo'
WHERE SSN = 1456;
```

---

## 3️⃣ DELETE

### حذف بيانات

### 📌 Syntax

```sql
DELETE FROM TableName
WHERE condition;
```

### 📌 Example

```sql
DELETE FROM Employee
WHERE SSN = 9999;
```

### ❌ بدون WHERE

```sql
DELETE FROM Employee;
```

✔️ يحذف كل الداتا
❌ مايرجّعش الـ Identity

---

## 4️⃣ TRUNCATE

### مسح الجدول بالكامل

```sql
TRUNCATE TABLE Employee;
```

✔️ أسرع
✔️ بيرجّع الـ Identity
❌ مفيش WHERE
❌ مينفعش مع FK

---

# 🔵 DQL

### **Data Query Language**

يعني **عرض البيانات**

---

## 1️⃣ SELECT

### 📌 Syntax

```sql
SELECT column1, column2
FROM TableName;
```

### 📌 Example

```sql
SELECT *
FROM Student;
```

---

## 2️⃣ CONCAT & Alias

### دمج أعمدة

```sql
SELECT St_Fname + ' ' + St_Lname AS FullName
FROM Student;
```

### أو

```sql
SELECT CONCAT(St_Fname, ' ', St_Lname) AS [Student Name]
FROM Student;
```

---

## 3️⃣ WHERE

### فلترة الداتا

```sql
SELECT *
FROM Student
WHERE St_Age > 23;
```

---

### BETWEEN

```sql
SELECT *
FROM Student
WHERE St_Age BETWEEN 21 AND 25;
```

---

### IN

```sql
SELECT *
FROM Student
WHERE St_Address IN ('Cairo', 'Alex');
```

---

### NOT IN

```sql
SELECT *
FROM Student
WHERE St_Address NOT IN ('Cairo', 'Alex');
```

---

### NULL

```sql
SELECT *
FROM Student
WHERE St_super IS NOT NULL;
```

---

## 4️⃣ LIKE

### Pattern Matching

| Symbol | Meaning     |
| ------ | ----------- |
| `%`    | أي عدد حروف |
| `_`    | حرف واحد    |

### 📌 Examples

```sql
-- ثاني حرف A
WHERE St_Fname LIKE '_A%';

-- يبدأ بـ A وينتهي بـ H
WHERE St_Fname LIKE 'A%H';

-- ينتهي بـ A
WHERE St_Fname LIKE '%A';
```

### Ranges

```sql
WHERE St_Fname LIKE '[A-H]%';
WHERE St_Fname LIKE '[^A-H]%';
```

---

## 5️⃣ DISTINCT

```sql
SELECT DISTINCT St_Fname
FROM Student;
```

✔️ يمنع التكرار

---

## 6️⃣ ORDER BY

```sql
SELECT St_Fname, St_Age
FROM Student
ORDER BY St_Age DESC;
```

---

## 7️⃣ TOP

```sql
SELECT TOP(4) *
FROM Student;
```

---

# 🟣 JOINS

### ربط جداول ببعض

---

## 1️⃣ INNER JOIN

### المشترك بس

```sql
SELECT *
FROM Student s
INNER JOIN Department d
ON s.Dept_Id = d.Dept_Id;
```

---

## 2️⃣ LEFT JOIN

### كل اللي على الشمال

```sql
SELECT *
FROM Student s
LEFT JOIN Department d
ON s.Dept_Id = d.Dept_Id;
```

---

## 3️⃣ RIGHT JOIN

```sql
SELECT *
FROM Student s
RIGHT JOIN Department d
ON s.Dept_Id = d.Dept_Id;
```

---

## 4️⃣ FULL OUTER JOIN

```sql
SELECT *
FROM Student s
FULL JOIN Department d
ON s.Dept_Id = d.Dept_Id;
```

---

## 5️⃣ CROSS JOIN

### Cartesian Product 😈

```sql
SELECT s.St_Fname, d.Dept_Name
FROM Student s
CROSS JOIN Department d;
```

---

## 6️⃣ SELF JOIN

### جدول مع نفسه

```sql
SELECT 
  CONCAT(e.Fname, ' ', e.Lname) AS Employee,
  CONCAT(m.Fname, ' ', m.Lname) AS Manager
FROM Employee e
JOIN Employee m
ON e.Superssn = m.SSN;
```

---

## 7️⃣ Multi Table Join

```sql
SELECT 
  CONCAT(s.St_Fname, ' ', s.St_Lname) AS Student,
  c.Crs_Name,
  sc.Grade
FROM Student s
JOIN Stud_Course sc ON s.St_Id = sc.St_Id
JOIN Course c ON c.Crs_Id = sc.Crs_Id;
```

---

# 🔴 JOIN + DML (مهم جداً)

---

## 🟢 INSERT + JOIN

### إدخال بيانات من جدول لجدول

```sql
INSERT INTO GraduateStudents (St_Id, Name)
SELECT St_Id, St_Fname
FROM Student
WHERE St_Age > 22;
```

---

## 🟡 UPDATE + JOIN

### تعديل باستخدام جدول تاني

```sql
UPDATE s
SET s.St_Age = s.St_Age + 1
FROM Student s
JOIN Department d
ON s.Dept_Id = d.Dept_Id
WHERE d.Dept_Name = 'IT';
```

---

## 🔴 DELETE + JOIN

### حذف بناءً على جدول تاني

```sql
DELETE s
FROM Student s
JOIN Department d
ON s.Dept_Id = d.Dept_Id
WHERE d.Dept_Name = 'HR';
```

---

## 🧠 ملحوظات مهمة

* `WHERE` مع `DELETE / UPDATE` = أمان
* `ON` مع `JOIN`
* `WHERE` للفلترة
* `CROSS JOIN` خطر مع داتا كبيرة
* دايماً استخدم **JOIN syntax** مش ANSI القديم
