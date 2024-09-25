using System.Drawing;

namespace VTIWindowsControlLibrary.Classes.Graphing.Util
{
    /// <summary>
    /// Represents a class that can pick default colors for Data Plot traces.
    /// </summary>
    public class DefaultTraceColors
    {
        private static Color[] DefaultColors =
            new Color[] {
                Color.Red,
                Color.Black,
                Color.Sienna,
                Color.Green,
                Color.DeepPink,
                Color.Blue,
                Color.DarkMagenta,
                Color.DarkViolet
                };

        private static int index = 0;

        /// <summary>
        /// Gets the next color in the sequence of default colors.
        /// </summary>
        public static Color NextColor
        {
            get
            {
                int i;
                i = index;
                index++;
                if (index >= DefaultColors.Length) index = 0;
                return DefaultColors[i];
            }
        }

        /// <summary>
        /// Resets the default color back to the start of the list
        /// </summary>
        public static void Reset()
        {
            index = 0;
        }
    }
}