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
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfValidationAttribute : RequiredAttribute
    {
        /// <summary>
        /// This validation attribute is used to validate required based on other property value
        /// </summary>
        /// <param name="otherPropertyName">The name of the other property</param>
        /// <param name="otherPropertyValue">The value of the other property</param>
        /// <param name="invert">If true, the validation will run if <paramref name="otherPropertyValue"/> NOT equals the target value</param>
        public RequiredIfValidationAttribute(string otherPropertyName, object otherPropertyValue, bool invert = false)
        {
            OtherPropertyName = otherPropertyName;
            OtherPropertyValue = otherPropertyValue;
            Invert = invert;
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
        /// A flag indicating that the validation will run if <see name="otherPropertyValue"/> NOT equals the target value
        /// </summary>
        public bool Invert { get; }

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