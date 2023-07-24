using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class ProductDiscountRepository : Repository<Product_Discount>, IProductDiscountRepository
    {
        public ProductDiscountRepository(MyDBContext context) : base(context)
        {
        }

        // Các phương thức tương tác với bảng trung gian Product_Discount
        public bool Exists(int productId, int discountId)
        {
            return _dbSet.Any(pd => pd.ProductId == productId && pd.DiscountId == discountId);
        }

        public IEnumerable<Product_Discount> GetAllProductDiscount()
        {
            return _dbSet.Include(pd => pd.Product).ThenInclude(p => p.Category).Include(pd => pd.Discount);
        }
        public List<Product_Discount> GetDiscountsByProductId(int productId)
        {
            return _dbSet.Where(pd => pd.ProductId == productId).ToList();
        }

        public List<Product_Discount> GetProductsByDiscountId(int discountId)
        {
            return _dbSet.Where(pd => pd.DiscountId == discountId).ToList();
        }
        
    }
}
