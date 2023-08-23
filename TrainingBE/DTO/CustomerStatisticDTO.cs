using static TrainingBE.Model.User;

namespace TrainingBE.DTO
{
    public class CustomerStatisticDTO
    {
        public int id { set; get; }
        public string name { set; get; }
        public UserStatus status { set; get; }
        public string email { set; get; }
        public string numberPhone { set; get; }
        public double TotalRevenue { set; get; }
    }
}
