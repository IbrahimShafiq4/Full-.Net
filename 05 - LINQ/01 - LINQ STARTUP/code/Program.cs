using static LinQ01.ListGenerator;
namespace LinQ01
{
    internal class Program
    {
        public static void Print(object x)
        {
            Console.WriteLine(x);
        }

        // public static void Print(dynamic x) // it will cause an error which say
        // type program already defines a member called print with the same parameter type
        // {
        //    Console.WriteLine(x);
        // }

        // so dynamic is equivelent to the object
        
        static void Main(string[] args)
        {
            #region Implicitly Typed Local Variable [Var, Dynamic]
            // all of our work will be with the LinQ so We need to focus with it
            // Var is like the var of js but there is big difference between them
            // var contains any thing but there is small difference
            // var data = 22; // data in here will give us warning and if you stand on it it will tell us
            // you can define it with int
            // data = "Ibrahim"; // this cause an error why is that ?
            // cause the data has been detected as an integer so you can assign any values to it except integers values
            // so the compiler will detect variable datatype based on its initial value at the compilation time
            // ================================================================================================
            // what if i did the following
            // var name;
            // name = "ibrahim"; // this won't work you must initialize the variable at the begining
            // also you can't do that
            // ================================================================================================
            // var name = null; // this will cause an error too
            // cannot assign <null> to an implicitly-typed variable
            // why ?
            // cause the compiler will need to detect the datatype based on its initial value at the compilation time
            // after initialization you can't change the its datatype at all
            // to make the type Safty cause in here it is staticly datatype
            // ================================================================================================
            // dynamic data = 10; // dynamic is the opposite of var
            // the compiler deals with it in a different way
            // the compiler won't detect its datatype
            // it will detect the datatype based on the last value it had
            // but will be in the run time not on the compilation
            // so i can do this
            // data = "ibrahim";
            // and can do this
            // dynamic anything;
            // dynamic two = null;
            // so everything will be valid in here
            // so after initialization you can change the datatype
            // to get the its datatype
            // Console.WriteLine(((object)data).GetType());
            #endregion

            #region Extension Methods
            // if i want to add more functions on any datatype
            // int X = 10;
            // and when i say
            // X. you will find more methods
            // if i want to do one method 
            // and if i made X. to show a method Like the actual built in methods
            // now i want to make method that is an extension method
            // string name = "Ibrahim".Reverse().ToString(); if i want to make the reverse on the int

            // now after the IntExtension Class
            // int X = 123456;
            // in order to make work 
            // you can 
            // int result = IntExtensions.Reverse(X);
            //var result = X.Reverse();
            // Console.WriteLine(result);

            // and now after implementing the Extension method we can it like the following
            // Console.WriteLine(X.Reverse()); // and it will output it probably
            // as long as you made Extension and dealing as it for that
            // and you bind for it
            // you don't need to pass any parameters
            #endregion

            #region Anonymous Type
            // if i'm dealing with an object that is type being anonymous
            // if need to create an object from class Employee to store data in
            // if there is any data comes from any where and i want to stroe those data temporary
            // it just an object i store data inside
            // Employee employee = new Employee() { };
            // sometimes i don't need to Store data
            // and i don't want to make class for it
            // so we are going to do the following ...
            // var employee01 = new { Id = 1, Name = "Ibrahim", Salary = 5000 };
            // var employee02 = new { Id = 1, NAME = "Ibrahim", Salary = 5000 }; // this is another type of employee01
            // Console.WriteLine(employee01);
            // and in larger systems this could cause alot of misunderstanding
            // now if i did this

            // Console.WriteLine(employee01.GetType().Name); // this will output AnonymousType0`3 // 3 will refere to the number of properties i've entered
            // 0 will refere to that is the first anonmous type i've of the sameType
            // it will output Same Anonymous Type as long as 
            // 1. same properties name (Case sensitive)
            // 2. same properties order
            // also the set of the anonymouse type is private
            // like if i did
            // employee01.Salary = 5000; // will cause an error
            // inside the ILyspy you will find the anonymous type as a class
            // cause it will be using the init accessor chat talk about it deeply please
            // to make it change
            // var em03 = employee02 with { Salary = 700 };
            // and if i did the following
            // Console.WriteLine(employee02.Salary); will output 5000
            // cause you havn't update on the employee02 itself
            // you updated it accross the em03 
            // and if you made Console.WriteLine(em03.Salary); will output 700
            // but if you wanted to update it 
            // you will make like this
            // employee02 = new { Id = 1, NAME = "Ibrahim", Salary = 7000
            #endregion

            #region LINQ introduction
            // Linq => Language Integrated Query
            // anything we've made using the select with SQL
            // but we will type c# code
            // here how we will deal with the data ?
            // in SQL we were dealing with tables
            // but in here we are dealing Collections or sequence
            // LINQ has +40 Extension Methods  WE CAN FIND THEM INSIDE (CLASS THAT IMPLEMENTING IEnumerable Interface)
            // THOSE CALLED [Query Operators] existing at Enumerable class and categorized into 13 category
            // what is usefull with the LINQ
            // the concept of integeration
            // dealing with any SQL server
            // like mongo db, ms sql , mysql
            // you write c# and the code you've wrote the compiler will go and deal with the database
            // is database understand the code i will write ?
            // no 
            // database will transform the code into OPTIMIZED SQL
            // it will change the query with the best LOOK
            // LINQ oeprators can be used agains Data (Sequence) regerdless of the data source 
            // sequnece => any class implementing IEnumerable => List, Array, Dictionary, etc...
            // any one implements the IEnumerable
            // there is two types of sequence
            // 1. Local Sequence: L2O [Linq to object], L2XML [Linq to xml]
            // 2. Remote sequence: L2E Linq to Entity

            // ==============================================
            #region Linq Syntax
            // 1. Fluent Syntax
            // 1.1  Calling LINQ Method as static method we call through the Enumerable Class
            //      we make them static cause of we can't make extension method unless if it is inside non-generic static class
            // List<int> numbers =new List<int>() { 1, 2, 3, 4,5 ,6, 7, 8, 9, 10, 11, 12};
            // var result = Enumerable.Where(numbers, (x) => x % 2 == 0);
            // 1.2 CAlling LINQ method as Extension Method this is recommended
            // cause there is operators doesn't work unless if used teh calling Linq Method
            // result = numbers.Where<int>((x) => x % 2 == 0);
            // Console.WriteLine(result);

            // 2. Query Syntax [Query Expression] => Like Sql Query
            // start with from introducing Range variable : represents each element at sequence that i'm dealing with
            // ending with either select or group by
            // result = from num in numbers
            //         where num % 2 == 0
            //         select num;
            // which syntax should i use ?
            // sometimes in some cases Query Syntx will be better
            // Like Join , Group, etc..
            // sometimes and the fluent syntax will be better

            #endregion
            // ==============================================
            #region LINQ Excution
            // List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            // var result = numbers.Where<int>(num => num % 2 == 0);
            // numbers.Remove(2); // i removed number 2 from the list
            // numbers.AddRange<int>(new int[] { 13, 14, 15, 16, 17, 18, 19, 20 }); // this will add those to actual List like appending the two together
            // foreach(int num in result) // and the result will contain both of the the two range
            //     // and the conditional operation will happen on both from 1 to 20
            //     Console.Write($"{num} "); // this will output the numbers to be next to each other

            // what actually happen on running this ?
            // 1 - Deffered Execution (Latest Update of Data)
            // it will remove from the numbers num 2
            // then add the newRange to The Actual List
            // then it starts Executioning the Conditional Query
            // then it outputs the data
            // it will waits untill you use the resutl

            // All LINQ Operators Works the same way but only three Operators
            // Element, Aggergate, Casting Operators

            // 2 - Immediate Execution
            // i'm working on the same last example
            // List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            // var result = numbers.Where<int>(num => num % 2 == 0);
            // if i wanted to make to be an Immediate Execution
            // in this example i'm going to make Casting Opeators
            // i will make the Where conditional Operators to becomes a list
            // var result = numbers.Where<int>(num => num % 2 == 0).ToList();
            // where uses the Predicate Delegate which is Taking Predicate takes an any input typedata,
            // and returns anydata type
            // now this numbers.Where().ToList() A List which will make it work as an Immediate Execution
            // that was because of the ToList() opertator which made it as a List and it will work Immediate not like the Deffered Operator
            // and it won't going to see the upcoming Lines 
            // numbers.Remove(2);
            // numbers.AddRange<int>(new int[] { 13, 14, 15, 16, 17, 18, 19, 20 });
            // foreach (int num in result)
            //     Console.WriteLine(num);
            #endregion
            #endregion

            #region Filteration (Restrications) : Operator => Where
            // i'm going to use the ListGenerator.Cs File
            // i need to get the Out of stock Products
            // this will be inside the product class which called UnitsInStock
            // i'm going to do it with the Fluent Syntax

            // var result = ProductsList.Where(product => product.UnitsInStock == 0);
            // foreach (var outOfStockProduct in result)
            //     Console.WriteLine(outOfStockProduct);

            // i want to do the same but with the Query Syntax  
            // IEnumerable<Product> result = from product in ProductsList
            //              where product.UnitsInStock == 0
            //              select product;

            // adding more conditions
            //IEnumerable<Product> ConditionalProducts = ProductsList.Where<Product>(product => product.UnitsInStock == 0 && product.Category == "Meal/Poultry");
            //ConditionalProducts = from product in ProductsList
            //                      where product.UnitsInStock == 0 && product.Category == "Meat/Poultry"
            //                      select product;

            // Indexed Where
            // if i want to Look for the UnitsOfStock == 0 in only the first 10 Products
            // var result = ProductsList.Where((product, index) => product.UnitsInStock == 0 && index <= 10);
            // this was valid only at the fluent syntax , can't be written using the query syntax
            // and this where was an overload for the where 
            // the first where takes the element only
            // the second where takes the element , index
            // foreach (var OutOfStockProduct in result)
            //    Console.WriteLine(OutOfStockProduct);
            #endregion
        }
    }
}
