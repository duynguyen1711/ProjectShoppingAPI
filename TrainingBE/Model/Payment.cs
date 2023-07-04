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
        public int Id { get; set; }
        public PaymentType Type { get; set; }
       
    }
}
