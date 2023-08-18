using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Controllers;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TestProject1.Controller
{
    [TestFixture]
    internal class ProductControllerTest
    {
        private Mock<IProductService> _mockProductService;
        private Mock<IMemoryCache> _mockMemoryCache;
        private ProductController _productController;

        [SetUp]
        public void Setup()
        {
            _mockProductService = new Mock<IProductService>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _productController = new ProductController(_mockProductService.Object, _mockMemoryCache.Object);
        }
        [Test]
        public void AddProduct_ValidProduct_ReturnsOkResult()
        {
            // Arrange
            var productToAdd = new Product { Id = 1, Name = "Áo da bò", Price = 150000, CategoryID = 1 };
            _mockProductService.Setup(service => service.ValidateAddProduct(productToAdd, out It.Ref<string>.IsAny)).Returns(true);
            _mockProductService.Setup(service => service.AddProduct(productToAdd));

            // Act
            var result = _productController.AddProduct(productToAdd);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public void DeleteProduct_ExistingProductId_ReturnsOkResult()
        {
            // Arrange
            var productId = 1;
            _mockProductService.Setup(service => service.DeleteProduct(productId));

            // Act
            var result = _productController.DeleteProduct(productId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
