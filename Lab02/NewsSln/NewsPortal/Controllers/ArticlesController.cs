using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly INewsRepository _repo;
        private const int PageSize = 5;

        public ArticlesController(INewsRepository repo) => _repo = repo;

        public IActionResult Index(int page = 1, string? category = null)
        {
            var query = _repo.Articles.OrderByDescending(a => a.PublishedAt);
            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(a => a.Category == category)
                             .OrderByDescending(a => a.PublishedAt);

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
    }
}
