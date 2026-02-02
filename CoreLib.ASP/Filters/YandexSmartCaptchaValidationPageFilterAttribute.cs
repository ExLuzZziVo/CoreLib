#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoreLib.ASP.Helpers.CheckHelpers;
using Microsoft.AspNetCore.Mvc.Filters;

#endregion

namespace CoreLib.ASP.Filters
{
    /// <summary>
    /// Yandex SmartCaptcha validation page filter attribute
    /// </summary>
    /// <remarks>
    /// The form sent to the server must contain the key 'smart-token'. Also, the application configuration file must contain the section 'YandexSmartCaptcha' with a secret key
    /// </remarks>
    public class YandexSmartCaptchaValidationPageFilterAttribute : Attribute, IAsyncPageFilter
    {
        private readonly bool _appendUserIp;

        /// <summary>
        /// Yandex SmartCaptcha validation page filter attribute
        /// </summary>
        /// <param name="appendUserIp">The flag indicating the use of the user's IP address. Default value: false</param>
        public YandexSmartCaptchaValidationPageFilterAttribute(bool appendUserIp = false)
        {
            _appendUserIp = appendUserIp;
        }

        /// <summary>
        /// Names of page handlers to validate. If empty, only POST handlers are validated
        /// </summary>
        public string[] HandlerNames { get; set; } = [];

        public Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            return next();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            if (HandlerNames.Length == 0)
            {
                return string.Equals(context.HandlerMethod?.HttpMethod, HttpMethod.Post.Method,
                    StringComparison.OrdinalIgnoreCase)
                    ? CheckYandexSmartCaptchaHelper.CheckYandexSmartCaptchaAsync(context, _appendUserIp)
                    : Task.CompletedTask;
            }

            return HandlerNames.Contains(context.HandlerMethod.Name)
                ? CheckYandexSmartCaptchaHelper.CheckYandexSmartCaptchaAsync(context, _appendUserIp)
                : Task.CompletedTask;
        }
    }
}
