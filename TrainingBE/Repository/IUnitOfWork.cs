using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        void Save();
    }
}
