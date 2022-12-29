//
// github.com/dkxce
// en,ru,1251,utf-8
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace dkxce
{
    /// <summary>
    ///     dkxce Transparent Alpha-Blended Opacity Panel
    /// </summary>
    public class AlphaBlendedPanel : Panel
    {
        #region WinAPI Consts
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_MOUSELEAVE = 0x02A3;
        private const int WM_PAINT = 0x000F;
        private const int WM_ERASEBKGND = 0x0014;
        private const int WM_PRINT = 0x0317;
        private const int EN_HSCROLL = 0x0601;
        private const int EN_VSCROLL = 0x0602;
        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;
        private const int EM_GETSEL = 0x00B0;
        private const int EM_LINEINDEX = 0x00BB;
        private const int EM_LINEFROMCHAR = 0x00C9;
        private const int EM_POSFROMCHAR = 0x00D6;
        private const int WM_PRINTCLIENT = 0x0318;
        private const long PRF_CHECKVISIBLE = 0x00000001L;
        private const long PRF_NONCLIENT = 0x00000002L;
        private const long PRF_CLIENT = 0x00000004L;
        private const long PRF_ERASEBKGND = 0x00000008L;
        private const long PRF_CHILDREN = 0x00000010L;
        private const long PRF_OWNED = 0x00000020L;
        #endregion WinAPI Consts

        private Control _parent = null;

        private Color _backColor  = Panel.DefaultBackColor;
        private byte  _backAlpha  = byte.MaxValue;
        private byte  _imageAlpha = byte.MaxValue;
        private Image _image      = null;

        private int _left = int.MinValue;
        private int _top  = int.MinValue;
        private int _wi   = int.MinValue;
        private int _he   = int.MinValue;        

        public AlphaBlendedPanel()
        {
            this.BackColor = _backColor;
            this.SetStyle(ControlStyles.UserPaint, false);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);            
        }

        #region protected functions

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        protected override void OnChangeUICues(UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
            this.Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (this.Parent != null && (this.Parent != _parent))
            {
                this.Parent.Resize += Parent_Resize;
                _parent = this.Parent;
            };

            if (m.Msg == WM_PAINT) UpdateStyles();
            //else if (m.Msg == WM_HSCROLL || m.Msg == WM_VSCROLL || m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_LBUTTONDBLCLK /* || m.Msg == win32.WM_MOUSELEAVE */)
            //    this.Invalidate();
            //else if (m.Msg == WM_MOUSEMOVE && m.WParam.ToInt32() != 0)
            //    this.Invalidate();                
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            if (this.Parent == null) return;

            if (_left != int.MinValue)
            {
                if (_left >= 0) this.Left = _left;
                else this.Left = Parent.Width + _left;
            };
            if (_top != int.MinValue)
            {
                if (_top >= 0) this.Top = _top;
                else this.Top = Parent.Height + _top;
            };
            if (_wi != int.MinValue)
            {
                if (_wi > 0) this.Width = _wi;
                else if (_wi < 0) this.Width = Parent.Width - (_left >= 0 ? _left : Parent.Width + _left) + _wi;
            };
            if (_he != int.MinValue)
            {
                if (_he > 0) this.Height = _he;
                else if (_he < 0) this.Height = Parent.Height - (_top >= 0 ? _top : Parent.Height + _top) + _he;
            };
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (Image != null)
            {
                if (this._imageAlpha == byte.MaxValue)
                    e.Graphics.DrawImage(this.Image, new Rectangle(0, 0, this.Image.Width, this.Image.Height), 0, 0, this.Image.Width, this.Image.Height, GraphicsUnit.Pixel);
                else
                    DrawAlphaImage(e.Graphics, this.Image, this.ImageOffset, this._imageAlpha);
            };
        }        

        protected void UpdateStyles()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.FromArgb(_backAlpha, _backColor);
        }

        #endregion protected functions

        #region Public functions

        public void DrawAlphaImage(Graphics g, Image im, Point offset, byte alpha)
        {
            g.DrawImage(im, new Rectangle(offset.X, offset.Y, im.Width, im.Height), 0, 0, im.Width, im.Height, GraphicsUnit.Pixel, SetImageOpacityAttribute(alpha));
        }

        public ImageAttributes SetImageOpacityAttribute(byte alpha)
        {
            float[][] ptsArray ={ new float[] {1, 0, 0, 0, 0}, new float[] {0, 1, 0, 0, 0}, new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, (float)(1f / 255f * alpha), 0}, new float[] {0, 0, 0, 0, 1}};
            ImageAttributes imgAttributes = new ImageAttributes();
            imgAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imgAttributes;
        }

        #endregion Public functions

        #region Parameters

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage { get; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageLayout BackgroundImageLayout { get; }

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new Color BackColor
        {
            get
            {
                return Color.FromArgb(base.BackColor.R, base.BackColor.G, base.BackColor.B);
            }
            set
            {
                _backColor = value;
                base.BackColor = value;
            }
        }

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public byte BackOpacity
        {
            get
            {
                return _backAlpha;
            }
            set
            {
                _backAlpha = value;
                Invalidate();
            }
        }        

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Image 
        { 
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public byte ImageOpacity
        {
            get
            {
                return _imageAlpha;
            }
            set
            {
                _imageAlpha = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int OffsetLeft
        {
            get
            {
                if (_left == int.MinValue) return this.Left;
                return _left;
            }
            set
            {
                _left = value;
                if (this.Parent == null) return;
                if (_left >= 0) this.Left = _left;
                else this.Left = Parent.Width + _left;
            }
        }

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int OffsetTop
        {
            get
            {
                if (_top == int.MinValue) return this.Top;
                return _top;
            }
            set
            {
                _top = value;
                if (this.Parent == null) return;
                if (_top >= 0) this.Top = _top;
                else this.Top = Parent.Height + _top;
            }
        }

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int OffsetWidth
        {
            get
            {
                if (_wi == int.MinValue) return this.Width;
                return _wi;
            }
            set
            {
                _wi = value;
                if (this.Parent == null) return;
                if (_wi > 0) this.Width = _wi;
                else if (_wi < 0) this.Width = Parent.Width - (_left >= 0 ? _left : Parent.Width + _left) + _wi;
            }
        }

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int OffsetHeight
        {
            get
            {
                if (_he == int.MinValue) return this.Height;
                return _he;
            }
            set
            {
                _he = value;
                if (this.Parent == null) return;
                if (_he > 0) this.Height = _he;
                else if (_he < 0) this.Height = Parent.Height - (_top >= 0 ? _top : Parent.Height + _top) + _he;
            }
        }

        [Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Point ImageOffset { get; set; } = new Point(0, 0);
        
        #endregion Parameters

    }
}
