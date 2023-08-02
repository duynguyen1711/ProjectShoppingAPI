using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IOrderService
    {
        Order CreateOrder(int userId, int paymentId, double shippingFee);
    }
}
