namespace CoreLib.ASP.CustomObjects.YouTube
{
    public class YouTubeResponseItem
    {
        public YouTubeResponseItem()
        {
            PageInfo = new PageInfo();
            Items = new Item[0];
        }

        public string Kind { get; set; }
        public string Etag { get; set; }
        public string NextPageToken { get; set; }
        public string RegionCode { get; set; }
        public PageInfo PageInfo { get; set; }
        public Item[] Items { get; set; }
    }
}