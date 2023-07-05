using System.Security.Cryptography;
using System.Text;

namespace TrainingBE.Model
{
    public class User
    {
        public enum UserStatus
        {
            active,
            inactive,
            delete
        }
        public string userName { set; get; }
        public string password { set; get; }
        public UserStatus status { set; get; }
        public string email { set; get; }
        public string numberPhone { set; get; }
        public User()
        {
            status = UserStatus.active;
        }
        public User(string userName, string password, string email, string numberPhone)
        {
            this.userName = userName;
            this.password = password;
            this.email = email;
            this.numberPhone = numberPhone;
        }
        public User(string userName, string password, UserStatus status, string email, string numberPhone)
        {
            this.userName = userName;
            this.password = password;
            this.status = status;
            this.email = email;
            this.numberPhone = numberPhone;
        }
    }
}
