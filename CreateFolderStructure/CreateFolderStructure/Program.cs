using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CreateSchoolsFolder
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] liveSchools = File.ReadAllLines("Live Schools.txt");
            foreach (string school in liveSchools)
            {
                string[] cells = school.Split('\t');
                cells[1] = cells[1].Replace('/',' ');
                string dirToCreate = @"C:\Users\Brian\Documents\Current Projects\Schools\" + cells[2] + @"\" + cells[1] + " [" + cells[0] + "]";
                Directory.CreateDirectory(dirToCreate);
                //Directory.CreateDirectory(dirToCreate + @"\" + "Grades");
                Console.WriteLine("Creating " + dirToCreate);
            }
        }
    }
}
