using Microsoft.EntityFrameworkCore;

namespace EF.Pagination.Benchmark;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        { 
           optionsBuilder.UseSqlServer("Server=DARSHAN-PC\\SQLEXPRESS;Database=Pagination;Trusted_Connection=True;TrustServerCertificate=True;");
        }       
    }
}
