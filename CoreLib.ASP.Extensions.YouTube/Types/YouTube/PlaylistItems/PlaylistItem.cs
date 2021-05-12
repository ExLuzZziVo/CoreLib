namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.PlaylistItems
{
    public class PlaylistItem : Item
    {
        public PlaylistItem()
        {
            Snippet = new PlaylistSnippet();
        }

        public PlaylistSnippet Snippet { get; set; }

        public string Id { get; set; }
    }
}