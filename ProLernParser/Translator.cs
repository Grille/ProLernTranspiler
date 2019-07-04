using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Security.Permissions;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProLernParser
{   
    public class Translator
    {

        public Translator()
        {
        }
        public string Parse(string code)
        {
            string body = "";
            ref string traget = ref body;
            string[] lines = code.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                bool castArray = true;
                ref string line = ref lines[i];
                string newLine = "";

                if  (parse(line, out newLine, "START", "public void START(){")) { castArray = false; }
                else if (parse(line, out newLine, "STOPP", "_S.Console.WriteLine(\"Bitte eine Taste druecken, um das Programm zu beenden.\");_S.Console.ReadKey();}")) ;
                else if (parse(line, out newLine, "ENDE", "}")) ;
                else if (parse(line, out newLine, "AUSGABE", "_S.Console.WriteLine(<#>);")) ;
                else if (parse(line, out newLine, "AUSGABEREIHE", "_S.Console.Write(<#>);")) ;
                else if (parse(line, out newLine, "RECHNEN", "<#>;")) ;
                else if (parse(line, out newLine, "ZAHL", "double <#>;")) ;
                else if (parse(line, out newLine, "WORT", "string <#>;")) ;
                else if (parse(line, out newLine, "ZAHLEINGABE", "<#>=double.Parse(_S.Console.ReadLine());")) ;
                else if (parse(line, out newLine, "WORTEINGABE", "<#>=_S.Console.ReadLine();")) ;
                else if (parse(line, out newLine, "EINGABE", "_S.Console.ReadKey();")) ;
                else if (parse(line, out newLine, "ZAHLFELD", "double[] <#>;", (input) => input.Replace("[", "=new double[(int)"))) { castArray = false; }
                else if (parse(line, out newLine, "WORTFELD", "string[] <#>;", (input) => input.Replace("[", "=new string[(int)"))) { castArray = false; }
                else if (parse(line, out newLine, "FALLS", "if(<#>){", (input) => Regex.Replace(input, "[^!><=]=", "==").Replace("UND", "&&").Replace("ODER", "||"))) ;
                else if (parse(line, out newLine, "SONST", "else{")) ;
                else if (parse(line, out newLine, "SOLANGE", "}while(<#>);", (input) => Regex.Replace(input, "[^!><=]=", "==").Replace("UND", "&&").Replace("ODER", "||"))) ;
                else if (parse(line, out newLine, "WIEDERHOLE", "do{")) ;
                else if (parse(line, out newLine, "UNTERPROGRAMM", "public void <#>{", (input) => input.Replace("ZAHLFELD", "double[]").Replace("WORTFELD", "string[]").Replace("ZAHL", "double").Replace("WORT", "string"))) { castArray = false; }
                else if (parse(line, out newLine, "FUNKTION", "public <#>{", (input) => input.Replace("ZAHLFELD", "double[]").Replace("WORTFELD", "string[]").Replace("ZAHL", "double").Replace("WORT", "string"))) { castArray = false; }
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

                else if (parse(line, out newLine, "FENSTER", "public class _proLernForm : _proLernFormProto{")) ;
                else if (parse(line, out newLine, "FENSTERKONSTRUKTOR", "public _proLernForm(){")) ;
                else if (parse(line, out newLine, "OEFFNEFENSTER", "_SWF.Application.Run(new _proLernForm());")) ;
                else if (parseFunction(line, out newLine, "FENSTERGROESSE", "base.Width=<arg0/int>;base.Height=<arg1/int>;")) ;
                else if (parseFunction(line, out newLine, "KNOPF", "public _button <#> = new _button(){Left = <arg0/int>,Top = <arg1/int>,Width = <arg2/int>,Height = <arg3/int>,Text=<arg4>,Font = new System.Drawing.Font(<arg5>, <arg6/float>),clickname=\"<arg7>\"};")) ;
                else if (parseFunction(line, out newLine, "WORTBOX", "public _SWF.TextBox <#> = new _SWF.TextBox(){Multiline = true,ScrollBars = _SWF.ScrollBars.Vertical,Left = <arg0/int>,Top = <arg1/int>,Width = <arg2/int>,Height = <arg3/int>,Font = new System.Drawing.Font(<arg4>, <arg5/float>)};")) ;
                else if (parse(line, out newLine, "KNOPFKLICK", "public void <#>(object _sender, _S.EventArgs _e){")) ;
                else if (parse(line, out newLine, "FENSTERLOESCHEN", "base.Refresh();")) ;
                else if (parse(line, out newLine, "FENSTERNEUZEICHNEN", "protected override void OnPaint(_SWF.PaintEventArgs _e){base.OnPaint(_e);this._g.Dispose(); var _g = _e.Graphics; this._g = this.CreateGraphics();")) ;
                else if (parseFunction(line, out newLine, "MAUSKLICK", "protected override void OnMouseClick(_SWF.MouseEventArgs _e){base.OnMouseClick(_e);double <arg0> = _e.X,<arg1> = _e.Y;")) ;
                else if (parseColor(line, out newLine, "FENSTERFARBE", "base.BackColor = _SD.Color.<color>;")) ;
                else if (parseColor(line, out newLine, "PINSEL", "_pinsel = new _SD.SolidBrush(_SD.Color.<color>);")) ;
                else if (parseFunction(line, out newLine, "STIFT", "_stift = new _SD.Pen(_SD.Color.<arg0/color>,<arg1/float>);")) ;

                else if (parseFunction(line, out newLine, "RECHTECKFUELLEN", "_g.FillRectangle(_pinsel, new _SD.Rectangle(<arg0/int>, <arg1/int>, <arg2/int>, <arg3/int>));")) ;
                else if (parseFunction(line, out newLine, "ELLIPSEFUELLEN", "_g.FillEllipse(_pinsel, new _SD.Rectangle(<arg0/int>, <arg1/int>, <arg2/int>, <arg3/int>));")) ;
                else if (parseFunction(line, out newLine, "RECHTECK", "_g.DrawRectangle(_stift, new _SD.Rectangle(<arg0/int>, <arg1/int>, <arg2/int>, <arg3/int>));")) ;
                else if (parseFunction(line, out newLine, "ELLIPSE", "_g.DrawEllipse(_stift, new _SD.Rectangle(<arg0/int>, <arg1/int>, <arg2/int>, <arg3/int>));")) ;
                else if (parseFunction(line, out newLine, "ZEICHNEWORT", "_g.DrawString(<arg0>,new _SD.Font(<arg1>,<arg2/float>),new _SD.SolidBrush(_SD.Color.<arg3/color>),new _SD.Point(<arg4/int>,<arg5/int>));")) ;
                else if (parseFunction(line, out newLine, "LINIE", "_g.DrawLine(_stift,new _SD.Point(<arg0/int>,<arg1/int>), new _SD.Point(<arg2/int>, <arg3/int>));")) ;
                else if (parseFunction(line, out newLine, "BILD", "_drawBitmap(_g,<arg0>, <arg1/int>, <arg2/int>, <arg3/int>, <arg4/int>);")) ;

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
                newLine = newLine.Replace("X-Position", "X_Position");
                newLine = newLine.Replace("Y-Position", "Y_Position");
                if (castArray) newLine = newLine.Replace("[", "[(int)");
                body += newLine;
            }
            TextBox h = new TextBox()
            {
                Left = 0,
                Top = 0,
                Width = 0,
                Height = 0,
                Font = new System.Drawing.Font("", 3)
            };
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ProLernParser.environment.cs";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd().Replace("///<include>code", body);
                //Console.WriteLine(result);
                return result;
            }
        }


        bool parse(string line, out string newLine, string command, string result)
        {
            return parse(line, out newLine, command, result, (string input) => input);
        }
        bool parse(string line, out string newLine, string command, string result, Func<string, string> func)
        {
            return parseFrame(line, out newLine, command, (string split) =>
            {
                string code = split.Replace('\n', ' ');
                return result.Replace("<#>", func(code)) + "\r\n";
            });
        }
        bool parseFunction(string line, out string newLine, string command, string result)
        {
            return parseFrame(line, out newLine, command, (string code) =>
            {
                string[] args = code.Split(',');
                args[0] = args[0].Trim();
                string name = null;
                if (args[0].Contains(" ") && args[0][0] != '"')
                {
                    string[] split = args[0].Split(' ');
                    name = split[0];
                    args[0] = split[1];
                }
                for (int i = 0; i < args.Length; i++)
                {
                    string arg = args[i].Trim();
                    result = result.Replace("<#>", name);
                    result = result.Replace("<arg" + i + ">", arg);
                    result = result.Replace("<arg" + i + "/byte>", "(byte)(" + arg + ")");
                    result = result.Replace("<arg" + i + "/int>", "(int)(" + arg + ")");
                    result = result.Replace("<arg" + i + "/float>", "(float)(" + arg + ")");
                    result = result.Replace("<arg" + i + "/color>", getColor(arg));
                }
                result = result.Replace('\n', ' ');
                return result + "\r\n";
            });
        }
        bool parseColor(string line, out string newLine, string command, string result)
        {
            return parseFrame(line, out newLine, command, (string code) =>
            {
                string plcolor = code.Trim();
                string cscolor = getColor(plcolor);
                result = result.Replace("<color>", cscolor);
                result = result.Replace('\n', ' ');
                return result + "\r\n";
            });
        }
        string getColor(string input)
        {
            switch (input)
            {
                case "ROT": return "Red";
                case "GELB": return "Yellow";
                case "GRUEN": return "Green";
                case "BLAU": return "Blue";
                case "LILA": return "Magenta";
                case "TUERKIS": return "Cyan";
                case "WEISS": return "White";
                case "SCHWARTZ": return "Black";
                default: return "Gray";
            }
        }
        bool parseFrame(string line, out string newLine, string command, Func<string, string> function)
        {
            if (line.Contains(command))
            {
                string[] split = line.Split(new string[] { command }, 2, StringSplitOptions.None);
                if (split[0].Trim() == "" && (split[1].Trim().Length == 0 || split[1][0] == ' ' || split[1][0] == '('))
                {
                    split[1] = split[1].Trim(new[] { ' ','\t'});
                    if (split[1].Length > 1 && split[1][0] == '(' && split[1][split[1].Length - 1] == ')')
                    {
                        split[1] = split[1].TrimStart(new[] { '(' });
                        split[1] = split[1].TrimEnd(new[] { ')' });
                    }
                    newLine = function(split[1]);
                    return true;
                }
            }
            newLine = null;
            return false;
        }
    }

}