using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<RecentlyViewedService>();

builder.Services.AddDbContext<NewsDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:NewsPortalConnection"]));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".NewsPortal.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<INewsRepository, EFNewsRepository>();

var app = builder.Build();

app.UseStaticFiles();
app.UseSession(); 


app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);
app.Run();
