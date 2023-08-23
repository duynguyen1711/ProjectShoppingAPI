using TrainingBE.Model;
using TrainingBE.Repository_Linq;

namespace TrainingBE.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IDiscountRepository DiscountRepository { get; }
        IProductDiscountRepository ProductDiscountRepository { get; }
        IOrderRepository_Linq OrderRepository { get; }
        IOrderItemRepository_Linq OrderItemRepository { get; }
        IPaymentRepository_Linq PaymentRepository { get; }
        IReviewRepository_Linq ReviewRepository { get; }

        void Save();
    }
}
