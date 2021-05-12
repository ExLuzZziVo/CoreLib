namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.Videos
{
    public class YouTubeVideoResponseItem : YouTubeResponseItem
    {
        public YouTubeVideoResponseItem()
        {
            Items = new VideoItem[0];
        }

        public VideoItem[] Items { get; set; }
    }
}