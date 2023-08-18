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
    internal class ProductServiceTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _productService = new ProductService(_mockUnitOfWork.Object);
        }
        [Test]
        public void AddProduct_ValidProduct()
        {
            // Arrange
            var productToAdd = new Product { Id = 1, Name = "Áo da bò", Price = 150000, CategoryID = 1 };
            _mockUnitOfWork.Setup(uow => uow.ProductRepository.Add(productToAdd)).Verifiable();
            // Act
            _productService.AddProduct(productToAdd);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.ProductRepository.Add(productToAdd), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }
        [Test]
        public void DeleteDiscount_ExistingDiscountId_DeletesDiscount()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new Product { Id = productId, Name = "Áo da bò", Price = 150000, CategoryID = 1 };
            _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetById(productId)).Returns(existingProduct);

            // Act
            _productService.DeleteProduct(productId);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.ProductRepository.Delete(existingProduct), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
            
        }
        [Test]
        public void GetAllProducts_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Áo da bò", Price = 150000, CategoryID = 1 },
                new Product { Id = 2, Name = "Quần jean", Price = 200000, CategoryID = 2 },
                new Product { Id = 3, Name = "Áo thun", Price = 100000, CategoryID = 1 }
            };
            _mockUnitOfWork.Setup(uow => uow.ProductRepository.GetAll()).Returns(products);

            // Act
            var result = _productService.GetAllProducts();

            // Assert
            CollectionAssert.AreEqual(products, result);
        }
    }
}
