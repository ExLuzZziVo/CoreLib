#region

using System;
using System.ComponentModel.DataAnnotations;
using CoreLib.CORE.Helpers.HtmlHelpers;
using CoreLib.CORE.Helpers.StringHelpers;
using CoreLib.CORE.Resources;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    public class RequiredHtmlTextValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorResult = new Lazy<ValidationResult>(() =>
                new ValidationResult(
                    string.Format(CommonStrings.ResourceManager.GetString("RequiredError"),
                        validationContext.DisplayName),
                    new[] {validationContext.MemberName}));
            if (value == null || value.ToString().TotalCleanHtml().TrimWholeString().IsNullOrEmptyOrWhiteSpace())
                return errorResult.Value;
            return ValidationResult.Success;
        }
    }
}