using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Provides the path to the I/O Interface assembly (example, VtiMCCInterface.DLL)
    /// Can be implemented in an ApplicationSettingsBase-derived class to allow the
    /// InterfaceDLL property to be specified in the user.config file.
    /// </summary>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class IOInterface : IIoConfig
    {
        #region Globals

        private String _Name;
        private String _InterfaceDLL;
        private String _InterfaceDLLFullPath;
        private System.Reflection.Assembly _asm;
        private IIoConfig _IoConfig;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="IOInterface">IOInterface</see> class
        /// </summary>
        public IOInterface()
        {
        }

        #endregion Construction

        #region Private Members

        /// <summary>
        /// SetIoConfig Method
        ///
        /// Finds the type (class) within the IO Interface Assembly that implements the IIoConfig interface,
        /// then retrieves the "Default" property from this class and stores it in the local _IoConfig field.
        /// </summary>
        private void SetIoConfig()
        {
            // Search through the classes within the IO Interface Assembly
            //foreach (Type t in _asm.GetTypes())
            //    // Find the one that implements the IIoConfig interface
            //    if (t.GetInterface("IIoConfig", true) != null)
            //    {
            //        // retrieve the "Default" property
            //        _IoConfig = t.UnderlyingSystemType.GetProperty("Default").GetValue(null, null) as IIoConfig;
            //        break; // no sense looking further
            //    }

            _IoConfig =
                _asm.GetTypes().First(t => t.GetInterface("IIoConfig", true) != null)
                    .UnderlyingSystemType.GetProperty("Default").GetValue(null, null) as IIoConfig;
        }

        #endregion Private Members

        #region Properties

        /// <summary>
        /// Name of the I/O Interface
        /// </summary>
        [XmlElement("Name")]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// When Set, this property attempts to locate the I/O Interface DLL,
        /// first in the application startup path, or a path specified in the
        /// InterfaceDLL string, then in C:\Windows\System32.
        /// If unable to locate the library, an error occurs.  If the library
        /// is located, it attempts to load the assembly and retrieve the I/O Configuration.
        /// </summary>
        [XmlElement("InterfaceDLL")]
        public String InterfaceDLL
        {
            get { return _InterfaceDLL; }
            set
            {
                String strInterface;
                _InterfaceDLL = value;
                strInterface = _InterfaceDLL;
                // Check to see if the InterfaceDLL exists in the application startup path (or specified path)
                FileInfo fileInfo = new FileInfo(strInterface);
                if (!fileInfo.Exists)
                {
                    // Look in the IOInterface directory within the application startup path
                    strInterface = fileInfo.DirectoryName;
                    if (!strInterface.EndsWith(@"\")) strInterface += @"\";
                    strInterface += @"IOInterface\" + fileInfo.Name;
                    fileInfo = new FileInfo(strInterface);
                    if (!fileInfo.Exists)
                    {
                        // Check to see if the InterfaceDLL exists in C:\Windows\System32
                        strInterface = @"C:\Windows\System32\" + fileInfo.Name;
                        fileInfo = new FileInfo(strInterface);
                        if (!fileInfo.Exists)
                            throw new Exception(String.Format("Unable to locate I/O Interface: {0}", _InterfaceDLL));
                    }
                }
                // Save the full pathname
                _InterfaceDLLFullPath = fileInfo.FullName;

                // Attempt to load the assembly
                try
                {
                    _asm = System.Reflection.Assembly.LoadFile(_InterfaceDLLFullPath);
                    this.SetIoConfig();
                }
                // Report any errors
                catch (Exception e)
                {
                    VtiEvent.Log.WriteError(e.ToString(), VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error);
                }
            }
        }

        /// <summary>
        /// Returns the full path to the I/O Interface DLL
        /// </summary>
        [XmlIgnore()]
        public String InterfaceDLLFullPath
        {
            get { return _InterfaceDLLFullPath; }
        }

        /// <summary>
        /// Returns the assembly of the I/O Interface DLL
        /// </summary>
        [XmlIgnore()]
        public System.Reflection.Assembly Assembly
        {
            get { return _asm; }
        }

        #endregion Properties

        #region IIoConfig Members

        /// <summary>
        /// Starts the I/O Processing Thread
        /// </summary>
        public void Start()
        {
            VtiEvent.Log.WriteVerbose("Starting I/O Processing Thread...");
            try
            {
                _IoConfig.Start();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error starting I/O Processing Thread.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error, e.ToString());
            }
        }

        /// <summary>
        /// Stops the I/O Processing Thread
        /// </summary>
        public void Stop()
        {
            VtiEvent.Log.WriteVerbose("Stopping I/O Processing Thread...");
            try
            {
                _IoConfig.Stop();
            }
            catch (Exception e)
            {
                VtiEvent.Log.WriteError("Error stopping I/O Processing Thread.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error, e.ToString());
            }
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IAnalogInput">AnalogInputs</see> from the I/O Interface
        /// </summary>
        [XmlIgnore()]
        public Dictionary<string, IAnalogInput> AnalogInputs
        {
            get { return _IoConfig.AnalogInputs; }
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IAnalogOutput">AnalogOutputs</see> from the I/O Interface
        /// </summary>
        [XmlIgnore()]
        public Dictionary<string, IAnalogOutput> AnalogOutputs
        {
            get { return _IoConfig.AnalogOutputs; }
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IDigitalInput">DigitalInputs</see> from the I/O Interface
        /// </summary>
        [XmlIgnore()]
        public Dictionary<string, IDigitalInput> DigitalInputs
        {
            get { return _IoConfig.DigitalInputs; }
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IDigitalOutput">DigitalOutputs</see> from the I/O Interface
        /// </summary>
        [XmlIgnore()]
        public Dictionary<string, IDigitalOutput> DigitalOutputs
        {
            get { return _IoConfig.DigitalOutputs; }
        }

        /// <summary>
        /// Gets the thread that processes the I/O
        /// </summary>
        public Thread ProcessThread
        {
            get
            {
                if (_IoConfig == null) return null;
                else return _IoConfig.ProcessThread;
            }
        }

        #endregion IIoConfig Members

        /// <summary>
        /// Turns off all of the <see cref="IDigitalOutput">DigitalOutputs</see> from the I/O Interface
        /// </summary>
        public void TurnAllOff()
        {
            if (_IoConfig != null)
                _IoConfig.TurnAllOff();
        }
        /// <summary>
        /// !NOT TESTED YET! Reads all of the <see cref="IDigitalOutput">DigitalOutput</see> bit values from the PLC and assigns those values to the Digital Outputs' values in the PC software.
        /// This is a property and will turn false when the PLC thread has finished reading in the values.
        /// </summary>
        public bool ReadPLCDigOutputsIntoPC
        {
            get
            {
                if (_IoConfig != null)
                {
                    return _IoConfig.ReadPLCDigOutputsIntoPC;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (_IoConfig != null)
                {
                    _IoConfig.ReadPLCDigOutputsIntoPC = value;
                }
            }
        }
    }
}