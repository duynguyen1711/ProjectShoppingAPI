using TrainingBE.DTO;
using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IOrderItemService
    {
        void AddOrderItem(OrderItem orderItem);
        List<OrderItemDTO> GetOrderItemsByOrderId(int orderId);
    }
}
