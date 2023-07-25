using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Service
{
    public class ProductDiscountService : IProductDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductDiscountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool AddDiscountToProduct(int productId, int discountId,out string errorMessage)
        {
            errorMessage = string.Empty;
            var existingProduct = _unitOfWork.ProductRepository.GetById(productId);
            var existingDiscount = _unitOfWork.DiscountRepository.GetById(discountId);

            if (existingProduct == null)
            {
                errorMessage = "Product not found";
                return false;
            }

            if (existingDiscount == null)
            {
                errorMessage = "Discount not found";
                return false;
            }

            if (_unitOfWork.ProductDiscountRepository.Exists(productId, discountId))
            {
                errorMessage = "Product already has this discount";
                return false;
            }

            var productDiscount = new Product_Discount
            {
                ProductId = productId,
                DiscountId = discountId
            };

            _unitOfWork.ProductDiscountRepository.Add(productDiscount);
            _unitOfWork.Save();
            return true;
        }

        public bool RemoveDiscountFromProduct (int productId, int discountId, out string errorMessage)
        {
            errorMessage = string.Empty;

            var product = _unitOfWork.ProductRepository.GetById(productId);
            if (product == null)
            {
                errorMessage = "Product not found";
                return false;
            }

            var discount = _unitOfWork.DiscountRepository.GetById(discountId);
            if (discount == null)
            {
                errorMessage = "Discount not found";
                return false;
            }
            bool isProductDiscountApplied = _unitOfWork.ProductDiscountRepository.Exists(productId, discountId);
            // Kiểm tra xem sản phẩm đã áp dụng mã giảm này hay chưa

            if (!isProductDiscountApplied)
            {
                errorMessage = "Product does not have this discount applied";
                return false;
            }

            // Xóa mã giảm khỏi sản phẩm
            var productDiscount = _unitOfWork.ProductDiscountRepository.GetAll()
            .FirstOrDefault(pd => pd.ProductId == productId && pd.DiscountId == discountId);

            if (productDiscount != null)
            {
                // Xóa Product_Discount
                _unitOfWork.ProductDiscountRepository.Delete(productDiscount);
                _unitOfWork.Save();
                return true;
            }

            return false;

        }

        public List<DiscountDTO> GetDiscountsByProductId(int productId,out string error)
        {
            error = string.Empty;
            var existingProduct = _unitOfWork.ProductRepository.GetById(productId);

            if (existingProduct == null)
            {
                error = "Not found product"; 
            }

            var discounts = _unitOfWork.ProductDiscountRepository.GetDiscountsByProductId(productId);
            var discountIDs = discounts.Select(pd => pd.DiscountId).ToList();
            var result = _unitOfWork.DiscountRepository.GetAll()
                .Where(p => discountIDs.Contains(p.Id))
                                    .Select(p => new DiscountDTO
                                    {
                                        Percentage = p.Percentage,
                                        StartDate=p.StartDate,
                                        EndDate=p.EndDate,
                                    })
                                    .ToList();
            return result;
        }

        public List<ProductDTO> GetProductsByDiscountId(int discountId,out string error)
        {
            error = string.Empty;
            var existingDiscount = _unitOfWork.DiscountRepository.GetById(discountId);

            if (existingDiscount == null)
            {
                error ="Discount not found";
            }

            var productDiscounts = _unitOfWork.ProductDiscountRepository.GetProductsByDiscountId(discountId);

            // Lấy danh sách sản phẩm liên kết với discount
            var productIds = productDiscounts.Select(pd => pd.ProductId).ToList();

            // Lấy danh sách sản phẩm từ repository Product, dựa trên danh sách productIds đã lấy
            var products = _unitOfWork.ProductRepository.GetAll()
                                    .Where(p => productIds.Contains(p.Id))
                                    .Select(p => new ProductDTO
                                    {
                                        Name = p.Name,
                                        Price = p.Price,
                                        CategoryID = p.CategoryID,
                                        // Các thông tin khác của ProductDTO
                                    })
                                    .ToList();

            return products;
        }

        public List<ProductDiscountDTO> GetAllProductDiscounts()
        {
            var productDiscounts = _unitOfWork.ProductDiscountRepository.GetAllProductDiscount();
            var result = productDiscounts.Select(pd => new ProductDiscountDTO
            {
                Id = pd.Id,
                ProductId = pd.ProductId,
                Product = new ProductDTO
                {
                    Name = pd.Product.Name,
                    Price = pd.Product.Price,
                    CategoryID =pd.Product.CategoryID
                },
                DiscountId = pd.DiscountId,

                Discount = new DiscountDTO
                {
                    Percentage = pd.Discount.Percentage,
                    EndDate = pd.Discount.EndDate,
                    StartDate = pd.Discount.StartDate,
                }

            }).ToList();

            return result;

        }

        public bool UpdateDiscountForProduct(int productId, int oldDiscountId, int newDiscountId, out string errorMessage)
        {
            errorMessage = string.Empty;

            var product = _unitOfWork.ProductRepository.GetAll() // Eager loading dữ liệu cho Product_Discounts
                         .FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                errorMessage = "Product not found";
                return false;
            }

            var oldDiscount = _unitOfWork.DiscountRepository.GetById(oldDiscountId);
            var newDiscount = _unitOfWork.DiscountRepository.GetById(newDiscountId);

            if (oldDiscount == null)
            {
                errorMessage = "Old discount not found";
                return false;
            }

            if (newDiscount == null)
            {
                errorMessage = "New discount not found";
                return false;
            }
            // Kiểm tra nếu mã giảm mới đã được áp dụng cho sản phẩm
            bool IsNewDiscountApplied = _unitOfWork.ProductDiscountRepository.Exists(productId, newDiscountId);
            if (IsNewDiscountApplied)
            {
                errorMessage = "Product already has the new discount applied";
                return false;
            }
            var productDiscount = _unitOfWork.ProductDiscountRepository.GetAllProductDiscount().FirstOrDefault(pd => pd.DiscountId == oldDiscountId);
            if (productDiscount == null)
            {
                errorMessage = "Product does not have the old discount applied";
                return false;
            }

            // Gán mã giảm mới vào đối tượng product_discount
            productDiscount.DiscountId = newDiscountId;

            _unitOfWork.Save();

            return true;
        }
    }
}
