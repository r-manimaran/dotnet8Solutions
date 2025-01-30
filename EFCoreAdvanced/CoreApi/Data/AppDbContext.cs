using CoreApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CoreApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    // Approach 1: Auto-implemented property
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    // Approach 2: Expression-bodied property
    // public DbSet<User> Users => Set<User>();
    // public DbSet<Order> Orders => Set<Order>();

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
       PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Owned Type configuration
        modelBuilder.Entity<User>().OwnsOne(u => u.Address);

        // Global query Filter
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);

        // Value Conversion
        modelBuilder.Entity<User>()
                    .Property(u => u.Metadata)
                    .HasConversion(
                       v => v == null ? null : JsonSerializer.Serialize(v, _jsonSerializerOptions),
                       v => v == null ? null : JsonSerializer.Deserialize<UserMetadata>(v, _jsonSerializerOptions));
                       //.HasColumnType("json"); // For Postgres use jsonb

      
        modelBuilder.Entity<User>(b =>
        {
            b.Property(u => u.EmailAddress)
                .HasConversion(v => v.Value, v => new Email(v));
        });
        
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
       // Global Type Conversion
       configurationBuilder.Properties<DateTime>()
                    .HaveConversion(typeof(UtcDateTimeConverter));
    }
}
