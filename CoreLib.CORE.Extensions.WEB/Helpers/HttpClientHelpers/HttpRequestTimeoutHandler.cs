#region

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace CoreLib.CORE.Helpers.HttpClientHelpers
{
    /// <summary>
    /// A helper class that handles http request cancellation using timeout and <see cref="CancellationToken"/>
    /// </summary>
    public class HttpRequestTimeoutHandler : DelegatingHandler
    {
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(100);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            using (var cts = GetCancellationTokenSource(request, cancellationToken))
            {
                try
                {
                    return await base.SendAsync(request, cts?.Token ?? cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
            }
        }

        /// <summary>
        /// Creates cancellation token source by combining http request's timeout and cancellation token
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Cancellation token source</returns>
        private CancellationTokenSource GetCancellationTokenSource(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var timeout = request.GetTimeout() ?? RequestTimeout;

            if (timeout == Timeout.InfiniteTimeSpan)
            {
                return null;
            }

            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            return cts;
        }
    }
}