using TrainingBE.Common;
using TrainingBE.DTO;
using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IOrderService
    {
        Order CreateOrder(int userId, int paymentId, double shippingFee);
        List<OrderDTO> GetAllOrders();
        List<OrderDTO> GetOrdersByUserId(int userId);
        void UpdateOrderStatus(int orderId, OrderStatus newStatus);
    }
}
