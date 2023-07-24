using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrainingBE.Model
{
    public class Payment
    {
        public enum PaymentType
        {
            COD = 0,
            VISA = 1,
            MOMO = 2,
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public PaymentType Type { get; set; }
    }
}
