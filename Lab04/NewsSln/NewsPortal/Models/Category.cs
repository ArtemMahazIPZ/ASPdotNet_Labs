using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models
{
    public class Category
    {
        public long CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Description { get; set; }

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
