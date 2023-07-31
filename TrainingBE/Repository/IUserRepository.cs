using TrainingBE.Model;
using TrainingBE.Repository_Linq;

namespace TrainingBE.Repository
{

    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUsername(string username);
    }
}
