#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Gets a <see cref="DescriptionAttribute"/> attribute value from provided <see cref="Enum"/>
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
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        /// <summary>
        /// Gets all <see cref="DescriptionAttribute"/> values from provided type of <see cref="Enum"/>
        /// </summary>
        /// <param name="type">A type of the <see cref="Enum"/> to process</param>
        /// <returns>All description attribute values from provided type of <see cref="Enum"/></returns>
        public static string[] GetDescriptions(Type type)
        {
            return Enum.GetValues(type).Cast<Enum>().Select(en => en.GetDescription()).ToArray();
        }

        /// <summary>
        /// Gets all values from provided type of <see cref="Enum"/> as <see cref="Dictionary{T,String}"/>, where key is the enumeration value and value is the value of its <see cref="DescriptionAttribute"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Enum"/></typeparam>
        /// <param name="type">A type of the <see cref="Enum"/> to process</param>
        /// <returns>All description attribute values from provided type of <see cref="Enum"/> as <see cref="Dictionary{T,String}"/></returns>
        public static Dictionary<T, string> GetDescriptionsDictionary<T>(Type type) where T : Enum
        {
            if (type != typeof(T))
            {
                throw new ArgumentOutOfRangeException(nameof(type), "The provided type must match the generic type.");
            }

            var enums = Enum.GetValues(type);

            var resultDictionary = new Dictionary<T, string>(enums.Length);

            foreach (Enum en in enums)
            {
                resultDictionary.Add((T)en, en.GetDescription());
            }

            return resultDictionary;
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

        /// <summary>
        /// Gets all <see cref="DisplayAttribute"/> attribute values from provided type of <see cref="Enum"/>
        /// </summary>
        /// <param name="type">A type of the <see cref="Enum"/> to process</param>
        /// <returns>All display attribute values from provided type of <see cref="Enum"/></returns>
        public static string[] GetDisplayNames(Type type)
        {
            return GetDisplayNames(type, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets all <see cref="DisplayAttribute"/> attribute values from provided type of <see cref="Enum"/>
        /// </summary>
        /// <param name="type">A type of the <see cref="Enum"/> to process</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>All display attribute values from provided type of <see cref="Enum"/></returns>
        public static string[] GetDisplayNames(Type type, CultureInfo cultureInfo)
        {
            return Enum.GetValues(type).Cast<Enum>().Select(en => en.GetDisplayName(cultureInfo)).ToArray();
        }

        /// <summary>
        /// Gets all values from provided type of <see cref="Enum"/> as <see cref="Dictionary{T,String}"/>, where key is the enumeration value and value is the value of its <see cref="DisplayAttribute"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Enum"/></typeparam>
        /// <param name="type">A type of the <see cref="Enum"/> to process</param>
        /// <returns>All display attribute values from provided type of <see cref="Enum"/> as <see cref="Dictionary{T,String}"/></returns>
        public static Dictionary<T, string> GetDisplayNamesDictionary<T>(Type type) where T : Enum
        {
            return GetDisplayNamesDictionary<T>(type, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets all values from provided type of <see cref="Enum"/> as <see cref="Dictionary{T,String}"/>, where key is the enumeration value and value is the value of its <see cref="DisplayAttribute"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Enum"/></typeparam>
        /// <param name="type">A type of the <see cref="Enum"/> to process</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>All display attribute values from provided type of <see cref="Enum"/> as <see cref="Dictionary{T,String}"/></returns>
        public static Dictionary<T, string> GetDisplayNamesDictionary<T>(Type type, CultureInfo cultureInfo)
            where T : Enum
        {
            if (type != typeof(T))
            {
                throw new ArgumentOutOfRangeException(nameof(type), "The provided type must match the generic type.");
            }

            var enums = Enum.GetValues(type);

            var resultDictionary = new Dictionary<T, string>(enums.Length);

            foreach (Enum en in enums)
            {
                resultDictionary.Add((T)en, en.GetDisplayName(cultureInfo));
            }

            return resultDictionary;
        }

        /// <summary>
        /// Converts the target <see cref="Enum"/> to an array of its values
        /// </summary>
        /// <param name="type">A type of the <see cref="Enum"/> to process</param>
        /// <returns></returns>
        public static T[] ToArray<T>(Type type) where T : Enum
        {
            return Enum.GetValues(type).Cast<T>().ToArray();
        }
    }
}