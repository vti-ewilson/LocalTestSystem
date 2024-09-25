using System;

namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for Digital Outputs
    /// </summary>
    public interface IDigitalOutput : IDigitalIO
    {
        /// <summary>
        /// Turns on the Digital Output
        /// </summary>
        void TurnOn();

        /// <summary>
        /// Turns off the Digital Output
        /// </summary>
        void TurnOff();

        /// <summary>
        /// Turns on the Digital Output
        /// </summary>
        void Enable();

        /// <summary>
        /// Turns off the Digital Output
        /// </summary>
        void Disable();

        /// <summary>
        /// Occurs when the value of the Digital I/O is about to change
        /// </summary>
        event DigitalOutputChangingEventHandler ValueChanging;

        /// <summary>
        /// Raises the <see cref="ValueChanging">ValueChanging</see> event
        /// </summary>
        void OnValueChanging(DigitalOutputChangingEventArgs e);

        /// <summary>
        /// Value of the Digital I/O
        /// </summary>
        new Boolean Value { get; set; }
    }
}