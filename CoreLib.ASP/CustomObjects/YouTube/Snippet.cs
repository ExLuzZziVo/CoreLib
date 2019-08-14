#region

using System;

#endregion

namespace CoreLib.ASP.CustomObjects.YouTube
{
    public class Snippet
    {
        public Snippet()
        {
            Thumbnails = new Thumbnails();
        }

        public DateTime PublishedAt { get; set; }
        public string ChannelId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Thumbnails Thumbnails { get; set; }
        public string ChannelTitle { get; set; }
        public string LiveBroadcastContent { get; set; }
    }
}