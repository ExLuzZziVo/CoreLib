namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.PlaylistItems
{
    public class YouTubePlaylistResponseItem : YouTubeResponseItem
    {
        public YouTubePlaylistResponseItem()
        {
            Items = new PlaylistItem[0];
        }

        public PlaylistItem[] Items { get; set; }
    }
}