#region

using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    /// <summary>
    /// Converts <see cref="IPAddress"/> to <see cref="string"/> and back
    /// </summary>
    public class IpAddressConverter : JsonConverter<IPAddress>
    {
        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var str = reader.GetString();

            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            if (!IPAddress.TryParse(str, out var ipAddress))
            {
                return null;
            }

            return ipAddress;
        }
    }
}