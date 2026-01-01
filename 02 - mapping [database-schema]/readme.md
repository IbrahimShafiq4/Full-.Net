# 1️⃣ Types of Attributes & Mapping

## 1️⃣ Simple Attribute
Attribute بسيط مش متقسم.

مثال:
- age
- salary

### Mapping:
- Column عادي في الجدول.

---

## 2️⃣ Composite Attribute
Attribute متقسم لأجزاء.

مثال:
```

Name → (Fname, Lname)
Address → (Street, City)

````

### Mapping:
- نفك الـ Composite
- كل جزء Column
- الـ Attribute الكبير **مش بيتخزن**

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

Attribute بيتحسب من غيره.

مثال:

* age (من date_of_birth)

### Mapping:

* ❌ لا يتحول Column
* ✔ يتحسب وقت الحاجة

---

## 4️⃣ Multi-valued Attribute

Attribute ليه أكتر من قيمة.

مثال:

* phone_number
* skills

### Mapping:

* Table جديد
* PK بتاع الـ Entity
* * Attribute
* الاتنين = Composite PK

مثال:

```text
STUDENT_PHONE(
  student_id PK,
  phone_number PK
)
```

---

# 2️⃣ Types of Entities & Mapping

## Strong Entity

* كيان مستقل
* ليه PK خاص بيه

### Mapping:

* Table عادي

---

## Weak Entity

* مالوش PK لوحده
* بيعتمد على Owner

### Mapping:

* PK Owner + Partial Key
* Composite PK

مثال:

```text
DEPENDENT(
  employee_id PK,
  dependent_name PK,
  age
)
```

---

# 3️⃣ Relationship Mapping – ALL CASES

## 🔹 1 : 1 Relationship

---

### 1️⃣ 1:1 (Partial , Partial)

يعني:

* العلاقة اختيارية من الطرفين

### Mapping:

* نحط FK في أي Table
* FK يسمح بـ NULL

مثال:

```text
PERSON(
  person_id PK,
  passport_id FK NULL
)
```

---

### 2️⃣ 1:1 (Total , Partial)

يعني:

* طرف لازم يدخل
* طرف اختياري

### Mapping:

* FK يتحط في طرف الـ Total
* FK NOT NULL

---

### 3️⃣ 1:1 (Total , Total)

يعني:

* الطرفين لازم يدخلوا

### Mapping (أفضل حل):

* ندمج الكيانين في Table واحد

### حل بديل:

* FK NOT NULL + UNIQUE

---

## 🔹 1 : M Relationship

---

### 4️⃣ 1:M (Partial , Partial)

يعني:

* الطرفين اختياريين

### Mapping:

* FK يتحط في M
* FK يسمح بـ NULL

---

### 5️⃣ 1:M (Total , Partial)

يعني:

* الـ M لازم يدخل
* الـ 1 اختياري

### Mapping:

* FK في M
* FK NOT NULL

---

### 6️⃣ 1:M (Partial , Total)

يعني:

* الـ 1 لازم
* الـ M اختياري

### Mapping:

* FK في M
* يسمح بـ NULL

---

### 7️⃣ 1:M (Total , Total)

يعني:

* الطرفين لازم

### Mapping:

* FK في M
* FK NOT NULL

---

## 🔹 M : N Relationship

> في كل حالات M:N → لازم Table جديد

---

### 8️⃣ M:N (Partial , Partial)

### Mapping:

* Table جديد
* PK من الطرفين
* Composite PK
* FK يسمح بـ NULL لو العلاقة اختيارية

---

### 9️⃣ M:N (Total , Partial)

### Mapping:

* Table جديد
* Composite PK
* FK بتاع الطرف Total → NOT NULL

---

### 🔟 M:N (Partial , Total)

### Mapping:

* Table جديد
* Composite PK
* FK بتاع الطرف Total → NOT NULL

---

### 1️⃣1️⃣ M:N (Total , Total)

### Mapping:

* Table جديد
* Composite PK
* كل FKs NOT NULL

مثال:

```text
ENROLLMENT(
  student_id PK,
  course_id PK,
  grade
)
```

---

# 4️⃣ Unary Relationship (Self Relationship)

### Partial

* FK يسمح بـ NULL

### Total

* FK NOT NULL

مثال:

```text
EMPLOYEE(
  employee_id PK,
  manager_id FK
)
```

---

# 5️⃣ Participation Rules Summary

| Participation | FK Rule      |
| ------------- | ------------ |
| Partial       | NULL allowed |
| Total         | NOT NULL     |

---

# 6️⃣ Golden Rules

* Derived Attribute → يتحسب وقت ال run time
* Composite Attribute → نفكه
* Multi-valued Attribute → Table جديد
* Weak Entity → Composite PK
* 1:1 Total Total → دمج
* 1:M → FK في M
* M:N → Table جديد
* Total Participation → NOT NULL
* Partial Participation → NULL allowed