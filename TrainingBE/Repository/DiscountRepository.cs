using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class DiscountRepository : Repository<Discount>,  IDiscountRepository
    {
        public DiscountRepository(MyDBContext context) : base(context)
        {
        }
        public Discount GetDiscountByDateRange(DateTime startDate, DateTime endDate)
        {
            
            return _dbSet.FirstOrDefault(d => d.StartDate <= endDate && d.EndDate >= startDate);
        }
    }
}
