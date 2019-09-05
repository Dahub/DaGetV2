using System.Collections.Generic;
using DaGetV2.Dal.Interface;
using DaGetV2.Domain;
using DaGetV2.Service.DTO;

namespace DaGetV2.Service.Interface
{
    public interface IBankAccountService
    {
        int Add(IContext context, BankAccount toCreate);

        IEnumerable<BankAccountDto> GetAll(IContext context, string userName);
    }
}
