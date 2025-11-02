using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;   
using System.Linq;                     
using NewsPortal.Data;
using NewsPortal.Infrastructure;
using NewsPortal.Models;


namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsDbContext _context;
        public HomeController(NewsDbContext context) => _context = context;


        public async Task<IActionResult> Index(int? categoryId)
        {
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();


            var query = _context.Articles
            .Include(a => a.Category)
            .OrderByDescending(a => a.PublishedAt)
            .AsQueryable();
            if (categoryId.HasValue) query = query.Where(a => a.CategoryId == categoryId);


            var articles = await query
            .Select(a => new ArticleCardVM
            {
                Id = a.Id,
                Title = a.Title,
                Author = a.Author,
                PublishedAt = a.PublishedAt,
                CategoryName = a.Category!.Name,
                Excerpt = a.Content.Length > 180 ? a.Content.Substring(0, 180) + "…" : a.Content
            })
            .ToListAsync();


            var recentIds = RecentArticlesCookie.Get(HttpContext);
            var recentRaw = await _context.Articles
            .Where(a => recentIds.Contains(a.Id))
            .Select(a => new { a.Id, a.Title })
            .ToListAsync();
            var recent = recentIds
            .Select(id => recentRaw.FirstOrDefault(r => r.Id == id))
            .Where(r => r != null)
            .Select(r => new ArticleMini { Id = r!.Id, Title = r.Title })
            .ToList();


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


    public class ArticleCardVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Author { get; set; }
        public DateTime PublishedAt { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
    }


    public class ArticleMini { public int Id { get; set; } public string Title { get; set; } = string.Empty; }


    public class HomeViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
        public int? ActiveCategoryId { get; set; }
        public IEnumerable<ArticleCardVM> Articles { get; set; } = Enumerable.Empty<ArticleCardVM>();
        public IEnumerable<ArticleMini> RecentlyViewed { get; set; } = Enumerable.Empty<ArticleMini>();
    }
}