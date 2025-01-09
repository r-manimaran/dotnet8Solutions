using BlogPostApi.Data.Configurations;
using BlogPostApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApi.Data;

public class PostDbContext : DbContext
{
    public PostDbContext(DbContextOptions<PostDbContext> dbContextOptions):base(dbContextOptions)
    {
        
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}
