using TrainingBE.Common;

namespace TrainingBE.DTO
{
    public class OrderUpdateDTO
    {
        public int Id { get; set; }
        public OrderStatus orderStatus { get; set; }
    }
}
