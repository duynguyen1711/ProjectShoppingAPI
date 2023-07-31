using TrainingBE.DTO;
using Microsoft.Extensions.Caching.Memory;
namespace TrainingBE.Service
{
    public class ShoppingCartService: IShoppingCartService
    {
        private readonly IMemoryCache _cache;
        private const string ShoppingCartCacheKey = "ShoppingCart";
        private List<ShoppingCartItemDTO> _shoppingCart = new List<ShoppingCartItemDTO>();
        public ShoppingCartService(IMemoryCache cache)
        {
            _cache = cache;
            InitializeShoppingCart();
        }
        public void AddToCart(ShoppingCartItemDTO cartItem)
        {
            var shoppingCart = _cache.Get<List<ShoppingCartItemDTO>>(ShoppingCartCacheKey);

            var existingItem = shoppingCart.FirstOrDefault(item => item.ProductId == cartItem.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                shoppingCart.Add(cartItem);
            }

            _cache.Set(ShoppingCartCacheKey, shoppingCart);
        }


        public void RemoveFromCart(int productId,out string error)
        {
            error = "";
            var productInCart = _shoppingCart.Any(item => item.ProductId == productId);
            if (!productInCart )
            {
                error = "Product not exist in Cart";
            }
            var shoppingCart = _cache.Get<List<ShoppingCartItemDTO>>(ShoppingCartCacheKey);
            if (shoppingCart != null)
            {
                var cartItem = shoppingCart.FirstOrDefault(item => item.ProductId == productId);
                if (cartItem != null)
                {
                    if (cartItem.Quantity > 1)
                    {
                        cartItem.Quantity -= 1; 
                    }
                    else
                    {
                        shoppingCart.Remove(cartItem); 
                    }

                    _cache.Set(ShoppingCartCacheKey, shoppingCart);
                }
            }
        }

        public List<ShoppingCartItemDTO> GetCartItems()
        {
            return _cache.Get<List<ShoppingCartItemDTO>>(ShoppingCartCacheKey);
        }

        public double GetTotalPrice()
        {
            var shoppingCart = _cache.Get<List<ShoppingCartItemDTO>>(ShoppingCartCacheKey);
            return shoppingCart.Sum(item => item.DiscountedPrice * item.Quantity);
        }
        public double GetTotalPriceWithShippingDiscount(double totalPrice, double shippingFee)
        {
            // Cộng thêm phí ship
            if (totalPrice > 500000)
            {
                totalPrice = totalPrice * 0.9;
            }
            totalPrice += shippingFee;

            return totalPrice;
        }
        public double CaculateShippingFee(double totalPrice,double shippingFee)
        {
            if (totalPrice > 500000)
            {
                shippingFee = 0;
            }
            else if (totalPrice > 100000)
            {
                shippingFee = shippingFee - 15000; // Giảm 15k
            }
            else if (totalPrice > 50000)
            {
                shippingFee = shippingFee - 10000; // Giảm 10k
            }
            return shippingFee;
        }

        private void InitializeShoppingCart()
        {
            if (!_cache.TryGetValue(ShoppingCartCacheKey, out List<ShoppingCartItemDTO> shoppingCart))
            {
                shoppingCart = new List<ShoppingCartItemDTO>();
                _cache.Set(ShoppingCartCacheKey, shoppingCart);
            }
        }
    }
}
