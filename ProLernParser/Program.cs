using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace ProLernParser
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>     
        static public TextBox TextBox;
        static string line;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form());
        }
        static public string Parse(string code)
        {
            string head = "namespace ProLernParser{\r\nstatic public class Program{\r\n";
            head += "static public double ZUFALLSZAHL(double max){var rnd = new System.Random();return rnd.Next((int)1,(int)max);}\r\n";
            head += "static public double WORTINZAHL(string str){return double.Parse(str);}\r\n";
            string end = "}}";
            string body = "";
            string consoleColor = "System.Console.ForegroundColor = System.ConsoleColor.";
            string[] lines = code.Split('\n');
            for (int i = 0; i < lines.Length;i++)
            {
                ref string line = ref lines[i];
                string newLine;
                if /**/ (parse(line, out newLine, "AUSGABE", "System.Console.WriteLine(<#>);")) ;
                else if (parse(line, out newLine, "START", "static public void Main(){")) ;
                else if (parse(line, out newLine, "STOPP", "}")) ;
                else if (parse(line, out newLine, "ENDE", "}")) ;
                else if (parse(line, out newLine, "RECHNEN", "<#>;")) ;
                else if (parse(line, out newLine, "UNTERPROGRAMM", "static public void <#>{", (string input) => { return input.Replace("ZAHLFELD", "double[]").Replace("WORTFELD", "string[]").Replace("ZAHL", "double").Replace("WORT", "string"); })) ;
                else if (parse(line, out newLine, "FUNKTION", "static public <#>{", (string input) => { return input.Replace("ZAHLFELD", "double[]").Replace("WORTFELD", "string[]").Replace("ZAHL", "double").Replace("WORT", "string"); })) ;
                else if (parse(line, out newLine, "RUECKGABE", "return <#>;")) ;
                else if (parse(line, out newLine, "ZAHLEINGABE", "<#>=double.Parse(System.Console.ReadLine());")) ;
                else if (parse(line, out newLine, "WORTEINGABE", "<#>=System.Console.ReadLine();")) ;
                else if (parse(line, out newLine, "ZAHLFELD", "double[] <#>;",(string input)=> { return input.Replace("[", "=new double["); })) ;
                else if (parse(line, out newLine, "WORTFELD", "string[] <#>;", (string input) => { return input.Replace("[", "=new string["); })) ;
                else if (parse(line, out newLine, "ZAHL", "double <#>;")) ;
                else if (parse(line, out newLine, "WORT", "string <#>;")) ;
                else if (parse(line, out newLine, "FALLS", "if(<#>){", (string input) => { return input.Replace("=", "==").Replace("UND", "&").Replace("ODER", "|"); })) ;
                else if (parse(line, out newLine, "SONST", "else{")) ;
                else if (parse(line, out newLine, "SOLANGE", "}while(<#>);", (string input) => { return input.Replace("=", "==").Replace("UND", "&").Replace("ODER", "|"); })) ;
                else if (parse(line, out newLine, "WIEDERHOLE", "do{")) ;
                else if (parse(line, out newLine, "FARBE ROT", consoleColor+"Red;")) ;
                else if (parse(line, out newLine, "FARBE GELB", consoleColor + "Yellow;")) ;
                else if (parse(line, out newLine, "FARBE GRUEN", consoleColor + "Green;")) ;
                else if (parse(line, out newLine, "FARBE WEISS", consoleColor + "White;")) ;
                else if (parse(line, out newLine, "FARBE NORMAL", consoleColor + "Gray;")) ;
                else if (parse(line, out newLine, "BEMERKUNG:", "//<#>")) ;
                else if (parse(line, out newLine, "BILDSCHIRMLOESCHEN:", "System.Console.Clear();")) ;
                else newLine = line + "\r\n";
                newLine = newLine.Replace("~LAENGE", ".Length");
                newLine = newLine.Replace("[", "[(int)");
                body += newLine;
            }
            return head + body + end;
        }
        static public void Evaluate(string cscode)
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Gray;

                var codeProvider = new CSharpCodeProvider();
                var compiler = codeProvider.CreateCompiler();
                var parameter = new CompilerParameters();

                parameter.ReferencedAssemblies.Add("system.dll");

                parameter.CompilerOptions = "/t:library";
                parameter.GenerateInMemory = true;

                var result = compiler.CompileAssemblyFromSource(parameter, cscode);
                if (result.Errors.Count > 0)
                {
                    Console.Error.WriteLine("LINE: " + result.Errors[0].Line+ " ERROR: " + result.Errors[0].ErrorText);
                }
                Assembly assembly = result.CompiledAssembly;
                //object program = assembly.CreateInstance("ProLernParser.Program");

                Type type = assembly.GetType("ProLernParser.Program");
                MethodInfo main = type.GetMethod("Main");
                main.Invoke(null, null);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
        static bool parse(string line,out string newLine,string command, string result)
        {
            if (line.Contains(command))
            {
                newLine = result.Replace("<#>", line.Split(new string[] { command }, StringSplitOptions.None)[1]).Replace('\n',' ') + "\r\n";
                return true;
            }
            else
            {
                newLine = null;
                return false;
            }
        }
        static bool parse(string line, out string newLine, string command, string result, Func<string, string> func)
        {
            if (line.Contains(command))
            {
                string code = line.Split(new string[] { command }, StringSplitOptions.None)[1].Replace('\n', ' ');
                newLine = result.Replace("<#>", func(code)) + "\r\n";
                return true;
            }
            else
            {
                newLine = null;
                return false;
            }
        }
    }
}
