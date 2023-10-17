#region

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    public class JsonUpperCaseStringEnumConverter : JsonStringEnumConverter
    {
        public JsonUpperCaseStringEnumConverter() : this(true) { }
        
        public JsonUpperCaseStringEnumConverter(bool allowIntegerValues = true) : base(UpperCaseNamingPolicy,
            allowIntegerValues) { }

        public static JsonNamingPolicy UpperCaseNamingPolicy { get; } = new UpperCaseNamingPolicy();
    }
}