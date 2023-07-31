using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDBContext _context;
        public CategoryRepository(MyDBContext context)
        {
            _context = context;
        }
        public Category GetById(int id)
        {
            var result = _context.Categories.FromSqlInterpolated($"EXEC GetCategoryById {id}");
            return result.AsEnumerable().FirstOrDefault();
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.FromSqlRaw("EXEC GetAllCategory").ToList();
        }

        public void Add(Category category)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC InsertCategories {category.Name}");
        }

        public void Update(Category category)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC UpdateCategory {category.Id}, {category.Name}");
        }

        public void Delete(Category category)
        {
            _context.Database.ExecuteSqlInterpolated($"EXEC DeleteCategory {category.Id}");
        }

        public Category GetCategoryByCategoryName(string categoryName)
        {
            var result = _context.Categories.FromSqlInterpolated($"EXEC GetCategoryByName {categoryName}");
            return result.AsEnumerable().FirstOrDefault();
            
        }

    }
}
