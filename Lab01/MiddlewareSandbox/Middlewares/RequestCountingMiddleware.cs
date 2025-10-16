using MiddlewareSandbox.Services;

namespace MiddlewareSandbox.Middlewares;

public class RequestCountingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCountingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, RequestCounter counter)
    {
        var current = counter.Increment();

        if (context.Request.Path.Equals("/count", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync($"The amount of processed requests is {current}.");
            return; 
        }

        await _next(context);
    }
}

public static class RequestCountingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestCounter(this IApplicationBuilder app) =>
        app.UseMiddleware<RequestCountingMiddleware>();
}

