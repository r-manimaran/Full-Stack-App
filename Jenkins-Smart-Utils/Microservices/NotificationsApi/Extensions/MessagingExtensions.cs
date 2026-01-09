using MassTransit;
using SharedLib;

namespace NotificationsApi.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddOrderEventConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        var messagingProvider = configuration.GetValue<string>("Messaging:Provider")?.ToLowerInvariant();
        return messagingProvider switch
        {
           // "wolverine" => AddWolerineEventPublisher(services, configuration),
            "masstransit" => AddMassTransitEventPublisher(services, configuration),//services.AddSingleton<IOrderEventPublisher, Publishers.MassTransitOrderPublisher>(),
         //   "rabbitmq" => services.AddSingleton<IOrderEventPublisher, Publishers.RabbitMqOrderPublisher>(),
            _ => throw new InvalidOperationException("Unsupported messaging provider. Please configure 'MessagingProvider' in appsettings.json to 'Wolverine', 'MassTransit', or 'RabbitMQ'.")
        };
    }

    private static IServiceCollection AddMassTransitEventPublisher(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderNotificationsConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(new Uri(configuration["Messaging:RabbitMq:Host"]), "/", h =>
                {
                    h.Username(configuration["Messaging:RabbitMq:UserName"]);
                    h.Password(configuration["Messaging:RabbitMq:Password"]);
                });
                cfg.ReceiveEndpoint(configuration["Messaging:RabbitMq:QueueName"], e =>
                {
                    e.ConfigureConsumer<OrderNotificationsConsumer>(ctx);
                });
            });
        });
        
        return services;
    }
}
