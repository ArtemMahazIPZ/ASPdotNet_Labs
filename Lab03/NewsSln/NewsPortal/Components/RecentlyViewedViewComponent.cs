using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;

namespace NewsPortal.Components
{
    public class RecentlyViewedViewComponent : ViewComponent
    {
        private readonly INewsRepository _repo;
        private readonly RecentlyViewedService _recent;

        public RecentlyViewedViewComponent(INewsRepository repo, RecentlyViewedService recent)
        {
            _repo = repo;
            _recent = recent;
        }

        public IViewComponentResult Invoke()
        {
            var ids = _recent.Get();
            var items = _repo.Articles
                             .Where(a => ids.Contains(a.ArticleID ?? -1))
                             .ToList();

            var ordered = ids.Select(id => items.FirstOrDefault(a => a.ArticleID == id))
                             .Where(a => a != null)!;

            return View(ordered);
        }
    }
}
