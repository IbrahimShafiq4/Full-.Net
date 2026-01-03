# 📘 SQL Practice

**Database:** `MyCompany` / `ITI`
### 📌 You will Find Them Next To That Readme File😊
---

## 🔹 Q1) Display all employees data

### ✅ Solution 1 (Basic)

```sql
SELECT *
FROM Employee;
```

📌 أبسط حل – يعرض كل الأعمدة وكل الصفوف.

---

## 🔹 Q2) Display employee first name, last name, salary and department number

### ✅ Solution 1

```sql
SELECT Fname, Lname, Salary, Dno
FROM Employee;
```

### ✅ Solution 2 (Alias)

```sql
SELECT 
  Fname AS FirstName,
  Lname AS LastName,
  Salary,
  Dno AS DeptNo
FROM Employee;
```

---

## 🔹 Q3) Display employee full name and annual commission (10%)

### ✅ Solution 1 (Direct calculation)

```sql
SELECT 
  CONCAT(Fname, ' ', Lname) AS [Full Name],
  Salary * 12 * 0.1 AS [Annual Comm]
FROM Employee;
```

### ✅ Solution 2 (Using Annual Salary Alias)

```sql
SELECT 
  CONCAT(Fname, ' ', Lname) AS [Full Name],
  (Salary * 12) * 0.1 AS [Annual Comm]
FROM Employee;
```

📌 commission = 10% من المرتب السنوي

---

## 🔹 Q4) Display employees who earn more than 1000 LE monthly

### ✅ Solution 1

```sql
SELECT 
  SSN AS ID,
  CONCAT(Fname, ' ', Lname) AS [Full Name],
  Salary
FROM Employee
WHERE Salary > 1000;
```

### ✅ Solution 2 (Explicit column names)

```sql
SELECT SSN, Fname, Lname, Salary
FROM Employee
WHERE Salary > 1000;
```

---

## 🔹 Q5) Display employees who earn more than 10000 LE annually

### ✅ Solution 1

```sql
SELECT 
  SSN AS ID,
  CONCAT(Fname, ' ', Lname) AS [Full Name],
  Salary
FROM Employee
WHERE Salary * 12 > 10000;
```

### ✅ Solution 2 (Using calculated column)

```sql
SELECT 
  SSN,
  CONCAT(Fname, ' ', Lname) AS [Full Name],
  Salary * 12 AS AnnualSalary
FROM Employee
WHERE Salary * 12 > 10000;
```

---

## 🔹 Q6) Display names and salaries of female employees

### ✅ Solution 1

```sql
SELECT 
  CONCAT(Fname, ' ', Lname) AS [Full Name],
  Salary
FROM Employee
WHERE Sex = 'Female';
```

### ✅ Solution 2 (Handling multiple values)

```sql
SELECT 
  CONCAT(Fname, ' ', Lname) AS [Full Name],
  Salary
FROM Employee
WHERE Sex IN ('Female', 'F');
```

---

## 🔹 Q7) Display departments managed by manager with SSN = 968754

### ✅ Solution 1

```sql
SELECT Dnum, Dname
FROM Departments
WHERE MGRSSN = 968754;
```

### ✅ Solution 2 (More readable)

```sql
SELECT 
  Dnum AS DeptID,
  Dname AS DeptName
FROM Departments
WHERE MGRSSN = 968754;
```

---

## 🔹 Q8) Display projects controlled by department 10

### ✅ Solution 1

```sql
SELECT Pnumber, Pname, Plocation
FROM Project
WHERE Dnum = 10;
```

### ✅ Solution 2 (Alias)

```sql
SELECT 
  Pnumber AS ProjectID,
  Pname AS ProjectName,
  Plocation
FROM Project
WHERE Dnum = 10;
```

---

## 🔹 Q9) Insert your personal data as new employee

### ✅ Solution 1 (Partial columns)

```sql
INSERT INTO Employee (SSN, Fname, Dno, Superssn, Salary)
VALUES (102672, 'Ibrahim', 30, 112233, 3000);
```

### ✅ Solution 2 (With last name)

```sql
INSERT INTO Employee (SSN, Fname, Lname, Dno, Superssn, Salary)
VALUES (102672, 'Ibrahim', 'Shafiq', 30, 112233, 3000);
```

---

## 🔹 Q10) Insert friend employee without salary or manager

### ✅ Solution 1

```sql
INSERT INTO Employee (SSN, Fname, Lname, Dno)
VALUES (102660, 'Muhammad', 'Tareq', 30);
```

### ✅ Solution 2 (Multiple rows)

```sql
INSERT INTO Employee (SSN, Fname, Lname, Dno)
VALUES
  (102660, 'Muhammad', 'Tareq', 30),
  (102661, 'Youssef', 'Muhammad', 30);
```

📌 Salary & Superssn هيتحطوا NULL

---

## 🔹 Q11) Upgrade your salary by 20%

### ✅ Solution 1

```sql
UPDATE Employee
SET Salary = Salary + (Salary * 0.2)
WHERE SSN = 102672;
```

### ✅ Solution 2 (Shortcut)

```sql
UPDATE Employee
SET Salary *= 1.2
WHERE SSN = 102672;
```

---

## 🔹 Q12) Return all instructors names without repetition

### ✅ Solution

```sql
SELECT DISTINCT Ins_Name
FROM Instructor;
```

📌 `DISTINCT` تمنع التكرار

---

## 🔹 Q13) Display projects in Cairo or Alex

### ✅ Solution 1

```sql
SELECT Pnumber, Pname, Plocation
FROM Project
WHERE Plocation = 'Cairo' OR Plocation = 'Alex';
```

### ✅ Solution 2 (IN)

```sql
SELECT Pnumber, Pname, Plocation
FROM Project
WHERE Plocation IN ('Cairo', 'Alex');
```

---

## 🔹 Q14) Display projects with names starting with "A"

### ✅ Solution

```sql
SELECT *
FROM Project
WHERE Pname LIKE 'A%';
```

📌 `%` = أي عدد حروف بعد A

---

## 🔹 Q15) Display employees in department 30 with salary between 1000 and 2000

### ✅ Solution 1

```sql
SELECT *
FROM Employee
WHERE Dno = 30
  AND Salary BETWEEN 1000 AND 2000;
```

### ✅ Solution 2 (Using comparison)

```sql
SELECT *
FROM Employee
WHERE Dno = 30
  AND Salary >= 1000
  AND Salary <= 2000;
```

---

# 🧠 Final Notes

* دايماً فكر: **WHERE ولا JOIN؟**
* الحسابات تتحط في `SELECT` أو `WHERE`
* `BETWEEN` شامل القيم
* `IN` أنضف من OR
* `DISTINCT` = منع تكرار
