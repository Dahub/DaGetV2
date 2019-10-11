namespace DaGetV2.Dal.EF.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DaGetV2.Dal.Interface.Repositories;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    internal class OperationRepository : RepositoryBase<Operation>, IOperationRepository
    {
        public IEnumerable<Operation> GetAll(Guid? bankAccountId, DateTime? startDate, DateTime? endDate,
            Guid? operationTypeId, bool? isClosed)
            => Context.Operations
                .Include(o => o.BankAccount)
                .Include(o => o.OperationType)
                .Where(o => (!bankAccountId.HasValue || o.BankAccountId.Equals(bankAccountId.Value))
                && (!startDate.HasValue || o.OperationDate >= (startDate.Value))
                && (!endDate.HasValue || o.OperationDate <= (endDate.Value))
                && (!operationTypeId.HasValue || o.OperationTypeId.Equals(operationTypeId.Value))
                && (!isClosed.HasValue || o.IsClosed.Equals(isClosed.Value)));

        public IEnumerable<Operation> GetAllByBankAccountId(Guid bankAccountId)
            => Context.Operations.Where(o => o.BankAccountId.Equals(bankAccountId));
    }
}
