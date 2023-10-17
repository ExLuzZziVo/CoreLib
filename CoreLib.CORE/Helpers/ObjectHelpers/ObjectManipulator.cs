#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.ObjectHelpers
{
    public static class ObjectManipulator
    {
        /// <summary>
        /// Global lock object
        /// </summary>
        public static readonly object Locker = new object();

        // https://stackoverflow.com/a/18939148
        /// <summary>
        /// Hex nibble
        /// </summary>
        private static readonly byte[] HexNibble =
        {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7,
            0x8, 0x9, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF
        };

        /// <summary>
        /// Gets all <see cref="DescriptionAttribute"/> values of properties from the specified object
        /// </summary>
        /// <returns>List of <see cref="DescriptionAttribute"/> values</returns>
        public static IEnumerable<string> GetPropertyDescriptions<T>()
        {
            var attributes = typeof(T).GetMembers()
                .SelectMany(member =>
                    member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                .ToArray();

            return attributes.Select(x => x.Description);
        }

        /// <summary>
        /// Compares two object by property values
        /// </summary>
        /// <param name="object1">First object</param>
        /// <param name="object2">Second object</param>
        /// <returns>True if property values of the objects are equal</returns>
        public static bool CompareObjects<T>(T object1, T object2)
        {
            var type = typeof(T);

            if (Equals(object1, default(T)) || Equals(object2, default(T)))
            {
                return false;
            }

            foreach (var property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    var object1Value = string.Empty;
                    var object2Value = string.Empty;

                    if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                    {
                        object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                    }

                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                    {
                        object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    }

                    if (object1Value.Trim() != object2Value.Trim())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Converts a hex string to a byte array
        /// </summary>
        /// <param name="hex">Hex string that will be converted to a byte array. Length must be even</param>
        /// <returns>A byte array obtained from a hex string</returns>
        public static byte[] GetDataFromHexString(string hex)
        {
            if (hex.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(hex));
            }

            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Hex value length must be even", nameof(hex));
            }

            var data = new byte[hex.Length / 2];

            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)((HexNibble[hex[i << 1] - 48] << 4) | HexNibble[hex[(i << 1) + 1] - 48]);
            }

            return data;
        }
    }
}