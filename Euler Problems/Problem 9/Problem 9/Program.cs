using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            bool runProgram = true;
            while (runProgram == true)
            {
                initialize();
                if (queryAgain() == false)
                    break;
            }
        }

        private static void initialize()
        {
            
        }

        private static bool queryAgain()
        {
            Console.WriteLine("Again? ");
            string again = Console.ReadLine();
            if (again == "y")
            {
                return true;
            }
            return false;
        }

        private static int product(int a, int b, int c)
        {
            return (a * b * c);
        }
    }
}
