using OrdersApi.Extensions;
using OrdersApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHttpClient<AppClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
});

builder.Services.AddServiceDiscoveryConfig(builder.Configuration, "v1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapGet("/orders", () =>
{
    return new[] {
        new
        {
            Id = 1,
            ProductName = "Product 1",
            Quantity = 2,
            TotalPrice = 19.98
        },
        new
        {
            Id = 2,
            ProductName = "Product 2",
            Quantity = 1,
            TotalPrice = 19.99
        }
    };
}).WithName("GetOrders");


app.MapGet("/products/{id}", async (int id, AppClient client) =>
{
    var response = await client.GetAsync(id);
    return response is not null ? Results.Ok(response) : Results.NotFound();
}).WithName("GetProductById");


app.Run();

