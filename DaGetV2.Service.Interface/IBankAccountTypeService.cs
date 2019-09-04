using System.Collections.Generic;
using DaGetV2.Dal.Interface;
using DaGetV2.Service.DTO;

namespace DaGetV2.Service.Interface
{
    public interface IBankAccountTypeService
    {
        IEnumerable<BankAccountTypeDto> GetAll(IContext context);
    }
}
