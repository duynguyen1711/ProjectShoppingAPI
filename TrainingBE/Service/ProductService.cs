using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddProduct(Product product)
        {
            if (!ValidateAddProduct(product, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Save();
        }

        public void DeleteProduct(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid ID. ID must be a non-negative number.");
            }
            var existingProduct = _unitOfWork.ProductRepository.GetById(id);

            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found");
            }

            _unitOfWork.ProductRepository.Delete(existingProduct);
            _unitOfWork.Save();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _unitOfWork.ProductRepository.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _unitOfWork.ProductRepository.GetById(id);
        }

        public void UpdateProduct(int id, Product product)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid ID. ID must be a non-negative number.");
            }
            var existingProduct = _unitOfWork.ProductRepository.GetById(id);

            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found");
            }

            if (!ValidateUpdateProduct(existingProduct, product, out string errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryID = product.CategoryID;

            _unitOfWork.ProductRepository.Update(existingProduct);
            _unitOfWork.Save();
        }

        private bool ValidateAddProduct(Product product, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                errorMessage = "Product name must not be empty or contain only spaces.";
                return false;
            }
            if (_unitOfWork.ProductRepository.GetProductByProductName(product.Name) != null)
            {
                errorMessage = "Product name already exists.";
                return false;
            }
            if (product.Name.Length < 3)
            {
                errorMessage = "Product name must be at least 3 characters.";
                return false;
            }

            if (product.Price < 0)
            {
                errorMessage = "Product price must be a non-negative number.";
                return false;
            }

            if (product.CategoryID < 0)
            {
                errorMessage = "CategoryID must be a non-negative number.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        private bool ValidateUpdateProduct(Product existingProduct,Product updatedProduct, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(updatedProduct.Name))
            {
                errorMessage = "Product name must not be empty or contain only spaces.";
                return false;
            }

            if (existingProduct.Name != updatedProduct.Name)
            {
                var otherProducts = _unitOfWork.ProductRepository.GetAll().Where(p => p.Id != existingProduct.Id);
                if (otherProducts.Any(p => p.Name == updatedProduct.Name))
                {
                    errorMessage = "Product name is already taken by another product.";
                    return false;
                }
            }
            if (updatedProduct.Name.Length < 3)
            {
                errorMessage = "Product name must be at least 3 characters.";
                return false;
            }

            if (updatedProduct.Price < 0)
            {
                errorMessage = "Product price must be a non-negative number.";
                return false;
            }

            if (updatedProduct.CategoryID < 0)
            {
                errorMessage = "CategoryID must be a non-negative number.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

    }

}
