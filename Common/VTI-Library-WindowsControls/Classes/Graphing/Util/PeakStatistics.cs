using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;

namespace VTIWindowsControlLibrary.Classes.Graphing.Util
{
    /// <summary>
    /// Result of the <see cref="GraphPointExtensions.FindNearestPeak{T}">FindNearestPeak</see> method.
    /// </summary>
    /// <typeparam name="T">Type of graph point</typeparam>
    public class PeakStatistics<T>
        where T : class, IGraphPoint, new()
    {
        /// <summary>
        /// Gets the nearest peak that was the result of the
        /// <see cref="GraphPointExtensions.FindNearestPeak{T}">FindNearestPeak</see> method.
        /// </summary>
        public T Peak { get; private set; }

        /// <summary>
        /// Gets the minimum value to the left of the <see cref="Peak">Peak</see>.
        /// </summary>
        public T LeftMin { get; private set; }

        /// <summary>
        /// Gets the minimum value to the right of the <see cref="Peak">Peak</see>.
        /// </summary>
        public T RightMin { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeakStatistics{T}">PeakStatistics</see> class.
        /// </summary>
        /// <param name="peak">Nearest peak that was the result of the
        /// <see cref="GraphPointExtensions.FindNearestPeak{T}">FindNearestPeak</see> method.</param>
        /// <param name="leftMin">Minimum value to the left of the <see cref="Peak">Peak</see>.</param>
        /// <param name="rightMin">Minimum value to the right of the <see cref="Peak">Peak</see>.</param>
        public PeakStatistics(T peak, T leftMin, T rightMin)
        {
            Peak = peak;
            LeftMin = leftMin;
            RightMin = rightMin;
        }
    }
}