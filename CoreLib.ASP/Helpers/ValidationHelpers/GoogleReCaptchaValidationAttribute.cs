#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using CoreLib.ASP.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

#endregion

namespace CoreLib.ASP.Helpers.ValidationHelpers
{
    public class GoogleReCaptchaValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorResult = new Lazy<ValidationResult>(() =>
                new ValidationResult(CommonResources.ResourceManager.GetString("ReCaptchaValidationError"),
                    new[] {validationContext.MemberName}));
            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return errorResult.Value;
            var configuration = (IConfiguration) validationContext.GetService(typeof(IConfiguration));
            var reCaptchResponse = value.ToString();
            var reCaptchaSecret = configuration.GetValue<string>("GoogleReCaptcha:SecretKey");
            var httpClient = new HttpClient();
            var httpResponse = httpClient
                .GetAsync(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={reCaptchaSecret}&response={reCaptchResponse}")
                .Result;
            if (httpResponse.StatusCode != HttpStatusCode.OK) return errorResult.Value;
            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(jsonResponse);
            return jsonData.success != true.ToString().ToLower() ? errorResult.Value : ValidationResult.Success;
        }
    }
}