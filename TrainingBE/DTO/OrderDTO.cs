using TrainingBE.Common;
using TrainingBE.Model;

namespace TrainingBE.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int PaymentID { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus orderStatus { get; set; }
        public double Total { get; set; }
        

        
    }
}
