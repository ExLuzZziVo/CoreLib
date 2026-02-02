namespace CoreLib.ASP.Types
{
    /// <summary>
    /// Yandex SnartCaptcha response
    /// </summary>
    public class YandexSmartCaptchaResponse
    {
        public string Status { get; init; }

        public string Message { get; init; }

        public string Host { get; init; }
    }
}
