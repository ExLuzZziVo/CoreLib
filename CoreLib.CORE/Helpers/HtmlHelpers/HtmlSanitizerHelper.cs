#region

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                    "b", "ul", "li", "br", "i", "u", "div", "p", "span", "details", "summary"
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
                    "b", "ul", "li", "br", "i", "u", "div", "p", "span", "details", "summary", "a", "img", "iframe"
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
            {
                if (i.ClassName.IsNullOrEmptyOrWhiteSpace() || !i.ClassName.Contains("img-fluid"))
                    i.ClassName += " img-fluid";
            }

            foreach (var i in dom.All.Where(el=>el.TagName=="iframe"))
            {
                if(i.ClassName.IsNullOrEmptyOrWhiteSpace() || !i.ClassName.Contains("embed-responsive-item"))
                    i.ClassName += " embed-responsive-item";
            }
            return dom.Body.InnerHtml;
        }

        public static IEnumerable<string> GetAllImagesSrc(this string htmlIn)
        {
            if (htmlIn == null)
                return null;
            var dom = _htmlParser.Parse("<html><body></body></html>");
            dom.Body.InnerHtml = htmlIn;
            return dom.Images.Select(i => i.Source.Replace("about://", "")).ToList();
        }

        public static string TotalCleanHtml(this string htmlIn)
        {
            return htmlIn == null ? null : _htmlParser.Parse(htmlIn).DocumentElement.TextContent;
            //Regex.Replace(htmlIn, "<.*?>|&.*?;", string.Empty);
            //
            //string.Concat(_htmlParser.ParseFragment(htmlIn,null).Select(n => n.Text()));
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