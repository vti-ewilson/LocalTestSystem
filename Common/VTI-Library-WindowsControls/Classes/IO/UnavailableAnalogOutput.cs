using System.Reflection;
using System;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Used when an analog output specified in the IO.cs class
    /// can't be located in the I/O Interface Config.
    /// </summary>
    internal class UnavailableAnalogOutput : IAnalogOutput
    {
        #region Globals

        private string _Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnavailableAnalogOutput">UnavailableAnalogOutput</see> class
        /// </summary>
        /// <param name="Name">Name of the input</param>
        public UnavailableAnalogOutput(string Name)
        {
            _Name = Name;
        }

        #endregion Globals

        #region IAnalogOutput Members

        /// <summary>
        /// Name of the analog output
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// Returns Double.NaN and writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
        /// </summary>
        public double Value
        {
            get
            {
                VtiEvent.Log.WriteWarning("I/O Warning: Attempted to read Value of unavailable analog output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
                return double.NaN;
            }
            set
            {
                VtiEvent.Log.WriteWarning("I/O Warning: Attempted to set Value of unavailable analog output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
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
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
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
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
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
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
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
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
                }
                return string.Empty;
            }
            set 
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable analog output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Analog_IO);
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

        #endregion IAnalogOutput Members
    }
}