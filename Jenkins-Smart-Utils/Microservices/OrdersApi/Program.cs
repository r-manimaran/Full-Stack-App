using OrdersApi;
using OrdersApi.Extensions;
using OrdersApi.Helpers;
using OrdersApi.Publishers;
using Serilog;
using SharedLib;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddLogging();

builder.Services.AddOpenApi();

// Add after builder.Services.AddOpenApi();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);


builder.Services.AddServiceDiscoveryConfig(builder.Configuration, "v1");

//var productServiceUrl = builder.Configuration.GetValue<string>("ServiceDiscoveryConfiguration:ProductsApiConfiguration:BaseUrl") ?? "http://product-service:8080";

builder.Services.AddHttpClient<AppClient>();
//client =>
//{
//    client.BaseAddress = new Uri(productServiceUrl);
//});

builder.Services.AddOrderEventPublisher(builder.Configuration);
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(options => {
    options.SwaggerEndpoint(
    "/openapi/v1.json", "OpenAPI v1");
});

// app.UseHttpsRedirection();

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


app.MapPost("/orders", async (Order newOrder, IOrderEventPublisher publisher, ILogger<Program> logger) =>
{
    logger.LogInformation("Received order: {OrderId}, {ProductName}", newOrder.Id, newOrder.ProductName);

    try
    {
        var newOrderEvent = new OrderCreated(newOrder.Id, newOrder.ProductName, newOrder.Quantity, newOrder.TotalPrice, DateTime.UtcNow);

        logger.LogInformation("Publishing OrderCreated event for OrderId: {OrderId}", newOrder.Id);
        await publisher.PublishAsync(newOrderEvent);
        logger.LogInformation("Successfully published OrderCreated event for OrderId: {OrderId}", newOrder.Id);

        return Results.Created($"/orders/{newOrder.Id}", newOrder);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to publish OrderCreated event for OrderId: {OrderId}", newOrder.Id);
        return Results.Problem("Failed to process order");
    }
});



app.Run();

