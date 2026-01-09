using Bogus;
using ProductApi.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddLogging();

builder.Services.AddOpenApi();

builder.Services.AddServiceDiscoveryConfig(builder.Configuration, "v1");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
    "/openapi/v1.json", "OpenAPI v1");
});

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGet("/api/products", () =>
{

    var faker = new Faker();

    var products = Enumerable.Range(1, 10).Select(i => new
    {
        Id = i,
        Name = faker.Commerce.ProductName(),
        Price = Math.Round(faker.Random.Double(5.0, 99.99), 2)
    }).ToArray();
    
    return Results.Ok(products);
    
})
.WithName("GetProducts");

app.MapGet("/api/products/{id}", (int id) =>
{
    var faker = new Faker();
    
    if (id < 1 || id > 10) return Results.NotFound();
    
    var product = new
    {
        Id = id,
        Name = faker.Commerce.ProductName(),
        Price = Math.Round(faker.Random.Double(5.0, 99.99), 2)
    };
    
    return Results.Ok(product);
});

app.Run();



