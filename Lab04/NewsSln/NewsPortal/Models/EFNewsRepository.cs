using Microsoft.EntityFrameworkCore;

namespace NewsPortal.Models
{
    public class EFNewsRepository : INewsRepository
    {
        private readonly NewsDbContext _context;
        public EFNewsRepository(NewsDbContext ctx) => _context = ctx;

        public IQueryable<Article> Articles => _context.Articles.Include(a => a.Category);
        public IQueryable<Comment> Comments => _context.Comments;
        public IQueryable<Favorite> Favorites => _context.Favorites;
        public IQueryable<Category> Categories => _context.Categories;

        public void AddArticle(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
        }

        public void UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
            _context.SaveChanges();
        }

        public void DeleteArticle(Article article)
        {
            _context.Articles.Remove(article);
            _context.SaveChanges();
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
