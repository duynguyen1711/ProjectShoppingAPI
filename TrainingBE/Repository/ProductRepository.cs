using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDBContext context) : base(context)
        {
        }
    }
}
