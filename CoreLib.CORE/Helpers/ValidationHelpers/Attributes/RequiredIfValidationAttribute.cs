#region

using System;
using System.ComponentModel.DataAnnotations;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfValidationAttribute : RequiredAttribute
    {
        public RequiredIfValidationAttribute(string otherPropertyName, object otherPropertyValue)
        {
            OtherPropertyName = otherPropertyName;
            OtherPropertyValue = otherPropertyValue;
        }

        public string OtherPropertyName { get; }

        public object OtherPropertyValue { get; }

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

            if (Equals(OtherPropertyValue, otherValue))
            {
                return base.IsValid(value, validationContext);
            }

            return null;
        }
    }
}