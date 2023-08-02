using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository_Linq;

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
        private IOrderRepository_Linq _orderRepository;
        private IOrderItemRepository_Linq _orderItemRepository;
        private IPaymentRepository_Linq _paymentRepository;

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
        public IOrderRepository_Linq OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository_Linq(_context);
                }
                return _orderRepository;
            }
        }
        public IOrderItemRepository_Linq OrderItemRepository
        {
            get
            {
                if (_orderItemRepository == null)
                {
                    _orderItemRepository = new OrderItemRepository_Linq(_context);
                }
                return _orderItemRepository;
            }
        }

        public IPaymentRepository_Linq PaymentRepository
        {
            get
            {
                if (_paymentRepository == null)
                {
                    _paymentRepository = new PaymentRepository_Linq(_context);
                }
                return _paymentRepository;
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
