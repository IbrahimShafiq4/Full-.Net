# 🧠 SQL Server DDL

## ✍️ بالكود + 🖱️ بالـ Wizard (الاتنين مع بعض)

---

# 🗄️ 1️⃣ CREATE DATABASE

---

## ✍️ بالكود

```sql
CREATE DATABASE Shafiq;
USE Shafiq;
```

📌 بتعمل Database
📌 وبعدين بتحدد إنك تشتغل عليها

---

## 🖱️ بالـ Wizard

1. Object Explorer
2. كليك يمين على **Databases**
3. New Database
4. اكتب الاسم
5. OK

---

🧠 **حفظ**

> CREATE DATABASE = كود
> New Database = Wizard

---

# 📂 Database Files (مهم جدًا)

| File | وظيفته               |
| ---- | -------------------- |
| MDF  | Structure + Metadata |
| LDF  | Logs                 |

🧠

> MDF = شكل
> LDF = تاريخ 📜

---

# 📊 2️⃣ CREATE TABLE

---

## ✍️ بالكود

```sql
CREATE TABLE Employees (
	SSN INT PRIMARY KEY IDENTITY(1,1),
	FName VARCHAR(15) NOT NULL,
	LName VARCHAR(15),
	Address VARCHAR(20) DEFAULT 'Cairo',
	Salary MONEY,
	Gender CHAR(1),
	BDate DATE
);
```

---

## 🖱️ بالـ Wizard

1. افتح Database
2. Tables
3. New → Table
4. املا:

   * Column Name
   * Data Type
   * Allow Nulls
5. Ctrl + S
6. اكتب اسم الجدول

---

🧠 **حفظ**

> CREATE TABLE = كود
> Tables → New Table = Wizard

---

# 🔑 3️⃣ PRIMARY KEY

---

## ✍️ بالكود

### Inline

```sql
SSN INT PRIMARY KEY
```

### Separate

```sql
ALTER TABLE Employees
ADD PRIMARY KEY (SSN);
```

---

## 🖱️ بالـ Wizard

1. كليك يمين على العمود
2. Set Primary Key
3. علامة 🔑 تظهر

---

🧠

> PK = 🔑 = Unique + Not Null

---

# 🔢 4️⃣ IDENTITY (Auto Increment)

---

## ✍️ بالكود

```sql
SSN INT IDENTITY(1,1)
```

---

## 🖱️ بالـ Wizard

1. اختار العمود
2. Column Properties
3. Identity Specification
4. Is Identity → Yes
5. Seed & Increment

---

🧠

> Identity = SQL يدخل الرقم لوحده 🔢

---

# 🔗 5️⃣ FOREIGN KEY (Relation)

---

## ✍️ بالكود

### Inline

```sql
DNo INT REFERENCES Departments(DNumber)
```

### ALTER

```sql
ALTER TABLE Employees
ADD FOREIGN KEY (DNo)
REFERENCES Departments(DNumber);
```

---

## 🖱️ بالـ Wizard (Diagram)

1. Database Diagrams
2. New Diagram
3. Add Tables
4. اسحب العمود FK على PK
5. SQL يعمل العلاقة تلقائي

---

🧠

> Relation = FK → PK 🔗

---

# 🧱 6️⃣ Composite Primary Key

---

## ✍️ بالكود

```sql
CREATE TABLE DeptLocations (
	DNum INT REFERENCES Departments(DNumber),
	Location VARCHAR(20),
	PRIMARY KEY (DNum, Location)
);
```

---

## 🖱️ بالـ Wizard

1. Table Design
2. ظلل أكتر من عمود
3. كليك يمين
4. Set Primary Key

---

🧠

> Composite = أكتر من عمود = PK واحد 🔐

---

# ✏️ 7️⃣ ALTER TABLE

---

## ➕ ADD Column

### ✍️ بالكود

```sql
ALTER TABLE Employees
ADD Test BIGINT;
```

### 🖱️ Wizard

* Table Design
* أضف عمود جديد
* Save

---

## ✏️ ALTER Column

### ✍️ بالكود

```sql
ALTER TABLE Employees
ALTER COLUMN Test VARCHAR(20);
```

### 🖱️ Wizard

* Table Design
* غير Data Type
* Save

---

## ❌ DROP Column

### ✍️ بالكود

```sql
ALTER TABLE Employees
DROP COLUMN Test;
```

### 🖱️ Wizard

* Table Design
* Delete Column
* Save

---

# 💣 8️⃣ DROP TABLE

---

## ✍️ بالكود

```sql
DROP TABLE Employees;
```

---

## 🖱️ Wizard

1. كليك يمين على الجدول
2. Delete
3. OK

---

## ⚠️ لو Drop فشل؟

### السبب:

* في Foreign Key معتمد عليه

---

### الحل الصح (بالترتيب)

#### ✍️ بالكود

```sql
ALTER TABLE Employees
DROP CONSTRAINT FK_Employees_Departments;

DROP TABLE Employees;
```

#### 🖱️ Wizard

* Diagram
* امسح العلاقة
* بعدين Delete Table

---

🧠 **قاعدة ذهبية**

```
FK ❌
Child Table ❌
Parent Table ❌
```

---

# ✏️ 9️⃣ Rename Table

---

## ✍️ بالكود

```sql
EXEC sp_rename 'Table_1', 'Projects';
```

---

## 🖱️ Wizard

1. كليك يمين على الجدول
2. Rename
3. اكتب الاسم الجديد

---

# 🏆 الخلاصة

```
Create DB      → Code / Wizard
Create Table   → Code / Wizard
PK             → Code / Wizard
Identity       → Code / Wizard
FK             → Code / Diagram
Alter           → Code / Design
Drop            → Code / Delete
```
