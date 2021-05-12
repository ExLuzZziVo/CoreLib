#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace CoreLib.CORE.Types
{
    /// <summary>
    /// The exception that is thrown on validation errors for all data fields of an object
    /// </summary>
    public class ExtendedValidationException : Exception
    {
        private readonly List<string> _validationErrors = new List<string>();

        /// <summary>
        /// The exception that is thrown on validation errors for all data fields of an object
        /// </summary>
        /// <param name="errors">List of errors as a collection of <see cref="ValidationResult"/></param>
        public ExtendedValidationException(IEnumerable<ValidationResult> errors)
        {
            foreach (var ve in errors)
            {
                _validationErrors.Add(ve.ErrorMessage);
            }
        }

        /// <summary>
        /// The exception that is thrown on validation errors for all data fields of an object
        /// </summary>
        /// <param name="errors">List of errors as a collection of strings</param>
        public ExtendedValidationException(IEnumerable<string> errors)
        {
            _validationErrors.AddRange(errors);
        }

        /// <summary>
        /// The exception that is thrown on validation errors for all data fields of an object
        /// </summary>
        /// <param name="message">Error text</param>
        public ExtendedValidationException(string message)
        {
            _validationErrors.Add(message);
        }

        /// <summary>
        /// List of all validation errors
        /// </summary>
        public IEnumerable<string> ValidationErrors => _validationErrors;
    }
}