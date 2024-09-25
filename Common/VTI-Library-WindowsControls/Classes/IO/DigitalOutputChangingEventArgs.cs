using System;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO
{
    /// <summary>
    /// Event arguments for the <see cref="Interfaces.IDigitalOutput.ValueChanging">ValueChanging</see>
    /// event of the <see cref="Interfaces.IDigitalOutput">IDigitalOutput</see> interface.
    /// </summary>
    public class DigitalOutputChangingEventArgs
    {
        /// <summary>
        /// Gets the Digital Output whose value is changing.
        /// </summary>
        public IDigitalOutput DigitalOutput { get; private set; }

        /// <summary>
        /// Gets the value that the output is changing to.
        /// </summary>
        public Boolean NewValue { get; private set; }

        /// <summary>
        /// Gets or sets a value to indicate that the change should be canceled.
        /// </summary>
        public Boolean Cancel { get; set; }

        /// <summary>
        /// Gets or sets a value to indicate the reason why the change was canceled.
        /// </summary>
        public String Reason { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalOutputChangingEventArgs">DigitalOutputChangingEventArgs</see> class.
        /// </summary>
        /// <param name="DigitalOutput">Digital Output whose value is changing.</param>
        /// <param name="NewValue">Value that the output is changing to.</param>
        public DigitalOutputChangingEventArgs(IDigitalOutput DigitalOutput, Boolean NewValue)
        {
            this.DigitalOutput = DigitalOutput;
            this.NewValue = NewValue;
            this.Cancel = false;
            this.Reason = String.Empty;
        }
    }

    /// <summary>
    /// Event handler for the <see cref="Interfaces.IDigitalOutput.ValueChanging">ValueChanging</see>
    /// event of the <see cref="Interfaces.IDigitalOutput">IDigitalOutput</see> interface.
    /// </summary>
    /// <param name="sender">Digital Output whose value is changing.</param>
    /// <param name="e">A <see cref="DigitalOutputChangingEventArgs">DigitalOutputChangingEventArgs</see> containing the event data.</param>
    public delegate void DigitalOutputChangingEventHandler(IDigitalOutput sender, DigitalOutputChangingEventArgs e);
}