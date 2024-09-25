using System.Collections.Generic;
using System.Threading;

namespace VTIWindowsControlLibrary.Classes.IO.Interfaces
{
    /// <summary>
    /// Interface for I/O Configurations
    /// </summary>
    /// <remarks>
    /// The <see cref="IOInterface">IOInterface</see> class implements this interface.
    /// The assembly referred to by the <see cref="IOInterface.InterfaceDLL">InterfaceDLL</see>
    /// property of the <see cref="IOInterface">IOInterface</see> must also implement
    /// this interface
    /// </remarks>
    public interface IIoConfig
    {
        /// <summary>
        /// Starts the I/O Processing Thread
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the I/O Processing Thread
        /// </summary>
        void Stop();

        /// <summary>
        /// Turns off all of the <see cref="IDigitalOutput">DigitalOutputs</see> from the I/O Interface
        /// </summary>
        void TurnAllOff();

        /// <summary>
        /// Reads all of the <see cref="IDigitalOutput">DigitalOutput</see> bit values from the PLC and assigns those values to the Digital Outputs' values in the PC software.
        /// </summary>
        bool ReadPLCDigOutputsIntoPC { get; set; }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IAnalogInput">AnalogInputs</see> from the I/O Interface
        /// </summary>
        Dictionary<string, IAnalogInput> AnalogInputs { get; }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IAnalogOutput">AnalogOutputs</see> from the I/O Interface
        /// </summary>
        Dictionary<string, IAnalogOutput> AnalogOutputs { get; }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}">Dictionary</see> collection of the
        /// <see cref="IDigitalInput">DigitalInputs</see> from the I/O Interface
        /// </summary>
        Dictionary<string, IDigitalInput> DigitalInputs { get; }

        /// <summary>
        /// Returns a <see cref="Dictionary{DigitalOutputs, TValue}">Dictionary</see> collection of the
        /// <see cref="IDigitalOutput">AnalogInputs</see> from the I/O Interface
        /// </summary>
        Dictionary<string, IDigitalOutput> DigitalOutputs { get; }

        /// <summary>
        /// Gets the thread that processes the I/O
        /// </summary>
        Thread ProcessThread { get; }
    }
}