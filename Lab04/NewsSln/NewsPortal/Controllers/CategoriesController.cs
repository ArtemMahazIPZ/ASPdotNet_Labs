using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using System.Linq;

namespace NewsPortal.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly INewsRepository _repo;

        public CategoriesController(INewsRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var categories = _repo.Categories
                .OrderBy(c => c.Name)
                .ToList();
            return View(categories);
        }

        public IActionResult Details(long id)
        {
            var category = _repo.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();

            category.Articles = _repo.Articles
                .Where(a => a.CategoryId == id)
                .OrderByDescending(a => a.PublishedAt)
                .ToList();

            return View(category);
        }

        public IActionResult Create() => View(new Category());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _repo.AddCategory(category);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(long id)
        {
            var category = _repo.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, Category category)
        {
            if (id != category.CategoryId) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _repo.UpdateCategory(category);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(long id)
        {
            var category = _repo.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();

            category.Articles = _repo.Articles
                .Where(a => a.CategoryId == id)
                .OrderByDescending(a => a.PublishedAt)
                .ToList();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var category = _repo.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();

            var hasArticles = _repo.Articles.Any(a => a.CategoryId == id);
            if (hasArticles)
            {
                ModelState.AddModelError(string.Empty, "Cannot delete a category that still contains articles.");
                category.Articles = _repo.Articles
                    .Where(a => a.CategoryId == id)
                    .OrderByDescending(a => a.PublishedAt)
                    .ToList();
                return View(category);
            }

            _repo.DeleteCategory(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
