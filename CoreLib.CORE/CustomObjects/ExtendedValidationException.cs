#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace CoreLib.CORE.CustomObjects
{
    public class ExtendedValidationException : Exception
    {
        private readonly List<string> _validationErrors = new List<string>();

        public ExtendedValidationException(IEnumerable<ValidationResult> errors)
        {
            foreach (var ve in errors) _validationErrors.Add(ve.ErrorMessage);
        }

        public ExtendedValidationException(IEnumerable<string> errors)
        {
            _validationErrors.AddRange(errors);
        }

        public ExtendedValidationException(string message)
        {
            _validationErrors.Add(message);
        }

        public IEnumerable<string> ValidationErrors => _validationErrors;
    }
}