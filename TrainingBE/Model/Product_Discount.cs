using System.ComponentModel.DataAnnotations;

namespace TrainingBE.Model
{
    public class Product_Discount
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
