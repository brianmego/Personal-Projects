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
            List<int> palindromes = checkAllThreeDigitCombos();

            int largestPalindrome = findLargestPalindrome(palindromes);
            Console.WriteLine(largestPalindrome);
        }

        private static int findLargestPalindrome(List<int> palindromes)
        {
            palindromes.Sort();
            palindromes.Reverse();
            return palindromes[0];
        }

        private static List<int> checkAllThreeDigitCombos()
        {
            List<int> listOfPalindromes = new List<int>();
            for (int i = 999; i >= 100; i--)
            {
                for (int j = 999; j >= 100; j--)
                {
                    int product = (i*j);
                    if (IsPalindrome(product.ToString()) == true)
                    {
                        listOfPalindromes.Add(product);
                    }
                }
            }
            return listOfPalindromes;
        }

        private static bool IsPalindrome(string numberToTest)
        {
            if (numberToTest.ToString() == reverseString(numberToTest.ToString()))
            {
                return true;
            }
            return false;
        }

        static string reverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
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
