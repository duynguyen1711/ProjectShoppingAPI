using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepository { get; }
        void Save();
    }
}
