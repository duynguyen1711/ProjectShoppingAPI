using TrainingBE.DTO;

namespace TrainingBE.Service
{
    public interface IShoppingCartService
    {
        void AddToCart(ShoppingCartItemDTO cartItem);
        void RemoveFromCart(int productId,out string error);
        List<ShoppingCartItemDTO> GetCartItems();
        double GetTotalPrice();
        double GetTotalPriceWithShippingDiscount(double totalPrice, double shippingFee);
        double CaculateShippingFee(double totalPrice, double shippingFee);
        void ClearShoppingCart();
    }
}
