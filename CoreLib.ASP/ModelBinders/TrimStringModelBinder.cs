#region

using System;
using System.Threading.Tasks;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace CoreLib.ASP.ModelBinders
{
    /// <summary>
    /// <see cref="IModelBinderProvider"/>, which trims all model binding strings/>
    /// </summary>
    public class TrimStringModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string))
            {
                return new TrimStringModelBinder(new SimpleTypeModelBinder(context.Metadata.ModelType,
                    context.Services.GetService<ILoggerFactory>()));
            }

            return null;
        }
    }

    /// <summary>
    /// An <see cref="IModelBinder"/> implementation for the <see cref="TrimStringModelBinderProvider"/>
    /// </summary>
    public class TrimStringModelBinder
        : IModelBinder
    {
        private readonly IModelBinder _modelBinder;

        public TrimStringModelBinder(IModelBinder modelBinder)
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

            if (valueProviderResult != ValueProviderResult.None &&
                valueProviderResult.FirstValue is string str &&
                !string.IsNullOrEmpty(str))
            {
                bindingContext.Result = ModelBindingResult.Success(str.TrimWholeString());

                return Task.CompletedTask;
            }

            return _modelBinder.BindModelAsync(bindingContext);
        }
    }
}