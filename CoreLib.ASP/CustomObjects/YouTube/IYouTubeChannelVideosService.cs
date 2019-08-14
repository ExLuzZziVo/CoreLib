#region

using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace CoreLib.ASP.CustomObjects.YouTube
{
    public interface IYouTubeChannelVideosService
    {
        Task<IEnumerable<YouTubeDTO>> GetChannelVideos(string apiKey, string channelId, int maxResult = 10);
    }
}