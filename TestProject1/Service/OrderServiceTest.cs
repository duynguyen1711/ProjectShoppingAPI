using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Common;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Repository_Linq;
using TrainingBE.Service;

namespace TestProject1.Service
{
    [TestFixture]
    internal class OrderServiceTest
    {
        private Mock<IShoppingCartService> _mockShoppingCartService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IOrderItemService> _mockOrderItemService;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _mockShoppingCartService = new Mock<IShoppingCartService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderItemService = new Mock<IOrderItemService>();

            _orderService = new OrderService(_mockShoppingCartService.Object, _mockUnitOfWork.Object, _mockOrderItemService.Object);
        }

        [Test]
        public void CreateOrder_ReturnsCreatedOrder()
        {
            // Arrange
            var userId = 1;
            var paymentId = 1;
            var shippingFee = 10.0;
            var shoppingCartItems = new List<ShoppingCartItemDTO>
            {
                new ShoppingCartItemDTO { ProductId = 1, Quantity = 2, DiscountedPrice = 50 },
                new ShoppingCartItemDTO { ProductId = 2, Quantity = 3, DiscountedPrice = 30 }
            };

            _mockShoppingCartService.Setup(s => s.GetCartItems()).Returns(shoppingCartItems);
            _mockShoppingCartService.Setup(s => s.GetTotalPrice()).Returns(220);
            _mockShoppingCartService.Setup(s => s.CaculateShippingFee(It.IsAny<double>(), It.IsAny<double>())).Returns(10);
            _mockShoppingCartService.Setup(s => s.GetTotalPriceWithShippingDiscount(It.IsAny<double>(), It.IsAny<double>())).Returns(230);

            _mockUnitOfWork.Setup(u => u.OrderRepository.Add(It.IsAny<Order>()));
            _mockOrderItemService.Setup(o => o.AddOrderItem(It.IsAny<OrderItem>()));

            // Act
            var createdOrder = _orderService.CreateOrder(userId, paymentId, shippingFee);

            // Assert
            _mockOrderItemService.Verify(o => o.AddOrderItem(It.IsAny<OrderItem>()), Times.Exactly(2));

            Assert.AreEqual(userId, createdOrder.UserID);
            Assert.AreEqual(paymentId, createdOrder.PaymentID);
            Assert.AreEqual(OrderStatus.PENDING, createdOrder.orderStatus);
        }
        [Test]
        public void UpdateOrderStatus_UpdatesOrderStatus()
        {
            // Arrange
            int orderId = 1;
            OrderStatus newStatus = OrderStatus.PROCESSED;
            var order = new Order { Id = orderId, UserID = 1, PaymentID = 1, orderStatus = OrderStatus.PENDING, Total = 150000 };
            _mockUnitOfWork.Setup(u => u.OrderRepository.GetById(orderId)).Returns(order);

            // Act
            _orderService.UpdateOrderStatus(orderId, newStatus);

            // Assert
            Assert.AreEqual(newStatus, order.orderStatus);
            _mockUnitOfWork.Verify(u => u.OrderRepository.Update(order), Times.Once);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }
        [Test]
        public void GetAllOrders_ReturnsCorrectOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, UserID = 1, PaymentID = 1, orderStatus = OrderStatus.PENDING, Total = 150000 },
                new Order { Id = 2, UserID = 2, PaymentID = 1, orderStatus = OrderStatus.PROCESSED, Total = 200000 }
            };
            _mockUnitOfWork.Setup(u => u.OrderRepository.GetAll()).Returns(orders);

            // Act
            var result = _orderService.GetAllOrders();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].UserID);
            Assert.AreEqual(2, result[1].UserID);
        }

        [Test]
        public void GetOrdersByUserId_ReturnsCorrectOrders()
        {
            // Arrange
            int userId = 1;
            var orders = new List<Order>
            {
                new Order { Id = 1, UserID = userId, PaymentID = 1, orderStatus = OrderStatus.PENDING, Total = 150000 },
                new Order { Id = 2, UserID = userId, PaymentID = 1, orderStatus = OrderStatus.PROCESSED, Total = 200000 }
            };
            _mockUnitOfWork.Setup(u => u.OrderRepository.GetAll()).Returns(orders.AsQueryable());

            // Act
            var result = _orderService.GetOrdersByUserId(userId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(userId, result[0].UserID);
            Assert.AreEqual(userId, result[1].UserID);
        }
    }
}
