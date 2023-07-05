namespace TrainingBE.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int PaymentID { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus orderStatus { get; set; }
        public double Total { get; set; }
        public enum OrderStatus
        {
            PENDING = 0,
            PROCESSED = 1,
            SHIPPED = 2,
            CANCELED = 3,
        }
    }
}
