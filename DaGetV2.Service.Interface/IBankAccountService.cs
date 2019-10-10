namespace DaGetV2.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using DaGetV2.Dal.Interface;
    using DTO;

    public interface IBankAccountService
    {
        IEnumerable<BankAccountDto> GetAll(IContext context, string userName);

        Guid Create(IContext context, string userName, CreateBankAccountDto toCreateBankAccount);

        void Update(IContext context, string userName, UpdateBankAccountDto toEditBankAccount);

        BankAccountDto GetById(IContext context, string userName, Guid bankAccountId);

        void DeleteBankAccountById(IContext context, string userName, Guid bankAccountId);
    }
}
