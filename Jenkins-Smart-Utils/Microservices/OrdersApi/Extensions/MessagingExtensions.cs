using MassTransit;
using OrdersApi.Publishers;
using SharedLib;
using Wolverine;
using Wolverine.RabbitMQ;

namespace OrdersApi.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddOrderEventPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        var messagingProvider = configuration.GetValue<string>("Messaging:Provider")?.ToLowerInvariant();
        return messagingProvider switch
        {
            "wolverine" => AddWolerineEventPublisher(services, configuration),
            "masstransit" => AddMassTransitEventPublisher(services,configuration),//services.AddSingleton<IOrderEventPublisher, Publishers.MassTransitOrderPublisher>(),
            "rabbitmq" => services.AddSingleton<IOrderEventPublisher, Publishers.RabbitMqOrderPublisher>(),
            _ => throw new InvalidOperationException("Unsupported messaging provider. Please configure 'MessagingProvider' in appsettings.json to 'Wolverine', 'MassTransit', or 'RabbitMQ'.")
        };
    }

    private static IServiceCollection AddMassTransitEventPublisher(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(new Uri(configuration["Messaging:RabbitMq:Host"]), "/", h =>
                {
                    h.Username(configuration["Messaging:RabbitMq:Username"]);
                    h.Password(configuration["Messaging:RabbitMq:Password"]);
                });
                cfg.Message<OrderCreated>(e => { e.SetEntityName(configuration["Messaging:RabbitMq:QueueName"]); });
                cfg.ConfigureEndpoints(ctx);
            });
        });
        services.AddScoped<IOrderEventPublisher, MassTransitOrderPublisher>();
        return services;
    }

    private static IServiceCollection AddWolerineEventPublisher(IServiceCollection services, IConfiguration config)
    {
        services.AddWolverine(opts =>
        {
            opts.UseRabbitMq(config["RabbitMq:Host"])
                .DeclareQueue(config["RabbitMq:QueueName"]);
        });
        services.AddScoped<IOrderEventPublisher, WolverineOrderPublisher>();
        return services;
    }
}
