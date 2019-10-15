namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using System.Linq.Expressions;
    using Domain;

    internal class OperationByBankAccountId : BaseSpecification<Operation>
    {
        public OperationByBankAccountId(Guid bankAccountId) : base(o => o.BankAccountId.Equals(bankAccountId))
        {
        }
    }
}
