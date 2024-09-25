using System;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// The exception that is thrown when the
    /// <see cref="Interfaces.IDigitalOutput.ValueChanging">ValueChanging</see> event of a
    /// <see cref="Interfaces.IDigitalOutput">Digital Output</see> is canceled.
    /// </summary>
    public class DigitalOutputChangeCanceledException : ApplicationException
    {
        /// <summary>
        /// Reason that was given when the
        /// <see cref="Interfaces.IDigitalOutput.ValueChanging">ValueChanging</see> event was canceled.
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalOutputChangeCanceledException">DigitalOutputChangeCanceledException</see> class.
        /// </summary>
        public DigitalOutputChangeCanceledException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalOutputChangeCanceledException">DigitalOutputChangeCanceledException</see> class.
        /// </summary>
        /// <param name="Reason">Reason why the change was canceled.</param>
        public DigitalOutputChangeCanceledException(string Reason)
        {
            this.Reason = Reason;
        }
    }
}