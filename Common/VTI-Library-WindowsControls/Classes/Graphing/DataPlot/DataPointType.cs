using System;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;

namespace VTIWindowsControlLibrary.Classes.Graphing.DataPlot
{
    /// <summary>
    /// Represents one data point on a Data Plot
    /// </summary>
    public class DataPointType : IGraphPoint
    {
        /// <summary>
        /// Date and Time of this data point
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Elapsed time of this data point
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// Signal value of this data point
        /// </summary>
        public float Signal { get; set; }

        /// <summary>
        /// X-Axis value of this data point (equivalent to <see cref="Time">Time</see>)
        /// </summary>
        [XmlIgnore()]
        public float X
        {
            get
            {
                return Time;
            }
            set
            {
                Time = value;
            }
        }

        /// <summary>
        /// Y-Axis value of this data point (equivalent to <see cref="Signal">Signal</see>)
        /// </summary>
        [XmlIgnore()]
        public float Y
        {
            get
            {
                return Signal;
            }
            set
            {
                Signal = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointType">DataPointType</see> class.
        /// </summary>
        public DataPointType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointType">DataPointType</see> class.
        /// </summary>
        /// <param name="time">Elapsed time</param>
        /// <param name="signal">Signal value</param>
        /// <param name="dateTime">Date and time of this data point</param>
        public DataPointType(float time, float signal, DateTime dateTime)
        {
            Time = time;
            Signal = signal;
            DateTime = dateTime;
        }
    }
}