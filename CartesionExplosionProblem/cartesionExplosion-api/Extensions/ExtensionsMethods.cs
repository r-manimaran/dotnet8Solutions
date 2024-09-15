using cartesionExplosion_api.Database;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace cartesionExplosion_api.Extensions
{
    public static class ExtensionsMethods
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContexts = scope.ServiceProvider.GetServices<AppDbContext>();
                foreach (var context in dbContexts) {
                    context.Database.Migrate();   
                }
            }
                
        }

        public static async Task SeedDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContexts = scope.ServiceProvider.GetServices<AppDbContext>();
                foreach (var context in dbContexts)
                {
                    await context.SeedDB();
                }
            }

        }
    }
}
