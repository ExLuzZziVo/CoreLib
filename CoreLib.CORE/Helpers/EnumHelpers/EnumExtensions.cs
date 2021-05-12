#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

#endregion

namespace CoreLib.CORE.Helpers.EnumHelpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets a description attribute value from provided <see cref="Enum"/>
        /// </summary>
        /// <param name="en">An <see cref="Enum"/> to process</param>
        /// <returns>Description attribute value from provided <see cref="Enum"/></returns>
        public static string GetDescription(this Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute) attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        /// <summary>
        /// Gets all description attribute values from provided type of <see cref="Enum"/>
        /// </summary>
        /// <param name="type">A type of the <see cref="Enum"/> to process</param>
        /// <returns>All description attribute values from provided type of <see cref="Enum"/></returns>
        public static IEnumerable<string> GetDescriptions(Type type)
        {
            var names = Enum.GetNames(type);

            return (from name in names
                select type.GetField(name)
                into field
                from DescriptionAttribute fd in field.GetCustomAttributes(typeof(DescriptionAttribute), true)
                select fd.Description).ToArray();
        }

        /// <summary>
        /// Gets a <see cref="DisplayAttribute"/> value from provided <see cref="Enum"/> using <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="en">An <see cref="Enum"/> to process</param>
        /// <returns>Display attribute value from provided <see cref="Enum"/></returns>
        public static string GetDisplayName(this Enum en)
        {
            return GetDisplayName(en, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets a <see cref="DisplayAttribute"/> value from provided <see cref="Enum"/> using <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="en">An <see cref="Enum"/> to process</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>Display attribute value from provided <see cref="Enum"/></returns>
        public static string GetDisplayName(this Enum en, CultureInfo cultureInfo)
        {
            var displayAttr = en
                .GetType()
                .GetMember(en.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            var resourceManager =
                displayAttr?.ResourceType
                    ?.GetProperty(@"ResourceManager",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    ?.GetValue(null, null) as ResourceManager;

            return resourceManager?.GetString(displayAttr.Name, cultureInfo) ?? displayAttr?.GetName() ?? en.ToString();
        }
    }
}