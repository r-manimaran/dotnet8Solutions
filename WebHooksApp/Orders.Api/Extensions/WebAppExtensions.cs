using Microsoft.EntityFrameworkCore;
using Orders.Api.Data;

namespace Orders.Api.Extensions;

public static class WebAppExtensions
{
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db =scope.ServiceProvider.GetRequiredService<WebhookDbContext>();

        await db.Database.MigrateAsync();
    }
}
