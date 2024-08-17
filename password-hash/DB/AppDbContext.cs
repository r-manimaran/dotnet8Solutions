using Microsoft.EntityFrameworkCore;

namespace password_hash;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }

}
