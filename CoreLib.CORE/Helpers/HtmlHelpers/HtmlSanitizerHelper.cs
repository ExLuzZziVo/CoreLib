#region

using System.Collections.Generic;
using AngleSharp.Parser.Html;
using CoreLib.CORE.Helpers.StringHelpers;
using Ganss.XSS;

#endregion

namespace CoreLib.CORE.Helpers.HtmlHelpers
{
    public static class HtmlSanitizerHelper
    {
        private static readonly HtmlParser _htmlParser = new HtmlParser();
        private static HtmlSanitizer _simpleTextSanitizerInstance;
        private static HtmlSanitizer _extendedTextSanitizerInstance;

        public static HtmlSanitizer SimpleTextSanitizerInstance =>
            _simpleTextSanitizerInstance ?? (_simpleTextSanitizerInstance = new HtmlSanitizer(
                new List<string>
                {
                    "b", "ul", "li", "br", "i", "u", "div", "p", "span"
                }, allowedAttributes: new List<string>
                {
                    "style"
                }, allowedCssProperties: new List<string>
                {
                    "font-weight",
                    "text-decoration-line",
                    "text-align"
                }));

        public static HtmlSanitizer ExtendedTextSanitizerInstance =>
            _extendedTextSanitizerInstance ?? (_extendedTextSanitizerInstance = new HtmlSanitizer(
                new List<string>
                {
                    "b", "ul", "li", "br", "i", "u", "div", "p", "span", "a", "img"
                }, allowedAttributes: new List<string>
                {
                    "style",
                    "href",
                    "target",
                    "src"
                }, allowedCssProperties: new List<string>
                {
                    "font-weight",
                    "text-decoration-line",
                    "text-align",
                    "color",
                    "background-color",
                    "width",
                    "float"
                },
                allowedSchemes: new List<string>
                {
                    "mailto",
                    "tel",
                    "http",
                    "https"
                }
            ));

        public static string UpdateImagesClass(this string htmlIn)
        {
            if (htmlIn == null)
                return null;
            var dom = _htmlParser.Parse("<html><body></body></html>");
            dom.Body.InnerHtml = htmlIn;
            foreach (var i in dom.Images)
                if (i.ClassName.IsNullOrEmptyOrWhiteSpace() || !i.ClassName.Contains("img-fluid"))
                    i.ClassName = i.ClassName + " img-fluid";
            return dom.Body.InnerHtml;
        }

        public static string TotalCleanHtml(this string htmlIn)
        {
            return htmlIn == null ? null : _htmlParser.Parse(htmlIn).DocumentElement.TextContent;
        }

        public static string RemoveHtmlXssFromSimpleText(this string htmlIn, string baseUrl = null)
        {
            return htmlIn == null ? null : SimpleTextSanitizerInstance.Sanitize(htmlIn, baseUrl);
        }

        public static string RemoveHtmlXssFromExtendedText(this string htmlIn, string baseUrl = null)
        {
            return htmlIn == null ? null : ExtendedTextSanitizerInstance.Sanitize(htmlIn, baseUrl);
        }
    }
}