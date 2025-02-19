using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsApi.Models;

namespace ProductsApi.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Configure the main Product entity
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.Description)
            .HasMaxLength(500);
        
        // Configure the ProductDetails as owned entity
        builder.OwnsOne(p=>p.Details, details=> 
        {
            details.Property(d=>d.Brand).IsRequired().HasMaxLength(100);
            details.Property(d=>d.Category).IsRequired().HasMaxLength(100);
            details.Property(d=>d.SubCategory).IsRequired().HasMaxLength(100);
            details.Property(d=>d.Manufacturer).IsRequired().HasMaxLength(200);
            details.Property(d=>d.CountryOfOrigin).IsRequired().HasMaxLength(100);
            details.Property(d=>d.Tags).HasColumnType("jsonb");
        });
        
        // Configure the ProductPricing as owned entity
        builder.OwnsOne(p=>p.Pricing, pricing=> 
        {
            pricing.Property(p=>p.BasePrice).IsRequired().HasColumnType("decimal(18,2)");
            pricing.Property(p=>p.DiscountedPrice).IsRequired().HasColumnType("decimal(18,2)");
            pricing.Property(p=>p.Currency).IsRequired().HasMaxLength(3);
        });

        // configure ProductInventory as owned entity
        builder.OwnsOne(p=>p.Inventory, inventory=>
        {
            inventory.Property(i=>i.Sku).IsRequired().HasMaxLength(20);
            inventory.Property(i=>i.StockQuanitity).IsRequired();
            inventory.Property(i=>i.WarehouseLocation).IsRequired().HasMaxLength(100);
            inventory.Property(i=>i.ReorderPoint).IsRequired();
        });

        // configure ProductSpecifications as owned entity
        builder.OwnsOne(p=>p.Specifications, specs=> 
        {
            specs.Property(s=>s.Dimensions).HasColumnType("jsonb");
            specs.Property(s=>s.WeightInKg).HasColumnType("decimal(10, 2)");
            specs.Property(s=>s.Materials).HasColumnType("jsonb");
            specs.Property(s=>s.TechnicalSpecs).HasColumnType("jsonb");
        });
    }
}
