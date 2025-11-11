using Lab05.Data;
using Lab05.Data.Models;
using Lab05.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab05.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticlesController : Controller
    {
        private readonly NewsDbContext _db;
        public ArticlesController(NewsDbContext db) => _db = db;

        // список для адміна
        public async Task<IActionResult> Index()
        {
            var items = await _db.Articles.Include(a => a.Category)
                .OrderByDescending(a => a.PublishedAt).ToListAsync();
            return View(items);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var article = await _db.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null) return NotFound();

            RecentArticlesCookie.Add(HttpContext, id);

            return View(article);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories.OrderBy(c => c.Name), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Author,CoverImageUrl,CategoryId")] Article article)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name", article.CategoryId);
                return View(article);
            }

            article.PublishedAt = DateTime.UtcNow;
            _db.Add(article);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var article = await _db.Articles.FindAsync(id);
            if (article == null) return NotFound();
            ViewData["CategoryId"] = new SelectList(_db.Categories.OrderBy(c => c.Name), "Id", "Name", article.CategoryId);
            return View(article);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Author,CoverImageUrl,CategoryId")] Article article)
        {
            if (id != article.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_db.Categories, "Id", "Name", article.CategoryId);
                return View(article);
            }

            var original = await _db.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (original == null) return NotFound();
            article.PublishedAt = original.PublishedAt;

            _db.Update(article);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var article = await _db.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _db.Articles.FindAsync(id);
            if (article != null)
            {
                _db.Articles.Remove(article);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
