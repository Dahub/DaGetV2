using System;
using DaGetV2.Domain;

namespace DaGetV2.Dal.Interface.Repositories
{
    public interface IUserBankAccountRepository : IRepository<UserBankAccount>
    {
        UserBankAccount GetByIdUserAndIdBankAccount(Guid idUser, Guid idBankAccount);
    }
}
