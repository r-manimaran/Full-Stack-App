using ProductApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//builder.Services.AddServiceDiscoveryConfig(builder.Configuration, "v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
    "/openapi/v1.json", "OpenAPI v1");
});

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGet("/api/products", () =>
{
  
    return new[] { new
        {
            Id=1,
            Name = "Product 1",
            Price = 9.99
        },
        new
        {
            Id=2,
            Name = "Product 2",
            Price = 19.99
        }
    };
})
.WithName("GetProducts");

app.MapGet("/api/products/{id}", (int id) =>
{
    var products = new[]
    {
        new
        {
            Id=1,
            Name = "Product 1",
            Price = 9.99
        },
        new
        {
            Id=2,
            Name = "Product 2",
            Price = 19.99
        }
    };
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.Run();

