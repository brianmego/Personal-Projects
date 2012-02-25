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
            short lowerBound = 1;
            short upperBound = 20;

            int answer = findEvenlyDivisibleTarget(lowerBound, upperBound);
            Console.WriteLine(answer);
        }

        private static int findEvenlyDivisibleTarget(short lowerBound, short upperBound)
        {
            for (int i = upperBound; i <= 1000000000; i++)
            {

                for (short j = lowerBound; j <= upperBound; j++)
                {
                    if (i % j != 0)
                        break;
                    if (j == upperBound)
                    {
                        return i;
                    }
                }
               
            }
            return 0;
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
    }
}
