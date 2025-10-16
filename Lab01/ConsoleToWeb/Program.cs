var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello from Artem Mahaz! I just converted a console application into a web application.");

app.Run();
