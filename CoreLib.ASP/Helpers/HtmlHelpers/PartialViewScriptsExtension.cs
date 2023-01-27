#region

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion

namespace CoreLib.ASP.Helpers.HtmlHelpers
{
    // https://stackoverflow.com/a/56918543
    public static partial class HtmlHelperExtensions
    {
        private const string PartialViewScriptsKey = nameof(PartialViewScriptsKey);

        /// <summary>
        /// Helper that allows partial views to have their own scripts section. This method must be used in partial views in conjunction with <see cref="RenderPartialViewScriptsSection"/>
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <returns><see cref="PartialViewScriptsBlock"/></returns>
        public static IDisposable PartialViewScriptsSection(this IHtmlHelper html)
        {
            return new PartialViewScriptsBlock(html.ViewContext);
        }

        /// <summary>
        /// Helper that allows partial views to have their own scripts section. This method must be placed at the end of _Layout.cshtml and used in conjunction with <see cref="PartialViewScriptsSection"/>
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <returns><see cref="HtmlString"/> with <see cref="PartialViewScriptsSection"/> content</returns>
        public static HtmlString RenderPartialViewScriptsSection(this IHtmlHelper html)
        {
            var resultHtml = string.Join(Environment.NewLine, GetPartialViewScriptsList(html.ViewContext.HttpContext));

            html.ViewContext.HttpContext.Items.Remove(PartialViewScriptsKey);

            return new HtmlString(resultHtml);
        }

        // Partial view scripts strings are stored in the HttpContext
        private static ICollection<string> GetPartialViewScriptsList(HttpContext httpContext)
        {
            var containsValue = httpContext.Items.TryGetValue(PartialViewScriptsKey, out var value);

            if (!containsValue || value is not ICollection<string> partialViewScripts)
            {
                partialViewScripts = new List<string>();
                httpContext.Items.Add(PartialViewScriptsKey, partialViewScripts);
            }

            return partialViewScripts;
        }

        private class PartialViewScriptsBlock : IDisposable
        {
            private readonly TextWriter _originalWriter;
            private readonly StringWriter _scriptsWriter = new();
            private readonly ViewContext _viewContext;

            public PartialViewScriptsBlock(ViewContext viewContext)
            {
                _viewContext = viewContext;
                _originalWriter = _viewContext.Writer;
                _viewContext.Writer = _scriptsWriter;
            }

            public void Dispose()
            {
                _viewContext.Writer = _originalWriter;
                var pageScripts = GetPartialViewScriptsList(_viewContext.HttpContext);
                pageScripts.Add(_scriptsWriter.ToString());
                _scriptsWriter.Dispose();
            }
        }
    }
}