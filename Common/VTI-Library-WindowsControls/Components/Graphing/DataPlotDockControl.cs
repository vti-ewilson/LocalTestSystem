using System;

namespace VTIWindowsControlLibrary.Components.Graphing
{
    /// <summary>
    /// Represents a <see cref="SimpleDockControl">dockable control</see> that contains a
    /// <see cref="DataPlotControl">DataPlot</see>
    /// </summary>
    public partial class DataPlotDockControl : SimpleDockControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPlotDockControl">DataPlotDockControl</see>
        /// </summary>
        public DataPlotDockControl()
        {
            InitializeComponent();
            this.Caption = "Data Plot";
            this.DockChanged += new EventHandler(DataPlotDockControl_DockChanged);
        }

        private void DataPlotDockControl_DockChanged(object sender, EventArgs e)
        {
            dataPlotControl1.Refresh();
        }

        private void dataPlotControl1_CaptionChanged(object sender, EventArgs e)
        {
            this.Caption = dataPlotControl1.Caption;
        }

        /// <summary>
        /// Gets the DataPlot contained within the control
        /// </summary>
        public DataPlotControl DataPlot
        {
            get { return dataPlotControl1; }
            set { dataPlotControl1 = value; }
        }

        private void dataPlotControl1_Close(object sender, EventArgs e)
        {
            if (this.IsDocked) this.Hide();
            else this.UndockFrame.Hide();
        }
    }
}