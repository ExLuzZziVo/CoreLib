#region

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using CoreLib.ASP.Extensions.YouTube.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;

#endregion

namespace CoreLib.ASP.Extensions.YouTube.Helpers.YouTubeHelpers
{
    public static class YouTubeServicesConfiguration
    {
        /// <summary>
        /// Configures YouTube Api services
        /// </summary>
        /// <param name="services">Service collection</param>
        public static void YouTubeApiServicesConfiguration(this IServiceCollection services)
        {
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            services.AddHttpClient<IYouTubeChannelService, YouTubeChannelService>(client =>
                {
                    client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/search?");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "YoutubeHttpService");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddPolicyHandler(retryPolicy);
        }
    }
}