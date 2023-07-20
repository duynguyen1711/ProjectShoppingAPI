using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class CategoryRepository:Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(MyDBContext context) : base(context)
        {
        }
    }
}
