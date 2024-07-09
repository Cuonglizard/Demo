namespace Payments.Services;

using Common.Protobuf;

using Payments.Models;

using Payment = Models.Payment;
using Payments.Infrastructure;
using Payments.Infrastructure.Enities;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

public class PaymentRpcService : Payments.PaymentsBase
{
    private readonly ILogger<PaymentRpcService> _logger;
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;



    public PaymentRpcService(ILogger<PaymentRpcService> logger, AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public override async Task<NewOrderResponse> NewOrder(NewOrderRequest request, Grpc.Core.ServerCallContext context)
    {
        _logger.LogInformation("Received: {}", request);

        var order = request.Order;

        var record = _context.PAYMENT.Max(x => x.ID);
        var data = new PAYMENT
        {
            ORDERID = (int)order.OrderId,
            AMOUNT = (decimal)order.Amount,
            PAYMENTMETHOD = order.PaymentMethod,
            ADDRESS = order.Address,
            DATEPAYMENT = DateTime.Now,
            ID = record + 1
        };
        _context.PAYMENT.Add(data);
        var item = _context.ITEM.FirstOrDefault(ITEM => ITEM.ID == order.OrderId);
        item.ISBUY = item.ISBUY - 1;
        _context.ITEM.Update(item);

        _context.SaveChanges();

        var payment = new Payment
        {
            Amount = order.Amount,
            Mode = order.PaymentMethod,
            OrderId = order.OrderId,
            Status = "Success",
        };
        
        _logger.LogInformation("information order: {}", payment);
        var webhookUrl = "https://localhost:7031/webhook";
        var httpClient = _httpClientFactory.CreateClient();

        // T?o n?i dung JSON
        var jsonBody = new
        {
            Header = "Header",
            Body = "HelloWebHook"
        };

        // Chuy?n ??i object thành JSON
        var content = new StringContent(JsonSerializer.Serialize(jsonBody), Encoding.UTF8, "application/json");

        // Thêm Header Authorization
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("APIKEY");

        // G?i yêu c?u POST
        var response = await httpClient.PostAsync(webhookUrl, content);
        try
        {
            var paymentRequest = new NewPaymentRequest
            {
                Order = order,
            };

            _logger.LogInformation("Payment created: {}", order);
            _logger.LogInformation("Save data in Db");

            return new NewOrderResponse
            {
                Success = true,
            };
        }
        catch (Exception)
        {
            payment.Status = "Failed";
            _logger.LogError("Payment failed: {}", order);

            throw;
        }
    }
}