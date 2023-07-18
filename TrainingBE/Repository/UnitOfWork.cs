using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDBContext _context;
        private IRepository<Product> _productRepository;

        public UnitOfWork(MyDBContext context)
        {
            _context = context;
        }

        public IRepository<Product> ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new Repository<Product>(_context);
                }
                return _productRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
