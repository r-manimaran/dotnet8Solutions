using System;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace Pagination.Infrastructure.Context;
using Pagination.Domain;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }

}
