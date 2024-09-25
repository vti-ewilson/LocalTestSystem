using System;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace VTIWindowsControlLibrary.Classes.IO.SignalConverters
{
    /// <summary>
    /// General purpose Linear Signal Converter
    /// </summary>
    public class LinearSignal : ILinearSignalConverter
    {
        #region Globals

        private Double inputMinimum, inputMaximum, fullScale, offset;

        private NumericParameter inputMinimumParameter, inputMaximumParameter, fullScaleParameter, offsetParameter;

        #endregion Globals

        #region IAnalogSignalConverter Members

        /// <summary>
        /// Converts the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see> to the
        /// <see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        /// <param name="RawValue"><see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see></param>
        /// <returns>Double (<see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>)</returns>
        public double Convert(double RawValue)
        {
            return (((RawValue - inputMinimum) / (inputMaximum - inputMinimum)) * fullScale - offset);
        }

        /// <summary>
        /// Returns true to indicate that this is a Linear Signal Converter
        /// </summary>
        public bool IsLinear
        {
            get { return true; }
        }

        #endregion IAnalogSignalConverter Members

        #region Construction

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LinearSignal">LinearSignal</see> class
        ///// using numeric (double) values for the parameters.
        ///// </summary>
        ///// <param name="FullScale">Full scale range of the <see cref="AnalogSignal.Value">Value</see> of the
        ///// <see cref="AnalogSignal">AnalogSignal</see></param>
        ///// <param name="Offset">Zero offset of the <see cref="AnalogSignal.Value">Value</see> of the
        ///// <see cref="AnalogSignal">AnalogSignal</see></param>
        ///// <param name="InputMininum">Minimum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        ///// <see cref="IAnalogInput">AnalogInput</see></param>
        ///// <param name="InputMaximum">Maximum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        ///// <see cref="IAnalogInput">AnalogInput</see></param>
        //public LinearSignal(Double FullScale, Double Offset, Double InputMinimum, Double InputMaximum)
        //{
        //    fullScale = FullScale;
        //    offset = Offset;
        //    inputMinimum = InputMinimum;
        //    inputMaximum = InputMaximum;
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LinearSignal">LinearSignal</see> class
        ///// using <see cref="VTIWindowsControlLibrary.Classes.Configuration.NumericParameter">NumericParameter</see> values for the parameters.
        ///// </summary>
        ///// <param name="FullScale">Full scale range of the <see cref="AnalogSignal.Value">Value</see> of the
        ///// <see cref="AnalogSignal">AnalogSignal</see></param>
        ///// <param name="Offset">Zero offset of the <see cref="AnalogSignal.Value">Value</see> of the
        ///// <see cref="AnalogSignal">AnalogSignal</see></param>
        ///// <param name="InputMininum">Minimum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        ///// <see cref="IAnalogInput">AnalogInput</see></param>
        ///// <param name="InputMaximum">Maximum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        ///// <see cref="IAnalogInput">AnalogInput</see></param>
        //public LinearSignal(NumericParameter FullScale, NumericParameter Offset, NumericParameter InputMinimum, NumericParameter InputMaximum)
        //{
        //    FullScaleParameter = FullScale;
        //    OffsetParameter = Offset;
        //    InputMinimumParameter = InputMinimum;
        //    InputMaximumParameter = InputMaximum;
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LinearSignal">LinearSignal</see> class
        ///// </summary>
        //public LinearSignal() { }

        private void fullScaleParam_ProcessValueChanged(object sender, EventArgs e)
        {
            fullScale = fullScaleParameter.ProcessValue;
        }

        private void offsetParam_ProcessValueChanged(object sender, EventArgs e)
        {
            offset = offsetParameter.ProcessValue;
        }

        private void inputMinimumParam_ProcessValueChanged(object sender, EventArgs e)
        {
            inputMaximum = inputMaximumParameter.ProcessValue;
        }

        private void inputMaximumParam_ProcessValueChanged(object sender, EventArgs e)
        {
            inputMaximum = inputMaximumParameter.ProcessValue;
        }

        #endregion Construction

        /// <summary>
        /// Minimum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// </summary>
        public Double InputMinimum
        {
            get { return inputMinimum; }
            set { inputMinimum = value; }
        }

        /// <summary>
        /// Maximum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// </summary>
        public Double InputMaximum
        {
            get { return inputMaximum; }
            set { inputMaximum = value; }
        }

        /// <summary>
        /// Full scale range of the <see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        public Double FullScale
        {
            get { return fullScale; }
            set { fullScale = value; }
        }

        /// <summary>
        /// Zero offset of the <see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        public Double Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        /// <summary>
        /// Minimum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// </summary>
        public NumericParameter InputMinimumParameter
        {
            get { return inputMinimumParameter; }
            set
            {
                inputMinimumParameter = value;
                inputMinimum = inputMinimumParameter.ProcessValue;
                inputMinimumParameter.ProcessValueChanged += new EventHandler(inputMinimumParam_ProcessValueChanged);
            }
        }

        /// <summary>
        /// Maximum value of the <see cref="IAnalogInput.RawValue">RawValue</see> of the
        /// <see cref="IAnalogInput">AnalogInput</see>
        /// </summary>
        public NumericParameter InputMaximumParameter
        {
            get { return inputMaximumParameter; }
            set
            {
                inputMaximumParameter = value;
                inputMaximum = inputMaximumParameter.ProcessValue;
                inputMaximumParameter.ProcessValueChanged += new EventHandler(inputMaximumParam_ProcessValueChanged);
            }
        }

        /// <summary>
        /// Full scale range of the <see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        public NumericParameter FullScaleParameter
        {
            get { return fullScaleParameter; }
            set
            {
                fullScaleParameter = value;
                fullScale = fullScaleParameter.ProcessValue;
                fullScaleParameter.ProcessValueChanged += new EventHandler(fullScaleParam_ProcessValueChanged);
            }
        }

        /// <summary>
        /// Zero offset of the <see cref="AnalogSignal.Value">Value</see> of the
        /// <see cref="AnalogSignal">AnalogSignal</see>
        /// </summary>
        public NumericParameter OffsetParameter
        {
            get { return offsetParameter; }
            set
            {
                offsetParameter = value;
                offset = offsetParameter.ProcessValue;
                offsetParameter.ProcessValueChanged += new EventHandler(offsetParam_ProcessValueChanged);
            }
        }
    }
}