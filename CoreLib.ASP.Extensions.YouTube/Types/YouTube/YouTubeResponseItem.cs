namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube
{
    public abstract class YouTubeResponseItem
    {
        public YouTubeResponseItem()
        {
            PageInfo = new PageInfo();
        }

        public string Kind { get; set; }

        public string Etag { get; set; }

        public string NextPageToken { get; set; }

        public string RegionCode { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}