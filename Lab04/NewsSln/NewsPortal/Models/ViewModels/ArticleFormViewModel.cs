using System.Collections.Generic;
using System.Linq;

namespace NewsPortal.Models.ViewModels
{
    public class ArticleFormViewModel
    {
        public Article Article { get; set; } = new();
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
    }
}
