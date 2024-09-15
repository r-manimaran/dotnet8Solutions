using cartesionExplosion_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace cartesionExplosion_api.Database
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }


        public DbSet<Employee> employees { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Team> teams { get; set; }
        public DbSet<SalaryPayment> salaryPayments { get; set; }

        public DbSet<Entities.Task> tasks { get; set; }

    }
}
