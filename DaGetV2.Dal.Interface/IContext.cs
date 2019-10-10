namespace DaGetV2.Dal.Interface
{
    using System;
    using Repositories;

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
