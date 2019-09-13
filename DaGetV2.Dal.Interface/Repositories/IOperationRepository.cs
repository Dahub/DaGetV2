using System;
using System.Collections.Generic;
using DaGetV2.Domain;

namespace DaGetV2.Dal.Interface.Repositories
{
    public interface IOperationRepository : IRepository<Operation>
    {
        IEnumerable<Operation> GetAllByBankAccountId(Guid bankAccountId);
    }
}
