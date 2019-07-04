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
using ProLernParser;
using ScintillaNET;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace ProLernEditor
{
    public partial class CodeEditorForm : System.Windows.Forms.Form
    {
        Translator translator;
        Performer performer;
        Button Button = new Button() { };
        bool processHighlight = true;
        string codePath = "";
        Theme theme;
        CodeBox codeBox;
        Button h = new Button();
        public CodeEditorForm()
        {
            InitializeComponent();
            ChangeTheme(new Theme()
            {
                Background = Color.FromArgb(45, 60, 65),
                Text = Color.LightGray,
                Calc = Color.Aquamarine,
                Flow = Color.SkyBlue,
                Type = Color.Violet,
                Action = Color.FromArgb(255, 255, 128),
                Comment = Color.LimeGreen,
                Symbol = Color.LightGray,
                Number = Color.LightGray,
                String = Color.FromArgb(255, 156, 100),
                LineBack = Color.FromArgb(45 / 2, 60 / 2, 65 / 2),
                LineFore = Color.LimeGreen,
                Crusor = Color.White,
                Selection = Color.FromArgb(45 / 4, 60 / 4, 65 / 2),
            });

            codeBox = new CodeBox();
            Controls.Add(codeBox);
            new Highlighter(codeBox, theme);

            codeBox.Size = this.ClientSize;
            codeBox.Top = menuStrip1.Height;
            codeBox.Height -= menuStrip1.Height;
            codeBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            codeBox.AddText("START \n  AUSGABE \"Hallo Welt\"\nSTOPP ");

            performer = new Performer();
            translator = new Translator();
            
        }
        public void ChangeTheme(Theme theme)
        {
            
            this.theme = theme;
            //codeBox.BackColor = theme.Background;
            //codeBox.ForeColor = theme.Text;
            //codeBox.Font = theme.Font;
            
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void erstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cscode = translator.Parse(codeBox.Text);
            performer.Compile(cscode, "start.exe");
            int count = 0;
            int max = performer.Errors.Count;
            foreach (var error in performer.Errors)
            {
                MessageBox.Show(this,
                    ((error.Line)) + ":  " + error.Message, "(" + (count + 1) + " / " + max + ") " + error.Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (count++ >= 10) break;
            }
        }
        private void startenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cscode = translator.Parse(codeBox.Text);
            performer.Compile(cscode, "start.exe");
            if (performer.Errors.Count > 0)
            {
                int count = 0;
                int max = performer.Errors.Count;
                foreach (var error in performer.Errors)
                {
                    MessageBox.Show(this,
                        ((error.Line)) + ":  " + error.Message, "(" + (count + 1) + " / " + max + ") " + error.Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (count++ >= 10) break;
                }
            }
            else
            {
                performer.Start("start.exe");
            }
        }

        private void neuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBox.Text = "START \n  AUSGABE \"Hallo Welt\"\nSTOPP ";
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
                        {
                            var rtb = new RichTextBox();
                            rtb.LoadFile(codePath, RichTextBoxStreamType.RichText);
                            codeBox.Text = rtb.Text;
                            rtb.Dispose();
                        }
                        else
                            codeBox.Text = File.ReadAllText(codePath, Encoding.UTF8);
                        this.Text = codePath + " - ProLern#";
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
                {
                    var rtb = new RichTextBox();
                    rtb.AppendText(codeBox.Text);
                    rtb.SaveFile(codePath, RichTextBoxStreamType.RichText);
                    rtb.Dispose();
                }
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
                        {
                            var rtb = new RichTextBox();
                            rtb.AppendText(codeBox.Text);
                            rtb.SaveFile(codePath, RichTextBoxStreamType.RichText);
                            rtb.Dispose();
                        }
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

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CodeEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            performer.Kill();
        }
    }
}
