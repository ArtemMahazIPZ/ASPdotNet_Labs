namespace MiddlewareSandbox.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _serverApiKey;
    private const string HeaderName = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration cfg)
    {
        _next = next;
        _serverApiKey = cfg["Security:ApiKey"] ?? string.Empty;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";
        if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(HeaderName, out var supplied) ||
            string.IsNullOrWhiteSpace(_serverApiKey) ||
            !string.Equals(supplied.ToString(), _serverApiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden: invalid or missing X-API-KEY.");
            return;
        }

        await _next(context);
    }
}

public static class ApiKeyMiddlewareExtensions
{
    public static IApplicationBuilder UseApiKeyGuard(this IApplicationBuilder app) =>
        app.UseMiddleware<ApiKeyMiddleware>();
}
