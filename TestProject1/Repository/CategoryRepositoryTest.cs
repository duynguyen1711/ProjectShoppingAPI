using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
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

namespace TestProject1.Repository
{
    [TestFixture]
    internal class CategoryRepositoryTest
    {
        private DbContextOptions<MyDBContext> _dbContextOptions;
        private CategoryRepository_Linq _categoryRepository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MyDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _categoryRepository = new CategoryRepository_Linq(new MyDBContext(_dbContextOptions));
            using (var context = new MyDBContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        [Test]
        public void AddCategory_Success()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };

            // Act
            _categoryRepository.Add(category);
            _categoryRepository.Save();

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var addedCategory = context.Categories.FirstOrDefault(c => c.Name == category.Name);
                Assert.NotNull(addedCategory);
            }
        }
        [Test]
        public void UpdateCategory_Success()
        {
            // Arrange
            var originalCategory = new Category { Name = "Original Category" };
            _categoryRepository.Add(originalCategory);
            _categoryRepository.Save();

            // Act
            originalCategory.Name = "Updated Category";
            _categoryRepository.Update(originalCategory); 
            _categoryRepository.Save(); 

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var updatedCategory = context.Categories.FirstOrDefault(c => c.Id == originalCategory.Id);
                Assert.NotNull(updatedCategory);
                Assert.AreEqual("Updated Category", updatedCategory.Name);
            }
        }
        [Test]
        public void DeleteCategory_Success()
        {
            // Arrange
            var category = new Category { Name = "Test Category" };
            _categoryRepository.Add(category);
            _categoryRepository.Save(); 

            // Act
            _categoryRepository.Delete(category);
            _categoryRepository.Save(); 

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var deletedCategory = context.Categories.FirstOrDefault(c => c.Id == category.Id);
                Assert.Null(deletedCategory); 
            }
        }
        [Test]
        public void GetById_ReturnsCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Test Category" };
            _categoryRepository.Add(category);
            _categoryRepository.Save();

            // Act
            var retrievedCategory = _categoryRepository.GetById(category.Id);

            // Assert
            Assert.NotNull(retrievedCategory);
            Assert.AreEqual(category.Id, retrievedCategory.Id);
            Assert.AreEqual(category.Name, retrievedCategory.Name);
        }


    }
}
