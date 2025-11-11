using Lab05.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab05.Data
{
    // один контекст на все: і Identity, і наші сутності
    public class NewsDbContext : IdentityDbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }

        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Category> Categories => Set<Category>();
    }
}
