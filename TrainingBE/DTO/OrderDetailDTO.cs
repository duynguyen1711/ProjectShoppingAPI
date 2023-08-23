using TrainingBE.Common;
using TrainingBE.Model;

namespace TrainingBE.DTO
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Name { get; set; }
        public double Total { get; set; }
        public OrderStatus orderStatus { get; set; }

    }
}
