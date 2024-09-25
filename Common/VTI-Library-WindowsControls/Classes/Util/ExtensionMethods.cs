using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace VTIWindowsControlLibrary.Classes.Util
{
    /// <summary>
    /// Static extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}">enumeration</see>.
        /// </summary>
        /// <typeparam name="T">Type of item in the list</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="action">Action to be performed on each item</param>
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            values.ToList().ForEach(action);
        }

        /// <summary>
        /// Calculates the median value for any list of double-precision numbers
        /// </summary>
        /// <param name="source">List of numbers</param>
        /// <returns>Median value</returns>
        public static double Median(this IEnumerable<double> source)
        {
            if (source.Count() == 0)
            {
                throw new InvalidOperationException("Cannot compute median for an empty set.");
            }

            var sortedList = from number in source
                             orderby number
                             select number;

            int itemIndex = sortedList.Count() / 2;

            if (sortedList.Count() % 2 == 0)
            {
                // Even number of items.
                return (sortedList.ElementAt(itemIndex) + sortedList.ElementAt(itemIndex - 1)) / 2;
            }
            else
            {
                // Odd number of items.
                return sortedList.ElementAt(itemIndex);
            }
        }

        /// <summary>
        /// Calculates the median value for any list of single-precision numbers
        /// </summary>
        /// <param name="source">List of numbers</param>
        /// <returns>Median value</returns>
        public static float Median(this IEnumerable<float> source)
        {
            //return (float)source.Select(n => (double)n).Median();
            return (float)(from num in source select (double)num).Median();
        }

        /// <summary>
        /// Attempts to convert the specified string to the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="s">The string to be parsed.</param>
        /// <param name="result">The result.</param>
        /// <returns>A value indicating whether the string was successfully parsed.</returns>
        public static bool TryParse<T>(string s, out T result)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                try
                {
                    result = (T)converter.ConvertFromString(s);
                    return true;
                }
                catch (Exception)
                {
                }
            }
            result = default(T);
            return false;
        }
    }
}