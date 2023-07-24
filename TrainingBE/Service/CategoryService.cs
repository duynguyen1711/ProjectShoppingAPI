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
            _unitOfWork.CategoryRepository.Add(category);
            _unitOfWork.Save();
        }

        public void DeleteCategory(int id, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "Invalid ID. ID must be a non-negative number.";
            }
            var existingCategory = _unitOfWork.CategoryRepository.GetById(id);

            if (existingCategory == null)
            {
                error = "Category not found";
            }
            else
            {
                _unitOfWork.CategoryRepository.Delete(existingCategory);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<Category> GetCategory()
        {
            return _unitOfWork.CategoryRepository.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            return _unitOfWork.CategoryRepository.GetById(id);
        }

        public bool UpdateCategory(int id, Category updatedCategory, out string errorMessage)
        {
            errorMessage = "";
            if (id <= 0)
            {
                errorMessage="Invalid ID. ID must be a non-negative number.";
            }
            var existingCategory = _unitOfWork.CategoryRepository.GetById(id);

            if (existingCategory == null)
            {
                errorMessage = "Category not found";
                return false;
            }

            if (!ValidateUpdateCategory(existingCategory, updatedCategory, out errorMessage))
            {
                return false;
            }

            existingCategory.Name = updatedCategory.Name;

            _unitOfWork.CategoryRepository.Update(existingCategory);
            _unitOfWork.Save();

            return true;
        }

        public bool ValidateAddCategory(Category category, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                errorMessage = "Category name must not be empty or contain only spaces.";
                return false;
            }
            if (_unitOfWork.CategoryRepository.GetCategoryByCategoryName(category.Name) != null)
            {
                errorMessage = "Category name already exists.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public bool ValidateUpdateCategory(Category existingCategory, Category updatedCategory, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(updatedCategory.Name))
            {
                errorMessage = "Category name must not be empty or contain only spaces.";
                return false;
            }

            if (existingCategory.Name != updatedCategory.Name)
            {
                var otherCategories = _unitOfWork.CategoryRepository.GetAll().Where(p => p.Id != existingCategory.Id);
                if (otherCategories.Any(p => p.Name == updatedCategory.Name))
                {
                    errorMessage = "Category name is already taken by another category.";
                    return false;
                }
            }
            errorMessage = string.Empty;
            return true;
        }
    }
}
