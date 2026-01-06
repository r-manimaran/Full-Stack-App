
using MassTransit;
using SharedLib;

namespace NotificationsApi;

public class OrderNotificationsConsumer(ILogger<OrderNotificationsConsumer> logger) : IConsumer<OrderCreated>
{
    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        var message = context.Message;
        logger.LogInformation("Received OrderCreated event for OrderId: {OrderId}, ProductName: {ProductName}, Count: {Count}, TotalPrice: {TotalPrice}, CreatedOn: {CreatedOn}",
            message.OrderId, message.ProductName, message.Count, message.TotalPrice, message.CreatedOn);
        // Here you can add logic to send email/SMS notifications
        return Task.CompletedTask;
    }
}
