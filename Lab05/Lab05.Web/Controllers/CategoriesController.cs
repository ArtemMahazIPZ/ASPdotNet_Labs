using Lab05.Data;
using Lab05.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab05.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly NewsDbContext _db;
        public CategoriesController(NewsDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var items = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(items);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            _db.Add(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var cat = await _db.Categories.Include(c => c.Articles).FirstOrDefaultAsync(c => c.Id == id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();
            if (!ModelState.IsValid) return View(category);

            _db.Update(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.Categories.Include(c => c.Articles).FirstOrDefaultAsync(c => c.Id == id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cat = await _db.Categories.Include(c => c.Articles).FirstOrDefaultAsync(c => c.Id == id);
            if (cat != null)
            {
                if (cat.Articles.Any())
                {
                    ModelState.AddModelError(string.Empty, "Спочатку приберіть новини з цієї категорії.");
                    return View(cat);
                }
                _db.Categories.Remove(cat);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
