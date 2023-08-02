using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.DTO;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;

        public OrderController(IOrderService orderService, IShoppingCartService shoppingCartService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost("create")]
        public IActionResult CreateOrder(int userId, int paymentId, double shippingFee)
        {
            try
            {
                

                // Pass the cart items to the CreateOrder method
                var order = _orderService.CreateOrder(userId, paymentId, shippingFee);

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
