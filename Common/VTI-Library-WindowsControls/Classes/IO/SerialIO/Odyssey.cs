using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VTIWindowsControlLibrary.Classes.IO.SerialIO
{
    public class Odyssey : SerialIOBase
    {

        /// <summary>
        /// Occurs when the <see cref="Value">Value</see> changes
        /// </summary>
        public override event EventHandler ValueChanged;
        /// <summary>
        /// Raises the <see cref="ValueChanged">ValueChanged</see> event
        /// </summary>
        protected override void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        /// <summary>
        /// Occurs when the <see cref="RawValue">RawValue</see> changes
        /// </summary>
        public override event EventHandler RawValueChanged;
        /// <summary>
        /// Raises the <see cref="RawValueChanged">RawValueChanged</see> event
        /// </summary>
        protected override void OnRawValueChanged()
        {
            if (RawValueChanged != null)
                RawValueChanged(this, null);
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        public override void BackgroundProcess()
        {
            throw new NotImplementedException();
        }

        public override double Value
        {
            get { throw new NotImplementedException(); }
            internal set { throw new NotImplementedException(); }
        }

        public override string FormattedValue
        {
            get { throw new NotImplementedException(); }
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        public override double RawValue
        {
            get { throw new NotImplementedException(); }
        }

        public override double Min
        {
            get { throw new NotImplementedException(); }
        }

        public override double Max
        {
            get { throw new NotImplementedException(); }
        }

        public override string Units
        {
            get { throw new NotImplementedException(); }
        }

        public override string Format
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
