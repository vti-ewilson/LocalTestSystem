using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components.Graphing
{
    /// <summary>
    /// Control used to display comments on a <see cref="DataPlotControl">DataPlotControl</see>
    /// </summary>
    public partial class CommentControl : UserControl
    {
        #region Fields (9)

        #region Private Fields (9)

        private Point _internalAnchor = new Point(2, 2);
        private Point _Offset = new Point(50, 50), _origOffset;
        private bool _tempVisible;
        private bool _userChanging;
        private bool _Visible = true;
        private Point OrigCursorPos;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentControl">CommentControl</see>
        /// </summary>
        public CommentControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            //Set style for double buffering
            //SetStyle(ControlStyles.DoubleBuffer |
            //         ControlStyles.AllPaintingInWmPaint |
            //         ControlStyles.UserPaint, true);

            //Set the default backcolor
            this.BackColor = Color.Transparent;
            this.textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            //this.textBox1.Focus();
        }

        #endregion Constructors

        #region Properties (9)

        /// <summary>
        /// Gets or sets a <see cref="System.Drawing.Point">System.Drawing.Point</see>
        /// that sets the callout anchor point of the control.
        /// </summary>
        public Point AnchorPoint
        {
            get { return _anchorPoint; }
            set
            {
                _anchorPoint = value;
                SetLocation(new Point(_anchorPoint.X - _internalAnchor.X, _anchorPoint.Y - _internalAnchor.Y));
                this.OnAnchorPointChanged();
            }
        }

        private void SetLocation(Point point)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<Point>(SetLocation), point);
            else
                base.Location = point;
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the comment control is visible.
        /// </summary>
        public Boolean CommentVisible
        {
            get { return _Visible; }
            set
            {
                SetCommentVisible(value);
            }
        }

        private void SetCommentVisible(bool value)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(SetCommentVisible), value);
            else
            {
                _Visible = value;
                textBox1.Visible = _Visible;
                lineControl1.Visible = _Visible;
                this.SetRegion(false);
            }
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the control relative
        /// to the upper-left corner of its container.
        /// </summary>
        public new Point Location
        {
            get { return base.Location; }
            set
            {
                base.Location = Location;
                this.SetControlSize();
                this._anchorPoint = new Point(this.Location.X + _internalAnchor.X, this.Location.Y + _internalAnchor.Y);
                this.pictureBoxCallout.Location = new Point(_internalAnchor.X - 2, _internalAnchor.Y - 2);
                this.OnAnchorPointChanged();
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="System.Drawing.Point">System.Drawing.Point</see>
        /// that sets the offset from the callout anchor point to the location of the textbox of the control.
        /// </summary>
        public Point Offset
        {
            get { return _Offset; }
            set
            {
                _Offset = value;
                this.SetInternalLocations();
                this.OnOffsetChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text in the comment control is read-only.
        /// </summary>
        public bool ReadOnly
        {
            get { return this.textBox1.ReadOnly; }
            set
            {
                SetReadOnly(value);
            }
        }

        private void SetReadOnly(bool value)
        {
            if (textBox1.InvokeRequired)
                textBox1.Invoke(new Action<bool>(SetReadOnly), value);
            else
                textBox1.ReadOnly = value;
        }

        /// <summary>
        /// Gets or sets the current text in the control.
        /// </summary>
        public override string Text
        {
            get { return textBox1.Text; }
            set { textBox1.SetText(value); }
        }

        /// <summary>
        /// Gets a value to indicate if the user is currently moving the comment
        /// </summary>
        public Boolean UserChanging
        {
            get { return this._userChanging; }
        }

        #endregion Properties

        #region Delegates and Events (4)

        #region Events (4)

        /// <summary>
        /// Occurs when the <see cref="AnchorPoint">AnchorPoint</see> changes
        /// </summary>
        public event EventHandler AnchorPointChanged;

        /// <summary>
        /// Occurs when the user clicks with the right mouse button on the callout
        /// </summary>
        public event EventHandler CalloutRightClicked;

        /// <summary>
        /// Occurs when the <see cref="Offset">Offset</see> changes
        /// </summary>
        public event EventHandler OffsetChanged;

        /// <summary>
        /// Occurs when the <see cref="Text">Text</see> changes
        /// </summary>
        [Browsable(true)]
        public new event EventHandler TextChanged;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (18)

        #region Public Methods (1)

        /// <summary>
        /// Sets input focus to the control
        /// </summary>
        /// <returns>true if the input focus request was successful; otherwise, false.</returns>
        public new bool Focus()
        {
            return this.textBox1.Focus();
        }

        #endregion Public Methods
        #region Protected Methods (4)

        /// <summary>
        /// Raises the <see cref="AnchorPointChanged">AnchorPointChanged</see> event
        /// </summary>
        protected virtual void OnAnchorPointChanged()
        {
            EventHandler handler = AnchorPointChanged;
            if (handler != null)
                handler(this, null);
        }

        /// <summary>
        /// Raises the <see cref="CalloutRightClicked">CalloutRightClicked</see> event
        /// </summary>
        protected virtual void OnCalloutRightClicked()
        {
            EventHandler handler = CalloutRightClicked;
            if (handler != null)
                handler(this, null);
        }

        /// <summary>
        /// Raises the <see cref="OffsetChanged">OffsetChanged</see> event
        /// </summary>
        protected virtual void OnOffsetChanged()
        {
            EventHandler handler = OffsetChanged;
            if (handler != null)
                handler(this, null);
        }

        /// <summary>
        /// Raises the <see cref="TextChanged">TextChanged</see> event
        /// </summary>
        protected virtual void OnTextChanged()
        {
            EventHandler handler = TextChanged;
            if (handler != null)
                handler(this, null);
        }

        #endregion Protected Methods
        #region Private Methods (13)

        private void pictureBoxCallout_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OrigCursorPos = Cursor.Position;
                _origAnchorPoint = _anchorPoint;
                _userChanging = true;
            }
        }

        private void pictureBoxCallout_MouseEnter(object sender, EventArgs e)
        {
            // If comment isn't visible when mouse enters callout, make it temporarily visible
            if (!_Visible)
            {
                textBox1.Visible = true;
                lineControl1.Visible = true;
                _tempVisible = true;
                this.SetRegion(false);

                this.BringToFront();
            }
        }

        private void pictureBoxCallout_MouseLeave(object sender, EventArgs e)
        {
            // If comment wasn't visible, hide it again when the mouse leaves
            if (!_Visible && _tempVisible)
            {
                textBox1.Visible = false;
                lineControl1.Visible = false;
                _tempVisible = false;
                this.SetRegion(false);
            }
        }

        private void pictureBoxCallout_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
                OrigCursorPos = Cursor.Position;
            else if (e.Button == MouseButtons.Left)
            {
                this.AnchorPoint = new Point(_origAnchorPoint.X + Cursor.Position.X - OrigCursorPos.X,
                    _origAnchorPoint.Y + Cursor.Position.Y - OrigCursorPos.Y);
            }
            this.Invalidate();

            //if (!_commentData.Visible)
        }

        private void pictureBoxCallout_MouseUp(object sender, MouseEventArgs e)
        {
            _userChanging = false;
            if (e.Button == MouseButtons.Right)
                OnCalloutRightClicked();
        }

        //private void CommentControl_Enter(object sender, EventArgs e)
        //{
        //    this.textBox1.Focus();
        //    this.textBox1.SelectionStart = this.textBox1.Text.Length;
        //}
        private void SetControlSize()
        {
            int prevWidth = this.Width, prevHeight = this.Height;
            if (Math.Abs(_Offset.X) + 2 - textBox1.Width / 2 > 0)
                this.Width = textBox1.Width + Math.Abs(_Offset.X) + 3 - textBox1.Width / 2;
            else
                this.Width = textBox1.Width;
            if (Math.Abs(_Offset.Y) + 2 - textBox1.Height / 2 > 0)
                this.Height = textBox1.Height + Math.Abs(_Offset.Y) + 3 - textBox1.Height / 2;
            else
                this.Height = textBox1.Height;
        }

        private void SetInternalLocations()
        {
            int newIntAnchorX, newIntAnchorY, newLocationX, newLocationY;
            int newBoxX, newBoxY;

            this.SetControlSize();
            if (_Offset.X + 2 - textBox1.Width / 2 > 0)
            {
                newIntAnchorX = 2;
                newBoxX = _Offset.X + 2 - textBox1.Width / 2;
            }
            else
            {
                newIntAnchorX = textBox1.Width / 2 - _Offset.X;
                newBoxX = 0;
            }
            if (_Offset.Y + 2 - textBox1.Height / 2 > 0)
            {
                newIntAnchorY = 2;
                newBoxY = _Offset.Y + 2 - textBox1.Height / 2;
            }
            else
            {
                newIntAnchorY = textBox1.Height / 2 - _Offset.Y;
                newBoxY = 0;
            }
            newLocationX = _anchorPoint.X - newIntAnchorX;
            newLocationY = _anchorPoint.Y - newIntAnchorY;
            base.Location = new Point(newLocationX, newLocationY);
            _internalAnchor = new Point(newIntAnchorX, newIntAnchorY);
            this.pictureBoxCallout.Location = new Point(_internalAnchor.X - 2, _internalAnchor.Y - 2);
            this.lineControl1.StartPoint = _internalAnchor;
            this.lineControl1.EndPoint = new Point(_internalAnchor.X + _Offset.X, _internalAnchor.Y + _Offset.Y);
            this.textBox1.Location = new Point(newBoxX, newBoxY);

            if (this.textBox1.Cursor == Cursors.IBeam)
                this.SetRegion(false);
            else
                this.SetRegion(true);
        }

        private void SetRegion(Boolean FullRegion)
        {
            if (FullRegion)
            {
                this.Region = new Region(new Rectangle(0, 0, this.Width, this.Height));
            }
            else
            {
                GraphicsPath path = new GraphicsPath();
                if (_Visible || _tempVisible)
                {
                    path.AddLine(_internalAnchor, new Point(_internalAnchor.X + _Offset.X, _internalAnchor.Y + _Offset.Y));
                    path.CloseFigure();
                    path.Widen(new Pen(Brushes.Black, 2));
                }
                Point[] points = new Point[5];
                points[0] = new Point(_internalAnchor.X - 3, _internalAnchor.Y);
                points[1] = new Point(_internalAnchor.X, _internalAnchor.Y - 3);
                points[2] = new Point(_internalAnchor.X + 3, _internalAnchor.Y);
                points[3] = new Point(_internalAnchor.X, _internalAnchor.Y + 3);
                points[4] = new Point(_internalAnchor.X - 3, _internalAnchor.Y);
                path.AddPolygon(points);
                if (_Visible || _tempVisible)
                    path.AddRectangle(new Rectangle(textBox1.Left, textBox1.Top, textBox1.Width, textBox1.Height));
                this.Region = new Region(path);
            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) &&
                ((e.X < 5) || (e.X > textBox1.Width - 10) || (e.Y < 5) || (e.Y > textBox1.Height - 10)))
            {
                _origOffset = _Offset;
                _userChanging = true;
            }
        }

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X == lastX && e.Y == lastY) return;
            if (e.Button == MouseButtons.None)
            {
                if ((e.X < 5) || (e.X > textBox1.Width - 10) || (e.Y < 5) || (e.Y > textBox1.Height - 10))
                {
                    textBox1.Cursor = Cursors.SizeAll;
                    OrigCursorPos = Cursor.Position;
                }
                else
                    textBox1.Cursor = Cursors.IBeam;
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (textBox1.Cursor == Cursors.SizeAll)
                {
                    Point newOffset = new Point(_origOffset.X + Cursor.Position.X - OrigCursorPos.X,
                        _origOffset.Y + Cursor.Position.Y - OrigCursorPos.Y);
                    if (this.Offset != newOffset)
                    {
                        this.Offset = newOffset;
                        this.SetRegion(true);
                        this.Refresh();
                    }
                    else
                    {
                        this.SetRegion(false);
                    }
                }
            }
            lastX = e.X;
            lastY = e.Y;
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.Cursor = Cursors.IBeam;
            _userChanging = false;
            this.SetInternalLocations();
        }

        private void textBox1_SizeChanged(object sender, EventArgs e)
        {
            this.textBox1.Cursor = Cursors.IBeam;
            this.SetInternalLocations();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.OnTextChanged();
        }

        #endregion Private Methods

        #endregion Methods

        private Point _anchorPoint, _origAnchorPoint;
        private int lastX, lastY;
    }
}