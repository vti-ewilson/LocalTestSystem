using System;
using System.Collections.Generic;
using System.Text;
//using MccDaq;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Diagnostics;

namespace VTIPLCInterface
{
    /// <summary>
    /// AnalogChannel Class
    /// 
    /// Represents one analog channel on a Measurement Computing analog board.
    /// Properties: Name, Channel, NumberOfBits, Range
    /// </summary>
    public class AnalogChannel
    {
        #region Globals

        internal int _NumberOfMovingAverages;
        private Single _Value;
        internal AnalogBoard Board;
        internal int[] MovingAverages;
        internal int MovingAverageNum;
        internal long MovingAverageSum;
        internal Stopwatch errorTimer;

        #endregion

        #region Event Handlers

        internal event EventHandler ValueChanged;

        #endregion

        #region Internal Properties

        internal Single Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                if (ValueChanged != null) ValueChanged(this, null);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public String Name { get; set; }
        /// <summary>
        /// Gets or sets the channel.
        /// </summary>
        /// <value>The channel.</value>
        public int Channel { get; set; }
        /// <summary>
        /// Gets or sets the number of samples.
        /// </summary>
        /// <value>The number of samples.</value>
        public int NumberOfSamples { get; set; }
        /// <summary>
        /// Gets or sets the number of moving averages.
        /// </summary>
        /// <value>The number of moving averages.</value>
        public int NumberOfMovingAverages
        {
            get { return _NumberOfMovingAverages; }
            set 
            { 
                _NumberOfMovingAverages = value;
                MovingAverages = new int[_NumberOfMovingAverages];
                MovingAverageNum = 0;
                MovingAverageSum = 0;
            }
        }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>The range.</value>
        public MccDaq.Range Range { get; set; }

        #endregion
    }
}
