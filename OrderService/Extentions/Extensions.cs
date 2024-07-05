internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        

        //services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

        //builder.AddRabbitMqEventBus("eventbus")
        //       .AddEventBusSubscriptions();

        //services.AddHttpContextAccessor();

        //// Configure mediatR
        //services.AddMediatR(cfg =>
        //{
        //    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

        //    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        //    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        //    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        //});

        //// Register the command validators for the validator behavior (validators based on FluentValidation library)
        //services.AddSingleton<IValidator<CancelOrderCommand>, CancelOrderCommandValidator>();
        //services.AddSingleton<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
        //services.AddSingleton<IValidator<IdentifiedCommand<CreateOrderCommand, bool>>, IdentifiedCommandValidator>();
        //services.AddSingleton<IValidator<ShipOrderCommand>, ShipOrderCommandValidator>();

        //services.AddScoped<IOrderQueries, OrderQueries>();
        //services.AddScoped<IBuyerRepository, BuyerRepository>();
        //services.AddScoped<IOrderRepository, OrderRepository>();
        //services.AddScoped<IRequestManager, RequestManager>();
    }

    private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        //eventBus.AddSubscription<GracePeriodConfirmedIntegrationEvent, GracePeriodConfirmedIntegrationEventHandler>();
        //eventBus.AddSubscription<OrderStockConfirmedIntegrationEvent, OrderStockConfirmedIntegrationEventHandler>();
        //eventBus.AddSubscription<OrderStockRejectedIntegrationEvent, OrderStockRejectedIntegrationEventHandler>();
        //eventBus.AddSubscription<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();
        //eventBus.AddSubscription<OrderPaymentSucceededIntegrationEvent, OrderPaymentSucceededIntegrationEventHandler>();
    }
}
