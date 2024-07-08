using Grpc.Core;
using Common.Protobuf;
using Orders.Application.Models;
using CustomerOrder = Orders.Models.CustomerOrder;

using OrderService.Models;
using static Common.Protobuf.Payments;
using Orders.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Google.Api;
using Orders.Infrastructure;
using Orders.Infrastructure.Enities;

namespace OrderService.Services;
    public interface IGrpcOrdersService
    {
    Task<Order> CreateOrderAsync(CustomerOrder customerOrder);
    }
    public class GrpcOrderService : IGrpcOrdersService
    {
        private readonly ILogger<GrpcOrderService> _logger;
        private readonly Payments.PaymentsClient _paymentClient;
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;


    public GrpcOrderService(ILogger<GrpcOrderService> logger, Payments.PaymentsClient paymentClient, IMediator mediator, AppDbContext context)
    {
        _logger = logger;
        _mediator = mediator;
        _paymentClient = paymentClient;
        _context = context;

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
        var dateTimeNow = DateTime.Now;
        var newProduct = new PRODUCTS
        {
            ORDERID = customerOrder.OrderId.ToString(),
            EMAIL = "cuong25081@gmail.com",
            LOG = $"??t",
            ACCOUNT = (float)customerOrder.Amount,
            DATECREATE = DateTime.Now
        };

        _context.PRODUCTS.Add(newProduct);

        _context.SaveChanges();

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
            var data = new CreateOrder
            {
                IdUser = 1,
                OrderId = order.Id,
                Email = "tranduccuong25082001@gmail.com",
                Log = "Create Order",
                Account = 200000,
                DateCreate = DateTime.Now
            };
            _mediator.Send(new ORDERCommand(data));

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
