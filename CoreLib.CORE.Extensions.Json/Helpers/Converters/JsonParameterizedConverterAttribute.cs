#region

using System;
using System.Text.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    public class JsonParameterizedConverterAttribute : JsonConverterAttribute
    {
        private readonly object[] _converterParameters;
        private readonly Type _converterType;

        public JsonParameterizedConverterAttribute(Type converterType, params object[] converterParameters)
        {
            if (!typeof(JsonConverter).IsAssignableFrom(converterType))
            {
                throw new ArgumentException("The converter type must be derived from the JsonConverter class");
            }

            _converterType = converterType;
            _converterParameters = converterParameters;
        }

        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            return (JsonConverter)Activator.CreateInstance(_converterType, _converterParameters);
        }
    }
}