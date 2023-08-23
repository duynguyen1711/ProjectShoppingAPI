using TrainingBE.DTO;

namespace TrainingBE.Service
{
    public interface IUserService
    {
        List<CustomerStatisticDTO> GetCustomerRevenues();
    }
}
