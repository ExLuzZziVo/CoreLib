#region

using Newtonsoft.Json.Converters;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    /// <summary>
    /// Converts <see cref="System.DateTime"/> to the specified format and back
    /// </summary>
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Converts <see cref="System.DateTime"/> to the specified format and back
        /// </summary>
        /// <param name="dateTimeFormat">DateTime format to convert from and to</param>
        public CustomDateTimeConverter(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }
    }
}