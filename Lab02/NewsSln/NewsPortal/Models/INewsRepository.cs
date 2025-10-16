using System.Linq;

namespace NewsPortal.Models
{
    public interface INewsRepository
    {
        IQueryable<Article> Articles { get; }
        IQueryable<Comment> Comments { get; }
        IQueryable<Favorite> Favorites { get; }
    }
}
