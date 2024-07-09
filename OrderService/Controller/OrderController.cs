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
                return new ResponseData(true, "200", data);
            }
            catch (Exception ex)
            {
                return new ResponseData(false, ex.Message);
            }

        }
        [HttpGet]
        [Route("GetCountOrder")]
        public async Task<ResponseData> GetCountOrder()
        {
            try
            {
                var data = await _service.GetCountOrder();
                return new ResponseData(true, "200", data);
            }
            catch (Exception ex)
            {
                return new ResponseData(false, ex.Message);
            }

        }
        [HttpPost]
        [Route("addToCart")]
        public async Task<ResponseData> AddToCart([FromBody] AddToCart_Dto data)
        {
            try
            {
                var res = await _service.AddToCart(data);
                return new ResponseData(true, "200", res);
            }
            catch (Exception ex)
            {
                return new ResponseData(false, ex.Message);
            }

        }
        [HttpGet]
        [Route("GetListProduct")]
        public async Task<ResponseData> GetListProduct(bool isBuy)
        {
            try
            {
                var data = await _service.GetListProduct(isBuy);
                return new ResponseData(true, "200", data);
            }
            catch (Exception ex)
            {
                return new ResponseData(false, ex.Message);
            }

        }
        [HttpGet]
        [Route("GetListPayment")]
        public async Task<ResponseData> GetListPayment()
        {
            try
            {
                var data = await _service.GetListPayment();
                return new ResponseData(true, "200", data);
            }
            catch (Exception ex)
            {
                return new ResponseData(false, ex.Message);
            }

        }



    }
}
