var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/who", () =>
{
    return "Artem Mahaz";
});


app.MapGet("/time", () =>
{
    return DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
});

app.Run();
