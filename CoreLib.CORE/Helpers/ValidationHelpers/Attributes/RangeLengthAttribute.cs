using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to validate the minimum and maximum length of a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeLengthValidationAttribute : ValidationAttribute
    {
        public RangeLengthValidationAttribute(int minimum, int maximum)
        {
            if (maximum < minimum)
            {
                Minimum = maximum;
                Maximum = minimum;
            }
            else
            {
                Minimum = minimum;
                Maximum = maximum;
            }
        }

        /// <summary>
        /// Minimum value for the length
        /// </summary>
        public int Minimum { get; }

        /// <summary>
        /// Maximum value for the length
        /// </summary>
        public int Maximum { get; }

        public override bool IsValid(object value)
        {
            if (Maximum < 0)
            {
                return false;
            }

            int length;

            if (value == null)
            {
                return true;
            }

            if (value is string str)
            {
                length = str.Length;
            }
            else if (value is ICollection col)
            {
                length = col.Count;
            }
            else
            {
                var countProperty = value.GetType().GetRuntimeProperty("Count");

                if (countProperty != null && countProperty.CanRead && countProperty.PropertyType == typeof(int))
                {
                    length = (int)countProperty.GetValue(value);
                }
                else
                {
                    throw new NotSupportedException(
                        "This validation attribute doesn't support the specified property type");
                }
            }

            return length >= Minimum && length <= Maximum;
        }
    }
}