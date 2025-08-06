using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Endpoints;

public static class ProductsEndpoint
{

    public static void MapProductEndpoints(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/api/products").WithTags("Products").WithOpenApi().WithDescription("Product management endpoints");

        app.MapGet("/", async (AppDbContext dbContext) =>
        {
            var products = await dbContext.Products.ToListAsync();
            return Results.Ok(products);
        })
        .WithName("GetProducts")
        .WithTags("Products");

        app.MapGet("/{id:guid}", async (Guid id, AppDbContext dbContext) =>
        {
            var product = await dbContext.Products.FindAsync(id);

            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithTags("Products");
    }
}