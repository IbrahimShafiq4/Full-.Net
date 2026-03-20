using System;
using System.Collections.Generic;
using System.Text;

namespace LinQ01
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public override string ToString() => $"Id= {Id}, Name = {Name} , Salary = {Salary}";
    }
}
