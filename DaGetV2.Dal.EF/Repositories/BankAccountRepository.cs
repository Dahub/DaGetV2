using System.Collections.Generic;
using System.Linq;
using DaGetV2.Dal.Interface.Repositories;
using DaGetV2.Domain;
using Microsoft.EntityFrameworkCore;

namespace DaGetV2.Dal.EF.Repositories
{
    internal class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        public IEnumerable<BankAccount> GetAllByUser(string userName)
        {
            return Context.UserBankAccounts.
                Where(uba => uba.User.UserName.Equals(userName)).
                Select(uba => uba.BankAccount).
                Include(ba => ba.BankAccountType).
                Include(ba => ba.UsersBanksAccounts).
                ThenInclude(uba => uba.User);
        }
    }
}
