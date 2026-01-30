## Stored Procedures, Triggers, Transactions, Indexes, Security & Comparison

---

# ===============================

# 🔹 TYPES OF STORED PROCEDURES

# ===============================

## 1️⃣ Built-in Stored Procedures

### 📌 يعني إيه Built-in SP؟

* Stored Procedures جاهزة من SQL Server
* معمولة لإدارة:

  * Metadata
  * Objects
  * Performance
  * Debugging

### أمثلة مهمة جدًا:

```sql
SP_HELPTEXT 'ObjectName'
```

📌 تستخدم عشان:

* تشوف كود View / Function / SP
* ❌ لا تعمل مع Tables

---

```sql
SP_RENAME 'OldName', 'NewName'
```

📌 بديل احترافي لـ:

```
Right Click → Rename
```

✔ Safe
✔ Scriptable
✔ Production-friendly

---

## 2️⃣ User-Defined Stored Procedures

### 📌 دي اللي إنت بتعملها

```sql
SP_GetStudentById
```

### تستخدم في:

* APIs
* Business Logic
* CRUD Operations
* Reports

✔ Performance
✔ Security
✔ Clean Architecture

---

## 3️⃣ Triggers (Special Stored Procedures)

### 📌 يعني إيه Trigger؟

* Stored Procedure **مش بتتنادي**
* بتشتغل تلقائيًا لما Event يحصل

📌 شبه جدًا:

```js
addEventListener('click', ...)
```

---

### ❗ Rules مهمة:

| Rule                    | Value |
| ----------------------- | ----- |
| Can be called manually  | ❌     |
| Accept parameters       | ❌     |
| Fires automatically     | ✔     |
| Runs inside transaction | ✔     |

---

# ===============================

# 🔹 TRIGGERS (TABLE LEVEL)

# ===============================

## 📌 Events

* INSERT
* UPDATE
* DELETE

---

## AFTER INSERT Trigger

```sql
CREATE TRIGGER WelcomeMessage
ON Student
AFTER INSERT
AS
	PRINT 'Welcome To the New Student'
```

### 📌 Use cases:

* Logging
* Notifications
* Audit Tables

⚠️ **مهم جدًا**

* Trigger بيشتغل مرة واحدة حتى لو Insert multiple rows

---

## BEFORE / AFTER Concept

SQL Server:

* ❌ BEFORE
* ✔ AFTER
* ✔ INSTEAD OF

---

## AFTER UPDATE (Audit Example)

```sql
ALTER TRIGGER InfoAboutUpdate
ON Student
AFTER UPDATE
AS
	SELECT 
		FORMAT(GETDATE(), 'dd-MM-yyyy hh tt') AS UpdateDate,
		SUSER_NAME() AS UserName;
```

📌 Production use:

* Audit Trail
* Compliance
* Tracking changes

---

## INSTEAD OF DELETE

```sql
CREATE TRIGGER PreventDeleting
ON Student
INSTEAD OF DELETE
AS
	PRINT 'You can not delete from this table'
```

📌 يمنع أي Delete نهائيًا
📌 يستخدم مع:

* Financial Tables
* Logs
* Sensitive Data

---

### ❗ Rule مهم:

* ❌ لا يمكن وجود أكثر من `INSTEAD OF DELETE`
* ✔ ممكن أكثر من `AFTER INSERT`

---

## Disable / Enable Trigger (مهم جدًا في الشغل)

```sql
ALTER TABLE Student DISABLE TRIGGER PreventAllActions;
ALTER TABLE Student ENABLE TRIGGER PreventAllActions;
```

📌 يستخدم في:

* Data Migration
* Bulk Insert
* Fixing Production Issues

---

# ===============================

# 🔹 INSERTED & DELETED TABLES

# ===============================

## 📌 Runtime Tables

SQL Server بيخلق تلقائيًا:

* `inserted`
* `deleted`

### حسب الحدث:

| Event  | inserted | deleted |
| ------ | -------- | ------- |
| INSERT | ✔ new    | ❌       |
| DELETE | ❌        | ✔ old   |
| UPDATE | ✔ new    | ✔ old   |

---

### Example:

```sql
CREATE TRIGGER UpdateCourse
ON Course
AFTER UPDATE
AS
	SELECT * FROM inserted
	SELECT * FROM deleted
```

📌 تستخدم في:

* History Tables
* Audit Logs
* Sync Systems

---

## Prevent Delete with Message

```sql
CREATE TRIGGER PreventDeletingCourse
ON Course
INSTEAD OF DELETE
AS
	SELECT 
		'You can not Delete Course With name ' 
		+ (SELECT Crs_Name FROM deleted) AS Message
```

✔ User-Friendly
✔ API-Friendly

---

## Transfer Deleted Data (Soft Delete Pattern)

```sql
CREATE TRIGGER TransferDeletedCourses
ON Course
AFTER DELETE
AS
	INSERT INTO DeletedCourses
	SELECT * FROM deleted;
```

