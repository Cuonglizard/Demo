using Grpc.Core;
using Common.Protobuf;

using CustomerOrder = Orders.Models.CustomerOrder;

using OrderService.Models;
using static Common.Protobuf.Payments;

namespace OrderService.Services;
    public interface IGrpcOrdersService
    {
    Task<Order> CreateOrderAsync(CustomerOrder customerOrder);
    }
    public class GrpcOrderService : IGrpcOrdersService
    {
        private readonly ILogger<GrpcOrderService> _logger;
        private readonly Payments.PaymentsClient _paymentClient;

    public GrpcOrderService(ILogger<GrpcOrderService> logger, Payments.PaymentsClient paymentClient)
    {
            _logger = logger;
           _paymentClient = paymentClient;

    }
    public async Task<Order> CreateOrderAsync(CustomerOrder customerOrder)
        {
            var order = new Order
            {
                Item = customerOrder.Item,
                Amount = customerOrder.Amount,
                Quantity = customerOrder.Quantity,
                Status = "Created",
            };

            try
            {

                customerOrder.OrderId = order.Id;

                var orderRequest = new NewOrderRequest
                {
                    Order = new Common.Protobuf.CustomerOrder
                    {
                        Address = customerOrder.Address,
                        Amount = customerOrder.Amount,
                        Item = customerOrder.Item,
                        OrderId = order.Id,
                        PaymentMethod = customerOrder.PaymentMethod,
                        Quantity = customerOrder.Quantity,
                    },
                };

                var result = await _paymentClient.NewOrderAsync(orderRequest);
                if (!result.Success)
                {
                    _logger.LogError("Error when creating paymeny: {} -> {}", customerOrder);

                    order.Status = "Failed";

                    throw new Exception("bug");
                }
            _logger.LogInformation("Order created {}", order);

            return order;
            }
            catch (Exception)
            {
                _logger.LogError("Order not created {}", order);
                throw;
            }
        }

    }
