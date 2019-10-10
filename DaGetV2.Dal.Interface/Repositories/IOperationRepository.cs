namespace DaGetV2.Dal.Interface.Repositories
{
    using System;
    using System.Collections.Generic;
    using DaGetV2.Domain;

    public interface IOperationRepository : IRepository<Operation>
    {
        IEnumerable<Operation> GetAllByBankAccountId(Guid bankAccountId);
    }
}
