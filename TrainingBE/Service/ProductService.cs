using PagedList;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddProduct(Product product)
        {
            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Save();
        }

        public void DeleteProduct(int id)
        { 
            var existingProduct = _unitOfWork.ProductRepository.GetById(id);

            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found");
            }

            _unitOfWork.ProductRepository.Delete(existingProduct);
            _unitOfWork.Save();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _unitOfWork.ProductRepository.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _unitOfWork.ProductRepository.GetById(id);
        }

        public bool UpdateProduct(int id, Product product, out string errorMessage)
        {
            if (id <= 0)
            {
                errorMessage = "Invalid ID. ID must be a non-negative number.";
                return false;
            }
            var existingProduct = _unitOfWork.ProductRepository.GetById(id);

            if (existingProduct == null)
            {
                errorMessage = "Product not found";
                return false;
            }

            if (!ValidateUpdateProduct(existingProduct, product, out errorMessage))
            {
                return false;
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryID = product.CategoryID;

            _unitOfWork.ProductRepository.Update(existingProduct);
            _unitOfWork.Save();

            errorMessage = string.Empty;
            return true;
        }

        public bool ValidateAddProduct(Product product, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                errorMessage = "Product name must not be empty or contain only spaces.";
                return false;
            }
            if (_unitOfWork.ProductRepository.GetProductByProductName(product.Name) != null)
            {
                errorMessage = "Product name already exists.";
                return false;
            }
            if (product.Name.Length < 3)
            {
                errorMessage = "Product name must be at least 3 characters.";
                return false;
            }

            if (product.Price < 0)
            {
                errorMessage = "Product price must be a non-negative number.";
                return false;
            }

            if (product.CategoryID < 0)
            {
                errorMessage = "CategoryID must be a non-negative number.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public bool ValidateUpdateProduct(Product existingProduct,Product updatedProduct, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(updatedProduct.Name))
            {
                errorMessage = "Product name must not be empty or contain only spaces.";
                return false;
            }

            if (existingProduct.Name != updatedProduct.Name)
            {
                var otherProducts = _unitOfWork.ProductRepository.GetAll().Where(p => p.Id != existingProduct.Id);
                if (otherProducts.Any(p => p.Name == updatedProduct.Name))
                {
                    errorMessage = "Product name is already taken by another product.";
                    return false;
                }
            }
            if (updatedProduct.Name.Length < 3)
            {
                errorMessage = "Product name must be at least 3 characters.";
                return false;
            }

            if (updatedProduct.Price < 0)
            {
                errorMessage = "Product price must be a non-negative number.";
                return false;
            }

            if (updatedProduct.CategoryID < 0)
            {
                errorMessage = "CategoryID must be a non-negative number.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        

        public List<CategoryProductDTO> GetProductsByCategoryIds(List<int> categoryIds)
        {
            var result = new List<CategoryProductDTO>();

            foreach (int categoryId in categoryIds)
            {
                var category = _unitOfWork.CategoryRepository.GetById(categoryId);
                if (category != null)
                {
                    var productsInCategory = _unitOfWork.ProductRepository.GetProductsByCategoryId(categoryId);
                    var productDTOs = productsInCategory.Select(p => new ProductDTO
                    {
                        Name = p.Name,
                        Price = p.Price,
                        CategoryID = categoryId,
                    }).ToList();

                    result.Add(new CategoryProductDTO
                    {
                        CategoryName = category.Name,
                        Products = productDTOs
                    });
                }
            }

            return result;
        }

        public List<ProductWithDiscountDTO> GetProductsWithDiscountedPrice(DateTime currentDate)
        {
            List<ProductWithDiscountDTO> discountedProducts = new List<ProductWithDiscountDTO>();

            // Lấy danh sách tất cả các sản phẩm từ cơ sở dữ liệu, bao gồm thông tin Product_Discounts
            List<Product> allProducts = _unitOfWork.ProductRepository.GetAllWithDiscounts().ToList();
            List <Category> allCategories = _unitOfWork.CategoryRepository.GetAll().ToList();

            // Lấy danh sách mã giảm giá có hiệu lực cho ngày hiện tại, có thể sử dụng GetValidProductDiscounts
            List<Discount> validDiscounts = GetValidProductDiscounts(currentDate);

            foreach (var product in allProducts)
            {
                double discountedPrice = product.Price;
                DiscountDTO appliedDiscount = null;

                // Tìm mã giảm giá đã áp dụng cho sản phẩm
                foreach (var discount in validDiscounts)
                {
                    // Kiểm tra xem sản phẩm đã áp dụng mã giảm này hay chưa
                    var productDiscount = product.Product_Discounts.FirstOrDefault(pd => pd.DiscountId == discount.Id);
                    if (productDiscount != null)
                    {
                        discountedPrice = product.Price - (product.Price * discount.Percentage / 100);
                        appliedDiscount = new DiscountDTO
                        {
                            Percentage = discount.Percentage,
                            StartDate = discount.StartDate,
                            EndDate = discount.EndDate
                        };
                        break;
                    }
                }

                // Tạo DTO cho sản phẩm đã giảm giá (nếu có)
                ProductWithDiscountDTO discountedProduct = new ProductWithDiscountDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    OriginalPrice = product.Price,
                    DiscountedPrice = discountedPrice,
                    CategoryId = product.CategoryID,
                    Category = product.Category,
                    Discount = appliedDiscount
                };

                discountedProducts.Add(discountedProduct);
            }
            return discountedProducts;
        }
        private List<Discount> GetValidProductDiscounts(DateTime currentDate)
        {
            List<Discount> validDiscounts = new List<Discount>();

            // Lấy danh sách tất cả các mã giảm giá từ cơ sở dữ liệu, có thể sử dụng DiscountRepository
            List<Discount> allDiscounts = _unitOfWork.DiscountRepository.GetAll().ToList();

            // Lọc các mã giảm giá có hiệu lực cho ngày hiện tại
            foreach (var discount in allDiscounts)
            {
                if (discount.StartDate.Date <= currentDate && discount.EndDate.Date >= currentDate)
                {
                    validDiscounts.Add(discount);
                }
            }

            return validDiscounts;
        }

        public List<ProductWithDiscountDTO> GetSortedProductsWithDiscount(DateTime currentDate, string sortColumn, string sortOrder)
        {
            // Lấy danh sách sản phẩm với mã giảm giá từ ProductService
            List<ProductWithDiscountDTO> discountedProducts = GetProductsWithDiscountedPrice(currentDate);

            // Sắp xếp danh sách sản phẩm tùy chọn
            switch (sortColumn)
            {
                case "Name":
                    discountedProducts = sortOrder == "asc"
                        ? discountedProducts.OrderBy(p => p.Name).ToList()
                        : discountedProducts.OrderByDescending(p => p.Name).ToList();
                    break;
                case "OriginalPrice":
                    discountedProducts = sortOrder == "asc"
                        ? discountedProducts.OrderBy(p => p.OriginalPrice).ToList()
                        : discountedProducts.OrderByDescending(p => p.OriginalPrice).ToList();
                    break;
                case "DiscountedPrice":
                    discountedProducts = sortOrder == "asc"
                        ? discountedProducts.OrderBy(p => p.DiscountedPrice).ToList()
                        : discountedProducts.OrderByDescending(p => p.DiscountedPrice).ToList();
                    break;
                case "CategoryId":
                    if (sortOrder == "asc")
                    {
                        discountedProducts = discountedProducts
                            .OrderBy(p => p.CategoryId)
                            .ThenBy(p => p.Name)
                            .ToList();
                    }
                    else
                    {
                        discountedProducts = discountedProducts
                            .OrderByDescending(p => p.CategoryId)
                            .ThenByDescending(p => p.Name)
                            .ToList();
                    }
                    break;
                // Thêm các trường khác mà bạn muốn sắp xếp theo
                default:
                    break;
            }

            return discountedProducts;
        }
        public List<ProductWithDiscountDTO> GetProductsWithDiscountByKeyword(DateTime currentDate, string keyword)
        {
            List<ProductWithDiscountDTO> discountedProducts = GetProductsWithDiscountedPrice(currentDate.Date);

            // Gọi phương thức mới để lấy danh sách sản phẩm đã tìm kiếm theo từ khóa
            List<Product> productsByKeyword = _unitOfWork.ProductRepository.SearchProductsWithDiscount(keyword);

            // Kết hợp danh sách sản phẩm đã giảm giá và danh sách sản phẩm tìm thấy theo từ khóa
            // để trả về danh sách kết quả
            var result = discountedProducts
                .Where(p => productsByKeyword.Any(pbk => pbk.Id == p.Id))
                .ToList();

            return result;
        }
        public Dictionary<string, List<ProductWithDiscountDTO>> GetProductsByPriceRange(DateTime currentDate, List<string> priceRanges)
        {
            List<ProductWithDiscountDTO> allProducts = GetProductsWithDiscountedPrice(currentDate.Date);

            Dictionary<string, List<ProductWithDiscountDTO>> productsInPriceRanges = new Dictionary<string, List<ProductWithDiscountDTO>>();

            foreach (var priceRange in priceRanges)
            {
                var range = priceRange.Split('-');
                if (range.Length == 2 && double.TryParse(range[0], out double minPrice) && double.TryParse(range[1], out double maxPrice))
                {
                    string rangeKey = $"RangePrice: {minPrice}-{maxPrice}";
                    productsInPriceRanges[rangeKey] = new List<ProductWithDiscountDTO>();

                    foreach (var product in allProducts)
                    {
                        if (product.DiscountedPrice >= minPrice && product.DiscountedPrice <= maxPrice)
                        {
                            productsInPriceRanges[rangeKey].Add(product);
                        }
                    }
                }
            }

            return productsInPriceRanges;
        }
        public ProductWithDiscountDTO GetProductWithDiscountPriceById(DateTime currentDate,int productId)
        {
            List<ProductWithDiscountDTO> allProducts = GetProductsWithDiscountedPrice(currentDate.Date);
            ProductWithDiscountDTO productWithDiscout = allProducts.FirstOrDefault(p => p.Id == productId);
            return productWithDiscout;
        }

        public IPagedList<ProductWithDiscountDTO> GetPagedProductsWithDiscountedPrice(DateTime selectedDate, int pageNumber, int pageSize)
        {
            List<ProductWithDiscountDTO> discountedProducts = GetProductsWithDiscountedPrice(selectedDate.Date);
            return discountedProducts.ToPagedList(pageNumber, pageSize);
        }
        public List<ProductWithDiscountDTO> SearchProductByName(DateTime currentDate,string keyword)
        {
            List<ProductWithDiscountDTO> allProducts = GetProductsWithDiscountedPrice(currentDate.Date);
            List<ProductWithDiscountDTO> productsMatchingKeyword = allProducts
                .Where(product => product.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return productsMatchingKeyword;
        }
        public List<ProductStatisticDTO> GetBestSellingProducts()
        {
            var orderItems = _unitOfWork.OrderItemRepository.GetAll();
            var productSales = orderItems
                .GroupBy(oi => oi.ProductId)
                .Select(group => new ProductStatisticDTO
                {
                    ProductId = group.Key,
                    TotalQuantitySold = group.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(10) // Lấy 10 sản phẩm bán chạy nhất
                .ToList();

            foreach (var productStat in productSales)
            {
                var product = _unitOfWork.ProductRepository.GetById(productStat.ProductId);
                if (product != null)
                {
                    productStat.ProductName = product.Name;
                }
            }

            return productSales;
        }



    }

}
