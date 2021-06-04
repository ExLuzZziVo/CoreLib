#region

using System;

#endregion

namespace CoreLib.CORE.Helpers.IntHelpers
{
    public static class IntExtensions
    {
        /// <summary>
        /// Data storage units
        /// </summary>
        public enum SizeUnits : byte
        {
            Byte,
            KB,
            MB,
            GB,
            TB,
            PB,
            EB,
            ZB,
            YB
        }

        /// <summary>
        /// Checks if specified value is even
        /// </summary>
        /// <param name="source">Value to process</param>
        /// <returns>True if specified value is even</returns>
        public static bool IsEven(this int source)
        {
            return source % 2 == 0;
        }

        /// <summary>
        /// Converts value to specified size units
        /// </summary>
        /// <param name="value">Value to process</param>
        /// <param name="unit">Size unit</param>
        /// <returns>String that represents size of the <paramref name="value"/> in specified <paramref name="unit"/>s</returns>
        public static string ToFileSize(this long value, SizeUnits unit)
        {
            return (value / Math.Pow(1024, (long) unit)).ToString("0.00") + unit;
        }

    }
}