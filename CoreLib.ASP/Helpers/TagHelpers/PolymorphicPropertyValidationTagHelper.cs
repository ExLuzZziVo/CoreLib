#region

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

#endregion

namespace CoreLib.ASP.Helpers.TagHelpers
{
    /// <summary>
    /// A tag helper that adds unobtrusive validation attributes to derived model properties
    /// </summary>
    [HtmlTargetElement(Attributes = ForPolymorphicAttributeName)]
    public class PolymorphicPropertyValidationTagHelper: TagHelper
    {
        private const string ForPolymorphicAttributeName = "coreLib-polymorphic-for";

        private readonly IModelMetadataProvider _metadataProvider;
        private readonly IClientModelValidatorProvider _clientModelValidatorProvider;
        private readonly ClientValidatorCache _clientValidatorCache;

        public PolymorphicPropertyValidationTagHelper(IModelMetadataProvider metadataProvider,
            IOptions<MvcViewOptions> optionsAccessor,
            ClientValidatorCache clientValidatorCache)
        {
            _metadataProvider = metadataProvider;
            _clientValidatorCache = clientValidatorCache;

            var clientValidatorProviders = optionsAccessor.Value.ClientModelValidatorProviders;

            _clientModelValidatorProvider = new CompositeClientModelValidatorProvider(clientValidatorProviders);
        }

        [HtmlAttributeName(ForPolymorphicAttributeName)]
        public ModelExpression For
        {
            get;
            set;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext
        {
            get;
            set;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var viewModelBaseProperty =
                For?.ModelExplorer?.Container?.Properties?.SingleOrDefault(p => p.Metadata.Name == For.Name);

            if (viewModelBaseProperty != null)
            {
                var clientValidators = _clientValidatorCache.GetValidators(
                    viewModelBaseProperty.Metadata,
                    _clientModelValidatorProvider);

                if (clientValidators.Count > 0)
                {
                    var validationContext = new ClientModelValidationContext(
                        ViewContext,
                        viewModelBaseProperty.Metadata,
                        _metadataProvider,
                        output.Attributes.ToDictionary(k => k.Name, v => v.Value?.ToString(),
                            StringComparer.OrdinalIgnoreCase));

                    foreach (var validator in clientValidators)
                    {
                        validator.AddValidation(validationContext);
                    }

                    foreach (var atr in validationContext.Attributes)
                    {
                        if (output.Attributes.TryGetAttribute(atr.Key, out var attribute))
                        {
                            if (attribute.Value?.ToString() != atr.Value)
                            {
                                output.Attributes.SetAttribute(atr.Key, atr.Value);
                            }
                        }
                        else
                        {
                            output.Attributes.SetAttribute(atr.Key, atr.Value);
                        }
                    }
                }
            }
        }
    }
}
