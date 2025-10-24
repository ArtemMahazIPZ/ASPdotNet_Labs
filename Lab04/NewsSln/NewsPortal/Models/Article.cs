using System.ComponentModel.DataAnnotations;

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

        [Required]
        [Display(Name = "Category")]
        public long CategoryId { get; set; }

        public Category? Category { get; set; }

        [MaxLength(300)]
        public string? CoverImageUrl { get; set; }
    }
}
