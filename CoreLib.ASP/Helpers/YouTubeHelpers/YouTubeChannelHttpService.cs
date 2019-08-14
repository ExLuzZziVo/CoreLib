#region

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CoreLib.ASP.CustomObjects.YouTube;
using Newtonsoft.Json;

#endregion

namespace CoreLib.ASP.Helpers.YouTubeHelpers
{
    public class YouTubeChannelHttpService : IYouTubeChannelHttpService
    {
        private readonly HttpClient _client;

        public YouTubeChannelHttpService(HttpClient client)
        {
            _client = client;
        }

        public async Task<YouTubeResponseItem> YouTubeChannelSearch(string key, string channelId, int maxResults)
        {
            var youTubeResponseItem = new YouTubeResponseItem();
            var url =
                $"https://www.googleapis.com/youtube/v3/search?key={key}&channelId={channelId}&part=snippet,id&order=date&maxResults={maxResults}";
            using (var response = await _client.SendAsync(new HttpRequestMessage {RequestUri = new Uri(url)}))
            {
                if (response.IsSuccessStatusCode)
                {
                    string data;
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var myStream = new StreamReader(stream))
                    {
                        data = await myStream.ReadToEndAsync();
                    }

                    youTubeResponseItem = JsonConvert.DeserializeObject<YouTubeResponseItem>(data);
                }
            }

            return youTubeResponseItem;
        }
    }
}