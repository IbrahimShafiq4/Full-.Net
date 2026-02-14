# C# Practice Problems with Solutions

---

## 01 - Read and Print a Number

```csharp
Console.WriteLine("Please enter a number: ");
int UNum;
bool success = int.TryParse(Console.ReadLine(), out UNum);

if (success)
    Console.WriteLine($"You entered: {UNum}");
else
    Console.WriteLine("Invalid input! Please enter a valid number.");
```

**Explanation:**
`int.TryParse` safely parses the input and avoids exceptions if the input is invalid.

---

## 02 - Convert String to Integer

```csharp
Console.WriteLine("Enter a string to convert to integer: ");
string input = Console.ReadLine()!;

try
{
    int num1 = int.Parse(input);
    int num2 = Convert.ToInt32(input);
    Console.WriteLine($"Parsed with int.Parse: {num1}");
    Console.WriteLine($"Parsed with Convert.ToInt32: {num2}");
}
catch (FormatException)
{
    Console.WriteLine("Invalid input! Cannot convert string to integer.");
}
```

**Explanation:**

* `int.Parse` and `Convert.ToInt32` throw `FormatException` for non-numeric strings.
* `TryParse` is safer when input may be invalid.

---

## 03 - Floating-Point Arithmetic

```csharp
double x = 0.1, y = 0.2;
Console.WriteLine($"x + y = {x + y}");
```

**Explanation:**

* Floating-point arithmetic can produce small precision errors. Output may not be exactly `0.3`.

---

## 04 - Extract a Substring

```csharp
string st = "Hello World";
string subSt = st.Substring(0, 5); // Extracts "Hello"
Console.WriteLine(subSt);
```

**Explanation:**

* `Substring(startIndex, length)` extracts a part of the string.

---

## 05 - Value Type Assignment

```csharp
int a = 10;
int b = a;
b = 20;
Console.WriteLine($"a = {a}, b = {b}");
```

**Explanation:**

* Value types (like int) are copied. Changing `b` does **not** affect `a`.

---

## 06 - Reference Type Assignment

```csharp
int[] arr01 = { 1, 2, 3 };
int[] arr02 = arr01;
arr01[0] = 8;
Console.WriteLine(arr02[0]);
```

**Explanation:**

* Reference types point to the same object. Modifying `arr01` also affects `arr02`.

---

## 07 - Concatenate Strings

```csharp
string st1 = "Hello", st2 = "World";
string st3 = st1 + " " + st2;
Console.WriteLine(st3);
```

**Output:** `Hello World`

---

## 08 - Boolean to Integer

```csharp
int d;
d = Convert.ToInt32(!(30 < 20));
Console.WriteLine(d);
```

**Explanation:**

* `30 < 20` → `false`
* `!false` → `true`
* `Convert.ToInt32(true)` → `1`

**Output:** `1`

---

## 09 - Integer Division and Modulus

```csharp
Console.WriteLine(13 / 2 + " " + 13 % 2);
```

**Explanation:**

* `13 / 2` → integer division → `6`
* `13 % 2` → remainder → `1`

**Output:** `6 1`

---

## 10 - Complex Increment/Decrement

```csharp
int num = 1, z = 5;
if (!(num <= 0))
    Console.WriteLine(++num + z++ + " " + ++z);
else
    Console.WriteLine(--num + z-- + " " + --z);
```

**Explanation:**

1. `!(num <= 0)` → true → enter `if` block
2. `++num` → 2
3. `z++` → 5 (then z becomes 6)
4. `++z` → 7
5. Output → `"7 7"`

---

## 11 - Swap Two Numbers Without Temp

```csharp
int a = 5, b = 10;
a = a + b;
b = a - b;
a = a - b;
Console.WriteLine($"a = {a}, b = {b}");
```

**Output:** `a = 10, b = 5`
**Explanation:** Swaps numbers without using a temporary variable.

---

## 12 - Check Even or Odd

```csharp
Console.Write("Enter a number: ");
int number = int.Parse(Console.ReadLine()!);

if (number % 2 == 0)
    Console.WriteLine($"{number} is Even");
else
    Console.WriteLine($"{number} is Odd");
```

---

## 13 - Count Vowels and Consonants

```csharp
Console.Write("Enter a string: ");
string text = Console.ReadLine()!.ToLower();
int vowels = 0, consonants = 0;

foreach (char c in text)
{
    if ("aeiou".Contains(c))
        vowels++;
    else if (char.IsLetter(c))
        consonants++;
}

Console.WriteLine($"Vowels: {vowels}, Consonants: {consonants}");
```

---

## 14 - Sum and Average of Array

```csharp
int[] arr = { 1, 2, 3, 4, 5 };
int sum = 0;

foreach (int val in arr)
    sum += val;

double avg = (double)sum / arr.Length;
Console.WriteLine($"Sum = {sum}, Average = {avg}");
```

---

## 15 - Reverse a String

```csharp
Console.Write("Enter a string: ");
string inputStr = Console.ReadLine()!;
string reversed = "";

for (int i = inputStr.Length - 1; i >= 0; i--)
    reversed += inputStr[i];

Console.WriteLine($"Reversed string: {reversed}");
```

---

## 16 - Find Largest Number in Array

```csharp
int[] nums = { 10, 25, 7, 99, 3 };
int max = nums[0];

foreach (int n in nums)
    if (n > max) max = n;

Console.WriteLine($"Largest number is {max}");
```

---

## 17 - Simple Calculator

```csharp
Console.Write("Enter first number: ");
double n1 = double.Parse(Console.ReadLine()!);
Console.Write("Enter second number: ");
double n2 = double.Parse(Console.ReadLine()!);

Console.WriteLine("Enter operator (+, -, *, /): ");
char op = Console.ReadLine()![0];

double result = op switch
{
    '+' => n1 + n2,
    '-' => n1 - n2,
    '*' => n1 * n2,
    '/' => n2 != 0 ? n1 / n2 : 0,
    _ => 0
};

Console.WriteLine($"Result: {result}");
```
