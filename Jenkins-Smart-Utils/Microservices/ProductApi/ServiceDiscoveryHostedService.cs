using Consul;
using ProductApi.Configuration;

namespace ProductApi;

public class ServiceDiscoveryHostedService : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly ServiceDiscoveryConfiguration _serviceDiscoveryConfiguration;
    private string _registrationId = string.Empty;

    public ServiceDiscoveryHostedService(IConsulClient consulClient, ServiceDiscoveryConfiguration serviceDiscoveryConfiguration)
    {
        _consulClient = consulClient;
        _serviceDiscoveryConfiguration = serviceDiscoveryConfiguration;
        _registrationId = $"{_serviceDiscoveryConfiguration.ServiceConfiguration.ServiceName}-{Guid.NewGuid()}";
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RegisterServiceAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _consulClient.Agent.ServiceDeregister(_registrationId, cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    private async Task RegisterServiceAsync(CancellationToken cancellationToken)
    {
        var serviceCfg = _serviceDiscoveryConfiguration.ServiceConfiguration;

        var registration = new AgentServiceRegistration()
        {
            ID = _registrationId,
            Name = serviceCfg.ServiceName,
            Address = serviceCfg.Host,
            Port = serviceCfg.Port,
            //Tags = new[] { $"version={serviceCfg.Version}" },
            Tags = new[] { $"urlprefix-/{_serviceDiscoveryConfiguration.ServiceConfiguration.ServiceName} strip=/{_serviceDiscoveryConfiguration.ServiceConfiguration.ServiceName}" },
            Check = new AgentServiceCheck()
            {
                HTTP = $"{_serviceDiscoveryConfiguration.ServiceConfiguration.Scheme}://{_serviceDiscoveryConfiguration.ServiceConfiguration.Host}:{_serviceDiscoveryConfiguration.ServiceConfiguration.Port}/health",
                Interval = TimeSpan.FromSeconds(30),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
            }
        };
        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
    }


}
