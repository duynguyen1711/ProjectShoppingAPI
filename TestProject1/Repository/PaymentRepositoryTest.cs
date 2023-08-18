using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository_Linq;

namespace TestProject1.Repository
{
    [TestFixture]
    internal class PaymentRepositoryTest
    {
        private DbContextOptions<MyDBContext> _dbContextOptions;
        private PaymentRepository_Linq _paymentRepository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MyDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _paymentRepository = new PaymentRepository_Linq(new MyDBContext(_dbContextOptions));
            using (var context = new MyDBContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        [Test]
        public void AddDiscount_Success()
        {
            // Arrange
            var payment = new Payment { Id = 1, Type = Payment.PaymentType.MOMO };
            // Act
            _paymentRepository.Add(payment);
            _paymentRepository.Save();

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var addedPayment = context.Payments.FirstOrDefault(d => d.Id == payment.Id);
                Assert.NotNull(addedPayment);
            }
        }
        [Test]
        public void DeleteDiscount_Success()
        {
            // Arrange
            var payment = new Payment { Id = 1, Type = Payment.PaymentType.MOMO };
            _paymentRepository.Add(payment);
            _paymentRepository.Save();

            // Act
            _paymentRepository.Delete(payment);
            _paymentRepository.Save();


            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var deletedPayment = context.Discounts.FirstOrDefault(d => d.Id == payment.Id);
                Assert.Null(deletedPayment);
            }
        }
    }
}
