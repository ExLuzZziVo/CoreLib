#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    // https://stackoverflow.com/a/66282422
    /// <summary>
    /// Serializes and deserializes two-dimensional arrays
    /// </summary>
    internal class TwoDimensionalArrayConverter<T>: JsonConverter<T[,]>
    {
        public override T[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var elements = JsonSerializer.Deserialize<List<List<T>>>(ref reader, options);

            var firstDimension = elements.Count;
            var secondDimension = elements.Select(row => row.Count).FirstOrDefault();

            if (elements.Any(row => row.Count != secondDimension))
            {
                throw new JsonException("The object is invalid two-dimensional array.");
            }

            var result = new T[firstDimension, firstDimension];

            for (var i = 0; i < firstDimension; i++)
            {
                for (var j = 0; j < elements[i].Count; j++)
                {
                    result[i, j] = elements[i][j];
                }
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, T[,] value, JsonSerializerOptions options)
        {
            var rowsFirstIndex = value.GetLowerBound(0);
            var rowsLastIndex = value.GetUpperBound(0);

            var columnsFirstIndex = value.GetLowerBound(1);
            var columnsLastIndex = value.GetUpperBound(1);

            writer.WriteStartArray();

            for (var i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                writer.WriteStartArray();

                for (var j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    JsonSerializer.Serialize(writer, value[i, j], options);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsArray && typeToConvert.GetArrayRank() == 2;
        }
    }
}