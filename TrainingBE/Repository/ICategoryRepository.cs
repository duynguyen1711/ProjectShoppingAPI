using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetCategoryByCategoryName(string categoryName);
    }
    
}
