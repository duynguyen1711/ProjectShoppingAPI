using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddPayment(Payment payment)
        {
            _unitOfWork.PaymentRepository.Add(payment);
            _unitOfWork.Save();
        }

        public void DeletePayment(int id, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "Invalid ID. ID must be a non-negative number.";
            }
            var existingPayment = _unitOfWork.PaymentRepository.GetById(id);

            if (existingPayment == null)
            {
                error = "Payment not found";
            }
            else
            {
                _unitOfWork.PaymentRepository.Delete(existingPayment);
                _unitOfWork.Save();
            }
        }

        public Payment GetPaymentById(int id)
        {
            return _unitOfWork.PaymentRepository.GetById(id);
        }

        public IEnumerable<Payment> GetPayment()
        {
            return _unitOfWork.PaymentRepository.GetAll();
        }
    }
}
