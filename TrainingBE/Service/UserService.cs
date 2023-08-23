using TrainingBE.Common;
using TrainingBE.DTO;
using TrainingBE.Repository;

namespace TrainingBE.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<CustomerStatisticDTO> GetCustomerRevenues()
        {
            var customerRevenues = _unitOfWork.OrderRepository
        .GetAll()
        .Where(order => order.orderStatus == OrderStatus.DONE)
        .GroupBy(order => order.UserID)
        .Select(group => new CustomerStatisticDTO
        {
            id = group.Key,
            TotalRevenue = group.Sum(order => order.Total)
        })
        .ToList();

            foreach (var customerRevenue in customerRevenues)
            {
                var user = _unitOfWork.UserRepository.GetById(customerRevenue.id);
                if (user != null)
                {
                    customerRevenue.name = user.name;
                    customerRevenue.status = user.status;
                    customerRevenue.email = user.email;
                    customerRevenue.numberPhone = user.numberPhone;
                }
            }
            return customerRevenues;
        }
    }
}
