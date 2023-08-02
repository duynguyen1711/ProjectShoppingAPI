using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public class PaymentRepository_Linq : Repository<Payment>, IPaymentRepository_Linq
    {
        public PaymentRepository_Linq(MyDBContext context) : base(context)
        {
        }
    }
}
