using Microsoft.EntityFrameworkCore;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductByProductName(string productName);
        IEnumerable<Product> GetAllProductsIncludingCategory();
        List<Product> GetProductsByCategoryId(int categoryId);
        IQueryable<Product> GetAllWithDiscounts();
        
    }
}
