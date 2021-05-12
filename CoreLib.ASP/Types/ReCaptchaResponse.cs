using System;

namespace CoreLib.ASP.Types
{
    /// <summary>
    /// Google ReCaptcha response
    /// </summary>
    public class GoogleReCaptchaResponse
    {
        public bool Success { get; set; }

        public DateTime Challenge_TS { get; set; }

        public string HostName { get; set; }

        public string Apk_Package_Name { get; set; }

        public float? Score { get; set; }

        public string Action { get; set; }
    }
}