#region

using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace CoreLib.CORE.Helpers.HttpClientHelpers
{
    public static class HttpClientBuilderExtensions
    {
        // https://stackoverflow.com/a/78494040
        /// <summary>
        /// This extension method adds logging for <see cref="HttpClient"/> requests and responses
        /// </summary>
        /// <example>
        /// To use it, add "System.Net.Http.HttpClient.YOUR_HTTP_CLIENT_NAME": "Trace" to the "Logging.LogLevel" section in the appsettings.json file
        /// </example>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/></param>
        /// <param name="headersToRedact">The names of the headers whose values will be masked</param>
        /// <param name="requestBodyLogLimit">Maximum request body size to log (in symbols). Defaults to 32768 symbols. If the value is 0, the request body will not be logged</param>
        /// <param name="responseBodyLogLimit">Maximum response body size to log (in symbols). Defaults to 32768 symbols. If the value is 0, the response body will not be logged</param>
        /// <returns>An <see cref="IHttpClientBuilder"/> that can be used to configure the client</returns>
        /// <remarks>This is useful for debugging third-party services in production</remarks>
        public static IHttpClientBuilder AddTraceLogHandler(
            this IHttpClientBuilder builder, string[] headersToRedact = null, int requestBodyLogLimit = 32768,
            int responseBodyLogLimit = 32768)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddHttpMessageHandler((sp) =>
            {
                var logger = sp.GetRequiredService<ILoggerFactory>()
                    .CreateLogger($"System.Net.Http.HttpClient.{builder.Name}");

                return new TraceLogHandler(logger, headersToRedact, requestBodyLogLimit, responseBodyLogLimit);
            });
        }
    }
}
