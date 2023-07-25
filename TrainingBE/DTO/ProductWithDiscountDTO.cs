using TrainingBE.Model;

namespace TrainingBE.DTO
{
    public class ProductWithDiscountDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double OriginalPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public int CategoryId { get; set; }
        public DiscountDTO? Discount { get; set; } 
    }
}
