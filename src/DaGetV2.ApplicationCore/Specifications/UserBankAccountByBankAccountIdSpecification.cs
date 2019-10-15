namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using Domain;

    internal class UserBankAccountByBankAccountIdSpecification : BaseSpecification<UserBankAccount>
    {
        public UserBankAccountByBankAccountIdSpecification(Guid bankAccountId) 
            : base(uba => uba.BankAccountId.Equals(bankAccountId))
        {
        }
    }
}
