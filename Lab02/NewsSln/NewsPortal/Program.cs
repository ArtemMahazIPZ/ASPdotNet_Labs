using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NewsDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:NewsPortalConnection"]));

builder.Services.AddScoped<INewsRepository, EFNewsRepository>();

var app = builder.Build();

app.UseStaticFiles();
app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app); 

app.Run();
