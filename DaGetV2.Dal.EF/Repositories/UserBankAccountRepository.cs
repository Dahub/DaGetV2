namespace DaGetV2.Dal.EF.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DaGetV2.Dal.Interface.Repositories;
    using DaGetV2.Domain;

    internal class UserBankAccountRepository : RepositoryBase<UserBankAccount>, IUserBankAccountRepository
    {
        public UserBankAccount GetByIdUserAndIdBankAccount(Guid idUser, Guid idBankAccount)
            => Context.UserBankAccounts.SingleOrDefault(
                uba => uba.UserId.Equals(idUser) 
                && uba.BankAccountId.Equals(idBankAccount));

        public IEnumerable<UserBankAccount> GetAllByIdBankAccount(Guid idBankAccount)
            => Context.UserBankAccounts.Where(uba => uba.BankAccountId.Equals(idBankAccount));
    }
}
