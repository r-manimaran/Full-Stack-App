using Consul;
using SharedLib;

namespace OrdersApi.Helpers;

public sealed class AppClient
{
    private readonly HttpClient _httpClient;
    private readonly IConsulClient _consul;
    private readonly ILogger<AppClient> _logger;

    public AppClient(HttpClient client, IConsulClient consul, ILogger<AppClient> logger)
    {
        _httpClient = client;
        _consul = consul;
        _logger = logger;
    }

    public async Task<Product?> GetAsync(int id)
    {
        // Get all registered services and print its details for debugging
        var servicesList = await _consul.Agent.Services();
        foreach (var serviceEntry in servicesList.Response)
        {
            var consulService = serviceEntry.Value;
            _logger.LogInformation("Service ID: {ServiceID}, Name: {ServiceName}, Address: {ServiceAddress}, Port: {ServicePort}",
                consulService.ID, consulService.Service, consulService.Address, consulService.Port);
        }

        // Get services from Consul
        var services = await _consul.Health.Service("products-api", string.Empty, true);
        var service = services.Response.FirstOrDefault();
        if (service == null)
        {
            _logger.LogError("Products API service not found in Consul.");
            throw new Exception("Products API service not found in Consul.");           
        }
        _logger.LogInformation("Discovered Products API service at {ServiceAddress}:{ServicePort}", service.Service.Address, service.Service.Port);
        _logger.LogInformation("Temporary: Using hardcoded URL for testing instead of discovered service.");
        // var baseUrl = $"http://{service.Service.Address}:{service.Service.Port}";
        var baseUrl = "http://172.31.57.64:8080"; // Temporary hardcoded URL for testing
        var response = await _httpClient.GetFromJsonAsync<Product>($"{baseUrl}/api/products/{id}");
        return response;
    }
}
