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
    public class Error
    {
        public string Name;
        public int Line;
        public string Title;
        public string Message;
        public bool Valid = true;
        public Error()
        {

        }
        public Error(CompilerError error)
        {
            Line = error.Line-129;
            if (Line < 1) Valid = false;
             Title = "ERROR: " + error.ErrorNumber;
            Message = error.ErrorText;
        }
        public static List<Error> GetErrors(CompilerErrorCollection cErrors)
        {
            var list = new List<Error>();

            foreach (CompilerError cError in cErrors)
            {
                var error = new Error(cError);
                if (error.Valid)
                    list.Add(error);
            }

            return list;
        }
    }
}
