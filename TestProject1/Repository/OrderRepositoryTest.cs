using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository_Linq;
using TrainingBE.Common;

namespace TestProject1.Repository
{
    [TestFixture]
    internal class OrderRepositoryTest
    {
        private DbContextOptions<MyDBContext> _dbContextOptions;
        private OrderRepository_Linq _orderRepository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MyDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _orderRepository = new OrderRepository_Linq(new MyDBContext(_dbContextOptions));
            using (var context = new MyDBContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        [Test]
        public void  AddOrder_AddsOrderToDatabase()
        {
            // Arrange
            var order = new Order {Id = 1, UserID=1,PaymentID=1,OrderDate=DateTime.Now,orderStatus=OrderStatus.PENDING,Total=150000};

            // Act
            _orderRepository.Add(order);
            _orderRepository.Save();

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var result = context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
                Assert.IsNotNull(result);
            }
        }
        [Test]
        public void UpdateOrder()
        {
            // Arrange
            var order = new Order { Id = 1, UserID = 1, PaymentID = 1, OrderDate = DateTime.Now, orderStatus = OrderStatus.PENDING, Total = 150000 };
            _orderRepository.Add(order);
            _orderRepository.Save();
            //Act
            order.orderStatus = OrderStatus.PROCESSED;
            _orderRepository.Update(order);
            _orderRepository.Save();
            //Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var updatedOrder = context.Orders.FirstOrDefault(o => o.Id == order.Id);
                Assert.AreEqual(OrderStatus.PROCESSED, updatedOrder.orderStatus);
            }
        }
        [Test]
        public void GetOrderByID_ReturnsCorrectOrder()
        {
            // Arrange
            var order = new Order { Id = 1, UserID = 1, PaymentID = 1, OrderDate = DateTime.Now, orderStatus = OrderStatus.PENDING, Total = 150000 };
            _orderRepository.Add(order);
            _orderRepository.Save();
            // Act
            var result = _orderRepository.GetById(order.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(order.Id, result.Id);
            Assert.AreEqual(order.UserID, result.UserID);
            Assert.AreEqual(order.PaymentID, result.PaymentID);
        }
    }
}
