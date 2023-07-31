namespace TrainingBE.DTO
{
    public class PriceRangeDTO
    {
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public List<ProductWithDiscountDTO> Products { get; set; }
    }
}
