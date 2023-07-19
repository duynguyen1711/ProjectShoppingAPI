using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IProductService
    {
        void AddProduct(Product product);
        void UpdateProduct(int id, Product product);
        void DeleteProduct(int id);
        Product GetProductById(int id);
        IEnumerable<Product> GetAllProducts();
    }
}
