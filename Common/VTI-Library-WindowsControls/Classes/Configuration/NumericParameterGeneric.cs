using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Classes.Configuration
{
    /// <summary>
    /// Abstract class for numeric parameters, with a generic units type
    /// which can be used by subclasses to specify specific units.
    /// </summary>
    /// <typeparam name="TUnits">The type of the units.</typeparam>
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public abstract class NumericParameter<TUnits> : EditCycleParameter<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericParameter{TUnits}"/> class.
        /// </summary>
        public NumericParameter()
        {
        }

        #region Properties (9)

        /// <summary>
        /// Minimum allowable value for the ProcessValue
        /// </summary>
        [XmlElement("MinValue")]
        public double MinValue { get; set; }

        /// <summary>
        /// Maximum allowable value for the ProcessValue
        /// </summary>
        [XmlElement("MaxValue")]
        public double MaxValue { get; set; }

        /// <summary>
        /// Larger incremental step for the slider bar used when displaying the ProcessValue
        /// </summary>
        [XmlElement("LargeStep")]
        public double LargeStep { get; set; }

        /// <summary>
        /// Smallest incremental step for the slider bar used when displaying the ProcessValue
        /// </summary>
        [XmlElement("SmallStep")]
        public double SmallStep { get; set; }

        /// <summary>
        /// Units to be displayed for this parameter (i.e. psi, Torr, atm-cc/sec)
        /// </summary>
        [XmlElement("Units")]
        public virtual TUnits Units { get; set; }

        /// <summary>
        /// Gets or sets the new value string.
        /// </summary>
        /// <value>The new value string.</value>
        [XmlIgnore()]
        public override string NewValueString
        {
            get { return NewValue.ToString(StringFormat); }
            set
            {
                double val;
                if (double.TryParse(value, out val))
                    NewValue = val;
                else
                    VtiEvent.Log.WriteVerbose(
                       string.Format(VtiLibLocalization.ErrorSettingProcessValue, DisplayName, value),
                       VtiEventCatType.Parameter_Update);
            }
        }

        /// <summary>
        /// Returns a string containing the ProcessValue formatted according to the
        /// StringFormat property followed by the Units.
        /// </summary>
        public string ProcessValueFormatted
        {
            get
            {
                return ProcessValue.ToString(StringFormat) + " " + Units;
            }
        }

        /// <summary>
        /// Gets or sets the process value string.
        /// </summary>
        /// <value>The process value string.</value>
        [XmlIgnore()]
        public override string ProcessValueString
        {
            get { return ProcessValue.ToString(StringFormat); }
            set
            {
                double val;
                if (double.TryParse(value, out val))
                    ProcessValue = val;
            }
        }

        /// <summary>
        /// Returns a custom format string based on the value of the SmallStep property.
        /// </summary>
        public string StringFormat
        {
            get
            {
                string stringFormat;
                if (this.SmallStep >= 1)
                    stringFormat = "0";
                else if (this.SmallStep >= 0.1)
                    stringFormat = "0.0";
                else if (this.SmallStep >= 0.01)
                    stringFormat = "0.00";
                else if (this.SmallStep >= 0.001)
                    stringFormat = "0.000";
                else if (this.SmallStep >= 0.0001)
                    stringFormat = "0.0000";
                else if (this.SmallStep == 0)
                    stringFormat = "0";
                else
                    stringFormat = "0.00E+00";
                return stringFormat;
            }
        }

        #endregion Properties

        /// <summary>
        /// Compare two <see cref="NumericParameter{TUnits}">NumericParameters</see>
        /// </summary>
        /// <param name="left">NumericParameter on the left</param>
        /// <param name="right">NumericParameter on the right</param>
        /// <returns>Value indicating if the process value of the NumericParameter on the left
        /// is less than the process value of the NumericParameter on the right</returns>
        public static bool operator <(NumericParameter<TUnits> left, NumericParameter<TUnits> right)
        {
            return left.ProcessValue < right.ProcessValue;
        }

        /// <summary>
        /// Compare two <see cref="NumericParameter">NumericParameters</see>
        /// </summary>
        /// <param name="left">NumericParameter on the left</param>
        /// <param name="right">NumericParameter on the right</param>
        /// <returns>Value indicating if the process value of the NumericParameter on the left
        /// is greater than the process value of the NumericParameter on the right</returns>
        public static bool operator >(NumericParameter<TUnits> left, NumericParameter<TUnits> right)
        {
            return left.ProcessValue < right.ProcessValue;
        }

        /// <summary>
        /// Compare two <see cref="NumericParameter">NumericParameters</see>
        /// </summary>
        /// <param name="left">NumericParameter on the left</param>
        /// <param name="right">NumericParameter on the right</param>
        /// <returns>Value indicating if the process value of the NumericParameter on the left
        /// is less than or equal to the process value of the NumericParameter on the right</returns>
        public static bool operator <=(NumericParameter<TUnits> left, NumericParameter<TUnits> right)
        {
            return left < right || left == right;
        }

        /// <summary>
        /// Compare two <see cref="NumericParameter">NumericParameters</see>
        /// </summary>
        /// <param name="left">NumericParameter on the left</param>
        /// <param name="right">NumericParameter on the right</param>
        /// <returns>Value indicating if the process value of the NumericParameter on the left
        /// is greater than or equal to the process value of the NumericParameter on the right</returns>
        public static bool operator >=(NumericParameter<TUnits> left, NumericParameter<TUnits> right)
        {
            return left > right || left == right;
        }
    }
}