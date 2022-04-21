using System.ComponentModel;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreLib.ASP.Helpers.TagHelpers
{
    /// <summary>
    /// A tag helper that adds a placeholder attribute with <see cref="DisplayNameAttribute"/> content to target input element
    /// </summary>
    [HtmlTargetElement("input", Attributes = AttributeName)]
    public class InputPlaceholderTagHelper : TagHelper
    {
        private const string AttributeName = "coreLib-placeholder-for";

        [HtmlAttributeName(AttributeName)] public ModelExpression Placeholder { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var placeholderValue = Placeholder.Metadata.Placeholder;

            if (placeholderValue.IsNullOrEmptyOrWhiteSpace())
            {
                placeholderValue = Placeholder.Metadata.GetDisplayName();
            }

            if (!output.Attributes.TryGetAttribute("placeholder", out _))
            {
                output.Attributes.Add(new TagHelperAttribute("placeholder", placeholderValue));
            }
        }
    }
}