using TrainingBE.DTO;

namespace TrainingBE.Service
{
    public interface IProductDiscountService
    {
        bool AddDiscountToProduct(int productId, int discountId,out string errorMessage);
        bool RemoveDiscountFromProduct(int productId, int discountId, out string errorMessage);
        List<DiscountDTO> GetDiscountsByProductId(int productId);
        List<ProductDTO> GetProductsByDiscountId(int discountId);
        List<ProductDiscountDTO> GetAllProductDiscounts();
    }
}
