namespace TrainingBE.DTO
{
    public class ReviewDTO
    {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public string UserName { get; set; }
            public float Rating { get; set; }
            public string Comment { get; set; }
            public DateTime DateCreated { get; set; }
            public string TimeAgo { get; set; }
    }
}
