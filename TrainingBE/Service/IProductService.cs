using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IProductService
    {
        void AddProduct(Product product);
        bool UpdateProduct(int id, Product product, out string errorMessage);
        void DeleteProduct(int id);
        Product GetProductById(int id);
        IEnumerable<Product> GetAllProducts();
        bool ValidateUpdateProduct(Product existingProduct, Product updatedProduct, out string errorMessage);
        bool ValidateAddProduct(Product product, out string errorMessage);
    }
}
