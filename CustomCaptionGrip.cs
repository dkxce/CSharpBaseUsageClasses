//
// C# 
// https://github.com/dkxce
// en,ru,1251,utf-8
//

using MSol;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace dkxce
{
	partial class CustomCaptionForm: Form
	{
        #region CustomGrip

        #region WM_NCHITTEST Message Consts

        const int WM_NCHITTEST = 0x84;

        const int HTNOWHERE = 0;
        const int HTCLIENT = 1;
        const int HTCAPTION = 2;
        const int HTSYSMENU = 3;
        const int HTSIZE = 4;
        const int HTMENU = 5;
        const int HTHSCROLL = 6;
        const int HTVSCROLL = 7;
        const int HTMINBUTTON = 8;
        const int HTMAXBUTTON = 9;
        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 16;
        const int HTBOTTOMRIGHT = 17;
        const int HTBORDER = 18;
        const int HTCLOSE = 20;
        const int HTHELP = 21;

        #endregion WM_NCHITTEST Message Consts

        #region defaults
        private const bool CustomGripIsVisible = false;             // Grip and Borders is Visible
        private const bool CustomGripInvisible = true;              // Grip and Borders is Invisible
        private int CustomGripSize = CustomGripIsVisible ? 16 : 00; // 16 (By Corner)
        private int CustomGripWidth = CustomGripIsVisible ? 0 : 00; //  0 (No By Sides) or 3 (By Sides)
        private Color CustomGripColorB = Panel.DefaultBackColor;
        private Color CustomGripColorF = Color.Gray;
        #endregion defaults

        private void InitCustomGrip()
        {
            if (!CustomGripIsVisible)
            {
                if (!CustomGripInvisible) return;

                TransparentPanel tp;

                // SizeGrip (Bottom-Left Angle)
                tp = new TransparentPanel(this) { Cursor = Cursors.SizeNWSE, OffsetLeft = -12, OffsetTop = -8, OffsetWidth = 12, OffsetHeight = 12 };
                tp.MouseDown += (object sender, MouseEventArgs e) => { ReleaseCapture(); if (e.Button == MouseButtons.Left && e.Clicks == 1) SendMessage(Handle, WM_NCLBUTTONDOWN, HTBOTTOMRIGHT, 0); };

                // Bottom Grip
                tp = new TransparentPanel(this) { Cursor = Cursors.SizeNS, OffsetLeft = 0, OffsetTop = -2, OffsetWidth = -12, OffsetHeight = 2 };
                tp.MouseDown += (object sender, MouseEventArgs e) => { ReleaseCapture(); if (e.Button == MouseButtons.Left && e.Clicks == 1) SendMessage(Handle, WM_NCLBUTTONDOWN, HTBOTTOM, 0); };

                // Left Grip
                tp = new TransparentPanel(this) { Cursor = Cursors.SizeWE, OffsetLeft = 0, OffsetTop = 0, OffsetWidth = 2, OffsetHeight = -2 };
                tp.MouseDown += (object sender, MouseEventArgs e) => { ReleaseCapture(); if (e.Button == MouseButtons.Left && e.Clicks == 1) SendMessage(Handle, WM_NCLBUTTONDOWN, HTLEFT, 0); };

                // Right Grip
                tp = new TransparentPanel(this) { Cursor = Cursors.SizeWE, OffsetLeft = -2, OffsetTop = 0, OffsetWidth = 2, OffsetHeight = -2 };
                tp.MouseDown += (object sender, MouseEventArgs e) => { ReleaseCapture(); if (e.Button == MouseButtons.Left && e.Clicks == 1) SendMessage(Handle, WM_NCLBUTTONDOWN, HTRIGHT, 0); };
            };
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e); // defaults

            if (!CustomGripIsVisible) return;

            // Remove Grip Regions From Form Active Area
            Region clipped = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            if (this.WindowState == FormWindowState.Maximized || (CustomGripSize == 0 && CustomGripWidth == 0))
                /* DO NOT CLIP CLIENT REGION */;
            else
            {
                if (CustomGripSize > 0)
                {
                    clipped.Exclude(new Rectangle(this.ClientRectangle.Width - CustomGripSize, this.ClientRectangle.Height - CustomGripSize, CustomGripSize, CustomGripSize));
                };
                if (CustomGripWidth > 0)
                {
                    clipped.Exclude(new Rectangle(0, 0, CustomGripWidth, this.ClientRectangle.Height)); // Left
                    clipped.Exclude(new Rectangle(0, 0, this.ClientRectangle.Width, CustomGripWidth));  // Top
                    clipped.Exclude(new Rectangle(this.ClientRectangle.Width - CustomGripWidth, 0, CustomGripWidth, this.ClientRectangle.Height)); // Right
                    clipped.Exclude(new Rectangle(0, this.ClientRectangle.Height - CustomGripWidth, this.ClientRectangle.Width, CustomGripWidth)); // Bottom
                };
            };
            wB.Region = clipped;
            this.Invalidate(); // redraw
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); // defaults

            if ((!CustomGripIsVisible) || this.WindowState == FormWindowState.Maximized || (CustomGripSize == 0 && CustomGripWidth == 0)) return;

            // Draw Grip and Grip Borders
            Rectangle rc = new Rectangle(this.ClientSize.Width - CustomGripSize, this.ClientSize.Height - CustomGripSize, CustomGripSize, CustomGripSize);
            if (CustomGripColorB.HasValue && CustomGripColorF.HasValue)
            {
                if (CustomGripWidth > 0)
                {
                    // Grip Borders: Left-Top-Tight-Bottom
                    e.Graphics.FillRectangle(new SolidBrush(CustomGripColorB), new Rectangle(0, 0, CustomGripWidth, this.ClientRectangle.Height)); // Left
                    e.Graphics.FillRectangle(new SolidBrush(CustomGripColorB), new Rectangle(0, 0, this.ClientRectangle.Width, CustomGripWidth)); // Top
                    e.Graphics.FillRectangle(new SolidBrush(CustomGripColorB), new Rectangle(this.ClientRectangle.Width - CustomGripWidth, 0, CustomGripWidth, this.ClientRectangle.Height)); // Right
                    e.Graphics.FillRectangle(new SolidBrush(CustomGripColorB), new Rectangle(0, this.ClientRectangle.Height - CustomGripWidth, this.ClientRectangle.Width, CustomGripWidth)); // Bottom
                };
                if (CustomGripSize > 0)
                {
                    // Grip Drag: 3 Lines
                    e.Graphics.FillRectangle(new SolidBrush(CustomGripColorB), rc);
                    e.Graphics.DrawLine(new Pen(new SolidBrush(CustomGripColorF), 1.5f), rc.Left + 2, rc.Bottom - 2, rc.Right - 2, rc.Top + 2);
                    e.Graphics.DrawLine(new Pen(new SolidBrush(CustomGripColorF), 1.5f), rc.Left + 6, rc.Bottom - 2, rc.Right - 2, rc.Top + 6);
                    e.Graphics.DrawLine(new Pen(new SolidBrush(CustomGripColorF), 1.5f), rc.Left + 10, rc.Bottom - 2, rc.Right - 2, rc.Top + 10);
                };
            }
            else
                ControlPaint.DrawSizeGrip(e.Graphics, Color.White, rc);
        }        

        #endregion CustomGrip

        #region CustomCaption

        private void InitCustomCaption()
		{
			if (string.IsNullOrEmpty(MSol.HostApplication.Config.NoCaption) || (MSol.HostApplication.Config.NoCaption == "0")) return;
			if (string.IsNullOrEmpty(HostApplication.Config.CustomCaption)) return;
			
			HoverPanel pb = new HoverPanel(this) { BoxOnHover = false, BackColor = CustomGripColorB, OffsetLeft = 0, OffsetTop =0 , OffsetWidth = -1, Height = 16 };
			pb.MouseDown +=  (object sender, MouseEventArgs e) => { ReleaseCapture(); if (e.Button == MouseButtons.Left && e.Clicks == 1) SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); };
			pb.MouseDoubleClick += (object sender, EventArgs e) = >  { if (this.WindowState == FormWindowState.Normal) this.WindowState = FormWindowState.Maximized; else if (this.WindowState == FormWindowState.Maximized) this.WindowState = FormWindowState.Normal; };			
		}        
		
        #endregion CustomCaption
		
		// Form.WndProc(ref Message m) -- Tell to OS that XY is form border
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (CustomGripSize > 0 || CustomGripWidth > 0)
            {
                // Resize Borderless Form
                // https://stackoverflow.com/questions/32310319/resize-a-borderless-form-that-has-controls-everywhere-no-empty-space
                // https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest
                if (m.Msg == WM_NCHITTEST)
                {
                    Point pos = this.PointToClient(new Point(m.LParam.ToInt32() & 0xFFFF, m.LParam.ToInt32() >> 16));
                    if (CustomGripSize > 0)
                    {
                        if (pos.X >= this.ClientSize.Width - CustomGripSize && pos.Y >= this.ClientSize.Height - CustomGripSize) { m.Result = (IntPtr)HTBOTTOMRIGHT; return; };
                    };
                    if (CustomGripWidth > 0)
                    {
                        if (pos.X <= CustomGripWidth) { m.Result = (IntPtr)HTLEFT; return; };
                        if (pos.X >= this.ClientSize.Width - CustomGripWidth) { m.Result = (IntPtr)HTRIGHT; return; };
                        if (pos.Y >= this.ClientSize.Height - CustomGripWidth) { m.Result = (IntPtr)HTBOTTOM; return; };
                    };
                };
            };
        }
    }

    #region Transparent and Hidden Controls

    /// <summary>
    ///     Transparent Panel (as Caption) Drag Window By Panel
    ///     Прозрачная панель (как заголовок) Перетаскивание окна за заголовок
    /// </summary>
    /// <remarks>
    ///     Прозрачная панель с возможностью создания ее как заголовка окна.
    ///     По умолчанию управляет активным окном (или WindowHandle)
    ///     var sample = new TransparentPanel(this) { Dock = DockStyle.None, Top = 0, Left = 0, Width = 800, Height = 600 };
    /// </remarks>
    public class TransparentPanel : Panel
    {
        #region WinAPI

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseCapture();

        // Consts
        public const int HT_CAPTION = 0x0002;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WS_EX_TRANSPARENT = 0x0020;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion WinAPI

        public enum TransparentMode : byte
        {
            None = 0, // Not Transparent at All
            Parent = 1, // Transparent with Parent background (Invisible Mode)
            Form = 1, // Transparent with Parent background (Invisible Mode)
            Invisible = 1, // Transparent with Parent background (Invisible Mode)
            Desktop = 2, // Transparent with Desktop background (Hole Mode)
            Hole = 2, // Transparent with Desktop background (Hole Mode)
            Throughout = 2  // Transparent with Desktop background (Hole Mode)
        }

        private static TransparentMode CreateMode = TransparentMode.Parent;

        private int _left = 0;
        private int _top = 0;
        private int _wi = 0;
        private int _he = 0;

        #region Params    

        public int OffsetLeft
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
                if (_left >= 0) this.Left = _left;
                else this.Left = Parent.Width + _left;
            }
        }

        public int OffsetTop
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
                if (_top >= 0) this.Top = _top;
                else this.Top = Parent.Height + _top;
            }
        }

        public int OffsetWidth
        {
            get
            {
                return _wi;
            }
            set
            {
                _wi = value;
                if (_wi > 0) this.Width = _wi;
                else if (_wi < 0) this.Width = Parent.Width - (_left >= 0 ? _left : Parent.Width + _left) + _wi;
            }
        }

        public int OffsetHeight
        {
            get
            {
                return _he;
            }
            set
            {
                _he = value;
                if (_he > 0) this.Height = _he;
                else if (_he < 0) this.Height = Parent.Height - (_top >= 0 ? _top : Parent.Height + _top) + _he;
            }
        }

        private TransparentMode RunTimeMode = TransparentMode.Parent;
        public TransparentMode Transparent { get { return RunTimeMode; } }
        public bool AsCaption { get; private set; } = false;
        public bool TopLeftMenu { get; set; } = false;
        public IntPtr WindowHandle { get; set; } = IntPtr.Zero;


        #endregion Params

        #region Constructor

        public TransparentPanel(Form parent, bool asCaption = false)
        {
            this.RunTimeMode = TransparentPanel.CreateMode;
            this.Visible = false;
            if (this.Parent != null) this.Parent.Controls.Add(this);
            this.Parent = parent;
            if (this.Parent != null) WindowHandle = this.Parent.Handle;
            if (this.AsCaption = asCaption) this.MouseDown += TransparentPanel_MouseDown;
            AfterCreate();
        }

        public TransparentPanel(Control parent = null, bool asCaption = false)
        {
            this.RunTimeMode = TransparentPanel.CreateMode;
            this.Visible = false;
            this.Parent = parent;
            if (this.AsCaption = asCaption) this.MouseDown += TransparentPanel_MouseDown;
            AfterCreate();
        }

        public static TransparentPanel Create(TransparentMode Mode, Form parent, bool asCaption = false)
        {
            TransparentPanel.CreateMode = Mode;
            TransparentPanel res = new TransparentPanel(parent, asCaption);
            TransparentPanel.CreateMode = TransparentMode.Parent;
            return res;
        }

        public static TransparentPanel Create(TransparentMode Mode, Control parent = null, bool asCaption = false)
        {
            TransparentPanel.CreateMode = Mode;
            TransparentPanel res = new TransparentPanel(parent, asCaption);
            TransparentPanel.CreateMode = TransparentMode.Parent;
            return res;
        }

        private void AfterCreate()
        {
            // https://learn.microsoft.com/en-us/previous-versions/dotnet/netframework-3.0/wk5b13s4(v=vs.85)?redirectedfrom=MSDN
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;
            if (Parent != null) Parent.Resize += Parent_Resize;
            if (Parent != null) this.BringToFront();
            this.Visible = true;
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            if (_left >= 0) this.Left = _left;
            else this.Left = Parent.Width + _left;
            if (_top >= 0) this.Top = _top;
            else this.Top = Parent.Height + _top;
            if (_wi > 0) this.Width = _wi;
            else if (_wi < 0) this.Width = Parent.Width - (_left >= 0 ? _left : Parent.Width + _left) + _wi;
            if (_he > 0) this.Height = _he;
            else if (_he < 0) this.Height = Parent.Height - (_top >= 0 ? _top : Parent.Height + _top) + _he;
        }

        #endregion Constructor

        #region Override

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (TransparentPanel.CreateMode == TransparentMode.Parent) cp.ExStyle |= WS_EX_TRANSPARENT;
                return cp;
            }
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);            
        //}

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.RunTimeMode != TransparentMode.Desktop) base.OnPaintBackground(e);
        }

        #endregion Override

        #region Captible

        private void TransparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
            IntPtr hWnd = WindowHandle == IntPtr.Zero ? GetForegroundWindow() : WindowHandle;
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                if (e.Clicks == 1)     // Drag Window by Caption
                    SendMessage(hWnd, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                else if (e.Clicks > 1) // Maximize/Restore by Caption
                    SendMessage(hWnd, WM_NCLBUTTONDBLCLK, HT_CAPTION, 0);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ReleaseCapture();
                TransparentPanel.GetWindowRect(hWnd, out TransparentPanel.RECT pos);
                IntPtr hMenu = GetSystemMenu(hWnd, false);
                // Point po = (sender as Control).PointToScreen(Point.Empty)
                int dX = TopLeftMenu ? 0 : e.X;
                int dY = TopLeftMenu ? 0 : e.Y;
                int hBool = TrackPopupMenu(hMenu, WM_KEYDOWN, pos.Left + dX, pos.Top + dY, 0, hWnd, IntPtr.Zero);
                if (hBool > 0) SendMessage(hWnd, WM_SYSCOMMAND, hBool, 0);
            };
        }

        #endregion Captible
    }

    /// <summary>
    ///     HoverPanel (with Image as Background)   
    /// </summary>
    public class HoverPanel : Panel
    {
        private bool _msOver = false;
        private int _left = 0;
        private int _top = 0;
        private int _wi = 0;
        private int _he = 0;

        public int OffsetLeft
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
                if (_left >= 0) this.Left = _left;
                else this.Left = Parent.Width + _left;
            }
        }

        public int OffsetTop
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
                if (_top >= 0) this.Top = _top;
                else this.Top = Parent.Height + _top;
            }
        }

        public int OffsetWidth
        {
            get
            {
                return _wi;
            }
            set
            {
                _wi = value;
                if (_wi > 0) this.Width = _wi;
                else if (_wi < 0) this.Width = Parent.Width - (_left >= 0 ? _left : Parent.Width + _left) + _wi;
            }
        }

        public int OffsetHeight
        {
            get
            {
                return _he;
            }
            set
            {
                _he = value;
                if (_he > 0) this.Height = _he;
                else if (_he < 0) this.Height = Parent.Height - (_top >= 0 ? _top : Parent.Height + _top) + _he;
            }
        }

        public Image Image
        {
            get
            {
                return this.BackgroundImage;
            }
            set
            {
                this.Width = value == null ? 0 : value.Width;
                this.Height = value == null ? 0 : value.Height;
                this.BackgroundImage = value;
            }
        }

        public bool EnlargeOnHover { set; get; } = false;
        public bool BoxOnHover { set; get; } = false;
        public Pen BoxPen { set; get; } = new Pen(new SolidBrush(Color.Silver), 3);

        public HoverPanel(Control Parent)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor |
            ControlStyles.UserPaint, true);
            UpdateStyles();

            // this.Cursor = Cursors.Hand;
            this.MouseEnter += (object sender, EventArgs e) => { _msOver = true; this.Invalidate(); };
            this.MouseLeave += (object sender, EventArgs e) => { _msOver = false; this.Invalidate(); };
            Parent.Controls.Add(this);
            this.BackColor = Color.Transparent;
            this.BringToFront();

            Parent.Resize += Parent_Resize;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (this.BackgroundImage != null)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                Rectangle rectSrc = new Rectangle(0, 0, this.BackgroundImage.Width, this.BackgroundImage.Height);
                Rectangle rectDes = new Rectangle(0, 0, Width, Height);
                if (EnlargeOnHover && _msOver) rectDes.Inflate(2, 2);
                g.DrawImage(this.BackgroundImage, rectDes, rectSrc, GraphicsUnit.Pixel);
                if (BoxOnHover && _msOver)
                {
                    g.DrawLine(BoxPen, rectDes.Left, rectDes.Top, rectDes.Right - 1, rectDes.Top);
                    g.DrawLine(BoxPen, rectDes.Right - 1, rectDes.Top, rectDes.Right - 1, rectDes.Bottom - 1);
                    g.DrawLine(BoxPen, rectDes.Right - 1, rectDes.Bottom - 1, rectDes.Left, rectDes.Bottom - 1);
                    g.DrawLine(BoxPen, rectDes.Left, rectDes.Bottom - 1, rectDes.Left, rectDes.Top);
                };
            }
            else
                base.OnPaint(e);
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            if (_left >= 0) this.Left = _left;
            else this.Left = Parent.Width + _left;
            if (_top >= 0) this.Top = _top;
            else this.Top = Parent.Height + _top;
            if (_wi > 0) this.Width = _wi;
            else if (_wi < 0) this.Width = Parent.Width - (_left >= 0 ? _left : Parent.Width + _left) + _wi;
            if (_he > 0) this.Height = _he;
            else if (_he < 0) this.Height = Parent.Height - (_top >= 0 ? _top : Parent.Height + _top) + _he;
        }
    }

    /*
    public class NonOpaquePanel : Panel
    {
        private int opacity = 100;

        public NonOpaquePanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = Color.Transparent;
        }

        public int Opacity
        {
            get
            {
                if (opacity > 100) opacity = 100;
                else if (opacity < 1) opacity = 1;
                return this.opacity;
            }
            set
            {
                this.opacity = value;
                if (this.Parent != null) Parent.Invalidate(this.Bounds, true);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            int alpha = (opacity * 255) / 100;
            Brush bckColor = new SolidBrush(Color.FromArgb(alpha, this.BackColor));
            if (this.BackColor != Color.Transparent) g.FillRectangle(bckColor, bounds);            
            bckColor.Dispose();
            g.Dispose();
            base.OnPaint(e);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            if (this.Parent != null) Parent.Invalidate(this.Bounds, true);
            base.OnBackColorChanged(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnParentBackColorChanged(e);
        }
    }
    */

    #endregion Transparent and Hidden Controls
}
