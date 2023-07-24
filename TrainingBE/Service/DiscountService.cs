using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Service
{
    public class DiscountService:IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddDiscount(Discount discount)
        {
            _unitOfWork.DiscountRepository.Add(discount);
            _unitOfWork.Save();
        }

        public void DeleteDiscount(int id, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "Invalid ID. ID must be a non-negative number.";
            }
            var existingDiscount = _unitOfWork.DiscountRepository.GetById(id);

            if (existingDiscount == null)
            {
                error = "Discount not found";
            }
            else
            {
                _unitOfWork.DiscountRepository.Delete(existingDiscount);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<Discount> GetAllDiscounts()
        {
            return _unitOfWork.DiscountRepository.GetAll();
        }

        public Discount GetDiscountById(int id)
        {
            return _unitOfWork.DiscountRepository.GetById(id);
        }

        public bool UpdateDiscount(int id, Discount discount, out string errorMessage)
        {
            errorMessage = "";
            if (id <= 0)
            {
                errorMessage = "Invalid ID. ID must be a non-negative number.";
            }
            var existingDiscount = _unitOfWork.DiscountRepository.GetById(id);

            if (existingDiscount == null)
            {
                errorMessage = "Discount not found";
                return false;
            }

            if (!ValidateUpdateDiscount(existingDiscount, discount, out errorMessage))
            {
                return false;
            }

            existingDiscount.Percentage = discount.Percentage;
            existingDiscount.StartDate = discount.StartDate;
            existingDiscount.EndDate = discount.EndDate;

            _unitOfWork.DiscountRepository.Update(existingDiscount);
            _unitOfWork.Save();

            return true;
        }
        public bool ValidateAddDisscount(Discount discount, out string errorMessage)
        {
            if (discount.Percentage <0  || discount.Percentage >= 100)
            {
                errorMessage = "Invalid percentage";
                return false;
            }
            var startDateWithoutTime = discount.StartDate.Date;
            var endDateWithoutTime = discount.EndDate.Date;
            if (startDateWithoutTime >= endDateWithoutTime)
            {
                errorMessage = "Start date must be earlier than end date.";
                return false;
            }
            
            if (_unitOfWork.DiscountRepository.GetDiscountByDateRange(startDateWithoutTime, endDateWithoutTime) != null)
            {
                errorMessage = "A discount with the same date range already exists.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public bool ValidateUpdateDiscount(Discount existingDiscount, Discount updatedDiscount, out string errorMessage)
        {
            if (updatedDiscount.Percentage < 0 || updatedDiscount.Percentage >= 100)
            {
                errorMessage = "Invalid percentage";
                return false;
            }
            if (updatedDiscount.StartDate.Date >= updatedDiscount.EndDate.Date)
            {
                errorMessage = "Start date must be earlier than end date.";
                return false;
            }

            if (existingDiscount.EndDate.Date != updatedDiscount.EndDate.Date && existingDiscount.StartDate.Date != updatedDiscount.StartDate.Date)
            {
                var otherDiscounts = _unitOfWork.DiscountRepository.GetAll().Where(d => d.Id != existingDiscount.Id);
                if (otherDiscounts.Any(d => d.EndDate.Date == updatedDiscount.EndDate.Date && d.StartDate.Date == updatedDiscount.StartDate.Date))
                {
                    errorMessage = "A discount with the same date range already exists.";
                    return false;
                }
            }
            errorMessage = string.Empty;
            return true;
        }
    }
}
