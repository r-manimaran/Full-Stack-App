using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;

namespace ProductsApi.Extensions;

public static class AppExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope= app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}
