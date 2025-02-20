using Bogus;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProductsApi.Data;
using ProductsApi.Extensions;
using ProductsApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    dataSourceBuilder.EnableDynamicJson(); // Enable JSON serialization

    options.UseNpgsql(dataSourceBuilder.Build());
    options.UseAsyncSeeding(async (context, _, token) =>
    {
        // Create faker data for ProductDetails
        var productDetailsFaker = new Faker<ProductDetails>()
            .RuleFor(pd => pd.Brand, f => f.Company.CompanyName())
            .RuleFor(pd => pd.Category, f => f.Commerce.Department())
            .RuleFor(pd => pd.SubCategory, f => f.Commerce.Categories(1)[0])
            .RuleFor(pd => pd.Manufacturer, f => f.Company.CompanyName())
            .RuleFor(pd => pd.CountryOfOrigin, f => f.Address.County())
            .RuleFor(pd => pd.Tags, f => f.Commerce.Categories(3).ToList());

        // Create faker for ProductPricing
        var productPricingFaker = new Faker<ProductPricing>()
            .RuleFor(pp => pp.BasePrice, f => decimal.Parse(f.Commerce.Price()))
            .RuleFor(pp => pp.DiscountedPrice, (f, pp) => f.Random.Bool() ? pp.BasePrice * 0.8m : null)
            .RuleFor(pp => pp.Currency, "USD")
            .RuleFor(pp => pp.IsOnSale, f => f.Random.Bool())
            .RuleFor(pp => pp.SaleEndAt, (f, pp) => pp.IsOnSale ? f.Date.Future().ToUniversalTime() : null);

        //create faker for ProductInventory
        var productInventoryFaker = new Faker<ProductInventory>()
            .RuleFor(pi => pi.StockQuanitity, f => f.Random.Number(0, 1000))
            .RuleFor(pi => pi.Sku, f => f.Commerce.Ean13())
            .RuleFor(pi => pi.WarehouseLocation, f => $"Zone-{f.Random.AlphaNumeric(2)}-{f.Random.Number(1, 100)}")
            .RuleFor(pi => pi.ReorderPoint, f => f.Random.Number(10, 50));

        // create faker for ProductSpecifications
        var productSpecificationsFaker = new Faker<ProductSpecifications>()
            .RuleFor(ps => ps.Dimensions, f => new Dictionary<string, string>
            {
                { "Length",$"{f.Random.Number(10,100)} cm" },
                { "Width", $"{f.Random.Number(10, 100)} cm" },
                { "Height", $"{f.Random.Number(10, 100)} cm" },
             })
            .RuleFor(ps => ps.WeightInKg, f => f.Random.Decimal(0.1m, 50.0m))
            .RuleFor(ps => ps.Materials, f => new List<string>
            {
                f.Commerce.ProductMaterial(),
                f.Commerce.ProductMaterial()
            })
            .RuleFor(ps => ps.TechnicalSpecs, f => new Dictionary<string, string>
            {
                {"Color",f.Commerce.Color() },
                {"Material Type",f.Commerce.ProductMaterial()},
                {"Model Number", f.Random.AlphaNumeric(8) }
            });

        // create the main product
        var productsFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Details, f => productDetailsFaker.Generate())
            .RuleFor(p => p.Pricing, f => productPricingFaker.Generate())
            .RuleFor(p => p.Inventory, f => productInventoryFaker.Generate())
            .RuleFor(p => p.Specifications, f => productSpecificationsFaker.Generate())
            .RuleFor(p => p.CreatedOn, f => f.Date.Past().ToUniversalTime())
            .RuleFor(p => p.UpdatedOn, f => f.Random.Bool() ? f.Date.Recent().ToUniversalTime() : null);

        // Generate 5 fake products
        var products = productsFaker.Generate(5);

        // Add to context
        if (!await context.Set<Product>().AnyAsync())
        {
            await context.Set<Product>().AddRangeAsync(products, token);
            await context.SaveChangesAsync(token);
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJS", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000") // Your Next.js app URL
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
await app.ApplyMigrations();

app.UseSwaggerUI(options => {
    options.SwaggerEndpoint(
    "/openapi/v1.json", "OpenAPI v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
