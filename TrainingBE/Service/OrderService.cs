using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Repository_Linq;

namespace TrainingBE.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderItemService _orderItemService;
        

        public OrderService(IShoppingCartService shoppingCartService, IUnitOfWork unitOfWork, IOrderItemService orderItemService)
        {
            _shoppingCartService = shoppingCartService;
            _unitOfWork = unitOfWork;
            _orderItemService = orderItemService;
        }
        public Order CreateOrder(int userId, int paymentId, double shippingFee)
        {  
                var shoppingCartItems = _shoppingCartService.GetCartItems();
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

                foreach (var cartItem in shoppingCartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = cartItem.ProductName,
                        PriceAtTime = cartItem.DiscountedPrice,
                        Quantity = cartItem.Quantity
                    };

                    _orderItemService.AddOrderItem(orderItem);
                    _unitOfWork.Save();
                }
                
                _shoppingCartService.ClearShoppingCart();

                return order;     
        }
    }
}
