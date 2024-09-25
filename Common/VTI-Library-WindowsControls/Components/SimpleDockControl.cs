using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Implements a dockable control that can be docked to a design-time determined location
    /// or undocked to a floating window.
    /// </summary>
    // give the control a designer category, to ensure design-time compatability
    [DesignerCategoryAttribute("Component")]
    // make sure we can add child controls at runtime
    // see Microsoft Knowledge Base Article - 813450
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class SimpleDockControl : UserControl
    {
        MenuStrip dock_ms = new MenuStrip();
        ToolStripMenuItem dockToolStripMenuItem = new ToolStripMenuItem("Dock");
        private UndockFrameForm _undockFrame;
        private Boolean _isDocked = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDockControl">SimpleDockControl</see> class
        /// </summary>
        public SimpleDockControl()
        {
            InitializeComponent();
            _undockFrame = new UndockFrameForm();
            _undockFrame.DockButtonClicked += new EventHandler(_undockFrame_DockButtonClicked);
            _undockFrame.FormClosing += new FormClosingEventHandler(_undockFrame_FormClosing);
            _undockFrame.Move += new EventHandler(_undockFrame_Move);
            dockToolStripMenuItem.Click += dockToolStripMenuItem_Click;
            dock_ms.Items.Add(dockToolStripMenuItem);
            dock_ms.Dock = DockStyle.Top;
        }

        private void dockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDocked();
        }

        /// <summary>
        /// Gets or sets the caption for the control
        /// </summary>
        public String Caption
        {
            get { return toolStripLabelCaption.Text; }
            set
            {
                toolStripLabelCaption.Text = value;
                _undockFrame.Text = value;
            }
        }

        /// <summary>
        /// Form to be used for the undocked state of the control
        /// </summary>
        public class UndockFrameForm : System.Windows.Forms.Form
        {
            private System.ComponentModel.Container components = null;
            private Font _dockFont = new Font("Arial", 8);
            private Boolean _mouseIsOver;
            //private ToolTip _toolTip = new ToolTip();

            /// <summary>
            /// Occurs when the Dock button is clicked
            /// </summary>
            public event EventHandler DockButtonClicked;

            /// <summary>
            /// Raises the <see cref="DockButtonClicked">DockButtonClicked</see> event
            /// </summary>
            protected virtual void OnDockButtonClicked()
            {
                if (DockButtonClicked != null)
                    DockButtonClicked(this, null);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="UndockFrameForm">UndockFrameForm</see> class
            /// </summary>
            public UndockFrameForm()
            {
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();
            }

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
            }

            /// <summary>
            /// Processes Win32 Messages
            /// </summary>
            /// <param name="m">Windows message</param>
            protected override void WndProc(ref Message m)
            {
                switch ((Win32Support.Win32MessageConstants)m.Msg)
                {
                    case Win32Support.Win32MessageConstants.WM_NCPAINT:
                    case Win32Support.Win32MessageConstants.WM_PAINT:
                        base.WndProc(ref m);
                        PaintNC(m.HWnd);
                        m.Result = IntPtr.Zero;
                        break;

                    case Win32Support.Win32MessageConstants.WM_ACTIVATE:
                        base.WndProc(ref m);
                        PaintNC(m.HWnd);
                        break;

                    case Win32Support.Win32MessageConstants.WM_NCMOUSEMOVE:
                        base.WndProc(ref m);
                        MouseMoveNC(ref m);
                        m.Result = IntPtr.Zero;
                        break;

                    case Win32Support.Win32MessageConstants.WM_NCLBUTTONDOWN:
                        base.WndProc(ref m);
                        //if (_mouseIsOver) OnDockButtonClicked(); do not dock if user clicked minimize window button on data plot window. Only dock if user clicks dock button in toolstrip.
                        m.Result = IntPtr.Zero;
                        break;

                    default:
                        base.WndProc(ref m);
                        PaintNC(m.HWnd);
                        break;
                }
            }

            private Rectangle GetButtonRect(System.IntPtr hWnd)
            {
                Size CloseButtonSize = SystemInformation.CaptionButtonSize;
                //int ButtonSize;
                //int Y;
                //if (this.FormBorderStyle == FormBorderStyle.FixedToolWindow ||
                //    this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                //{
                //    ButtonSize = 13;
                //    Y = 5;
                //}
                //else
                //{
                //    ButtonSize = 21;
                //    Y = 6;
                //}

                //int X = Bounds.Width - (ButtonSize + 3);
                //if (this.MinimizeBox) X -= (ButtonSize + 3);
                //if (this.MaximizeBox) X -= (ButtonSize + 3);

                //Rectangle rect = new Rectangle(X, Y, ButtonSize, ButtonSize);
                //return rect;

                IntPtr hDC = Win32Support.GetWindowDC(hWnd);
                Graphics g = Graphics.FromHdc(hDC);
                Size ButtonSize;
                int Y;
                if (this.FormBorderStyle == FormBorderStyle.FixedToolWindow ||
                    this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                {
                    ButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
                    Y = 4;
                }
                else
                {
                    ButtonSize = SystemInformation.CaptionButtonSize;
                    Y = 5;
                }

                int X = Bounds.Width - 12 - (ButtonSize.Width + 3);
                //Y = SystemInformation.CaptionHeight ;
                if (this.MinimizeBox) X -= (ButtonSize.Width + 3);
                if (this.MaximizeBox) X -= (ButtonSize.Width + 3);

                Rectangle rect = new Rectangle(X - 12 - (int)g.MeasureString("Dock", _dockFont).Width, Y, 12 + (int)g.MeasureString("Dock", _dockFont).Width, ButtonSize.Height - 3);
                g.Dispose();
                Win32Support.ReleaseDC(hWnd, hDC);
                return rect;
            }

            /// <summary>
            /// Paints the Non-Client area of the form
            /// </summary>
            /// <param name="hWnd">Handle to the Non-Client area</param>
            protected void PaintNC(System.IntPtr hWnd)
            {
                //IntPtr hDC = Win32Support.GetWindowDC(hWnd);
                //Graphics g = Graphics.FromHdc(hDC);
                ////int CaptionHeight = Bounds.Height - ClientRectangle.Height; //Titlebar
                //Rectangle ButtonRect = GetButtonRect();

                //if (this.FormBorderStyle == FormBorderStyle.FixedToolWindow ||
                //    this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                //{
                //    g.DrawImage(Properties.Resources.dock_small, ButtonRect);
                //}
                //else
                //{
                //    if (_mouseIsOver)
                //        g.DrawImage(Properties.Resources.dock_button2, ButtonRect);
                //    else
                //        g.DrawImage(Properties.Resources.dock_button, ButtonRect);
                //}
                //g.Dispose();
                //Win32Support.ReleaseDC(hWnd, hDC);

                IntPtr hDC = Win32Support.GetWindowDC(hWnd);
                Graphics g = Graphics.FromHdc(hDC);
                //int CaptionHeight = Bounds.Height - ClientRectangle.Height; //Titlebar
                Rectangle ButtonRect = GetButtonRect(hWnd);
                /*
                        if (_mouseIsOver)
                        {
                          g.FillRectangle(Brushes.Cornsilk, ButtonRect);
                          g.DrawRectangle(new Pen(Brushes.Black), ButtonRect);
                        }
                        else
                        {
                          g.FillRectangle(Brushes.Wheat, ButtonRect);
                          g.DrawRectangle(new Pen(Brushes.Black), ButtonRect);
                        }
                */
                //if (this.FormBorderStyle == FormBorderStyle.FixedToolWindow ||
                //    this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                //    g.DrawString("Dock", _dockFont, Brushes.Black, new PointF(ButtonRect.X + 6, ButtonRect.Y));
                //else
                //    g.DrawString("Dock", _dockFont, Brushes.Black, new PointF(ButtonRect.X + 6, ButtonRect.Y + 1));
                //g.DrawString("Dock", _dockFont, Brushes.Black, new PointF(ButtonRect.X + 6, ButtonRect.Y + (ButtonRect.Height - g.MeasureString("Dock", _dockFont).Height) / 2));

                g.Dispose();
                Win32Support.ReleaseDC(hWnd, hDC);
            }

            /// <summary>
            /// Detects mouse movements in the Non-Client area of the form
            /// </summary>
            /// <param name="m">Windows message containing information about the mouse movement</param>
            protected void MouseMoveNC(ref Message m)
            {
                //Size CloseButtonSize = SystemInformation.CaptionButtonSize;
                Rectangle ButtonRect = GetButtonRect(m.HWnd);

                Point mousePoint = this.PointToClient(Cursor.Position);
                mousePoint = new Point(mousePoint.X, -mousePoint.Y);

                _mouseIsOver = ButtonRect.Contains(mousePoint);
                //if (buttonRect.Contains(mousePoint))
                //{
                //    if (!_mouseIsOver)
                //    {
                //        _toolTip.SetToolTip(this, "Dock");
                //        _toolTip.ShowAlways = true;
                //        //_toolTip.Show("Dock", this);
                //    }
                //    _mouseIsOver = true;
                //}
                //else
                //{
                //    _mouseIsOver = false;
                //    //_toolTip.RemoveAll();
                //}

                PaintNC(m.HWnd);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                //
                // UndockFrameForm
                //
                //this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
                this.ClientSize = new System.Drawing.Size(650, 420);
                //this.MaximizeBox = false;
                //this.MinimizeBox = false;
                this.Name = "UndockFrameForm";
                this.Text = "UndockFrameForm";
            }

            #endregion Windows Form Designer generated code

            /// <summary>
            /// Raises the onPaint event
            /// </summary>
            /// <param name="e">A <see cref="System.Windows.Forms.PaintEventArgs">System.Windows.Forms.PaintEventArgs</see>
            /// that contains the event data</param>
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                PaintNC(this.Handle);
            }

            /// <summary>
            /// Raises the VisibleChanged event
            /// </summary>
            /// <param name="e">The <see cref="System.EventArgs">System.EventArgs</see> that contains the event data.</param>
            protected override void OnVisibleChanged(EventArgs e)
            {
                if (this.Visible == false) _mouseIsOver = false;
                base.OnVisibleChanged(e);
            }
        }

        /// <summary>
        /// Raises the ControlAdded event
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.ControlEventArgs">System.Windows.Forms.ControlEventArgs</see>
        /// that contains the event data</param>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            //if (!(e.Control == panel1 ||
            //    e.Control == toolStrip1))
            //{
            //    //this.Controls.Remove(e.Control);
            //    panel1.Controls.Add(e.Control);
            //}
            e.Control.BringToFront();
        }

        private void toolStripButtonUndock_Click(object sender, EventArgs e)
        {
            ShowUndocked();
        }

        /// <summary>
        /// Shows the control in the undocked state.
        /// </summary>
        public void ShowUndocked()
        {
            if (_isDocked)
            {
                _isDocked = false;
                this.Visible = false;
                //this.Controls.Remove(panel1);
                //_undockFrame.Controls.Add(panel1);
                foreach (Control control in this.Controls)
                {
                    if (control != toolStrip1)
                    {
                        this.Controls.Remove(control);
                        _undockFrame.Controls.Add(control);
                        if (_undockFrame.Controls.OfType<TestHistoryControl>().FirstOrDefault() != null)
                        {
                            _undockFrame.Controls.OfType<TestHistoryControl>().FirstOrDefault().Controls.Add(dock_ms);
                        }
                        control.Location = new Point(control.Location.X, control.Location.Y - toolStrip1.Height);
                    }
                }
            }
            _undockFrame.Show();
            _undockFrame.BringToFront();
            _isDocked = false;
            OnDockChanged(null);
        }

        /// <summary>
        /// Assign the undocked frame location of the DataPlot container
        /// </summary>
        public void AssignUndockFrameLocationOfDataPlotControl(int x, int y)
        {
            UndockFrameLocation = new Point(x, y);
        }

        private void _undockFrame_Move(object sender, EventArgs e)
        {
            Properties.Settings.Default.OpFormSingleTestHistoryX = _undockFrame.Location.X;
            Properties.Settings.Default.OpFormSingleTestHistoryY = _undockFrame.Location.Y;
            Properties.Settings.Default.OpFormSingleValvesPanelX = _undockFrame.Location.X;
            Properties.Settings.Default.OpFormSingleValvesPanelY = _undockFrame.Location.Y;
            Properties.Settings.Default.Save();
        }

        private void _undockFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _undockFrame.Hide();
                e.Cancel = true;
            }
        }

        private void _undockFrame_DockButtonClicked(object sender, EventArgs e)
        {
            ShowDocked();
        }

        /// <summary>
        /// Shows the control in the docked state.
        /// </summary>
        public void ShowDocked()
        {
            if (!_isDocked)
            {
                _undockFrame.Hide();
                foreach (Control control in _undockFrame.Controls)
                {
                    _undockFrame.Controls.Remove(control);
                    this.Controls.Add(control);
                    control.Location = new Point(control.Location.X, control.Location.Y + toolStrip1.Height);
                    //control.BringToFront();
                }
                if (this.Controls.OfType<TestHistoryControl>().FirstOrDefault() != null)
                {
                    MenuStrip currentDockButton = this.Controls.OfType<TestHistoryControl>().FirstOrDefault().Controls.OfType<MenuStrip>().FirstOrDefault();
                    this.Controls.OfType<TestHistoryControl>().FirstOrDefault().Controls.Remove(currentDockButton);
                }
            }
            _isDocked = true;
            this.Visible = true;
            OnDockChanged(null);
        }

        /// <summary>
        /// Shows the control.
        /// </summary>
        public new void Show()
        {
            if (_isDocked) ShowDocked();
            else ShowUndocked();
        }

        /// <summary>
        /// Gets the form that represents the undocked frame for the control.
        /// </summary>
        [Browsable(false)]
        public Form UndockFrame
        {
            get { return _undockFrame; }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the Maximize button is displayed
        /// in the caption of the undocked frame form.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Boolean UndockFrameMaximizeBox
        {
            get { return _undockFrame.MaximizeBox; }
            set { _undockFrame.MaximizeBox = value; }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the Minimize button is displayed
        /// in the caption of the undocked frame form.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Boolean UndockFrameMinimizeBox
        {
            get { return _undockFrame.MinimizeBox; }
            set { _undockFrame.MinimizeBox = value; }
        }

        /// <summary>
        /// Gets or sets the border style of the undocked frame form.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public FormBorderStyle UndockFrameFormBorderStyle
        {
            get { return _undockFrame.FormBorderStyle; }
            set { _undockFrame.FormBorderStyle = value; }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the undocked frame form
        /// should be displayed as a topmost form.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Boolean UndockFrameTopMost
        {
            get { return _undockFrame.TopMost; }
            set { _undockFrame.TopMost = value; }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the undocked frame form
        /// is displayed in the Windows Taskbar.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Boolean UndockFrameShowInTaskbar
        {
            get { return _undockFrame.ShowInTaskbar; }
            set { _undockFrame.ShowInTaskbar = value; }
        }

        /// <summary>
        /// Resize the form according to the setting of System.Windows.Forms.Form.AutoSizeMode.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Boolean UndockFrameAutoSize
        {
            get { return _undockFrame.AutoSize; }
            set { _undockFrame.AutoSize = value; }
        }

        /// <summary>
        /// Gets or sets the starting position of the undocked frame form at run time.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public FormStartPosition UndockFrameStartPosition
        {
            get { return _undockFrame.StartPosition; }
            set { _undockFrame.StartPosition = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Drawing.Point">System.Drawing.Point</see>
        /// that represents the upper-left corner of the
        /// <see cref="System.Windows.Forms.Form">System.Windows.Forms.Form</see> in screen coordinates.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Point UndockFrameLocation
        {
            get { return _undockFrame.Location; }
            set { _undockFrame.Location = value; }
        }

        /// <summary>
        /// Gets a value to indicate if the control is docked.
        /// </summary>
        public Boolean IsDocked
        {
            get { return _isDocked; }
            set
            {
                if (value) ShowDocked();
                else ShowUndocked();
            }
        }
    }
}