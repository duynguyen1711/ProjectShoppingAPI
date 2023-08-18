using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository_Linq;

namespace TestProject1.Repository
{
    [TestFixture]
    internal class ProductRepositoryTest
    {
        private DbContextOptions<MyDBContext> _dbContextOptions;
        private ProductRepository_Linq _productRepository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MyDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _productRepository = new ProductRepository_Linq(new MyDBContext(_dbContextOptions));
            using (var context = new MyDBContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        [Test]
        public void AddProduct_Success()
        {
            // Arrange
            var product = new Product { Id = 1, Name="Áo da bò",Price=150000,CategoryID=1};

            // Act
            _productRepository.Add(product);
            _productRepository.Save();

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var addedProduct = context.Products.FirstOrDefault(d => d.Id == product.Id);
                Assert.NotNull(addedProduct);
            }
        }
        [Test]
        public void DeleteProduct_Success()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Áo da bò", Price = 150000, CategoryID = 1 };

            _productRepository.Add(product);
            _productRepository.Save();

            // Act
            _productRepository.Delete(product);
            _productRepository.Save();


            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var deletedProduct = context.Products.FirstOrDefault(d => d.Id == product.Id);
                Assert.Null(deletedProduct);
            }
        }
        [Test]
        public void UpdateProduct_Success()
        {
            var product = new Product { Id = 1, Name = "Áo da bò", Price = 150000, CategoryID = 1 };

            _productRepository.Add(product);
            _productRepository.Save();


            // Act
            product.Name = "Áo updated";
            product.Price = 140000;
            product.CategoryID = 1;

            _productRepository.Update(product);
            _productRepository.Save();

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var updatedProduct = context.Products.FirstOrDefault(d => d.Id == product.Id);
                Assert.NotNull(updatedProduct);
                Assert.AreEqual(product.Name, updatedProduct.Name);
                Assert.AreEqual(product.Price, updatedProduct.Price);
                Assert.AreEqual(product.CategoryID, updatedProduct.CategoryID);
            }
        }
        [Test]
        public void GetById_ReturnsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Áo da bò", Price = 150000, CategoryID = 1 };

            _productRepository.Add(product);
            _productRepository.Save();

            // Act
            var retrievedProduct = _productRepository.GetById(product.Id);

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.NotNull(retrievedProduct);
            Assert.AreEqual(product.Id, retrievedProduct.Id);
            Assert.AreEqual(product.Name, retrievedProduct.Name);
            Assert.AreEqual(product.Price, retrievedProduct.Price);
            Assert.AreEqual(product.CategoryID, retrievedProduct.CategoryID);
        }
        [Test]
        public void GetAll_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Áo da bò", Price = 150000, CategoryID = 1 },
                new Product { Id = 2, Name = "Quần jean", Price = 200000, CategoryID = 2 },
                new Product { Id = 3, Name = "Áo thun", Price = 100000, CategoryID = 1 }
            };
            foreach (var product in products)
            {
                _productRepository.Add(product);
            }
            _productRepository.Save();

            // Act
            var retrievedProducts = _productRepository.GetAll();

            // Assert
            CollectionAssert.AreEqual(products, retrievedProducts);
        }
    }
}
