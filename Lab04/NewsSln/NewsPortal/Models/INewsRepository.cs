using System.Linq;

namespace NewsPortal.Models
{
    public interface INewsRepository
    {
        IQueryable<Article> Articles { get; }
        IQueryable<Comment> Comments { get; }
        IQueryable<Favorite> Favorites { get; }
        IQueryable<Category> Categories { get; }

        void AddArticle(Article article);
        void UpdateArticle(Article article);
        void DeleteArticle(Article article);

        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
