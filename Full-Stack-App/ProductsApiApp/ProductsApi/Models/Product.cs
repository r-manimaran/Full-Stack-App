namespace ProductsApi.Models;

public sealed record Product
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; }
    public required ProductDetails Details { get; init; }
    public required ProductPricing Pricing { get; init; }
    public required ProductInventory Inventory { get; init; }
    public required ProductSpecifications Specifications { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime? UpdatedOn { get; init; }
}

public sealed record ProductDetails
{
    public required string Brand { get; init; }
    public required string Category { get; init; }
    public required string SubCategory { get;init; }
    public required string Manufacturer { get; init; }
    public required string CountryOfOrigin { get; init; }
    public required List<string> Tags { get; init; }

}

public sealed record ProductPricing
{
    public decimal BasePrice { get; init; }
    public decimal? DiscountedPrice { get; init; }
    public string Currency { get; init; } = "USD";
    public bool IsOnSale { get; init; }
    public DateTime? SaleEndAt { get; init; }

}

public sealed record ProductInventory
{
    public int StockQuanitity { get; init; }
    public required string Sku { get;init; }
    public string? WarehouseLocation { get; init; }
    public bool IsInStock => StockQuanitity > 0;
    public int ReorderPoint { get;init; }
    public bool NeedsReorder => StockQuanitity <=ReorderPoint;
}

public sealed record ProductSpecifications
{
    public required Dictionary<string, string> Dimensions { get; init; }
    public decimal WeightInKg { get; init; }
    public required List<string> Materials { get;init; }
    public required Dictionary<string,string> TechnicalSpecs { get; init; }
}
