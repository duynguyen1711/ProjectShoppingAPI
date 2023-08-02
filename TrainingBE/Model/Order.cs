using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TrainingBE.Common;

namespace TrainingBE.Model
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserID { get; set; }
        public int PaymentID { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus orderStatus { get; set; }
        public double Total { get; set; }
        public User? User { get; set; }
        public Payment? Payment { get; set; }
        
        
    }
}
