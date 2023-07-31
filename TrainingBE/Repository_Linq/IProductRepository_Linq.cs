using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public interface IProductRepository_Linq:IRepository<Product>
    {
        Product GetProductByProductName(string productName);
        IEnumerable<Product> GetAllProductsIncludingCategory();
        List<Product> GetProductsByCategoryId(int categoryId);
        IQueryable<Product> GetAllWithDiscounts();

    }
}
