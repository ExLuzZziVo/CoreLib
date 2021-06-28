using System;
using CoreLib.CORE.Helpers.StringHelpers;
using CoreLib.CORE.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.ASP.Filters
{
    /// <summary>
    /// An action filter attribute that limits the execution of the target action in different ways
    /// </summary>
    public class ThrottleActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// An unique key that is used to store the connection info in <see cref="IMemoryCache"/>
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The time in milliseconds clients must wait before executing the target action
        /// </summary>
        public long ThrottlingPeriod { get; set; }

        /// <summary>
        /// The number of allowed connections for clients during the <see cref="ThrottlingPeriod"/> an limited by the <see cref="ThrottlingBy"/>
        /// </summary>
        /// <remarks>
        /// Default value: 1
        /// </remarks>
        public int MaximumConnectionsCountBeforeThrottling { get; set; } = 1;

        /// <summary>
        /// The type of the limitation
        /// </summary>
        /// <remarks>
        /// Default value: <see cref="ThrottlingType.ByIp"/>
        /// </remarks>
        public ThrottlingType ThrottlingBy { get; set; } = ThrottlingType.ByIp;

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (Key.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(Key));
            }

            if (MaximumConnectionsCountBeforeThrottling < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaximumConnectionsCountBeforeThrottling));
            }

            if (ThrottlingPeriod <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ThrottlingPeriod));
            }

            var throttlingTypeKeyPart = ThrottlingBy.ToString();

            switch (ThrottlingBy)
            {
                case ThrottlingType.ByIp:
                    throttlingTypeKeyPart += $":{context.HttpContext.Connection.RemoteIpAddress}";

                    break;
                case ThrottlingType.ByUser:
                    if (context.HttpContext.User.Identity.IsAuthenticated)
                    {
                        throttlingTypeKeyPart += $":{context.HttpContext.User.Identity.Name}";
                    }
                    else
                    {
                        return;
                    }

                    break;
            }

            var key = $"{nameof(ThrottleActionFilterAttribute)}:{Key}:{throttlingTypeKeyPart}";

            var memoryCache = context.HttpContext.RequestServices.GetService<IMemoryCache>();

            var semaphore = await ConcurrentSemaphore.WaitAsync(key);

            try
            {
                if (!memoryCache.TryGetValue(key, out Tuple<int, DateTimeOffset> throttleData))
                {
                    var expirationTime = DateTimeOffset.Now.Add(TimeSpan.FromMilliseconds(ThrottlingPeriod));

                    memoryCache.Set(key,
                        new Tuple<int, DateTimeOffset>(MaximumConnectionsCountBeforeThrottling, expirationTime),
                        expirationTime);
                }
                else
                {
                    if (throttleData.Item1 <= 1)
                    {
                        context.Result = new ConflictResult();
                    }
                    else
                    {
                        memoryCache.Set(key, new Tuple<int, DateTimeOffset>(throttleData.Item1 - 1, throttleData.Item2),
                            throttleData.Item2);
                    }
                }
            }
            finally
            {
                semaphore.Dispose();
            }

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Type of the limitation
        /// </summary>
        public enum ThrottlingType : byte
        {
            /// <summary>
            /// Limits requests by clients Ip address
            /// </summary>
            ByIp,

            /// <summary>
            /// Limits requests by identity user name
            /// </summary>
            ByUser,

            /// <summary>
            /// Limits all requests
            /// </summary>
            All
        }
    }
}