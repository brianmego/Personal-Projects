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
            int lowerBound = 1;
            int upperBound = 100;

            long sumOfTheSquares = 0;
            long sum = 0;
            long squareOfTheSums = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                sumOfTheSquares += (i * i);
                sum += i;
            }
            squareOfTheSums = (sum * sum);

            Console.WriteLine("Sum of the Squares: ");
            Console.WriteLine(sumOfTheSquares + "\n\n");
            Console.WriteLine("Square of the Sums: ");
            Console.WriteLine(squareOfTheSums + "\n\n");

            Console.WriteLine("Difference: ");
            Console.WriteLine(squareOfTheSums - sumOfTheSquares + "\n\n");
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
