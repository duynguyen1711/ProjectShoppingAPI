using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IDiscountRepository DiscountRepository { get; }
        IProductDiscountRepository ProductDiscountRepository { get; }
        void Save();
    }
}
