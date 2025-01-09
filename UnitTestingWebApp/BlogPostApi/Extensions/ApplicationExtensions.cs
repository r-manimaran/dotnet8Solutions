using BlogPostApi.Data;

namespace BlogPostApi.Extensions;

public static class ApplicationExtensions
{
    public static async void ApplyMigrationAndSeedData(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<PostDbContext>())
            {
                await dbContext.Database.EnsureCreatedAsync();
                
            }
        }
    }
}
