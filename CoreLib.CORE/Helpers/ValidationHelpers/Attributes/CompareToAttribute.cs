#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using CoreLib.CORE.Helpers.ObjectHelpers;
using CoreLib.CORE.Helpers.StringHelpers;
using CoreLib.CORE.Resources;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to validate target property value by comparing it with other property value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CompareToAttribute : ValidationAttribute
    {
        private readonly object _instance = new object();

        /// <summary>
        /// This validation attribute is used to validate target property value by comparing it with other property value
        /// </summary>
        /// <param name="compareToPropertyName">The name of the property to compare with</param>
        /// <param name="comparisonType">Comparison type</param>
        /// <remarks>
        /// Comparable properties must be numeric OR of the same type that implements <see cref="IComparable"/> interface
        /// </remarks>
        public CompareToAttribute(string compareToPropertyName, ComparisonType comparisonType) : base(
            GetDefaultErrorMessage(comparisonType))
        {
            if (compareToPropertyName.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(compareToPropertyName));
            }

            CompareToPropertyName = compareToPropertyName;
            ComparisonType = comparisonType;
        }

        /// <summary>
        /// The name of the property to compare with
        /// </summary>
        public string CompareToPropertyName { get; }

        /// <summary>
        /// Comparison type
        /// </summary>
        public ComparisonType ComparisonType { get; }

        public override object TypeId => _instance;

        /// <summary>
        /// Generates the default error message using <paramref name="comparisonType"/>
        /// </summary>
        /// <param name="comparisonType">Comparison type</param>
        private static string GetDefaultErrorMessage(ComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case ComparisonType.Equal:
                    return ValidationStrings.ResourceManager.GetString("CompareToEqualError");
                case ComparisonType.NotEqual:
                    return ValidationStrings.ResourceManager.GetString("CompareToNotEqualError");
                case ComparisonType.Less:
                    return ValidationStrings.ResourceManager.GetString("CompareToSmallerThanError");
                case ComparisonType.LessOrEqual:
                    return ValidationStrings.ResourceManager.GetString("CompareToSmallerThanOrEqualError");
                case ComparisonType.Greater:
                    return ValidationStrings.ResourceManager.GetString("CompareToGreaterThanError");
                case ComparisonType.GreaterOrEqual:
                    return ValidationStrings.ResourceManager.GetString("CompareToGreaterThanOrEqualError");
                default:
                    throw new ArgumentNullException(nameof(comparisonType));
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            var compareToProperty = validationContext.ObjectType.GetProperty(CompareToPropertyName);

            if (compareToProperty == null)
            {
                throw new NullReferenceException($"The object has no property with '{CompareToPropertyName}' name");
            }

            var compareToPropertyType = Nullable.GetUnderlyingType(compareToProperty.PropertyType) ??
                                        compareToProperty.PropertyType;

            var compareToPropertyValue = compareToProperty.GetValue(validationContext.ObjectInstance);

            var targetProperty = validationContext.ObjectType.GetProperty(validationContext.MemberName);

            var targetPropertyType =
                Nullable.GetUnderlyingType(targetProperty.PropertyType) ?? targetProperty.PropertyType;

            var errorMessage = string.Format(CultureInfo.CurrentCulture, ErrorMessageString,
                targetProperty.GetPropertyDisplayName(), compareToProperty.GetPropertyDisplayName());

            if (value == null && compareToPropertyValue == null && ComparisonType == ComparisonType.NotEqual)
            {
                return new ValidationResult(errorMessage);
            }
            else if (value == null || compareToPropertyValue == null)
            {
                return ValidationResult.Success;
            }
            else if (compareToPropertyType.IsNumeric() && targetPropertyType.IsNumeric())
            {
                var validationResult = CompareValues(Convert.ToDecimal(value, CultureInfo.InvariantCulture),
                    Convert.ToDecimal(compareToPropertyValue, CultureInfo.InvariantCulture), ComparisonType);

                return validationResult ? ValidationResult.Success : new ValidationResult(errorMessage);
            }
            else if (compareToPropertyType == targetPropertyType)
            {
                if (!targetPropertyType.GetInterfaces().Contains(typeof(IComparable)))
                {
                    throw new NotSupportedException(
                        "The type of the comparable properties must implement the IComparable interface");
                }

                var validationResult =
                    CompareValues((IComparable)value, compareToPropertyValue, ComparisonType);

                return validationResult ? ValidationResult.Success : new ValidationResult(errorMessage);
            }
            else
            {
                throw new NotSupportedException("The types of suggested objects are not comparable");
            }
        }

        /// <summary>
        /// Compares two values using <paramref name="comparisonType"/>
        /// </summary>
        /// <param name="targetValue">First value</param>
        /// <param name="compareToValue">Second value</param>
        /// <param name="comparisonType">Comparison type</param>
        /// <returns>True if comparison operation is valid for <paramref name="targetValue"/></returns>
        /// <remarks>For internal usage</remarks>
        internal static bool CompareValues(IComparable targetValue, object compareToValue,
            ComparisonType comparisonType)
        {
            var comparisonResult = targetValue.CompareTo(compareToValue);

            switch (comparisonType)
            {
                case ComparisonType.Equal:
                    return comparisonResult == 0;
                case ComparisonType.NotEqual:
                    return comparisonResult != 0;
                case ComparisonType.Less:
                    return comparisonResult < 0;
                case ComparisonType.LessOrEqual:
                    return comparisonResult <= 0;
                case ComparisonType.Greater:
                    return comparisonResult > 0;
                case ComparisonType.GreaterOrEqual:
                    return comparisonResult >= 0;
                default:
                    return true;
            }

            return true;
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