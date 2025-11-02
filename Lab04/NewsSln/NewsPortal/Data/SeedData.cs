using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Models;


namespace NewsPortal.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<NewsDbContext>();


            ctx.Database.Migrate();


            if (!ctx.Categories.Any())
            {
                var tech = new Category { Name = "Technology", Description = "Gadgets, software, startups" };
                var world = new Category { Name = "World", Description = "Global news" };
                var sport = new Category { Name = "Sports" };


                ctx.Categories.AddRange(tech, world, sport);
                ctx.SaveChanges();


                ctx.Articles.Add(new Article
                {
                    Title = "Welcome to NewsPortal",
                    Content = "This is a seeded article to verify your setup.",
                    Author = "System",
                    PublishedAt = DateTime.UtcNow,
                    CategoryId = tech.Id,
                    CoverImageUrl = null
                });
                ctx.SaveChanges();
            }
        }
    }
}