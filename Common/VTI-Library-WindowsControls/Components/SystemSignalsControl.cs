using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components.Graphing;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Implements a panel containing <see cref="SystemSignalControl">System Signal Controls</see>.
    /// </summary>
    public partial class SystemSignalsControl : UserControl
    {
        private bool _showXButton;
        private bool _showingFromDataPlot = false;
        private string _caption = "SYSTEM SIGNALS";
        private Font _signalCaptionFont = new System.Drawing.Font("Arial", 10.125F);
        private int _signalCaptionWidth = 300;
        private Font _signalValueFont = new System.Drawing.Font("Microsoft Sans Serif", 7.875F);
        private bool _rawValuesEnabled = true;
        private bool _showingRawValues = false;
        private DataPlotControl _dataPlotControl;
        private List<AnalogSignal> _analogSignalList;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSignalsControl"/> class.
        /// </summary>
        public SystemSignalsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the data plot control.
        /// </summary>
        /// <value>The data plot control.</value>
        public DataPlotControl DataPlotControl
        {
            get { return _dataPlotControl; }
            set { _dataPlotControl = value; }
        }

        public List<AnalogSignal> AnalogSignalList
        {
            get { return _analogSignalList; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether or not the close button is visible.
        /// </summary>
        public bool ShowXButton
        {
            get
            {
                return _showXButton;
            }
            set
            {
                _showXButton = value;
                buttonHide.Visible = value;
            }
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void SystemSignalsControl2_Resize(object sender, EventArgs e)
        {
            if (this.Width < 130)
            {
                labelCaption.Height = 26;
            }
            else
            {
                labelCaption.Height = 16;
            }
        }

        private void labelCaption_Click(object sender, EventArgs e)
        {
            ShowingRawValues = !ShowingRawValues;
        }

        /// <summary>
        /// Gets or sets a value that indicates if the <see cref="VTIWindowsControlLibrary.Classes.IO.AnalogSignal.RawValue">RawValue</see>
        /// (i.e. volts, amps, etc.) of the <see cref="VTIWindowsControlLibrary.Classes.IO.AnalogSignal">AnalogSignals</see> can be displayed.
        /// </summary>
        public bool RawValuesEnabled
        {
            get { return _rawValuesEnabled; }
            set { _rawValuesEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the <see cref="VTIWindowsControlLibrary.Classes.IO.AnalogSignal.RawValue">RawValue</see>
        /// (i.e. volts, amps, etc.) of the <see cref="VTIWindowsControlLibrary.Classes.IO.AnalogSignal">AnalogSignals</see> are being displayed.
        /// </summary>
        public bool ShowingRawValues
        {
            get { return _showingRawValues; }
            set
            {
                if (_rawValuesEnabled)
                {
                    _showingRawValues = value;
                    _caption = (_showingRawValues ? "RAW VALUES" : "SYSTEM SIGNALS");
                    labelCaption.SetText(_caption);

                    foreach (SystemSignalControl systemSignalControl in panelBody.Controls)
                        systemSignalControl.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the caption for the control.
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                labelCaption.SetText(_caption);
            }
        }

        /// <summary>
        /// Adds a manual signal.
        /// </summary>
        /// <param name="label">The label to be displayed.</param>
        /// <returns>The <see cref="SystemSignalControl"/>.</returns>
        public SystemSignalControl AddManualSignal(string label)
        {
            SystemSignalControl systemSignalControl = new SystemSignalControl(this, label);
            panelBody.Controls.Add(systemSignalControl);
            systemSignalControl.Dock = DockStyle.Top;
            systemSignalControl.BringToFront();
            return systemSignalControl;
        }

        /// <summary>
        /// Clear all analog signals
        /// </summary>
        public void ClearAnalogSignals()
        {
            panelBody.Controls.Clear();
        }

        /// <summary>
        /// Gets or sets the Signal caption font for the control.
        /// </summary>
        public Font SignalCaptionFont
        {
            get { return _signalCaptionFont; }
            set
            {
                _signalCaptionFont = value;
            }
        }

        /// <summary>
        /// Gets or sets the Signal caption width for the control.
        /// </summary>
        public int SignalCaptionWidth
        {
            get { return _signalCaptionWidth; }
            set
            {
                _signalCaptionWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the Signal Value font for the control.
        /// </summary>
        public Font SignalValueFont
        {
            get { return _signalValueFont; }
            set
            {
                _signalValueFont = value;
            }
        }

        /// <summary>
        /// Adds the analog signal.
        /// </summary>
        /// <param name="analogSignal">The <see cref="AnalogSignal">analog signal</see>.</param>
        /// <returns>The <see cref="SystemSignalControl"/>.</returns>
        public SystemSignalControl AddAnalogSignal(AnalogSignal analogSignal)
        {
            SystemSignalControl systemSignalControl = new SystemSignalControl(this, analogSignal);
            panelBody.Controls.Add(systemSignalControl);
            systemSignalControl.Dock = DockStyle.Top;
            systemSignalControl.BringToFront();
            systemSignalControl.Width = _signalCaptionWidth;
            systemSignalControl.labelCaption.Font = _signalCaptionFont;
            systemSignalControl.labelCaption.Width = _signalCaptionWidth;
            systemSignalControl.horizSignalIndicatorValue.Font = _signalValueFont;
            systemSignalControl.horizSignalIndicatorValue.Width = _signalCaptionWidth;

            //systemSignalControl.labelCaption.Font = Properties.Settings.Default.TestHi
            if (!_analogSignalList.Contains(analogSignal)) // keep track of analog signals
                _analogSignalList.Add(analogSignal);
            return systemSignalControl;
        }

        /// <summary>
        /// Adds all analog signals.
        /// </summary>
        /// <param name="analogSignals">The analog signals.</param>
        public void AddAllAnalogSignals(AnalogSignalCollection analogSignals)
        {
            foreach (var signal in analogSignals)
                AddAnalogSignal(signal);
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            refreshTimer.Enabled = false;
            if (_dataPlotControl != null)
                ShowingFromDataPlot = _dataPlotControl.GraphControl.ShowingPlotCursor;
            foreach (SystemSignalControl systemSignalControl in panelBody.Controls)
                systemSignalControl.Refresh();
            refreshTimer.Enabled = true;
        }

        private void SystemSignalsControl2_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && !DesignMode)
                refreshTimer.Enabled = true;
            else
                refreshTimer.Enabled = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is showing values from the data plot.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if showing values from the data plot; otherwise, <c>false</c>.
        /// </value>
        public bool ShowingFromDataPlot
        {
            get { return _showingFromDataPlot; }
            set
            {
                if (_showingFromDataPlot != value)
                {
                    _showingFromDataPlot = value;
                    labelFromDataPlot.SetVisible(value);
                }
            }
        }
        /// <summary>
        /// Gets or sets the interval of the timer that refreshes/updates all of the signals in the System Signals panel.
		/// Usage: In Machine.cs, InitializeOperatorForm(), add: _OpFormDual.SystemSignals[0].refreshTimerInterval = 100; and _OpFormDual.SystemSignals[1].refreshTimerInterval = 100;
        /// </summary>
        public int refreshTimerInterval
        {
            get { return refreshTimer.Interval; }
            set { refreshTimer.Interval = value; }
        }
    }
}