using OrdersApi.Extensions;
using OrdersApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddServiceDiscoveryConfig(builder.Configuration, "v1");

var productServiceUrl = builder.Configuration.GetValue<string>("ServiceDiscoveryConfiguration:ProductsApiConfiguration:BaseUrl") ?? "http://product-service:8080";

builder.Services.AddHttpClient<AppClient>(client =>
{
    client.BaseAddress = new Uri(productServiceUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(options => {
    options.SwaggerEndpoint(
    "/openapi/v1.json", "OpenAPI v1");
});

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


app.MapGet("/product/{id}", async (int id, AppClient client) =>
{
    var response = await client.GetAsync(id);
    return response is not null ? Results.Ok(response) : Results.NotFound();
}).WithName("GetProductById");


app.Run();

