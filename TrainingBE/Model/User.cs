using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace TrainingBE.Model
{
    public class User
    {
        public enum UserStatus
        {
            active =0,
            inactive = 1,
            delete=2
        }
        public enum Role
        {
            User = 0,
            Admin = 1,
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }
        public string name { set; get; }
        public string userName { set; get; }
        public string password { set; get; }
        public UserStatus status { set; get; }
        public string email { set; get; }
        public string numberPhone { set; get; }
        public Role role { set; get; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
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
