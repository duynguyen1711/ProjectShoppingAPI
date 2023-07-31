using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public interface ICategoryRepository 
    {
        Category GetById(int id);
        IEnumerable<Category> GetAll();
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
        Category GetCategoryByCategoryName(string categoryName);
    }
    
}
