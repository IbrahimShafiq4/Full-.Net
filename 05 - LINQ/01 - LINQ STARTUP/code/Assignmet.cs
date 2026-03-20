using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using System.Text;
using static LinQ01.ListGenerator;

namespace LinQ01
{
    // Use ListGenerators.cs & Customers.xml
    // 1. Find all products that are out of stock.
    public class Assignment
    {
        // Use ListGenerators.cs & Customers.xml
        // 1. Find all products that are out of stock. 
        internal static IEnumerable<Product> OutOfStockProducts()
        {
            return ProductsList.Where(product => product.UnitsInStock == 0);
        }

        // 2. Find all products that are in stock and cost more than 3.00 per unit. 
        internal static IEnumerable<Product> InStockProductsWithMoreThanThreeUnitsPer()
        {
            return ProductsList.Where(product => product.UnitsInStock != 0 && product.UnitPrice > 3.00M);
        }

        // 3. Returns digits whose name is shorter than their value. 
        internal static IEnumerable<int> DigitsWhosNameIsShorterThanTheirValue()
        {
            string[] digitNames = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            return Enumerable.Range(0, 10).Where(digit => digitNames[digit].Length < digit);
        }
    }
}
