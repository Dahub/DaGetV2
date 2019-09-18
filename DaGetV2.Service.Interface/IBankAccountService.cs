using System;
using System.Collections.Generic;
using DaGetV2.Dal.Interface;
using DaGetV2.Service.DTO;

namespace DaGetV2.Service.Interface
{
    public interface IBankAccountService
    {
        IEnumerable<BankAccountDto> GetAll(IContext context, string userName);

        Guid Create(IContext context, string userName, CreateBankAccountDto toCreateBankAccount);

        void Update(IContext context, string userName, UpdateBankAccountDto toEditBankAccount);

        BankAccountDto GetById(IContext context, string userName, Guid id);
    }
}
