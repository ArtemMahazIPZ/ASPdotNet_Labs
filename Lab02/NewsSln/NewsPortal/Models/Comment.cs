using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models
{
    public class Comment
    {
        public long? CommentID { get; set; }
        [Required]
        public long ArticleID { get; set; }
        [Required, MaxLength(100)]
        public string UserName { get; set; } = "Anonymous";
        [Required, MaxLength(2000)]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
