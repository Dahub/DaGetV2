using System.Collections.Generic;
using DaGetV2.Domain;

namespace DaGetV2.Dal.Interface.Repositories
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        IEnumerable<BankAccount> GetAllByUser(string userName);
    }
}
