using Microsoft.EntityFrameworkCore;

namespace TicTacToeServer.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> MigrateAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Count() != 0)
            await dbContext.Database.MigrateAsync();
        
        return app;
    }
}
