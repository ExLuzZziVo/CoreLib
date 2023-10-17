#region

using System;
using System.Threading.Tasks;

#endregion

namespace CoreLib.ASP.Helpers.CheckHelpers
{
    public interface ICheckGoogleReCaptchaHelper : IDisposable
    {
        /// <summary>
        /// Asynchronously validates Google's RecaptchaV2
        /// </summary>
        /// <param name="reCaptchaResponse">ReCaptchaV2 response</param>
        /// <param name="reCaptchaSecret">ReCaptchaV2 secret key</param>
        /// <returns>A task that represents the asynchronous validation of ReCaptchaV2. If the validation is passed, the result of the task will be true</returns>
        public Task<bool> CheckV2Async(string reCaptchaResponse, string reCaptchaSecret);

        /// <summary>
        /// Asynchronously validates Google's RecaptchaV3
        /// </summary>
        /// <param name="reCaptchaResponse">ReCaptchaV3 response</param>
        /// <param name="reCaptchaSecret">ReCaptchaV3 secret key</param>
        /// <param name="actionName">RecaptchaV3 action name</param>
        /// <param name="requiredScore">Required ReCaptchaV3 score to pass the validation</param>
        /// <returns>A task that represents the asynchronous validation of ReCaptchaV3. If the validation is passed, the result of the task will be true</returns>
        public Task<bool> CheckV3Async(string reCaptchaResponse, string reCaptchaSecret, string actionName,
            float requiredScore);
    }
}