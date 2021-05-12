namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.Videos
{
    public class VideoItem : Item
    {
        public VideoItem()
        {
            Id = new VideoId();
            Snippet = new VideoSnippet();
        }

        public VideoId Id { get; set; }

        public VideoSnippet Snippet { get; set; }
    }
}