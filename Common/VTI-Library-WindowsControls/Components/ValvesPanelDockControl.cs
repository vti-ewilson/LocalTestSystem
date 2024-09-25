using System;
using System.ComponentModel;
using System.Drawing;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a valves panel control that can be docked
    /// </summary>
    public partial class ValvesPanelDockControl : SimpleDockControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValvesPanelDockControl">ValvesPanelDockControl</see>
        /// </summary>
        public ValvesPanelDockControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the Valves Panel Control contained withing this control
        /// </summary>
        [Browsable(false)]
        public ValvesPanelControl ValvesPanel
        {
            get { return ValvesPanelControl1; }
        }

        /// <summary>
        /// Gets or sets the size of the labels for the control
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Size LabelSize
        {
            get { return ValvesPanelControl1.LabelSize; }
            set { ValvesPanelControl1.LabelSize = value; }
        }

        /// <summary>
        /// Gets or sets the number of rows in the control
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Rows
        {
            get { return ValvesPanelControl1.Rows; }
            set { ValvesPanelControl1.Rows = value; }
        }

        /// <summary>
        /// Gets or sets the number of columns of the control
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Columns
        {
            get { return ValvesPanelControl1.Columns; }
            set { ValvesPanelControl1.Columns = value; }
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
                this.UndockFrame.Size = ValvesPanelControl1.Size;
            }
        }
    }
}