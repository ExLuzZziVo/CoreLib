using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreLib.ASP.Helpers.TagHelpers
{
    /// <summary>
    /// A tag helper that appends additional text to label's content
    /// </summary>
    [HtmlTargetElement("label", Attributes = AttributeName)]
    public class AppendTextTagHelper
        : TagHelper
    {
        private const string AttributeName = "coreLib-appendText";

        [HtmlAttributeName(AttributeName)] public string TextToAppend { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            output.Content.Append(TextToAppend);
        }

        public override int Order => 100;
    }
}