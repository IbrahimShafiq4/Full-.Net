using System;
using System.Collections.Generic;
using System.Text;

namespace LinQ01
{
    //public class IntExtensions
    public static class IntExtensions // to make it work and remove on the extension method
        // we need to make it static cause the extension method must be inside a static class
    {
        //public static (string reversed, string original) Reverse(this int number)
        //{
        //    int originalNumber = number; // نحفظ القيمة الأصلية

        //    int reversedNum = 0, remainder;

        //    while (number != 0)
        //    {
        //        remainder = number % 10;
        //        reversedNum = reversedNum * 10 + remainder;
        //        number /= 10;
        //    }

        //    return ($"Reversed Number = {reversedNum}",
        //            $"Original Number = {originalNumber}");
        //}

        //public static int Reverse(int number) // in order to make it extension method
            // we need to path the this inside the parameter before the datatype
            // and this will mean that this Reverse will only be applied on the datatype comes after this
        public static int Reverse(this int number)
        {
            int reversedNum = 0, reminder;
            while(number != 0)
            {
                reminder = number % 10;
                reversedNum = reversedNum * 10 + reminder;
                number /= 10;
            }

            return reversedNum;
        }
        // so now we can go and do int.Reverse();
    }
}
