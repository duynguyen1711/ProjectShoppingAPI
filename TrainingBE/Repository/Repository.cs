using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrainingBE.Data;

namespace TrainingBE.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private MyDBContext _context =null;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository()
        {
            _context = new MyDBContext(new DbContextOptions<MyDBContext>());
            _dbSet = _context.Set<TEntity>();
        }
        public Repository(MyDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
