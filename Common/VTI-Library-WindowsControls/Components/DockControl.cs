//===============================================================================
// DockControl.cs
// This file contains the definition for the DockControl and supporting classes.
//===============================================================================
// Copyright (C) 2003 Todd Andrews for BitStorm Co.
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
//
//
//===============================================================================
//
// Special thanks to Phil Wright for his original dock.cs docking control, without 
// which this would not be possible (or at least would have been a whole lot more 
// difficult!)
//
//===============================================================================

using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel.Design;

namespace VTIWindowsControlLibrary.Components
{
	/// <summary>
	/// DockControl is a component for making docking and sizing
	/// windows and controls
	/// </summary>
	
	// give the control a designer category, to ensure design-time compatability
	[DesignerCategoryAttribute("Component")]
	// makse sure we can add child controls at runtime
	// see Microsoft Knowledge Base Article - 813450
	[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))] 
	[ToolboxBitmap(typeof(DockingControl), "Images.Folder_Locked.ico")]
	public class DockingControl : UserControl 
	{
		#region Instance variables
		
		private const int _dockTolerance = 10;	// how close to the edge of the parent form 
												// when dragging before docking takes place
		private DockingResize _resize;			// Provide resizing functionality
		private DockingHandle _handle;			// Handle for grabbing and moving
		private MiniFrameWnd _miniFrame;		// frame wnd for detached control
		private DockHelper _dockHelper;			// utility class for drawing docking
												// rectangles & behavior
		private bool _isDocked = true;			// are we docked or floating?
		private string _caption;				// Caption for floating frame

		#endregion

		#region Static Member Variables
		
		// Static variables defining colours for drawing
		private static Pen _lightPen = new Pen(Color.FromKnownColor(KnownColor.ControlLightLight));
		private static Pen _darkPen = new Pen(Color.FromKnownColor(KnownColor.ControlDark));
		private static Brush _plainBrush = Brushes.LightGray;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem bringToFrontToolStripMenuItem;
        private ToolStripMenuItem sendToBackToolStripMenuItem;
        private IContainer components;

		// Static properties for read-only access to drawing colours
		public static Pen LightPen		{ get { return _lightPen;	} }
		public static Pen DarkPen		{ get { return _darkPen;	} }
		public static Brush PlainBrush	{ get { return _plainBrush; } }

		#endregion


        #region Constructors/Destructors

        public DockingControl()
		{
			/// <summary>
			/// Required for Windows.Forms Class Composition Designer support
			/// </summary>
			InitializeComponent();
			Init();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		protected void Init()
		{
			// Create the resizing bar, gripper handle and border control
			_miniFrame = new MiniFrameWnd();
			_resize = new DockingResize(DockStyle.Left);
			_handle = new DockingHandle(this, DockStyle.Left);
            _handle.ContextMenuStrip = contextMenuStrip1;
            
			// Define our own initial docking position for when we are added to host form
			Dock = DockStyle.Left;

			_dockHelper = new DockHelper(this, this.Parent);
            
			// NOTE: Order of array contents is important
			// Controls in the array are positioned from right to left when the 
			// form makes size/position calculations for docking controls
			Controls.AddRange(new Control[]{_handle, _resize});
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bringToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bringToFrontToolStripMenuItem,
            this.sendToBackToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            // 
            // bringToFrontToolStripMenuItem
            // 
            this.bringToFrontToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.BringToFront;
            this.bringToFrontToolStripMenuItem.Name = "bringToFrontToolStripMenuItem";
            this.bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.bringToFrontToolStripMenuItem.Text = "Bring to Front";
            this.bringToFrontToolStripMenuItem.Click += new System.EventHandler(this.bringToFrontToolStripMenuItem_Click);
            // 
            // sendToBackToolStripMenuItem
            // 
            this.sendToBackToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.SendToBack;
            this.sendToBackToolStripMenuItem.Name = "sendToBackToolStripMenuItem";
            this.sendToBackToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sendToBackToolStripMenuItem.Text = "Send to Back";
            this.sendToBackToolStripMenuItem.Click += new System.EventHandler(this.sendToBackToolStripMenuItem_Click);
            // 
            // DockingControl
            // 
            this.Name = "DockingControl";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		[Browsable(false)]
		public Control HostForm { get { return this.Parent; } }

		[Browsable(true), Category("Appearance")]
		public string Caption
		{
			get { return _caption; }
			set { _caption = value; }
		}

		[Browsable(false)]
		public bool IsDocked
		{
			get {return _isDocked; }
		}

        [Browsable(false)]
        internal DockHelper DockHelper
        {
            get { return _dockHelper; }
        }
		#endregion

		#region Public Methods
		#endregion

		#region Docking functions

		/// <summary>
		/// this is called implicitly by the Drag() function to set up the 
		/// docking control to be dragged to another location in the parent.
		/// </summary>
		public void BeginDrag(int X, int Y)
		{
			_dockHelper.StartDrag(new Point(X, Y));
		}

		/// <summary>
		/// Called by the mousemove of the drag handle to draw the 
		/// coontrol's frame as it moves
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		public void Drag(int X, int Y)
		{
			// erase the last drawn frame
			if (!_dockHelper.IsDragging)
			{
				BeginDrag(X, Y);
			}
			else
			{
				_dockHelper.Draw(X, Y);
			}
		}

		/// <summary>
		/// Finishes drag operation and cleans up
		/// </summary>
		public void EndDrag()
		{
			DockStyle style;
			style = _dockHelper.FinishDrag();

			if (style == DockStyle.None)
			{
				this.UnDock();
			}
			else
			{
				this.Dock = style;
			}
		}

		/// <summary>
		/// Moves the control to the floating frame
		/// </summary>
		public void UnDock()
		{
            //this.SuspendLayout();

            if (_isDocked)
            {
                // remember the last size and settings
                _dockHelper.LastDockStyle = this.Dock;
                this._resize.Visible = false;

                _dockHelper.DockParent = this.Parent;

                // remove from parent control array
                Parent.Controls.Remove(this);
            }

			// tell the frame about its size (calc'ed by the dockhelper)
			_miniFrame.Bounds = _dockHelper.FrameRectangle;
            
            if (_isDocked)
            {
                // reparent this control to the frame
                _miniFrame.Controls.Add(this);

                // show the floating frame
                _miniFrame.Show();

                // make this control fill the miniframewnd
                this.Dock = DockStyle.Fill;

                //_miniFrame.Text = this.Caption;
                //_miniFrame.ShowInTaskbar = true;
                //_miniFrame.TopMost = true;

            }

			// this only works b/c it is being called AFTER the dock property is being set.
			// the code sets _docked to true in the dock property (as the default, and 
			// unsets it (set it to false) here because this is the only place we KNOW
			// that it is being undocked.
			_isDocked = false;

            //this.ResumeLayout();
            this.Refresh();
        }
		
		/// <summary>
		/// Places the control back into the parent frame at the last saved dock position
		/// </summary>
		public void ReDock()
		{
			this.Dock = _dockHelper.LastDockStyle;
		}

        public void Maximize()
        {
            if (!_isDocked) _miniFrame.WindowState = FormWindowState.Maximized;
        }

        public void Restore()
        {
            if (!_isDocked) _miniFrame.WindowState = FormWindowState.Normal;
        }

		#endregion

		#region Overrides

		/// <summary>
		/// Overrides the base class property to allow extra work
		/// </summary>
		public override DockStyle Dock
		{
			get { return base.Dock; }

			set
            {
                // Our size before docking position is changed
                Size size = this.ClientSize;

				if (false == _isDocked)
				{
					// we are being re-docked.  Handle getting rid of the floating frame
					// and restoring our controls
					_miniFrame.Controls.Remove(this);
					_miniFrame.Hide();
					this._resize.Visible = true;
					_dockHelper.DockParent.Controls.Add(this);
				}

	
				// Remember the current docking position
				DockStyle dsOldResize = _resize.Dock;

				// New handle size is dependant on the orientation of the new docking position
				_handle.SizeToOrientation(value);

				// Modify docking position of child controls based on our new docking position
				_resize.Dock = DockingResize.ResizeStyleFromControlStyle(value);
				_handle.Dock = DockingHandle.HandleStyleFromControlStyle(value);
                _resize.SendToBack();
                _handle.SendToBack();

				// Now safe to update ourself through base class
				base.Dock = value;

				// Change in orientation occured?
				if (dsOldResize != _resize.Dock)
				{
					// Must update our client size to ensure the correct size is used when
					// the docking position changes.  We have to transfer the value that determines
					// the vector of the control to the opposite dimension
					if (!((this.Dock == DockStyle.Top) || 
						(this.Dock == DockStyle.Bottom)))
						//size.Height = size.Width;
					//else
						size.Width = size.Height;

					this.ClientSize = size;
				}

				_isDocked = true;
			}
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Repaint of the our controls 
            _handle.Invalidate();
            _resize.Invalidate();
        }
		#endregion

        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SendToBack();
        }
	}

	/// <summary>
	/// DockHelper encapsulates functionality for the docking control and 
	/// miniframewnd to help them dock to a form
	/// </summary>
	#region class DockHelper
	internal class DockHelper
	{
		#region Member Variables

		private Control _control = null;
		private Control _dockParent = null;
		private DockStyle _oldDockStyle = DockStyle.None;	// the previous dockstyle of an undocked
															// toolbar
		private Rectangle _floatLocation = Rectangle.Empty; // the rectangle in screen 
															// coordinates of an undocked toolar
		private Size _dockSize = Size.Empty;
		private Point _lastDragPoint = Point.Empty;			// last point the parent was dragged to
		private bool _dragging = false;						// Are we moving?

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a DockHelper object
		/// </summary>
		/// <param name="Control"></param>
		public DockHelper(System.Windows.Forms.Control Control)
		{
			ctor(Control, Control.Parent);
		}

		/// <summary>
		/// Creates a DockHelper object
		/// </summary>
		/// <param name="Control"></param>
		public DockHelper(System.Windows.Forms.Control Control, System.Windows.Forms.Control DockParent)
		{
			ctor(Control, DockParent);			
		}

		/// <summary>
		/// Common constructor code
		/// </summary>
		/// <param name="Control"></param>
		/// <param name="DockParent"></param>
		private void ctor(System.Windows.Forms.Control Control, System.Windows.Forms.Control DockParent)
		{
			_control = Control;
			_dockParent = Control.Parent;
			_floatLocation = new Rectangle(0, 0, 400, 400);
			_dockSize = new Size(100, 100); // default dock size
		}

		#endregion

		#region Properties
		
		/// <summary>
		/// The parent control the dockbar is associated with 
		/// </summary>
		public Control DockParent
		{
			get { return _dockParent; }
			set { _dockParent = value; }
		}

		/// <summary>
		/// Read-only, true if the DockHelper is currently drawing drag rectangles
		/// </summary>
		public bool IsDragging
		{
			get { return _dragging; }
		}
		
		/// <summary>
		/// Stores the size for a floating frame
		/// </summary>
		public Rectangle FrameRectangle
		{
			get { return _floatLocation; }
			set { _floatLocation = value;
            _dockSize = new Size(_floatLocation.Height, _floatLocation.Height);
            }
		}

		/// <summary>
		/// Stores the size for a docked frame
		/// </summary>
		public Size DockSize
		{
			get { return _dockSize; }
			set { _dockSize = value; }
		}
		
		/// <summary>
		/// Stores the last dockstyle if the owner is floated
		/// </summary>
		public DockStyle LastDockStyle
		{
			get { return _oldDockStyle; }
			set { _oldDockStyle = value; }
		}

		#endregion

		#region Public Methods
		
		/// <summary>
		/// Initializes the members and starts drawing drag rectangle
		/// </summary>
		/// <param name="startingPoint"></param>
		public void StartDrag(Point startingPoint)
		{
			StartDrag(startingPoint.X, startingPoint.Y);
		}
		/// <summary>
		/// Initializes the members and starts drawing drag rectangle
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		public void StartDrag(int X, int Y)
		{
			_dragging = true;
			_lastDragPoint = new Point(X, Y);
			DrawReversibleFrame(X, Y);
		}

		/// <summary>
		/// Draws the drag rect, erasing any old rect first
		/// </summary>
		/// <param name="TopLeft"></param>
		public void Draw(Point TopLeft)
		{
			Draw(TopLeft.X, TopLeft.Y);
		}

		/// <summary>
		/// Draws the drag rect, erasing any old rect first
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		public void Draw(int X, int Y)
		{
			DrawReversibleFrame(X, Y);
			DrawReversibleFrame(_lastDragPoint.X, _lastDragPoint.Y);

			_lastDragPoint.X = X;
			_lastDragPoint.Y = Y;
		}

		/// <summary>
		/// Finishes the dragging and cleans up
		/// </summary>
		/// <returns></returns>
		public DockStyle FinishDrag()
		{
			//erase final frame
			DockStyle style = DockStyle.None;
            
			Rectangle finalRect = DrawReversibleFrame(_lastDragPoint.X, _lastDragPoint.Y, ref style);
            if (style == DockStyle.None)
            {
                _floatLocation = finalRect;
                _dockSize = new Size(_floatLocation.Height, _floatLocation.Height);
            }

			_lastDragPoint = Point.Empty;

			_dragging = false;

			return style;
		}

		#endregion

		#region DrawReversibleFrame

		/// <summary>
		/// Determines how to draw the drag frame rect based on how close
		/// we are to which client edge of the parent form
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		/// <returns>Rectangle</returns>
		private Rectangle DrawReversibleFrame(int X, int Y)
		{
			DockStyle ds = DockStyle.None;
			return DrawReversibleFrame(X, Y, ref ds);
		}

		/// <summary>
		/// Determines how to draw the drag frame rect based on how close
		/// we are to which client edge of the parent form
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		/// <param name="newStyle"></param>
		/// <returns>Rectangle</returns>
		private Rectangle DrawReversibleFrame(int X, int Y, ref DockStyle newStyle)
		{
			// can't have a null dockparent
			if (null == _dockParent)
				_dockParent = _control.Parent;

			Rectangle rect = Rectangle.Empty;
			Rectangle parent_client;

			//_control.Parent.
			Point client_point = _dockParent.PointToClient(new Point(X, Y));
			parent_client = _dockParent.ClientRectangle;
			
			if ((-10 < client_point.X) && (client_point.X < 10))
			{
				// draw docked left
				rect = new Rectangle(0, 0, _dockSize.Width, parent_client.Height);
				rect = _dockParent.RectangleToScreen(rect);
				newStyle = DockStyle.Left;
			}
			else if (((parent_client.Width) - 10 < client_point.X) && (client_point.X < (parent_client.Width)+ 10))
			{
				// draw docked right
				rect = new Rectangle(parent_client.Width - _dockSize.Width, 0, _dockSize.Width, parent_client.Height);
				rect = _dockParent.RectangleToScreen(rect);
				newStyle = DockStyle.Right;
			}
			else if ((-10 < client_point.Y) && (client_point.Y < 10))
			{
				// draw docked top
				rect = new Rectangle(0, 0, parent_client.Width, _dockSize.Height);
				rect = _dockParent.RectangleToScreen(rect);
				newStyle = DockStyle.Top;
			}
			else if (((parent_client.Height) - 10 < client_point.Y) && (client_point.Y < (parent_client.Height)+ 10))
			{
				// draw docked bottom
				rect = new Rectangle(0, parent_client.Height - _dockSize.Height, parent_client.Width, _dockSize.Height);
				rect = _dockParent.RectangleToScreen(rect);
				newStyle = DockStyle.Bottom;
			}
			else 
			{
				// draw free floating
				rect.Location = new Point(X, Y);
				rect.Size = _floatLocation.Size;
	
				newStyle = DockStyle.None;
			}

			ControlPaint.DrawReversibleFrame(rect, SystemColors.Control, FrameStyle.Thick);

			return rect;
		}

		#endregion
	}

	#endregion

	/// <summary>
	/// The DockingResize class provides additional functionality for the 
	/// DockingControl component
	/// </summary>
	#region class DockingResize
	internal class DockingResize : UserControl
	{
		// Class constants
		private const int _fixedLength = 4;

		// Instance variables
		private Point _pointStart;
		private Point _pointLast;
		private Size _size;

		public DockingResize(DockStyle ds)
		{
			this.Dock = ResizeStyleFromControlStyle(ds);
			this.Size = new Size(_fixedLength, _fixedLength);
		}	

		#region Static Methods

		public static DockStyle ResizeStyleFromControlStyle(DockStyle ds)
		{
			switch(ds)
			{
				case DockStyle.Left:
					return DockStyle.Right;
				case DockStyle.Top:
					return DockStyle.Bottom;
				case DockStyle.Right:
					return DockStyle.Left;
				case DockStyle.Bottom:
					return DockStyle.Top;
				case DockStyle.Fill:
					return DockStyle.None;
				default:
					// Should never happen!
					throw new ApplicationException("Invalid DockStyle argument");
			}
		}

		#endregion

		#region Overrides

		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Remember the mouse position and client size when capture occured
			_pointStart = _pointLast = PointToScreen(new Point(e.X, e.Y));
			_size = Parent.ClientSize;

			// Ensure delegates are called
			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Cursor depends on if we a vertical or horizontal resize
			if ((this.Dock == DockStyle.Top) || 
				(this.Dock == DockStyle.Bottom))
				this.Cursor = Cursors.HSplit;
			else
				this.Cursor = Cursors.VSplit;

			// Can only resize if we have captured the mouse
			if (this.Capture)
			{
				// Find the new mouse position
				Point point = PointToScreen(new Point(e.X, e.Y));

				// Have we actually moved the mouse?
				if (point != _pointLast)
				{
					// Update the last processed mouse position
					_pointLast = point;

					// Find delta from original position
					int xDelta = _pointLast.X - _pointStart.X;
					int yDelta = _pointLast.Y - _pointStart.Y;

					// Resizing from bottom or right of form means inverse movements
					if ((this.Dock == DockStyle.Top) || 
						(this.Dock == DockStyle.Left))
					{
						xDelta = -xDelta;
						yDelta = -yDelta;
					}

					// New size is original size plus delta
					if ((this.Dock == DockStyle.Top) || 
						(this.Dock == DockStyle.Bottom))
						Parent.ClientSize = new Size(_size.Width, _size.Height + yDelta);
					else
						Parent.ClientSize = new Size(_size.Width + xDelta, _size.Height);

					// Force a repaint of parent so we can see changed appearance
					Parent.Refresh();
				}
			}

			// Ensure delegates are called
			base.OnMouseMove(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			// Create objects used for drawing
			Point[] ptLight = new Point[2];
			Point[] ptDark = new Point[2];
			Rectangle rectMiddle = new Rectangle();

			// Drawing is relative to client area
			Size sizeClient = this.ClientSize;

			// Painting depends on orientation
			if ((this.Dock == DockStyle.Top) || 
				(this.Dock == DockStyle.Bottom))
			{
				// Draw as a horizontal bar
				ptDark[1].Y = ptDark[0].Y = sizeClient.Height - 1;
				ptLight[1].X = ptDark[1].X = sizeClient.Width;
				rectMiddle.Width = sizeClient.Width;
				rectMiddle.Height = sizeClient.Height - 2;
				rectMiddle.X = 0;
				rectMiddle.Y = 1;
			}
			else if ((this.Dock == DockStyle.Left) || 
				(this.Dock == DockStyle.Right))
			{
				// Draw as a vertical bar
				ptDark[1].X = ptDark[0].X = sizeClient.Width - 1;
				ptLight[1].Y = ptDark[1].Y = sizeClient.Height;
				rectMiddle.Width = sizeClient.Width - 2;
				rectMiddle.Height = sizeClient.Height;
				rectMiddle.X = 1;
				rectMiddle.Y = 0;
			}

			// Use colours defined by docking control that is using us
			pe.Graphics.DrawLine(DockingControl.LightPen, ptLight[0], ptLight[1]);
			pe.Graphics.DrawLine(DockingControl.DarkPen, ptDark[0], ptDark[1]);
			pe.Graphics.FillRectangle(DockingControl.PlainBrush, rectMiddle);

			// Ensure delegates are called
			base.OnPaint(pe);
		}
	}

	#endregion

	#endregion

	/// <summary>
	/// The DockingHandle is what the user grabs to move and resize the 
	/// Docking control
	/// </summary>
	#region class DockingHandle
	internal class DockingHandle : UserControl 
	{
		// Class constants
		private const int _fixedLength = 14;
		private const int _offset = 3;
		private const int _inset = 3;

		private bool _dragging = false;
		// Instance variables
		private DockingControl _dockingControl = null;
		private Font _captionFont = null;

        private Point _dragOffset;

        private PictureBox _MaximizeButton;
        private PictureBox _DockButton;

        private ToolTip _ToolTip;
        private Boolean _isMaximized;

		public DockingHandle(DockingControl dockingControl, DockStyle ds)
		{
			_dockingControl = dockingControl;
			// creating a new font is an expensive operation.  Do it once here to use in the
			// DrawFrameTitle routine.
			_captionFont = new System.Drawing.Font(Form.DefaultFont.FontFamily.ToString(), 8);

            _ToolTip = new ToolTip();


            _DockButton = new PictureBox();
            _DockButton.Size = new Size(16, 16);
            _DockButton.Image = Properties.Resources.minimize_small;
            _DockButton.Dock = DockStyle.Right;
            _DockButton.Visible = false;
            _DockButton.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.ActiveCaption);
            _DockButton.Padding = new Padding(1, 1, 1, 1);
            //_DockButton.MouseEnter += delegate { _DockButton.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.Control); };
            //_DockButton.MouseLeave += delegate { _DockButton.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.ActiveCaption); };
            _DockButton.Click += delegate 
            { 
                _dockingControl.ReDock();
                _DockButton.Visible = false;
                _MaximizeButton.Visible = false;
            };
            this.Controls.Add(_DockButton);
            _ToolTip.SetToolTip(_DockButton, "Dock");

            _MaximizeButton = new PictureBox();
            _MaximizeButton.Size = new Size(16, 16);
            _MaximizeButton.Image = Properties.Resources.maximize_small;
            _MaximizeButton.Dock = DockStyle.Right;
            _MaximizeButton.Visible = false;
            _MaximizeButton.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.ActiveCaption);
            _MaximizeButton.Padding = new Padding(1, 1, 1, 1);
            //_MaximizeButton.MouseEnter += delegate { _MaximizeButton.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.Control); };
            //_MaximizeButton.MouseLeave += delegate { _MaximizeButton.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.ActiveCaption); };
            _MaximizeButton.Click += delegate
            {
                if (_isMaximized)
                {
                    _dockingControl.Restore();
                    _isMaximized = false;
                    _MaximizeButton.Image = Properties.Resources.maximize_small;
                }
                else
                {
                    _dockingControl.Maximize();
                    _isMaximized = true;
                    _MaximizeButton.Image = Properties.Resources.restore_small;
                }
            };
            this.Controls.Add(_MaximizeButton);
            _ToolTip.SetToolTip(_MaximizeButton, "Maximize");

            this.Dock = HandleStyleFromControlStyle(ds);
			SizeToOrientation(ds);
		}	

		#region Static Methods

		public static DockStyle HandleStyleFromControlStyle(DockStyle ds)
		{
			switch(ds)
			{
				case DockStyle.Left:
					return DockStyle.Top;
				case DockStyle.Top:
					return DockStyle.Left;
				case DockStyle.Right:
					return DockStyle.Top;
				case DockStyle.Bottom:
					return DockStyle.Left;
				case DockStyle.Fill:
					return DockStyle.Top;
				default:
					// Should never happen!
					throw new ApplicationException("Invalid DockStyle argument");
			}
		}

		#endregion

		#region Methods

		public void SizeToOrientation(DockStyle ds)
		{
			if ((ds == DockStyle.Top) || (ds == DockStyle.Bottom))
				this.ClientSize = new Size(_fixedLength, 0);
			else
				this.ClientSize = new Size(0, _fixedLength);
		}

		protected void DrawFrameTitle(PaintEventArgs pe)
		{
			ControlPaint.DrawBorder3D(pe.Graphics, _dockingControl.ClientRectangle, Border3DStyle.Etched, Border3DSide.All);

			if (_dockingControl.ContainsFocus)
			{
				string str = this._dockingControl.Caption;

				pe.Graphics.FillRectangle(System.Drawing.SystemBrushes.ActiveCaption, this.ClientRectangle);
				SizeF sf = pe.Graphics.MeasureString(str, _captionFont, this.Width, System.Drawing.StringFormat.GenericDefault); 
				pe.Graphics.DrawString(str, _captionFont, new SolidBrush(System.Drawing.SystemColors.ActiveCaptionText), new RectangleF(0,0,sf.Width, sf.Height), System.Drawing.StringFormat.GenericDefault); 
			}
			else
			{
				pe.Graphics.FillRectangle(System.Drawing.SystemBrushes.InactiveCaption, this.ClientRectangle);

			}
		}

		protected void DrawGrabHandle(PaintEventArgs pe)
		{
			Size sizeClient = this.ClientSize;
			Point[] ptLight = new Point[4];
			Point[] ptDark = new Point[4];

            // Depends on orientation
			if ((_dockingControl.Dock == DockStyle.Top) || 
				(_dockingControl.Dock == DockStyle.Bottom))
			{
				int iBottom = sizeClient.Height - _inset - 1;
				int iRight = _offset + 2;

				ptLight[3].X = ptLight[2].X = ptLight[0].X = _offset;
				ptLight[2].Y = ptLight[1].Y = ptLight[0].Y = _inset;
				ptLight[1].X = _offset + 1;
				ptLight[3].Y = iBottom;
		
				ptDark[2].X = ptDark[1].X = ptDark[0].X = iRight;
				ptDark[3].Y = ptDark[2].Y = ptDark[1].Y = iBottom;
				ptDark[0].Y = _inset;
				ptDark[3].X = iRight - 1;
			}
			else
			{
				int iBottom = _offset + 2;
				int iRight = sizeClient.Width - _inset - 1;
			
				ptLight[3].X = ptLight[2].X = ptLight[0].X = _inset;
				ptLight[1].Y = ptLight[2].Y = ptLight[0].Y = _offset;
				ptLight[1].X = iRight;
				ptLight[3].Y = _offset + 1;
		
				ptDark[2].X = ptDark[1].X = ptDark[0].X = iRight;
				ptDark[3].Y = ptDark[2].Y = ptDark[1].Y = iBottom;
				ptDark[0].Y = _offset;
				ptDark[3].X = _inset;
			}
	
			Pen lightPen = DockingControl.LightPen;
			Pen darkPen = DockingControl.DarkPen;

			pe.Graphics.DrawLine(lightPen, ptLight[0], ptLight[1]);
			pe.Graphics.DrawLine(lightPen, ptLight[2], ptLight[3]);
			pe.Graphics.DrawLine(darkPen, ptDark[0], ptDark[1]);
			pe.Graphics.DrawLine(darkPen, ptDark[2], ptDark[3]);

			// Shift coordinates to draw section grab bar
			if ((_dockingControl.Dock == DockStyle.Top) || 
				(_dockingControl.Dock == DockStyle.Bottom))
			{
				for(int i=0; i<4; i++)
				{
					ptLight[i].X += 4;
					ptDark[i].X += 4;
				}
			}
			else
			{
				for(int i=0; i<4; i++)
				{
					ptLight[i].Y += 4;
					ptDark[i].Y += 4;
				}
			}

			pe.Graphics.DrawLine(lightPen, ptLight[0], ptLight[1]);
			pe.Graphics.DrawLine(lightPen, ptLight[2], ptLight[3]);
			pe.Graphics.DrawLine(darkPen, ptDark[0], ptDark[1]);
			pe.Graphics.DrawLine(darkPen, ptDark[2], ptDark[3]);

			// Ensure delegates are called
			base.OnPaint(pe);
		}

		#endregion

		#region Overrides
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if(_dragging)
			{
				_dragging = false;
				_dockingControl.EndDrag();

                if (_dockingControl.IsDocked)
                {
                    _MaximizeButton.Visible = false;
                    _DockButton.Visible = false;
                }
                else
                {
                    _MaximizeButton.Visible = true;
                    _DockButton.Visible = true;
                }
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Can only move the DockingControl is we have captured the
			// mouse otherwise the mouse is not currently pressed
			if (this.Capture)
			{
				// Must have reference to parent object
				if (null != _dockingControl)
				{
					this.Cursor = Cursors.Hand;
                    if (!_dockingControl.IsDocked)
                        _dockingControl.DockHelper.FrameRectangle = _dockingControl.Parent.Bounds;
                    // get the point on the title bar where the mouse was clicked
                    if (_dragging == false) _dragOffset = new Point(e.X, e.Y);
					// if we are withing docking range of an edge, show the docked control
					// otherwise, drag the form around
					_dragging = true;
					Point pt = this.PointToScreen(new Point(e.X, e.Y));
					_dockingControl.Drag(pt.X - _dragOffset.X, pt.Y - _dragOffset.Y);
				}
			}
			else
				this.Cursor = Cursors.Default;

			// Ensure delegates are called
			base.OnMouseMove(e);
		}

		protected override void OnDoubleClick(System.EventArgs e)
		{
            if (_dockingControl.IsDocked)
            {
                _dockingControl.UnDock();
                _DockButton.Visible = true;
                _MaximizeButton.Visible = true;
            }
            else
            {
                _dockingControl.ReDock();
                _DockButton.Visible = false;
                _MaximizeButton.Visible = false;
            }
		
			base.OnDoubleClick(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if (false == _dockingControl.IsDocked)
			{
				DrawFrameTitle(pe);
			}
			else
			{
				DrawGrabHandle(pe);
			}
		}

		#endregion
	}

	#endregion

	/// <summary>
	/// MiniFrameWnd is an implementation of a toolwindow to represent the
	/// dockbar when it is not attached to a form
	/// </summary>
	#region class MiniFrameWnd
	
	internal class MiniFrameWnd : System.Windows.Forms.Form
	{
		private Rectangle _lastLocation = Rectangle.Empty;
		private DockHelper _dockHelper = null;

		private System.ComponentModel.Container components = null;

		public MiniFrameWnd()
		{
			this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
			this.Text = "";
			this.ControlBox = false;
			this.StartPosition = FormStartPosition.Manual;
			this.ShowInTaskbar = false;
            this.TopMost = true;
            //this.Font = new Font("Arial", 1);
			_dockHelper = new DockHelper(this);
			_dockHelper.FrameRectangle = this.Bounds;

			InitializeComponent();

		}

		#region Windows Code
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			
			this.ResumeLayout(false);
		}

		#endregion

		#region Protected Methods

		protected Rectangle GetCaptionRect()
		{
			Rectangle r = new Rectangle(0,0,this.Width,Form.DefaultFont.FontFamily.GetEmHeight(FontStyle.Regular)+2);

			return r;
		}
		protected bool HitCaption(Point XY)
		{
			Rectangle r = GetCaptionRect();
			
			return r.Contains(XY);
		}

		#endregion
		
		#region Overrides

		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize(e);
            //_dockHelper.FrameRectangle = this.Bounds;
			if ((this.Controls.Count != 0) && (null != this.Controls[0]))
				this.Controls[0].Refresh();
		}

		#endregion

	}
	
	#endregion

}
	