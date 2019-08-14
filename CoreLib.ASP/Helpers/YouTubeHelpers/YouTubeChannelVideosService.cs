#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.ASP.CustomObjects.YouTube;

#endregion

namespace CoreLib.ASP.Helpers.YouTubeHelpers
{
    public class YouTubeChannelVideosService : IYouTubeChannelVideosService
    {
        private readonly IYouTubeChannelHttpService _youTubeChannelHttpService;

        public YouTubeChannelVideosService(IYouTubeChannelHttpService youTubeChannelHttpService)
        {
            _youTubeChannelHttpService = youTubeChannelHttpService;
        }

        public async Task<IEnumerable<YouTubeDTO>> GetChannelVideos(string apiKey, string channelId, int maxResult = 10)
        {
            var response = await _youTubeChannelHttpService.YouTubeChannelSearch(
                apiKey,
                channelId,
                maxResult
            );
            var result = new List<YouTubeDTO>();
            if (response.Items != null && response.Items.Any())
                result = response.Items.Select(i => new YouTubeDTO
                {
                    Description = i.Snippet.Description, Id = i.Id.VideoId, Thumbnails = i.Snippet.Thumbnails,
                    Title = i.Snippet.Title, PublishDate = i.Snippet.PublishedAt
                }).ToList();
            return result;
        }
    }
}