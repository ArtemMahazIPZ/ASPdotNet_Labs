using System.ComponentModel.DataAnnotations;


namespace NewsPortal.Models
{
    public class Category
    {
        public int Id { get; set; }


        [Required, StringLength(64)]
        public string Name { get; set; } = string.Empty;


        [StringLength(256)]
        public string? Description { get; set; }


        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}