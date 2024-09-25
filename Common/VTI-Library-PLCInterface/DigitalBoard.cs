using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
//using MccDaq;

namespace VTIPLCInterface
{
    /// <summary>
    /// DigitalBoard Class
    /// 
    /// Contains the instance of a Measurement Computing digital board.
    /// Contains a list of Digital Ports
    /// Properties: BoardNum
    /// </summary>
    public class DigitalBoard
    {
        #region Globals

        private int _BoardNum;
        private VTIPLCInterface.MccDaq.MccBoard _MccBoard;
        private List<DigitalPort> _DigitalPorts;
        private Boolean _invertedLogic;

        #endregion

        #region Internal Properties

        internal VTIPLCInterface.MccDaq.MccBoard MccBoard
        {
            get { return _MccBoard; }
            set { _MccBoard = value; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the board number.
        /// </summary>
        /// <value>The board number.</value>
        public int BoardNum
        {
            get { return _BoardNum; }
            set { _BoardNum = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [inverted logic].
        /// </summary>
        /// <value><c>true</c> if [inverted logic]; otherwise, <c>false</c>.</value>
        public Boolean InvertedLogic
        {
            get { return _invertedLogic; }
            set { _invertedLogic = value; }
        }

        /// <summary>
        /// Gets or sets the digital ports.
        /// </summary>
        /// <value>The digital ports.</value>
        public List<DigitalPort> DigitalPorts
        {
            get { return _DigitalPorts; }
            set { _DigitalPorts = value; }
        }

        #endregion
    }
}
