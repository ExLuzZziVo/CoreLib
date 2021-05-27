using CoreLib.ASP.Helpers.CheckHelpers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLib.ASP.Filters
{
    /// <summary>
    /// Google ReCaptcha validation action filter attribute
    /// </summary>
    /// <remarks>
    /// The form sent to the server must contain the key 'g-recaptcha-response' or 'g-recaptcha-response-v3'. Also the application configuration file must contain the values 'GoogleReCaptchaV2', 'GoogleReCaptchaV2Invisible' or 'GoogleReCaptchaV3' with a secret key
    /// </remarks>
    public class GoogleReCaptchaValidationActionFilterAttribute : ActionFilterAttribute
    {
        private readonly float? _requiredScore;
        private readonly string _actionName;
        private readonly bool _invisible;

        /// <summary>
        /// This constructor is used for ReCaptchaV2
        /// </summary>
        /// <param name="invisible">The flag indicating the use of invisible ReCaptchaV2. Default value: false</param>
        public GoogleReCaptchaValidationActionFilterAttribute(bool invisible = false)
        {
            _invisible = invisible;
        }

        /// <summary>
        /// This constructor is used for ReCaptchaV3
        /// </summary>
        /// <param name="actionName">RecaptchaV3 action name</param>
        /// <param name="requiredScore">Required ReCaptchaV3 score to pass the validation. Default value: 0.5f</param>
        public GoogleReCaptchaValidationActionFilterAttribute(string actionName, float requiredScore = 0.5f)
        {
            _actionName = actionName;
            _requiredScore = requiredScore;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CheckGoogleReCaptchaHelper.CheckGoogleReCaptchaAsync(filterContext, _invisible, _requiredScore, _actionName).Wait();
            base.OnActionExecuting(filterContext);
        }
    }
}