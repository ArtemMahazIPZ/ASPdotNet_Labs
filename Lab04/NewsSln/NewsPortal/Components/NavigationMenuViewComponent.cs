using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;

namespace NewsPortal.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly INewsRepository _repo;
        public NavigationMenuViewComponent(INewsRepository repo) => _repo = repo;

        public IViewComponentResult Invoke(string? category = null)
        {
            category ??= HttpContext.Request.Query["category"].ToString();

            var categories = _repo.Articles
                .Where(a => !string.IsNullOrEmpty(a.Category))
                .Select(a => a.Category!)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            ViewBag.SelectedCategory = category;
            return View(categories);
        }
    }
}
