using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TrainingBE.Data;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMemoryCache _cache;
        public ProductController(IProductService productService, IMemoryCache cache)
        {
            _productService = productService;
            _cache = cache;

        }
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    var model =_productService.GetAllProducts();
        //    return Ok(model);
        //}
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetByID(int id ) {
            if (id <= 0)
            {
                return BadRequest("Invalid ID. ID must be a non-negative number.");
            }
            var model = _productService.GetProductById(id);
            if (model == null)
            {
                return NotFound("Not Found");
            }
            return Ok(model);
        }
        [HttpGet("productsByCategories")]
        public IActionResult GetProductsByCategories()
        {
            
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                var productsWithDiscounts = _productService.GetProductsWithDiscountedPrice(selectedDate.Date);
                var productsByCategories = productsWithDiscounts.GroupBy(p => p.CategoryId).Select(group => new
                {
                    CategoryId = group.Key,
                    CategoryName = group.First().Category.Name,
                    Products = group.ToList()
                });
                return Ok(productsByCategories);
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }  
        }
        [HttpGet("productsByCategoriesID")]
        public IActionResult GetProductsByCategories([FromQuery] List<int> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0)
            {
                return BadRequest("No category IDs provided.");
            }
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                var productsWithDiscounts = _productService.GetProductsWithDiscountedPrice(selectedDate.Date);
                var productsByCategories = productsWithDiscounts
                .Where(p => categoryIds.Contains(p.CategoryId)) 
                .GroupBy(p => p.CategoryId)
                .Select(group => new
                {
                    CategoryId = group.Key,
                    CategoryName = group.First().Category.Name,
                    Products = group.ToList()
                });
                return Ok(productsByCategories);
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }
        }

        [HttpPost]
        public ActionResult AddProduct( Product product) {
            string errorMessage;
            if (!_productService.ValidateAddProduct(product, out errorMessage))
            {
                return BadRequest(errorMessage);
            }
            try
            {
                _productService.AddProduct(product);
                return Ok("Product added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            string errorMessage;
            if (!_productService.UpdateProduct(id, product, out errorMessage))
            {
                return BadRequest(errorMessage);
            }

            return Ok("Product updated successfully.");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID. ID must be a non-negative number.");
            }
            try
            {
                _productService.DeleteProduct(id);
                return Ok("Product deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("discounted")]
        public IActionResult GetProductsDiscountedPrice()
        {
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                List<ProductWithDiscountDTO> discountedProducts = _productService.GetProductsWithDiscountedPrice(selectedDate.Date);
                return Ok(discountedProducts);
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }
        }
        [HttpGet("sortedProducts")]
        public IActionResult GetSortedProducts( [FromQuery] string sortColumn, [FromQuery] string sortOrder)
        {
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                var sortedProducts = _productService.GetSortedProductsWithDiscount(selectedDate.Date, sortColumn, sortOrder);
                return Ok(sortedProducts);
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }

        }
        [HttpGet("productsByPriceRange")]
        public IActionResult GetProductsByPriceRange([FromQuery] List<string> priceRanges)
        {
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                var productsInPriceRanges = _productService.GetProductsByPriceRange(selectedDate.Date, priceRanges);

                return Ok(productsInPriceRanges);
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }
        }
        [HttpGet("productDetail/{productId}")]
        public IActionResult GetProductById(int productId)
        {
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                var product = _productService.GetProductWithDiscountPriceById(selectedDate.Date,productId);
                
                if (product == null)
                {
                    return NotFound("Product not found");
                }
                else
                {
                    return Ok(product);
                }
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }   
        }
        [HttpGet("countAllProducts")]
        public IActionResult CountAllProducts()
        {
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                List<ProductWithDiscountDTO> discountedProducts = _productService.GetProductsWithDiscountedPrice(selectedDate.Date);
                int count =discountedProducts.Count();
                return Ok(new {
                    ListProduct =discountedProducts,
                    TotalProduct=count
                });
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }
        }
        [HttpGet("countProductsByCategory")]
        public IActionResult CountAllProductsByCategory()
        {
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                List<ProductWithDiscountDTO> discountedProducts = _productService.GetProductsWithDiscountedPrice(selectedDate.Date);
                int count = discountedProducts.Count();

                var productsByCategory = discountedProducts
                .GroupBy(p => p.Category.Name)
                .ToDictionary(
                    group => group.Key,
                    group => new
                    {
                        TotalProductsInCategory = group.Count(),
                        Products = group.ToList()
                    }
                );

                return Ok(new
                {
                    TotalProducts = count,
                    ProductsByCategory = productsByCategory
                });
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }
        }
        [HttpGet("countProductsByPriceRange")]
        public IActionResult GetCountProductsByPriceRange([FromQuery] List<string> priceRanges)
        {
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                var productsInPriceRanges = _productService.GetProductsByPriceRange(selectedDate.Date, priceRanges);
                int totalCount = productsInPriceRanges.SelectMany(kv => kv.Value).Count();

                var result = new
                {
                    TotalProducts = totalCount,
                    ProductsInPriceRanges = productsInPriceRanges.Select(kv => new
                    {
                        Range = kv.Key,
                        TotalInRange = kv.Value.Count,
                        Products = kv.Value
                    })
                };
                return Ok(result);
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }
        }
        [HttpGet("countProductsValidDiscount")]
        public IActionResult CountProductsWithValidDiscount()
        {
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                List<ProductWithDiscountDTO> productWithDiscount = _productService.GetProductsWithDiscountedPrice(selectedDate.Date);
                var discountedProducts = productWithDiscount.Where(dp => dp.Discount != null).ToList();
                int countDiscountedProducts = discountedProducts.Count();
                var nonDiscountedProducts= productWithDiscount.Except(discountedProducts).ToList();
                int countNonDiscountedProducts = nonDiscountedProducts.Count();
                return Ok(new
                {
                    TotalProductDiscounted = countDiscountedProducts,
                    ProductDiscounted = discountedProducts,
                    TotalNonDiscountedProducts = countNonDiscountedProducts,
                    NonDiscountedProducts = nonDiscountedProducts,
                    
                });
            }
            else
            {
                return BadRequest("Selected date has not been set.");
            }
        }
        
    }
}
