using Microsoft.AspNetCore.Http;


namespace NewsPortal.Infrastructure
{
    public static class RecentArticlesCookie
    {
        private const string Key = "recent_articles";


        public static List<int> Get(HttpContext ctx)
        {
            if (ctx.Request.Cookies.TryGetValue(Key, out var raw) && !string.IsNullOrWhiteSpace(raw))
            {
                return raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .ToList();
            }
            return new List<int>();
        }


        public static void Add(HttpContext ctx, int id, int max = 5)
        {
            var ids = Get(ctx);
            ids.Remove(id);
            ids.Insert(0, id);
            if (ids.Count > max) ids = ids.Take(max).ToList();


            ctx.Response.Cookies.Append(Key, string.Join(',', ids), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                IsEssential = true,
                HttpOnly = false
            });
        }
    }
}