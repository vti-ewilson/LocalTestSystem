using System;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// Can be applied to a StringParameter
    /// </summary>
    /// <remarks>
    /// <para>Identifies a source type and method to be used to retrieve an array of strings to be used to fill a combobox list</para>
    /// <para>Automatically causes the StringParameter to appear as a combobox on the EditCycle form</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringSourceAttribute : System.Attribute
    {
        private Type _StringSourceType;
        private String _StringSourceMethod;

        /// <param name="StringSourceType">Type of object that will return an array of strings</param>
        /// <param name="StringSourceMethod">Method of the StringSourceType that must return an array of strings</param>
        /// <example>
        /// <code>
        /// [StringSource(typeof(System.IO.Ports.SerialPort), "GetPortNames")]
        /// StringParameter param1;
        /// </code>
        /// </example>
        public StringSourceAttribute(
            Type StringSourceType,
            String StringSourceMethod)
        {
            this._StringSourceType = StringSourceType;
            this._StringSourceMethod = StringSourceMethod;
        }

        /// <summary>
        /// Type of the String Source
        /// </summary>
        /// <remarks>
        /// The StringSourceType can be any class that contains a method that returns an array of strings
        /// </remarks>
        /// <example>
        /// System.IO.Ports.SerialPort
        /// </example>
        public Type StringSourceType
        { get { return _StringSourceType; } }

        /// <summary>
        /// Method that returns an array of strings
        /// </summary>
        /// <example>
        /// SerialPort.GetPortNames()
        /// </example>
        public String StringSourceMethod
        { get { return _StringSourceMethod; } }
    }
}