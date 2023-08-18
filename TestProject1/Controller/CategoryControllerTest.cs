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
    internal class CategoryControllerTest
    {
        private Mock<ICategoryService> mockCategoryService;
        private CategoryController categoryController;

        [SetUp]
        public void Setup()
        {
            mockCategoryService = new Mock<ICategoryService>();
            categoryController = new CategoryController(mockCategoryService.Object);
        }

        [Test]
        public void Index_ReturnsOkResultWithCategories()
        {
            var categories = new List<Category> { new Category(), new Category() };
            mockCategoryService.Setup(service => service.GetCategory()).Returns(categories);

            var result = categoryController.Index();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(categories, okResult.Value);
        }

        [Test]
        public void GetByID_InvalidID_ReturnsBadRequest()
        {
            var result = categoryController.GetByID(-1);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void GetByID_ValidID_CategoryFound_ReturnsOkResult()
        {
            var category = new Category { Id = 1, Name = "Test Category" };
            mockCategoryService.Setup(service => service.GetCategoryById(1)).Returns(category);

            var result = categoryController.GetByID(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(category, okResult.Value);
        }

        [Test]
        public void GetByID_ValidID_CategoryNotFound_ReturnsNotFoundResult()
        {
            mockCategoryService.Setup(service => service.GetCategoryById(1)).Returns((Category)null);

            var result = categoryController.GetByID(1);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
