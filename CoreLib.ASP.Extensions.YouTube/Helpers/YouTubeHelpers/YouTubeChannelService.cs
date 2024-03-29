﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CoreLib.ASP.Extensions.YouTube.Interfaces;
using CoreLib.ASP.Extensions.YouTube.Types.YouTube;
using CoreLib.ASP.Extensions.YouTube.Types.YouTube.Channels;
using CoreLib.ASP.Extensions.YouTube.Types.YouTube.PlaylistItems;
using CoreLib.ASP.Extensions.YouTube.Types.YouTube.Videos;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.ASP.Extensions.YouTube.Helpers.YouTubeHelpers
{
    public class YouTubeChannelService : IYouTubeChannelService
    {
        private static readonly JsonSerializerOptions WebJsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        private readonly HttpClient _client;

        public YouTubeChannelService(HttpClient client)
        {
            _client = client;
        }

        public async Task<YouTubeVideoResponseItem> YouTubeSearchVideosByChannelId(string key, string channelId,
            int maxResults, string pageToken)
        {
            var youTubeVideoResponseItem = new YouTubeVideoResponseItem();

            var url =
                $"https://www.googleapis.com/youtube/v3/search?key={key}&channelId={channelId}&part=snippet,id&order=date&maxResults={maxResults}{(pageToken.IsNullOrEmptyOrWhiteSpace() ? string.Empty : $"&pageToken={pageToken}")}";

            using (var response = await _client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseResult = await response.Content.ReadAsStringAsync();

                    youTubeVideoResponseItem =
                        JsonSerializer.Deserialize<YouTubeVideoResponseItem>(responseResult, WebJsonSerializerOptions);
                }
            }

            return youTubeVideoResponseItem;
        }

        public async Task<YouTubePlaylistResponseItem> YouTubeSearchVideosByPlaylistId(string key, string playlistId,
            int maxResults, string pageToken)
        {
            var youTubePlaylistResponseItem = new YouTubePlaylistResponseItem();

            var url =
                $"https://www.googleapis.com/youtube/v3/playlistItems?key={key}&playlistId={playlistId}&part=snippet,id&order=date&maxResults={maxResults}{(pageToken.IsNullOrEmptyOrWhiteSpace() ? string.Empty : $"&pageToken={pageToken}")}";

            using (var response = await _client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseResult = await response.Content.ReadAsStringAsync();

                    youTubePlaylistResponseItem =
                        JsonSerializer.Deserialize<YouTubePlaylistResponseItem>(responseResult,
                            WebJsonSerializerOptions);
                }
            }

            return youTubePlaylistResponseItem;
        }

        public async Task<YouTubeChannelResponseItem> YouTubeSearchChannelById(string key, string channelId)
        {
            var youTubeChannelResponseItem = new YouTubeChannelResponseItem();
            var url = $"https://www.googleapis.com/youtube/v3/channels?key={key}&id={channelId}&part=contentDetails";

            using (var response = await _client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseResult = await response.Content.ReadAsStringAsync();

                    youTubeChannelResponseItem =
                        JsonSerializer.Deserialize<YouTubeChannelResponseItem>(responseResult,
                            WebJsonSerializerOptions);
                }
            }

            return youTubeChannelResponseItem;
        }

        public async Task<IEnumerable<YouTubeDTO>> GetLastChannelVideos(string apiKey, string channelId,
            int maxResult = 10)
        {
            if (maxResult < 1 || maxResult > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(maxResult));
            }

            var response = await YouTubeSearchVideosByChannelId(
                apiKey,
                channelId,
                maxResult, null
            );

            var result = new YouTubeDTO[0];

            if (response.Items != null && response.Items.Any())
            {
                result = response.Items.Select(i => new YouTubeDTO
                {
                    Description = i.Snippet.Description, Id = i.Id.VideoId, Thumbnails = i.Snippet.Thumbnails,
                    Title = i.Snippet.Title, PublishDate = i.Snippet.PublishedAt
                }).ToArray();
            }

            return result;
        }

        public async Task<IEnumerable<YouTubeDTO>> GetAllUploadedChannelVideos(string apiKey, string channelId)
        {
            var channelSearchResponse = await YouTubeSearchChannelById(apiKey, channelId);
            var result = new List<YouTubeDTO>();

            if (channelSearchResponse.Items != null && channelSearchResponse.Items.Any())
            {
                var uploadPlayListId = channelSearchResponse.Items[0].ContentDetails.RelatedPlaylists.Uploads;

                var response = await YouTubeSearchVideosByPlaylistId(
                    apiKey,
                    uploadPlayListId,
                    50, null
                );

                if (response.Items != null && response.Items.Any())
                {
                    result = response.Items.Select(i => new YouTubeDTO
                    {
                        Description = i.Snippet.Description, Id = i.Snippet.ResourceId.VideoId,
                        Thumbnails = i.Snippet.Thumbnails,
                        Title = i.Snippet.Title, PublishDate = i.Snippet.PublishedAt
                    }).ToList();
                }

                var nextPageToken = response.NextPageToken;

                while (!nextPageToken.IsNullOrEmptyOrWhiteSpace())
                {
                    response = await YouTubeSearchVideosByPlaylistId(
                        apiKey,
                        uploadPlayListId,
                        50, nextPageToken
                    );

                    if (response.Items != null && response.Items.Any())
                    {
                        result.AddRange(response.Items.Select(i => new YouTubeDTO
                        {
                            Description = i.Snippet.Description, Id = i.Snippet.ResourceId.VideoId,
                            Thumbnails = i.Snippet.Thumbnails,
                            Title = i.Snippet.Title, PublishDate = i.Snippet.PublishedAt
                        }));
                    }

                    nextPageToken = response.NextPageToken;
                }
            }

            return result;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}