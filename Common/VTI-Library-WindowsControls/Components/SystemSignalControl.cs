using System;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// A control which can display the current value of an
    /// <see cref="AnalogSignal"/>
    /// </summary>
    public partial class SystemSignalControl : UserControl
    {
        private AnalogSignal _analogSignal;
        private SystemSignalsControl _systemSignalsControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSignalControl"/> class.
        /// </summary>
        public SystemSignalControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSignalControl"/> class.
        /// </summary>
        /// <param name="systemSignalsControl">The system signals control.</param>
        /// <param name="label">The label.</param>
        public SystemSignalControl(SystemSignalsControl systemSignalsControl, string label)
            : this()
        {
            _systemSignalsControl = systemSignalsControl;
            labelCaption.Text = label;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSignalControl"/> class.
        /// </summary>
        /// <param name="systemSignalsControl">The system signals control.</param>
        /// <param name="analogSignal">The analog signal.</param>
        public SystemSignalControl(SystemSignalsControl systemSignalsControl, AnalogSignal analogSignal)
            : this()
        {
            _systemSignalsControl = systemSignalsControl;
            _analogSignal = analogSignal;
            
            labelCaption.Text = analogSignal.Label.Trim();
            this.Visible = analogSignal.Visible;
            analogSignal.VisibleChanged += new EventHandler(signal_VisibleChanged);
            analogSignal.LabelChanged += new EventHandler(analogSignal_LabelChanged);
            if (analogSignal.IsFormattedInput)
            {
                horizSignalIndicatorValue.SemiLog = false;
                horizSignalIndicatorValue.LinMax = (float)analogSignal.FormattedAnalogInput.Max;
                horizSignalIndicatorValue.LinMin = (float)analogSignal.FormattedAnalogInput.Min;
            }
            else if (analogSignal.SignalConverter == null)
            {
                horizSignalIndicatorValue.SemiLog = false;
                horizSignalIndicatorValue.LinMax = analogSignal.TurnRedLimit;
                horizSignalIndicatorValue.LinMin = 0;
            }
            else
            {
                if (analogSignal.SignalConverter is ILinearSignalConverter)
                {
                    horizSignalIndicatorValue.SemiLog = false;
                    horizSignalIndicatorValue.LinMax = (float)((analogSignal.SignalConverter as ILinearSignalConverter).FullScale);
                    horizSignalIndicatorValue.LinMin = 0;
                }
                else
                {
                    horizSignalIndicatorValue.SemiLog = true;
                    horizSignalIndicatorValue.LogMaxExp = (int)((analogSignal.SignalConverter as ILogLinearSignalConverter).MaxExponent);
                    horizSignalIndicatorValue.LogMinExp = (int)((analogSignal.SignalConverter as ILogLinearSignalConverter).MinExponent);
                }
            }
            labelCaption.MinimumSize = new Size(this.Width, 15);
            labelCaption.MaximumSize = new Size(this.Width, 30);
        }

        private void analogSignal_LabelChanged(object sender, EventArgs e)
        {
            labelCaption.SetText(_analogSignal.Label.Trim());
        }

        private void signal_VisibleChanged(object sender, EventArgs e)
        {
            this.SetVisible(_analogSignal.Visible);
        }

        /// <summary>
        /// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        public override void Refresh()
        {
            try
            {
                if (_analogSignal == null) return;

                if (_systemSignalsControl.ShowingFromDataPlot)
                {
                    horizSignalIndicatorValue.Value =
                        _systemSignalsControl.DataPlotControl.GraphControl.GetValueAtPlotCursor(_analogSignal.Key);
                    horizSignalIndicatorValue.SetText(
                        string.Format(
                            "{0} {1}",
                            horizSignalIndicatorValue.Value.ToString(_analogSignal.Format),
                            _analogSignal.Units));
                }
                else
                {
                    if (_systemSignalsControl.ShowingRawValues && _analogSignal.ShowsRawValue)
                    {
                        if (_analogSignal.AnalogInput != null)
                        {
                            horizSignalIndicatorValue.SetText(_analogSignal.RawValue.ToString("0.000") + " " + _analogSignal.AnalogInput.Units);
                        }
                        else if (_analogSignal.Label != string.Empty)
                        {
                            horizSignalIndicatorValue.SetText(_analogSignal.RawValue.ToString("0.000") + " V");
                        }
                        else
                        {
                            //if you have a dummy signal for spacing the signals out in the system signals panel,
                            //do not show "0.00 V" for the dummy signal when switching to "ShowingRawValues" mode.
                            horizSignalIndicatorValue.SetText(string.Empty);
                        }
                    }
                    else
                    {
                        horizSignalIndicatorValue.SetText(_analogSignal.FormattedValue);
                    }
                    horizSignalIndicatorValue.Value = (float)_analogSignal.Value;
                }

                if ((_analogSignal.TurnRedLimit != -1) && (horizSignalIndicatorValue.Value > _analogSignal.TurnRedLimit))
                {
                    horizSignalIndicatorValue.IndicatorColor = System.Drawing.Color.Red;
                    horizSignalIndicatorValue.ForeColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    horizSignalIndicatorValue.IndicatorColor = System.Drawing.Color.FromArgb(0, 153, 0);
                    horizSignalIndicatorValue.ForeColor = System.Drawing.Color.Yellow;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return labelCaption.Text; }
            set { labelCaption.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the text of the indicator.
        /// </summary>
        public override string Text
        {
            get { return horizSignalIndicatorValue.Text; }
            set { horizSignalIndicatorValue.SetText(value); }
        }

        private void SystemSignalControl_Resize(object sender, EventArgs e)
        {
            labelCaption.MinimumSize = new Size(this.Width, 15);
            labelCaption.MaximumSize = new Size(this.Width, 30);
        }
    }
}