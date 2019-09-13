using DaGetV2.Dal.Interface.Repositories;
using System;

namespace DaGetV2.Dal.Interface
{
    public interface IContext : IDisposable
    {
        void Commit();

        void CommitAsync();

        IUserRepository GetUserRepository();

        IBankAccountRepository GetBankAccountRepository();

        IBankAccountTypeRepository GetBankAccountTypeRepository();

        IOperationTypeRepository GetOperationTypeRepository();
        
        IUserBankAccountRepository GetUserBankAccountRepository();

        IOperationRepository GetOperationRepository();
    }
}
