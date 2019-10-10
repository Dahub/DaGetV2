namespace DaGetV2.Dal.Interface.Repositories
{
    using System;
    using System.Collections.Generic;
    using Domain;
    
    public interface IUserBankAccountRepository : IRepository<UserBankAccount>
    {
        UserBankAccount GetByIdUserAndIdBankAccount(Guid idUser, Guid idBankAccount);
        
        IEnumerable<UserBankAccount> GetAllByIdBankAccount(Guid idBankAccount);
    }
}
