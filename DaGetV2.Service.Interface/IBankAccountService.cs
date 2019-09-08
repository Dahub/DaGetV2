using System;
using System.Collections.Generic;
using DaGetV2.Dal.Interface;
using DaGetV2.Service.DTO;

namespace DaGetV2.Service.Interface
{
    public interface IBankAccountService
    {
        IEnumerable<BankAccountDto> GetAll(IContext context, string userName);

        Guid Add(IContext context, string userName, CreateBankAccountDto toCreateBankAccount);
    }
}
