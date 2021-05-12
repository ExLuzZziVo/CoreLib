namespace CoreLib.ASP.Extensions.YouTube.Types.YouTube
{
    public class Thumbnails
    {
        public Thumbnails()
        {
            Default = new Default();
            Medium = new Medium();
            High = new High();
        }

        public Default Default { get; set; }

        public Medium Medium { get; set; }

        public High High { get; set; }
    }
}