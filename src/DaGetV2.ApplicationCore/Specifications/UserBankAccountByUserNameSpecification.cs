namespace DaGetV2.ApplicationCore.Specifications
{
    using Domain;

    internal class UserBankAccountByUserNameSpecification : BaseSpecification<UserBankAccount>
    {
        public UserBankAccountByUserNameSpecification(string userName) 
            : base(uba => uba.User.UserName.Equals(userName))
        {
            AddInclude(uba => uba.User);
            AddInclude(uba => uba.BankAccount);
            AddInclude(uba => uba.BankAccount.BankAccountType);
        }
    }
}
