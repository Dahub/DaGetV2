namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using Domain;

    internal class OperationsFilteredSpecification : BaseSpecification<Operation>
    {
        public OperationsFilteredSpecification(
            Guid? bankAccountId,
            DateTime? startDate,
            DateTime? endDate,
            Guid? operationTypeId,
            bool? isClosed) : base(o => (!bankAccountId.HasValue || o.BankAccountId.Equals(bankAccountId.Value))
                                        && (!startDate.HasValue || o.OperationDate >= (startDate.Value))
                                        && (!endDate.HasValue || o.OperationDate <= (endDate.Value))
                                        && (!operationTypeId.HasValue || o.OperationTypeId.Equals(operationTypeId.Value))
                                        && (!isClosed.HasValue || o.IsClosed.Equals(isClosed.Value)))
        {
            AddInclude(o => o.BankAccount);
            AddInclude(o => o.OperationType);
        }
    }
}
