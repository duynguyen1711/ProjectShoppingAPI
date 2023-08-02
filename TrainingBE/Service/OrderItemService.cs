using TrainingBE.DTO;
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

        public void AddOrderItem(OrderItem orderItem)
        {
            _unitOfWork.OrderItemRepository.Add(orderItem);
        }
        public List<OrderItemDTO> GetOrderItemsByOrderId(int orderId)
        {
            var orderItems = _unitOfWork.OrderItemRepository
                .GetAll()
                .Where(item => item.OrderId == orderId)
                .ToList();

            var orderItemDTOs = orderItems.Select(item => new OrderItemDTO
            {
                Id = item.Id,
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PriceAtTime = item.PriceAtTime,
                Quantity = item.Quantity
            }).ToList();

            return orderItemDTOs;
        }
    }
}
