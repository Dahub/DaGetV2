using System;
using System.Collections.Generic;
using System.Linq;
using DaGetV2.Dal.Interface.Repositories;
using DaGetV2.Domain;

namespace DaGetV2.Dal.EF.Repositories
{
    internal class OperationTypeRepository : RepositoryBase<OperationType>, IOperationTypeRepository
    {
        public IEnumerable<OperationType> GetAllByBankAccountId(Guid bankAccountId)
            => Context.OperationTypes.Where(ot => ot.BankAccountId.Equals(bankAccountId));

        public bool OperationTypeHasOperations(Guid operationTypeId)
            => Context.Operations.Any(o => o.OperationTypeId.Equals(operationTypeId));
    }
}
