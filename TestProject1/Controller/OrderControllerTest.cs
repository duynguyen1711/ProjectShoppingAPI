using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Common;
using TrainingBE.Controllers;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TestProject1.Controller
{
    [TestFixture]
    internal class OrderControllerTest
    {
        private Mock<IOrderService> _mockOrderService;
        private Mock<IShoppingCartService> _mockShoppingCartService;
        private Mock<IPaymentService> _mockPaymentService;
        private Mock<IAuthService> _mockAuthService;
        private Mock<IOrderItemService> _mockOrderItemService;
        private OrderController _orderController;
        [SetUp]
        public void Setup()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockShoppingCartService = new Mock<IShoppingCartService>();
            _mockPaymentService = new Mock<IPaymentService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockOrderItemService = new Mock<IOrderItemService>();

            _orderController = new OrderController(
                _mockOrderService.Object,
                _mockShoppingCartService.Object,
                _mockPaymentService.Object,
                _mockAuthService.Object,
                _mockOrderItemService.Object
            );
        }
        [Test]
        public void CreateOrder_WithValidInputs_ReturnsOk()
        {
            // Arrange
            var userId = 1;
            var paymentId = 1;
            var shippingFee = 10.0;

            // Mock the dependencies' behaviors
            _mockPaymentService.Setup(p => p.GetPaymentById(paymentId)).Returns(new Payment());
            _mockAuthService.Setup(a => a.getUserByID(userId)).Returns(new User());
            _mockShoppingCartService.Setup(s => s.GetCartItems()).Returns(new List<ShoppingCartItemDTO>());
            _mockOrderService.Setup(o => o.CreateOrder(userId, paymentId, shippingFee)).Returns(new Order());

            // Act
            var result = _orderController.CreateOrder(userId, paymentId, shippingFee) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual("Order Sucessfully!!", result.Value);

        }
        [Test]
        public void GetAllOrders_ReturnsOkWithOrderList()
        {
            // Arrange
            var orders = new List<OrderDTO>
            {
                new OrderDTO { Id = 1, UserID = 1, PaymentID = 1, Total = 100 },
                new OrderDTO { Id = 2, UserID = 2, PaymentID = 2, Total = 200 }
            };
            _mockOrderService.Setup(o => o.GetAllOrders()).Returns(orders);

            // Act
            var result = _orderController.GetAllOrders() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var orderList = result.Value as List<OrderDTO>;
            Assert.NotNull(orderList);
            Assert.AreEqual(2, orderList.Count); 
        }
        [Test]
        public void UpdateOrder_WithValidInputs_ReturnsOk()
        {
            // Arrange
            var orderId = 1;
            var orderUpdateDTO = new OrderUpdateDTO { orderStatus = OrderStatus.PROCESSED };

            var existingOrder = new OrderDTO { Id = orderId, orderStatus = OrderStatus.PENDING };
            _mockOrderService.Setup(o => o.GetAllOrders()).Returns(new List<OrderDTO> { existingOrder });

            // Act
            var result = _orderController.UpdateOrder(orderId, orderUpdateDTO) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            Assert.AreEqual("Order updated successfully.", result.Value);

            // Verify that the order status has been updated
            Assert.AreEqual(OrderStatus.PROCESSED, existingOrder.orderStatus);
        }
        [Test]
        public void GetOrdersByUserId_WithValidUserId_ReturnsOkWithOrders()
        {
            // Arrange
            var userId = 1;
            var orders = new List<OrderDTO>
            {
                new OrderDTO { Id = 1, UserID = userId },
                new OrderDTO { Id = 2, UserID = userId }
            };

            _mockOrderService.Setup(o => o.GetOrdersByUserId(userId)).Returns(orders);

            // Act
            var result = _orderController.GetOrdersByUserId(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var returnedOrders = result.Value as List<OrderDTO>;
            Assert.NotNull(returnedOrders);
            Assert.AreEqual(2, returnedOrders.Count);
        }

        [Test]
        public void GetOrdersByUserId_WithNoOrders_ReturnsNotFound()
        {
            // Arrange
            var userId = 1;
            var emptyOrders = new List<OrderDTO>();

            _mockOrderService.Setup(o => o.GetOrdersByUserId(userId)).Returns(emptyOrders);

            // Act
            var result = _orderController.GetOrdersByUserId(userId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual("No orders found for the given user.", result.Value);
        }
        [Test]
        public void GetOrderItemsByOrderId_WithValidOrderId_ReturnsOkWithOrderItems()
        {
            // Arrange
            var orderId = 1;
            var orderItems = new List<OrderItemDTO>
    {
        new OrderItemDTO { OrderId = orderId, ProductId = 1 },
        new OrderItemDTO { OrderId = orderId, ProductId = 2 }
    };

            _mockOrderItemService.Setup(o => o.GetOrderItemsByOrderId(orderId)).Returns(orderItems);

            // Act
            var result = _orderController.GetOrderItemsByOrderId(orderId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var returnedOrderItems = result.Value as List<OrderItemDTO>;
            Assert.NotNull(returnedOrderItems);
            Assert.AreEqual(2, returnedOrderItems.Count);
        }

        [Test]
        public void GetOrderItemsByOrderId_WithNoOrderItems_ReturnsNotFound()
        {
            // Arrange
            var orderId = 1;
            var emptyOrderItems = new List<OrderItemDTO>();

            _mockOrderItemService.Setup(o => o.GetOrderItemsByOrderId(orderId)).Returns(emptyOrderItems);

            // Act
            var result = _orderController.GetOrderItemsByOrderId(orderId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual("No order items found for the given order.", result.Value);
        }
    }
}
