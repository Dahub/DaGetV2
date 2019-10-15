namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using Domain;

    internal class BankAccountByIdWithBankAccountTypeSpecification : BaseSpecification<BankAccount>
    {
        public BankAccountByIdWithBankAccountTypeSpecification(Guid bankAccountId) : base(ba => ba.Id.Equals(bankAccountId))
        {
            AddInclude(ba => ba.BankAccountType);
        }
    }
}
