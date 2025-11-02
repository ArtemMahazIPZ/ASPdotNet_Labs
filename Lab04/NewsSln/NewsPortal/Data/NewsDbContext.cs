using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;


namespace NewsPortal.Data
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options) { }


        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Category> Categories => Set<Category>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>()
            .HasMany(c => c.Articles)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}