using PagedList;
using TrainingBE.DTO;
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
        List<CategoryProductDTO> GetProductsByCategoryIds(List<int> categoryIds);
        List<ProductWithDiscountDTO> GetProductsWithDiscountedPrice(DateTime currentDate);
        List<ProductWithDiscountDTO> GetSortedProductsWithDiscount(DateTime currentDate, string sortColumn, string sortOrder);
        List<ProductWithDiscountDTO> GetProductsWithDiscountByKeyword(DateTime currentDate, string keyword);
        Dictionary<string, List<ProductWithDiscountDTO>> GetProductsByPriceRange(DateTime currentDate, List<string> priceRanges);
        ProductWithDiscountDTO GetProductWithDiscountPriceById(DateTime currentDate, int productId);
        IPagedList<ProductWithDiscountDTO> GetPagedProductsWithDiscountedPrice(DateTime selectedDate, int pageNumber, int pageSize);
        List<ProductWithDiscountDTO> SearchProductByName(DateTime currentDate, string keyword);  
        List<ProductStatisticDTO> GetBestSellingProducts();
    }
}
