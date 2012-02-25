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
            List<long> foundPrimes = new List<long>();
            int targetPrimeIndex = 10001;
            long counter = 2;
            while (foundPrimes.Count < targetPrimeIndex)
            {
                if (isPrime(counter) == true)
                    foundPrimes.Add(counter);
                counter += 1;
            }

            Console.WriteLine(foundPrimes[targetPrimeIndex - 1]);
        }

        private static bool isPrime(long numToTest)
        {
            for (long i = 2; i <= numToTest / 2; i++)
            {
                if (numToTest % i == 0)
                {
                    return false;
                }
            }
            return true;
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
