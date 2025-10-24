using Microsoft.AspNetCore.Mvc;
using System.Linq;
using NewsPortal.Models;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly INewsRepository _repo;
        private readonly RecentlyViewedService _recent; 
        private const int PageSize = 5;

        public ArticlesController(INewsRepository repo, RecentlyViewedService recent)
        {
            _repo = repo;
            _recent = recent;
        }

        public IActionResult Index(int page = 1, string? category = null)
        {
            var query = _repo.Articles.OrderByDescending(a => a.PublishedAt);
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(a => a.Category == category)
                             .OrderByDescending(a => a.PublishedAt);
            }

            var total = query.Count();
            var items = query.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            var vm = new ArticleListViewModel
            {
                Articles = items,
                CurrentCategory = category,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = total
                }
            };

            return View(vm);
        }

        public IActionResult Details(long id)
        {
            var article = _repo.Articles.FirstOrDefault(a => a.ArticleID == id);
            if (article == null) return NotFound();
            _recent.Add(id);
            return View(article);
        }
    }
}
