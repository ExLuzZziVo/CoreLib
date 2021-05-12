#region

using System;
using Newtonsoft.Json;

#endregion

namespace CoreLib.STANDALONE.Types
{
    /// <summary>
    /// An object that is used for binding to datetime with 24-hour time format
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TimeDate : ViewModelBase
    {
        /// <summary>
        /// An object that is used for binding to datetime with 24-hour time format
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value that is used for object initialization</param>
        public TimeDate(DateTime? value)
        {
            var dateTime = value ?? new DateTime();
            Hours = dateTime.Hour;
            Minutes = dateTime.Minute;
            Date = dateTime.Date;
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> using <see cref="Date"/>, <see cref="Hours"/> and <see cref="Minutes"/>
        /// </summary>
        public DateTime GetDateTime => new DateTime(Date.Year, Date.Month, Date.Day, Hours, Minutes, 0);

        /// <summary>
        /// Hours value from 0 to 23
        /// </summary>
        [JsonProperty(nameof(Hours))]
        public int Hours
        {
            get => GetValue<int>();
            set
            {
                if (value > 23)
                {
                    value = 23;
                }

                if (value < 0)
                {
                    value = 0;
                }

                SetValue(value);
            }
        }

        /// <summary>
        /// Minutes value from 0 to 59
        /// </summary>
        [JsonProperty(nameof(Minutes))]
        public int Minutes
        {
            get => GetValue<int>();
            set
            {
                if (value > 59)
                {
                    value = 59;
                }

                if (value < 0)
                {
                    value = 0;
                }

                SetValue(value);
            }
        }

        [JsonProperty(nameof(Date))]
        public DateTime Date
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public string ToLongDateString()
        {
            return GetDateTime.ToLongDateString();
        }

        public string ToShortDateString()
        {
            return GetDateTime.ToShortDateString();
        }
    }
}