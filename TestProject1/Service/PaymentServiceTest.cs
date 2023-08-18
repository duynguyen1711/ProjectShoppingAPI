using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Repository_Linq;
using TrainingBE.Service;

namespace TestProject1.Service
{
    [TestFixture]
    internal class PaymentServiceTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IPaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _paymentService = new PaymentService(_mockUnitOfWork.Object);
        }
        [Test]
        public void AddDiscount_ValidDiscount()
        {
            // Arrange
            var payment = new Payment { Id = 1, Type = Payment.PaymentType.MOMO };
            _mockUnitOfWork.Setup(uow => uow.PaymentRepository.Add(payment)).Verifiable();

            // Act
            _paymentService.AddPayment(payment);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.PaymentRepository.Add(payment), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Test]
        public void DeleteDiscount_ExistingDiscountId_DeletesDiscount()
        {
            // Arrange
            var paymentId = 1;
            var payment = new Payment { Id = 1, Type = Payment.PaymentType.MOMO };
            _mockUnitOfWork.Setup(uow => uow.PaymentRepository.GetById(paymentId)).Returns(payment);

            // Act
            _paymentService.DeletePayment(paymentId, out string error);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.PaymentRepository.Delete(payment), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
            Assert.IsEmpty(error);
        }

    }
}
