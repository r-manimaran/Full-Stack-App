namespace ProductApi.Configuration;

public class ConsulConfig
{
    public string Scheme { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
}

public class ServiceConfig
{
    public string Host { get; set; }
    public string ServiceName { get; set; }
    public int Port { get; set; }
    public string Scheme { get; set; }
    public string Version { get; set; }
}

public class ServiceDiscoveryConfiguration
{
    public ConsulConfig ConsulConfiguration { get; set; }
    public ServiceConfig ServiceConfiguration { get; set; }

}