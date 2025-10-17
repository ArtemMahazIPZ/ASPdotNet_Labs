namespace NewsPortal.Models.ViewModels
{
    public class ArticleListViewModel
    {
        public IEnumerable<NewsPortal.Models.Article> Articles { get; set; } = Enumerable.Empty<NewsPortal.Models.Article>();
        public PagingInfo PagingInfo { get; set; } = new();
        public string? CurrentCategory { get; set; }
    }
}
