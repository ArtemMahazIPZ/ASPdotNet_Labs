using System.Text.Json;

namespace Lab05.Web.Infrastructure
{
    public static class RecentArticlesCookie
    {
        private const string CookieName = "recent_articles";
        private const int MaxItems = 5;

        public static List<int> Get(HttpContext ctx)
        {
            var json = ctx.Request.Cookies[CookieName];
            if (string.IsNullOrWhiteSpace(json))
                return new List<int>();              

            try
            {
                var ids = JsonSerializer.Deserialize<List<int>>(json);
                return ids ?? new List<int>();        
            }
            catch
            {
                return new List<int>();               
            }
        }

        public static void Add(HttpContext ctx, int articleId)
        {
            var ids = Get(ctx);

            ids.Remove(articleId);
            ids.Insert(0, articleId);

            if (ids.Count > MaxItems)
                ids = ids.Take(MaxItems).ToList();

            var json = JsonSerializer.Serialize(ids);

            ctx.Response.Cookies.Append(
                CookieName,
                json,
                new CookieOptions
                {
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
        }
    }
}
