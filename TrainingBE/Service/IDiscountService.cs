using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IDiscountService
    {
        void AddDiscount(Discount discount);
        bool UpdateDiscount(int id, Discount discount, out string errorMessage);
        void DeleteDiscount (int id, out string error);
        Discount GetDiscountById(int id);
        IEnumerable<Discount> GetAllDiscounts();
        bool ValidateAddDisscount(Discount discount, out string errorMessage);
    }
}
