using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Service;

namespace TestProject1.Service
{
    [TestFixture]
    internal class DiscountServiceTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IDiscountService _discountService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _discountService = new DiscountService(_mockUnitOfWork.Object);
        }
        [Test]
        public void AddDiscount_ValidDiscount()
        {
            // Arrange
            var discountToCreate = new Discount { Percentage = 15, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(14) };
            _mockUnitOfWork.Setup(uow => uow.DiscountRepository.Add(discountToCreate)).Verifiable();

            // Act
            _discountService.AddDiscount(discountToCreate);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.DiscountRepository.Add(discountToCreate), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Test]
        public void DeleteDiscount_ExistingDiscountId_DeletesDiscount()
        {
            // Arrange
            var discountId = 1;
            var existingDiscount = new Discount { Id = discountId, Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };
            _mockUnitOfWork.Setup(uow => uow.DiscountRepository.GetById(discountId)).Returns(existingDiscount);

            // Act
            _discountService.DeleteDiscount(discountId, out string error);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.DiscountRepository.Delete(existingDiscount), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
            Assert.IsEmpty(error);
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
            _mockUnitOfWork.Setup(uow => uow.DiscountRepository.GetAll()).Returns(discounts);

            // Act
            var result = _discountService.GetAllDiscounts();

            // Assert
            CollectionAssert.AreEqual(discounts, result);
        }
        [Test]
        public void GetDiscountById_ExistingId_ReturnsDiscount()
        {
            // Arrange
            var discountId = 1;
            var expectedDiscount = new Discount { Id = discountId, Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };
            _mockUnitOfWork.Setup(uow => uow.DiscountRepository.GetById(discountId)).Returns(expectedDiscount);

            // Act
            var result = _discountService.GetDiscountById(discountId);

            // Assert
            Assert.AreEqual(expectedDiscount, result);
        }

        [Test]
        public void UpdateDiscount_ValidData_UpdatesDiscount()
        {
            // Arrange
            var discountId = 1;
            var existingDiscount = new Discount { Id = discountId, Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };
            var updatedDiscount = new Discount { Id = discountId, Percentage = 20, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(14) };
            _mockUnitOfWork.Setup(uow => uow.DiscountRepository.GetById(discountId)).Returns(existingDiscount);

            // Act
            var result = _discountService.UpdateDiscount(discountId, updatedDiscount, out string errorMessage);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.DiscountRepository.Update(existingDiscount), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
            Assert.IsTrue(result);
            Assert.IsEmpty(errorMessage);
        }
    }
}
