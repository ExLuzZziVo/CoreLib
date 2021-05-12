#region

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#endregion

namespace CoreLib.CORE.Helpers.ObjectHelpers
{
    public static class ObjectManipulator
    {
        /// <summary>
        /// Global lock object
        /// </summary>
        public static readonly object Locker = new object();

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
    }
}