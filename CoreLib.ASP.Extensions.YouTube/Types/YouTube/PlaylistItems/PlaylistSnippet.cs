namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.PlaylistItems
{
    public class PlaylistSnippet : Snippet
    {
        public PlaylistSnippet()
        {
            ResourceId = new PlaylistResourceId();
        }

        public string PlaylistId { get; set; }

        public int Position { get; set; }

        public PlaylistResourceId ResourceId { get; set; }
    }
}