namespace CoreLib.ASP.CustomObjects.YouTube
{
    public class Item
    {
        public Item()
        {
            Id = new Id();
            Snippet = new Snippet();
        }

        public string Kind { get; set; }
        public string Etag { get; set; }
        public Id Id { get; set; }
        public Snippet Snippet { get; set; }
    }
}