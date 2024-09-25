using System;
using System.Reflection;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Used when an digital output specified in the IO.cs class
    /// can't be located in the I/O Interface Config.
    /// </summary>
    public class UnavailableDigitalOutput : IDigitalOutput
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

        /// <summary>
        /// Implements the <see cref="Interfaces.IDigitalOutput.ValueChanging">ValueChanging</see>
        /// event of the <see cref="Interfaces.IDigitalIO">IDigitalOutput</see> interface, but it will
        /// never be called in an <see cref="UnavailableDigitalInput">UnavailableDigitalInput</see>.
        /// </summary>
        public event DigitalOutputChangingEventHandler ValueChanging;

        /// <summary>
        /// Raises the <see cref="ValueChanging">ValueChanging</see> event, but it will
        /// never be called in an <see cref="UnavailableDigitalInput">UnavailableDigitalInput</see>.
        /// </summary>
        public void OnValueChanging(DigitalOutputChangingEventArgs e)
        {
            if (ValueChanging != null)
                ValueChanging(this, null);
        }

        #endregion Event Handlers

        #region Globals

        private string _Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnavailableDigitalOutput">UnavailableDigitalOutput</see> class
        /// </summary>
        /// <param name="Name">Name for the <see cref="UnavailableDigitalOutput">UnavailableDigitalOutput</see></param>
        public UnavailableDigitalOutput(string Name)
        {
            _Name = Name;
        }

        #endregion Globals

        #region Public Methods

        /// <summary>
        /// Writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
        /// since the output is unavailable
        /// </summary>
        public void TurnOn()
        {
            if (VTIPLCInterfaceAccessMethods.PLCEnabled())
            {
                VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
            }
        }

        /// <summary>
        /// Writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
        /// since the output is unavailable
        /// </summary>
        public void TurnOff()
        {
            if (VTIPLCInterfaceAccessMethods.PLCEnabled())
            {
                VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
            }
        }

        /// <summary>
        /// Writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
        /// since the output is unavailable
        /// </summary>
        public void Enable()
        {
            if (VTIPLCInterfaceAccessMethods.PLCEnabled())
            {
                VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
            }
        }

        /// <summary>
        /// Writes a warning to the <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
        /// since the output is unavailable
        /// </summary>
        public void Disable()
        {
            if (VTIPLCInterfaceAccessMethods.PLCEnabled())
            {
                VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
            }
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Name of the digital output
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
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
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
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
                }
                return false;
            }
            set 
            {
                if (VTIPLCInterfaceAccessMethods.PLCEnabled())
                {
                    VtiEvent.Log.WriteWarning("I/O Warning: Attempted to access unavailable digital output '" + _Name + "'.", VTIWindowsControlLibrary.Enums.VtiEventCatType.Digital_IO);
                }
            }
        }

        /// <summary>
        /// Returns false to indicate that this is an output
        /// </summary>
        public bool IsInput
        {
            get { return false; }
        }

        /// <summary>
        /// Returns false to indicate that the value is unavailable
        /// </summary>
        public bool IsAvailable
        {
            get { return false; }
        }

        #endregion Public Properties
    }
}