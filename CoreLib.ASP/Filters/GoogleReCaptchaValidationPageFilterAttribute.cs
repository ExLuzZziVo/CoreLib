using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoreLib.ASP.Helpers.CheckHelpers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLib.ASP.Filters
{
    /// <summary>
    /// Google's ReCaptcha validation page filter attribute
    /// </summary>
    /// <remarks>
    /// The form sent to the server must contain the key 'g-recaptcha-response' or 'g-recaptcha-response-v3'. Also the application configuration file must contain the values 'GoogleReCaptchaV2', 'GoogleReCaptchaV2Invisible' or 'GoogleReCaptchaV3' with a secret key
    /// </remarks>
    public class GoogleReCaptchaValidationPageFilterAttribute : Attribute, IAsyncPageFilter
    {
        private readonly float? _requiredScore;
        private readonly string _actionName;
        private readonly bool _invisible;

        /// <summary>
        /// Names of page handlers to validate. If empty, only POST handlers are validated
        /// </summary>
        public string[] HandlerNames { get; set; } = new string[0];

        /// <summary>
        /// This constructor is used for ReCaptchaV2
        /// </summary>
        /// <param name="invisible">The flag indicating the use of invisible ReCaptchaV2. Default value: false</param>
        public GoogleReCaptchaValidationPageFilterAttribute(bool invisible = false)
        {
            _invisible = invisible;
        }

        /// <summary>
        /// This constructor is used for ReCaptchaV3
        /// </summary>
        /// <param name="actionName">RecaptchaV3 action name</param>
        /// <param name="requiredScore">Required ReCaptchaV3 score to pass the validation. Default value: 0.5f</param>
        public GoogleReCaptchaValidationPageFilterAttribute(string actionName, float requiredScore = 0.5f)
        {
            _actionName = actionName;
            _requiredScore = requiredScore;
        }

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
                    ? CheckGoogleReCaptchaHelper.CheckGoogleReCaptchaAsync(context, _invisible, _requiredScore, _actionName)
                    : Task.CompletedTask;
            }

            return HandlerNames.Contains(context.HandlerMethod.Name)
                ? CheckGoogleReCaptchaHelper.CheckGoogleReCaptchaAsync(context, _invisible, _requiredScore, _actionName)
                : Task.CompletedTask;
        }
    }
}