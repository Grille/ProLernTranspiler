using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using ScintillaNET;


namespace ProLernEditor
{
    [System.ComponentModel.DesignerCategory("code")]
    class CodeBox : Scintilla
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == (Keys.Shift|Keys.Enter) || keyData == (Keys.Control | Keys.Enter))
            {
                //SelectionStart
                string text = "\r\n";
                int begin = GetLineStart(SelectionStart-1);
                int end = SelectionStart;
                for (int i = begin; i < end; i++)
                {
                    if (Text[i] == ' ') text += ' ';
                    else if (Text[i] == '\t') text += '\t';
                    else if (Text[i] != '\n' && Text[i] != '\r') break;
                }
                AddText(text);
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public int GetLineStart(int pos)
        {
            while (pos > 0)
            {
                if (this.Text[pos] == '\n' || this.Text[pos] == '\r')
                    return pos;
                pos--;
            }
            return 0;
        }
        public int GetLineEnd(int pos)
        {
            while (pos < this.Text.Length)
            {
                if (this.Text[pos] == '\n' || this.Text[pos] == '\r')
                    return pos;
                pos++;
            }
            return this.Text.Length;
        }
    }

}
