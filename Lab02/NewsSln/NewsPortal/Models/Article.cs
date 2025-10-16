using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsPortal.Models
{
    public class Article
    {
        public long? ArticleID { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Author { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(300)]
        public string? CoverImageUrl { get; set; }
    }
}
