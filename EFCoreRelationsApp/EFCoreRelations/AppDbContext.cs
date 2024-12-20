using EFCoreRelations.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelations;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    // 1 to 1 Relationship
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    // 1 to Many RelationShip
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    //Many to Many Relationship
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentCourse> StudentsCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One to One : User --> UserProfile
        modelBuilder.Entity<User>()
                    .HasOne(u=>u.Profile)
                    .WithOne(p=>p.User)
                    .HasForeignKey<UserProfile>(p=>p.UserId);

        // One to Many : Category <--> Products
        modelBuilder.Entity<Category>()
                    .HasMany(c=>c.Products)
                    .WithOne(p=>p.Category)
                    .HasForeignKey(p=>p.CategoryId);

        // Many-to-Many : Student <--> Course
        modelBuilder.Entity<StudentCourse>()
                    .HasKey(sc => new {sc.StudentId, sc.CourseId});

        modelBuilder.Entity<StudentCourse>()
                    .HasOne(sc => sc.Student)
                    .WithMany(s => s.StudentCourses)
                    .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c=>c.StudentCourses)
            .HasForeignKey(sc=>sc.CourseId);

        // Seed the data

    }

}
