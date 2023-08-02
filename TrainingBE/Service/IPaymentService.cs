using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IPaymentService
    {
        void AddPayment(Payment payment);
        
        void DeletePayment(int id, out string error);
        Payment GetPaymentById(int id);
        IEnumerable<Payment> GetPayment();
    }
}
