namespace TrainingBE.DTO
{
    public class AddReviewResquest
    {
        public int ProductId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
