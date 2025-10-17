namespace NewsPortal.Models
{
    public class Favorite
    {
        public long? FavoriteID { get; set; }
        public long ArticleID { get; set; }
        public string UserKey { get; set; } = "demo-user";
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
