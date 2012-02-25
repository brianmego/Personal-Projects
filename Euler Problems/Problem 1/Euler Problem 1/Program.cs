using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Euler_Problem_1
{
    class Program
    {   
        static void Main(string[] args)
        {
            List<int> numbersBelow1000 = new List<int>();
            int sum = 0;

            for (int num = 1; num < 1000; num++)
            {
                if (num % 3 == 0)
                    numbersBelow1000.Add(num);
                else if (num % 5 == 0)
                    numbersBelow1000.Add(num);
            }

            foreach (int num in numbersBelow1000)
                sum += num;

            Console.WriteLine(sum);
            Console.Read();
        }
    }
}
