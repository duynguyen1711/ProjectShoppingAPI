namespace TrainingBE.Service
{
    public interface IOrderItemService
    {
        void AddOrderItem(int orderId, int productId, string productName, double price, int quantity);
    }
}
