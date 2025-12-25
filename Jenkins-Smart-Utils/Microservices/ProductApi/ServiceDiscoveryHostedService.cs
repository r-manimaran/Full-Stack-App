using Consul;
using ProductApi.Configuration;

namespace ProductApi;

public class ServiceDiscoveryHostedService : BackgroundService
{
    private readonly IConsulClient _consulClient;
    private readonly ServiceDiscoveryConfiguration _serviceDiscoveryConfiguration;
    private readonly ILogger<ServiceDiscoveryHostedService> _logger;
    private string _registrationId = string.Empty;

    public ServiceDiscoveryHostedService(IConsulClient consulClient, ServiceDiscoveryConfiguration serviceDiscoveryConfiguration, ILogger<ServiceDiscoveryHostedService> logger)
    {
        _consulClient = consulClient;
        _serviceDiscoveryConfiguration = serviceDiscoveryConfiguration;
        _logger = logger;
        _registrationId = $"{_serviceDiscoveryConfiguration.ServiceConfiguration.ServiceName}-{Guid.NewGuid()}";
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Registering service {ServiceName} with Consul using registration id {RegistrationId}",
            _serviceDiscoveryConfiguration.ServiceConfiguration.ServiceName, _registrationId);
        await RegisterServiceAsync(stoppingToken);

    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deregistering service {RegistrationId} from Consul...", _registrationId);
        await _consulClient.Agent.ServiceDeregister(_registrationId, cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    private async Task RegisterServiceAsync(CancellationToken cancellationToken)
    {
        var serviceCfg = _serviceDiscoveryConfiguration.ServiceConfiguration;
        _logger.LogInformation("Service configuration: Name={ServiceName}, Host={Host}, Port={Port}, Scheme={Scheme}, Version={Version}",
            serviceCfg.ServiceName, serviceCfg.Host, serviceCfg.Port, serviceCfg.Scheme, serviceCfg.Version);
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
        _logger.LogInformation("Registering service {ServiceName} ({RegistrationId}) at {Address}:{Port}", registration.Name, registration.ID, registration.Address, registration.Port);
        await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        _logger.LogInformation("Service {ServiceName} registered with Consul successfully.", serviceCfg.ServiceName);
    }


}
