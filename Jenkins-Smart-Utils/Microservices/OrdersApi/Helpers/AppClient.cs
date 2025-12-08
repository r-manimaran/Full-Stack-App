using SharedLib;

namespace OrdersApi.Helpers;

public sealed class AppClient
{
    private readonly HttpClient _httpClient;

    public AppClient(HttpClient client)
    {
        _httpClient = client;
    }

    public async Task<Response?> GetAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<Response>($"api/Product/{id}");
        return response;
    }
}
