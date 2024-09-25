using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Diagnostics;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
  /// <summary>
  /// Serial Interface for a TerraNova 934 Controller
  /// </summary>
  public class TerraNova934 : SerialIOBase
  {
    #region Event Handlers

    /// <summary>
    /// Occurs when the <see cref="Value">Value</see> changes
    /// </summary>
    public override event EventHandler ValueChanged;
    /// <summary>
    /// Raises the <see cref="ValueChanged">ValueChanged</see> event
    /// </summary>
    protected override void OnValueChanged()
    {
      if (ValueChanged != null)
        ValueChanged(this, null);
    }

    /// <summary>
    /// Occurs when the <see cref="RawValue">RawValue</see> changes
    /// </summary>
    public override event EventHandler RawValueChanged;
    /// <summary>
    /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
    /// </summary>
    protected override void OnRawValueChanged()
    {
      if (RawValueChanged != null)
        RawValueChanged(this, null);
    }

    #endregion

    #region Globals

    private Boolean _DoNotCall;
    private Double _pressure;
    private Single _min, _max;
    private String _units;
    private String _format = "0.00";
    private Boolean _sp1Latch = true, _sp2Latch = true, _sp1ActiveHigh = true, _sp2ActiveHigh = false;
    private int _decimalPlaces = 3;
    private String[] pressureUnitNames = { "mbar", "Pa", "atm", "Torr" };
    private AnalogSignal pressureIonGauge, pressureSignalA, pressureSignalB;

    private Stopwatch _errorSW = new Stopwatch();

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="TerraNova934">TerraNova934</see> class
    /// </summary>
    public TerraNova934(Single Min, Single Max, String Units)
      : base()
    {
      _min = Min;
      _max = Max;
      _units = Units;
      this.BaudRate = 2400;
      this.SerialPort.ReadTimeout = 100000;
      pressureSignalA = new AnalogSignal("Gauge A Press", pressureUnitNames[3], "0.000E-0", 1000, false, true);
      pressureSignalB = new AnalogSignal("Gauge B Press", pressureUnitNames[3], "0.000E-0", 1000, false, true);
      pressureIonGauge = new AnalogSignal("Ion Gauge Press", pressureUnitNames[3], "0.000E-0", 1000, false, true);
      Format = "0.0000E-0";
    }

    #endregion

    #region Private Methods

    private void SendWithDelay(String Text)
    {
      if (!serialPort1.IsOpen) serialPort1.Open();
      for (int i = 0; i < Text.Length; i++) {
        serialPort1.Write(Text.Substring(i, 1));
        Thread.Sleep(25);
      }
    }

    private void SendConfig()
    {
      int iConfig;
      String sConfig;

      iConfig = 1 << _decimalPlaces;
      if (_sp1Latch) iConfig += 256;
      if (!_sp1ActiveHigh) iConfig += 512;
      if (_sp2Latch) iConfig += 1024;
      if (!_sp2ActiveHigh) iConfig += 2048;
      sConfig = String.Format(">{0:0}\r", iConfig);
      Monitor.Enter(this.SerialLock);
      try {
        serialPort1.DiscardInBuffer();
        SendWithDelay(sConfig);
        String s = serialPort1.ReadLine(); // throw away the return value
      }
      catch (Exception e) {
        //commError = true;
        VtiEvent.Log.WriteError("Error sending configuration to TerraNova 934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
      }
      finally {
        Monitor.Exit(this.SerialLock);
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Thread for reading the <see cref="Value">Value</see> of the TerraNova934 Controller
    /// </summary>
    public override void Process()
    {
      if (Monitor.TryEnter(this.SerialLock, 500)) {
        try {
          serialPort1.DiscardInBuffer();
          Thread.Sleep(300);
          pressureSignalA.Value = Pressure1;
          pressureSignalB.Value = Pressure2;
          pressureIonGauge.Value = IonGaugePressure;
          if (!this.backgroundWorker1.IsBusy)
            this.backgroundWorker1.RunWorkerAsync();    // use the BackgroundWorker thread to handle anything UI-related via the BackgroundProcess method
        }
        catch (Exception e) {
          pressureSignalA.Value = Double.NaN;
          pressureSignalB.Value = Double.NaN;
          pressureIonGauge.Value = Double.NaN;
          //commError = true;~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        }
        finally {
          Monitor.Exit(this.SerialLock);
        }
      }
    }

    /// <summary>
    /// Get status of Ion Gauge parameter
    /// </summary>
    public string GetIonGaugeParameter(int Index)
    {
      int ii;
      string strRead = "", strRead2 = "", strRead3 = "";
      if (Index < 1 || Index > 26)
        return "Index out of range";
      if (!_DoNotCall) {
        //_DoNotCall = true;
        serialPort1.DiscardInBuffer();
        Thread.Sleep(50);
        serialPort1.Write("E");
        if (IsAvailable) {
          try {
            //Actions.WaitForUpTo(1250, () => serialPort1.BytesToRead >= 1);
            DateTime dtStart = DateTime.Now;
            TimeSpan ts;
            int count = 0, numReads = 0, prevStrReadLength, numEmptyReads = 0;
            do {
              Thread.Sleep(50);
              prevStrReadLength = strRead.Length;
              strRead += serialPort1.ReadExisting();
              if (strRead.Length == prevStrReadLength)
                numEmptyReads++;
              else
                numEmptyReads = 0;
              count = 0;
              int i = 0, iPrev = -2;
              while ((i = strRead.IndexOf("\n\r", i)) != -1) {
                strRead3 = strRead.Substring(iPrev + 2, i - (iPrev + 2));
                count++;
                if (count == Index)
                  strRead2 = strRead.Substring(iPrev + 2, i - (iPrev + 2));
                iPrev = i;
                i += "\n\r".Length;
              }
              numReads++;
              ts = DateTime.Now - dtStart;
            } while (numEmptyReads < 3 && count < Index);
            if (count < Index)
              strRead2 = "Error parsing Ion Gauge Parameters";
            _DoNotCall = false;
          }
          catch (Exception e) {
            //commError = true;
            VtiEvent.Log.WriteError(
                String.Format("Error reading Ion Gauge Parameter while communicating with {0}.", this.Name),
                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                e.ToString());
            _DoNotCall = false;
            return "Error reading Ion Gauge Parameter";
          }
        }
      }
      return strRead2;
    }

    /// <summary>
    /// Start Ion Gauge; remember to allow time for stabilization
    /// </summary>
    /// 
    public bool IonGaugeStart()
    {
      byte[] retData = null;
      byte len = 0;
      if (!_DoNotCall) {
        //_DoNotCall = true;
        Thread.Sleep(50);
        serialPort1.DiscardInBuffer();
        serialPort1.Write("A");
        Thread.Sleep(50);
        if (IsAvailable) {
          try {
            Actions.WaitForUpTo(250, () => serialPort1.BytesToRead >= 1);
            string strRead = serialPort1.ReadLine();
            _DoNotCall = false;
          }
          catch (Exception e) {
            //commError = true;
            VtiEvent.Log.WriteError(
                String.Format("Error starting Ion Gauge while communicating with {0}.", this.Name),
                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                e.ToString());
            _DoNotCall = false;
            return false;
          }
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Stop Ion Gauge; remember to allow about 3 seconds to stop
    /// </summary>
    /// 
    public bool IonGaugeStop()
    {
      byte[] retData = null;
      byte len = 0;
      if (!_DoNotCall) {
        //_DoNotCall = true;
        Thread.Sleep(50);
        serialPort1.DiscardInBuffer();
        serialPort1.Write("B");
        Thread.Sleep(50);
        if (IsAvailable) {
          try {
            Actions.WaitForUpTo(250, () => serialPort1.BytesToRead >= 1);
            string strRead = serialPort1.ReadLine();
            _DoNotCall = false;
          }
          catch (Exception e) {
            //commError = true;
            VtiEvent.Log.WriteError(
                String.Format("Error stopping Ion Gauge while communicating with {0}.", this.Name),
                VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                e.ToString());
            _DoNotCall = false;
            return false;
          }
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Resets the setpoint relays
    /// </summary>
    public void ResetRelays()
    {
    }

    /// <summary>
    /// Calculates a new <see cref="Offset">Offset</see> based on the current <see cref="Value">Value</see>
    /// </summary>
    public void AutoZero()
    {
      //            this.Offset = this.Offset - (Single)this.Value / this.Gain;
    }

    #endregion

    #region Events

    /// <summary>
    /// When called, this method invokes the <see cref="OnValueChanged">OnValueChanged</see>
    /// method on the main thread.
    /// </summary>
    public override void BackgroundProcess()
    {
      OnValueChanged();
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets current Pressure1 from TerraNova 934
    /// </summary>
    public float Pressure1
    {
      get
      {
        byte[] retData = null;
        byte len = 0;
        float fRet = 0.0f;
        if (!_DoNotCall) {
          //_DoNotCall = true;
          Thread.Sleep(50);
          serialPort1.DiscardInBuffer();
          serialPort1.Write("G");
          Thread.Sleep(50);
          if (IsAvailable) {
            try {
              Actions.WaitForUpTo(250, () => serialPort1.BytesToRead >= 1);
              string strRead = serialPort1.ReadLine();
              _DoNotCall = false;
              string strMan, strExp, strRet;
              try {
                strMan = strRead.Substring(0, 2);
                strExp = strRead.Substring(2);
                float fMan = Convert.ToSingle(strMan), fExp = Convert.ToInt32(strExp);
                strRet = String.Format("{0:0.0}E{1:0}", fMan, fExp);
                fRet = Convert.ToSingle(strRet);
              }
              catch (Exception e) {
                _DoNotCall = false;
              }
              if (fRet == 991)
                fRet = 1000;
              else
                return fRet;
            }
            catch (Exception e) {
              //commError = true;
              VtiEvent.Log.WriteError(
                  String.Format("Error reading Pressure1 while communicating with {0}.", this.Name),
                  VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                  e.ToString());
              _DoNotCall = false;
              return -1f;
            }
            return fRet;
          }
        }
        return -1f;
      }
    }

    /// <summary>
    /// Gets current Pressure2 from TerraNova 934
    /// </summary>
    public float Pressure2
    {
      get
      {
        byte[] retData = null;
        byte len = 0;
        float fRet = 0.0f;
        if (!_DoNotCall) {
          //_DoNotCall = true;
          Thread.Sleep(50);
          serialPort1.DiscardInBuffer();
          serialPort1.Write("H");
          Thread.Sleep(50);
          if (IsAvailable) {
            try {
              Actions.WaitForUpTo(250, () => serialPort1.BytesToRead >= 1);
              string strRead = serialPort1.ReadLine();
              _DoNotCall = false;
              string strMan, strExp, strRet;
              try {
                strMan = strRead.Substring(0, 2);
                strExp = strRead.Substring(2);
                float fMan = Convert.ToSingle(strMan), fExp = Convert.ToInt32(strExp);
                strRet = String.Format("{0:0.0}E{1:0}", fMan, fExp);
                fRet = Convert.ToSingle(strRet);
              }
              catch (Exception e) {
                _DoNotCall = false;
              }
              if (fRet == 991)
                fRet = 1000;
              else
                return fRet;
            }
            catch (Exception e) {
              //commError = true;
              VtiEvent.Log.WriteError(
                  String.Format("Error reading Pressure2 while communicating with {0}.", this.Name),
                  VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                  e.ToString());
              _DoNotCall = false;
              return -1f;
            }
            return fRet;
          }
        }
        return -1f;
      }
    }

    /// <summary>
    /// Gets current Ion Gauge Pressure from TerraNova 934
    /// </summary>
    public float IonGaugePressure
    {
      get
      {
        byte[] retData = null;
        byte len = 0;
        float fRet = 0.0f;
        if (!_DoNotCall) {
          //_DoNotCall = true;
          Thread.Sleep(50);
          serialPort1.DiscardInBuffer();
          serialPort1.Write("F");
          Thread.Sleep(50);
          if (IsAvailable) {
            try {
              string strRead = "", strRead2 = "";
              try {
                do {
                  strRead = strRead2;
                  strRead2 = serialPort1.ReadLine();
                } while (strRead2.Length > 0);
              }
              catch (Exception e) {
              }
              _DoNotCall = false;
              string strMan, strExp, strRet;
              try {
                strMan = strRead.Substring(0, 2);
                strExp = strRead.Substring(2);
                float fMan = Convert.ToSingle(strMan), fExp = Convert.ToInt32(strExp);
                strRet = String.Format("{0:0.0}E{1:0}", fMan, fExp);
                fRet = Convert.ToSingle(strRet);
                if (fRet > 1)
                  fRet = fRet;
              }
              catch (Exception e) {
                _DoNotCall = false;
              }
              if (fRet == 991)
                fRet = 1000;
              else
                return fRet;
            }
            catch (Exception e) {
              //commError = true;
              VtiEvent.Log.WriteError(
                  String.Format("Error reading Ion Gauge pressure while communicating with {0}.", this.Name),
                  VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO,
                  e.ToString());
              _DoNotCall = false;
              return -1f;
            }
            return fRet;
          }
        }
        return -1f;
      }
    }

    /// <summary>
    /// Value (pressure) of the TerraNova 934 Controller
    /// </summary>
    public override double Value
    {
      get
      {
        return _pressure;
      }
      internal set
      {
        _pressure = value;
        OnValueChanged();
      }
    }

    /// <summary>
    /// Formatted value including the <see cref="Units">Units</see>
    /// </summary>
    public override string FormattedValue
    {
      get
      {
        if (Double.IsNaN(this.Value))
          return "ERROR";
        else
          return this.Value.ToString(_format) + " " + this.Units;
      }
    }

    /// <summary>
    /// Name for the TerraNova 934 Controller
    /// </summary>
    public override string Name
    {
      get { return "TerraNova934 Controller on port " + this.PortName; }
    }

    /// <summary>
    /// Not implemented
    /// </summary>
    public override double RawValue
    {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    /// <summary>
    /// Gets the Ion Gauge pressure 
    /// </summary>
    public Double PressureIG
    {
      get
      {
        return pressureIonGauge;
      }
    }

    /// <summary>
    /// Gets the current pressure on Gauge A
    /// </summary>
    public Double PressureA
    {
      get
      {
        return pressureSignalA;
      }
    }

    /// <summary>
    /// Gets the current pressure on Gauge B
    /// </summary>
    public Double PressureB
    {
      get
      {
        return pressureSignalB;
      }
    }

    /// <summary>
    /// Ion Gauge Pressure as an AnalogSignal
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AnalogSignal PressureIonGauge
    {
      get { return pressureIonGauge; }
    }

    /// <summary>
    /// Pressure Signal A as an AnalogSignal
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AnalogSignal PressureSignalA
    {
      get { return pressureSignalA; }
    }

    /// <summary>
    /// Pressure Signal B as an AnalogSignal
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AnalogSignal PressureSignalB
    {
      get { return pressureSignalB; }
    }

    /// <summary>
    /// Minimum value for the TerraNova 934 Controller
    /// </summary>
    public override double Min
    {
      get { return _min; }
    }

    /// <summary>
    /// Maximum value for the TerraNova 934 controller
    /// </summary>
    public override double Max
    {
      get { return _max; }
    }

    /// <summary>
    /// Units for the value for the TerraNova934 controller
    /// </summary>
    public override string Units
    {
      get { return _units; }
      set { _units = value; }
    }

    /// <summary>
    /// Format string for the value for the TerraNova934 controller
    /// </summary>
    public override string Format
    {
      get
      {
        return _format;
      }
      set
      {
        _format = value;
      }
    }
    /*
        /// <summary>
        /// Setpoint 1 of the TerraNova934 controller
        /// </summary>
        public Single Setpoint1
        {
          get
          {
            Single temp = Single.NaN;
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              serialPort1.Write("1");
              temp = Convert.ToSingle(serialPort1.ReadLine().Substring(4));
            }
            catch (Exception e)
            {
              commError = true;
              VtiEvent.Log.WriteError("Error reading Setpoint 1 from TerraNova934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
            return temp;
          }
          set
          {
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              SendWithDelay(string.Format("!{0:0.000}\r", value));
              String s = serialPort1.ReadLine(); // throw away the return value
            }
            catch
            {
              commError = true;
              throw new Exception("Error setting Setpoint 1 on TerraNova934 Controller on port " + serialPort1.PortName);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
          }
        }

        /// <summary>
        /// Setpoint 2 of the TerraNova934 controller
        /// </summary>
        public Single Setpoint2
        {
          get
          {
            Single temp;
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              serialPort1.Write("2");
              temp = Convert.ToSingle(serialPort1.ReadLine().Substring(4));
            }
            catch
            {
              commError = true;
              throw new Exception("Error reading Setpoint 2 from TerraNova934 Controller on port " + serialPort1.PortName);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
            return temp;
          }
          set
          {
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              SendWithDelay(string.Format("@{0:0.000}\r", value));
              String s = serialPort1.ReadLine(); // throw away the return value
            }
            catch
            {
              commError = true;
              throw new Exception("Error setting Setpoint 2 on TerraNova934 Controller on port " + serialPort1.PortName);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
          }
        }

        /// <summary>
        /// Gain setting for the TerraNova934 controller
        /// </summary>
        public Single Gain
        {
          get
          {
            Single Value = Single.NaN;
            Monitor.Enter(this.SerialLock);
            try
            {
              //if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              serialPort1.Write("g");
              Value = Convert.ToSingle(serialPort1.ReadLine().Substring(2));
            }
            catch (Exception e)
            {
              commError = true;
              VtiEvent.Log.WriteError("Error reading Gain from TerraNova934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }

            return Value;
          }
          set
          {
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              SendWithDelay(string.Format("G{0:0.000}\r", value));
              String s = serialPort1.ReadLine(); // throw away the return value
            }
            catch (Exception e)
            {
              commError = true;
              VtiEvent.Log.WriteError("Error setting Gain on TerraNova934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
          }
        }

        /// <summary>
        /// Offset setting for the TerraNova934 controller
        /// </summary>
        public Single Offset
        {
          get
          {
            Single temp = Single.NaN;
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              serialPort1.Write("o");
              temp = Convert.ToSingle(serialPort1.ReadLine().Substring(2));
            }
            catch (Exception e)
            {
              commError = true;
              VtiEvent.Log.WriteError("Error reading Offset from TerraNova934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
            return temp;
          }
          set
          {
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              SendWithDelay(string.Format("O{0:0.000}\r", value));
              String s = serialPort1.ReadLine(); // throw away the return value
            }
            catch (Exception e)
            {
              commError = true;
              VtiEvent.Log.WriteError("Error setting Offset on TerraNova934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
          }
        }

        /// <summary>
        /// Dac setting for the TerraNova934 controller
        /// </summary>
        public Single Dac
        {
          get
          {
            Single Value = Single.NaN;
            Monitor.Enter(this.SerialLock);
            try
            {
              //if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              serialPort1.Write("d");
              Value = Convert.ToSingle(serialPort1.ReadLine().Substring(2));
            }
            catch (Exception e)
            {
              commError = true;
              VtiEvent.Log.WriteError("Error reading Dac from TerraNova934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }

            return Value;
          }
          set
          {
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              SendWithDelay(string.Format("D{0:0}\r", value));
              String s = serialPort1.ReadLine(); // throw away the return value
            }
            catch (Exception e)
            {
              commError = true;
              VtiEvent.Log.WriteError("Error setting Dac on TerraNova934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
          }
        }

        /// <summary>
        /// RelayStatus of the TerraNova934 Controller
        /// </summary>
        /// <remarks>
        /// The Relay Status is a two character string with each character being either a
        /// 0 or a 1 to indicate the status of each relay.
        /// </remarks>
        public String RelayStatus
        {
          get
          {
            String s = string.Empty;
            Monitor.Enter(this.SerialLock);
            try
            {
              if (!serialPort1.IsOpen) serialPort1.Open();
              serialPort1.DiscardInBuffer();
              serialPort1.Write("r");
              s = serialPort1.ReadLine();
            }
            catch (Exception e)
            {
              commError = true;
              VtiEvent.Log.WriteError("Error reading Relay Status from TerraNova934 Controller on port " + serialPort1.PortName, VTIWindowsControlLibrary.Enums.VtiEventCatType.Serial_IO, e.Message);
            }
            finally
            {
              Monitor.Exit(this.SerialLock);
            }
            return s;
          }
        }

        /// <summary>
        /// Indicates whether or not Setpoint 1 will latch when hit
        /// </summary>
        public Boolean SP1Latch
        {
          get { return _sp1Latch; }
          set
          {
            _sp1Latch = value;
            if (!this.DesignMode) this.SendConfig();
          }
        }

        /// <summary>
        /// Indicates whether or not Setpoint 2 will latch when hit
        /// </summary>
        public Boolean SP2Latch
        {
          get { return _sp2Latch; }
          set
          {
            _sp2Latch = value;
            if (!this.DesignMode) this.SendConfig();
          }
        }

        /// <summary>
        /// Indicates if Setpoint 1 is active when the presure is above the setpoint
        /// </summary>
        /// <remarks>
        /// <para>If <see cref="SP1ActiveHigh">SP1ActiveHigh</see> is true, the Setpoint 1 relay will
        /// turn on if the pressure is above the setpoint.</para>
        /// <para>If <see cref="SP1ActiveHigh">SP1ActiveHigh</see> is false, the Setpoint 1 relay will
        /// turn on if the pressure is below the setpoint.</para>
        /// </remarks>
        public Boolean SP1ActiveHigh
        {
          get { return _sp1ActiveHigh; }
          set
          {
            _sp1ActiveHigh = value;
            if (!this.DesignMode) this.SendConfig();
          }
        }

        /// <summary>
        /// Indicates if Setpoint 2 is active when the presure is above the setpoint
        /// </summary>
        /// <remarks>
        /// <para>If <see cref="SP2ActiveHigh">SP2ActiveHigh</see> is true, the Setpoint 2 relay will
        /// turn on if the pressure is above the setpoint.</para>
        /// <para>If <see cref="SP2ActiveHigh">SP2ActiveHigh</see> is false, the Setpoint 2 relay will
        /// turn on if the pressure is below the setpoint.</para>
        /// </remarks>
        public Boolean SP2ActiveHigh
        {
          get { return _sp2ActiveHigh; }
          set
          {
            _sp2ActiveHigh = value;
            if (!this.DesignMode) this.SendConfig();
          }
        }

        /// <summary>
        /// Number of decimal places for the TerraNova934 controller to display
        /// </summary>
        public int DecimalPlaces
        {
          get { return _decimalPlaces; }
          set
          {
            _decimalPlaces = value;
            if (_decimalPlaces < 0) _decimalPlaces = 0;
            if (_decimalPlaces > 3) _decimalPlaces = 3;
            if (!this.DesignMode) this.SendConfig();
          }
        }
    */
    #endregion
  }
}
