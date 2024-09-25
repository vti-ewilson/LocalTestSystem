using System;
using System.Reflection;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Used when an digital input specified in the IO.cs class
    /// can't be located in the I/O Interface Config.
    /// </summary>
    public class UnavailableDigitalInput : IDigitalInput
    {
        #region Event Handlers

        /// <summary>
        /// Implements the <see cref="IDigitalIO.ValueChanged">ValueChanged</see>
        /// event of the <see cref="IDigitalIO">IDigitalIO</see> interface, but it will
        /// never be called in an <see cref="UnavailableDigitalInput">UnavailableDigitalInput</see>.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event, but it will
        /// never be called in an <see cref="UnavailableDigitalInput">UnavailableDigitalInput</see>.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        void IDigitalIO.OnValueChanged()
        {
            OnValueChanged();
        }

        #endregion Event Handlers

        #region Globals

        private string _Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnavailableDigitalInput">UnavailableDigitalInput</see> class
        /// </summary>
        /// <param name="Name">Name of the input</param>
        public UnavailableDigitalInput(string Name)
        {
            _Name = Name;
        }

        #endregion Globals

        #region Public Properties

        /// <summary>
        /// Name of the analog input
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// Returns String.Empty and writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
        /// </summary>
        public string Description
        {
            get
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
                }
                return string.Empty;
            }
            set { }
        }

        /// <summary>
        /// Returns false and writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>.
        /// </summary>
        public bool Value
        {
            get
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
                }
                return false;
            }
            set
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital input '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
                }
            }
        }

        /// <summary>
        /// Returns true to indicate that this is an input
        /// </summary>
        public bool IsInput
        {
            get { return true; }
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