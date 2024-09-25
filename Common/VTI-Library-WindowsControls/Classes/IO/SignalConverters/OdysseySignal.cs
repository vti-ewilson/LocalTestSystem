namespace VTIWindowsControlLibrary.Classes.IO.SignalConverters
{
    /// <summary>
    /// Signal converter for the Leak Rate signal of an AEROVAC Odyssey Leak Detector
    /// convection gauge controller
    /// </summary>
    public class OdysseySignal : LogLinearSignal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdysseySignal">OdysseySignal</see>
        /// </summary>
        public OdysseySignal()
        {
            VoltsPerDecade = 1;
            MinExponent = -11;
            MaxExponent = -6;
        }
    }
}