using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            long numberToFactor = getNumberToFactor();

            List<long> allFactors = calculateAllFactors(numberToFactor);

            long largestPrime = checkForPrimes(allFactors);
            if (largestPrime == 0)
            {
                Console.WriteLine("No Prime Factors");
            }
            else
            {
                Console.WriteLine("Largest Prime = " + largestPrime);
            }

        }

        private static long getNumberToFactor()
        {
            Console.WriteLine("Number to factor: ");
            //try
            //{
                long numberToFactor = Convert.ToInt64(Console.ReadLine());
            //}
            //catch (FormatException e)
            //{
            //    Console.WriteLine("Please enter a number\n");

            //}

            Console.WriteLine("\n\n");
            return numberToFactor;
        }

        private static List<long> calculateAllFactors(long numberToFactor)
        {
            List<long> factorsSeen = new List<long>();
            bool allFactorsFound = false;

            for (int i = 2; i <= (numberToFactor / 2); i++)
            {
                if (factorsSeen.Count != 0)
                {
                    foreach (long factor in factorsSeen)
                    {
                        if (i >= (numberToFactor / factor))
                        {
                            allFactorsFound = true;
                        }
                    }
                }

                if (allFactorsFound == true)
                { break; }

                if (numberToFactor % i == 0)
                {
                    factorsSeen.Add(i);
                    //Console.Write(i + "\n");
                }
            }

            //Console.Write("\n\n");

            List<long> allFactors = new List<long>();
            foreach (long factor in factorsSeen)
            { 
                allFactors.Add(factor);
                allFactors.Add(numberToFactor / factor);
            }
            
            allFactors.Sort();

            //foreach (long factor in allFactors)
            //{
            //    Console.Write(factor + "\n");
            //}

            return allFactors;
        }

        private static long checkForPrimes(List<long> allFactors)
        {
            long largestPrime = 0;

            foreach (long factor in allFactors)
            {
                if (isPrime(factor) == true)
                {
                    largestPrime = factor;
                }
            }

            return largestPrime;
        }

        private static bool isPrime(long factor)
        {
            for (long i = 2; i < factor / 2; i++)
            {
                if (factor % i == 0)
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
