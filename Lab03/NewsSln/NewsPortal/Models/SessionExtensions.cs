using System.Text.Json;

namespace NewsPortal.Models
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value) =>
            session.SetString(key, JsonSerializer.Serialize(value));

        public static T? GetJson<T>(this ISession session, string key) =>
            session.GetString(key) is { } data
                ? JsonSerializer.Deserialize<T>(data)
                : default;
    }
}
