#region

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace CoreLib.ASP.ModelBinders
{
    /// <summary>
    /// <see cref="IModelBinderProvider"/>, which allows model binding of <see cref="decimal"/> with dot or comma
    /// </summary>
    public class InvariantDecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Metadata.IsComplexType && (context.Metadata.ModelType == typeof(decimal) ||
                                                    context.Metadata.ModelType == typeof(decimal?)))
            {
                return new InvariantDecimalModelBinder(new SimpleTypeModelBinder(context.Metadata.ModelType,
                    context.Services.GetService<ILoggerFactory>()));
            }

            return null;
        }
    }

    /// <summary>
    /// An <see cref="IModelBinder"/> implementation for the <see cref="InvariantDecimalModelBinderProvider"/>
    /// </summary>
    public class InvariantDecimalModelBinder : IModelBinder
    {
        private readonly IModelBinder _modelBinder;

        public InvariantDecimalModelBinder(IModelBinder modelBinder)
        {
            _modelBinder = modelBinder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                var valueAsString = valueProviderResult.FirstValue;

                if (decimal.TryParse(valueAsString, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out var result))
                {
                    bindingContext.Result = ModelBindingResult.Success(result);

                    return Task.CompletedTask;
                }
            }

            return _modelBinder.BindModelAsync(bindingContext);
        }
    }
}