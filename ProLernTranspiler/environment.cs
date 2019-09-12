using _S = System;
using _SI = System.IO;
using _SWF = System.Windows.Forms;
using _SD = System.Drawing;
using _SR = System.Reflection;
using _SCG = System.Collections.Generic;

namespace ProLernProgram
{
    public class _button : _SWF.Button
    {
        public string clickname;
    }
    public class _proLernFormProto : _SWF.Form
    {
        public _SI.StreamWriter _writer;
        public _SI.StreamReader _reader;
        public _SD.Pen _stift = new _SD.Pen(_SD.Color.Black, 1);
        public _SD.SolidBrush _pinsel = new _SD.SolidBrush(_SD.Color.Black);
        public _SD.Graphics _g;
        public _SCG.SortedList<string, _SD.Bitmap> _bitmaps = new _SCG.SortedList<string, _SD.Bitmap>();

        public _proLernFormProto()
        {
            _g = base.CreateGraphics();
            base.DoubleBuffered = true;
            var type = this.GetType();
            var objFields = type.GetFields();

            foreach (var field in objFields)
            {
                if (field.FieldType.IsSubclassOf(typeof(_SWF.Control)))
                {
                    _SWF.Control control = (_SWF.Control)field.GetValue(this);
                    if (control != null)
                    {
                        if (field.FieldType == typeof(_button))
                        {
                            var but = (_button)control;
                            var method = type.GetMethod(but.clickname);
                            if (method != null)
                            {
                                var onclick = (_S.Action<object, _S.EventArgs>)_S.Delegate.CreateDelegate(typeof(_S.Action<object, _S.EventArgs>), this, method);
                                but.Click += new _S.EventHandler(onclick);
                            }
                        }
                        this.Controls.Add(control);
                    }
                }
            }
            AutoSize = true;
        }
        public void _drawBitmap(_SD.Graphics _g, string path, double x, double y, double width, double height)
        {
            _SD.Bitmap bitmap;
            if (_bitmaps.TryGetValue(path, out bitmap)) ;
            else if (_SI.File.Exists(path)) _bitmaps.Add(path, bitmap = new _SD.Bitmap(path));
            else
            {
                bitmap = new _SD.Bitmap(64, 64);
                using (var g = _SD.Graphics.FromImage(bitmap))
                {
                    g.DrawRectangle(new _SD.Pen(_SD.Color.Red, 4), new _SD.Rectangle(0, 0, 64, 64));
                }
                _bitmaps.Add(path, bitmap);
            }
            _g.DrawImage(bitmap, (int)x, (int)y, (int)width, (int)height);
        }
        protected override void OnClosed(_S.EventArgs e)
        {
            base.OnClosed(e);
            _S.Environment.Exit(0);
        }
        protected override void OnResize(_S.EventArgs e)
        {
            base.OnResize(e);
            this.Refresh();
            _g = base.CreateGraphics();
        }
        public double ZUFALLSZAHL(double max)
        {
            var rnd = new _S.Random(); return rnd.Next((int)1, (int)max);
        }
        public double WORTINZAHL(string str)
        {
            return double.Parse(str);
        }
    }
    public class _array<T>
    {
        T[] array;

        public _array(int size)
        {
            array = new T[size];
        }

        public static _array<T> operator +(_array<T> lhs, T rhs)
        {
            int index = lhs.array.Length;
            lhs.Length = index + 1;
            lhs.array[index] = rhs;
            return lhs;
        }

        public static _array<T> operator +(_array<T> lhs, _array<T> rhs)
        {
            int offset = (int)lhs.Length;
            lhs.Length += rhs.Length;
            _S.Array.Copy(rhs.array, 0, lhs.array, offset, (int)rhs.Length);
            return lhs;
        }

        public double Length
        {
            get { return array.Length; }
            set
            {
                _S.Array.Resize(ref array, (int)value);
            }
        }
        public T this[int i]
        {
            get { return array[i]; }
            set { array[i] = value; }
        }
        public override string ToString()
        {
            string result = "("+ array.Length + ") [";
            for (int i = 0; i < array.Length; i++)
            {
                result += ""+array[i];
                if (i < array.Length - 1)
                    result += ", ";
                else
                    result += "]";
            }
            return result;
        }
    }
    public class Program
    {
        static public void Main()
        {
            new Program().START();
        }
        public _SI.StreamWriter _writer;
        public _SI.StreamReader _reader;

        public double ZUFALLSZAHL(double max)
        {
            var rnd = new _S.Random(); return rnd.Next((int)1, (int)max);
        }
        public double WORTINZAHL(string str)
        {
            return double.Parse(str);
        }

        ///<include>code

    }
}
