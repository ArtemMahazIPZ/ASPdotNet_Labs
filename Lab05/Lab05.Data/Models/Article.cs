using System;
using System.ComponentModel.DataAnnotations;

namespace Lab05.Data.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required, MaxLength(160)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? Author { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        public string? CoverImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
