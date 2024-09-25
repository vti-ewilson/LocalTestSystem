using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Line control created to draw the callout line in the
    /// <see cref="Graphing.CommentControl">CommentControl</see>
    /// </summary>
    /// <remarks>
    /// Given two points, a <see cref="StartPoint">StartPoint</see>
    /// and an <see cref="EndPoint">EndPoint</see>, the control
    /// calculates its own <see cref="Location">Location</see>
    /// and <see cref="Size">Size</see> such that the control
    /// will contain the line determined by the start and end points.
    /// The control will then draw the line and set its own
    /// region such that the background will be transparent.
    /// </remarks>
    public partial class LineControl : Control
    {
        #region Globals

        private Point _startPoint = new Point(0, 0);
        private Point _endPoint = new Point(0, 0);
        private Point _internalStartPoint = new Point(0, 0), _internalEndPoint = new Point(0, 0);
        private Pen p = new Pen(Color.Black);
        private Pen p2 = new Pen(Brushes.Black, 2);
        private Pen _linePen = new Pen(Color.Black);

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LineControl">LineControl</see> class
        /// </summary>
        public LineControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            //Set style for double buffering
            SetStyle(ControlStyles.DoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            //Set the default backcolor
            this.BackColor = Color.Transparent;
        }

        #endregion Construction

        #region Public Properties

        /// <summary>
        /// Start point of the line that will be contained by the control
        /// </summary>
        public Point StartPoint
        {
            get { return _startPoint; }
            set
            {
                _startPoint = value;
                base.Location = new Point(Math.Min(_startPoint.X, _endPoint.X), Math.Min(_startPoint.Y, _endPoint.Y));
                this.Size = new Size(Math.Max(_startPoint.X, _endPoint.X) - Math.Min(_startPoint.X, _endPoint.X) + 1,
                    Math.Max(_startPoint.Y, _endPoint.Y) - Math.Min(_startPoint.Y, _endPoint.Y) + 1);
                _internalStartPoint = new Point(_startPoint.X - this.Location.X, _startPoint.Y - this.Location.Y);
                _internalEndPoint = new Point(_endPoint.X - this.Location.X, _endPoint.Y - this.Location.Y);
            }
        }

        /// <summary>
        /// End point of the line that will be contained by the control
        /// </summary>
        public Point EndPoint
        {
            get { return _endPoint; }
            set
            {
                _endPoint = value;
                base.Location = new Point(Math.Min(_startPoint.X, _endPoint.X), Math.Min(_startPoint.Y, _endPoint.Y));
                this.Size = new Size(Math.Max(_startPoint.X, _endPoint.X) - Math.Min(_startPoint.X, _endPoint.X) + 1,
                    Math.Max(_startPoint.Y, _endPoint.Y) - Math.Min(_startPoint.Y, _endPoint.Y) + 1);
                _internalStartPoint = new Point(_startPoint.X - this.Location.X, _startPoint.Y - this.Location.Y);
                _internalEndPoint = new Point(_endPoint.X - this.Location.X, _endPoint.Y - this.Location.Y);
            }
        }

        /// <summary>
        /// Pen to use to draw the line
        /// </summary>
        public Pen LinePen
        {
            get { return _linePen; }
            set { _linePen = value; }
        }

        /// <summary>
        /// Location of the line control
        /// </summary>
        /// <remarks>
        /// The line control will automatically recalculate its own location
        /// (thus moving the control) when either the
        /// <see cref="StartPoint">StartPoint</see> or the
        /// <see cref="EndPoint">EndPoint</see> are changed.
        /// </remarks>
        public new Point Location
        {
            get { return base.Location; }
            set
            {
                base.Location = Location;
                _startPoint = new Point(_startPoint.X - Math.Min(_startPoint.X, _endPoint.X) + Location.X,
                    _startPoint.Y - Math.Min(_startPoint.Y, _endPoint.Y) + Location.Y);
                _endPoint = new Point(_endPoint.X - Math.Min(_startPoint.X, _endPoint.X) + Location.X,
                    _endPoint.Y - Math.Min(_startPoint.Y, _endPoint.Y) + Location.Y);
                _internalStartPoint = new Point(_startPoint.X - this.Location.X, _startPoint.Y - this.Location.Y);
                _internalEndPoint = new Point(_endPoint.X - this.Location.X, _endPoint.Y - this.Location.Y);
            }
        }

        #endregion Public Properties

        #region Events

        /// <summary>
        /// Raises the OnPaint event.
        /// </summary>
        /// <param name="pe">Event arguments</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Calling the base class OnPaint
            base.OnPaint(pe);

            // Create Region, allowing rest of control to be transparent
            GraphicsPath path = new GraphicsPath();
            path.AddLine(_internalStartPoint, _internalEndPoint);
            path.CloseFigure();
            path.Widen(p2);
            this.Region = new Region(path);
            // Draw the line
            pe.Graphics.DrawLine(p, _internalStartPoint, _internalEndPoint);
        }

        #endregion Events
    }
}