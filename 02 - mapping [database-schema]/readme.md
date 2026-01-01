الملف ده بيشرح **تحويل ER Diagram إلى Relational Database**
من أول تعريفات الكيانات لحد كل أنواع العلاقات والـ attributes.

الهدف:
تفهم *إزاي أي رسمة ERD تتحول لجداول Database صح*.

---

# 1️⃣ Basic Relational Database Definitions

## Entity (Table)
الـ Entity هو كيان حقيقي في الواقع وليه بيانات.
لما بنحوّله لـ Database بيبقى Table.

أمثلة:
- Student
- Employee
- Course
- Department

---

## Attribute (Column)
الـ Attribute هو خاصية من خصائص الـ Entity.
ولما يتحول Database بيبقى Column.

---

## Record (Row / Tuple)
الـ Record هو صف واحد في الجدول
وبيمثل كيان واحد بس.

---

## Database
الـ Database هي مجموعة Tables مترابطة بعلاقات.

---

# 2️⃣ أنواع الـ Attributes في ERD وازاي نعملها Mapping

## 1️⃣ Simple Attribute
Attribute بسيط مش متقسم.

مثال:
- age
- salary

### Mapping:
- يتحول Column عادي في الجدول.

---

## 2️⃣ Composite Attribute
Attribute متقسم لأكتر من جزء.

مثال:
```

Name = (Fname, Lname)
Address = (Street, City)

````

### Mapping:
- بنفك الـ Composite Attribute
- وكل جزء يتحول Column لوحده
- الـ Attribute الكبير **مش بيتعمله Column**

مثال:
```text
STUDENT(
  student_id PK,
  fname,
  lname,
  street,
  city
)
````

---

## 3️⃣ Derived Attribute

Attribute بيتحسب من Attribute تاني.

مثال:

* age (متحسب من date_of_birth)

### Mapping:

* ❌ مش بيتخزن في Database
* ✔ بيتحسب وقت الحاجة

ليه؟
عشان نتجنب التكرار وعدم التناسق.

---

## 4️⃣ Multi-valued Attribute

Attribute ليه أكتر من قيمة.

مثال:

* phone_number
* skills

### Mapping:

* بنعمل Table جديد
* نحط Primary Key بتاع الـ Entity
* * الـ Attribute نفسه
* الاتنين = Composite PK

مثال:

```text
STUDENT_PHONE(
  student_id PK,
  phone_number PK
)
```

---

# 3️⃣ Types of Keys & Mapping

## Primary Key (PK)

* Unique
* Not Null
* ثابت

---

## Composite Primary Key

Primary Key مكوّن من أكتر من Attribute.

بيستخدم في:

* Weak Entity
* M:N Relationship
* Multi-valued Attribute

---

## Foreign Key (FK)

* Primary Key في Table
* Foreign Key في Table تاني
* بيربط الجداول ببعض

---

# 4️⃣ Types of Entities & Mapping

## 1️⃣ Strong Entity

كيان مستقل
ليه Primary Key خاص بيه.

### Mapping:

* Table مستقل
* Primary Key واضح

مثال:

```text
STUDENT(
  student_id PK,
  name,
  age
)
```

---

## 2️⃣ Weak Entity

كيان:

* مالوش Primary Key لوحده
* بيعتمد على Owner Entity

### Mapping:

1. نعمل Table
2. ناخد PK بتاع الـ Owner
3. نضيف Partial Key
4. الاتنين = Composite PK

مثال:

```text
DEPENDENT(
  employee_id PK,
  dependent_name PK,
  age
)
```

---

# 5️⃣ Relationship Mapping (الأهم)

## Step 0️⃣

### 1 : 1 Relationship + Total Participation من الطرفين

حالة نادرة.

### Mapping:

* ندمج الكيانين في Table واحد
* نحط كل Attributes مع بعض

ليه؟

* مفيش NULL
* أبسط وأكفأ

---

## Step 1️⃣ Mapping of Regular (Strong) Entity

* كل Strong Entity → Table
* Key → Primary Key
* Attributes → Columns

---

## Step 2️⃣ Mapping of Weak Entity

* Table
* Owner PK + Partial Key = Composite PK
* FK موجود ضمن الـ PK

---

## Step 3️⃣ Mapping of Binary 1 : 1 Relationship

العلاقة: واحد لواحد

### Mapping:

* نحط FK في واحد من الجدولين
* نفضل الجدول اللي:

  * Total Participation

---

## Step 4️⃣ Mapping of Binary 1 : N Relationship

العلاقة: واحد لمتعدد

### Mapping:

* PK بتاع (1)
* يتحط FK في Table (N)

مثال:

```text
STUDENT(
  student_id PK,
  dept_id FK
)
```

---

## Step 5️⃣ Mapping of Binary M : N Relationship

العلاقة: متعدد لمتعدد

### Mapping:

1. نعمل Table جديد
2. PK من الطرف الأول
3. PK من الطرف التاني
4. الاتنين = Composite PK
5. Attributes العلاقة تتحط هنا

مثال:

```text
ENROLLMENT(
  student_id PK,
  course_id PK,
  grade
)
```

---

## Step 6️⃣ Mapping of N-ary Relationship

العلاقة بين 3 كيانات أو أكتر.

### Mapping:

* Table جديد
* PKs لكل الكيانات
* Composite PK
* Attributes العلاقة

---

## Step 7️⃣ Mapping of Unary Relationship

العلاقة:

* Entity مرتبط بنفسه

### Mapping:

* FK في نفس الجدول
* يشاور على PK

مثال:

```text
EMPLOYEE(
  employee_id PK,
  manager_id FK
)
```

---

# 6️⃣ Participation Constraints & Mapping

## Total Participation

* الكيان لازم يدخل في العلاقة
* FK:

  * NOT NULL

## Partial Participation

* العلاقة اختيارية
* FK:

  * يسمح بـ NULL

---

# 7️⃣ Summary (الخلاصة الذهبية)

* ❗ Derived Attribute → مش بيتخزن
* ❗ Composite Attribute → نفكه
* ❗ Multi-valued Attribute → Table جديد
* ❗ Weak Entity → Composite PK
* ❗ M:N → Table جديد
* ❗ 1:N → FK في N
* ❗ 1:1 → FK أو دمج
* ❗ Unary → FK لنفس الجدول

---

الـ README ده يعتبر **مرجع كامل**
لو فهمته → أي ERD تتحول Database صح 💯

```
