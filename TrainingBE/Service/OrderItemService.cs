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
    }
}
