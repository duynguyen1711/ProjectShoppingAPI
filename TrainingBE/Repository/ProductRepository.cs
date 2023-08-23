using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.DTO;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class ProductRepository:  IProductRepository
    {
        private readonly MyDBContext _context;
        public ProductRepository(MyDBContext context)
        { 
            _context = context;
        }
        public Product GetProductByProductName(string productName)
        {
            var result = _context.Products.FromSqlInterpolated($"EXEC GetProductByName {productName}");
            return result.AsEnumerable().FirstOrDefault();
        }
 
        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            var categoryIdParam = new SqlParameter("@CategoryId", categoryId);
            return _context.Products.FromSqlRaw("EXEC usp_GetProductsByCategoryId @CategoryId", categoryIdParam).ToList();
        }

        public IQueryable<Product> GetAllWithDiscounts()
        {
            return _context.Products.Include(p => p.Product_Discounts);
        }

        public Product GetById(int id)
        {
            var result = _context.Products.FromSqlInterpolated($"EXEC GetProductById {id}");
            return result.AsEnumerable().FirstOrDefault();
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.FromSqlRaw("EXEC GetAllProducts").ToList();
        }

        public void Add(Product product)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC InsertProduct {product.Name}, {product.Price}, {product.CategoryID}");
        }

        public void Update(Product product)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC UpdateProduct {product.Id}, {product.Name}, {product.Price}, {product.CategoryID}");
        }

        public void Delete(Product product)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC DeleteProduct {product.Id}");
        }
        public List<Product> SearchProductsWithDiscount(string keyword)
        {
            return _context.Products
        .FromSqlRaw("EXEC GetProductByKeyWord @Keyword", new SqlParameter("@Keyword", keyword))
        .ToList();
        }
    }
}
