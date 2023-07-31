using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public interface IProductDiscountRepository_Linq : IRepository<Product_Discount>
    {
        bool Exists(int productId, int discountId);
        List<Product_Discount> GetDiscountsByProductId(int productId);
        List<Product_Discount> GetProductsByDiscountId(int discountId);
        IEnumerable<Product_Discount> GetAllProductDiscount();
    }
}
