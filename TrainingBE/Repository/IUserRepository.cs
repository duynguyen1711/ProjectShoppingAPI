using TrainingBE.Model;

namespace TrainingBE.Repository
{

    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUsername(string username);
    }
}
