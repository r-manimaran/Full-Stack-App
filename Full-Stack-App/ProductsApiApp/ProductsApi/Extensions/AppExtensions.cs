using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using Bogus;

namespace ProductsApi.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Check if there are any pending Migrations
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if(pendingMigrations.Any())
        {
            // Apply the pending Migrations
            await dbContext.Database.MigrateAsync();
        }       
    }

    public static void SeedDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Insert 5 Products using Faker.

        }
    }
}
