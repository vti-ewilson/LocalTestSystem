using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VtiOdysseyLibrary.Classes
{
    /// <summary>
    /// Represents a 24-bit integer.
    /// </summary>
    /// <remarks>
    /// Internally, the value is stored as a 32-bit integer.  This type was created
    /// to distinguish 24-bit and 32-bit integers, primarily for communicating with the
    /// AERO VAC Odyssey.
    /// </remarks>
    public struct Int24
    {
        private int _Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Int24">Int24</see> struct.
        /// </summary>
        /// <param name="value">Value of the integer</param>
        public Int24(int value)
        {
            _Value = value;
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        /// <summary>
        /// Implicitly converts a <see cref="Int32">32-bit integer</see> to a <see cref="Int24">24-bit integer</see>.
        /// </summary>
        /// <param name="x"><see cref="Int32">32-bit integer</see> to be converted</param>
        /// <returns>Equivalent <see cref="Int24">24-bit integer</see> value.</returns>
        public static implicit operator Int24(int x)
        {
            return new Int24(x);
        }

        /// <summary>
        /// Implicitly converts a <see cref="Int24">24-bit integer</see> to a <see cref="Int32">32-bit integer</see>.
        /// </summary>
        /// <param name="x"><see cref="Int24">24-bit integer</see> to be converted</param>
        /// <returns>Equivalent <see cref="Int32">32-bit integer</see> value.</returns>
        public static implicit operator int(Int24 x)
        {
            return x.Value;
        }
    }
}
