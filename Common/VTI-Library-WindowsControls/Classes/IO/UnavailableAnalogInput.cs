using System;
using System.Reflection;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Used when an analog input specified in the IO.cs class of the client application
    /// can't be located in the <see cref="IOInterface">IOInterface</see>.
    /// </summary>
    public class UnavailableAnalogInput : IAnalogInput
    {
        #region Event Handlers

        /// <summary>
        /// Implements the <see cref="IAnalogInput.RawValueChanged">RawValueChanged</see>
        /// event of the <see cref="IAnalogInput">IAnalogInput</see> interface, but it will
        /// never be called in an <see cref="UnavailableAnalogInput">UnavailableAnalogInput</see>.
        /// </summary>
        public event EventHandler RawValueChanged;

        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event, but it will
        /// never be called in an <see cref="UnavailableAnalogInput">UnavailableAnalogInput</see>.
        /// </summary>
        protected virtual void OnRawValueChanged()
        {
            if (RawValueChanged != null)
                RawValueChanged(this, null);
        }

        #endregion Event Handlers

        #region Globals

        private string _Name;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="UnavailableAnalogInput">UnavailableAnalogInput</see> class
        /// </summary>
        /// <param name="Name">Name of the input</param>
        public UnavailableAnalogInput(string Name)
        {
            _Name = Name;
        }

        #endregion Construction

        #region Public Properties

        /// <summary>
        /// Name of the analog input
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// Returns Double.NaN and writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
        /// </summary>
        public double RawValue
        {
            get
            {
                VtiEvent.Log.WriteWarning("I/O Warning: Attempted to read RawValue of unavailable analog input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
                return double.NaN;
            }
        }

        /// <summary>
        /// Returns Double.NaN and writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
        /// </summary>
        public double Min
        {
            get
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
                }
                return double.NaN;
            }
        }

        /// <summary>
        /// Returns Double.NaN and writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
        /// </summary>
        public double Max
        {
            get
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
                }
                return double.NaN;
            }
        }

        /// <summary>
        /// Returns String.Empty and writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
        /// </summary>
        public string Units
        {
            get
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns String.Empty and writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
        /// </summary>
        public string Format
        {
            get
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
                }
                return string.Empty;
            }
            set 
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
                }
            }
        }

        /// <summary>
        /// Returns false to indicate that the input is not available
        /// </summary>
        public bool IsAvailable
        {
            get { return false; }
        }

        #endregion Public Properties
    }
}