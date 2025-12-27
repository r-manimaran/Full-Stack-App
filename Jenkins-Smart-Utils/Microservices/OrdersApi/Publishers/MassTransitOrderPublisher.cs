using MassTransit;
using SharedLib;

namespace OrdersApi.Publishers;

public class MassTransitOrderPublisher(IPublishEndpoint publishEndpoint, 
                                       ILogger<MassTransitOrderPublisher> logger) : IOrderEventPublisher
{
    public Task PublishAsync(OrderCreated order)
    {
        logger.LogInformation("Publishing OrderCreated event for OrderId: {OrderId}", order.OrderId);
        return publishEndpoint.Publish(order);
    }
}
