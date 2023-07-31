using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class ProductDiscountRepository :  IProductDiscountRepository
    {
        private readonly MyDBContext _context;
        public ProductDiscountRepository(MyDBContext context) 
        {
            _context = context;
        }

        public void Add(Product_Discount product_discount)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC AddDiscountToProduct {product_discount.ProductId},{product_discount.DiscountId}");
        }

        public void Delete(Product_Discount product_discount)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC DeleteProductDiscount {product_discount.Id}");
        }

        // Các phương thức tương tác với bảng trung gian Product_Discount
        public bool Exists(int productId, int discountId)
        {
            var existsParameter = new SqlParameter("@Exists", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var productIdParameter = new SqlParameter("@ProductId", productId);
            var discountIdParameter = new SqlParameter("@DiscountId", discountId);

            _context.Database.ExecuteSqlRaw("EXEC usp_CheckProductDiscountExistence @ProductId, @DiscountId, @Exists OUT",
                productIdParameter, discountIdParameter, existsParameter);

            return (bool)existsParameter.Value;
        }

        public IEnumerable<Product_Discount> GetAll()
        {
            return _context.Products_Discount.FromSqlRaw("EXEC GetAllProductDiscount").ToList();
        }

        public IEnumerable<Product_Discount> GetAllProductDiscount()
        {
            string query = "EXEC GetAllProductDiscountWithCategory";

            // Thực hiện truy vấn và lấy kết quả thành danh sách
            List<Product_Discount> productDiscounts = _context.Products_Discount
                                                    .FromSqlRaw(query)
                                                    .ToList();
            return productDiscounts;

        }

        public Product_Discount GetById(int id)
        {
            var result = _context.Products_Discount.FromSqlInterpolated($"EXEC GetCategoryById {id}");
            return result.AsEnumerable().FirstOrDefault();
        }

        public List<Product_Discount> GetDiscountsByProductId(int productId)
        {
            var productIdParameter = new SqlParameter("@ProductId", productId);
            return _context.Products_Discount.FromSqlRaw("EXEC GetDiscountsByProductId @ProductId", productIdParameter).ToList();
        }

        public List<Product_Discount> GetProductsByDiscountId(int discountId)
        {
            var discountIdParameter = new SqlParameter("@DiscountId", discountId);
            return _context.Products_Discount.FromSqlRaw("EXEC GetProductsByDiscountId @DiscountId", discountIdParameter).ToList();
        }

        public void Update(Product_Discount product_discount)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC UpdateProductDiscount {product_discount.Id}, {product_discount.ProductId}, {product_discount.DiscountId}");
        }
    }
}
