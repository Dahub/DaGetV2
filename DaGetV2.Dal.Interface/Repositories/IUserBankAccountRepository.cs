namespace DaGetV2.Dal.Interface.Repositories
{
    using System.Collections.Generic;
    using DaGetV2.Domain;

    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        IEnumerable<BankAccount> GetAllByUser(string userName);
    }
}
