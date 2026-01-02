# 🧠 SQL Server Data Types – Cheat Sheet

---

## 🔢 Numeric (أرقام صحيحة)

| Data Type  | الحجم   | الرينج     | تستخدمه إمتى     |
| ---------- | ------- | ---------- | ---------------- |
| `bit`      | 1 bit   | 0 / 1      | True / False     |
| `tinyint`  | 1 Byte  | 0 → 255    | سن صغير / Status |
| `smallint` | 2 Bytes | -32K → 32K | أرقام متوسطة     |
| `int`      | 4 Bytes | ±2 مليار   | IDs / PK         |
| `bigint`   | 8 Bytes | كبير جدًا  | Counters ضخمة    |

🧠 **حفظ سريع**

> ID = int
> Boolean = bit

---

## 💰 Fraction / Decimal (أرقام بكسور)

| Data Type      | الحجم     | الدقة   | ملاحظات    |
| -------------- | --------- | ------- | ---------- |
| `smallmoney`   | 4 Bytes   | 4 أرقام | فلوس بسيطة |
| `money`        | 8 Bytes   | 4 أرقام | مش مفضل    |
| `real`         | 4 Bytes   | ~7      | مش دقيق    |
| `float`        | 4/8 Bytes | ~15     | مش للفلوس  |
| `decimal(p,s)` | حسب p     | ثابت    | ✅ الأفضل   |

🧠 **حفظ سريع**

> فلوس = decimal
> دقة عالية = decimal
> تقريب = float

---

## 🔤 String (نصوص)

| Data Type       | الطول | لغة     | تستخدمه إمتى |
| --------------- | ----- | ------- | ------------ |
| `char(n)`       | ثابت  | EN      | طول ثابت     |
| `varchar(n)`    | متغير | EN      | نص عادي      |
| `nchar(n)`      | ثابت  | Unicode | عربي         |
| `nvarchar(n)`   | متغير | Unicode | عربي         |
| `varchar(max)`  | 2GB   | EN      | نص طويل      |
| `nvarchar(max)` | 2GB   | Unicode | وصف طويل     |

🧠 **حفظ سريع**

> عربي = nvarchar
> إنجليزي = varchar
> ثابت = char

---

## ⏰ Date & Time

| Data Type        | بيخزن إيه   | الأفضلية |
| ---------------- | ----------- | -------- |
| `date`           | تاريخ بس    | ✔️       |
| `time(n)`        | وقت بس      | ✔️       |
| `smalldatetime`  | تاريخ + وقت | ❌        |
| `datetime`       | تاريخ + وقت | ❌        |
| `datetime2(n)`   | تاريخ + وقت | ✅ الأفضل |
| `datetimeoffset` | + Timezone  | للسفر 🌍 |

🧠 **حفظ سريع**

> DateTime حديث = datetime2

---

## 🧬 Binary

| Data Type        | ملاحظة        |
| ---------------- | ------------- |
| `binary`         | Bits          |
| `image`          | ❌ قديم        |
| `varbinary(max)` | ✔️ بديل image |

---

## 🧩 Other

| Data Type     | شبه إيه   |
| ------------- | --------- |
| `XML`         | ملف XML   |
| `sql_variant` | var في JS |

---

## 🏆 أفضل اختيار (Golden Rules)

```
Primary Key        → int
Boolean            → bit
Money              → decimal(10,2)
Arabic Text        → nvarchar
English Text       → varchar
DateTime           → datetime2
Long Text          → nvarchar(max)
Image/File         → varbinary(max)
```

---

## 🧠 جملة سحرية للحفظ

> **"ID int – فلوس decimal – عربي nvarchar – وقت datetime2"**