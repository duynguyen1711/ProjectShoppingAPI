using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
           
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
        public IActionResult GetProductsByCategories([FromQuery] List<int> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0)
            {
                return BadRequest("Category IDs are required.");
            }

            //var productsByCategories = _productService.GetProductsByCategoryIds(categoryIds);

            //if (productsByCategories.Count == 0)
            //{
            //    return NotFound("No products found for the provided category IDs.");
            //}
            var currentDate = DateTime.Now.Date;
            var productsWithDiscounts = _productService.GetProductsWithDiscountedPrice(currentDate);
            var productsByCategories = productsWithDiscounts.GroupBy(p => p.CategoryId).Select(group => new
            {
                CategoryId = group.Key,
                CategoryName = group.First().Category.Name,
                Products = group.ToList()
            });
            return Ok(productsByCategories);
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
        public IActionResult GetProductsDiscountedPrice(DateTime currentDate)
        {   
            List<ProductWithDiscountDTO> discountedProducts = _productService.GetProductsWithDiscountedPrice(currentDate);
            return Ok(discountedProducts);
        }
        [HttpGet("sortedProducts")]
        public IActionResult GetSortedProducts([FromQuery] DateTime currentDate, [FromQuery] string sortColumn, [FromQuery] string sortOrder)
        {
            var sortedProducts = _productService.GetSortedProductsWithDiscount(currentDate.Date, sortColumn, sortOrder);
            return Ok(sortedProducts);
        }
        [HttpGet("productsByKeyword")]
        public IActionResult GetProductsByKeyword([FromQuery] DateTime currentDate, [FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest("Keyword is required.");
            }

            var productsWithDiscount = _productService.GetProductsWithDiscountByKeyword(currentDate.Date, keyword);
            return Ok(productsWithDiscount);
        }
       
    }
}
