using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        private readonly IOrderItemService _orderItemService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IShoppingCartService shoppingCartService, IPaymentService paymentService,IAuthService authService, IOrderItemService orderItemService, IUserService userService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
            _paymentService = paymentService;
            _authService = authService;
            _orderItemService = orderItemService;
            _userService = userService;
        }
        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateOrder(int paymentId, double shippingFee)
        {
            var userId = _userService.GetUserIdFromClaims(User);
            var existingPayment = _paymentService.GetPaymentById(paymentId);
            if (existingPayment == null)
            {
               return BadRequest("Payment type not exist");
            }
            var existingUser = _authService.getUserByID(userId);
            if (existingUser == null)
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

             return Ok("Order Sucessfully!!");
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("getByUserId/{userId}")]
        public IActionResult GetOrdersByUserId(int userId)
        {
            int currentUserId = _userService.GetUserIdFromClaims(User); // Lấy userId từ claims
            string currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value; // Lấy vai trò từ claims

            if (currentUserId != userId && currentUserRole != "Admin")
            {
                return Forbid(); // Người dùng không có quyền xem đơn hàng của người khác
            }
            var orders = _orderService.GetOrdersByUserId(userId);

            if (orders.Count == 0)
            {
                return NotFound("No orders found for the given user.");
            }

            return Ok(orders);
        }
        [Authorize]
        [HttpGet("getOrderItems/{orderId}")]
        public IActionResult GetOrderItemsByOrderId(int orderId)
        {
            int currentUserId = _userService.GetUserIdFromClaims(User);
            string currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value; // Lấy vai trò từ claims

            var order = _orderService.GetOrderByOrderId(orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }
            if (currentUserId != order.UserID && currentUserRole != "Admin")
            {
                return Forbid(); // Người dùng không có quyền xem đơn hàng của người khác
            }
            
            var orderItems = _orderItemService.GetOrderItemsByOrderId(orderId);

            if (orderItems.Count == 0)
            {
                return NotFound("No order items found for the given order.");
            }

            return Ok(orderItems);

            // If the user doesn't have permission

        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{orderId}")]
        public IActionResult UpdateOrder(int orderId, [FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            try
            {
                var existingOrder = _orderService.GetAllOrders().FirstOrDefault(order => order.Id == orderId);
                if (existingOrder == null)
                {
                    return NotFound("Order not found.");
                }

                existingOrder.orderStatus = orderUpdateDTO.orderStatus;

                _orderService.UpdateOrderStatus(orderId, orderUpdateDTO.orderStatus);
                return Ok("Order updated successfully.");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
