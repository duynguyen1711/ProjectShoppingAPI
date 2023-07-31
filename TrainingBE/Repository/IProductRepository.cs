using Microsoft.EntityFrameworkCore;
using TrainingBE.DTO;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IProductRepository 
    {
        Product GetById(int id);
        IEnumerable<Product> GetAll();
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        Product GetProductByProductName(string productName);
        List<Product> GetProductsByCategoryId(int categoryId);
        IQueryable<Product> GetAllWithDiscounts();
        List<Product> SearchProductsWithDiscount(string keyword);
        

    }
}
