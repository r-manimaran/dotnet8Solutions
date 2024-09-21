using ApiHealthChecks.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace ApiHealthChecks
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //use the Bogus Faker to generate some data
            Faker faker = new Faker();
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = faker.Commerce.ProductName(),Description = faker.Commerce.ProductDescription(), Price = 9.99M, Quantity=50 },
                 new Product { Id = 2, Name = faker.Commerce.ProductName(), Description = faker.Commerce.ProductDescription(), Price = 6.99M, Quantity = 100 },
                  new Product { Id = 3, Name = faker.Commerce.ProductName(), Description = faker.Commerce.ProductDescription(), Price = 36.5M, Quantity = 20 },
                   new Product { Id = 4, Name = faker.Commerce.ProductName(), Description = faker.Commerce.ProductDescription(), Price = 180.50M, Quantity = 150 }

                
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
