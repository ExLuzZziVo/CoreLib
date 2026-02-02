#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using CoreLib.CORE.Helpers.AssemblyHelpers;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.ObjectHelpers
{
    public static class ObjectExtensions
    {
        // https://stackoverflow.com/a/24343727/48700
        /// <summary>
        /// Hex string lookup table
        /// </summary>
        private static readonly Lazy<uint[]> HexStringTable = new Lazy<uint[]>(() =>
        {
            var result = new uint[256];

            for (var i = 0; i < 256; i++)
            {
                var s = i.ToString("X2");
                result[i] = s[0] + ((uint)s[1] << 16);
            }

            return result;
        });

        /// <summary>
        /// Sets property value by its name
        /// </summary>
        /// <param name="obj">Target object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="value">Property value</param>
        public static void SetPropertyValueByName(this object obj, string propertyName, object value)
        {
            var type = obj.GetType();

            if (propertyName.Contains("."))
            {
                var array = propertyName.Split('.');

                if (array.Length < 2)
                {
                    throw new ArgumentOutOfRangeException(propertyName,
                        $"Invalid format of property name: {propertyName}");
                }

                var propertyInfo = type.GetProperty(array[0]);
                var propertyObject = propertyInfo.GetValue(obj, null);

                propertyObject.SetPropertyValueByName(string.Join(".", array.Skip(1).ToArray()), value);
            }
            else
            {
                var propertyInfo = type.GetProperty(propertyName);

                if (propertyInfo == null)
                {
                    throw new ArgumentOutOfRangeException(propertyName,
                        $"There is no property with name {propertyName} in object {type.FullName}");
                }

                propertyInfo.SetValue(obj, value, null);
            }
        }

        /// <summary>
        /// Gets property value by its name
        /// </summary>
        /// <param name="obj">Target object</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property value</returns>
        public static object GetPropertyValueByName(this object obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (propertyName.IsNullOrEmptyOrWhiteSpace() || propertyName == ".")
            {
                return obj;
            }

            var type = obj.GetType();

            if (propertyName.Contains("."))
            {
                var array = propertyName.Split('.');

                if (array.Length < 2)
                {
                    throw new ArgumentOutOfRangeException(propertyName,
                        $"Invalid format of property name: {propertyName}");
                }

                var propertyInfo = type.GetProperty(array[0]);
                var propertyObject = propertyInfo.GetValue(obj, null);

                return propertyObject.GetPropertyValueByName(string.Join(".", array.Skip(1).ToArray()));
            }
            else
            {
                var propertyDescriptor = type.GetProperty(propertyName);

                if (propertyDescriptor == null)
                {
                    throw new ArgumentOutOfRangeException(propertyName);
                }

                return propertyDescriptor.GetValue(obj);
            }
        }

        /// <summary>
        /// Gets property value by its name
        /// </summary>
        /// <param name="obj">Target object</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property value</returns>
        public static T GetPropertyValueByName<T>(this object obj, string propertyName)
        {
            return (T)obj.GetPropertyValueByName(propertyName);
        }

        /// <summary>
        /// Gets property value by its name
        /// </summary>
        /// <param name="obj">Target object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="value">When this method returns, contains the value associated with the specified <paramref name="propertyName"/>, if it was found. Otherwise, the default value for the type of the value parameter</param>
        /// <returns>True if <paramref name="obj"/> contains a property with the specified name and type</returns>
        public static bool TryGetPropertyValueByName<T>(this object obj, string propertyName, out T value)
        {
            try
            {
                value = obj.GetPropertyValueByName<T>(propertyName);

                return true;
            }
            catch
            {
                value = default;

                return false;
            }
        }

        /// <summary>
        /// Gets a <see cref="DescriptionAttribute"/> value from provided property
        /// </summary>
        /// <param name="propertyInfo">Property info</param>
        /// <returns>Description attribute value from provided property</returns>
        public static string GetPropertyDescription(this PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                try
                {
                    return ((DescriptionAttribute)propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                        [0]).Description;
                }
                catch
                {
                    return propertyInfo.Name;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a <see cref="DisplayAttribute"/> value from provided property using <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="propertyInfo">Property info</param>
        /// <returns>Display attribute value from provided property</returns>
        public static string GetPropertyDisplayName(this PropertyInfo propertyInfo)
        {
            return GetPropertyDisplayName(propertyInfo, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets a <see cref="DisplayAttribute"/> value from provided property using <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="propertyInfo">Property info</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>Display attribute value from provided property</returns>
        public static string GetPropertyDisplayName(this PropertyInfo propertyInfo, CultureInfo cultureInfo)
        {
            if (propertyInfo != null)
            {
                var displayAttr = (DisplayAttribute)propertyInfo
                    .GetCustomAttributes(typeof(DisplayAttribute), false)
                    .FirstOrDefault();

                var resourceManager =
                    displayAttr?.ResourceType
                        ?.GetProperty(@"ResourceManager",
                            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        ?.GetValue(null, null) as ResourceManager;

                return resourceManager?.GetString(displayAttr.Name, cultureInfo) ??
                       displayAttr?.GetName() ?? propertyInfo.Name;
            }

            return null;
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Converts an object to byte array
        /// </summary>
        /// <param name="obj">Target object</param>
        /// <returns>Byte array that represent <paramref name="obj"/></returns>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets an object from byte array
        /// </summary>
        /// <param name="data">Byte array that represent an object</param>
        /// <typeparam name="T">Target object type</typeparam>
        /// <returns>Target object</returns>
        public static T GetObject<T>(this byte[] data)
        {
            if (data == null)
            {
                return default;
            }

            using (var ms = new MemoryStream(data))
            {
                return (T)new BinaryFormatter().Deserialize(ms);
            }
        }

        /// <summary>
        /// Gets an object from byte array using specified assembly to search
        /// </summary>
        /// <param name="data">Byte array that represent an object</param>
        /// <param name="assembly">The assembly in which the object is located</param>
        /// <typeparam name="T">Target object type</typeparam>
        /// <returns>Target object</returns>
        public static T GetObject<T>(this byte[] data, Assembly assembly)
        {
            if (data == null)
            {
                return default;
            }

            using (var ms = new MemoryStream(data))
            {
                var obj = new BinaryFormatter { Binder = new SearchAssembliesBinder(assembly, true) }.Deserialize(ms);

                return (T)obj;
            }
        }
#endif

        /// <summary>
        /// Changes type of object
        /// </summary>
        /// <param name="obj">Target object</param>
        /// <typeparam name="T">Target type</typeparam>
        /// <returns>Target object of the specified type</returns>
        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// Checks if the first object is greater than the second
        /// </summary>
        /// <param name="value">First object</param>
        /// <param name="other">Second object</param>
        /// <typeparam name="T">Object type that implements the <see cref="IComparable"/> interface</typeparam>
        /// <returns>True if the first object is greater than the second</returns>
        public static bool IsGreaterThan<T>(this T value, T other) where T : IComparable
        {
            return value.CompareTo(other) > 0;
        }

        /// <summary>
        /// Checks if the first object is less than the second
        /// </summary>
        /// <param name="value">First object</param>
        /// <param name="other">Second object</param>
        /// <typeparam name="T">Object type that implements the <see cref="IComparable"/> interface</typeparam>
        /// <returns>True if the first object is less than the second</returns>
        public static bool IsLessThan<T>(this T value, T other) where T : IComparable
        {
            return value.CompareTo(other) < 0;
        }

        /// <summary>
        /// Converts provided string to target structure
        /// </summary>
        /// <param name="input">A string containing an object to convert</param>
        /// <param name="parsedValue">Object reference for conversion result. If conversion failed the result object will have a default value</param>
        /// <typeparam name="T">Structure</typeparam>
        /// <returns>True if provided value was converted successfully</returns>
        public static bool TryParse<T>(this string input, out T parsedValue) where T : struct
        {
            parsedValue = default;

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                var value = converter.ConvertFromString(input);

                if (value == null)
                {
                    return false;
                }

                parsedValue = (T)value;

                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a deep copy of an object asynchronously
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <returns>A task that represents the asynchronous deep copy of an object operation</returns>
        public static Task CopyToAsync<T>(this T source, T destination)
        {
            return Task.Run(() => CopyTo(source, destination));
        }

        /// <summary>
        /// Creates a deep copy of an object
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public static void CopyTo<T>(this T source, T destination)
        {
            var typeDest = destination.GetType();
            var typeSrc = source.GetType();

            var results = from srcProp in typeSrc.GetProperties()
                let targetProperty = typeDest.GetProperty(srcProp.Name)
                where srcProp.CanRead && targetProperty?.GetSetMethod(true) != null &&
                      !targetProperty.GetSetMethod(true).IsPrivate &&
                      (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0 &&
                      targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                select new { sourceProperty = srcProp, targetProperty };

            foreach (var props in results)
            {
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }

        /// <summary>
        /// Checks if specified value is in provided range
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="value1">Range start value</param>
        /// <param name="value2">Range end value</param>
        /// <typeparam name="T">A type that implements the <see cref="IComparable{T}"/> interface</typeparam>
        /// <returns>True if specified value is in provided range</returns>
        public static bool IsInRange<T>(this T value, T value1, T value2) where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(value2, value1) < 0)
            {
                return value.IsInRange(value2, value1);
            }

            return Comparer<T>.Default.Compare(value, value1) >= 0 && Comparer<T>.Default.Compare(value, value2) <= 0;
        }

        /// <summary>
        /// Converts a byte array to a hex string
        /// </summary>
        /// <param name="data">Byte array that will be converted to a hex string</param>
        /// <returns>Hex representation of the provided byte array</returns>
        public static string ToHexString(this byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                return string.Empty;
            }

            var result = new char[data.Length * 2];

            for (var i = 0; i < data.Length; i++)
            {
                var val = HexStringTable.Value[data[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }

            return new string(result);
        }

        // https://stackoverflow.com/questions/124411/
        /// <summary>
        /// Checks if specified type is nullable
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if specified type is nullable</returns>
        public static bool IsNullable(this Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        // https://stackoverflow.com/questions/124411/
        /// <summary>
        /// Checks if specified type is numeric
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if specified type is numeric</returns>
        public static bool IsNumeric(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            var nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType != null)
            {
                return nullableType.IsNumeric();
            }

            return Type.GetTypeCode(type) switch
            {
                TypeCode.SByte or
                    TypeCode.Byte or
                    TypeCode.Int16 or
                    TypeCode.Int32 or
                    TypeCode.Int64 or
                    TypeCode.UInt16 or
                    TypeCode.UInt32 or
                    TypeCode.UInt64 or
                    TypeCode.Decimal or
                    TypeCode.Double or
                    TypeCode.Single => true,
                _ => false
            };
        }
    }
}
