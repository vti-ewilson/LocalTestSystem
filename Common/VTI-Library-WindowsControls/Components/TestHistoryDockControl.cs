using System;
using System.ComponentModel;
using System.Drawing;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a test history control that can be docked
    /// </summary>
    public partial class TestHistoryDockControl : SimpleDockControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestHistoryDockControl">TestHistoryDockControl</see>
        /// </summary>
        public TestHistoryDockControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the Test History Control contained withing this control
        /// </summary>
        [Browsable(false)]
        public TestHistoryControl TestHistory
        {
            get { return testHistoryControl1; }
        }

        /// <summary>
        /// Gets or sets the size of the labels for the control
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Size LabelSize
        {
            get { return testHistoryControl1.LabelSize; }
            set { testHistoryControl1.LabelSize = value; }
        }

        /// <summary>
        /// Gets or sets the number of rows in the control
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Rows
        {
            get { return testHistoryControl1.Rows; }
            set { testHistoryControl1.Rows = value; }
        }

        /// <summary>
        /// Gets or sets the number of columns of the control
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Columns
        {
            get { return testHistoryControl1.Columns; }
            set { testHistoryControl1.Columns = value; }
        }

        /// <summary>
        /// Raises the DockChanged event
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);
            if (!this.IsDocked)
            {
                this.UndockFrame.Size = testHistoryControl1.Size;
            }
        }
    }
}