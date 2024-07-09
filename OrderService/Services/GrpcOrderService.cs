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
using Orders.Models;
using System.Linq;
using OrderService.Models.ResponseData;

namespace OrderService.Services;
    public interface IGrpcOrdersService
    {
    Task<Order> CreateOrderAsync(CustomerOrder customerOrder);
    Task<int> GetCountOrder();
    Task<bool> AddToCart(AddToCart_Dto data);
    Task<List<ListProducts>> GetListProduct(bool isBuy);
    Task<List<ListPayment>> GetListPayment();
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

        try
        {

            var orderRequest = new NewOrderRequest
            {
                Order = new Common.Protobuf.CustomerOrder
                {
                    Address = customerOrder.Address,
                    Amount = customerOrder.Amount,
                    Item = customerOrder.Item,
                    OrderId = customerOrder.OrderId,
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

    public Task<int> GetCountOrder()
    {
        try
        {
            int count = 0;
            count = _context.ITEM.Sum(item => item.ISBUY);
            return Task.FromResult(count);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public Task<bool> AddToCart(AddToCart_Dto data)
    {
        try
        {
            var item = _context.ITEM.FirstOrDefault(item => item.ID == data.OrderId);
            if (item != null)
            {
                item.ISBUY = item.ISBUY + 1; // Set the value you want to update
                _context.ITEM.Update(item);
                _context.SaveChanges();
            };
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public Task<List<ListProducts>> GetListProduct(bool isBuy)
    {
        try
        {
            var query = _context.ITEM.AsQueryable();

            if (isBuy)
            {
                query = query.Where(item => item.ISBUY != 0);
            }

            var data = query.Select(item => new ListProducts
            {
                OrderId = item.ID,
                ProductName = item.NAME,
                Price = item.PRICE,
                Amount = item.ISBUY,
                Description = item.DETAIL,
                Img = item.IMGLINK
            }).ToList();

            return Task.FromResult(data);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public Task<List<ListPayment>> GetListPayment()
    {
        try
        {
            var data = _context.PAYMENT.Select(payment => new ListPayment
            {
                OrderId = payment.ORDERID.ToString(),
                Amount = payment.AMOUNT,
                ProductName = _context.ITEM.FirstOrDefault(ITEM => ITEM.ID == payment.ORDERID).NAME,
                DatePayment = payment.DATEPAYMENT,
            }).ToList();

            return Task.FromResult(data);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    } 

}
