using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public class CategoryRepository_Linq : Repository<Category>, ICategoryRepository_Linq
    {
        public CategoryRepository_Linq(MyDBContext context) : base(context)
        {
        }
        public Category GetCategoryByCategoryName(string categoryName)
        {
            return _dbSet.FirstOrDefault(c => c.Name == categoryName);
        }
    }
}
