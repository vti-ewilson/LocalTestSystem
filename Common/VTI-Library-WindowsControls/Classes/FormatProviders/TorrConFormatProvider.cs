using System;

namespace VTIWindowsControlLibrary.Classes.FormatProviders
{
    /// <summary>
    /// Impliments a format provider that formats a numerical value in the same
    /// way that a TorrCon II instrument does.
    /// </summary>
    public class TorrConFormatProvider : IFormatProvider, ICustomFormatter
    {
        #region IFormatProvider Members

        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        #endregion IFormatProvider Members

        #region ICustomFormatter Members

        /// <summary>
        /// Returns a string format that will format a numerical value in the same way that a TorrCon II does
        /// </summary>
        /// <param name="format">unused</param>
        /// <param name="arg">argument should be convertible to a Single</param>
        /// <param name="formatProvider">unused</param>
        /// <returns></returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Convert argument to a single
            Single value = Convert.ToSingle(arg);

            if (value >= 100)
                return value.ToString("000.");
            else if (value >= 10)
                return value.ToString("00.0");
            else if (value >= 1)
                return value.ToString("0.00");
            else
            {
                value *= 1000;
                if (value >= 100)
                    return value.ToString("000.");
                else if (value >= 10)
                    return value.ToString("00.0");
                else
                    return value.ToString("0.00");
            }
        }

        #endregion ICustomFormatter Members
    }
}