
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Orders.Models;
using OrderService.Models.ResponseData;

namespace OrderService
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IGrpcOrdersService _service;

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, IGrpcOrdersService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<ResponseData> CreatOrder(CustomerOrder customerOrder)
        {
            try
            {
                var data = await _service.CreateOrderAsync(customerOrder);
                return new ResponseData(true,"200",data);
            }
            catch (Exception ex) 
            {
                return new ResponseData(false, ex.Message);
            }

        }


    }
}
