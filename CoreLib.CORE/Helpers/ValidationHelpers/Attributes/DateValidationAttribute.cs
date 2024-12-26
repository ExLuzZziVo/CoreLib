#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CoreLib.CORE.Helpers.ObjectHelpers;
using CoreLib.CORE.Resources;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to validate target date or age
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = true)]
    public class DateValidationAttribute : ValidationAttribute
    {
        private readonly object _instance = new object();

        /// <summary>
        /// This constructor is used to validate AGE
        /// </summary>
        /// <param name="comparisonType">Comparison type</param>
        /// <param name="years">Years count</param>
        /// <param name="months">Months count. Default value: 0</param>
        /// <param name="days">Days count. Default value: 0</param>
        public DateValidationAttribute(ComparisonType comparisonType, int years, int months = 0, int days = 0) :
            base(
                GetDefaultErrorMessage(comparisonType, true))
        {
            if (years < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(years));
            }

            if (months < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(months));
            }

            if (days < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(days));
            }

            DateToCompare = DateTime.Today.AddYears(-years).AddMonths(-months).AddDays(-days);

            if (comparisonType == ComparisonType.Equal || comparisonType == ComparisonType.NotEqual)
            {
                ComparisonType = comparisonType;
            }
            else
            {
                ComparisonType = (ComparisonType)(-(sbyte)comparisonType);
            }
        }

        /// <summary>
        /// This constructor is used to validate DATE
        /// </summary>
        /// <param name="year">Year. Must be greater than 1</param>
        /// <param name="month">Month. Must be between 1 and 12</param>
        /// <param name="day">Day. Must be between 1 and 31</param>
        /// <param name="comparisonType">Comparison type</param>
        public DateValidationAttribute(int year, int month, int day, ComparisonType comparisonType) : base(
            GetDefaultErrorMessage(comparisonType))
        {
            if (year < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }

            if (!month.IsInRange(1, 12))
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }

            if (!day.IsInRange(1, 31))
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            DateToCompare = new DateTime(year, month, day);
            ComparisonType = comparisonType;
        }

        /// <summary>
        /// This constructor is used to validate DATE
        /// </summary>
        /// <param name="dateString">Date to compare</param>
        /// <param name="comparisonType">Comparison type</param>
        public DateValidationAttribute(string dateString, ComparisonType comparisonType) :
            base(GetDefaultErrorMessage(comparisonType))
        {
            if (DateTime.TryParse(dateString, out var dateToCompare))
            {
                DateToCompare = dateToCompare;
            }
            else
            {
                throw new ArgumentException($"The '{nameof(dateString)}' has wrong format", nameof(dateString));
            }

            ComparisonType = comparisonType;
        }

        /// <summary>
        /// This constructor is used to compare DATE with <see cref="DateTime.Today"/> or DATETIME with <see cref="DateTime.Now"/>
        /// </summary>
        /// <param name="comparisonType">Comparison type</param>
        /// <param name="isTimePartIncluded">Include time part to compare DATETIME. Only works when comparing <see cref="DateTime"/>. Default value: false</param>
        public DateValidationAttribute(ComparisonType comparisonType, bool isTimePartIncluded = false) : base(GetDefaultErrorMessage(comparisonType))
        {
            ComparisonType = comparisonType;
            IsDateTimeTodayToCompare = true;
            IsTimePartIncluded = isTimePartIncluded;
        }

        /// <summary>
        /// Date to compare
        /// </summary>
        public DateTime DateToCompare { get; }

        /// <summary>
        /// Flag indicating that <see cref="DateTime.Today"/> will be used instead of <see cref="DateToCompare"/>
        /// </summary>
        public bool IsDateTimeTodayToCompare { get; } = false;
        
        /// <summary>
        /// Flag indicating that <see cref="DateTime.Now"/> will be used instead of <see cref="DateTime.Today"/> if <see cref="IsDateTimeTodayToCompare"/> is set to true
        /// </summary>
        /// <remarks>
        /// Only works when comparing <see cref="DateTime"/>
        /// </remarks>
        public bool IsTimePartIncluded { get; } = false;

        /// <summary>
        /// Comparison type
        /// </summary>
        public ComparisonType ComparisonType { get; }

        /// <summary>
        /// The date format to be used in the error message to format the date for comparison
        /// </summary>
        public string ErrorMessageDateToCompareFormat { get; set; } = "dd-MM-yyyy";

        public override object TypeId => _instance;

        /// <summary>
        /// Generates the default error message using <paramref name="comparisonType"/> and <paramref name="isAge"/>
        /// </summary>
        /// <param name="comparisonType">Comparison type</param>
        /// <param name="isAge">Generate default error message for age or date validation</param>
        private static string GetDefaultErrorMessage(ComparisonType comparisonType, bool isAge = false)
        {
            switch (comparisonType)
            {
                case ComparisonType.Equal:
                    return ValidationStrings.ResourceManager.GetString("ValueEqualError");
                case ComparisonType.NotEqual:
                    return ValidationStrings.ResourceManager.GetString("ValueNotEqualError");
                case ComparisonType.Less:
                    return isAge
                        ? ValidationStrings.ResourceManager.GetString("ValueGreaterThanError")
                        : ValidationStrings.ResourceManager.GetString("ValueSmallerThanError");
                case ComparisonType.LessOrEqual:
                    return isAge
                        ? ValidationStrings.ResourceManager.GetString("ValueGreaterThanOrEqualError")
                        : ValidationStrings.ResourceManager.GetString("ValueSmallerThanOrEqualError");
                case ComparisonType.Greater:
                    return isAge
                        ? ValidationStrings.ResourceManager.GetString("ValueSmallerThanError")
                        : ValidationStrings.ResourceManager.GetString("ValueGreaterThanError");
                case ComparisonType.GreaterOrEqual:
                    return isAge
                        ? ValidationStrings.ResourceManager.GetString("ValueSmallerThanOrEqualError")
                        : ValidationStrings.ResourceManager.GetString("ValueGreaterThanOrEqualError");
                default:
                    throw new ArgumentNullException(nameof(comparisonType));
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name,
                (IsDateTimeTodayToCompare ? (IsTimePartIncluded ? DateTime.Now : DateTime.Today) : DateToCompare).ToString(ErrorMessageDateToCompareFormat,
                    CultureInfo.CurrentCulture));
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
            else if (value is DateTime dateTime)
            {
                return CompareToAttribute.CompareValues(dateTime,
                    IsDateTimeTodayToCompare ? (IsTimePartIncluded ? DateTime.Now : DateTime.Today) : DateToCompare, ComparisonType)
                    ? ValidationResult.Success
                    : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                return CompareToAttribute.CompareValues(dateTimeOffset,
                    IsDateTimeTodayToCompare ? (IsTimePartIncluded ? new DateTimeOffset(DateTime.Now) : new DateTimeOffset(DateTime.Today)) : new DateTimeOffset(DateToCompare), ComparisonType)
                    ? ValidationResult.Success
                    : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
#if NET6_0_OR_GREATER
            else if (value is DateOnly dateOnly)
            {
                return CompareToAttribute.CompareValues(dateOnly,
                    DateOnly.FromDateTime(IsDateTimeTodayToCompare ? DateTime.Today : DateToCompare), ComparisonType)
                    ? ValidationResult.Success
                    : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
#endif
            else
            {
                throw new NotSupportedException(
                    "This attribute is only valid on types 'System.DateTime', 'System.DateTimeOffset' or 'System.DateOnly'");
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