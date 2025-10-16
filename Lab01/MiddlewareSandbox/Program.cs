using MiddlewareSandbox.Middlewares;
using MiddlewareSandbox.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RequestCounter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRequestLogging();

app.UseCustomQueryShortCircuit();

app.UseApiKeyGuard();

app.UseRequestCounter();

app.MapGet("/", () => "OK: root reached");

app.Run();
