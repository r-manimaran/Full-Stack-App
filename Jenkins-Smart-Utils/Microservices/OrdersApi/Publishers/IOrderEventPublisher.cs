using SharedLib;

namespace OrdersApi.Publishers;

public interface IOrderEventPublisher
{
    Task PublishAsync(OrderCreated orderCreated);
}
