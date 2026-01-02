# 🧠 SQL Server – Variables & SQL Languages Cheat Sheet

---

## 🧩 أولًا: Variables في SQL Server (T-SQL)

---

## 1️⃣ Global Variables (System Variables)

### 📌 يعني إيه Global Variables؟

* دي **Variables جاهزة من SQL Server**
* انت **مبتعملهاش**
* **بتقراها بس**

⚠️ **مفيش User Defined Global Variables**
يعني:

> متقدرش تعمل Global Variable بإيدك ❌

---

### 🔹 أمثلة مهمة

```sql
SELECT @@version;
```

📌 بترجع:

* Version بتاع SQL Server
* OS
* Build info

---

```sql
SELECT @@SERVERNAME;
```

📌 بترجع:

* اسم السيرفر

---

🧠 **طريقة الحفظ**

> أي حاجة بتبدأ بـ `@@` → Global Variable 🌍

---

## 2️⃣ Local Variables (User Defined)

### 📌 يعني إيه Local Variable؟

* Variable بتعمله **انت**
* شغال **جوا الـ Batch أو Procedure بس**

---

### 🔹 Declare Variable

```sql
DECLARE @age INT = 10;
```

📌 لازم:

* يبدأ بـ `@`
* تحدد نوع البيانات

---

### 🔹 Print Variable

#### باستخدام `SELECT`

```sql
SELECT @age AS age;
```

✔️ بيرجع **Table**

---

#### باستخدام `PRINT`

```sql
PRINT @age;
```

✔️ بيرجع **Text Message**

---

🧠 **فرق مهم جدًا (بييجي في الامتحان)**

| SELECT               | PRINT      |
| -------------------- | ---------- |
| Table                | Message    |
| ينفع مع أكتر من قيمة | قيمة واحدة |
| أفضل في Debugging    | محدود      |

---

### 🔹 Change Variable Value

```sql
SET @age = 20;
PRINT @age;
```

🧠 **طريقة الحفظ**

> Declare → SET → Use 🔁

---

## 🧠 ملحوظة مهمة جدًا

```sql
-- ANSI SQL IS THE PARENT
```

يعني:

* ANSI SQL = الأساس
* وكل Database ليها Extension

---

## 🏛️ SQL Dialects (لهجات SQL)

| Database             | Language   |
| -------------------- | ---------- |
| Microsoft SQL Server | T-SQL      |
| Oracle               | PL-SQL     |
| IBM                  | IBM-PL-SQL |
| MySQL                | MySQL SQL  |

🧠 **طريقة الحفظ**

> SQL واحد – التنفيذ مختلف

---

# 🧱 SQL Commands Types (أهم جزء 🔥)

---

## 1️⃣ DDL – Data Definition Language

### 📌 مسؤولة عن:

* **Structure**
* **Metadata**

يعني:

> شكل الداتا مش الداتا نفسها

---

### 🔹 Commands

```sql
CREATE
ALTER
DROP
SELECT INTO
```

---

### 🔹 Examples

```sql
CREATE TABLE Students (...);
```

```sql
ALTER TABLE Students ADD Age INT;
```

```sql
DROP TABLE Students;
```

🧠 **طريقة الحفظ**

> DDL = Design 🏗️

---

## 2️⃣ DML – Data Manipulation Language

### 📌 مسؤولة عن:

* **الداتا نفسها**

---

### 🔹 Commands

```sql
INSERT
UPDATE
DELETE
MERGE
```

---

### 🔹 Examples

```sql
INSERT INTO Students VALUES (...);
```

```sql
UPDATE Students SET Age = 20;
```

```sql
DELETE FROM Students WHERE Id = 1;
```

🧠 **طريقة الحفظ**

> DML = Modify ✏️

---

## 3️⃣ DCL – Data Control Language

### 📌 مسؤولة عن:

* Security
* Permissions

---

### 🔹 Commands

```sql
GRANT
DENY
REVOKE
```

---

### 🔹 Example

```sql
GRANT SELECT ON Students TO User1;
```

🧠 **طريقة الحفظ**

> DCL = Doors 🔐

---

## 4️⃣ DQL – Data Query Language

### 📌 مسؤولة عن:

* عرض الداتا
* تحليلها

---

### 🔹 Commands

```sql
SELECT
```

* معاها:

- Aggregates
- Joins
- Subqueries
- Grouping
- Union

---

### 🔹 Example

```sql
SELECT COUNT(*) FROM Students;
```

🧠 **طريقة الحفظ**

> DQL = Display 👀

---

## 5️⃣ TCL – Transaction Control Language

### 📌 مسؤولة عن:

* Execution
* التحكم في العمليات

---

### 🔹 Commands

```sql
BEGIN TRANSACTION
COMMIT
ROLLBACK
```

---

### 🔹 Example

```sql
BEGIN TRANSACTION;

UPDATE Accounts SET Balance -= 100 WHERE Id = 1;
UPDATE Accounts SET Balance += 100 WHERE Id = 2;

COMMIT;
```

🧠 **طريقة الحفظ**

> TCL = Trust ✔️❌

---

# 🧠 الخلاصة

```
@@       → Global Variable
@        → Local Variable

DDL      → Structure
DML      → Data
DCL      → Security
DQL      → Display
TCL      → Transactions
```