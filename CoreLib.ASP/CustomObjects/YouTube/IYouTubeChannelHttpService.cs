#region

using System.Threading.Tasks;

#endregion

namespace CoreLib.ASP.CustomObjects.YouTube
{
    public interface IYouTubeChannelHttpService
    {
        Task<YouTubeResponseItem> YouTubeChannelSearch(string key, string channelId, int maxResults);
    }
}