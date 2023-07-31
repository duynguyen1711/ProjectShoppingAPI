using Microsoft.EntityFrameworkCore;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IProductDiscountRepository 
    {
        Product_Discount GetById(int id);
        IEnumerable<Product_Discount> GetAll();
        void Add(Product_Discount product_discount);
        void Update(Product_Discount product_discount);
        void Delete(Product_Discount product_discount);
        bool Exists(int productId, int discountId);
        List<Product_Discount> GetDiscountsByProductId(int productId);
        List<Product_Discount> GetProductsByDiscountId(int discountId);
        IEnumerable<Product_Discount> GetAllProductDiscount();
    }
}
