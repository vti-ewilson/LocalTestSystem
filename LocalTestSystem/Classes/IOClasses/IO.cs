using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using LocalTestSystem.Classes.Configuration;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;
using VTIWindowsControlLibrary.Classes.IO.SignalConverters;
using VTIWindowsControlLibrary.Components;

namespace LocalTestSystem.Classes.IOClasses
{
    /// <summary>
    /// IO
    /// 
    /// 
    /// </summary>
    public class IO : GenericSingleton<IO>
    {
        #region Fields (6)

        #region Protected Fields (6)

        protected AnalogInputs _AIn;
        protected AnalogOutputs _AOut;
        protected DigitalInputs _DIn;
        protected DigitalOutputs _DOut;
        protected SerialInputs _SerialIn;
        protected AnalogSignalConverters _SignalConverters;
        protected AnalogSignals _Signals;
        protected EthernetInputs _EthernetIn;

        #endregion Protected Fields

        #endregion Fields

        #region Constructors (1)

        protected IO() { }

        #endregion Constructors

        #region Properties (6)

        public static AnalogInputs AIn { get { return Instance._AIn; } }

        public static AnalogOutputs AOut { get { return Instance._AOut; } }
        public static DigitalInputs DIn { get { return Instance._DIn; } }

        public static DigitalOutputs DOut { get { return Instance._DOut; } }

        public static SerialInputs SerialIn { get { return Instance._SerialIn; } }

        public static AnalogSignalConverters SignalConverters { get { return Instance._SignalConverters; } }

        public static AnalogSignals Signals { get { return Instance._Signals; } }
        public static EthernetInputs EthernetIn { get { return Instance._EthernetIn; } }

        #endregion Properties

        #region Methods (2)

        #region Public Methods (1)

        public static void Initialize()
        {
            Instance = new IO();
        }

        #endregion Public Methods
        #region Protected Methods (1)

        protected override void InitializeInstance()
        {
            VtiEvent.Log.WriteVerbose("Initializing I/O Interface...");

            _AIn = new AnalogInputs(Config.IO.Interface);
            _AOut = new AnalogOutputs(Config.IO.Interface);
            _SerialIn = new SerialInputs();
            _EthernetIn = new EthernetInputs();
            _SignalConverters = new AnalogSignalConverters();
            _Signals = new AnalogSignals();
            _DIn = new DigitalInputs(Config.IO.Interface);
            _DOut = new DigitalOutputs(Config.IO.Interface);
        }

        public void RestartSerialInDevices()
        {
            var _SerialInList = _SerialIn.GetType()
                     .GetFields()
                     .Select(field => field.GetValue(_SerialIn))
                     .Where(x => x != null)
                     .OfType<VTIWindowsControlLibrary.Classes.IO.SerialIO.SerialIOBase>()
                     .ToList();
            if (_SerialInList != null)
            {
                foreach (VTIWindowsControlLibrary.Classes.IO.SerialIO.SerialIOBase device in _SerialInList)
                {
                    device.Start();
                }
            }
        }

        //public void RestartEthernetInDevices()
        //{
        //    var _EthernetInList = _EthernetIn.GetType()
        //             .GetFields()
        //             .Select(field => field.GetValue(_EthernetIn))
        //             .Where(x => x != null)
        //             .OfType<VTIWindowsControlLibrary.Classes.IO.EthernetIO.EthernetIOBase>()
        //             .ToList();
        //    if (_EthernetInList != null)
        //    {
        //        foreach (VTIWindowsControlLibrary.Classes.IO.EthernetIO.EthernetIOBase device in _EthernetInList)
        //        {
        //            device.Start();
        //        }
        //    }
        //}

        #endregion Protected Methods

        #endregion Methods
    }
}