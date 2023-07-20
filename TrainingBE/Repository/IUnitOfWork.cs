using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        void Save();
    }
}
