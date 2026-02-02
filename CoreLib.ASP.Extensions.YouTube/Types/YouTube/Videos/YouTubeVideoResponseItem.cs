namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.Videos
{
    public class YouTubeVideoResponseItem : YouTubeResponseItem
    {
        public YouTubeVideoResponseItem()
        {
            Items = [];
        }

        public VideoItem[] Items { get; set; }
    }
}
