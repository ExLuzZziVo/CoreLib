#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using CoreLib.ASP.Resources;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Http;

#endregion

namespace CoreLib.ASP.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// This validation attribute is used to validate the MIME type of the <see cref="IFormFile"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FileTypeAttribute : ValidationAttribute
    {
        /// <summary>
        /// This validation attribute is used to validate the MIME type of the <see cref="IFormFile"/>
        /// </summary>
        /// <param name="allowedFileTypes">The allowed MIME types</param>
        public FileTypeAttribute(params string[] allowedFileTypes) :
            base(ValidationStrings.ResourceManager.GetString("FileFormatError"))
        {
            ArgumentNullException.ThrowIfNull(allowedFileTypes);

            if (allowedFileTypes.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(allowedFileTypes));
            }

            AllowedFileTypes = allowedFileTypes;
        }

        /// <summary>
        /// The allowed MIME types
        /// </summary>
        public string[] AllowedFileTypes { get; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);

            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is IFormFile file)
            {
                if (file.Length == 0 || file.ContentType.IsNullOrEmptyOrWhiteSpace())
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                return AllowedFileTypes.Any(ft =>
                    ft.Contains(file.ContentType, StringComparison.InvariantCultureIgnoreCase))
                    ? ValidationResult.Success
                    : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            else
            {
                throw new NotSupportedException(
                    "This validation attribute doesn't support the specified property type");
            }
        }
    }
}