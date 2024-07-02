namespace Payments.Services;

using Common.Protobuf;

using Payments.Models;

using Payment = Models.Payment;

public class PaymentRpcService : Payments.PaymentsBase
{
    private readonly ILogger<PaymentRpcService> _logger;


    public PaymentRpcService(ILogger<PaymentRpcService> logger)
    {
        _logger = logger;
    }

    public override async Task<NewOrderResponse> NewOrder(NewOrderRequest request, Grpc.Core.ServerCallContext context)
    {
        _logger.LogInformation("Received: {}", request);

        var order = request.Order;

        var payment = new Payment
        {
            Amount = order.Amount,
            Mode = order.PaymentMethod,
            OrderId = order.OrderId,
            Status = "Success",
        };
        
        _logger.LogInformation("information order: {}", payment);

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