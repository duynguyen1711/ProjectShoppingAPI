using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyDBContext context) : base(context)
        {
        }
        public User GetUserByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.userName == username);

        }
        

    }
}
