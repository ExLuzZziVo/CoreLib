#region

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.Extensions.Logging;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;
using Math = System.Math;

#endregion

namespace CoreLib.CORE.Helpers.HttpClientHelpers
{
    // https://stackoverflow.com/a/78494040
    /// <summary>
    /// This handler adds logging for <see cref="HttpClient"/> requests and responses
    /// </summary>
    internal class TraceLogHandler: DelegatingHandler
    {
        private readonly string[] _headersToRedact;
        private readonly ILogger _logger;
        private readonly int _requestBodyLogLimit;
        private readonly int _responseBodyLogLimit;

        /// <summary>
        /// This handler adds logging for <see cref="HttpClient"/> requests and responses
        /// </summary>
        /// <example>
        /// To use it, add "System.Net.Http.HttpClient.YOUR_HTTP_CLIENT_NAME": "Trace" to the "Logging.LogLevel" section in the appsettings.json file
        /// </example>
        /// <param name="logger">The <see cref="ILogger"/></param>
        /// <param name="headersToRedact">The names of the headers whose values will be masked</param>
        /// <param name="requestBodyLogLimit">Maximum request body size to log (in symbols). Defaults to 32768 symbols. If the value is 0, the request body will not be logged</param>
        /// <param name="responseBodyLogLimit">Maximum response body size to log (in symbols). Defaults to 32768 symbols. If the value is 0, the response body will not be logged</param>
        /// <remarks>This is useful for debugging third-party services in production</remarks>
        public TraceLogHandler(ILogger logger, string[] headersToRedact = null, int requestBodyLogLimit = 32768,
            int responseBodyLogLimit = 32768)
        {
            if (requestBodyLogLimit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(requestBodyLogLimit));
            }

            if (responseBodyLogLimit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(responseBodyLogLimit));
            }

            _logger = logger;
            _headersToRedact = headersToRedact;
            _requestBodyLogLimit = requestBodyLogLimit;
            _responseBodyLogLimit = responseBodyLogLimit;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_logger == null || !_logger.IsEnabled(LogLevel.Trace))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var requestContent = request.Content == null || _requestBodyLogLimit < 1
                ? "(empty)"
                : await request.Content.ReadAsStringAsync(
#if NET6_0_OR_GREATER
                        cancellationToken
#endif
                );

            _logger.LogTrace("Request:\n\n" +
                             "Path: {Uri}\n" +
                             "Method: {Method}\n" +
                             "Headers:\n\t" +
                             "{Headers}\n" +
                             "ContentType: {ContentType}\n" +
                             "ContentLength: {ContentLength}\n" +
                             "Content:\n" +
                             "{Content}",
                request.RequestUri,
                request.Method,
                string.Join("\n\t", GetHeadersStrings(request.Headers)),
                request.Content?.Headers.ContentType?.ToString() ?? "(none)",
                request.Content?.Headers.ContentLength ?? 0,
                requestContent.Substring(0, Math.Min(requestContent.Length, _requestBodyLogLimit)));

            var response =
                await base.SendAsync(request,
                    cancellationToken);

            var responseContent = response.Content == null || _responseBodyLogLimit < 1
                ? "(empty)"
                : await response.Content.ReadAsStringAsync(
#if NET6_0_OR_GREATER
                        cancellationToken
#endif
                );

            _logger.LogTrace("Response:\n\n" +
                             "StatusCode: {StatusCode}\n" +
                             "Headers:\n\t" +
                             "{Headers}\n" +
                             "ContentType: {ContentType}\n" +
                             "ContentLength: {ContentLength}\n" +
                             "Content:\n" +
                             "{Content}",
                response.StatusCode.ToString("D"),
                string.Join("\n\t", GetHeadersStrings(response.Headers)),
                response.Content?.Headers.ContentType?.ToString() ?? "(none)",
                response.Content?.Headers.ContentLength ?? 0,
                responseContent.Substring(0, Math.Min(responseContent.Length, _responseBodyLogLimit)));

            return response;
        }

        private IEnumerable<string> GetHeadersStrings(HttpHeaders headers)
        {
            foreach (var h in headers)
            {
                var headerValue = h.Value?.FirstOrDefault();

                if (headerValue.IsNullOrEmptyOrWhiteSpace())
                {
                    continue;
                }

                if (_headersToRedact != null)
                {
                    if (_headersToRedact.Contains(h.Key))
                    {
                        headerValue = new string('*', headerValue!.Length);
                    }
                }

                yield return $"{h.Key}: {headerValue}";
            }
        }
    }
}
