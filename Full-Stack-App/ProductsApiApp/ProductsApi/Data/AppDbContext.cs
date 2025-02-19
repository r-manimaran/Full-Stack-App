using Microsoft.EntityFrameworkCore;
using ProductsApi.Data.Configurations;
using ProductsApi.Models;

namespace ProductsApi.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductConfiguration());

        modelBuilder.HasDefaultSchema("public");
    }
}
