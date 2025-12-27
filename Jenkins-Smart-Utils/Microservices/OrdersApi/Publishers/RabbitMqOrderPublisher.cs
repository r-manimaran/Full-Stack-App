using SharedLib;

namespace OrdersApi.Publishers;

public class RabbitMqOrderPublisher(IConfiguration configuration, ILogger<RabbitMqOrderPublisher> logger) : IOrderEventPublisher
{
    public Task PublishAsync(OrderCreated orderCreated)
    {
        throw new NotImplementedException();
    }
}
