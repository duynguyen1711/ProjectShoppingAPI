using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface ICategoryService
    {
        void AddCategory(Category category);
        bool UpdateCategory(int id, Category category, out string errorMessage);
        void DeleteCategory(int id,out string error);
        Category GetCategoryById(int id);
        IEnumerable<Category> GetCategory();
        bool ValidateAddCategory(Category category, out string errorMessage);
    }
}
