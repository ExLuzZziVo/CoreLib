#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

#endregion

namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to specify that the inner text of an HTML string must not be empty
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class HTMLTextRequiredAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value == null ? null : Regex.Replace(value.ToString(), @"<.*?>|&nbsp;", string.Empty),
                validationContext);
        }
    }
}