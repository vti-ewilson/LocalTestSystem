using System;
using System.Collections.Generic;
using System.Text;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIPLCInterface
{
    /// <summary>
    /// DigitalInput Class
    /// Implements the IDigitalInput interface.
    /// Links to an instance of a DigitalPortBit
    /// </summary>
    public class DigitalInput : IDigitalInput 
    {
        #region Globals

        private DigitalPortBit _DigitalPortBit;
        
        #endregion

        #region Construction

        /// <summary>
        /// Initializes an instance of the <see cref="DigitalInput">Digital Input</see> class.
        /// </summary>
        /// <param name="Bit">Bit to be used for the input</param>
        public DigitalInput(DigitalPortBit Bit)
        {
            _DigitalPortBit = Bit;

            // Attach an event to the ValueChanged event of the DigitalPortBit,
            // in order to pass that event on to the calling application
            _DigitalPortBit.ValueChanged += new EventHandler(_DigitalPortBit_ValueChanged);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Occurs when the value of the Digital I/O changes
        /// </summary>
        public event EventHandler ValueChanged;
        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
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
            set { throw new Exception("Cannot set the value of a Digital Input"); }
        }

        #endregion

        #region IDigitalIO Members


        /// <summary>
        /// Indicates if the Digital I/O is an input
        /// </summary>
        /// <value></value>
        public bool IsInput
        {
            get { return true; }
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
