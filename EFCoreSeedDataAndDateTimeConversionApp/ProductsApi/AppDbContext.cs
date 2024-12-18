using Microsoft.EntityFrameworkCore;
using ProductsApi.Converters;
using ProductsApi.Models;
using System.Reflection;

namespace ProductsApi;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTime>()
        .HaveConversion<DateTimeUtcConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.Load("ProductsApi"));
        base.OnModelCreating(modelBuilder);
    }


}

