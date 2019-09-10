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
    class Highlighter
    {
        CodeBox control;
        Theme theme;
        public Theme Theme
        {
            set
            {
                theme = value;
            }
        }
        public Highlighter(CodeBox control,Theme theme)
        {
            this.control = control;
            this.theme = theme;

            control.Lexer = Lexer.Container;
            control.Margins[0].Width = 32;
            control.StyleNeeded += Highlight;
            control.KeyDown += Control_KeyDown;
            control.PreviewKeyDown += Control_PreviewKeyDown;
            control.TabWidth = 2;
            //control.IdleStyling = IdleStyling.AfterVisible;

            control.CaretForeColor = theme.Crusor;
            control.Styles[Style.Default].BackColor = theme.Background;
            control.Styles[Style.Default].ForeColor = theme.Text;
            control.Styles[Style.Default].Font = "consolas";
            control.Styles[Style.Default].Size = 10;
            control.StyleClearAll();

            control.SetSelectionBackColor(true,theme.Selection);

            control.Styles[Style.LineNumber].BackColor = theme.LineBack;
            control.Styles[Style.LineNumber].ForeColor = theme.LineFore;

            control.Styles[(int)Theme.Style.Action].ForeColor = theme.Action;
            control.Styles[(int)Theme.Style.Background].ForeColor = theme.Background;
            control.Styles[(int)Theme.Style.Calc].ForeColor = theme.Calc;
            control.Styles[(int)Theme.Style.Comment].ForeColor = theme.Comment;
            control.Styles[(int)Theme.Style.Flow].ForeColor = theme.Flow;
            control.Styles[(int)Theme.Style.String].ForeColor = theme.String;
            control.Styles[(int)Theme.Style.Text].ForeColor = theme.Text;
            control.Styles[(int)Theme.Style.Type].ForeColor = theme.Type;
            control.Styles[(int)Theme.Style.Symbol].ForeColor = theme.Symbol;
            control.Styles[(int)Theme.Style.Number].ForeColor = theme.Number;


        }

        private void Control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.A)
            {
                e.IsInputKey = false;

            }
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;

        }

        public void Highlight()
        {
            
            Highlight(0, control.Text.Length);
        }
        public void Highlight(object obj, StyleNeededEventArgs args)
        {
            var control = (CodeBox)obj;
            int endStyled = control.GetEndStyled();
            int begin = control.GetLineStart(endStyled - 2);
            int end = control.GetLineEnd(args.Position + 2);
            Highlight(begin, end);
        }
        public void Highlight(int begin, int end)
        {
            if (begin < 0) begin = 0;
            if (end > control.Text.Length) end = control.Text.Length;

            control.StartStyling(begin);
            string code = control.Text;
            int index = begin;

            while (index < end)
            {
                if (highlightWordToLineEnd(ref code, ref index, "//", Theme.Style.Comment)) ;
                else if (highlightWordToLineEnd(ref code, ref index, "BEMERKUNG:", Theme.Style.Comment)) ;
                else if (highlightBetwenWords(ref code, ref index, "\"", Theme.Style.String)) ;
                else if (highlightWord(ref code, ref index, "START", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "STOPP", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "AUSGABE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "AUSGABEREIHE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "ENDE", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "RECHNEN", Theme.Style.Calc)) ;
                else if (highlightWord(ref code, ref index, "ZAHL", Theme.Style.Type)) ;
                else if (highlightWord(ref code, ref index, "WORT", Theme.Style.Type)) ;
                else if (highlightWord(ref code, ref index, "ZAHLEINGABE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "WORTEINGABE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "EINGABE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "ZAHLFELD", Theme.Style.Type)) ;
                else if (highlightWord(ref code, ref index, "WORTFELD", Theme.Style.Type)) ;
                else if (highlightWord(ref code, ref index, "FALLS", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "SONST", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "SOLANGE", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "WIEDERHOLE", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "UNTERPROGRAMM", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "FUNKTION", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "RUECKGABE", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "FARBE", Theme.Style.Action)) ;

                else if (highlightWord(ref code, ref index, "BILDSCHIRMLOESCHEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "SCHREIBEN-OEFFNEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "LESEN-OEFFNEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "ZAHL-LESEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "WORT-LESEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "WORT-SCHREIBEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "ZAHL-SCHREIBEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "SCHREIBEN-SCHLIESSEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "LESEN-SCHLIESSEN", Theme.Style.Action)) ;

                else if (highlightWord(ref code, ref index, "FENSTER", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "FENSTERKONSTRUKTOR", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "FENSTERNEUZEICHNEN", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "OEFFNEFENSTER", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "FENSTERFARBE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "FENSTERGROESSE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "FENSTERLOESCHEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "MAUSKLICK", Theme.Style.Flow)) ;

                else if (highlightWord(ref code, ref index, "ZEICHNEWORT", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "RECHTECK", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "RECHTECKFUELLEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "ELLIPSE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "ELLIPSEFUELLEN", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "LINIE", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "PINSEL", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "STIFT", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "BILD", Theme.Style.Action)) ;

                else if (highlightWord(ref code, ref index, "KNOPF", Theme.Style.Type)) ;
                else if (highlightWord(ref code, ref index, "WORTBOX", Theme.Style.Type)) ;
                else if (highlightWord(ref code, ref index, "KNOPFKLICK", Theme.Style.Flow)) ;

                else if (highlightWord(ref code, ref index, "VERSUCH", Theme.Style.Flow)) ;
                else if (highlightWord(ref code, ref index, "FEHLER", Theme.Style.Flow)) ;

                else if (highlightWord(ref code, ref index, "ZUFALLSZAHL", Theme.Style.Action)) ;
                else if (highlightWord(ref code, ref index, "WORTINZAHL", Theme.Style.Action)) ;

                else if (highlightSymbols(ref code, ref index, null, Theme.Style.Symbol)) ;
                else if (highlightNumbers(ref code, ref index, null, Theme.Style.Number)) ;

                else
                {
                    control.SetStyling(1, (int)Theme.Style.Text);
                    index += 1;
                }
            }
        }

        private bool highlightSymbols(ref string code, ref int index,string symbols, Theme.Style style)
        {
            char symbol = code[index];
            if (!(symbol >= '0' && symbol <= '9') && !(symbol >= 'A' && symbol <= 'Z') && !(symbol >= 'a' && symbol <= 'z') && !(symbol >= 128))
            {
                control.SetStyling(1, (int)style);
                index += 1;
                return true;
            }
            return false;
        }
        private bool highlightNumbers(ref string code, ref int index, string symbols, Theme.Style style)
        {
            if (index > 0)
            {
                char symbol = code[index - 1];
                if (!(symbol >= '0' && symbol <= '9') && !(symbol >= 'A' && symbol <= 'Z') && !(symbol >= 'a' && symbol <= 'z') && !(symbol >= 128))
                {
                    int max = control.GetLineEnd(index);
                    int length = 0;
                    while (index < max)
                    {
                        if (code[index] >= '0' && code[index] <= '9' || code[index] == '.')
                        {
                            length += 1;
                            index += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    control.SetStyling(length, (int)style);
                    return length > 0;
                }
            }
            return false;

        }
        private bool highlightWord(ref string code, ref int index, string word, Theme.Style style)
        {
            if (index + word.Length > code.Length) return false;

            for (int i = 0;i< word.Length; i++)
            {
                if (code[index + i] != word[i]) return false;
            }

            if (index + word.Length + 1 <= code.Length)
            {
                if (code[index + word.Length] != ' '&& code[index + word.Length] != '\n'&& code[index + word.Length] != '\r'&& code[index + word.Length] != '('&& code[index + word.Length] != '\t') return false;
            }
            control.SetStyling(word.Length, (int)style);
            index += word.Length;
            return true;
        }
        private bool highlightWordToLineEnd(ref string code, ref int index, string word, Theme.Style style)
        {
            if (index + word.Length > code.Length) return false;

            for (int i = 0; i < word.Length; i++)
            {
                if (code[index + i] != word[i]) return false;
            }

            int max = control.GetLineEnd(index);
            int length = word.Length;
            index += word.Length;
            while (index < max)
            {
                length += 1;
                index += 1;
            }
            control.SetStyling(length, (int)style);
            return true;
        }
        private bool highlightBetwenWords(ref string code, ref int index, string word, Theme.Style style)
        {
            if (index >= code.Length || code[index] != word[0]) return false;

            int max = control.GetLineEnd(index);
            int length = word.Length;
            index += 1;
            while (index < max)
            {
                length += 1;
                index += 1;
                if (code[index-1] == word[0])
                {
                    break;
                }
            }
            control.SetStyling(length, (int)style);
            return true;
        }
    }
}
