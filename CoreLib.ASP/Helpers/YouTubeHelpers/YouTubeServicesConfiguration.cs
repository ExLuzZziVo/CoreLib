#region

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using CoreLib.ASP.CustomObjects.YouTube;
using Microsoft.Extensions.DependencyInjection;
using Polly;

#endregion

namespace CoreLib.ASP.Helpers.YouTubeHelpers
{
    public static class YouTubeServicesConfiguration
    {
        public static void YouTubeApiServicesConfiguration(this IServiceCollection services)
        {
            //services.AddScoped<IYouTubeChannelHttpService, YouTubeChannelHttpService>();
            services.AddScoped<IYouTubeChannelVideosService, YouTubeChannelVideosService>();
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            services.AddHttpClient<IYouTubeChannelHttpService, YouTubeChannelHttpService>(client =>
                {
                    client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/search?");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "YoutubeHttpService");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddPolicyHandler(retryPolicy);
        }
    }
}