using Payments.Application.EventHandlers;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using Payments.Application.Events;
using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddSingleton<MicroRabbit.Domain.Core.Bus.IEventBus, MicroRabbit.Infra.Bus.RabbitMQBus>();

        //Subscription
        services.AddTransient<ORDEREventHandler>();

        //DI for RabbitMQ

        services.AddTransient<IEventHandler<ORDEREvent>, ORDEREventHandler>();

        services.AddHttpClient();

        services.AddMediatR(typeof(Program));

    }

    private static void AddEventBusSubscriptions(IEventBus eventBus)
    {
        eventBus.Subscribe<ORDEREvent, ORDEREventHandler>();
    }
}
