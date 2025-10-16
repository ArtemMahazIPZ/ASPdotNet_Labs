namespace MiddlewareSandbox.Middlewares;

public class CustomQueryMiddleware
{
    private readonly RequestDelegate _next;

    public CustomQueryMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Query.ContainsKey("custom"))
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync("You've hit a custom middleware!");
            return; 
        }

        await _next(context);
    }
}

public static class CustomQueryMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomQueryShortCircuit(this IApplicationBuilder app) =>
        app.UseMiddleware<CustomQueryMiddleware>();
}
