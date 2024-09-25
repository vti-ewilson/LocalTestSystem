using System;
using VTIWindowsControlLibrary.Classes.Graphing.DataPlot;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Represents the conditioned signal for an analog input.  The Value parameter
    /// is scaled to end-user units (i.e., psi, Torr, cc/s, etc.)
    /// The RawValue is acquired from an object derived from the AnalogInputBase class.
    /// The analog input can be directed at any I/O interface that derives from AnalogInputBase.
    /// </summary>
    public class AnalogSignal
    {
        #region Fields (15)

        #region Private Fields (15)

        //private String _IOName;
        //private IOInterface _IOInterface;
        private IAnalogInput _AnalogInput;

        private AnalogSignal _AnalogSignal;
        private String _format = "0.00";
        private IFormattedAnalogInput _FormattedAnalogInput;
        private String _formattedValue;
        private Boolean _IsFormattedInput;
        private Boolean _IsStringInput;
        private String _key;
        private String _label;
        private String _englishLabel;
        private Double _rawValue;
        private Boolean _showsRawValue = true;
        private IAnalogSignalConverter _SignalConverter;
        private Single _turnRedLimit = 100;
        private String _units = string.Empty;
        private Double _value;
        private Boolean _visible = true;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (4)

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogSignal">AnalogSignal</see> class based on another analog signal, with a signal converter
        /// </summary>
        /// <param name="Label">Display label</param>
        /// <param name="Units">Units of the analog signal (i.e. psi, Torr, etc.)</param>
        /// <param name="Format">Format string for displaying the value</param>
        /// <param name="TurnRedLimit">Level at which the displayed value turns red</param>
        /// <param name="ShowsRawValue">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> can show a raw value (i.e. volts, amps, etc.)</param>
        /// <param name="Visible">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> should be visible</param>
        /// <param name="AnalogSignal">A different <see cref="AnalogSignal">AnalogSignal</see> from which to calculate the value of this signal</param>
        /// <param name="SignalConverter">The <see cref="IAnalogSignalConverter">IAnalogSignalConverter</see> to calculate this signal</param>
        public AnalogSignal(String Label, String Units, String Format, Single TurnRedLimit, Boolean ShowsRawValue, Boolean Visible, AnalogSignal AnalogSignal, IAnalogSignalConverter SignalConverter)
        {
            _key = Label.Replace(" ", string.Empty);
            _label = Label;
            _value = 0;
            _rawValue = 0;
            _units = Units;
            _format = Format;
            _turnRedLimit = TurnRedLimit;
            _showsRawValue = ShowsRawValue;
            _visible = Visible;
            //_IOName = IOName;
            _SignalConverter = SignalConverter;
            _AnalogSignal = AnalogSignal;
            _AnalogSignal.ValueChanged += new EventHandler(_AnalogSignal_ValueChanged);
            _IsFormattedInput = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogSignal">AnalogSignal</see> class based on another analog signal, with a signal converter
        /// </summary>
        /// <param name="Label">Display label</param>
        /// <param name="Units">Units of the analog signal (i.e. psi, Torr, etc.)</param>
        /// <param name="Format">Format string for displaying the value</param>
        /// <param name="TurnRedLimit">Level at which the displayed value turns red</param>
        /// <param name="ShowsRawValue">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> can show a raw value (i.e. volts, amps, etc.)</param>
        /// <param name="Visible">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> should be visible</param>
        /// <param name="AnalogSignal">A different <see cref="AnalogSignal">AnalogSignal</see> from which to calculate the value of this signal</param>
        public AnalogSignal(String Label, String Units, String Format, Single TurnRedLimit, Boolean ShowsRawValue, Boolean Visible, AnalogSignal AnalogSignal)
        {
            _key = Label.Replace(" ", string.Empty);
            _label = Label;
            _value = 0;
            _rawValue = 0;
            _units = Units;
            _format = Format;
            _turnRedLimit = TurnRedLimit;
            _showsRawValue = ShowsRawValue;
            _visible = Visible;
            //_IOName = IOName;
            _AnalogSignal = AnalogSignal;
            _AnalogSignal.ValueChanged += new EventHandler(_AnalogSignal_ValueChanged);
            _IsFormattedInput = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogSignal">AnalogSignal</see> class based on an <see cref="IAnalogInput">AnalogInput</see>
        /// </summary>
        /// <param name="Label">Display label</param>
        /// <param name="Units">Units of the analog signal (i.e. psi, Torr, etc.)</param>
        /// <param name="Format">Format string for displaying the value</param>
        /// <param name="TurnRedLimit">Level at which the displayed value turns red</param>
        /// <param name="ShowsRawValue">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> can show a raw value (i.e. volts, amps, etc.)</param>
        /// <param name="Visible">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> should be visible</param>
        /// <param name="AnalogInput">The <see cref="IAnalogInput">AnalogInput</see> from which to calculate the value of this signal</param>
        /// <param name="SignalConverter">The <see cref="IAnalogSignalConverter">IAnalogSignalConverter</see> to calculate this signal</param>
        public AnalogSignal(String Label, String Units, String Format, Single TurnRedLimit, Boolean ShowsRawValue, Boolean Visible, IAnalogInput AnalogInput, IAnalogSignalConverter SignalConverter)
        {
            _key = Label.Replace(" ", string.Empty);
            _label = Label;
            _value = 0;
            _rawValue = 0;
            _units = Units;
            _format = Format;
            _turnRedLimit = TurnRedLimit;
            _showsRawValue = ShowsRawValue;
            _visible = Visible;
            //_IOName = IOName;
            _SignalConverter = SignalConverter;
            _AnalogInput = AnalogInput; // _IOInterface.AnalogInputs[_IOName];
            _AnalogInput.RawValueChanged += new EventHandler(_AnalogInput_RawValueChanged);
            _IsFormattedInput = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogSignal">AnalogSignal</see> class without an associated AnalogInput
        /// </summary>
        /// <param name="Label">Display label</param>
        /// <param name="Units">Units of the analog signal (i.e. psi, Torr, etc.)</param>
        /// <param name="Format">Format string for displaying the value</param>
        /// <param name="TurnRedLimit">Level at which the displayed value turns red</param>
        /// <param name="ShowsRawValue">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> can show a raw value (i.e. volts, amps, etc.)</param>
        /// <param name="Visible">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> should be visible</param>
        public AnalogSignal(String Label, String Units, String Format, Single TurnRedLimit, Boolean ShowsRawValue, Boolean Visible)
        {
            _key = Label.Replace(" ", string.Empty);
            _label = Label;
            _value = 0;
            _rawValue = 0;
            _units = Units;
            _format = Format;
            _turnRedLimit = TurnRedLimit;
            _showsRawValue = ShowsRawValue;
            _visible = Visible;
            _IsFormattedInput = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogSignal">AnalogSignal</see> class which displays a string value instead of a double value (use StringValue property).
        /// </summary>
        /// <param name="Label">Display label</param>
        public AnalogSignal(String Label)
        {
            _key = Label.Replace(" ", string.Empty);
            _label = Label;
            _value = 0;
            _rawValue = 0;
            _units = "";
            _format = "(0:#;;' '";
            _turnRedLimit = 0;
            _showsRawValue = true;
            _visible = true;
            _IsStringInput = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogSignal">AnalogSignal</see> class based on an <see cref="IFormattedAnalogInput">FormattedAnalogInput</see>
        /// </summary>
        /// <param name="Label">Display label</param>
        /// <param name="TurnRedLimit">Level at which the displayed value turns red</param>
        /// <param name="ShowsRawValue">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> can show a raw value (i.e. volts, amps, etc.)</param>
        /// <param name="Visible">Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> should be visible</param>
        /// <param name="FormattedAnalogInput">The <see cref="FormattedAnalogInput">FormattedAnalogInput</see> to use for the value of this signal</param>
        /// <remarks>
        /// <list>
        /// <listheader>Examples of <see cref="IFormattedAnalogInput">FormattedAnalogInputs</see> include:</listheader>
        /// <item><see cref="VTIWindowsControlLibrary.Classes.IO.SerialIO.TorrConII">TorrConnII</see></item>
        /// <item><see cref="VTIWindowsControlLibrary.Classes.IO.SerialIO.PVGC5Controller">PVGC5Controller</see></item>
        /// <item><see cref="VTIWindowsControlLibrary.Classes.IO.SerialIO.IAIRoboCylinder">IAIRoboCylinder</see></item>
        /// <item><see cref="VTIWindowsControlLibrary.Classes.IO.SerialIO.AccuLabScale">AccuLabScale</see></item>
        /// </list>
        /// </remarks>
        public AnalogSignal(String Label, Single TurnRedLimit, Boolean ShowsRawValue, Boolean Visible, IFormattedAnalogInput FormattedAnalogInput)
        {
            _key = Label.Replace(" ", string.Empty);
            _label = Label;
            _turnRedLimit = TurnRedLimit;
            _showsRawValue = ShowsRawValue;
            _visible = Visible;
            _FormattedAnalogInput = FormattedAnalogInput;
            _FormattedAnalogInput.ValueChanged += new EventHandler(_FormattedAnalogInput_ValueChanged);
            _FormattedAnalogInput.RawValueChanged += new EventHandler(_FormattedAnalogInput_RawValueChanged);
            _units = _FormattedAnalogInput.Units;
            _format = _FormattedAnalogInput.Format;
            _IsFormattedInput = true;
        }

        #endregion Constructors

        #region Properties (15)

        /// <summary>
        /// AnalogInput on which the analog signal can be based
        /// </summary>
        public IAnalogInput AnalogInput
        {
            get { return _AnalogInput; }
        }

        /// <summary>
        /// Format string to be used when displaying the value of the analog signal
        /// </summary>
        public String Format
        {
            get { return _format; }
            set
            {
                _format = value;
                OnChanged(AnalogSignalChangeType.Format);
            }
        }

        /// <summary>
        /// FormattedAnalogInput on which the analog signal can be based
        /// </summary>
        public IFormattedAnalogInput FormattedAnalogInput
        {
            get { return _FormattedAnalogInput; }
        }

        /// <summary>
        /// Returns the <see cref="Value">Value</see> formatted according to the <see cref="Format">Format</see>, followed by the <see cref="Units">Units</see>.
        /// If the <see cref="AnalogSignal">AnalogSignal</see> is based on a <see cref="IFormattedAnalogInput">FormattedAnalogInput</see>, this
        /// returns the value of the <see cref="IFormattedAnalogInput.FormattedValue">FormattedAnalogInput.FormattedValue</see>.
        /// </summary>
        public String FormattedValue
        {
            get
            {
                if (_IsFormattedInput)
                    return _FormattedAnalogInput.FormattedValue;
                else
                  if (Double.IsNaN(_value))
                    return "ERROR";
                else
                    return _value.ToString(_format) + " " + _units;
            }
        }

        /// <summary>
        /// Returns the <see cref="StringValue">StringValue</see>.
        /// </summary>
        public String StringValue
        {
            get
            {
                if (_IsStringInput)
                    return _units;
                else
                    return "NOT STRING INPUT, incorrect AnalogSignal() constructor used.";
            }
            set
            {
                _units = value;
                OnChanged(AnalogSignalChangeType.Units);
                OnValueChanged();
            }
        }

        /// <summary>
        /// Returns true if the analog input is a <see cref="IFormattedAnalogInput">FormattedAnalogInput</see>
        /// </summary>
        public Boolean IsFormattedInput
        {
            get { return _IsFormattedInput; }
        }

        /// <summary>
        /// Key value is calculated internally to be equal to the <see cref="Label">Label</see> without any spaces
        /// </summary>
        public String Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Display Label
        /// </summary>
        public String Label
        {
            get { return _label; }
            set
            {
                _label = value;
                if (_key == null)
                    _key = value.Replace(" ", string.Empty);
                OnChanged(AnalogSignalChangeType.Label);
                OnLabelChanged();
            }
        }

        /// <summary>
        /// Set the label of the Analog Signal
        /// </summary>
        /// <param name="strLabel"></param>
        public void SetLabel(String strLabel)
        {
            if (strLabel != null)
            {
                _label = strLabel;
                if (_englishLabel == null)
                    _englishLabel = _label;
            }
        }

        /// <summary>
        /// Get English Label
        /// </summary>
        public string EnglishLabel
        {
            get
            {
                if (_englishLabel != null)
                    return _englishLabel;
                else
                    return _label;
            }
        }

        /// <summary>
        /// Raw value (i.e. volts, amps, etc.) of the analog input
        /// </summary>
        public Double RawValue
        {
            get { return _rawValue; }
            set
            {
                _rawValue = value;
                if (_SignalConverter != null) this.Value = _SignalConverter.Convert(_rawValue);
                OnChanged(AnalogSignalChangeType.RawValue);
                OnRawValueChanged();
            }
        }

        /// <summary>
        /// Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> can show a raw value (i.e. volts, amps, etc.)
        /// </summary>
        public Boolean ShowsRawValue
        {
            get { return _showsRawValue; }
            set { _showsRawValue = value; }
        }

        /// <summary>
        /// Signal converter for the analog signal
        /// </summary>
        public IAnalogSignalConverter SignalConverter
        {
            get { return _SignalConverter; }
        }

        /// <summary>
        /// Data plot trace associated with this analog signal
        /// </summary>
        public DataPlotTraceType Trace { get; internal set; }

        /// <summary>
        /// Level at which the displayed value turns red
        /// </summary>
        public Single TurnRedLimit
        {
            get { return _turnRedLimit; }
            set
            {
                _turnRedLimit = value;
                OnChanged(AnalogSignalChangeType.TurnRedLimit);
            }
        }

        /// <summary>
        /// Units (i.e. psi, Torr, etc.) of the analog signal
        /// </summary>
        public String Units
        {
            get { return _units; }
            set
            {
                _units = value;
                OnChanged(AnalogSignalChangeType.Units);
            }
        }

        /// <summary>
        /// Value of the analog signal
        /// </summary>
        public Double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnChanged(AnalogSignalChangeType.Value);
                OnValueChanged();
            }
        }

        /// <summary>
        /// Indicates whether or not the <see cref="AnalogSignal">AnalogSignal</see> is visible
        /// </summary>
        public Boolean Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnChanged(AnalogSignalChangeType.Visible);
                OnVisibleChanged();
            }
        }

        #endregion Properties

        #region Delegates and Events (5)

        #region Events (5)

        /// <summary>
        /// Occurs when any property of the analog signal changes
        /// </summary>
        public event EventHandler<AnalogSignalChangedEventArgs> Changed;

        /// <summary>
        /// Occurs when the <see cref="Visible">Visible</see> property of the analog signal changes
        /// </summary>
        public event EventHandler LabelChanged;

        /// <summary>
        /// Occurs when the <see cref="RawValue">RawValue</see> of the analog signal changes
        /// </summary>
        public event EventHandler RawValueChanged;

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> of the analog signal changes
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Occurs when the <see cref="Visible">Visible</see> property of the analog signal changes
        /// </summary>
        public event EventHandler VisibleChanged;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (11)

        #region Public Methods (2)

        /// <summary>
        /// Implicitely convers the <see cref="Value">Value</see> to a <see cref="Double">Double</see>
        /// </summary>
        /// <param name="x">AnalogSignal</param>
        /// <returns>Double (value of the <see cref="Value">Value</see> property)</returns>
        public static implicit operator Double(AnalogSignal x)
        {
            return (Double)(x.Value);
        }

        //// overload the == operator so comparing two Parameters actually compares the ProcessValues
        //public static Boolean operator >(AnalogSignalClass x, NumericParameter y)
        //{
        //    return (x.Value > y.ProcessValue);
        //}
        /// <summary>
        /// Implicitely convers the <see cref="Value">Value</see> to a <see cref="Single">Single</see>
        /// </summary>
        /// <param name="x">AnalogSignal</param>
        /// <returns>Single (value of the <see cref="Value">Value</see> property)</returns>
        public static implicit operator Single(AnalogSignal x)
        {
            return (Single)(x.Value);
        }

        #endregion Public Methods
        #region Protected Methods (5)

        /// <summary>
        /// Raises the <see cref="Changed">Changed</see> event
        /// </summary>
        /// <param name="Change">Indicates which property has changed</param>
        protected virtual void OnChanged(AnalogSignalChangeType Change)
        {
            if (Changed != null)
                Changed(this, new AnalogSignalChangedEventArgs(Change));
        }

        /// <summary>
        /// Raises the <see cref="VisibleChanged">VisibleChanged</see> event
        /// </summary>
        protected virtual void OnLabelChanged()
        {
            if (LabelChanged != null)
                LabelChanged(this, null);
        }

        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
        /// </summary>
        protected virtual void OnRawValueChanged()
        {
            if (RawValueChanged != null)
                RawValueChanged(this, null);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        /// <summary>
        /// Raises the <see cref="VisibleChanged">VisibleChanged</see> event
        /// </summary>
        protected virtual void OnVisibleChanged()
        {
            if (VisibleChanged != null)
                VisibleChanged(this, null);
        }

        #endregion Protected Methods
        #region Private Methods (4)

        private void _AnalogInput_RawValueChanged(object sender, EventArgs e)
        {
            this.RawValue = _AnalogInput.RawValue;
        }

        private void _AnalogSignal_ValueChanged(object sender, EventArgs e)
        {
            this.RawValue = _AnalogSignal.Value;
        }

        private void _FormattedAnalogInput_RawValueChanged(object sender, EventArgs e)
        {
            _rawValue = _FormattedAnalogInput.RawValue;
        }

        private void _FormattedAnalogInput_ValueChanged(object sender, EventArgs e)
        {
            _value = _FormattedAnalogInput.Value;
            _formattedValue = _FormattedAnalogInput.FormattedValue;
            OnChanged(AnalogSignalChangeType.Value);
            OnValueChanged();
        }

        #endregion Private Methods

        #endregion Methods
    }

    /// <summary>
    /// Event arguments for the <see cref="AnalogSignal.Changed">Changed</see> event
    /// </summary>
    public class AnalogSignalChangedEventArgs : EventArgs
    {
        #region Fields (1)

        #region Private Fields (1)

        private AnalogSignalChangeType _changeType;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Event arguments for the <see cref="AnalogSignal.Changed">Changed</see> event
        /// </summary>
        /// <param name="change">Indicates which property changed</param>
        public AnalogSignalChangedEventArgs(AnalogSignalChangeType change)
        {
            _changeType = change;
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Indicates which property changed
        /// </summary>
        public AnalogSignalChangeType ChangeType { get { return _changeType; } }

        #endregion Properties
    }

    /// <summary>
    /// Used to indicate which property is being referenced by the <see cref="AnalogSignal.Changed">Changed</see> event
    /// </summary>
    public enum AnalogSignalChangeType
    {
        /// <summary>
        /// Refers to the <see cref="AnalogSignal.Label">Label</see> property of the <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        Label,

        /// <summary>
        /// Refers to the <see cref="AnalogSignal.Value">Value</see> property of the <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        Value,

        /// <summary>
        /// Refers to the <see cref="AnalogSignal.RawValue">RawValue</see> property of the <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        RawValue,

        /// <summary>
        /// Refers to the <see cref="AnalogSignal.Units">Units</see> property of the <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        Units,

        /// <summary>
        /// Refers to the <see cref="AnalogSignal.Format">Format</see> property of the <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        Format,

        /// <summary>
        /// Refers to the <see cref="AnalogSignal.TurnRedLimit">TurnRedLimit</see> property of the <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        TurnRedLimit,

        /// <summary>
        /// Refers to the <see cref="AnalogSignal.Visible">Visible</see> property of the <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        Visible
    }
}