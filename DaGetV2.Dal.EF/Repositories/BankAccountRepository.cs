namespace DaGetV2.Dal.EF.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DaGetV2.Dal.Interface.Repositories;
    using DaGetV2.Domain;
    using Microsoft.EntityFrameworkCore;

    internal class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        public IEnumerable<BankAccount> GetAllByUser(string userName)
            => Context.UserBankAccounts.
                Where(uba => uba.User.UserName.Equals(userName)).
                Select(uba => uba.BankAccount).
                Include(ba => ba.BankAccountType).
                Include(ba => ba.UsersBanksAccounts).
                ThenInclude(uba => uba.User);

        public override BankAccount GetById(Guid id)
            => Context.BankAccounts.
                Where(ba => ba.Id.Equals(id)).
                Include(ba => ba.BankAccountType).
                Include(ba => ba.UsersBanksAccounts).
                ThenInclude(uba => uba.User).
                SingleOrDefault();
    }
}
