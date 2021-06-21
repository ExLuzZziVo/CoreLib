using System;
using Newtonsoft.Json;

namespace CoreLib.CORE.Helpers.Converters
{
    /// <summary>
    /// Converts unix timestamp to <see cref="System.DateTime"/> and back
    /// </summary>
    public class UnixTimestampConverter : JsonConverter<DateTime?>
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly bool _useLocalTime;
        
        /// <summary>
        /// Converts unix timestamp to <see cref="System.DateTime"/> and back
        /// </summary>
        /// <param name="useLocalTime"></param>
        public UnixTimestampConverter()
        { }
        
        /// <summary>
        /// Converts unix timestamp to local <see cref="System.DateTime"/> and back if the <paramref name="useLocalTime"/> is set to true
        /// </summary>
        /// <param name="useLocalTime">If true, the provided unix timestamp will be converted to local time and back</param>
        public UnixTimestampConverter(bool useLocalTime)
        {
            _useLocalTime = useLocalTime;
        }
        
        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(((_useLocalTime ? value.Value.ToUniversalTime() : value) - UnixEpoch).Value.TotalMilliseconds + "000");
            }
        }

        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            if (!long.TryParse(reader.Value.ToString(), out var value))
            {
                return null;
            }

            var result = UnixEpoch.AddMilliseconds(value);
            
            return _useLocalTime ? result.ToLocalTime() : result;
        }
    }
}