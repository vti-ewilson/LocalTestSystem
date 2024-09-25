using System.Collections.Generic;

namespace VTIWindowsControlLibrary.Classes.Util
{
    /// <summary>
    /// String helper class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates the string to maximum of a given length.  If the original
        /// string is less than <c>Length</c>, it is returned with no changes.
        /// </summary>
        /// <param name="Value">String to be truncated.</param>
        /// <param name="Length">Maximum length of the string to be returned.</param>
        /// <returns>The truncated string.</returns>
        public static string Truncate(this string Value, int Length)
        {
            if (string.IsNullOrEmpty(Value)) return string.Empty;
            else if (Value.Length <= Length) return Value;
            else return Value.Substring(0, Length);
        }

        /// <summary>
        /// Replaces instances of only one single quote with two single quotes. Used to avoid exceptions in SQL commands.
        /// </summary>
        public static string ReplaceOneSingleQuoteWithTwoSingleQuotes(this string Value)
        {
            // Replace "O'Connor''s watch" with "O'Connor's watch"
            string newValue = Value.Replace("''", "'");
            // Replace "O'Connor's watch" with "O''Connor''s watch"
            newValue = newValue.Replace("'", "''");
            return newValue;
        }
    }
}