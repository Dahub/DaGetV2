﻿namespace DaGetV2.Dal.EF.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DaGetV2.Dal.Interface.Repositories;
    using DaGetV2.Domain;

    internal class OperationRepository : RepositoryBase<Operation>, IOperationRepository
    {
        public IEnumerable<Operation> GetAllByBankAccountId(Guid bankAccountId)
            => Context.Operations.Where(o => o.BankAccountId.Equals(bankAccountId));
    }
}
