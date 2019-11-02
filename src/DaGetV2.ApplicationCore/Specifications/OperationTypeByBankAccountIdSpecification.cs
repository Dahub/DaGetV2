namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using Domain;

    internal class OperationTypeByBankAccountIdSpecification : BaseSpecification<OperationType>
    {
        public OperationTypeByBankAccountIdSpecification(Guid bankAccountId) 
            : base(ot => ot.BankAccountId.Equals(bankAccountId))
        {

        }
    }
}
