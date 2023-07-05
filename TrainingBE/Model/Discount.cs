namespace TrainingBE.Model
{
    public class Discount
    {
        public int Id { get; set; }
        public double Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Discount()
        {

        }
    }
}
