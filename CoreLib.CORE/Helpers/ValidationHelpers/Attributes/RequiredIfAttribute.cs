#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using CoreLib.CORE.Helpers.ObjectHelpers;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to validate required based on other property value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfAttribute : RequiredAttribute
    {
        private readonly object _instance = new object();

        /// <summary>
        /// This validation attribute is used to validate required based on other property value
        /// </summary>
        /// <param name="otherPropertyName">The name of the other property</param>
        /// <param name="otherPropertyValue">The value of the other property</param>
        /// <param name="comparisonType">Comparison type. Default value: <see cref="ComparisonType.Equal"/></param>
        public RequiredIfAttribute(string otherPropertyName, object otherPropertyValue,
            ComparisonType comparisonType = ComparisonType.Equal)
        {
            if (otherPropertyName.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(otherPropertyName));
            }

            OtherPropertyName = otherPropertyName;
            OtherPropertyValue = otherPropertyValue;
            ComparisonType = comparisonType;
        }

        /// <summary>
        /// The name of the other property
        /// </summary>
        public string OtherPropertyName { get; }

        /// <summary>
        /// The value of the other property
        /// </summary>
        public object OtherPropertyValue { get; }

        /// <summary>
        /// Comparison type
        /// </summary>
        public ComparisonType ComparisonType { get; }

        /// <summary>
        /// Flag indicating that the target <see cref="string"/> property is a html string and the inner text should be validated
        /// </summary>
        public bool IsStringHtmlText { get; set; }

        public override object TypeId => _instance;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            var otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);

            if (otherProperty == null)
            {
                throw new NullReferenceException($"The object has no property with '{OtherPropertyName}' name");
            }

            var currentOtherPropertyType =
                Nullable.GetUnderlyingType(otherProperty.PropertyType) ?? otherProperty.PropertyType;

            var currentOtherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance);

            var isRequired = true;

            if (!(currentOtherPropertyValue is string str))
            {
                if (OtherPropertyValue == null && currentOtherPropertyValue == null &&
                    ComparisonType == ComparisonType.Equal)
                {
                    isRequired = true;
                }
                else if (OtherPropertyValue == null && currentOtherPropertyValue != null &&
                         ComparisonType == ComparisonType.NotEqual)
                {
                    isRequired = true;
                }
                else if (OtherPropertyValue != null && currentOtherPropertyValue == null &&
                         ComparisonType == ComparisonType.NotEqual)
                {
                    isRequired = true;
                }
                else if (OtherPropertyValue == null || currentOtherPropertyValue == null)
                {
                    isRequired = false;
                }
                else if (currentOtherPropertyValue.GetType().IsNumeric())
                {
                    isRequired = CompareToAttribute.CompareValues(
                        Convert.ToDecimal(currentOtherPropertyValue, CultureInfo.InvariantCulture),
                        Convert.ToDecimal(OtherPropertyValue, CultureInfo.InvariantCulture), ComparisonType);
                }
                else if (currentOtherPropertyValue is DateTime dateTime)
                {
                    isRequired = CheckIfOtherPropertyValueIsStringAndCompare(dateTime);
                }
                else if (currentOtherPropertyValue is DateTimeOffset dateTimeOffset)
                {
                    isRequired = CheckIfOtherPropertyValueIsStringAndCompare(dateTimeOffset);
                }
#if NET6_0_OR_GREATER
                else if (currentOtherPropertyValue is DateOnly dateOnly)
                {
                    isRequired = CheckIfOtherPropertyValueIsStringAndCompare(dateOnly);
                }
                else if (currentOtherPropertyValue is TimeOnly timeOnly)
                {
                    isRequired = CheckIfOtherPropertyValueIsStringAndCompare(timeOnly);
                }
#endif
                else if (currentOtherPropertyValue is TimeSpan timeSpan)
                {
                    isRequired = CheckIfOtherPropertyValueIsStringAndCompare(timeSpan);
                }
                else if (currentOtherPropertyValue is bool boolean)
                {
                    if (ComparisonType == ComparisonType.Equal || ComparisonType == ComparisonType.NotEqual)
                    {
                        isRequired = CompareToAttribute.CompareValues(boolean, OtherPropertyValue, ComparisonType);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            $"The comparison type '{ComparisonType.ToString("G")}' is not supported for the '{nameof(Boolean)}' property");
                    }
                }
                else
                {
                    throw new NotSupportedException($"The type {currentOtherPropertyType} is not supported");
                }
            }
            else
            {
                if (ComparisonType == ComparisonType.Equal || ComparisonType == ComparisonType.NotEqual)
                {
                    if (OtherPropertyValue == null)
                    {
                        isRequired =
                            CompareToAttribute.CompareValues(str.IsNullOrEmptyOrWhiteSpace(), true, ComparisonType);
                    }
                    else
                    {
                        isRequired =
                            CompareToAttribute.CompareValues(str, OtherPropertyValue.ToString(), ComparisonType);
                    }
                }
                else
                {
                    throw new NotSupportedException(
                        $"The comparison type '{ComparisonType.ToString("G")}' is not supported for the '{nameof(String)}' property");
                }
            }

            if (IsStringHtmlText && value is string val)
            {
                value = Regex.Replace(val, @"<.*?>|&nbsp;", string.Empty);
            }

            return isRequired ? base.IsValid(value, validationContext) : ValidationResult.Success;
        }

        private bool CheckIfOtherPropertyValueIsStringAndCompare<T>(T currentOtherPropertyValue)
            where T : struct, IComparable
        {
            if (OtherPropertyValue is string str)
            {
                if (str.TryParse<T>(out var val))
                {
                    return CompareToAttribute.CompareValues(currentOtherPropertyValue, val, ComparisonType);
                }
                else
                {
                    throw new NotSupportedException(
                        $"The value '{OtherPropertyValue}' cannot be parsed to {typeof(T)}");
                }
            }
            else
            {
                throw new NotSupportedException(
                    $"The value '{OtherPropertyValue}' must be a string if it compares to {typeof(T)}");
            }
        }

        public override bool Equals(object obj)
        {
            return _instance.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _instance.GetHashCode();
        }
    }
}