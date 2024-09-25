using VTIWindowsControlLibrary.Classes.Graphing.DataPlot;

namespace VTIWindowsControlLibrary.Classes.Graphing.Util
{
    /// <summary>
    /// Represents a class that can compare two
    /// <see cref="DataPointType">DataPointType</see> objects by comparing their
    /// <see cref="DataPointType.Time">Time</see> properties.
    /// </summary>
    public class PlotTimeComparer : System.Collections.Generic.IComparer<DataPointType>
    {
        #region IComparer<DataPointType> Members

        /// <summary>
        /// Compares two
        /// <see cref="DataPointType">DataPointType</see> objects by comparing their
        /// <see cref="DataPointType.Time">Time</see> properties.
        /// </summary>
        /// <param name="x"><see cref="DataPointType">DataPointType</see> object</param>
        /// <param name="y"><see cref="DataPointType">DataPointType</see> object</param>
        /// <returns>Value indicating the relation of the two objects according to their <see cref="DataPointType.Time">Time</see> properties</returns>
        public int Compare(DataPointType x, DataPointType y)
        {
            return (x.Time.CompareTo(y.Time));
        }

        #endregion IComparer<DataPointType> Members
    }
}