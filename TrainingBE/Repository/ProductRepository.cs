using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDBContext context) : base(context)
        { 
        }
        public Product GetProductByProductName(string productName)
        {
            return _dbSet.FirstOrDefault(p => p.Name == productName);
        }
    }
}
