using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductByProductName(string productName);
    }
}
