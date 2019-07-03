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
        public _SD.Pen _stift = new _SD.Pen(_SD.Color.Black, 1);
        public _SD.SolidBrush _pinsel = new _SD.SolidBrush(_SD.Color.Black);
        public _SD.Graphics _g;
        public _SCG.SortedList<string, _SD.Bitmap> _bitmaps;

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
                            var onclick = (_S.Action<object, _S.EventArgs>)_S.Delegate.CreateDelegate(typeof(_S.Action<object, _S.EventArgs>), this, method);
                            but.Click += new _S.EventHandler(onclick);
                        }
                        this.Controls.Add(control);
                    }
                }
            }
        }
        public void _drawBitmap(string path, double x, double y, double width, double height)
        {
            _SD.Bitmap bitmap;
            if (_bitmaps.TryGetValue(path, out bitmap)) ;
            else _bitmaps.Add(path, bitmap = new _SD.Bitmap(path));
            _g.DrawImage(bitmap, (int)x, (int)y, (int)width, (int)height);
        }
        protected override void OnResize(_S.EventArgs e)
        {
            base.OnResize(e);
            this.Refresh();
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

        public int Length
        {
            get { return array.Length; }
            set
            {
                _S.Array.Resize(ref array, value);
            }
        }
        public T this[int i]
        {
            get { return array[i]; }
            set { array[i] = value; }
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