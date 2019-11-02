namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using Domain;

    internal class OperationByBankAccountIdSpecification : BaseSpecification<Operation>
    {
        public OperationByBankAccountIdSpecification(Guid bankAccountId) 
            : base(o => o.BankAccountId.Equals(bankAccountId))
        {
        }
    }
}
