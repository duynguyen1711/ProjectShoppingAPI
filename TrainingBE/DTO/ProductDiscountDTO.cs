namespace TrainingBE.DTO
{
    public class ProductDiscountDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int DiscountId { get; set; }
        public DiscountDTO? Discount { get; set; }
    }
}
