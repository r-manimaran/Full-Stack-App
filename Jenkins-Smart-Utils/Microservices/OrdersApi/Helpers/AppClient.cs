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
        var catalogServices = await _consul.Health.Service("products-api", string.Empty, true);
        var service = catalogServices.Response.FirstOrDefault();
        if (service == null)
        {
            _logger.LogError("Products API service not found in Consul.");
            throw new Exception("Products API service not found in Consul.");           
        }
        // Use ServiceAddress if available, otherwise fall back to Node Address
    var serviceAddress = !string.IsNullOrEmpty(service.Service.Address) 
        ? service.Service.Address 
        : service.Node.Address;
        var port = service.Service.Port;

     _logger.LogInformation("Products API resolved to: {Address}:{Port}", serviceAddress, port);
        // If we still get a hostname, try to resolve it or use a fallback
    if (serviceAddress == "products-api")
    {
        _logger.LogWarning("Service address is hostname, trying alternative resolution...");
        
        // Try to get all nodes and find the one with the service
        var nodes = await _consul.Catalog.Nodes();
        foreach (var node in nodes.Response)
        {
            var nodeServices = await _consul.Catalog.Node(node.Name);
            var targetService = nodeServices.Response?.Services?.Values
                .FirstOrDefault(s => s.Service == "products-api");
            
            if (targetService != null)
            {
                serviceAddress = node.Address;
                _logger.LogInformation("Found service on node {NodeName} with IP {NodeAddress}", 
                    node.Name, serviceAddress);
                break;
            }
        }
    }

    var baseUrl = $"http://{serviceAddress}:{port}";
    var response = await _httpClient.GetFromJsonAsync<Product>($"{baseUrl}/api/products/{id}");
    return response;
        
    }
}
