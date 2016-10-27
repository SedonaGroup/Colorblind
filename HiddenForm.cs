using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ColorFormTracker
{
    public partial class HiddenForm : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        private Graphics _gfx;

        private Bitmap _pixel;

        private Stopwatch _stopWatch;

        private List<ColorListModel> _colorList;

        const int WS_EX_TRANSPARENT = 0x20;

        private long elapsedTime = 0;

        public HiddenForm()
        {
            this.AutoSize = true;
            this.Opacity = 1;
            this.TopMost = true;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.FormBorderStyle = FormBorderStyle.None;
            var GP = new System.Drawing.Drawing2D.GraphicsPath();
            GP.AddRectangle(this.ClientRectangle);
            this.Region = new Region(GP);
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            InitializeComponent();
            this.displayColorName.AutoSize = true;
            _colorList = this.GenerateColorList();
            _pixel = new Bitmap(1, 1);
            _gfx = Graphics.FromImage(_pixel);
            MouseHook.Start();
            MouseHook.MouseAction += new EventHandler(Event);
        }

        private void Event(object sender, EventArgs e)
        {
            if (_stopWatch.ElapsedMilliseconds - elapsedTime > 50)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                _gfx.CopyFromScreen(new Point(((MouseHook.POINT)sender).x, ((MouseHook.POINT)sender).y), new Point(), _pixel.Size);
                var result = _pixel.LockBits(new Rectangle(0, 0, 1, 1), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                unsafe
                {
                    byte* resultPointer = (byte*)result.Scan0;
                    var blue = resultPointer[0]; // Blue
                    var green = resultPointer[1]; // Green
                    var red = resultPointer[2]; // Red
                    //var alpha = resultPointer[3]; // Alpha
                    var color = Color.FromArgb(255, red, green, blue);
                    this.displayColorName.Text = GetColorName(color);
                }
                _pixel.UnlockBits(result);
                _stopWatch.Restart();
                elapsedTime = 0;
                Point pt = Cursor.Position;
                pt.Offset(+20, +10);
                this.Location = pt;
                this.Size = this.displayColorName.Size;
                return;
            }
            elapsedTime = _stopWatch.ElapsedMilliseconds;
        }

        private List<ColorListModel> GenerateColorList()
        {
            var result = new List<ColorListModel>();
            var colorType = typeof(Color);
            PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public); ;
            foreach(var prop in propInfos)
            {
                if (!prop.Name.Equals("Transparent"))
                {
                    result.Add(new ColorListModel
                    {
                        Name = prop.Name,
                        R = Color.FromName(prop.Name).R,
                        G = Color.FromName(prop.Name).G,
                        B = Color.FromName(prop.Name).B,
                    });
                }
            }

            return result.OrderBy(x => x.Name).ToList();
        }

        private string GetColorName(Color color)
        {
            var neareastColor = new ColorListModel();
            var neareastColorValue = int.MaxValue;

            foreach (var c in this._colorList)
            {
                if (color == Color.FromArgb(255, c.R, c.G, c.B))
                {
                    return c.Name;
                }
                else if(Math.Abs(color.R - c.R) + Math.Abs(color.G - c.G) + Math.Abs(color.B - c.B) < neareastColorValue)
                {
                    neareastColorValue = Math.Abs(color.R - c.R) + Math.Abs(color.G - c.G) + Math.Abs(color.B - c.B);
                    neareastColor = c;
                }
            }

            return neareastColor.Name;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
                return cp;
            }
        }
    }
}
