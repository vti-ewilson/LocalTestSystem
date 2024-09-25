using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VTIWindowsControlLibrary.Classes.Graphing.Interfaces;

namespace VTIWindowsControlLibrary.Classes.Graphing.Util
{
    /// <summary>
    /// Extention methods related to the <see cref="IGraphPoint">IGraphPoint</see> interface.
    /// </summary>
    public static class GraphPointExtensions
    {
        /// <summary>
        /// Gets the linear regression.
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">The points.</param>
        /// <param name="startValue">The start value.</param>
        /// <param name="range">The range.</param>
        /// <returns><see cref="LinearRegressionType">Linear regression</see> of the points</returns>
        /// <remarks>This method locks the <see cref="ICollection.SyncRoot">SyncRoot</see> of the list
        /// while performing the linear regression, to prevent the data from being changed.</remarks>
        public static LinearRegressionType GetLinearRegression<T>(this IList<T> points, double startValue, double range)
            where T : IGraphPoint
        {
            lock (((IList)points).SyncRoot)
            {
                return points.Where(p => p.X >= startValue && p.X <= startValue + range).GetLinearRegression();
            }
        }

        /// <summary>
        /// Gets the linear regression.
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">The points.</param>
        /// <param name="range">The range.</param>
        /// <returns><see cref="LinearRegressionType">Linear regression</see> of the points</returns>
        /// <remarks>This method locks the <see cref="ICollection.SyncRoot">SyncRoot</see> of the list
        /// while performing the linear regression, to prevent the data from being changed.</remarks>
        public static LinearRegressionType GetLinearRegression<T>(this IList<T> points, double range)
            where T : IGraphPoint
        {
            lock (((IList)points).SyncRoot)
            {
                return points.Where(p => p.X >= points.Last().X - range).GetLinearRegression();
            }
        }

        /// <summary>
        /// Performs a Linear Regression on the given list of points
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">List of points</param>
        /// <returns><see cref="LinearRegressionType">Linear regression</see> of the points</returns>
        /// <remarks>This method locks the <see cref="ICollection.SyncRoot">SyncRoot</see> of the list
        /// while performing the linear regression, to prevent the data from being changed.</remarks>
        public static LinearRegressionType GetLinearRegression<T>(this IList<T> points)
            where T : IGraphPoint
        {
            lock (((IList)points).SyncRoot)
            {
                return ((IEnumerable<T>)points).GetLinearRegression();
            }
        }

        /// <summary>
        /// Gets the linear regression.
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">The points.</param>
        /// <param name="startValue">The start value.</param>
        /// <param name="range">The range.</param>
        /// <returns><see cref="LinearRegressionType">Linear regression</see> of the points</returns>
        /// <remarks>This method locks the <see cref="ICollection.SyncRoot">SyncRoot</see> of the list
        /// while performing the linear regression, to prevent the data from being changed.</remarks>
        public static LinearRegressionType GetLinearRegression<T>(this IEnumerable<T> points, double startValue, double range)
            where T : IGraphPoint
        {
            return points.Where(p => p.X >= startValue && p.X <= startValue + range).GetLinearRegression();
        }

        /// <summary>
        /// Gets the linear regression.
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">The points.</param>
        /// <param name="range">The range.</param>
        /// <returns><see cref="LinearRegressionType">Linear regression</see> of the points</returns>
        /// <remarks>This method locks the <see cref="ICollection.SyncRoot">SyncRoot</see> of the list
        /// while performing the linear regression, to prevent the data from being changed.</remarks>
        public static LinearRegressionType GetLinearRegression<T>(this IEnumerable<T> points, double range)
            where T : IGraphPoint
        {
            return points.Where(p => p.X >= points.Last().X - range).GetLinearRegression();
        }

        /// <summary>
        /// Gets the linear regression.
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">The points.</param>
        /// <returns><see cref="LinearRegressionType">Linear regression</see> of the points</returns>
        public static LinearRegressionType GetLinearRegression<T>(this IEnumerable<T> points)
            where T : IGraphPoint
        {
            double n = points.Count();
            double sumX = points.Sum(p => p.X);
            double sumY = points.Sum(p => p.Y);
            double sumXY = points.Sum(p => p.X * p.Y);
            double sumX2 = points.Sum(p => p.X * p.X);

            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double intercept = (sumY - slope * sumX) / n;

            double sumYErr = points.Sum(p => Math.Pow((p.Y - (slope * p.X + intercept)), 2));
            double sumYTot = points.Sum(p => Math.Pow((p.Y - sumY / n), 2));

            double rSquared = (1 - ((sumYErr) / sumYTot));

            return new LinearRegressionType(slope, intercept, rSquared);
        }

