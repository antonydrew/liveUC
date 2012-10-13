using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payroll
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Test each class by entering employee via constructor and printing details
            HourlyEmployee Hire1 = new HourlyEmployee("Joe","Dunn","070723889",1000,40);
            Console.WriteLine(Hire1);
            Console.WriteLine("{0}\t {1}\n","Total earnings:",Hire1.Earnings());
         

            SalariedEmployee Hire2 = new SalariedEmployee("John", "Rex", "970723889", 80000);
            Console.WriteLine(Hire2);
            Console.WriteLine("{0}\t {1}\n", "Total earnings:", Hire2.Earnings());

            BasePlusCommissionEmployee Hire3 = new BasePlusCommissionEmployee("Reg", "Smith", "370723889", 10000, 1/5, 40000);
            Console.WriteLine(Hire3);
            Console.WriteLine("{0}\t {1}\n", "Total earnings:", Hire3.Earnings());

            CommissionEmployee Hire4 = new CommissionEmployee("Lisa", "Pepper", "170723889", 100000, 1 / 5);
            Console.WriteLine(Hire4);
            Console.WriteLine("{0}\t {1}\n", "Total earnings:", Hire4.Earnings());

           
            

            Console.Read();


        }
    }
}
