using System;
using Exception_ProblemDetails.Models;
using Microsoft.EntityFrameworkCore;

namespace Exception_ProblemDetails;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoItem>().HasData(
            new ToDoItem { Id = 1, Title = "Learn EF Core", IsDone = true },
            new ToDoItem { Id = 2, Title = "Learn ASP.NET Core", IsDone = false },
            new ToDoItem { Id = 3, Title = "Learn Blazor", IsDone = false }
        );
    }
    public DbSet<ToDoItem> ToDoItems { get; set; }    
}

