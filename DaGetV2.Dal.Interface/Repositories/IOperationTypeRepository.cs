using System;
using System.Collections.Generic;
using DaGetV2.Domain;

namespace DaGetV2.Dal.Interface.Repositories
{
    public interface IOperationTypeRepository : IRepository<OperationType>
    {
        IEnumerable<OperationType> GetAllByBankAccountId(Guid bankAccountId);

        bool OperationTypeHasOperations(Guid operationTypeId);
    }
}
