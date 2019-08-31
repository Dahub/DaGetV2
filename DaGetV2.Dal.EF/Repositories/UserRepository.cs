using System.Linq;
using DaGetV2.Dal.Interface.Repositories;
using DaGetV2.Domain;

namespace DaGetV2.Dal.EF.Repositories
{
    internal class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public bool UserExists(string username)
        {
            return Context.Users.Where(u => u.UserName.Equals(username, System.StringComparison.OrdinalIgnoreCase)).Any();
        }
    }
}