        /// <summary>
        /// Gets the normalized standard deviation
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">The points.</param>
        /// <returns><see cref="LinearRegressionType">Linear regression</see> of the points</returns>
        public static double GetNormStdDev<T>(this IEnumerable<T> points)
            where T : IGraphPoint
        {
            double n = points.Count();
            double sumX = points.Sum(p => p.X);
            double sumY = points.Sum(p => p.Y);
            double sumXY = points.Sum(p => p.X * p.Y);
            double sumX2 = points.Sum(p => p.X * p.X);

            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double intercept = (sumY - slope * sumX) / n;
            //int jj, cnt = test.PTA.Count();
            double meanX, meanY;
            meanX = sumX / n;
            meanY = sumY / n;
            // Next we calculate the standard deviation of the
            // residuals (vertical distances to the regression line).
            double sumResiduals = 0, SSTotal = 0;
            foreach (var point in points)
            {
                double regressionValue = intercept + slope * point.X;
                double residual = Math.Abs(point.Y - regressionValue);
                SSTotal += (point.Y - meanY) * (point.Y - meanY);
                sumResiduals += residual;
            }
            //for (jj = cnt - 1; jj > cnt - 1 - n; jj--) {
            //  double regressionValue = intercept + slope * points.ToList()..x;
            //  double residual = Math.Abs(test.PTA[jj].y - regressionValue);
            //  SSTotal += (test.PTA[jj].y - meanY) * (test.PTA[jj].y - meanY);
            //  sumResiduals += residual;
            //}
            double avgResiduals = sumResiduals / n;
            sumResiduals = 0;
            foreach (var point in points)
            {
                double regressionValue = intercept + slope * point.X;
                double residual = Math.Abs(point.Y - regressionValue);
                sumResiduals += (residual - avgResiduals) * (residual - avgResiduals);
            }
            //for (jj = cnt - 1; jj > cnt - 1 - n; jj--) {
            //  double regressionValue = intercept + slope * test.PTA[jj].x;
            //  double residual = Math.Abs(test.PTA[jj].y - regressionValue);
            //  sumResiduals += (residual - avgResiduals) * (residual - avgResiduals);
            //}
            double stdDeviation = Math.Sqrt(sumResiduals / n);
            double normStdDevTemp = stdDeviation / n;
            return normStdDevTemp;
        }

        private static IEnumerable<T> GetNearbyPoints<T>(this IList<T> points, T point, float range)
            where T : class, IGraphPoint, new()
        {
            lock (((IList)points).SyncRoot)
            {
                return ((IEnumerable<T>)points).GetNearbyPoints(point, range);
            }
        }

        private static IEnumerable<T> GetNearbyPoints<T>(this IEnumerable<T> points, T point, float range)
            where T : class, IGraphPoint, new()
        {
            return points.Where(p => p.X > point.X - range && p.X < point.X + range);
        }

        private static float GetNearbyAverage<T>(this IList<T> points, T point, float range)
            where T : class, IGraphPoint, new()
        {
            lock (((IList)points).SyncRoot)
            {
                return ((IEnumerable<T>)points).GetNearbyAverage(point, range);
            }
        }

        private static float GetNearbyAverage<T>(this IEnumerable<T> points, T point, float range)
            where T : class, IGraphPoint, new()
        {
            return points.GetNearbyPoints(point, range).Average(p => p.Y);
        }

        private static IEnumerable<T> GetSmoothPoints<T>(this IList<T> points)
            where T : class, IGraphPoint, new()
        {
            lock (((IList)points).SyncRoot)
            {
                return ((IEnumerable<T>)points).GetSmoothPoints();
            }
        }

        /// <summary>
        /// Generates a list of points using a smoothing algorithm
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">Source points</param>
        /// <returns>Smoothed points</returns>
        public static IEnumerable<T> GetSmoothPoints<T>(this IEnumerable<T> points)
            where T : class, IGraphPoint, new()
        {
            IEnumerable<T> smoothPoints =
            points.Select(
                p =>
                {
                    float range;
                    if (p.Y > 5F * points.GetNearbyAverage(p, 1F))
                        range = 0.25F;
                    else
                        range = 1F;

                    IEnumerable<T> nearbyPoints2 = points.GetNearbyPoints(p, range);
                    float sum = nearbyPoints2.Sum(p2 => p2.Y * (float)Math.Pow((range - Math.Abs(p.X - p2.X)) / range, 5));
                    float divisor = nearbyPoints2.Sum(p2 => (float)Math.Pow((range - Math.Abs(p.X - p2.X)) / range, 5));
                    T point = Activator.CreateInstance(typeof(T), p.X, sum / divisor) as T;

                    return point;
                });
            return smoothPoints;
        }

