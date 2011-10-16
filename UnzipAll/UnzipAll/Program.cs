using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace UnzipAll
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                var dirpath = args[0];
                //var dirpath = @"C:\Users\Brian\Documents\Current Projects\ScheduleData";
                DirectoryInfo di = new DirectoryInfo(dirpath);
                foreach (FileInfo fi in di.GetFiles("*.zip"))
                {
                    Decompress(fi, dirpath);
                }
            }
            else
            {
                Console.WriteLine("Need a filepath full of zip files");
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
            var proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.FileName = "unzip";
            proc.StartInfo.Arguments = "-o " + "\"" + fi.FullName + "\"" + " -d " + "\"" + fi.DirectoryName + "\\unzipped" + "\"";
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            //Process.Start(new ProcessStartInfo("unzip", fi.FullName + " -d unzipped"));

            //Shell("cmd", " /B /c unzip \"" + fi.FullName + "\" -d " + dirpath + @"\unzipped\", true);
        }
    }
}
