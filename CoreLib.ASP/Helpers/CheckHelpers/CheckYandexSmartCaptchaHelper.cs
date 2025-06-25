#region

using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CoreLib.ASP.Resources;
using CoreLib.ASP.Types;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace CoreLib.ASP.Helpers.CheckHelpers
{
    public class CheckYandexSmartCaptchaHelper : ICheckYandexSmartCaptchaHelper
    {
        private static readonly JsonSerializerOptions ResponseJsonSerializerOptions = new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true };
        private readonly HttpClient _httpClient;

        [ActivatorUtilitiesConstructor]
        public CheckYandexSmartCaptchaHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckAsync(string verificationToken, string secretKey, IPAddress userIp = null)
        {
            if (secretKey.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            if (verificationToken.IsNullOrEmptyOrWhiteSpace())
            {
                return false;
            }

            string jsonResponse;

            using (var response = await _httpClient
                       .GetAsync(
                           $"https://smartcaptcha.yandexcloud.net/validate?secret={secretKey}&token={verificationToken}{(userIp == null ? string.Empty : $"&ip={userIp.ToString()}")}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                jsonResponse = await response.Content.ReadAsStringAsync();
            }

            var captchaResponseResult = JsonSerializer.Deserialize<YandexSmartCaptchaResponse>(jsonResponse, ResponseJsonSerializerOptions);

            if (captchaResponseResult.Status.Equals("failed", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (captchaResponseResult.Host.IsNullOrEmptyOrWhiteSpace())
            {
                throw new InvalidOperationException("The cloud is blocked or there was an internal service failure.");
            }

            return true;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        /// <summary>
        /// Asynchronously validates Yandex SmartCaptcha
        /// </summary>
        /// <param name="filterContext">Action/Page filter execution context</param>
        /// <param name="appendUserIp">The flag indicating the use of the user's IP address</param>
        /// <returns>A task that represents the asynchronous validation of Yandex SmartCaptcha. If validation was failed, it adds a model error to <see cref="filterContext"/> model state</returns>
        /// <remarks>
        /// The form sent to the server must contain the key 'smart-token'. Also, the application configuration file must contain the section 'YandexSmartCaptcha' with a secret key
        /// </remarks>
        internal static async Task CheckYandexSmartCaptchaAsync(ActionContext filterContext, bool appendUserIp)
        {
            var verificationToken = string.Empty;

            if (filterContext.HttpContext.Request.Form.ContainsKey("smart-token"))
            {
                verificationToken = filterContext.HttpContext.Request.Form["smart-token"];
            }

            if (verificationToken.IsNullOrEmptyOrWhiteSpace())
            {
                AddCaptchaValidationError(filterContext);

                return;
            }

            var configuration = filterContext.HttpContext.RequestServices.GetService<IConfiguration>();

            var captchaSecret = configuration.GetValue<string>("YandexSmartCaptcha:SecretKey");

            var checkYandexSmartCaptchaHelper =
                filterContext.HttpContext.RequestServices.GetService<ICheckYandexSmartCaptchaHelper>();

            if (!await checkYandexSmartCaptchaHelper.CheckAsync(verificationToken, captchaSecret, appendUserIp ? filterContext.HttpContext.Connection.RemoteIpAddress : null))
            {
                AddCaptchaValidationError(filterContext);
            }
        }

        /// <summary>
        /// Adds a model error to <see cref="filterContext"/> model state
        /// </summary>
        /// <param name="filterContext">Action/Page filter execution context</param>
        private static void AddCaptchaValidationError(ActionContext filterContext)
        {
            filterContext.ModelState.AddModelError(string.Empty,
                ValidationStrings.ResourceManager.GetString("CaptchaValidationError"));
        }
    }
}
