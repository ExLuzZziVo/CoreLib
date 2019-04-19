#region

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using CoreLib.CORE.Helpers.AssemblyHelpers;

#endregion

namespace CoreLib.CORE.Helpers.ObjectHelpers
{
    public static class ObjectExtensions
    {
        public static void SetPropertyValueByName(this object obj, string propertyName, object value)
        {
            var type = obj.GetType();

            if (propertyName.Contains("."))
            {
                var array = propertyName.Split('.');
                if (array.Length < 2)
                    throw new ArgumentOutOfRangeException(propertyName, $"Wrong format of property: {propertyName}");
                var x = type.GetProperty(array[0]);
                var y = x.GetValue(obj, null);

                y.SetPropertyValueByName(array[1], value);
            }
            else
            {
                var propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(propertyName,
                        $"There is no property with name {propertyName} in object {type.FullName}");
                propertyInfo.SetValue(obj, value, null);
            }
        }

        public static string GetPropertyDescription(this PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
                try
                {
                    return ((DescriptionAttribute) propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .First()).Description;
                }
                catch
                {
                    return propertyInfo.Name;
                }

            return null;
        }

        public static object GetPropertyValueByName(this object obj, string name)
        {
            foreach (var part in name.Split('.'))
            {
                if (obj == null) return null;

                var type = obj.GetType();
                var info = type.GetProperty(part);
                if (info == null) return null;

                obj = info.GetValue(obj, null);
            }

            return obj;
        }

        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T GetObject<T>(this byte[] data)
        {
            if (data == null)
                return default;
            using (var ms = new MemoryStream(data))
            {              
                return (T)new BinaryFormatter().Deserialize(ms);
            }
        }

        public static T GetObject<T>(this byte[] data, Assembly assembly)
        {
            if (data == null)
                return default;
            using (var ms = new MemoryStream(data))
            {
                var obj = new BinaryFormatter {Binder = new SearchAssembliesBinder(assembly, true)}.Deserialize(ms);
                return (T) obj;
            }
        }

        public static void CopyTo(this object source, object destination)
        {
            var typeDest = destination.GetType();
            var typeSrc = source.GetType();
            var results = from srcProp in typeSrc.GetProperties()
                let targetProperty = typeDest.GetProperty(srcProp.Name)
                where srcProp.CanRead && targetProperty?.GetSetMethod(true) != null &&
                      !targetProperty.GetSetMethod(true).IsPrivate &&
                      (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0 &&
                      targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                select new {sourceProperty = srcProp, targetProperty};
            foreach (var props in results)
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
        }
    }
}