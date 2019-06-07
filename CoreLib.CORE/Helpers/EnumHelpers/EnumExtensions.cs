#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#endregion

namespace CoreLib.CORE.Helpers.EnumHelpers
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());
            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0) return ((DescriptionAttribute) attrs[0]).Description;
            }

            return en.ToString();
        }

        public static IEnumerable<string> GetDescriptions(Type type)
        {
            var names = Enum.GetNames(type);
            return (from name in names select type.GetField(name) into field from DescriptionAttribute fd in field.GetCustomAttributes(typeof(DescriptionAttribute), true) select fd.Description).ToList();
        }
    }
}