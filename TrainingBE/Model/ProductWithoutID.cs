namespace TrainingBE.Model
{
    public class ProductWithoutID
    {
        public String Name { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public List <Discount> Discount { get; set; }
    }
}
