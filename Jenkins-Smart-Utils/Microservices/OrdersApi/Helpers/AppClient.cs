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
        var catalogServices = await _consul.Catalog.Service("products-api");//, string.Empty, true);
        var service = catalogServices.Response.FirstOrDefault();
        if (service == null)
        {
            _logger.LogError("Products API service not found in Consul.");
            throw new Exception("Products API service not found in Consul.");           
        }
        // Get the node information to find the actual IP address
        var nodeInfo = await _consul.Catalog.Node(service.Node);
        var nodeAddress = nodeInfo.Response?.Node?.Address;
        if(string.IsNullOrEmpty(nodeAddress))
        {
            _logger.LogError("Node address not found for node {NodeName}.", service.Node);
            throw  new Exception($"Node address not found for node {service.Node}.");
        }

        _logger.LogInformation("Discovered Products API service at {ServiceAddress}:{ServicePort}", service.ServiceAddress, service.ServicePort);

        //var ipAddress = service.ServiceAddress ?? service.Address;
        var port = service.ServicePort;

        _logger.LogInformation("Products API IP Address: {IPAddress}, Port: {Port}", nodeAddress, port);
        // _logger.LogInformation("Temporary: Using hardcoded URL for testing instead of discovered service.");
        var baseUrl = $"http://{nodeAddress}:{port}";
        var response = await _httpClient.GetFromJsonAsync<Product>($"{baseUrl}/api/products/{id}");
        return response;
    }
}
