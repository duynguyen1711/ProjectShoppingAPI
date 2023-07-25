using TrainingBE.DTO;

namespace TrainingBE.Service
{
    public interface IProductDiscountService
    {
        bool AddDiscountToProduct(int productId, int discountId,out string errorMessage);
        bool RemoveDiscountFromProduct(int productId, int discountId, out string errorMessage);
        bool UpdateDiscountForProduct(int productId, int oldDiscountId, int newDiscountId, out string errorMessage);
        List<DiscountDTO> GetDiscountsByProductId(int productId, out string error);
        List<ProductDTO> GetProductsByDiscountId(int discountId,out string error);
        List<ProductDiscountDTO> GetAllProductDiscounts();
    }
}
