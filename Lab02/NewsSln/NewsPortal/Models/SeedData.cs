using Microsoft.EntityFrameworkCore;

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

            if (!context.Articles.Any())
            {
                context.Articles.AddRange(
                    new Article
                    {
                        Title = "City Council Approves New Park",
                        Body = "The city council voted 8–3...",
                        Author = "Editor",
                        Category = "Local",
                        PublishedAt = DateTime.UtcNow.AddHours(-8)
                    },
                    new Article
                    {
                        Title = "Ukraine Tech Exports Rise 12%",
                        Body = "IT services continue to grow...",
                        Author = "Staff",
                        Category = "Business",
                        PublishedAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new Article
                    {
                        Title = "Championship Finals Set for Sunday",
                        Body = "After a dramatic semifinal...",
                        Author = "Sport Desk",
                        Category = "Sports",
                        PublishedAt = DateTime.UtcNow.AddHours(-20)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