📌 ده **Enterprise Pattern**

* No data loss
* Full history

---

## Conditional Delete (Business Rule)

```sql
IF DATEPART(WEEKDAY, GETDATE()) <> 4
```

📌 `DATEFIRST`:

* بيحدد أول يوم في الأسبوع
* افتراضيًا:

  * Sunday = 1
  * Wednesday = 4

---

```sql
RAISERROR('Deleting is not allowed on wednesday', 16, 1)
```

📌

* Level 16 = User Error
* يظهر للـ API مباشرة

---

## View All Triggers

```sql
SELECT 
	name,
	is_disabled,
	is_instead_of_trigger
FROM sys.triggers
WHERE parent_id = OBJECT_ID('Student');
```

📌 Production Debugging Tool 🔥

---

# ===============================

# 🔹 TRANSACTIONS (TCL)

# ===============================

## Implicit Transaction

* أي INSERT / UPDATE / DELETE
* كل Statement لوحده

---

## Explicit Transaction

* All or Nothing

```sql
BEGIN TRANSACTION
	INSERT ...
	INSERT ...
COMMIT
```

---

## ROLLBACK

```sql
ROLLBACK TRANSACTION
```

📌 يرجّع DB للحظة البداية

---

## TRY / CATCH + SAVEPOINT

```sql
SAVE TRANSACTION SavePoint;
```

📌 نقطة رجوع جزئية
📌 مفيدة جدًا في:

* Complex Business Logic
* Multi-Steps Process

---

# ===============================

# 🔹 INDEXES

# ===============================

## Why Index?

* Search Speed
* Performance
* Scalability

---

## Clustered Index

| Feature               | Value |
| --------------------- | ----- |
| One per table         | ✔     |
| Physically sorts data | ✔     |
| Default on PK         | ✔     |
| Fastest               | ✔     |

📌 Binary Search بدل Linear

---

## Non-Clustered Index

| Feature            | Value |
| ------------------ | ----- |
| Up to 999          | ✔     |
| Separate structure | ✔     |
| Pointer to data    | ✔     |

📌 Faster than no index
📌 Slower than clustered

---

## When NOT to use Index?

* TEXT / NVARCHAR(MAX)
* Columns with high duplication
* Tables with heavy INSERT/UPDATE

---

## SQL Server Profiler (Production Tip 🔥)

### Steps:

1. Run SQL Server Profiler
2. Capture workload
3. Save trace
4. Open Database Engine Tuning Advisor
5. Analyze
6. Create indexes based on usage

📌 **ده اللي بيحصل في الشركات بجد**

---

# ===============================

# 🔹 INDEXED VIEW

# ===============================

## 📌 Indexed View

* View + Clustered Index
* Data stored physically

### شروط:

* WITH SCHEMABINDING
* Explicit schema
* COUNT_BIG()

---

### Example:

```sql
CREATE VIEW SalesOrderSummary
WITH SCHEMABINDING
AS
	SELECT 
		SH.SalesOrderID,
		COUNT_BIG(*) OrderCount,
		SUM(SD.LineTotal) OrderTotal
	FROM Sales.SalesOrderHeader SH
	JOIN Sales.SalesOrderDetail SD
	ON SH.SalesOrderID = SD.SalesOrderID
	GROUP BY SH.SalesOrderID;
```

✔ Reporting
✔ Aggregations
✔ Heavy queries

---

# ===============================

# 🔥 VIEW vs FUNCTION vs STORED PROCEDURE

# ===============================

| Feature           | VIEW   | FUNCTION       | STORED PROCEDURE |
| ----------------- | ------ | -------------- | ---------------- |
| Return Type       | Table  | Scalar / Table | Any              |
| Parameters        | ❌      | ✔              | ✔                |
| DML               | ❌      | ❌              | ✔                |
| Performance       | Medium | Slow           | Fastest          |
| Can use TRY/CATCH | ❌      | ❌              | ✔                |
| Use in SELECT     | ✔      | ✔              | ❌                |
| Security          | Medium | Medium         | High             |
| .NET Usage        | Good   | Limited        | Best             |

### 🏆 Winner for .NET APIs:

**Stored Procedures**

---

# ===============================

# 🔐 DCL (PERMISSIONS)

# ===============================

## Permissions Types

* GRANT
* DENY
* REVOKE

```sql
GRANT SELECT ON StudentsView TO ApiUser;
DENY DELETE ON Student TO ApiUser;
```

📌 Best Practice:

* Grant on Views / SP
* ❌ Never grant on Tables

---

# ===============================

# 💾 BACKUP & SCRIPTS

# ===============================

## Backup Types

| Type         | Use              |
| ------------ | ---------------- |
| Full         | Daily            |
| Differential | Between full     |
| Log          | Critical systems |

---

## Generate Scripts

SSMS:

```
Right Click DB → Tasks → Generate Scripts
```

✔ Deploy
✔ CI/CD
✔ Version Control
