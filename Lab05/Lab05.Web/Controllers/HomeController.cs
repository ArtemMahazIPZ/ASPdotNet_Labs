using Lab05.Data;
using Lab05.Data.Models;
using Lab05.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab05.Web.Controllers
{
    public class ArticleMini
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class HomeViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
        public int? ActiveCategoryId { get; set; }
        public IEnumerable<Article> Articles { get; set; } = Enumerable.Empty<Article>();
        public IEnumerable<ArticleMini> RecentlyViewed { get; set; } = Enumerable.Empty<ArticleMini>();
    }

    public class HomeController : Controller
    {
        private readonly NewsDbContext _context;
        public HomeController(NewsDbContext context) => _context = context;

        public async Task<IActionResult> Index(int? categoryId)
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            var query = _context.Articles
                .Include(a => a.Category)
                .OrderByDescending(a => a.PublishedAt)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(a => a.CategoryId == categoryId);

            var articles = await query.ToListAsync();

            var recentIds = RecentArticlesCookie.Get(HttpContext);
            var recent = new List<ArticleMini>();

            if (recentIds.Any())
            {
                var raw = await _context.Articles
                    .Where(a => recentIds.Contains(a.Id))
                    .Select(a => new { a.Id, a.Title })
                    .ToListAsync();

                recent = recentIds
                    .Select(id => raw.FirstOrDefault(r => r.Id == id))
                    .Where(r => r != null)
                    .Select(r => new ArticleMini { Id = r!.Id, Title = r.Title })
                    .ToList();
            }

            var vm = new HomeViewModel
            {
                Categories = categories,
                ActiveCategoryId = categoryId,
                Articles = articles,
                RecentlyViewed = recent
            };

            return View(vm);
        }
    }
}
