#region

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    public class DateTimeValidatonAttribute : ValidationAttribute
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }

        public bool MinDateDateTimeNow { get; set; }
        public bool MaxDateDateTimeNow { get; set; }

        public override bool IsValid(object value)
        {
            if (MinDateDateTimeNow)
                MinDate = DateTime.Now;
            if (MaxDateDateTimeNow)
                MaxDate = DateTime.Now;
            if (MinDate == null && MaxDate == null)
                return true;
            DateTime dateTimeToCheck;
            try
            {
                dateTimeToCheck = Convert.ToDateTime(value);
            }
            catch
            {
                return false;
            }

            if (MinDate == null && MaxDate != null) return dateTimeToCheck <= MaxDate;

            if (MinDate != null && MaxDate == null) return dateTimeToCheck >= MinDate;

            return dateTimeToCheck <= MaxDate && dateTimeToCheck >= MinDate;
        }
    }
}