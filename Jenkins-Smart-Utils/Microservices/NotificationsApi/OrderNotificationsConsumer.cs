
namespace NotificationsApi;

public class OrderNotificationsConsumer(ILogger<OrderNotificationsConsumer> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

}
