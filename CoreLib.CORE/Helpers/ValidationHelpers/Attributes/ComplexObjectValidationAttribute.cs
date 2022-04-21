#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to validate all entire properties of target property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ComplexObjectValidationAttribute : ValidationAttribute
    {
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

            var validationResults = new List<ValidationResult>();
            var objectValidationContext = new ValidationContext(value);
            Validator.TryValidateObject(value, objectValidationContext, validationResults, true);

            return validationResults.Any()
                ? new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName) +
                                       validationResults.Aggregate(string.Empty,
                                           (current, c) => current + "\n\t" + c.ErrorMessage))
                : ValidationResult.Success;
        }
    }
}