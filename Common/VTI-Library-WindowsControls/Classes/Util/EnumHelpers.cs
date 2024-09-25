using System;
using System.Collections.Generic;

namespace VTIWindowsControlLibrary.Classes.Util
{
    /// <summary>
    /// Implements an array which can be indexed by an enumerated value.
    /// </summary>
    /// <typeparam name="E">Type of the enumeration to be used for the index.</typeparam>
    /// <typeparam name="T">Type to be used for the values.</typeparam>
    public class EnumArray<E, T>
    {
        private T[] _Values;

        /// <summary>
        /// Creates an instance of the EnumArray.
        /// </summary>
        public EnumArray()
        {
            _Values = new T[System.Enum.GetValues(typeof(E)).Length];
        }

        /// <summary>
        /// Returns a value from the EnumArray using an enumerated index
        /// </summary>
        /// <param name="x">Enumerated value indicating which value to return from the array</param>
        /// <returns>Value to be returned</returns>
        public T this[E x]
        {
            get
            {
                return _Values[Convert.ToInt32(x)];
            }
            set
            {
                _Values[Convert.ToInt32(x)] = value;
            }
        }

        /// <summary>
        /// Returns a value from the EnumArray using an integer index
        /// </summary>
        /// <param name="x">Index of the value to be returned</param>
        /// <returns>Value to be returned</returns>
        public T this[int x]
        {
            get
            {
                return _Values[x];
            }
            set
            {
                _Values[x] = value;
            }
        }
    }

    /// <summary>
    /// Generic helper class for Enums
    /// </summary>
    /// <typeparam name="T">Generic Enum type</typeparam>
    public static class Enum<T>
    {
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        ///     more enumerated constants to an equivalent enumerated object.
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type enumType whose value is represented by value.</returns>
        public static T Parse(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Retrieves a collection of the values of the constants in a specified enumeration
        /// that can be individually accessed by index.
        /// </summary>
        /// <returns>Collection of the values of the constants in a specified enumeration.</returns>
        public static IList<T> GetValues()
        {
            IList<T> list = new List<T>();
            foreach (object value in System.Enum.GetValues(typeof(T)))
            {
                list.Add((T)value);
            }
            return list;
        }
    }

    //public static class EnumExtensions
    //{
    //    public static T Next<T>(this T value)
    //    {
    //        int x = Convert.ToInt32(value);
    //        x++;
    //        if (x > System.Enum.GetValues(typeof(T)).Length) x = 0;

    //        return Enum<T>.Parse(x.ToString());
    //    }
    //}
}