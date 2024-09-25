using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel.Design;

namespace VTIWindowsControlLibrary.Components
{

    // give the control a designer category, to ensure design-time compatability
    [DesignerCategoryAttribute("Component")]
    // makse sure we can add child controls at runtime
    // see Microsoft Knowledge Base Article - 813450
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class DockControl2 : UserControl
    {
        private UndockFrameForm _undockFrame;

        public DockControl2()
        {
            InitializeComponent();
            _undockFrame = new UndockFrameForm();
            _undockFrame.DockButtonClicked += new EventHandler(_undockFrame_DockButtonClicked);
        }


        /// <summary>
        /// Summary description for UndockFrameForm.
        /// </summary>
        public class UndockFrameForm : System.Windows.Forms.Form
        {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.Container components = null;

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            //Message constants
            public const int WM_NCPAINT = 0x0085;
            public const int WM_NCMOUSEMOVE = 0x00A0;
            //public const int WM_NCLBUTTONUP = 0x00A2;
            public const int WM_NCLBUTTONDOWN = 0x00A1;

            private Boolean _mouseIsOver;

            public event EventHandler DockButtonClicked;
            protected virtual void OnDockButtonClicked()
            {
                if (DockButtonClicked != null)
                    DockButtonClicked(this, null);
            }

            public UndockFrameForm()
            {
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();

                //
                // TODO: Add any constructor code after InitializeComponent call
                //
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

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_NCPAINT:
                        {
                            base.WndProc(ref m);
                            Debug.WriteLine("WM_NCPAINT received");
                            //IntPtr hDC = GetWindowDC(m.HWnd);
                            //Graphics g = Graphics.FromHdc(hDC);
                            PaintNC(m.HWnd);
                            //g.Dispose();
                            //ReleaseDC(m.HWnd, hDC);
                            m.Result = IntPtr.Zero;
                        }
                        break;

                    case WM_NCMOUSEMOVE:
                        {
                            base.WndProc(ref m);
                            MouseMoveNC(ref m);
                            m.Result = IntPtr.Zero;
                        }
                        break;

                    case WM_NCLBUTTONDOWN:
                        {
                            base.WndProc(ref m);
                            if (_mouseIsOver) OnDockButtonClicked();
                            m.Result = IntPtr.Zero;
                        }
                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }

            protected void PaintNC(System.IntPtr hWnd)
            {
                IntPtr hDC = GetWindowDC(hWnd);
                Graphics g = Graphics.FromHdc(hDC);
                int CaptionHeight = Bounds.Height - ClientRectangle.Height; //Titlebar
                Size CloseButtonSize = SystemInformation.CaptionButtonSize;
                int X = Bounds.Width + 4 - CloseButtonSize.Width * 4;
                int Y = 6;
                if (_mouseIsOver)
                    g.DrawImage(Properties.Resources.dock_button2, new Rectangle(X, Y, 21, 21));
                else
                    g.DrawImage(Properties.Resources.dock_button, new Rectangle(X, Y, 21, 21));
                g.Dispose();
                ReleaseDC(hWnd, hDC);
            }

            protected void MouseMoveNC(ref Message m)
            {
                Size CloseButtonSize = SystemInformation.CaptionButtonSize;
                int X = Bounds.Width + 4 - CloseButtonSize.Width * 4;
                int Y = 6;
                Rectangle buttonRect = new Rectangle(X, Y, 21, 21);
                
                Point mousePoint = this.PointToClient(Cursor.Position);
                mousePoint = new Point(mousePoint.X, -mousePoint.Y);
                
                _mouseIsOver = buttonRect.Contains(mousePoint);

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

            #endregion
        }
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
            this.Visible = false;
            //this.Controls.Remove(panel1);
            //_undockFrame.Controls.Add(panel1);
            foreach (Control control in this.Controls)
            {
                if (control != toolStrip1)
                {
                    this.Controls.Remove(control);
                    _undockFrame.Controls.Add(control);
                }
            }
            _undockFrame.Show();
        }

        private void _undockFrame_DockButtonClicked(object sender, EventArgs e)
        {
            _undockFrame.Hide();
            foreach (Control control in _undockFrame.Controls)
            {
                _undockFrame.Controls.Remove(control);
                this.Controls.Add(control);
                //control.BringToFront();
            }
            this.Visible = true;
        }
    }
}
