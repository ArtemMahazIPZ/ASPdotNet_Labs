namespace NewsPortal.Models
{
    public class RecentlyViewedService
    {
        private const string Key = "recently_viewed";
        private readonly IHttpContextAccessor _http;

        public RecentlyViewedService(IHttpContextAccessor http) => _http = http;

        public void Add(long id, int max = 5)
        {
            var session = _http.HttpContext!.Session;
            var list = session.GetJson<List<long>>(Key) ?? new List<long>();
            list.Remove(id);
            list.Insert(0, id);
            if (list.Count > max) list = list.Take(max).ToList();
            session.SetJson(Key, list);
        }

        public IReadOnlyList<long> Get() =>
            _http.HttpContext!.Session.GetJson<List<long>>(Key) ?? new List<long>();
    }
}
