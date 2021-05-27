#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLib.ASP.Extensions.YouTube.Types.YouTube;
using CoreLib.ASP.Extensions.YouTube.Types.YouTube.Channels;
using CoreLib.ASP.Extensions.YouTube.Types.YouTube.PlaylistItems;
using CoreLib.ASP.Extensions.YouTube.Types.YouTube.Videos;

#endregion

namespace CoreLib.ASP.Extensions.YouTube.Interfaces
{
    public interface IYouTubeChannelService: IDisposable
    {
        Task<YouTubeVideoResponseItem> YouTubeSearchVideosByChannelId(string key, string channelId, int maxResults,
            string pageToken);

        Task<YouTubePlaylistResponseItem> YouTubeSearchVideosByPlaylistId(string key, string playlistId, int maxResults,
            string pageToken);

        Task<YouTubeChannelResponseItem> YouTubeSearchChannelById(string key, string channelId);

        Task<IEnumerable<YouTubeDTO>> GetLastChannelVideos(string apiKey, string channelId, int maxResult = 10);

        Task<IEnumerable<YouTubeDTO>> GetAllUploadedChannelVideos(string apiKey, string channelId);
    }
}