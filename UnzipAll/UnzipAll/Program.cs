using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnzipAll
{
    class Program
    {
        static string dirpath = @"C:\Users\Brian\Repos\myeduScripts\scripts\local\schedule_importer\docs";

        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(dirpath);
            foreach (FileInfo fi in di.GetFiles("*.zip"))
            {
                Decompress(fi, dirpath);
            }
        }

        public static bool Shell(string exefile, string args, bool async)
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = exefile;
                proc.StartInfo.Arguments = args;
                proc.Start();

                if (async == false)
                {
                    proc.WaitForExit();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Decompress(FileInfo fi, string dirpath)
        {
            Shell("cmd", " /B /c unzip \"" + fi.FullName + "\" -d " + dirpath + @"\unzipped\", true);
        }
    }
}
