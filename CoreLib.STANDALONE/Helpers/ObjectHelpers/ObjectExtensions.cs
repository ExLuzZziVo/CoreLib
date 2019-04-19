#region

using System.IO;
using Newtonsoft.Json;

#endregion

namespace CoreLib.STANDALONE.Helpers.ObjectHelpers
{
    public static class ObjectExtensions
    {
        public static void SerializeToJson(this object value, Stream s)
        {
            using (var sw = new StreamWriter(s))
            using (var jwr = new JsonTextWriter(sw))
            {
                var ser = new JsonSerializer();
                ser.Serialize(jwr, value);
            }
        }

        public static T DeserializeFromJson<T>(Stream s)
        {
            using (var sr = new StreamReader(s))
            using (var jr = new JsonTextReader(sr))
            {
                var ser = new JsonSerializer();
                return ser.Deserialize<T>(jr);
            }
        }
    }
}