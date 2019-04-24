using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProLernParser
{
    public partial class CodeForm : System.Windows.Forms.Form
    {
        Translator translator;
        Performer performer;
        Button Button = new Button() { };
        bool processHighlight = true;
        string codePath = "";
        public CodeForm()
        {
            InitializeComponent();
            codeBox.MaxLength = 100000;
            codeBoxCs.MaxLength = 100000;
            performer = new Performer();
            translator = new Translator();
            codeBox.Text = "START \n  AUSGABE \"Hallo Welt\"\nSTOPP ";
            highlight(0, codeBox.Text.Length);
            codeBoxCs.Text = translator.Parse(codeBox.Text);
            //Program.Evaluate(textBox1.Text);
            translator = new Translator();
            this.Paint += new PaintEventHandler(paint);
            splitContainer1.Panel2Collapsed = true;
        }

        private void paint(object sender,PaintEventArgs e)
        {

        }
        private void codeBox_KeyDown(object sender, KeyEventArgs e)
        {


        }
        private void codeBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!processHighlight && (e.KeyData == Keys.Space || e.KeyData == Keys.Tab || e.KeyData == Keys.Return || e.KeyData == Keys.Decimal))
                highlight(getPriorLineBreak(codeBox.SelectionStart - 1), getNextLineBreak(codeBox.SelectionStart + codeBox.SelectionLength));
        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void codeBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.Z))

            {

                Console.WriteLine("UNDO: " + this.codeBox.UndoActionName);
                while (this.codeBox.CanUndo && this.codeBox.UndoActionName.Equals("Unknown"))

                {

                    this.codeBox.Undo();

                }

            }

            /*
            var box = (RichTextBox)sender;
            string text = box.Text;
            Console.WriteLine(e.KeyData);
            if (e.KeyData.HasFlag(Keys.Tab))
            {
                e.IsInputKey = false;
                int pos = box.SelectionStart;
                if (box.SelectedText.Contains("\n"))
                {

                }
                else
                {
                    if (e.Shift)
                    {
                        box.Text = box.Text.Remove(pos, 2);
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
            }
            */
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CodeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            performer.Kill();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBoxCode_TextChanged(object sender, EventArgs e)
        {
            /*
            if (!processHighlight && codeBox.UndoActionName != "Unknown" && codeBox.UndoActionName != "")
            {
                Console.WriteLine("<"+codeBox.UndoActionName+">");
                highlight(getPriorLineBreak(codeBox.SelectionStart - 1), getNextLineBreak(codeBox.SelectionStart + codeBox.SelectionLength));

            }
            */
        }
        private void highlight(int start, int end)
        {
            processHighlight = true;
            codeBox.SuspendLayout();
            codeBox.Enabled = false;
            int backupS = codeBox.SelectionStart;
            int backupL = codeBox.SelectionLength;
            codeBox.Select(start, end - start);
            codeBox.SelectionColor = Color.LightGray;
            codeBox.SelectionFont = new Font("consolas", 10);

            Color command = Color.SkyBlue, variable = Color.Violet, other = Color.FromArgb(255, 255, 128), comment = Color.LightGreen;
            highlight("START", command, start, end);
            highlight("STOPP", command, start, end);
            highlight("AUSGABE", other, start, end);
            highlight("ENDE", command, start, end);
            highlight("RECHNEN", other, start, end);
            highlight("ZAHL", variable, start, end);
            highlight("WORT", variable, start, end);
            highlight("ZAHLEINGABE", other, start, end);
            highlight("WORTEINGABE", other, start, end);
            highlight("ZAHLFELD", variable, start, end);
            highlight("WORTFELD", variable, start, end);
            highlight("FALLS", command, start, end);
            highlight("SONST", command, start, end);
            highlight("SOLANGE", command, start, end);
            highlight("WIEDERHOLE", command, start, end);
            highlight("UNTERPROGRAMM", command, start, end);
            highlight("FUNKTION", command, start, end);
            highlight("RUECKGABE", command, start, end);
            highlight("FARBE", other, start, end);
            highlight("BEMERKUNG:", comment, start, end);
            highlight("BILDSCHIRMLOESCHEN", other, start, end);
            highlight("SCHREIBEN-OEFFNEN", other, start, end);
            highlight("LESEN-OEFFNEN", other, start, end);

            highlight("VERSUCH", command, start, end);
            highlight("FEHLER", command, start, end);

            codeBox.SelectionStart = backupS;
            codeBox.SelectionLength = backupL;
            codeBox.Enabled = true;
            codeBox.ResumeLayout();
            codeBox.Focus();
            processHighlight = false;
        }
        private void highlight(string element, Color color, int index, int endIndex)
        {
            int elementStart = codeBox.Text.IndexOf(element, index, endIndex-index);
            if (elementStart != -1)
            {
                int backIndex = elementStart + element.Length;
                if (backIndex < codeBox.Text.Length && (codeBox.Text[backIndex] == ' ' || codeBox.Text[backIndex] == '\n' || codeBox.Text[backIndex] == '\t'))
                {
                    codeBox.Select(elementStart, element.Length);
                    codeBox.SelectionColor = color;
                }
                highlight(element, color, elementStart + element.Length, endIndex);
            }
        }
        private int getPriorLineBreak(int index)
        {
            while (index >= 0)
            {
                if (codeBox.Text[index] == '\n')
                    return index;
                index--;
            }
            return 0;
        }
        private int getNextLineBreak(int index)
        {
            while (index < codeBox.Text.Length)
            {
                if (codeBox.Text[index] == '\n')
                    return index;
                index++;
            }
            return codeBox.Text.Length-1;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void startenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBoxCs.Text = "...";
            codeBoxCs.Text = translator.Parse(codeBox.Text);
            performer.Compile(codeBoxCs.Text, "start.exe");
            if (performer.Errors.Count > 0)
            {
                codeBoxCs.Select(codeBoxCs.GetFirstCharIndexFromLine(performer.Errors[0].Line), 2);
                codeBoxCs.SelectionBackColor = Color.DarkRed;
                splitContainer1.Panel2Collapsed = false;
                MessageBox.Show(this, performer.Errors[0].Line+": "+performer.Errors[0].ErrorText, "ERROR: " + performer.Errors[0].ErrorNumber, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
                performer.Start("start.exe");
            }
        }

        private void neuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBox.Text = "START \n  AUSGABE \"Hallo Welt\"\nSTOPP ";
            highlight(0, codeBox.Text.Length);
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        codePath = filePath;
                        if (Path.GetExtension(codePath)==".rtf")
                            codeBox.LoadFile(codePath, RichTextBoxStreamType.RichText);
                        else
                            codeBox.LoadFile(codePath, RichTextBoxStreamType.UnicodePlainText);
                        highlight(0, codeBox.Text.Length);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Path.GetExtension(codePath) == ".rtf")
                    codeBox.SaveFile(codePath, RichTextBoxStreamType.RichText);
                else
                    codeBox.SaveFile(codePath, RichTextBoxStreamType.UnicodePlainText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void speichernUnterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = codePath;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    try
                    {
                        codePath = filePath;
                        if (Path.GetExtension(codePath) == ".rtf")
                            codeBox.SaveFile(codePath, RichTextBoxStreamType.RichText);
                        else
                            codeBox.SaveFile(codePath, RichTextBoxStreamType.UnicodePlainText);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
