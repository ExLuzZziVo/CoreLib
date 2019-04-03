#region

using System;
using System.ComponentModel;

#endregion

namespace UIServiceLib.CORE.Helpers.EnumHelpers
{
    public static class EnumManipulator
    {
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T) field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T) field.GetValue(null);
                }

            throw new ArgumentException("Not found exception", $"Description \"{description}\" not found!");
        }
    }
}