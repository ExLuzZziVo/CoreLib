#region

using System;
using System.ComponentModel.DataAnnotations;
using CoreLib.ASP.Helpers.CheckHelpers;
using Microsoft.Extensions.Configuration;

#endregion

namespace CoreLib.ASP.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// Google ReCaptchaV2 validation attribute
    /// </summary>
    public class GoogleReCaptchaV2ValidationAttribute : ValidationAttribute
    {
        private readonly bool _invisible;

        /// <summary>
        /// Google ReCaptchaV2 validation attribute
        /// </summary>
        /// <param name="invisible">The flag indicating the use of invisible ReCaptchaV2. Default value: false</param>
        public GoogleReCaptchaV2ValidationAttribute(bool invisible = false)
        {
            _invisible = invisible;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorResult = new Lazy<ValidationResult>(() =>
                new ValidationResult(Resources.ValidationStrings.ResourceManager.GetString("ReCaptchaValidationError"),
                    new[] {validationContext.MemberName}));

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return errorResult.Value;
            }

            var configuration = (IConfiguration) validationContext.GetService(typeof(IConfiguration));
            var reCaptchaResponse = value.ToString();

            var reCaptchaSecret =
                configuration.GetValue<string>(
                    $"GoogleReCaptchaV2{(_invisible ? "Invisible" : string.Empty)}:SecretKey");

            return !CheckGoogleReCaptchaHelper.CheckV2Async(reCaptchaResponse, reCaptchaSecret).Result
                ? errorResult.Value
                : ValidationResult.Success;
        }
    }
}