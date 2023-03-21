#region

using System;
using System.IO;
using System.Reflection;
using CoreLib.CORE.Helpers.AssemblyHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.ObjectHelpers
{
    public static class ObjectExtensions_Json
    {
        /// <summary>
        /// The default <see cref="JsonSerializerSettings"/> value that is used for <see cref="SerializeToJson(object)"/> and <see cref="DeserializeFromJson{T}(string)"/> methods
        /// </summary>
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace, TypeNameHandling = TypeNameHandling.All,
            SerializationBinder = new JsonSearchAssembliesBinder(Assembly.GetEntryAssembly(), true),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// Serializes the supplied object to json using the provided <see cref="Stream"/>
        /// </summary>
        /// <param name="value">Target object</param>
        /// <param name="s">A stream to write a json serialized <paramref name="value"/></param>
        public static void SerializeToJson(this object value, Stream s)
        {
            using (var sw = new StreamWriter(s))
            {
                using (var jwr = new JsonTextWriter(sw))
                {
                    var ser = new JsonSerializer();
                    ser.Serialize(jwr, value);
                }
            }
        }

        /// <summary>
        /// Deserializes an object from json using the provided <see cref="Stream"/>
        /// </summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="s">A stream to read a json serialized object from</param>
        /// <returns>Target object</returns>
        public static T DeserializeFromJson<T>(Stream s)
        {
            using (var sr = new StreamReader(s))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    var ser = new JsonSerializer();

                    return ser.Deserialize<T>(jr);
                }
            }
        }

        /// <summary>
        /// Serializes the provided object to json
        /// </summary>
        /// <param name="value">Target object</param>
        /// <returns>A json string representation of the object</returns>
        public static string SerializeToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, JsonSerializerSettings);
        }

        /// <summary>
        /// Deserializes an object from json
        /// </summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="source">A json string</param>
        /// <returns>An object deserialized from json</returns>
        public static T DeserializeFromJson<T>(string source)
        {
            return JsonConvert.DeserializeObject<T>(source, JsonSerializerSettings);
        }

        /// <summary>
        /// <see cref="DefaultSerializationBinder"/> that uses <see cref="SearchTypeHelper"/> to search object type
        /// </summary>
        private class JsonSearchAssembliesBinder : DefaultSerializationBinder
        {
            private readonly SearchTypeHelper _searchTypeHelper;

            /// <summary>
            /// <see cref="DefaultSerializationBinder"/> that uses <see cref="SearchTypeHelper"/> to search the type of object
            /// </summary>
            /// <param name="currentAssembly">Current assembly</param>
            /// <param name="searchInDlls">Enable search in dependent assemblies</param>
            public JsonSearchAssembliesBinder(Assembly currentAssembly, bool searchInDlls)
            {
                _searchTypeHelper = new SearchTypeHelper(currentAssembly, searchInDlls);
            }

            public override Type BindToType(string assemblyName, string typeName)
            {
                return _searchTypeHelper.GetTypeToDeserialize(typeName);
            }
        }
    }
}