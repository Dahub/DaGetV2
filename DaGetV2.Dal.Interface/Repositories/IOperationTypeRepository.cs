namespace DaGetV2.Dal.Interface.Repositories
{
    using System;
    using System.Collections.Generic;
    using DaGetV2.Domain;

    public interface IOperationTypeRepository : IRepository<OperationType>
    {
        IEnumerable<OperationType> GetAllByBankAccountId(Guid bankAccountId);

        bool OperationTypeHasOperations(Guid operationTypeId);
    }
}
