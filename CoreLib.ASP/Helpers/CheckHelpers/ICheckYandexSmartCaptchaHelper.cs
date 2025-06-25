#region

using System;
using System.Net;
using System.Threading.Tasks;

#endregion

namespace CoreLib.ASP.Helpers.CheckHelpers
{
    public interface ICheckYandexSmartCaptchaHelper : IDisposable
    {
        /// <summary>
        /// Asynchronously validates Yandex SmartCaptcha
        /// </summary>
        /// <param name="verificationToken">A one-time token received after passing verification</param>
        /// <param name="secretKey">Yandex SmartCaptcha secret key</param>
        /// <param name="userIp">IP address of the user from whom the request to verify the token came. Default value: null.</param>
        /// <returns>A task that represents the asynchronous validation of Yandex SmartCaptcha. If the validation is passed, the result of the task will be true</returns>
        public Task<bool> CheckAsync(string verificationToken, string secretKey, IPAddress userIp = null);
    }
}
