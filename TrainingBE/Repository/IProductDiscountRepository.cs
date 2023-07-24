using Microsoft.EntityFrameworkCore;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IProductDiscountRepository : IRepository<Product_Discount>
    {
        bool Exists(int productId, int discountId);
        List<Product_Discount> GetDiscountsByProductId(int productId);
        List<Product_Discount> GetProductsByDiscountId(int discountId);
        IEnumerable<Product_Discount> GetAllProductDiscount();
    }
}
