using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace NewsPortal.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NewsDbContext>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Local" },
                    new Category { Name = "Business" },
                    new Category { Name = "Sports" }
                );
                context.SaveChanges();
            }

            if (!context.Articles.Any())
            {
                var categories = context.Categories.ToDictionary(c => c.Name);

                context.Articles.AddRange(
                    new Article
                    {
                        Title = "City Council Approves New Park",
                        Body = "The city council voted 8–3...",
                        Author = "Editor",
                        CategoryId = categories["Local"].CategoryId,
                        PublishedAt = DateTime.UtcNow.AddHours(-8)
                    },
                    new Article
                    {
                        Title = "Ukraine Tech Exports Rise 12%",
                        Body = "IT services continue to grow...",
                        Author = "Staff",
                        CategoryId = categories["Business"].CategoryId,
                        PublishedAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new Article
                    {
                        Title = "Championship Finals Set for Sunday",
                        Body = "After a dramatic semifinal...",
                        Author = "Sport Desk",
                        CategoryId = categories["Sports"].CategoryId,
                        PublishedAt = DateTime.UtcNow.AddHours(-20)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
