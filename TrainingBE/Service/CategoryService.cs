using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetCategory()
        {
            return _unitOfWork.CategoryRepository.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateCategory(int id, Category category)
        {
            throw new NotImplementedException();
        }
    }
}
