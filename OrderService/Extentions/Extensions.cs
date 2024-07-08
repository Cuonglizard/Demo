using MediatR;
using MicroRabbit.Domain.Core.Bus;
using Ordering.Domain.CommandHandlers;
using Orders.Application.Commands;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddSingleton<MicroRabbit.Domain.Core.Bus.IEventBus, MicroRabbit.Infra.Bus.RabbitMQBus>();
        services.AddTransient<IRequestHandler<ORDERCommand, bool>, ORDERCommandHandler>();
        services.AddMediatR(typeof(ORDERCommandHandler));

    }

    private static void AddEventBusSubscriptions(IEventBus eventBus)
    {

    }
}
