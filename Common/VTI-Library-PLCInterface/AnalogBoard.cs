using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
//using MccDaq;

namespace VTIPLCInterface
{
    /// <summary>
    /// AnalogBoard Class
    /// 
    /// Contains the instanace of a Measurement Computing analog board.
    /// Contains a list of analog channels. 
    /// Properties: BoardNum, NumberOfBits
    /// </summary>
    public class AnalogBoard
    {
        #region Internal Properties

        internal VTIPLCInterface.MccDaq.MccBoard MccBoard { get; set; }

        #endregion

        #region public Properties

        /// <summary>
        /// Gets or sets the board number.
        /// 0-9 is for MCC IO cards, 10-19 is for CLICK PLCs, 20-29 is for Allen-Bradley Micro830 PLCs, 30-39 is for Allen-Bradley CompactLogix PLCs.
        /// </summary>
        /// <value>The board number.</value>
        public int BoardNum { get; set; }
        /// <summary>
        /// Gets or sets the number of bits.
        /// </summary>
        /// <value>The number of bits.</value>
        public int NumberOfBits { get; set; }
        /// <summary>
        /// Gets or sets the analog inputs.
        /// </summary>
        /// <value>The analog inputs.</value>
        public List<AnalogChannel> AnalogInputs { get; set; }
        /// <summary>
        /// Gets or sets the analog outputs.
        /// </summary>
        /// <value>The analog outputs.</value>
        public List<AnalogChannel> AnalogOutputs { get; set; }
        
        #endregion
    }
}
