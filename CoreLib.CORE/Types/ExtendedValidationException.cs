#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#endregion

namespace CoreLib.CORE.Types
{
    /// <summary>
    /// The exception that is thrown on validation errors for all data fields of an object
    /// </summary>
    public class ExtendedValidationException : Exception
    {
        private readonly string[] _validationErrors;
        private readonly ValidationResult[] _validationResults;

        /// <summary>
        /// The exception that is thrown on validation errors for all data fields of an object
        /// </summary>
        /// <param name="errors">List of errors as a collection of <see cref="ValidationResult"/></param>
        public ExtendedValidationException(IEnumerable<ValidationResult> errors)
        {
            _validationResults = errors.ToArray();
            _validationErrors = errors.Select(ve => ve.ErrorMessage).ToArray();
        }

        /// <summary>
        /// The exception that is thrown on validation errors for all data fields of an object
        /// </summary>
        /// <param name="errors">List of errors as a collection of strings</param>
        public ExtendedValidationException(IEnumerable<string> errors)
        {
            _validationResults = errors.Select(err => new ValidationResult(err)).ToArray();
            _validationErrors = errors.ToArray();
        }

        /// <summary>
        /// The exception that is thrown on validation errors for all data fields of an object
        /// </summary>
        /// <param name="message">Error text</param>
        public ExtendedValidationException(string message)
        {
            _validationResults = new[] { new ValidationResult(message) };
            _validationErrors = new[] { message };
        }

        /// <summary>
        /// List of all validation errors
        /// </summary>
        public IEnumerable<string> ValidationErrors => _validationErrors;

        /// <summary>
        /// List of all validation results
        /// </summary>
        public IEnumerable<ValidationResult> ValidationResults => _validationResults;

        public override string Message => string.Join("\n", _validationErrors);
    }
}