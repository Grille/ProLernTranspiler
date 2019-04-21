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

namespace ProLernParser
{
    static class Program
    {
        public class ProLernForm : Form
        {
            public ProLernForm()
            {
            }
        }
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>     
        static public Process process;
        static public Task task;
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CodeForm());
            var __cssw = new StreamWriter("path");
            __cssw.WriteLine();
            using (var _writer = new StreamWriter("tset.txt")){
                _writer.WriteLine(585);
                _writer.Close();
            }
        }


        static public string Parse(string code)
        {
            string head = "using _S=System;using _SI=System.IO;\r\nusing _SWF=System.Windows.Forms;using _SD=System.Drawing;\r\n";
            head += "namespace ProLernProgram{\r\nstatic public class Program{\r\n";
            head += "static public _SI.StreamWriter _writer;\r\n";
            head += "static public _SI.StringReader _reader;\r\n";
            head += "static public double ZUFALLSZAHL(double max){var rnd = new _S.Random();return rnd.Next((int)1,(int)max);}\r\n";
            head += "static public double WORTINZAHL(string str){return double.Parse(str);}\r\n";
            string end = "}}";
            string body = "";
            ref string traget = ref body;
            string consoleColor = "Console.ForegroundColor = ConsoleColor.";
            string[] lines = code.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                bool castArray = true;
                ref string line = ref lines[i];
                string newLine;
                if /**/ (parse(line, out newLine, "START", "static public void Main(){\r\ntry{")) { castArray = false; }
                else if (parse(line, out newLine, "STOPP", "}catch{_S.Console.WriteLine(\"Fehler.\");_S.Console.ReadKey();}_S.Console.WriteLine(\"Bitte eine Taste druecken, um das Programm zu beenden.\");_S.Console.ReadKey();}")) ;
                else if (parse(line, out newLine, "ENDE", "}")) ;
                else if (parse(line, out newLine, "AUSGABE", "_S.Console.WriteLine(<#>);")) ;
                else if (parse(line, out newLine, "RECHNEN", "<#>;")) ;
                else if (parse(line, out newLine, "ZAHL", "double <#>;")) ;
                else if (parse(line, out newLine, "WORT", "string <#>;")) ;
                else if (parse(line, out newLine, "ZAHLEINGABE", "<#>=double.Parse(_S.Console.ReadLine());")) ;
                else if (parse(line, out newLine, "WORTEINGABE", "<#>=_S.Console.ReadLine();")) ;
                else if (parse(line, out newLine, "ZAHLFELD", "var <#>;", (input) => input.Replace("[", "=new double["))) ;
                else if (parse(line, out newLine, "WORTFELD", "var <#>;", (input) => input.Replace("[", "=new string["))) ;
                else if (parse(line, out newLine, "FALLS", "if(<#>){", (input) => input.Replace("=", "==").Replace("UND", "&").Replace("ODER", "|"))) ;
                else if (parse(line, out newLine, "SONST", "else{")) ;
                else if (parse(line, out newLine, "SOLANGE", "}while(<#>);", (input) => input.Replace("=", "==").Replace("UND", "&").Replace("ODER", "|"))) ;
                else if (parse(line, out newLine, "WIEDERHOLE", "do{")) ;
                else if (parse(line, out newLine, "UNTERPROGRAMM", "static public void <#>{", (input) => input.Replace("ZAHLFELD", "double[]").Replace("WORTFELD", "string[]").Replace("ZAHL", "double").Replace("WORT", "string"))) { castArray = false; }
                else if (parse(line, out newLine, "FUNKTION", "static public <#>{", (input) => input.Replace("ZAHLFELD", "double[]").Replace("WORTFELD", "string[]").Replace("ZAHL", "double").Replace("WORT", "string"))) { castArray = false; }
                else if (parse(line, out newLine, "RUECKGABE", "return <#>;")) ;
                else if (parseColor(line, out newLine, "FARBE", "_S.Console.ForegroundColor = _S.ConsoleColor.<color>;")) ;
                else if (parse(line, out newLine, "BEMERKUNG:", "//<#>")) ;
                else if (parse(line, out newLine, "BILDSCHIRMLOESCHEN", "_S.Console.Clear();")) ;
                else if (parse(line, out newLine, "SCHREIBEN-OEFFNEN", "_writer = new _SI.StreamWriter(<#>);")) ;
                else if (parse(line, out newLine, "LESEN-OEFFNEN", "_reader = new _SI.StreamReader(<#>);")) ;
                else if (parse(line, out newLine, "ZAHL-LESEN", "<#>=double.Parse(_reader.ReadLine());")) ;
                else if (parse(line, out newLine, "WORT-LESEN", "<#>=_reader.ReadLine();")) ;
                else if (parse(line, out newLine, "WORT-SCHREIBEN", "_writer.WriteLine(<#>);")) ;
                else if (parse(line, out newLine, "ZAHL-SCHREIBEN", "_writer.WriteLine(<#>);")) ;
                else if (parse(line, out newLine, "SCHREIBEN-SCHLIESSEN", "_writer.Close();")) ;
                else if (parse(line, out newLine, "LESEN-SCHLIESSEN", "_reader.Close();")) ;
                else if (parse(line, out newLine, "FENSTER", "public class ProLernForm : _SWF.Form{")) ;
                else if (parse(line, out newLine, "FENSTERKONSTRUKTOR", "public ProLernForm(){")) ;
                else if (parse(line, out newLine, "OEFFNEFENSTER", "_SWF.Application.Run(new ProLernForm());")) ;
                else if (parseFunction(line, out newLine, "FENSTERGROESSE", "base.Width=(int)<arg0>;base.Height=(int)<arg1>;")) ;
                else if (parseFunction(line, out newLine, "KNOPF", "_SWF.TextBox <#> = new _SWF.TextBox(){Left = (int)<arg0>,Top = (int)<arg1>,Width = (int)<arg2>,Height = (int)<arg3>,Text=<arg4>,Font = new System.Drawing.Font(<arg5>, (float)<arg6>),Click+=<arg7>};")) ;
                else if (parseFunction(line, out newLine, "WORTBOX", "_SWF.TextBox <#> = new _SWF.TextBox(){Left = (int)<arg0>,Top = (int)<arg1>,Width = (int)<arg2>,Height = (int)<arg3>,Font = new System.Drawing.Font(<arg4>, (float)<arg5>)};")) ;
                else if (parse(line, out newLine, "KNOPFKLICK ", "public void <#>(object _sender, EventArgs _e){")) ;
                else if (parse(line, out newLine, "FENSTERLOESCHEN", "base.Refresh();")) ;
                else if (parse(line, out newLine, "FENSTERNEUZEICHNEN", "protected override void OnPaint(_SWF.PaintEventArgs _e){base.OnPaint(_e);")) ;
                else if (parseColor(line, out newLine, "FENSTERFARBE", "base.BackColor = _SD.Color.<color>;")) ;
                else if (parse(line, out newLine, "VERSUCH", "try{")) ;
                else if (parse(line, out newLine, "FEHLER", "catch{")) ;
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
                newLine = newLine.Replace("~INHALT", ".Text");
                if (castArray) newLine = newLine.Replace("[", "[(int)");
                body += newLine;
            }
            TextBox h = new TextBox()
            {
                Left = 0,Top = 0,Width = 0,Height = 0,
                Font = new System.Drawing.Font("", 3)
            };

            return head + body + end;


        }
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        static public void Evaluate(string cscode)
        {
            Kill();
            using (var codeProvider = new CSharpCodeProvider())
            {
                var compiler = codeProvider.CreateCompiler();
                var parameter = new CompilerParameters();
                parameter.ReferencedAssemblies.Add("microsoft.csharp.dll");
                parameter.ReferencedAssemblies.Add("system.dll");
                parameter.ReferencedAssemblies.Add("system.core.dll");
                parameter.ReferencedAssemblies.Add("system.data.dll");
                parameter.ReferencedAssemblies.Add("system.deployment.dll");
                parameter.ReferencedAssemblies.Add("system.drawing.dll");
                parameter.ReferencedAssemblies.Add("system.windows.forms.dll");
                parameter.CompilerOptions = "/t:exe";
                parameter.GenerateExecutable = true;
                parameter.OutputAssembly = "start.exe";
                var result = compiler.CompileAssemblyFromSource(parameter, cscode);
                if (result.Errors.Count > 0)
                {
                    MessageBox.Show("LINE: " + result.Errors[0].Line + "\nID: " + result.Errors[0].ErrorNumber + "\nERROR: " + result.Errors[0].ErrorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.Error.WriteLine("LINE: " + result.Errors[0].Line + " ERROR: " + result.Errors[0].ErrorText);
                }
                else
                {
                    process = Process.Start("start.exe");
                }
            }
        }
        static public void Kill()
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
                process.Refresh();
                process.WaitForExit();
            }
        }
        static void f(int[] gg)
        {

        }
        static bool parse(string line, out string newLine, string command, string result)
        {
            return parse(line, out newLine, command, result, (string input) => input);
        }
        static bool parse(string line, out string newLine, string command, string result, Func<string, string> func)
        {
            return parseFrame(line, out newLine, command, (string split) =>
            {
                string code = split.Replace('\n', ' ');
                return result.Replace("<#>", func(code)) + "\r\n";
            });
        }
        static bool parseFunction(string line, out string newLine, string command, string result)
        {
            return parseFrame(line, out newLine, command, (string code) =>
            {
                string[] args = code.Split(',');
                args[0] = args[0].Trim();
                string name = null;
                if (args[0].Contains(" "))
                {
                    string[] split = args[0].Split(' ');
                    name = split[0];
                    args[0] = split[1];
                }
                for (int i = 0; i < args.Length; i++)
                {
                    result = result.Replace("<#>", name);
                    result = result.Replace("<arg" + i + ">", args[i].Trim());
                }
                result = result.Replace('\n', ' ');
                return result + "\r\n";
            });
        }
        static bool parseColor(string line, out string newLine, string command, string result)
        {
            return parseFrame(line, out newLine, command, (string code) =>
            {
                string plcolor = code.Trim();
                string cscolor = plcolor;
                switch (plcolor)
                {
                    case "ROT": cscolor = "Red"; break;
                    case "GELB": cscolor = "Yellow"; break;
                    case "GRUEN": cscolor = "Green"; break;
                    case "BLAU": cscolor = "Blue"; break;
                    case "LILA": cscolor = "Magenta"; break;
                    case "TUERKIS": cscolor = "Cyan"; break;
                    case "WEISS": cscolor = "White"; break;
                    case "SCHWARTZ": cscolor = "Black"; break;
                    case "NORMAL": case "GRAU": cscolor = "Gray"; break;
                }
                result = result.Replace("<color>", cscolor);
                result = result.Replace('\n', ' ');
                return result + "\r\n";
            });
        }
        static bool parseFrame(string line, out string newLine, string command,Func<string,string>function)
        {
            if (line.Contains(command))
            {
                string[] split = line.Split(new string[] { command },2, StringSplitOptions.None);
                if (split[0].Trim() == "" && (split[1].Trim().Length == 0 || split[1][0] == ' '))
                {
                    newLine = function(split[1]);
                    return true;
                }
            }
            newLine = null;
            return false;
        }
        
    }
}
