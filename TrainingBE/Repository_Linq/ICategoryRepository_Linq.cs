using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public interface ICategoryRepository_Linq : IRepository<Category>
    {
        Category GetCategoryByCategoryName(string categoryName);
    }

}
