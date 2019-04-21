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
        Button Button = new Button() { };
        public CodeForm()
        {
            InitializeComponent();
            codeBox.MaxLength = 100000;
            textBox1.MaxLength = 100000;
            textBox1.Text = Program.Parse(codeBox.Text);
            //Program.Evaluate(textBox1.Text);

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
            /*
            var box = (TextBox)sender;
            string text = box.Text;
            Console.WriteLine(e.KeyData);
            if (e.KeyData.HasFlag(Keys.Tab))
            {
                e.IsInputKey = false;
                int pos = box.SelectionStart;
                //if (box.SelectionLength==0)
                if (e.Shift)
                {
                    box.Text = box.Text.Remove(pos,2);
                    box.SelectionStart = pos - 2;
                    Console.WriteLine(pos);
                }
                else
                {
                    Console.WriteLine(pos);
                    box.Text = box.Text.Insert(pos, "  ");
                    box.SelectionStart = pos + 2;
                }
            }
            */
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CodeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.Kill();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
