using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsPortal.Data;
using NewsPortal.Infrastructure;   
using NewsPortal.Models;

namespace NewsPortal.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly NewsDbContext _context;
        public ArticlesController(NewsDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                .Include(a => a.Category)
                .OrderByDescending(a => a.PublishedAt)
                .ToListAsync();
            return View(articles);
        }

        [HttpGet("/news/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null) return NotFound();

            RecentArticlesCookie.Add(HttpContext, article.Id);

            return View(article);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(
                _context.Categories.OrderBy(c => c.Name), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Author,CoverImageUrl,CategoryId")] Article article)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", article.CategoryId);
                return View(article);
            }

            article.PublishedAt = DateTime.UtcNow;

            _context.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(
                _context.Categories.OrderBy(c => c.Name), "Id", "Name", article.CategoryId);
            return View(article);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Author,CoverImageUrl,CategoryId")] Article article)
        {
            if (id != article.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", article.CategoryId);
                return View(article);
            }

            var original = await _context.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            if (original is null) return NotFound();
            article.PublishedAt = original.PublishedAt;

            try
            {
                _context.Update(article);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Articles.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
