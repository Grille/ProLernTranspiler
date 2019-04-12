using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProLernParser
{
    public partial class CodeForm : System.Windows.Forms.Form
    {
        public CodeForm()
        {
            InitializeComponent();
            codeBox.MaxLength = 100000;
            textBox1.MaxLength = 100000;
            textBox1.Text = Program.Parse(codeBox.Text);
            Program.Evaluate(textBox1.Text);

            this.Paint += new PaintEventHandler(paint);
        }

        private void paint(object sender,PaintEventArgs e)
        {

        }

        private void codeBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                textBox1.Text = "...";
                textBox1.Text = Program.Parse(codeBox.Text);
                Program.Evaluate(textBox1.Text);
            }
            if (e.KeyData == Keys.F6)
            {
                textBox1.Text = "...";
                textBox1.Text = Program.Parse(codeBox.Text);
            }
        }

        private void codeBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                e.IsInputKey = false;
                int pos = codeBox.SelectionStart;
                codeBox.Text += "  ";
                codeBox.SelectionStart = pos+2;
            }
        }
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
