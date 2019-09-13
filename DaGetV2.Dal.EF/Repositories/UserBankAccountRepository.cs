using System;
using System.Linq;
using DaGetV2.Dal.Interface.Repositories;
using DaGetV2.Domain;

namespace DaGetV2.Dal.EF.Repositories
{
    internal class UserBankAccountRepository : RepositoryBase<UserBankAccount>, IUserBankAccountRepository
    {
        public UserBankAccount GetByIdUserAndIdBankAccount(Guid idUser, Guid idBankAccount)
            => Context.UserBankAccounts.SingleOrDefault(
                uba => uba.UserId.Equals(idUser) 
                && uba.BankAccountId.Equals(idBankAccount));
    }
}
