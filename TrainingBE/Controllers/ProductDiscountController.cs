using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TrainingBE.DTO;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDiscountController : ControllerBase
    {
        private readonly IProductDiscountService _productDiscountService;

        public ProductDiscountController(IProductDiscountService productDiscountService)
        {
            _productDiscountService = productDiscountService;
        }

        // Thêm Discount cho Product
        [HttpPost("{productId}/add-discount/{discountId}")]
        public IActionResult AddDiscountToProduct(int productId, int discountId)
        {
            string errorMessage;
            if (_productDiscountService.AddDiscountToProduct(productId, discountId, out errorMessage))
            {
                return Ok("Discount added to Product successfully.");
                
            }
            return BadRequest(errorMessage);
        }

        // Xóa Discount khỏi Product
        [HttpDelete("products/{productId}/discounts/{discountId}")]
        public IActionResult RemoveDiscountFromProduct(int productId, int discountId)
        {
            string error;
            _productDiscountService.RemoveDiscountFromProduct(productId, discountId, out error);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return Ok("Discount removed from product successfully.");
        }

        // Lấy tất cả các Discount của một Product
        [HttpGet("{productId}/discounts")]
        public IActionResult GetDiscountsByProductId(int productId)
        {
            string error;
            var discounts = _productDiscountService.GetDiscountsByProductId(productId,out error);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            return Ok(discounts);
        }

        // Lấy tất cả các Product có cùng một Discount
        [HttpGet("discount/{discountId}/products")]
        public IActionResult GetProductsByDiscountId(int discountId)
        {
            string error;
            var products = _productDiscountService.GetProductsByDiscountId(discountId,out error);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            return Ok(products);
        }
        [HttpGet]
        public ActionResult<IEnumerable<ProductDiscountDTO>> GetAllProductDiscount()
        {
            var productDiscounts = _productDiscountService.GetAllProductDiscounts();
            return Ok(productDiscounts);
        }
        [HttpPut("products/{productId}/discounts")]
        public IActionResult UpdateDiscountForProduct(int productId, [FromQuery] int oldDiscountId, [FromQuery] int newDiscountId)
        {
            string error;
            _productDiscountService.UpdateDiscountForProduct(productId, oldDiscountId, newDiscountId, out error);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return Ok("Discount for product updated successfully.");
        }
    }
}
