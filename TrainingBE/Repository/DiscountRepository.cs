using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class DiscountRepository :   IDiscountRepository
    {
        private readonly MyDBContext _context;
        public DiscountRepository(MyDBContext context) 
        {
            _context = context;
        }

        public void Add(Discount discount)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC InsertDiscount {discount.Percentage},{discount.StartDate},{discount.EndDate}");
        }

        public void Delete(Discount discount)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC DeleteDiscount {discount.Id}");
        }

        public IEnumerable<Discount> GetAll()
        {
            return _context.Discounts.FromSqlRaw("EXEC GetAllDiscount").ToList();
        }

        public Discount GetById(int id)
        {
            var result = _context.Discounts.FromSqlInterpolated($"EXEC GetDiscountByID {id}");
            return result.AsEnumerable().FirstOrDefault();
        }

        public Discount GetDiscountByDateRange(DateTime startDate, DateTime endDate)
        {
            var result = _context.Discounts.FromSqlRaw("EXEC GetDiscountByDateRange {0}, {1}", startDate, endDate);
            return result.AsEnumerable().FirstOrDefault();
        }

        public void Update(Discount discount)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC UpdateDiscount {discount.Id},{discount.Percentage},{discount.StartDate},{discount.EndDate}");
        }
    }
}
