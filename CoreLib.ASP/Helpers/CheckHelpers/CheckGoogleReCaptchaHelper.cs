#region

using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CoreLib.ASP.Types;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

#endregion

namespace CoreLib.ASP.Helpers.CheckHelpers
{
    public static class CheckGoogleReCaptchaHelper
    {
        /// <summary>
        /// Asynchronously validates Google's Recaptcha
        /// </summary>
        /// <param name="filterContext">Action/Page filter execution context</param>
        /// <param name="invisibleV2">The flag indicating the use of invisible ReCaptchaV2. Default value: false</param>
        /// <param name="requiredScore">Required ReCaptchaV3 score to pass the validation. Default value: null</param>
        /// <param name="actionName">RecaptchaV3 action name</param>
        /// <returns>A task that represents the asynchronous validation of ReCaptcha. If validation was failed, it adds a model error to <see cref="filterContext"/> model state</returns>
        /// <remarks>
        /// The form sent to the server must contain the key 'g-recaptcha-response' or 'g-recaptcha-response-v3'. Also the application configuration file must contain the values 'GoogleReCaptchaV2', 'GoogleReCaptchaV2Invisible' or 'GoogleReCaptchaV3' with a secret key
        /// </remarks>
        internal static async Task CheckGoogleReCaptchaAsync(this ActionContext filterContext, bool invisibleV2 = false,
            float? requiredScore = null, string actionName = null)
        {
            var reCaptchaResponse = string.Empty;
            var reCaptchaV3 = false;

            if (filterContext.HttpContext.Request.Form.ContainsKey("g-recaptcha-response"))
            {
                reCaptchaResponse = filterContext.HttpContext.Request.Form["g-recaptcha-response"];
                reCaptchaV3 = false;
            }
            else if (filterContext.HttpContext.Request.Form.ContainsKey("g-recaptcha-response-v3"))
            {
                if (requiredScore == null)
                {
                    throw new ArgumentNullException(nameof(requiredScore));
                }

                reCaptchaResponse = filterContext.HttpContext.Request.Form["g-recaptcha-response-v3"];
                reCaptchaV3 = true;
            }

            if (reCaptchaResponse.IsNullOrEmptyOrWhiteSpace())
            {
                AddReCaptchaValidationError(filterContext);

                return;
            }

            var configuration =
                (IConfiguration) filterContext.HttpContext.RequestServices.GetService(typeof(IConfiguration));

            var reCaptchaSecret =
                configuration.GetValue<string>(
                    $"GoogleReCaptchaV{(reCaptchaV3 ? "3" : invisibleV2 ? "2Invisible" : "2")}:SecretKey");

            if (reCaptchaV3)
            {
                if (!await CheckV3Async(reCaptchaResponse, reCaptchaSecret, actionName, requiredScore.Value))
                {
                    AddReCaptchaValidationError(filterContext);
                }
            }
            else
            {
                if (!await CheckV2Async(reCaptchaResponse, reCaptchaSecret))
                {
                    AddReCaptchaValidationError(filterContext);
                }
            }
        }

        /// <summary>
        /// Adds a model error to <see cref="filterContext"/> model state
        /// </summary>
        /// <param name="filterContext">Action/Page filter execution context</param>
        private static void AddReCaptchaValidationError(ActionContext filterContext)
        {
            filterContext.ModelState.AddModelError(string.Empty,
                Resources.ValidationStrings.ResourceManager.GetString("ReCaptchaValidationError"));
        }

        /// <summary>
        /// Asynchronously validates Google's RecaptchaV2
        /// </summary>
        /// <param name="reCaptchaResponse">ReCaptchaV2 response</param>
        /// <param name="reCaptchaSecret">ReCaptchaV2 secret key</param>
        /// <returns>A task that represents the asynchronous validation of ReCaptchaV2. If the validation is passed, the result of the task will be true</returns>
        public static Task<bool> CheckV2Async(string reCaptchaResponse, string reCaptchaSecret)
        {
            return CheckAsync(reCaptchaResponse, reCaptchaSecret);
        }

        /// <summary>
        /// Asynchronously validates Google's RecaptchaV3
        /// </summary>
        /// <param name="reCaptchaResponse">ReCaptchaV3 response</param>
        /// <param name="reCaptchaSecret">ReCaptchaV3 secret key</param>
        /// <param name="actionName">RecaptchaV3 action name</param>
        /// <param name="requiredScore">Required ReCaptchaV3 score to pass the validation</param>
        /// <returns>A task that represents the asynchronous validation of ReCaptchaV3. If the validation is passed, the result of the task will be true</returns>
        public static Task<bool> CheckV3Async(string reCaptchaResponse, string reCaptchaSecret, string actionName,
            float requiredScore)
        {
            return CheckAsync(reCaptchaResponse, reCaptchaSecret, actionName, requiredScore);
        }

        /// <summary>
        /// Asynchronously validates Google's Recaptcha
        /// </summary>
        /// <param name="reCaptchaResponse">ReCaptcha response</param>
        /// <param name="reCaptchaSecret">ReCaptcha secret key</param>
        /// <param name="actionName">RecaptchaV3 action name. Default value: null</param>
        /// <param name="requiredScore">Required ReCaptchaV3 score to pass the validation. Default value: null</param>
        /// <returns>A task that represents the asynchronous validation of ReCaptcha. If the validation is passed, the result of the task will be true</returns>
        private static async Task<bool> CheckAsync(string reCaptchaResponse, string reCaptchaSecret,
            string actionName = null, float? requiredScore = null)
        {
            if (reCaptchaSecret.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(reCaptchaSecret));
            }

            if (requiredScore != null && (requiredScore < 0.1 || requiredScore > 1))
            {
                throw new ArgumentOutOfRangeException(nameof(requiredScore));
            }

            if (reCaptchaResponse.IsNullOrEmptyOrWhiteSpace())
            {
                return false;
            }

            string jsonResponse;

            //ToDo dependency injection
            using (var httpClient = new HttpClient())
            {
                var httpResponse = httpClient
                    .GetAsync(
                        $"https://www.google.com/recaptcha/api/siteverify?secret={reCaptchaSecret}&response={reCaptchaResponse}")
                    .Result;

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }

                jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            }

            var reCaptchaResponseResult = JsonSerializer.Deserialize<GoogleReCaptchaResponse>(jsonResponse,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            if (requiredScore != null)
            {
                if (!reCaptchaResponseResult.Success)
                {
                    return false;
                }

                if (!actionName.IsNullOrEmptyOrWhiteSpace() && actionName != reCaptchaResponseResult.Action)
                {
                    return false;
                }

                return !(reCaptchaResponseResult.Score + 0.01 < requiredScore);
            }

            return reCaptchaResponseResult.Success;
        }
    }
}