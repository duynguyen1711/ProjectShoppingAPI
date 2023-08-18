using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Repository_Linq;

namespace TestProject1.Repository
{
    [TestFixture]
    internal class DiscountRepositoryTest
    {
        private DbContextOptions<MyDBContext> _dbContextOptions;
        private DiscountRepository_Linq _discountRepository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MyDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _discountRepository = new DiscountRepository_Linq(new MyDBContext(_dbContextOptions));
            using (var context = new MyDBContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        [Test]
        public void AddDiscount_Success()
        {
            // Arrange
            var discount = new Discount {Id = 1,Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };

            // Act
            _discountRepository.Add(discount);
            _discountRepository.Save();

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var addedDiscount = context.Discounts.FirstOrDefault(d => d.Id == discount.Id);
                Assert.NotNull(addedDiscount);
            }
        }
        [Test]
        public void UpdateDiscount_Success()
        {
            var existDiscount = new Discount { Id=10,Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };
            _discountRepository.Add(existDiscount);
            _discountRepository.Save();
            

            // Act
            existDiscount.Percentage = 20;
            existDiscount.StartDate = DateTime.Now;
            existDiscount.EndDate = DateTime.Now.AddDays(14);

            _discountRepository.Update(existDiscount);
            _discountRepository.Save();

            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var updatedDiscount = context.Discounts.FirstOrDefault(d => d.Id == existDiscount.Id);
                Assert.NotNull(updatedDiscount);
                Assert.AreEqual(existDiscount.Percentage, updatedDiscount.Percentage);
                Assert.AreEqual(existDiscount.StartDate, updatedDiscount.StartDate);
                Assert.AreEqual(existDiscount.EndDate, updatedDiscount.EndDate);
            }
        }

        [Test]
        public void DeleteDiscount_Success()
        {
            // Arrange
            var discount = new Discount { Id=10, Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };
            _discountRepository.Add(discount);
            _discountRepository.Save();

            // Act
            _discountRepository.Delete(discount);
            _discountRepository.Save();


            // Assert
            using (var context = new MyDBContext(_dbContextOptions))
            {
                var deletedDiscount = context.Discounts.FirstOrDefault(d => d.Id== discount.Id);
                Assert.Null(deletedDiscount);
            }
        }
        [Test]
        public void GetById_ReturnsDiscount()
        {
            // Arrange
            var discount = new Discount { Id = 10, Percentage = 10, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };

            _discountRepository.Add(discount);
            _discountRepository.Save();

            // Act
            var retrievedDiscount = _discountRepository.GetById(discount.Id);

            // Assert
            Assert.NotNull(retrievedDiscount);
            Assert.AreEqual(discount.Id, retrievedDiscount.Id);
            Assert.AreEqual(discount.Percentage, retrievedDiscount.Percentage);
            Assert.AreEqual(discount.StartDate, retrievedDiscount.StartDate);
            Assert.AreEqual(discount.EndDate, retrievedDiscount.EndDate);

        }
    }
}
