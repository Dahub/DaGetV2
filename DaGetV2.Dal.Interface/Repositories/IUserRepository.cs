using DaGetV2.Domain;

namespace DaGetV2.Dal.Interface.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool UserExists(string username);
        
        User GetByUserName(string username);
    }
}
