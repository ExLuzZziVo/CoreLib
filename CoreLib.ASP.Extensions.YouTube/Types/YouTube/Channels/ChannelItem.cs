#region

#endregion

namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube.Channels
{
    public class ChannelItem : Item
    {
        public ChannelItem()
        {
            ContentDetails = new ChannelContentDetails();
        }

        public string Id { get; set; }

        public ChannelContentDetails ContentDetails { get; set; }
    }
}