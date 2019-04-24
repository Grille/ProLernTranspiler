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
    public class Performer
    {
        public CompilerErrorCollection Errors;
        public Process process;
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public bool Compile(string cscode, string dst)
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
                parameter.CompilerOptions = "/t:exe /unsafe";
                parameter.GenerateExecutable = true;
                parameter.OutputAssembly = dst;
                var result = compiler.CompileAssemblyFromSource(parameter, cscode);
                Errors = result.Errors;
                if (result.Errors.Count > 0)
                {
                    //MessageBox.Show("LINE: " + result.Errors[0].Line + "\nID: " + result.Errors[0].ErrorNumber + "\nERROR: " + result.Errors[0].ErrorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Console.Error.WriteLine("LINE: " + result.Errors[0].Line + " ERROR: " + result.Errors[0].ErrorText);
                    return false;
                }
                return true;
            }
        }
        public void Kill()
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
                process.Refresh();
                process.WaitForExit();
            }
        }
        public void Start(string path)
        {
            process = Process.Start(path);
        }
    }
}
