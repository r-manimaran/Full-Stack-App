using Consul;
using SharedLib;

namespace OrdersApi.Helpers;

public sealed class AppClient
{
    private readonly HttpClient _httpClient;
    private readonly IConsulClient _consul;

    public AppClient(HttpClient client, IConsulClient consul)
    {
        _httpClient = client;
        _consul = consul;
    }

    public async Task<Product?> GetAsync(int id)
    {
        // Get services from Consul
        var services = await _consul.Health.Service("products-api", string.Empty, true);
        var service = services.Response.FirstOrDefault();
        if (service  == null)
            throw new Exception("Products API service not found in Consul.");

        var baseUrl = $"http://{service.Service.Address}:{service.Service.Port}";
        var response = await _httpClient.GetFromJsonAsync<Product>($"{baseUrl}/api/products/{id}");
        return response;
    }
}
