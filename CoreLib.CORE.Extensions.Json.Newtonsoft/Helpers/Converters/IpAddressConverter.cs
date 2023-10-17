#region

using System;
using System.Net;
using Newtonsoft.Json;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    /// <summary>
    /// Converts <see cref="IPAddress"/> to <see cref="string"/> and back
    /// </summary>
    public class IpAddressConverter : JsonConverter<IPAddress>
    {
        public override void WriteJson(JsonWriter writer, IPAddress value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.ToString());
            }
        }

        public override IPAddress ReadJson(JsonReader reader, Type objectType, IPAddress existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            if (!IPAddress.TryParse(reader.Value.ToString(), out var ipAddress))
            {
                return null;
            }

            return ipAddress;
        }
    }
}