namespace DaGetV2.Dal.Interface.Repositories
{
    using DaGetV2.Domain;

    public interface IUserRepository : IRepository<User>
    {
        bool UserExists(string username);
        
        User GetByUserName(string username);
    }
}
