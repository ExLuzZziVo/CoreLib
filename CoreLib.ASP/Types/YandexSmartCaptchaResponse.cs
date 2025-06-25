#region

using System;

#endregion

namespace CoreLib.ASP.Types
{
    /// <summary>
    /// Yandex SnartCaptcha response
    /// </summary>
    public class YandexSmartCaptchaResponse
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public string Host { get; set; }
    }
}
