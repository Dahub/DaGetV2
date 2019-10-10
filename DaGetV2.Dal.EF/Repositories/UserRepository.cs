﻿namespace DaGetV2.Dal.EF.Repositories
{
    using System.Linq;
    using DaGetV2.Dal.Interface.Repositories;
    using DaGetV2.Domain;

    internal class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public User GetByUserName(string username)
            => Context.Users.Where(u => u.UserName.Equals(username, System.StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

        public bool UserExists(string username)
            => Context.Users.Where(u => u.UserName.Equals(username, System.StringComparison.OrdinalIgnoreCase)).Any();
    }
}
