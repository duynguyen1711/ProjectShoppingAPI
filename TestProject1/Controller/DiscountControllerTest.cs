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
    internal class DiscountControllerTest
    {
        
        
            private Mock<IDiscountService> _mockDiscountService;
            private DiscountController _discountController;

            [SetUp]
            public void Setup()
            {
                _mockDiscountService = new Mock<IDiscountService>();
                _discountController = new DiscountController(_mockDiscountService.Object);
            }

            [Test]
            public void GetAllDiscounts_ReturnsListOfDiscounts()
            {
                // Arrange
                var discounts = new List<Discount>
                {
                    new Discount { Id = 1, Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) },
                    new Discount { Id = 2, Percentage = 20, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(14) }
                };
                _mockDiscountService.Setup(service => service.GetAllDiscounts()).Returns(discounts);

                // Act
                var result = _discountController.GetAllDiscounts();

                // Assert
                
                Assert.IsInstanceOf<ActionResult<IEnumerable<Discount>>>(result);
                var okResult = (OkObjectResult)result.Result; 
                var returnedDiscounts = (IEnumerable<Discount>)okResult.Value;
                CollectionAssert.AreEqual(discounts, returnedDiscounts);
            }

            [Test]
            public void GetDiscountById_ValidId_ReturnsDiscount()
            {
                // Arrange
                var discountId = 1;
                var expectedDiscount = new Discount { Id = discountId, Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };
                _mockDiscountService.Setup(service => service.GetDiscountById(discountId)).Returns(expectedDiscount);

                // Act
                var result = _discountController.GetDiscountById(discountId);

                // Assert
                Assert.IsInstanceOf<ActionResult<Discount>>(result);
                var okResult = (OkObjectResult)result.Result;
                var returnedDiscount = (Discount)okResult.Value;
                Assert.AreEqual(expectedDiscount, returnedDiscount);
            }
            [Test]
            public void CreateDiscount_ValidDiscount_ReturnsCreatedDiscount()
            {
                // Arrange
                var discountToCreate = new Discount { Percentage = 15, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(14) };
                _mockDiscountService.Setup(service => service.ValidateAddDisscount(discountToCreate, out It.Ref<string>.IsAny)).Returns(true);

                // Act
                var result = _discountController.AddDiscount(discountToCreate);

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result);
                var okResult = (OkObjectResult)result;
                var message = (string)okResult.Value;
                Assert.AreEqual("Discount added successfully.", message);
            }

            [Test]
            public void UpdateDiscount_ValidDiscount_ReturnsSuccessMessage()
            {
                // Arrange
                var discountId = 1;
                var updatedDiscount = new Discount { Id = discountId, Percentage = 20, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(14) };
                _mockDiscountService.Setup(service => service.UpdateDiscount(discountId, updatedDiscount, out It.Ref<string>.IsAny)).Returns(true);

                // Act
                var result = _discountController.UpdateDiscount(discountId, updatedDiscount);

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result);
                var okResult = (OkObjectResult)result;
                var message = (string)okResult.Value;
                Assert.AreEqual("Discount updated successfully.", message);
            }

            [Test]
            public void DeleteDiscount_ValidId_ReturnsSuccessMessage()
            {
                // Arrange
                var discountId = 1;
                _mockDiscountService.Setup(service => service.DeleteDiscount(discountId,  out It.Ref<string>.IsAny));

                // Act
                var result = _discountController.DeleteDiscount(discountId);

                // Assert
                Assert.IsInstanceOf<OkObjectResult>(result);
                var okResult = (OkObjectResult)result;
                var message = (string)okResult.Value;
                Assert.AreEqual("Discount deleted successfully.", message);
            }
     }
}
