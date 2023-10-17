#region

using System;
using System.ComponentModel;

#endregion

namespace CoreLib.CORE.Helpers.EnumHelpers
{
    public static class EnumManipulator
    {
        /// <summary>
        /// Gets enum value by its <see cref="DescriptionAttribute"/> value
        /// </summary>
        /// <param name="description">Value of <see cref="DescriptionAttribute"/></param>
        /// <typeparam name="T">Must be enum</typeparam>
        /// <returns>Enum value that has <see cref="DescriptionAttribute"/> with <paramref name="description"/> value</returns>
        /// <exception cref="InvalidOperationException">Throws if <typeparamref name="T"/> is not enum</exception>
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            return (T)Enum.ToObject(typeof(T), 0);
        }
    }
}