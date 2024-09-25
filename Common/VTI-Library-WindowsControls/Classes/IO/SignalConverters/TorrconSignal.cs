namespace VTIWindowsControlLibrary.Classes.IO.SignalConverters
{
    /// <summary>
    /// Signal converter for the analog signal of a TorrConn
    /// convection gauge controller
    /// </summary>
    /// <remarks>
    /// A <see cref="TorrconSignal">TorrconSignal</see> is a sub-class of a
    /// <see cref="LogLinearSignal">LogLinearSignal</see> with the following properties:
    /// <list type="bullet">
    /// <item><para><see cref="LogLinearSignal.VoltsPerDecade">VoltsPerDecade</see>: 0.5</para></item>
    /// <item><para><see cref="LogLinearSignal.MinExponent">MinExponent</see>: -3</para></item>
    /// <item><para><see cref="LogLinearSignal.MaxExponent">MaxExponent</see>: 3</para></item>
    /// </list>
    /// </remarks>
    public class TorrconSignal : LogLinearSignal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TorrconSignal">TorrconSignal</see> class
        /// </summary>
        public TorrconSignal()
        {
            VoltsPerDecade = 0.5;
            MinExponent = -3;
            MaxExponent = 3;
        }
    }
}