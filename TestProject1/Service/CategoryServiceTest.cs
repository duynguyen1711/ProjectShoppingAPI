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
    internal class CategoryServiceTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private CategoryService _categoryService;
        private Mock<ICategoryRepository> _mockCategoryRepository;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUnitOfWork.Setup(uow => uow.CategoryRepository).Returns(_mockCategoryRepository.Object);
            _categoryService = new CategoryService(_mockUnitOfWork.Object);
        }

        [Test]
        public void AddCategory_ValidCategory_Success()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };
            _mockCategoryRepository.Setup(repo => repo.Add(It.IsAny<Category>()));

            // Act
            _categoryService.AddCategory(category);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.Add(category), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Test]
        public void UpdateCategory_ExistingCategory_Success()
        {
            // Arrange
            int categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "Existing Category" };
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryId)).Returns(existingCategory);
            string errorMessage = string.Empty;
            var updatedCategory = new Category { Id = categoryId, Name = "Updated Category Name" };

            // Act
            var result = _categoryService.UpdateCategory(categoryId, updatedCategory, out errorMessage);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.Update(existingCategory), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
            Assert.IsTrue(result);
            Assert.AreEqual(string.Empty, errorMessage);
        }

        [Test]
        public void DeleteCategory_ExistingCategory_Success()
        {
            // Arrange
            int categoryIdToDelete = 1;
            var existingCategory = new Category { Id = categoryIdToDelete, Name = "Category to Delete" };
            _mockCategoryRepository.Setup(repo => repo.GetById(categoryIdToDelete)).Returns(existingCategory);
            string errorMessage = string.Empty;

            // Act
            _categoryService.DeleteCategory(categoryIdToDelete, out errorMessage);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.Delete(existingCategory), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
            Assert.AreEqual(string.Empty, errorMessage);
        }

        [Test]
        public void DeleteCategory_NonExistingCategory_Error()
        {
            // Arrange
            _mockCategoryRepository.Setup(repo => repo.GetById(1)).Returns((Category)null);
            string errorMessage;

            // Act
            _categoryService.DeleteCategory(1, out errorMessage);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.Delete(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
            Assert.AreEqual("Category not found", errorMessage);
        }
    }
}
