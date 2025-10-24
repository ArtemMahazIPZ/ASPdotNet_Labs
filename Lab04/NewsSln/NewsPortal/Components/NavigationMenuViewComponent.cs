using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using System.Linq;

namespace NewsPortal.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly INewsRepository _repo;
        public NavigationMenuViewComponent(INewsRepository repo) => _repo = repo;

        public IViewComponentResult Invoke(string? category = null)
        {
            category ??= HttpContext.Request.Query["category"].ToString();

            var categories = _repo.Categories
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.SelectedCategory = category;
            return View(categories);
        }
    }
}
