using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDBContext _context;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private IUserRepository _userRepository;
        private IDiscountRepository _discountRepository;
        private IProductDiscountRepository _productDiscountRepository;

        public UnitOfWork(MyDBContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }
        public IDiscountRepository DiscountRepository
        {
            get
            {
                if (_discountRepository == null)
                {
                    _discountRepository = new DiscountRepository(_context);
                }
                return _discountRepository;
            }
        }
        public IProductDiscountRepository ProductDiscountRepository
        {
            get
            {
                if (_productDiscountRepository == null)
                {
                    _productDiscountRepository = new ProductDiscountRepository(_context);
                }
                return _productDiscountRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
