using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Controllers;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TestProject1.Controller
{
    [TestFixture]
    internal class PaymentControllerTest
    {
        private Mock<IPaymentService> _mockPaymentService;
        private PaymentController _paymentController;

        [SetUp]
        public void Setup()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _paymentController = new PaymentController(_mockPaymentService.Object);
        }
        [Test]
        public void AddPayment_ValidPayment_ReturnsOkResult()
        {
            // Arrange
            var paymentToAdd = new Payment { Id = 1, Type=Payment.PaymentType.MOMO };

            // Act
            var result = _paymentController.AddPayment(paymentToAdd) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Payment added successfully.", result.Value);
            _mockPaymentService.Verify(service => service.AddPayment(paymentToAdd), Times.Once);
        }

        [Test]
        public void DeletePayment_ValidID_ReturnsOkResult()
        {
            // Arrange
            var paymentId = 1;
            string errorMessage = null;
            _mockPaymentService.Setup(service => service.DeletePayment(paymentId, out errorMessage)).Callback(() => errorMessage = null);

            // Act
            var result = _paymentController.DeletePayment(paymentId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Payment deleted successfully.", result.Value);
            _mockPaymentService.Verify(service => service.DeletePayment(paymentId, out errorMessage), Times.Once);
            Assert.IsNull(errorMessage);
        }
    }
}







    
