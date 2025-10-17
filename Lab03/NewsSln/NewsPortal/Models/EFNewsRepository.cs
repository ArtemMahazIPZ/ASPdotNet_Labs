namespace NewsPortal.Models
{
    public class EFNewsRepository : INewsRepository
    {
        private readonly NewsDbContext _context;
        public EFNewsRepository(NewsDbContext ctx) => _context = ctx;

        public IQueryable<Article> Articles => _context.Articles;
        public IQueryable<Comment> Comments => _context.Comments;
        public IQueryable<Favorite> Favorites => _context.Favorites;
    }
}
