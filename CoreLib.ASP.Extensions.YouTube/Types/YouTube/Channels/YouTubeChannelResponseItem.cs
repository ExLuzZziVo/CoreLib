#region

#endregion

namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.Channels
{
    public class YouTubeChannelResponseItem : YouTubeResponseItem
    {
        public YouTubeChannelResponseItem()
        {
            Items = [];
        }

        public ChannelItem[] Items { get; set; }
    }
}
