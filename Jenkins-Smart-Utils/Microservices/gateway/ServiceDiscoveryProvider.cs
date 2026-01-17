namespace gateway;

public class ServiceDiscoveryProvider
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Type { get; set; }
    public string Scheme { get; set; }
    public int PollingInterval { get; set; }
    public string ConfigurationKey { get; set; }
}

public class LoadBalancerOptions
{
    public string Type { get; set; }
}

public class RateLimitOptions
{
    public bool EnableRateLimiting { get; set; }
}
