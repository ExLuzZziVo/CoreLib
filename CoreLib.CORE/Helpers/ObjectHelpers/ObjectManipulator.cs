#region

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#endregion

namespace CoreLib.CORE.Helpers.ObjectHelpers
{
    public static class ObjectManipulator
    {
        public static IEnumerable<string> GetPropertyDescriptions<T>()
        {
            var attributes = typeof(T).GetMembers()
                .SelectMany(member =>
                    member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                .ToList();

            return attributes.Select(x => x.Description);
        }

        public static bool CompareObjects<T>(T object1, T object2)
        {
            var type = typeof(T);
            if (Equals(object1, default(T)) || Equals(object2, default(T)))
                return false;
            foreach (var property in type.GetProperties())
                if (property.Name != "ExtensionData")
                {
                    var object1Value = string.Empty;
                    var object2Value = string.Empty;
                    if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                        object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                        object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    if (object1Value.Trim() != object2Value.Trim())
                        return false;
                }

            return true;
        }
    }
}