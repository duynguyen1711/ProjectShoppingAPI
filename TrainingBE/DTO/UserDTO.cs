using static TrainingBE.Model.User;

namespace TrainingBE.DTO
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NumberPhone { set; get; }
        public UserStatus Status { set; get; }
        public Role Role { set; get; }


    }
}
