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

namespace ProLernParser
{
    static class Program
    {
        public class ProLernForm : Form
        {
            public ProLernForm() {
            }
        }
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>     
        static public Action kill;
        static public Task task;
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CodeForm());
            var __cssw = new StreamWriter("path");
        }
        static public string Parse(string code)
        {
            string head = "using System;using System.IO;\r\nusing System.Windows.Forms;\r\n";
            head += "namespace ProLernProgram{\r\nstatic public class Program{\r\n";
            head += "static public StreamWriter __cs_sw;\r\n";
            head += "static public double ZUFALLSZAHL(double max){var rnd = new System.Random();return rnd.Next((int)1,(int)max);}\r\n";
            head += "static public double WORTINZAHL(string str){return double.Parse(str);}\r\n";
            string end = "}}";
            string body = "";
            string consoleColor = "Console.ForegroundColor = ConsoleColor.";
            string[] lines = code.Split('\n');
            for (int i = 0; i < lines.Length;i++)
            {
                bool castArray = true;
                ref string line = ref lines[i];
                string newLine;
                if /**/ (parse(line, out newLine, "START", "[STAThread]\r\nstatic public void Main(){")) { castArray = false; }
                else if (parse(line, out newLine, "STOPP", "}")) ;
                else if (parse(line, out newLine, "ENDE", "}")) ;
                else if (parse(line, out newLine, "AUSGABE", "Console.WriteLine(<#>);")) ;
                else if (parse(line, out newLine, "RECHNEN", "<#>;")) ;
                else if (parse(line, out newLine, "ZAHL", "double <#>;")) ;
                else if (parse(line, out newLine, "WORT", "string <#>;")) ;
                else if (parse(line, out newLine, "ZAHLEINGABE", "<#>=double.Parse(Console.ReadLine());")) ;
                else if (parse(line, out newLine, "WORTEINGABE", "<#>=Console.ReadLine();")) ;
                else if (parse(line, out newLine, "ZAHLFELD", "var <#>;", (string input) => { return input.Replace("[", "=new double["); })) ;
                else if (parse(line, out newLine, "WORTFELD", "var <#>;", (string input) => { return input.Replace("[", "=new string["); })) ;
                else if (parse(line, out newLine, "FALLS", "if(<#>){", (string input) => { return input.Replace("=", "==").Replace("UND", "&").Replace("ODER", "|"); })) ;
                else if (parse(line, out newLine, "SONST", "else{")) ;
                else if (parse(line, out newLine, "SOLANGE", "}while(<#>);", (string input) => { return input.Replace("=", "==").Replace("UND", "&").Replace("ODER", "|"); })) ;
                else if (parse(line, out newLine, "WIEDERHOLE", "do{")) ;
                else if (parse(line, out newLine, "UNTERPROGRAMM", "static public void <#>{", (string input) => { return input.Replace("ZAHLFELD", "double[]").Replace("WORTFELD", "string[]").Replace("ZAHL", "double").Replace("WORT", "string"); })) { castArray = false; }
                else if (parse(line, out newLine, "FUNKTION", "static public <#>{", (string input) => { return input.Replace("ZAHLFELD", "double[]").Replace("WORTFELD", "string[]").Replace("ZAHL", "double").Replace("WORT", "string"); })) { castArray = false; }
                else if (parse(line, out newLine, "RUECKGABE", "return <#>;")) ;
                else if (parse(line, out newLine, "FARBE ROT", consoleColor + "Red;")) ;
                else if (parse(line, out newLine, "FARBE GELB", consoleColor + "Yellow;")) ;
                else if (parse(line, out newLine, "FARBE GRUEN", consoleColor + "Green;")) ;
                else if (parse(line, out newLine, "FARBE WEISS", consoleColor + "White;")) ;
                else if (parse(line, out newLine, "FARBE NORMAL", consoleColor + "Gray;")) ;
                else if (parse(line, out newLine, "BEMERKUNG:", "//<#>")) ;
                else if (parse(line, out newLine, "BILDSCHIRMLOESCHEN", "Console.Clear();")) ;
                else if (parse(line, out newLine, "FENSTER", "public class ProLernForm : Form{")) ;
                else if (parse(line, out newLine, "FENSTERKONSTRUKTOR", "public ProLernForm(){")) ;
                else if (parse(line, out newLine, "OEFFNEFENSTER", "Application.Run(new ProLernForm());")) ;
                else
                {
                    if (line.Trim() == "" || line.Contains(";") || line.Contains("{") || line.Contains("}"))
                    {
                        newLine = line + "\r\n";
                        castArray = false;
                    }
                    else newLine = line + ";\r\n";
                }
                newLine = newLine.Replace("~LAENGE", ".Length");
                if (castArray)newLine = newLine.Replace("[", "[(int)");
                body += newLine;
            }
            return head + body + end;
        }
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        static public void Evaluate(string cscode)
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Gray;

                var codeProvider = new CSharpCodeProvider();
                var compiler = codeProvider.CreateCompiler();
                var parameter = new CompilerParameters();

                parameter.ReferencedAssemblies.Add("microsoft.csharp.dll");
                parameter.ReferencedAssemblies.Add("system.dll");
                parameter.ReferencedAssemblies.Add("system.core.dll");
                parameter.ReferencedAssemblies.Add("system.data.dll");
                parameter.ReferencedAssemblies.Add("system.deployment.dll");
                parameter.ReferencedAssemblies.Add("system.drawing.dll");
                parameter.ReferencedAssemblies.Add("system.windows.forms.dll");
                parameter.CompilerOptions = "/t:library";
                parameter.GenerateInMemory = true;

                var result = compiler.CompileAssemblyFromSource(parameter, cscode);
                if (result.Errors.Count > 0)
                {
                    Console.Error.WriteLine("LINE: " + result.Errors[0].Line+ " ERROR: " + result.Errors[0].ErrorText);
                }
                Assembly assembly = result.CompiledAssembly;
                //object program = assembly.CreateInstance("ProLernParser.Program");

                Type type = assembly.GetType("ProLernProgram.Program"); 
                Action main = (Action)Delegate.CreateDelegate(typeof(Action), type.GetMethod("Main"));
                task = new Task(() =>
                {
                    main();
                    Console.WriteLine("press key to continue...");
                    Console.ReadKey();

                });
                task.Start();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
        static void f(int[] gg)
        {

        }
        static bool parse(string line,out string newLine,string command, string result)
        {
            return parse(line, out newLine, command, result, (string input) => input);
        }
        static bool parse(string line, out string newLine, string command, string result, Func<string, string> func)
        {
            if (line.Contains(command))
            {
                string[] split = line.Split(new string[] { command }, StringSplitOptions.None);
                if (split[0].Trim() == "" && (split[1].Trim().Length == 0 || split[1][0] == ' '))
                {
                    string code = split[1].Replace('\n', ' ');
                    newLine = result.Replace("<#>", func(code)) + "\r\n";
                    return true;
                }
            }
            newLine = null;
            return false;
        }
    }
}
