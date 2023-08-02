using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Service
{
    public class OrderItemService:IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public OrderItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddOrderItem(int orderId, int productId, string productName, double price, int quantity)
        {
            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                ProductName = productName,
                PriceAtTime = price,
                Quantity = quantity
            };

            _unitOfWork.OrderItemRepository.Add(orderItem);
            _unitOfWork.Save();
        }
    }
}
