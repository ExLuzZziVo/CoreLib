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
    /// <see cref="IModelBinderProvider"/>, which allows model binding of <see cref="double"/> with dot or comma
    /// </summary>
    public class InvariantDoubleModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Metadata.IsComplexType && (context.Metadata.ModelType == typeof(double) ||
                                                    context.Metadata.ModelType == typeof(double?)))
            {
                return new InvariantDoubleModelBinder(new SimpleTypeModelBinder(context.Metadata.ModelType,
                    context.Services.GetService<ILoggerFactory>()));
            }

            return null;
        }
    }

    /// <summary>
    /// An <see cref="IModelBinder"/> implementation for the <see cref="InvariantDoubleModelBinderProvider"/>
    /// </summary>
    public class InvariantDoubleModelBinder : IModelBinder
    {
        private readonly IModelBinder _modelBinder;

        public InvariantDoubleModelBinder(IModelBinder modelBinder)
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

                if (double.TryParse(valueAsString, NumberStyles.Any,
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