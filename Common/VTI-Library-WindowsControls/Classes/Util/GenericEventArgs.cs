using System;

namespace VTIWindowsControlLibrary.Classes.Util
{
    /// <summary>
    /// Event arguments with one generic parameter.
    /// </summary>
    /// <typeparam name="T">Type of the generic parameter.</typeparam>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// Creates an instance of the event arguments.
        /// </summary>
        /// <param name="value">Value of the event argument parameter.</param>
        public EventArgs(T value)
        {
            m_value = value;
        }

        private T m_value;

        /// <summary>
        /// Gets the value of the event argument parameter.
        /// </summary>
        public T Value
        {
            get { return m_value; }
        }
    }

    /// <summary>
    /// Event arguments with two generic parameters.
    /// </summary>
    /// <typeparam name="T">Type of the first generic parameter.</typeparam>
    /// <typeparam name="U">Type of the second generic parameter.</typeparam>
    public class EventArgs<T, U> : EventArgs<T>
    {
        /// <summary>
        /// Creates an instance of the event arguments.
        /// </summary>
        /// <param name="value">Value of the first event argument parameter.</param>
        /// <param name="value2">Value of the second event argument parameter.</param>
        public EventArgs(T value, U value2)
            : base(value)
        {
            m_value2 = value2;
        }

        private U m_value2;

        /// <summary>
        /// Gets the value of the second event argument parameter.
        /// </summary>
        public U Value2
        {
            get { return m_value2; }
        }
    }
}