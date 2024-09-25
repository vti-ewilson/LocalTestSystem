using System;

namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for Digital Inputs and Outputs
    /// </summary>
    public interface IDigitalIO : IAnalogDigitalIO
    {
        ///// <summary>
        ///// Name of the Digital I/O
        ///// </summary>
        //string Name { get; }
        /// <summary>
        /// Description of the Digital I/O. "Set" feature only to be used to hide unused IO in DigitalIO Form.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Value of the Digital I/O
        /// </summary>
        bool Value { get; }

        /// <summary>
        /// Indicates if the Digital I/O is an input
        /// </summary>
        bool IsInput { get; }

        ///// <summary>
        ///// Indicates that the Digital I/O was located in the <see cref="IOInterface">IOInterface</see>
        ///// </summary>
        //bool IsAvailable { get; }

        /// <summary>
        /// Occurs when the value of the Digital I/O changes
        /// </summary>
        event EventHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        void OnValueChanged();
    }
}