        /// <summary>
        /// Calculates the average of the baseline "noise" of a list of data points
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">Source points</param>
        /// <returns>Average of the baseline "noise"</returns>
        public static float CalculateAverageMinimum<T>(this IEnumerable<T> points)
            where T : class, IGraphPoint, new()
        {
            // Average of all the data
            float avg1 = points.Average(p => p.Y);
            for (int i = 0; i < 3; i++)
                avg1 = points.Where(p => p.Y < avg1).Average(p => p.Y);

            return avg1;
        }

        /// <summary>
        /// Generates a list of all potential peaks from a list of data points
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">Source points</param>
        /// <returns>List of potential peaks</returns>
        public static IEnumerable<T> FindPeaks<T>(this IEnumerable<T> points)
            where T : class, IGraphPoint, new()
        {
            float scanMinimum = points.CalculateAverageMinimum();

            foreach (var point in points)
            {
                IEnumerable<T> nearPoints;
                nearPoints = points.Where(p => p.X >= point.X - 0.8F && p.X <= point.X + 0.8F);

                float minVal = nearPoints.Min(p => p.Y);
                float maxVal = nearPoints.Max(p => p.Y);

                if (point.Y > scanMinimum * 5F && point.Y >= maxVal)
                {
                    if (point.Y > minVal * 5F)
                    {
                        yield return point;
                    }
                    else
                    {
                        nearPoints = points.Where(p => p.X >= point.X - 2 && p.X <= point.X + 2);

                        minVal = nearPoints.Min(p => p.Y);

                        if (point.Y > minVal * 2.5F)
                        {
                            yield return point;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds the peak in a list of points nearest to a given location
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">Source data</param>
        /// <param name="location">Peak search location</param>
        /// <param name="searchWidth">Peak search width</param>
        /// <returns>Statistics of the located peak</returns>
        public static PeakStatistics<T> FindNearestPeak<T>(this IEnumerable<T> points, float location, float searchWidth)
            where T : class, IGraphPoint, new()
        {
            try
            {
                IEnumerable<T> nearbyPoints = points.Where(p => p.X >= location - searchWidth / 2 && p.X <= location + searchWidth / 2);

                // Nearest point which is taller than all points within 0.5 amu
                T maxPoint = nearbyPoints
                    .OrderBy(p => Math.Abs(p.X - location))
                    .FirstOrDefault(p =>
                        p.Y == points
                                .Where(p2 => p2.X >= p.X - 0.5F && p2.X <= p.X + 0.5F)
                                .Max(p2 => p2.Y));
                T leftMin = nearbyPoints
                    .LastOrDefault(p =>
                        p.X < maxPoint.X &&
                        p.Y == points
                                .Where(p2 => p2.X >= p.X - 0.5F && p2.X <= p.X + 0.5F)
                                .Min(p2 => p2.Y));
                T rightMin = nearbyPoints
                    .LastOrDefault(p =>
                        p.X < maxPoint.X &&
                        p.Y == points
                                .Where(p2 => p2.X >= p.X - 0.5F && p2.X <= p.X + 0.5F)
                                .Min(p2 => p2.Y));
                PeakStatistics<T> nearestPeak = new PeakStatistics<T>(maxPoint, leftMin, rightMin);
                return nearestPeak;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Interpolates the nearest Y value.
        /// </summary>
        /// <typeparam name="T"><see cref="IGraphPoint">Type</see> of the points.</typeparam>
        /// <param name="points">The points.</param>
        /// <param name="xValue">The x value.</param>
        /// <returns></returns>
        public static float InterpolateNearestYValue<T>(this IEnumerable<T> points, float xValue)
            where T : class, IGraphPoint, new()
        {
            // convert points to list
            //List<T> listPoints = points.ToList();
            // locate x value in points array that is nearest to xValue
            int ndx = 0, ndxLow = 0, ndxHigh = points.Count() - 1;
            while (ndxHigh - ndxLow > 1)
            {
                ndx = (ndxLow + ndxHigh) / 2;
                if (points.ElementAt(ndx).X <= xValue)
                    ndxLow = ndx;
                else
                    ndxHigh = ndx;
            }
            if (ndxHigh == points.Count() - 1)
                return float.NaN;
            return points.ElementAt(ndx).Y;
        }

        /*
            public static float InterpolateNearestYValue2<T>(this IEnumerable<T> points, float xValue)
                where T : class, IGraphPoint, new()
            {
              try {
                var left = points
                    .Select(
                        p => new
                        {
                          point = p,
                          distance = p.X - xValue
                        })
                    .Where(p => p.distance < 0)
                    .OrderBy(p => p.distance)
                    .Last()
                    .point;

                var right = points.FirstOrDefault(p => p.X > left.X);

                float yValue;
                if (right == null)
                  yValue = left.Y;
                else
                  yValue = left.Y + (right.Y - left.Y) * (xValue - left.X) / (right.X - left.X);

                return yValue;
              }
              catch {
                return float.NaN;
              }
            }
        */
    }
}