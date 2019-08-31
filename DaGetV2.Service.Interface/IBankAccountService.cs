using DaGetV2.Dal.EF;
using DaGetV2.Dal.Interface;
using DaGetV2.Domain;
using DaGetV2.Service.DTO;
using System.Collections.Generic;

namespace DaGetV2.Service.Interface
{
    public interface IBankAccountService
    {
        int Add(IContext context, BankAccount toCreate);

        IEnumerable<BankAccountDto> GetAll(IContext context, string userName);
    }
}
