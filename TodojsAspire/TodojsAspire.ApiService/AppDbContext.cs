using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TodojsAspire.ApiService.Models.Todo> Todo { get; set; } = default!;
}
