namespace DaGetV2.ApplicationCore.Specifications
{
    using Domain;

    public class UserByUserNameSpecification : BaseSpecification<User>
    {
        public UserByUserNameSpecification(string userName) : base(u => u.UserName.Equals(userName))
        {
        }
    }
}
