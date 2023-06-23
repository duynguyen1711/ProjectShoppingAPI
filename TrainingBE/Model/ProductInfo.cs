namespace TrainingBE.Model
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public double OriginalPrice { set; get; }
        public double PriceWithDiscount { set; get; }
        public Category Category { set; get; }
        public List<Discount> Discount { set; get; }

    }
}
