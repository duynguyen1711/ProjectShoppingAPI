using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IDiscountRepository:IRepository<Discount>
    {
        Discount GetDiscountByDateRange(DateTime startDate, DateTime endDate);
    }
}
