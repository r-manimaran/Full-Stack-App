using SharedLib;
using Wolverine;

namespace OrdersApi.Publishers;

public class WolverineOrderPublisher(IMessageBus bus, ILogger<WolverineOrderPublisher> logger) : IOrderEventPublisher
{
    public async Task PublishAsync(OrderCreated order)
    {
        logger.LogInformation("Publishing OrderCreated event for OrderId: {OrderId}", order.OrderId);
        await bus.PublishAsync(order);
    }
}
