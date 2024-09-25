namespace VTIWindowsControlLibrary.Classes.IO.SignalConverters
{
    /// <summary>
    /// Signal converter for the LeakRate Signal of an Inficon LDS2010 Leak Detector
    /// </summary>
    /// <remarks>
    /// A <see cref="InficonLDS2010LeakRateSignal">InficonLDS2010LeakRateSignal</see> is a sub-class of a
    /// <see cref="LogLinearSignal">LogLinearSignal</see> with the following properties:
    /// <list type="bullet">
    /// <item><para><see cref="LogLinearSignal.VoltsPerDecade">VoltsPerDecade</see>: 0.5</para></item>
    /// <item><para><see cref="LogLinearSignal.MinExponent">MinExponent</see>: -14</para></item>
    /// <item><para><see cref="LogLinearSignal.MaxExponent">MaxExponent</see>: 6</para></item>
    /// </list>
    /// </remarks>
    public class InficonLDS2010LeakRateSignal : LogLinearSignal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InficonLDS2010LeakRateSignal">InficonLDS2010LeakRateSignal</see> class
        /// </summary>
        public InficonLDS2010LeakRateSignal()
        {
            VoltsPerDecade = 0.5;
            MinExponent = -14;
            MaxExponent = 6;
        }
    }
}