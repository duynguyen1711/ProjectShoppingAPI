using System.Text;
using System.Security.Cryptography;
namespace TrainingBE
{
    public class UserHelpers
    {
        public string HashPassword(string password)
        {
            using SHA256 sha256Hash = SHA256.Create();
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i]);
                }
                return builder.ToString();
            }
        }
        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            string hashPassword = HashPassword(enteredPassword);
            return hashPassword.Equals(storedHashedPassword);
        }
    }
}
