#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CoreLib.CORE.Helpers.ObjectHelpers;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.STANDALONE.Types
{
    /// <summary>
    /// Base class of view models that implements <see cref="INotifyPropertyChanged"/>,<see cref="IDataErrorInfo"/> and <see cref="IValidatableObject"/> interfaces
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo, IValidatableObject
    {
        #region Implement IValidatableObject

        /// <returns><see cref="ValidationResult.Success"/></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }

        #endregion

        /// <summary>
        /// Gets the value of a property by its name
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="propertyName">Name of the property. Uses <see cref="CallerMemberNameAttribute"/> by default</param>
        /// <returns>Property value. If no property value was found, the default is returned</returns>
        protected T GetValue<T>([CallerMemberName] string propertyName = "")
        {
            if (propertyName.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            if (!_values.TryGetValue(propertyName, out var value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

        /// <summary>
        /// Sets the value of a property by its name
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="value">Value of the property</param>
        /// <param name="propertyName">Name of the property. Uses <see cref="CallerMemberNameAttribute"/> by default</param>
        /// <returns>True if the property value was changed</returns>
        protected bool SetValue<T>(T value, [CallerMemberName] string propertyName = "")
        {
            if (propertyName.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            if (_values.TryGetValue(propertyName, out var val))
            {
                if (Equals(val, value))
                {
                    return false;
                }
            }

            _values[propertyName] = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        #region Implement INotifyPropertyChanged

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Static

        protected static void OnStaticPropertyChanged(PropertyChangedEventHandler staticPropertyChangedEvent,
            [CallerMemberName] string propertyName = null)
        {
            staticPropertyChangedEvent?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #endregion

        #region Implement IDataErrorInfo

        /// <summary>
        /// Dictionary for storing backing fields
        /// </summary>
        [field: NonSerialized] [JsonIgnore] private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        [JsonIgnore]
        string IDataErrorInfo.Error =>
            throw new NotSupportedException(
                "IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");

        [JsonIgnore] string IDataErrorInfo.this[string propertyName] => OnValidate(propertyName);

        /// <summary>
        /// Validates a property by its name
        /// </summary>
        /// <param name="propertyName">Name of the property to validate</param>
        /// <returns>Validation error. If there is no validation error, returns <see cref="string.Empty"/></returns>
        protected virtual string OnValidate(string propertyName)
        {
            if (propertyName.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            var error = string.Empty;
            var value = GetValue(propertyName);

            var results = new List<ValidationResult>(1);

            var result = Validator.TryValidateProperty(
                value, new ValidationContext(this)
                {
                    MemberName = propertyName
                }, results);

            if (!result)
            {
                var validationResult = results[0];
                error = validationResult.ErrorMessage;
            }

            return error;
        }

        /// <summary>
        /// Gets the value of a property by its name
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Property value</returns>
        private object GetValue(string propertyName)
        {
            if (!_values.TryGetValue(propertyName, out var value))
            {
                value = this.GetPropertyValueByName(propertyName);
                _values.Add(propertyName, value);
            }

            return value;
        }

        /// <summary>
        /// Validates this view model
        /// </summary>
        /// <returns>The sequence of view model errors</returns>
        protected virtual IEnumerable<string> Validate()
        {
            var results = new List<ValidationResult>(40);
            Validator.TryValidateObject(this, new ValidationContext(this), results, true);
            var result = results.Select(r => r.ErrorMessage).ToArray();
            ValidationErrors = result;

            return result;
        }

        /// <summary>
        /// Returns true if this view model is valid
        /// </summary>
        [JsonIgnore]
        public bool IsValid => !Validate().Any();

        /// <summary>
        /// Validation errors of this view model
        /// </summary>
        [JsonIgnore]
        public IEnumerable<string> ValidationErrors
        {
            get => GetValue<IEnumerable<string>>();
            private set => SetValue(value);
        }

        #endregion
    }
}
