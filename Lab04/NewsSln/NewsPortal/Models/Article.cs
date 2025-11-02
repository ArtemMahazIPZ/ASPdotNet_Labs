using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NewsPortal.Models
{
    public class Article
    {
        public int Id { get; set; }


        [Required, StringLength(160)]
        public string Title { get; set; } = string.Empty;


        [Required]
        public string Content { get; set; } = string.Empty;


        [StringLength(64)]
        public string? Author { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;


        [Url, Display(Name = "Cover Image URL")]
        public string? CoverImageUrl { get; set; }


        // FK
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}