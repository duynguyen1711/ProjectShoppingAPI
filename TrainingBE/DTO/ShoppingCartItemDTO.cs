namespace TrainingBE.DTO
{
    public class ShoppingCartItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double DiscountedPrice { get; set; } 
        public DiscountDTO AppliedDiscount { get; set; }
        public ShoppingCartItemDTO(int productId, string productName, double price, int quantity, double discountedPrice, DiscountDTO appliedDiscount)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
            DiscountedPrice = discountedPrice;
            AppliedDiscount = appliedDiscount;
        }
        public ShoppingCartItemDTO(int productId, int quantity)
        {
            ProductId = productId;
           
            Quantity = quantity;
            
        }
        public ShoppingCartItemDTO()
        {
        }
    }
}
