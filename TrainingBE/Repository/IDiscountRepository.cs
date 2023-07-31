using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IDiscountRepository
    {
        Discount GetById(int id);
        IEnumerable<Discount> GetAll();
        void Add(Discount discount);
        void Update(Discount discount);
        void Delete(Discount discount);
        Discount GetDiscountByDateRange(DateTime startDate, DateTime endDate);
    }
}
