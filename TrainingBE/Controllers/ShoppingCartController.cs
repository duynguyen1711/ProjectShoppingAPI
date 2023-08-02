using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private  readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;
        private readonly IMemoryCache _cache;


        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService, IMemoryCache cache)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
            _cache = cache;
        }


        [HttpPost("add")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return BadRequest("Product not found");
            }
            if (_cache.TryGetValue<DateTime>("SelectedDate", out DateTime selectedDate))
            {
                var discountedProducts = _productService.GetProductsWithDiscountedPrice(selectedDate.Date);
                var discountedProduct = discountedProducts.FirstOrDefault(p => p.Id == productId);

                var cartItem = new ShoppingCartItemDTO(productId, product.Name, product.Price, quantity, discountedProduct.DiscountedPrice, discountedProduct.Discount);

                _shoppingCartService.AddToCart(cartItem);
                return Ok("Product added to cart successfully.");
            }
            else
            {
                return Ok("Product added to cart successfully.");
            }
            
        }
       

        [HttpGet("checkout")]
        public IActionResult Checkout(double shippingFee)
        {
            var cartItems = _shoppingCartService.GetCartItems();
            double totalPrice = _shoppingCartService.GetTotalPrice();
            double recalculatedShippingFee = _shoppingCartService.CaculateShippingFee(totalPrice, shippingFee);
            double totalPriceWithShipping = _shoppingCartService.GetTotalPriceWithShippingDiscount(totalPrice, recalculatedShippingFee);



            return Ok(new
            {
                cartItems,
                totalPrice,
                recalculatedShippingFee,
                totalPriceWithShipping
            });
        }
        [HttpGet("view")]
        public IActionResult ViewCart()
        {
            var cartItems = _shoppingCartService.GetCartItems();
            return Ok(new
            {
                cartItems,
               
            });
        }

        [HttpGet("total")]
        public IActionResult GetTotalPrice()
        {
            double totalPrice = _shoppingCartService.GetTotalPrice();
            return Ok(totalPrice);
        }

        [HttpDelete("remove/{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            string error;
            _shoppingCartService.RemoveFromCart(productId,out error);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            return Ok("Product removed from cart successfully.");
        }
       
    }   
}

