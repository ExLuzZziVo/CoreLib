#region

using CoreLib.ASP.Helpers.CheckHelpers;
using Microsoft.AspNetCore.Mvc.Filters;

#endregion

namespace CoreLib.ASP.Filters
{
    /// <summary>
    /// Yandex SmartCaptcha validation action filter attribute
    /// </summary>
    /// <remarks>
    /// The form sent to the server must contain the key 'smart-token'. Also, the application configuration file must contain the section 'YandexSmartCaptcha' with a secret key
    /// </remarks>
    public class YandexSmartCaptchaValidationActionFilterAttribute : ActionFilterAttribute
    {
        private readonly bool _appendUserIp;

        /// <summary>
        /// Yandex SmartCaptcha validation action filter attribute
        /// </summary>
        /// <param name="appendUserIp">The flag indicating the use of the user's IP address. Default value: false</param>
        public YandexSmartCaptchaValidationActionFilterAttribute(bool appendUserIp = false)
        {
            _appendUserIp = appendUserIp;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CheckYandexSmartCaptchaHelper.CheckYandexSmartCaptchaAsync(filterContext, _appendUserIp).Wait();

            base.OnActionExecuting(filterContext);
        }
    }
}
