namespace TrainingBE.DTO
{
    public class OrderStatisticDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
        public double TotalInMonth { get; set; }
        public int TotalOrders { get; set; }
    }
}
