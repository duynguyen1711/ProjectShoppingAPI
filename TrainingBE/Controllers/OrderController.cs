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
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _authService;

        public OrderController(IOrderService orderService, IShoppingCartService shoppingCartService, IPaymentService paymentService,IAuthService authService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _paymentService = paymentService;
            _authService = authService;
        }

        [HttpPost("create")]
        public IActionResult CreateOrder(int userId, int paymentId, double shippingFee)
        {
            
                var existingPayment = _paymentService.GetPaymentById(paymentId);
                if (existingPayment == null)
                {
                    return BadRequest("Payment type not exist");
                }
                var existingUser = _authService.getUserByID(paymentId);
                if (existingPayment == null)
                {
                    return BadRequest("User not exist");
                }
                var shoppingCartItems = _shoppingCartService.GetCartItems();
                if (shoppingCartItems == null || shoppingCartItems.Count == 0)
                {
                   return BadRequest("The shopping cart is empty.");
                }

                // Pass the cart items to the CreateOrder method
                var order = _orderService.CreateOrder(userId, paymentId, shippingFee);

                return Ok(order);
         } 
    }
}
