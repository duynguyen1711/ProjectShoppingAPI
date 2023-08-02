using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Repository_Linq;

namespace TrainingBE.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartService _shoppingCartService;
        
        public OrderService(IShoppingCartService shoppingCartService, IUnitOfWork unitOfWork)
        {
            _shoppingCartService = shoppingCartService;
            _unitOfWork = unitOfWork;
        }
        public Order CreateOrder(int userId, int paymentId, double shippingFee)
        {
            var shoppingCartItems = _shoppingCartService.GetCartItems();
            if (shoppingCartItems == null || shoppingCartItems.Count == 0)
            {
                throw new Exception("The shopping cart is empty.");
            }
            double totalPrice = _shoppingCartService.GetTotalPrice();
            double recalculatedShippingFee = _shoppingCartService.CaculateShippingFee(totalPrice, shippingFee);
            double totalPriceWithShipping = _shoppingCartService.GetTotalPriceWithShippingDiscount(totalPrice, recalculatedShippingFee);
            var order = new Order
            {
                UserID = userId,
                PaymentID = paymentId,
                OrderDate = DateTime.Now,
                orderStatus = Order.OrderStatus.PENDING,
                Total = totalPriceWithShipping
            };

            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.Save();
            _shoppingCartService.ClearShoppingCart();

            return order;
        }
    }
}
