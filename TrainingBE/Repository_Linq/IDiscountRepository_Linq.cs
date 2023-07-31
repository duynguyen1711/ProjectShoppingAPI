using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public interface IDiscountRepository_Linq : IRepository<Discount>
    {
        Discount GetDiscountByDateRange(DateTime startDate, DateTime endDate);
    }
}
