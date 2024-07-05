

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {

        //builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<CatalogContext>>();

        //builder.Services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

        //builder.AddRabbitMqEventBus("eventbus")
        //       .AddSubscription<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>()
        //       .AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();

    }
}
