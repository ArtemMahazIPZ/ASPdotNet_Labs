using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using NewsPortal.Models.ViewModels;
using System.Linq;

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
                query = query.Where(a => a.Category != null && a.Category.Name == category)
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

        public IActionResult Create()
        {
            var vm = BuildArticleFormViewModel(new Article
            {
                PublishedAt = DateTime.UtcNow
            });
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Article article)
        {
            if (!ModelState.IsValid)
            {
                return View(BuildArticleFormViewModel(article));
            }

            _repo.AddArticle(article);
            return RedirectToAction(nameof(Details), new { id = article.ArticleID });
        }

        public IActionResult Edit(long id)
        {
            var article = _repo.Articles.FirstOrDefault(a => a.ArticleID == id);
            if (article == null) return NotFound();

            return View(BuildArticleFormViewModel(article));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, Article article)
        {
            if (id != article.ArticleID) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(BuildArticleFormViewModel(article));
            }

            _repo.UpdateArticle(article);
            return RedirectToAction(nameof(Details), new { id = article.ArticleID });
        }

        public IActionResult Delete(long id)
        {
            var article = _repo.Articles.FirstOrDefault(a => a.ArticleID == id);
            if (article == null) return NotFound();

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var article = _repo.Articles.FirstOrDefault(a => a.ArticleID == id);
            if (article == null) return NotFound();

            _repo.DeleteArticle(article);
            return RedirectToAction(nameof(Index));
        }

        private ArticleFormViewModel BuildArticleFormViewModel(Article article)
        {
            return new ArticleFormViewModel
            {
                Article = article,
                Categories = _repo.Categories.OrderBy(c => c.Name).ToList()
            };
        }
    }
}
