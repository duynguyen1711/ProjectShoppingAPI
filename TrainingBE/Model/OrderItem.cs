namespace TrainingBE.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double PriceAtTime { get; set; }
        public int Quantity { get; set; }
    }
}
