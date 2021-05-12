namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.Channels
{
    public class ChannelContentDetails
    {
        public ChannelContentDetails()
        {
            RelatedPlaylists = new ChannelRelatedPlaylists();
        }

        public ChannelRelatedPlaylists RelatedPlaylists { get; set; }
    }
}