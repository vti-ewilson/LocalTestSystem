namespace VTIWindowsControlLibrary.Classes.Graphing.Util
{
    /// <summary>
    /// Represents a line calculated from a data plot trace using linear regression.
    /// </summary>
    public class LinearRegressionType
    {
        /// <summary>
        /// Slope of the line in terms of the units of the trace per second.
        /// </summary>
        public double Slope { get; private set; }

        /// <summary>
        /// Y-Intercept of the line.
        /// </summary>
        public double Intercept { get; private set; }

        /// <summary>
        /// R-Squared value for the linear regression.
        /// </summary>
        public double RSquaredValue { get; private set; }

        /// <summary>
        /// Creates an instance of the linear regression line.
        /// </summary>
        /// <param name="slope">Slope of the line in terms of the units of the trace per second.</param>
        /// <param name="intercept">Y-Intercept of the line.</param>
        /// <param name="rSquaredValue">R-Squared value</param>
        public LinearRegressionType(double slope, double intercept, double rSquaredValue)
        {
            Slope = slope;
            Intercept = intercept;
            RSquaredValue = rSquaredValue;
        }
    }
}