using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface ICategoryService
    {
        void AddCategory(Category category);
        void UpdateCategory(int id, Category category);
        void DeleteCategory(int id);
        Category GetCategoryById(int id);
        IEnumerable<Category> GetCategory();
    }
}
