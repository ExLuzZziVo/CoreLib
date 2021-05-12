#region

using System;
using System.Net.Http;

#endregion

namespace CoreLib.CORE.Helpers.HttpClientHelpers
{
    public static class HttpRequestExtensions
    {
        private const string TimeoutPropertyKey = "RequestTimeout";

        /// <summary>
        /// Sets timeout for http request
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="timeout">Timeout value</param>
        public static void SetTimeout(this HttpRequestMessage request, TimeSpan? timeout)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Properties[TimeoutPropertyKey] = timeout;
        }

        /// <summary>
        /// Gets timeout of http request
        /// </summary>
        /// <param name="request">Http request</param>
        /// <returns>Timeout value</returns>
        public static TimeSpan? GetTimeout(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Properties.TryGetValue(TimeoutPropertyKey, out var value) && value is TimeSpan timeout)
            {
                return timeout;
            }

            return null;
        }
    }
}