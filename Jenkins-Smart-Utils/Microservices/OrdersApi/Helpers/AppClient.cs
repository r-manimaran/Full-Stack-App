using SharedLib;

namespace OrdersApi.Helpers;

public sealed class AppClient
{
    private readonly HttpClient _httpClient;

    public AppClient(HttpClient client)
    {
        _httpClient = client;
    }

    public async Task<Product?> GetAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<Product>($"api/products/{id}");
        return response;
    }
}
