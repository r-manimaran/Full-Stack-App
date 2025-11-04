using Microsoft.EntityFrameworkCore;
using GasTrackerApi.Models;

namespace GasTrackerApi.Data;

public class GasTrackerDbContext : DbContext
{
    public GasTrackerDbContext(DbContextOptions<GasTrackerDbContext> options)
        : base(options)
    {
    }

    public DbSet<GasPurchase> GasPurchases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GasPurchase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PricePerGallon)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.GallonsPurchased)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.FuelStation)
                .IsRequired()
                .HasMaxLength(200);
        });
    }
}
