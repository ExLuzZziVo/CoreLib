#region

using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using CoreLib.CORE.Resources;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to validate the minimum and maximum length of a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class RangeLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// This validation attribute is used to validate the minimum and maximum length of a property
        /// </summary>
        /// <param name="minimum">Minimum value for the length. Must me greater than 0</param>
        /// <param name="maximum">Maximum value for the length. Must me greater than 0</param>
        public RangeLengthAttribute(int minimum, int maximum) : base(
            ValidationStrings.ResourceManager.GetString("CollectionRangeLengthError"))
        {
            if (minimum < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimum));
            }

            if (maximum < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximum));
            }

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

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum, Maximum);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            if (value == null)
            {
                return ValidationResult.Success;
            }

            int length;

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

            return length >= Minimum && length <= Maximum
                ? ValidationResult.Success
                : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}