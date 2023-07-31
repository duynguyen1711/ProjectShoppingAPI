using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Repository_Linq
{
    public class DiscountRepository_Linq : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository_Linq(MyDBContext context) : base(context)
        {
        }
        public Discount GetDiscountByDateRange(DateTime startDate, DateTime endDate)
        {

            return _dbSet.FirstOrDefault(d => d.StartDate <= endDate && d.EndDate >= startDate);
        }
    }
}
