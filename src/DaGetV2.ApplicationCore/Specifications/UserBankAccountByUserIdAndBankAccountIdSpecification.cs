namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using Domain;

    internal class UserBankAccountByUserIdAndBankAccountIdSpecification : BaseSpecification<UserBankAccount>
    {
        public UserBankAccountByUserIdAndBankAccountIdSpecification(Guid userId, Guid bankAccountId) : 
            base(uba => uba.UserId.Equals(userId) && uba.BankAccountId.Equals(bankAccountId))
        {
        }
    }
}
