using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        Theme theme;
        public CodeForm()
        {
            InitializeComponent();
            ChangeTheme(new Theme()
            {
                Background = Color.FromArgb(45, 60, 65), Text = Color.LightGray,
                Calc = Color.Aquamarine, Flow = Color.SkyBlue, Type = Color.Violet, Action = Color.FromArgb(255, 255, 128), Comment = Color.LimeGreen, String = Color.FromArgb(255, 128, 128)
            });

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
            codeBox.AutoWordSelection = false;
            codeBox.SelectionBullet = false;
        }
        public void ChangeTheme(Theme theme)
        {
            this.theme = theme;
            codeBox.BackColor = theme.Background;
            codeBox.ForeColor = theme.Text;
            codeBox.Font = theme.Font;
        }

        private void paint(object sender,PaintEventArgs e)
        {

        }
        private void codeBox_KeyDown(object sender, KeyEventArgs e)
        {


        }
        private void codeBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!processHighlight && (e.KeyData == Keys.Space || e.KeyData == Keys.Return || e.KeyData == (Keys.Return | Keys.Shift) || e.KeyData == Keys.Decimal || e.KeyData == (Keys.OemPeriod | Keys.Shift) || e.KeyData == (Keys.D7 | Keys.Shift)|| e.KeyData == (Keys.D2 | Keys.Shift)))
                highlight();
        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void codeBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            if (!processHighlight && (e.KeyData == Keys.Return || e.KeyData == (Keys.Return | Keys.Shift)))
                highlight();

            
            var box = (RichTextBox)sender;
            /*
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = false;
                int pos = box.SelectionStart;
                int breakCount = box.SelectedText.Split('\n').Length-1;
                if (breakCount > 0)
                {
                    box.SuspendLayout();
                    box.Enabled = false;
                    int start = getPriorLineBreak(box.SelectionStart-1);
                    int end = getNextLineBreak(box.SelectionStart + box.SelectionLength);
                    int backupS = box.SelectionStart;
                    int backupL = box.SelectionLength;
                    box.SelectionStart = start;
                    box.SelectionStart = getPriorLineBreak(box.SelectionStart);
                    box.SelectedText = box.SelectedText.Replace("\n","\n  ");
                    Console.WriteLine(box.UndoActionName);
                    box.SelectionStart = backupS;
                    highlight(start, end);
                    Console.WriteLine(box.UndoActionName);
                    box.Select(backupS, backupL+ breakCount * 2);
                    box.Enabled = true;
                    box.ResumeLayout();
                    box.Focus();
                }
                else
                {
                    if (e.Shift)
                    {
                        box.Text = box.Text.Remove(pos, 2);
                        box.SelectionStart = pos - 2;
                        Console.WriteLine(pos);
                        //highlight();
                    }
                    else
                    {
                        Console.WriteLine(pos);
                        box.SelectedText = "  ";
                        box.SelectionStart = pos + 2;
                        //highlight();
                    }
                }
            }
            */
            if (e.Control && (e.KeyCode == Keys.Z))
            {
                while (box.CanUndo && box.UndoActionName.Equals("Unknown"))
                {
                    box.Undo();
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
        private void highlight()
        {
            highlight(getPriorChar(codeBox.SelectionStart - 1, '\n'), getNextChar(codeBox.SelectionStart + codeBox.SelectionLength, '\n'));
        }
        private void highlight(int start, int end)
        {
            //Color text = Color.Gray,calc = Color.Aquamarine, command = Color.FromArgb(130, 180, 255), variable = Color.Violet, action = Color.FromArgb(255, 255, 128), comment = Color.LimeGreen;

            processHighlight = true;
            codeBox.SuspendLayout();
            codeBox.Enabled = false;
            int backupS = codeBox.SelectionStart;
            int backupL = codeBox.SelectionLength;
            codeBox.Select(start, end - start);
            codeBox.SelectionColor = theme.Text;
            codeBox.SelectionFont = theme.Font;

            highlight("START", theme.Flow, start, end);
            highlight("STOPP", theme.Flow, start, end);
            highlight("AUSGABE", theme.Action, start, end);
            highlight("AUSGABEREIHE", theme.Action, start, end);
            highlight("ENDE", theme.Flow, start, end);
            highlight("RECHNEN", theme.Calc, start, end);
            highlight("ZAHL", theme.Type, start, end);
            highlight("WORT", theme.Type, start, end);
            highlight("ZAHLEINGABE", theme.Action, start, end);
            highlight("WORTEINGABE", theme.Action, start, end);
            highlight("EINGABE", theme.Action, start, end);
            highlight("ZAHLFELD", theme.Type, start, end);
            highlight("WORTFELD", theme.Type, start, end);
            highlight("FALLS", theme.Flow, start, end);
            highlight("SONST", theme.Flow, start, end);
            highlight("SOLANGE", theme.Flow, start, end);
            highlight("WIEDERHOLE", theme.Flow, start, end);
            highlight("UNTERPROGRAMM", theme.Flow, start, end);
            highlight("FUNKTION", theme.Flow, start, end);
            highlight("RUECKGABE", theme.Flow, start, end);
            highlight("FARBE", theme.Action, start, end);
            highlight("BILDSCHIRMLOESCHEN", theme.Action, start, end);
            highlight("SCHREIBEN-OEFFNEN", theme.Action, start, end);
            highlight("LESEN-OEFFNEN", theme.Action, start, end);
            highlight("VERSUCH", theme.Flow, start, end);
            highlight("FEHLER", theme.Flow, start, end);

            highlight("ZUFALLSZAHL", theme.Action, start, end);
            highlight("WORTINZAHL", theme.Action, start, end);

            highlightComment("BEMERKUNG:", theme.Comment, start, end);
            highlightComment("//", theme.Comment, start, end);
            highlightString("\"",theme.String,start, end);
            codeBox.Select(backupS, backupL);
            codeBox.Enabled = true;
            codeBox.ResumeLayout();
            codeBox.Focus();
            processHighlight = false;
        }
        private void highlight(string element, Color color, int index, int endIndex)
        {
            if (index > endIndex) return;
            int elementStart = codeBox.Text.IndexOf(element, index, endIndex-index);
            if (elementStart != -1)
            {
                int backIndex = elementStart + element.Length;
                if (backIndex < codeBox.Text.Length && (codeBox.Text[backIndex] == ' ' || codeBox.Text[backIndex] == '\n' || codeBox.Text[backIndex] == '\t' || codeBox.Text[backIndex] == '('))
                {
                    codeBox.Select(elementStart, element.Length);
                    codeBox.SelectionColor = color;
                }
                highlight(element, color, elementStart + element.Length, endIndex);
            }
        }
        private void highlightComment(string element, Color color, int index, int endIndex)
        {
            if (index > endIndex) return;
            int elementStart = codeBox.Text.IndexOf(element, index, endIndex - index);
            if (elementStart != -1)
            {
                int backIndex = elementStart + element.Length;
                if (backIndex < codeBox.Text.Length)
                {
                    codeBox.Select(elementStart, getNextChar(elementStart,'\n')- elementStart);
                    codeBox.SelectionColor = color;
                }
                highlightComment(element, color, elementStart + element.Length, endIndex);
            }
        }
        private void highlightString(string element, Color color, int index, int endIndex)
        {
            int startPos = 0;
            bool enable = false;
            for (int i = index; i < endIndex; i++)
            {
                if (enable)
                {
                    if (codeBox.Text[i] == '"')
                    {
                        enable = false;
                        codeBox.Select(startPos, i-startPos+1);
                        codeBox.SelectionColor = color;
                    }
                }
                else
                {
                    if (codeBox.Text[i] == '"')
                    {
                        startPos = i;
                        enable = true;
                    }
                }
            }
        }
        private int getPriorChar(int index,char ch)
        {
            while (index >= 0)
            {
                if (codeBox.Text[index] == ch)
                    return index;
                index--;
            }
            return 0;
        }
        private int getNextChar(int index,char ch)
        {
            while (index < codeBox.Text.Length)
            {

                if (codeBox.Text[index] == ch)
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
                        if (Path.GetExtension(codePath) == ".rtf")
                            codeBox.LoadFile(codePath, RichTextBoxStreamType.RichText);
                        else
                            codeBox.Text = File.ReadAllText(codePath, Encoding.UTF8);
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
                    File.WriteAllText(codePath, codeBox.Text, Encoding.UTF8);
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
                            File.WriteAllText(codePath, codeBox.Text, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void codeBox_MouseDown(object sender, MouseEventArgs e)
        {
            //highlight();
        }

        private void codeBox_MouseClick(object sender, MouseEventArgs e)
        {
            highlight();
        }
    }
}
