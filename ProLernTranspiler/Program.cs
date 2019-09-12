using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Security.Permissions;
using System.Diagnostics;

namespace ProLernTranspiler
{
    class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>     
        [STAThread]
        public static void Main()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            var performer = new Performer();
            var translator = new Translator();
            Console.WriteLine("ProLern# Parser\n");
            string path = AppDomain.CurrentDomain.BaseDirectory;
            bool running = true;
            while (running)
            {
                Console.Write(path+">");
                string input = Console.ReadLine();
                string[] splitBin = input.Split(new[] { " " },2, StringSplitOptions.RemoveEmptyEntries);
                string[] split = input.Split(new[]{" "},StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    switch (splitBin[0])
                    {
                        case "exit": running = false; break;
                        case "cd":
                            {
                                string newPath = "";
                                if (Path.IsPathRooted(splitBin[1]))
                                    newPath = Path.GetFullPath(Path.Combine(path, splitBin[1]));
                                else
                                    newPath = Path.GetFullPath(splitBin[1]);
                                if (Directory.Exists(newPath))
                                {
                                    path = newPath;
                                    Directory.SetCurrentDirectory(path);
                                }
                                else
                                    Console.WriteLine("directory \"" + newPath + "\" not found");
                            }
                            break;
                        case "start":
                            {
                                string dst = split.Length > 2 ? split[2] : "start.exe";
                                performer.Compile(translator.Parse(File.ReadAllText(split[1])), split[2]);
                                performer.Start(dst);
                            }
                            break;
                        case "build":
                            {
                                string dst = split.Length > 2 ? split[2] : "start.exe";
                                performer.Compile(translator.Parse(File.ReadAllText(split[1])), split[2]);
                            }
                            break;
                        default: Console.WriteLine("unknown command \"" + split[0] + "\""); break;
                    }
                }
                catch
                {
                    Console.WriteLine("incorrect input");
                }
            }
        }
    }
}
