using Consul;
using ProductApi.Configuration;

namespace ProductApi.Extensions;

public static class ServiceDiscoveryExtensions
{
    public static IServiceCollection AddServiceDiscoveryConfig(this IServiceCollection services, IConfiguration configuration, string swaggerVersion)
    {
        var discoveryConfig = GetServiceDiscoveryConfig(configuration, swaggerVersion);
        services.AddSingleton(discoveryConfig);
        RegisterServiceDiscovery(services, discoveryConfig);
        // Example: Register service discovery related services here
        // services.AddSingleton<IServiceDiscovery, ConsulServiceDiscovery>();
        return services;
    }

    private static ServiceDiscoveryConfiguration GetServiceDiscoveryConfig(IConfiguration configuration, string swaggerVersion)
    {
        ServiceDiscoveryConfiguration serviceDiscoveryConfiguration = configuration.GetSection("ServiceDiscoveryConfiguration").Get<ServiceDiscoveryConfiguration>()
                ?? throw new InvalidOperationException("ServiceDiscoveryConfiguration section is missing in configuration.");
        serviceDiscoveryConfiguration.ServiceConfiguration.Version = swaggerVersion;
        return serviceDiscoveryConfiguration;
    }

    private static void RegisterServiceDiscovery(IServiceCollection services, ServiceDiscoveryConfiguration discoveryConfig)
    {
        services.AddHostedService<ServiceDiscoveryHostedService>();
        services.AddSingleton<IConsulClient, ConsulClient>(sp => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri($"{discoveryConfig.ConsulConfiguration.Scheme}://{discoveryConfig.ConsulConfiguration.Host}:{discoveryConfig.ConsulConfiguration.Port}");
        }));
    }
}

