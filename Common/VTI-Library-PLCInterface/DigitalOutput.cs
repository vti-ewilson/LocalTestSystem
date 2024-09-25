using System;
using System.Collections.Generic;
using System.Text;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.IO;

namespace VTIPLCInterface
{
    /// <summary>
    /// DigitalOutput Class
    /// 
    /// Implements the IDigitalOutput interface
    /// Links to an instance of a DigitalPortBit
    /// </summary>
    public class DigitalOutput : IDigitalOutput
    {
        #region Globals

        private DigitalPortBit _DigitalPortBit;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalOutput"/> class.
        /// </summary>
        /// <param name="Bit">The bit.</param>
        public DigitalOutput(DigitalPortBit Bit)
        {
            _DigitalPortBit = Bit;

            _DigitalPortBit.ValueChanged += new EventHandler(_DigitalPortBit_ValueChanged);
        }

        #endregion

        #region Events

        void _DigitalPortBit_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name of the I/O.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _DigitalPortBit.Name; }
        }

        /// <summary>
        /// Description of the Digital I/O
        /// </summary>
        /// <value></value>
        public string Description
        {
            get { return _DigitalPortBit.Description; }
            set { _DigitalPortBit.Description = value; }
        }

        /// <summary>
        /// Value of the Digital I/O
        /// </summary>
        /// <value></value>
        public bool Value
        {
            get { return _DigitalPortBit.Value; }
            set 
            {
                DigitalOutputChangingEventArgs eventArgs = new DigitalOutputChangingEventArgs(this, value);
                OnValueChanging(eventArgs);
                if (!eventArgs.Cancel)
                    _DigitalPortBit.Value = value;
                else
                    throw new DigitalOutputChangeCanceledException(eventArgs.Reason);
            }
        }

        /// <summary>
        /// Turns on the Digital Output
        /// </summary>
        public void TurnOn()
        {
            this.Value = true;
        }

        /// <summary>
        /// Turns off the Digital Output
        /// </summary>
        public void TurnOff()
        {
            this.Value = false;
        }

        /// <summary>
        /// Turns on the Digital Output
        /// </summary>
        public void Enable()
        {
            this.Value = true;
        }

        /// <summary>
        /// Turns off the Digital Output
        /// </summary>
        public void Disable()
        {
            this.Value = false;
        }

        #endregion

        #region IDigitalOutput Members


        /// <summary>
        /// Occurs when the value of the Digital I/O changes
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        public virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        /// <summary>
        /// Occurs when the value of the Digital I/O is about to change
        /// </summary>
        public event DigitalOutputChangingEventHandler ValueChanging;

        /// <summary>
        /// Raises the <see cref="ValueChanging">ValueChanging</see> event
        /// </summary>
        /// <param name="e"></param>
        public void OnValueChanging(DigitalOutputChangingEventArgs e)
        {
            if (ValueChanging != null)
                ValueChanging(this, e);
        }

        #endregion

        #region IDigitalIO Members


        /// <summary>
        /// Indicates if the Digital I/O is an input
        /// </summary>
        /// <value></value>
        public bool IsInput
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether this I/O instance is available.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this I/O instance is available; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailable
        {
            get { return true; }
        }

        #endregion

    }
}
