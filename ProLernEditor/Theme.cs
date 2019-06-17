using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ProLernEditor
{
    public class Theme
    {
        public enum Style
        {
            Background, Text, Flow, Type, Action, Calc, Comment, String, Symbol, Number
        }
        public Font Font;
        public Color Background;
        public Color Text,Flow,Type,Action,Calc, Comment, String, Symbol,Number;
        public Theme()
        {
            Font = new Font("consolas", 9.75f);
            Background = Color.White;
            Text = Number = Symbol = Color.Black;
            Flow = Type = Action = Calc = Color.Blue;
            Comment = Color.Green;
            String = Color.DarkRed;
        }
    }
}
