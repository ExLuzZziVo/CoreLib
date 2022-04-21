#region

using System;
using System.ComponentModel.DataAnnotations;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to validate required based on other property value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class RequiredIfAttribute : RequiredAttribute
    {
        /// <summary>
        /// This validation attribute is used to validate required based on other property value
        /// </summary>
        /// <param name="otherPropertyName">The name of the other property</param>
        /// <param name="otherPropertyValue">The value of the other property</param>
        /// <param name="comparisonType">The comparison type with <paramref name="otherPropertyValue"/>. Default value: <see cref="ComparisonType.Equal"/></param>
        public RequiredIfAttribute(string otherPropertyName, object otherPropertyValue, ComparisonType comparisonType = ComparisonType.Equal)
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
        /// The comparison type with <see name="OtherPropertyValue"/>
        /// </summary>
        public ComparisonType ComparisonType { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (OtherPropertyName.IsNullOrEmptyOrWhiteSpace())
            {
                return new ValidationResult("Other property name is empty");
            }

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherPropertyName);

            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {OtherPropertyName}");
            }

            var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var valuesEqual = Equals(OtherPropertyValue, otherValue);

            if ((valuesEqual && !Invert) || (!valuesEqual && Invert))
            {
                return base.IsValid(value, validationContext);
            }

            return null;
        }
    }
}